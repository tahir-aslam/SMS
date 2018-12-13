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
using MySql.Data.MySqlClient;

namespace SMS.EmployeeManagement.EmpPayment
{
    /// <summary>
    /// Interaction logic for EmpPayment.xaml
    /// </summary>
    public partial class EmpPayment : Page
    {
        List<employees> emp_list;        
        
        public EmpPayment()
        {
            InitializeComponent();

            emp_list = new List<employees>();            
            SearchTextBox.Focus();
            load_grid();
        }

        public void load_grid()
        {
            emp_list.Clear();
            get_all_employees();
            emp_grid.ItemsSource = emp_list;
            this.emp_grid.Items.Refresh();
        }

        // -------------      Get All Employees       --------------------

        public void get_all_employees()
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {



                    cmd.CommandText = "SELECT* FROM sms_emp where is_active='Y'";
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
    }
}
