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
using SMS.AdmissionManagement.FineManagement.Fine;

namespace SMS.AdmissionManagement.FineManagement.Fine
{
    /// <summary>
    /// Interaction logic for FineSearch.xaml
    /// </summary>
    public partial class FineSearch : Page
    {
        List<sms_fine_type> fine_type_list;
        List<sms_fine> fine_list;
        FineWindow fw;
        sms_fine row_obj;
        string mode;
        string insertion;
        string updation;
        public static int total_amount = 0;
        public static DateTime dt = DateTime.Now;
        List<sms_months> months_list;
        List<admission> adm_list;
        int fine = 0;
        int rem_fine = 0;
        List<fee> paid_fee_list;
        List<sms_InsertUpdateStatus> statusList;
        sms_InsertUpdateStatus statusObj;

        public FineSearch()
        {
            InitializeComponent();

            get_all_admissions();
            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;

            get_all_fine_types();
            fine_type_cmb.SelectedIndex = 0;
            fine_type_list.Insert(0, new sms_fine_type() { fine_type = "--Select Type--", id = "-1" });
            fine_type_cmb.ItemsSource = fine_type_list;

            row_obj = new sms_fine();
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
            date_picker.SelectedDate = DateTime.Now;
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            fw = new FineWindow(mode, this, row_obj);
            fw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            fw.ShowDialog();
        }

