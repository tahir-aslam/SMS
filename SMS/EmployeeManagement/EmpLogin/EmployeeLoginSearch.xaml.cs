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
using SMS.EmployeeManagement.EmpLogin;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;

namespace SMS.EmployeeManagement.EmpLogin
{
    /// <summary>
    /// Interaction logic for EmployeeLoginSearch.xaml
    /// </summary>
    public partial class EmployeeLoginSearch : Page
    {
        List<emp_login> emp_login_list;
        List<employees> emp_list;
        EmployeeLoginForm elf;
        emp_login row_obj;
        string mode;
        string insertion;
        string updation;

        public EmployeeLoginSearch()
        {
            InitializeComponent();            
            row_obj = new emp_login();
            SearchTextBox.Focus();
            load_grid();

        }

        public void load_grid()
        {            
            get_all_emp_login();
            get_all_employees();
            set_emp_login_list();
            emp_login_grid.ItemsSource = emp_login_list;
            this.emp_login_grid.Items.Refresh();
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            elf = new EmployeeLoginForm(mode, this, row_obj);
            elf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            elf.ShowDialog();
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (emp_login)emp_login_grid.SelectedItem;
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
                        cmd.CommandText = "insert into sms_emp_login_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_emp_login where id = " + id;
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
                    cmd.CommandText = "Delete from sms_emp_login where id=" + row_obj.id;
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
            row_obj = (emp_login)emp_login_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                elf = new EmployeeLoginForm(mode, this, row_obj);
                elf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                elf.ShowDialog();
            }
        }

        //-----------       Get All Employee Login    ----------------------

        public void get_all_emp_login()
        {
            emp_login_list = new List<emp_login>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_emp_login";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            emp_login el = new emp_login()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_user_name = Convert.ToString(reader["emp_user_name"].ToString()),
                                emp_pwd = Convert.ToString(reader["emp_pwd"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            if(el.emp_id != "0")
                            {
                                emp_login_list.Add(el);
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }
        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            emp_login_grid.ItemsSource = emp_login_list.Where(x => x.emp_name.ToUpper().StartsWith(v_search.ToUpper()));
            emp_login_grid.Items.Refresh();
        }



        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 200, 150, 120, 150 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }


        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Employee Name", typeof(string));
            AddColumn(dataTable, "User Name", typeof(string));
            AddColumn(dataTable, "Created By", typeof(string));
            AddColumn(dataTable, "DateTime", typeof(string));



            foreach (emp_login el in emp_login_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = el.emp_name;
                dataRow[1] = el.emp_user_name;
                dataRow[2] = el.created_by.ToString();
                dataRow[3] = el.date_time.ToString();

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        // -------------      Get All Employees       --------------------

        public void get_all_employees()
        {
            emp_list = new List<employees>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp";
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

        public void set_emp_login_list() 
        {
            foreach (emp_login el in emp_login_list) 
            {
                foreach(employees emp in emp_list)
                {
                    if(el.emp_id == emp.id)
                    {
                        el.emp_name = emp.emp_name;
                    }
                }
            }
        }
    }
}
