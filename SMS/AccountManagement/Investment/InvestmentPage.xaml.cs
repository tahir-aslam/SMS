using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SMS.Models;
using SMS.AccountManagement.Investment;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.AccountManagement.AccountDataEntry;
using SMS.PrintHeaderTemplates;

namespace SMS.AccountManagement.Investment
{
    /// <summary>
    /// Interaction logic for InvestmentPage.xaml
    /// </summary>
    public partial class InvestmentPage : Page
    {
        List<sms_investor> investor_list;
        List<sms_investments> investment_list;
        InvestmentForm inf;
        sms_investments row_obj;
        string mode;
        string insertion;
        string updation;
        public static int total_amount = 0;
        public static DateTime dt = DateTime.Now;
        List<sms_months> months_list;

        public InvestmentPage()
        {
            InitializeComponent();
            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;

            get_all_investors();
            investor_cmb.SelectedIndex = 0;
            investor_list.Insert(0, new sms_investor() { investor_name = "---Investor---", id = "-1" });
            investor_cmb.ItemsSource = investor_list;

            load_grid();            
        }

        //---------------           Get All Months    ----------------------------------
        public void get_all_months()
        {
            months_list = new List<sms_months>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_months";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_months sm = new sms_months()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                month_id = Convert.ToString(reader["month"].ToString()),
                            };
                            months_list.Add(sm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
                
        public void load_grid()
        {            
            month_cmb.SelectedIndex = 0;
            investor_cmb.SelectedIndex = 0;
            date_picker.SelectedDate = DateTime.Now;
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            inf = new InvestmentForm(mode, this, row_obj);
            inf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inf.ShowDialog();
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (sms_investments)investments_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(row_obj.id);
                    if (delete() == 1)
                    {
                        if (check == false)
                        {
                            insert_deleted(row_obj.id);
                        }
                        load_grid();
                    }
                    else
                    {
                        load_grid();
                        MessageBox.Show("OOPs! Theres is some problem");

                    }
                }
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
        }

        //  ----------------------insert deleted------------------------------
        public void insert_deleted(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into sms_investments_deleted (id) values (@id)";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // ---------     Check Insertion           --------------------------------------
        public bool check_insertion(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select insertion from sms_investments where id = " + id;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            insertion = Convert.ToString(reader["insertion"].ToString());
                            if (insertion == "true")
                            {
                                return true;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        //-------------     Delete          ---------------------------
        public int delete()
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_investments where id=" + row_obj.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return i;
        }

        //-------------     Editing          ---------------------------
        private void section_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            row_obj = (sms_investments)investments_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                inf = new InvestmentForm(mode, this, row_obj);
                inf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                inf.ShowDialog();
            }
        }

        //-----------       Get All Investors    ----------------------
        public void get_all_investors()
        {
            investor_list = new List<sms_investor>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_investors";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_investor si = new sms_investor()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                investor_name = Convert.ToString(reader["investor_name"].ToString()),
                                investor_cell = Convert.ToString(reader["investor_cell"].ToString()),
                                investor_address = Convert.ToString(reader["investor_address"].ToString()),
                                investor_description = Convert.ToString(reader["investor_description"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            investor_list.Add(si);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //-----------       Get All Accounts Data entry    ----------------------
        public void get_all_investments(DateTime dt)
        {
            total_amount = 0;
            investment_list = new List<sms_investments>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_investments where investment_date=@dt ORDER BY investor_id";
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_investments si = new sms_investments()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                investor_id = Convert.ToString(reader["investor_id"].ToString()),
                                investor_name = Convert.ToString(reader["investor_name"].ToString()),
                                description = Convert.ToString(reader["description"].ToString()),
                                cheque_no = Convert.ToString(reader["cheque_no"].ToString()),
                                investment_date = Convert.ToDateTime(reader["investment_date"]),
                                amount = Convert.ToString(reader["amount"].ToString()),                                
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            investment_list.Add(si);
                            total_amount = total_amount + Convert.ToInt32(si.amount);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //-----------       Get All Accounts Data entry    ----------------------
        public void get_all_investments(string id)
        {
            total_amount = 0;
            investment_list = new List<sms_investments>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_investments ORDER BY investor_id";
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_investments si = new sms_investments()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                investor_id = Convert.ToString(reader["investor_id"].ToString()),
                                investor_name = Convert.ToString(reader["investor_name"].ToString()),
                                description = Convert.ToString(reader["description"].ToString()),
                                cheque_no = Convert.ToString(reader["cheque_no"].ToString()),
                                investment_date = Convert.ToDateTime(reader["investment_date"]),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            if (si.investment_date.Month == Convert.ToInt32(id))
                            {
                                investment_list.Add(si);
                                total_amount = total_amount + Convert.ToInt32(si.amount);
                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 70, 220, 280, 120 };
            var ht = new ExpenseHeader();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }

        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Investor Name", typeof(string));
            AddColumn(dataTable, "Date", typeof(string));
            AddColumn(dataTable, "cheque#", typeof(string));
            AddColumn(dataTable, "Amount(Rs)", typeof(string));


            foreach (sms_investments ae in investment_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = ae.investor_name.ToString();
                dataRow[1] = ae.investment_date.ToString();
                dataRow[2] = ae.cheque_no.ToString();
                dataRow[3] = ae.amount.ToString();

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker.SelectedDate != null)
            {
                dt = date_picker.SelectedDate.Value;
                if (dt != null)
                {

                    get_all_investments(dt);
                    investments_grid.ItemsSource = investment_list;
                    total_amount_tb.Text = total_amount.ToString();
                    this.investments_grid.Items.Refresh();
                    investor_cmb.SelectedIndex = 0;
                }
            }
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            sms_months sm = (sms_months)month_cmb.SelectedItem;
            if (sm != null)
            {
                if (sm.month_id != "0")
                {
                    date_picker.SelectedDate = null;
                    get_all_investments(sm.month_id);
                    investments_grid.ItemsSource = investment_list;
                    total_amount_tb.Text = total_amount.ToString();
                    this.investments_grid.Items.Refresh();
                    investor_cmb.SelectedIndex = 0;
                }
                else
                {
                }
            }
        }

        private void investor_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int total = 0;
            sms_investor si = (sms_investor)investor_cmb.SelectedItem;
            if (si != null)
            {
                if (si.id != "0")
                {
                    investments_grid.ItemsSource = investment_list.Where(x => x.investor_id == si.id);
                    investments_grid.Items.Refresh();

                    foreach (sms_investments sii in investment_list.Where(x => x.investor_id == si.id))
                    {
                        total = total + Convert.ToInt32(sii.amount);
                    }
                    total_amount_tb.Text = total.ToString();
                }
            }
        }        
    }
}