        // ===============     Get All Admissions          ================
        public void get_all_admissions()
        {
            adm_list = new List<admission>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && session_id=" + MainWindow.session.id;
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();                   

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                        };
                        adm_list.Add(adm);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (sms_fine)fine_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("Please Select A Row", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(row_obj.id);
                    if (delete(row_obj) == 1)
                    {
                        if (check == false)
                        {
                            insert_deleted(row_obj.id);
                        }
                        MessageBox.Show("Deleted","Successfully",MessageBoxButton.OK,MessageBoxImage.Information);
                        load_grid();
                    }
                    else
                    {
                        load_grid();
                        MessageBox.Show("OOPs! Cannot Delete", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
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
                        cmd.CommandText = "insert into sms_fine_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_fine where id = " + id;
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
        public int delete(sms_fine obj)
        {
            int i = 0;
            statusList = new List<sms_InsertUpdateStatus>();
            statusObj = new sms_InsertUpdateStatus();
            statusObj.adm_no = obj.adm_no;
            statusObj.std_name = obj.std_name;
            statusObj.operation = "Update Fine";            

            if (check_fee_paid(obj.std_id, obj.month))
            {
                statusObj.status = "Error";
            }
            else
            {
                try
                {
                    statusObj.status = "Success";
                    getFeeData(obj.month, obj.std_id);

                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Delete from sms_fine WHERE id = @id";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                            cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            con.Close();
                        }
                    }

                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {
                            cmd_fee.CommandText = "Update sms_fee SET fine_fee=@fine_fee, rem_fine_fee=@rem_fine_fee where std_id = @std_id && month=@months && session_id=" + MainWindow.session.id;
                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.std_id;
                            cmd_fee.Parameters.Add("@months", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.month;

                            fine = fine - Convert.ToInt32(obj.amount);
                            rem_fine = rem_fine - Convert.ToInt32(obj.amount);

                            cmd_fee.Parameters.Add("@fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine.ToString();
                            cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_fine.ToString();

                            con_fee.Open();
                            cmd_fee.ExecuteScalar();
                            con_fee.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return i;
        }

        public void getFeeData(string month, string id)
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where std_id = @id && month=@month && session_id =" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;
                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fine = Convert.ToInt32(reader["fine_fee"]);
                            rem_fine = Convert.ToInt32(reader["rem_fine_fee"]);
                        };

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public bool check_fee_paid(string std_id, string month)
        {
            int total = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where std_id=@id && month=@month && session_id=" + MainWindow.session.id;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = std_id;
                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee paid_fee = new fee()
                            {
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                            };
                            try
                            {
                                total = total + Convert.ToInt32(paid_fee.reg_fee) + Convert.ToInt32(paid_fee.adm_fee) + Convert.ToInt32(paid_fee.security_fee) + Convert.ToInt32(paid_fee.exam_fee) + Convert.ToInt32(paid_fee.transport_fee) + Convert.ToInt32(paid_fee.tution_fee) + Convert.ToInt32(paid_fee.fine_fee) + Convert.ToInt32(paid_fee.other_expenses);
                            }
                            catch (Exception ex)
                            {
                                total = 1;
                            }


                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (total > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            row_obj = (sms_fine)fine_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                mode = "edit";
                fw = new FineWindow(mode, this, row_obj);
                fw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                fw.ShowDialog();
            }
        }

        //-----------       Get All Fine Types    ----------------------
        public void get_all_fine_types()
        {
            fine_type_list = new List<sms_fine_type>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_fine_types";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fine_type acc = new sms_fine_type()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                fine_type = Convert.ToString(reader["fine_type"].ToString()),                               
                            };
                            fine_type_list.Add(acc);

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
        public void get_all_fine(DateTime dt)
        {
            total_amount = 0;
            fine_list = new List<sms_fine>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_fine where fine_date=@dt ORDER BY fine_type_id";
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fine sf = new sms_fine()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                session_id = Convert.ToString(reader["session_id"].ToString()),
                                fine_type_id = Convert.ToString(reader["fine_type_id"].ToString()),
                                fine_type = Convert.ToString(reader["fine_type"].ToString()),
                                fine_description = Convert.ToString(reader["fine_description"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                monthId = Convert.ToString(reader["monthId"].ToString()),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                fine_date = Convert.ToDateTime(reader["fine_date"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),                                
                            };
                            fine_list.Add(sf);
                            total_amount = total_amount + Convert.ToInt32(sf.amount);
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
        public void get_all_fine(string id)
        {
            total_amount = 0;
            fine_list = new List<sms_fine>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_fine ORDER BY fine_type_id";
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fine sf = new sms_fine()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                session_id = Convert.ToString(reader["session_id"].ToString()),
                                fine_type_id = Convert.ToString(reader["fine_type_id"].ToString()),
                                fine_type = Convert.ToString(reader["fine_type"].ToString()),
                                fine_description = Convert.ToString(reader["fine_description"].ToString()),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                monthId = Convert.ToString(reader["monthId"].ToString()),
                                fine_date = Convert.ToDateTime(reader["fine_date"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            
                            if (sf.fine_date.Month == Convert.ToInt32(id))
                            {
                                fine_list.Add(sf);
                                total_amount = total_amount + Convert.ToInt32(sf.amount);
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

            AddColumn(dataTable, "AccID", typeof(string));
            AddColumn(dataTable, "Acc Name", typeof(string));
            AddColumn(dataTable, "Desc", typeof(string));
            AddColumn(dataTable, "Amount(Rs)", typeof(string));


            foreach (sms_fine sf in fine_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = sf.std_id.ToString();
                dataRow[1] = sf.std_id.ToString();
                dataRow[2] = sf.std_id.ToString();
                dataRow[3] = sf.amount.ToString();

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
                month_cmb.SelectedIndex = 0;
                fine_type_cmb.SelectedIndex = 0;
                get_all_fine(dt);
                fillFineList();
                fine_grid.ItemsSource = fine_list;
                total_amount_tb.Text = total_amount.ToString();
                this.fine_grid.Items.Refresh();
            }
        }

        public void fillFineList() 
        {
            foreach(sms_fine sf in fine_list)
            {
                foreach (admission adm in adm_list.Where(x => x.id == sf.std_id)) 
                {
                    sf.std_name = adm.std_name;
                    sf.father_name = adm.father_name;
                    sf.adm_no = adm.adm_no;
                    sf.class_name = adm.class_name;
                    sf.section_name = adm.section_name;
                }
            }
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sms_months sm = (sms_months)month_cmb.SelectedItem;
            if (sm != null)
            {
                if (sm.id != "0")
                {
                    date_picker.SelectedDate = null;
                    fine_type_cmb.SelectedIndex = 0;
                    get_all_fine(sm.month_id);
                    fillFineList();
                    fine_grid.ItemsSource = fine_list;
                    total_amount_tb.Text = total_amount.ToString();
                    this.fine_grid.Items.Refresh();
                }
            }
        }

        private void account_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int total = 0;
            sms_fine_type acc = (sms_fine_type)fine_type_cmb.SelectedItem;
            if (acc.id != "0")
            {
                fine_grid.ItemsSource = fine_list.Where(x => x.fine_type_id == acc.id);
                fine_grid.Items.Refresh();

                foreach (sms_fine ae in fine_list.Where(x => x.fine_type_id == acc.id))
                {
                    total = total + Convert.ToInt32(ae.amount);
                }
                total_amount_tb.Text = total.ToString();
            }
        }        
    }
}
