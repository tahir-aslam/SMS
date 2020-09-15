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

namespace SMS.EmployeeManagement.EmpRoles
{
    /// <summary>
    /// Interaction logic for EmployeeRolesSearch.xaml
    /// </summary>
    public partial class EmployeeRolesSearch : Page
    {

        List<emp_login> emp_login_list;
        List<employees> emp_list;
        EmployeeRolesForm erf;
        emp_login row_obj;
        string mode;
        string insertion;
        string updation;

        public EmployeeRolesSearch()
        {
            InitializeComponent();

            row_obj = new emp_login();
            //SearchTextBox.Focus();
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
                erf = new EmployeeRolesForm(mode, this, row_obj);
                erf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                erf.ShowDialog();
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
                            if (el.emp_id != "0")
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
                foreach (employees emp in emp_list)
                {
                    if (el.emp_id == emp.id)
                    {
                        el.emp_name = emp.emp_name;
                    }
                }
            }
        }
    }
}
