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
using SMS.EmployeeManagement.ADDEMP;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using System.ComponentModel;

namespace SMS.EmployeeManagement.WithdrawEmployees
{
    /// <summary>
    /// Interaction logic for WithdrawEmployeesPage.xaml
    /// </summary>
    public partial class WithdrawEmployeesPage : Page
    {
        List<employees> emp_list;
        AddEmpForm AEF;
        employees obj;
        string mode;
        string insertion;        

        public WithdrawEmployeesPage()
        {
            InitializeComponent();

            emp_list = new List<employees>();
            obj = new employees();
            SearchTextBox.Focus();
            load_grid();
            strength_textblock.Text = emp_grid.Items.Count.ToString();
        }
        
        public void load_grid() 
        {
            emp_list.Clear();
            get_all_employees();
            emp_grid.ItemsSource = emp_list;
            this.emp_grid.Items.Refresh();
        }

       
        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
        }

        // -------------      Get All Employees       --------------------

        public void get_all_employees() 
        {
            emp_list = new List<employees>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp where is_active='N'";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_father = Convert.ToString(reader["emp_father"].ToString()),
                                emp_nationality = Convert.ToString(reader["emp_nationality"].ToString()),
                                emp_religion = Convert.ToString(reader["emp_religion"].ToString()),
                                emp_exp = Convert.ToString(reader["emp_exp"].ToString()),
                                emp_cnic = Convert.ToString(reader["emp_cnic"].ToString()),
                                emp_qual = Convert.ToString(reader["emp_qual"].ToString()),
                                emp_sex = Convert.ToString(reader["emp_sex"].ToString()),
                                emp_marital = Convert.ToString(reader["emp_marital"].ToString()),
                                emp_dob = Convert.ToDateTime(reader["emp_dob"]),
                                joining_date = Convert.ToDateTime(reader["joining_date"]),
                                leaving_date = Convert.ToDateTime(reader["leaving_date"]),
                                emp_email = Convert.ToString(reader["emp_email"].ToString()),
                                emp_address = Convert.ToString(reader["emp_address"].ToString()),
                                emp_remarks = Convert.ToString(reader["emp_remarks"].ToString()),
                                emp_pay = Convert.ToString(reader["emp_pay"].ToString()),
                                emp_cell = Convert.ToString(reader["emp_cell"].ToString()),
                                emp_phone = Convert.ToString(reader["emp_phone"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                            };
                            emp_list.Add(emp);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("oops! Employees DB not connected");
                    }

                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            emp_grid.ItemsSource = emp_list.Where(x => x.emp_name.ToUpper().StartsWith(v_search.ToUpper()));
            emp_grid.Items.Refresh();
        }


        // ==============        Printing        ==============================================

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 150, 150, 100, 100, 100, 100, 70,70,70 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }

        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Employee Name", typeof(string));
            AddColumn(dataTable, "Father Name", typeof(string));
            AddColumn(dataTable, "Emp Type", typeof(string));
            AddColumn(dataTable, "Cell#", typeof(string));
            AddColumn(dataTable, "Qualification", typeof(string));            
            AddColumn(dataTable, "Pay", typeof(string));
            AddColumn(dataTable, "DOB", typeof(string));
            AddColumn(dataTable, "Joining", typeof(string));
            AddColumn(dataTable, "Leaving", typeof(string));



            foreach (employees c in emp_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = c.emp_name.ToString();
                dataRow[1] = c.emp_father.ToString();
                dataRow[2] = c.emp_type.ToString();                
                dataRow[3] = c.emp_cell.ToString();
                dataRow[4] = c.emp_qual.ToString();                
                dataRow[5] = c.emp_pay.ToString();
                dataRow[6] = c.emp_dob.ToString("dd MMM yy");
                dataRow[7] = c.joining_date.ToString("dd MMM yy");
                dataRow[8] = c.leaving_date.ToString("dd MMM yy");
                                

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        private void click_edit(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Restore This Employee ?", "Restore Confirmation", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                obj = (employees)emp_grid.SelectedItem;
                if (obj == null)
                {
                    //MessageBox.Show("plz select a row");
                }
                else
                {
                    update_restore();
                    MessageBox.Show("Restored Successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                    load_grid();
                }
                
            }       
        }

        public void update_restore() 
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_emp SET is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;                      

                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "Y";
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();
                        cmd.ExecuteScalar();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(emp_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = emp_grid.Items.Count.ToString();
        }
       
    }
}

