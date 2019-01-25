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
using SMS.AccountManagement.Account;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.AccountManagement.AccountDataEntry;
using SMS.PrintHeaderTemplates;

namespace SMS.AccountManagement.AccountDataEntry
{
    /// <summary>
    /// Interaction logic for AccountDataSearch.xaml
    /// </summary>
    public partial class AccountDataSearch : Page
    {
        List<account> account_list;
        List<account_entry> account_entry_list;
        AccountDataForm adf;
        account_entry row_obj;
        string mode;
        string insertion;
        string updation;
        public static int total_amount = 0;
        public static DateTime dt = DateTime.Now;
        List<sms_months> months_list;

        public AccountDataSearch()
        {
            InitializeComponent();

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;

            get_all_accounts();
            account_cmb.SelectedIndex = 0;
            account_list.Insert(0, new account() { account_name = "--Select Account--", id = "-1" });
            account_cmb.ItemsSource = account_list;

            row_obj = new account_entry();            
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
            date_picker_to.SelectedDate = DateTime.Now;
            date_picker.SelectedDate = DateTime.Now;
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            adf = new AccountDataForm(mode, this, row_obj);
            adf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            adf.ShowDialog();
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (account_entry)account_entry_grid.SelectedItem;
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
                        cmd.CommandText = "insert into sms_account_entry_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_account_entry where id = " + id;
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
                    cmd.CommandText = "Delete from sms_account_entry where id=" + row_obj.id;
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
            row_obj = (account_entry)account_entry_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                adf = new AccountDataForm(mode, this, row_obj);
                adf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                adf.ShowDialog();
            }
        }

        //-----------       Get All Accounts Data    ----------------------
        public void get_all_accounts()
        {
            account_list = new List<account>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_accounts";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            account acc = new account()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                account_name = Convert.ToString(reader["account_name"].ToString()),
                                account_desc = Convert.ToString(reader["account_desc"].ToString()),
                                account_holder_name = Convert.ToString(reader["account_holder_name"].ToString()),
                                account_holder_cell = Convert.ToString(reader["account_holder_cell"].ToString()),
                                account_holder_phn = Convert.ToString(reader["account_holder_phn"].ToString()),

                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            account_list.Add(acc);

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
        public void get_all_accounts_entry(DateTime startDate, DateTime endDate)
        {
            total_amount = 0;            
            account_entry_list = new List<account_entry>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_account_entry where date >= @sDate && date <=@eDate ORDER BY account_id";
                    cmd.Parameters.Add("@sDate",MySqlDbType.Date).Value=startDate;
                    cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = endDate;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            account_entry acc = new account_entry()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                account_id = Convert.ToString(reader["account_id"].ToString()),
                                account_name = Convert.ToString(reader["account_name"].ToString()),
                                cheque_no = Convert.ToString(reader["cheque_no"].ToString()),
                                expenditure = Convert.ToString(reader["expenditure"].ToString()),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                date = Convert.ToDateTime(reader["date"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            account_entry_list.Add(acc);
                            total_amount = total_amount + Convert.ToInt32(acc.amount);
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
        public void get_all_accounts_entry(string id)
        {
            total_amount = 0;
            account_entry_list = new List<account_entry>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_account_entry ORDER BY account_id";
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            account_entry acc = new account_entry()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                account_id = Convert.ToString(reader["account_id"].ToString()),
                                account_name = Convert.ToString(reader["account_name"].ToString()),
                                cheque_no = Convert.ToString(reader["cheque_no"].ToString()),
                                expenditure = Convert.ToString(reader["expenditure"].ToString()),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                date = Convert.ToDateTime(reader["date"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };

                            if(acc.date.Month == Convert.ToInt32(id))
                            {
                                account_entry_list.Add(acc);
                                total_amount = total_amount + Convert.ToInt32(acc.amount);
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
            try {
                var dataTable = CreateSampleDataTable();
                var columnWidths = new List<double>() { 70, 220, 280, 120 };
                var ht = new ExpenseHeader();
                var headerTemplate = XamlWriter.Save(ht);
                var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
                printControl.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "AccID", typeof(string));
            AddColumn(dataTable, "Acc Name", typeof(string));
            AddColumn(dataTable, "Desc", typeof(string));
            AddColumn(dataTable, "Amount(Rs)", typeof(string));

            //account acc = (account)account_cmb.SelectedItem;
            foreach (account_entry ae in account_entry_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = ae.account_id.ToString();
                dataRow[1] = ae.account_name.ToString();
                dataRow[2] = ae.expenditure.ToString();
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
            if (date_picker.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker.SelectedDate)
                {
                    dt = date_picker.SelectedDate.Value;
                    month_cmb.SelectedIndex = 0;
                    account_cmb.SelectedIndex = 0;
                    get_all_accounts_entry(date_picker_to.SelectedDate.Value, date_picker.SelectedDate.Value);
                    account_entry_grid.ItemsSource = account_entry_list;
                    total_amount_tb.Text = total_amount.ToString();
                    this.account_entry_grid.Items.Refresh();
                }
            }            
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker.SelectedDate;

                if (date_picker_to.SelectedDate <= date_picker.SelectedDate)
                {
                    month_cmb.SelectedIndex = 0;
                    dt = date_picker.SelectedDate.Value;
                    month_cmb.SelectedIndex = 0;
                    account_cmb.SelectedIndex = 0;
                    get_all_accounts_entry(date_picker_to.SelectedDate.Value, date_picker.SelectedDate.Value);
                    account_entry_grid.ItemsSource = account_entry_list;
                    total_amount_tb.Text = total_amount.ToString();
                    this.account_entry_grid.Items.Refresh();
                }
            }
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sms_months sm = (sms_months)month_cmb.SelectedItem;
            if(sm != null)
            {
                if(sm.id != "0")
                {
                    date_picker.SelectedDate = null;
                    account_cmb.SelectedIndex = 0;
                    get_all_accounts_entry(sm.month_id);
                    account_entry_grid.ItemsSource = account_entry_list;
                    total_amount_tb.Text = total_amount.ToString();
                    this.account_entry_grid.Items.Refresh();
                }
            }
        }

        private void account_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int total = 0;
            account acc = (account)account_cmb.SelectedItem;
            if(acc.id != "0")
            {
                account_entry_grid.ItemsSource = account_entry_list.Where(x=>x.account_id == acc.id);
                account_entry_grid.Items.Refresh();

                foreach (account_entry ae in account_entry_list.Where(x => x.account_id == acc.id)) 
                {
                    total = total + Convert.ToInt32(ae.amount);
                }
                total_amount_tb.Text = total.ToString();
                total_amount = total;
            }
        }        
    }
}
