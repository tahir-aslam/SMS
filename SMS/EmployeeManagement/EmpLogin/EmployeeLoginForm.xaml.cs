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
using System.Windows.Shapes;
using SMS.Models;
using SMS.EmployeeManagement.EmpLogin;
using MySql.Data.MySqlClient;

namespace SMS.EmployeeManagement.EmpLogin
{
    /// <summary>
    /// Interaction logic for EmployeeLoginForm.xaml
    /// </summary>
    public partial class EmployeeLoginForm : Window
    {
        EmployeeLoginSearch els;
        List<emp_login> emp_login_list;
        List<employees> emp_list;
        List<employees_types> emp_types_list;
        emp_login emp_login_obj;
        emp_login obj;
        string mode;

        public EmployeeLoginForm(string mode, EmployeeLoginSearch ELS, emp_login obj)
        {
            InitializeComponent();

            els = ELS;
            this.obj = obj;
            this.mode = mode;
            emp_types_cmb.Focus();

            get_all_emp_login();
            get_all_emp_types();
            get_all_employees();

            emp_types_cmb.SelectedIndex = 0;
            emp_types_list.Insert(0, new employees_types() { emp_types = "---Select Category---", id = "-1" });
            emp_types_cmb.ItemsSource = emp_types_list;

            emp_cmb.IsEnabled = false;
            //emp_cmb.SelectedIndex = 0;
            //emp_list.Insert(0, new employees() { emp_name = "---Select Employee---", id = "-1" });
            //emp_cmb.ItemsSource = emp_list;

            if (mode == "edit")
            {
                fill_control();
                emp_cmb.IsEnabled = false;
                emp_types_cmb.IsEnabled = false;
                user_name_textbox.IsEnabled = false;
            }
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //--------------          Click_Save             -----------------------------------

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        //---------------           Submit Form    ----------------------------------

        public int submit()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_emp_login(emp_id,emp_user_name,emp_pwd,emp_type_id,created_by,date_time) Values(@emp_id,@emp_user_name,@emp_pwd,@emp_type_id,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.emp_id;
                        cmd.Parameters.Add("@emp_user_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@emp_pwd", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.emp_pwd;
                        cmd.Parameters.Add("@emp_type_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.emp_type_id;
                        
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = emp_login_obj.date_time;


                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Form        ---------------------------------

        public int update()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_emp_login SET emp_pwd=@emp_pwd,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.emp_id;
                        cmd.Parameters.Add("@emp_user_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@emp_pwd", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.emp_pwd;

                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_login_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = emp_login_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

       
        //------------------    Fill Object      ------------------------

        public void fill_object()
        {
            emp_login_obj = new emp_login();

            employees_types et = (employees_types)emp_types_cmb.SelectedItem;
            employees emp = (employees)emp_cmb.SelectedItem;

            emp_login_obj.emp_id = emp.id;
            emp_login_obj.emp_type_id = et.id;
            emp_login_obj.emp_user_name = user_name_textbox.Text.Trim();
            emp_login_obj.emp_pwd = pwd_textbox.Password;
            emp_login_obj.date_time = DateTime.Now;
            emp_login_obj.created_by = MainWindow.emp_login_obj.emp_user_name;


        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            emp_types_cmb.SelectedValue = obj.emp_type_id;
            emp_cmb.SelectedValue = obj.emp_id;
            user_name_textbox.Text = obj.emp_user_name;
            pwd_textbox.Password = obj.emp_pwd;
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (emp_types_cmb.SelectedIndex == 0)
            {
                emp_types_cmb.Focus();
                string alertText = "Designation Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (emp_cmb.SelectedIndex == 0)
            {
                emp_cmb.Focus();
                string alertText = "Employee Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (pwd_textbox.Password.Length == 0)
            {
                pwd_textbox.Focus();
                string alertText = "Password Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (pwd_textbox.Password != re_pwd_textbox.Password)
            {
                pwd_textbox.Focus();
                string alertText = "Please Re-type Password";
                MessageBox.Show(alertText);
                return false;
            }
            else
            {
                return true;
            }

        }

        //------------           Check Login   -------------------

       public bool check_login()
        {
            foreach (emp_login el in emp_login_list)
            {
                if (el.emp_id.ToString().Equals(emp_login_obj.emp_id.ToString()))
                {
                    return true;
                }
            }
            return false;
        }


        //--------------           Save          ----------------------

        public void save()
        {
            fill_object();
            if (validate())
            {
                if (mode == "insert")
                {
                    if (check_login() == false)
                    {
                        if (submit() > 0)
                        {
                            MessageBox.Show("Record Added Successfully");
                            this.Close();
                            els.load_grid();
                        }
                        else
                        {
                            MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Employee Login Already Exists");
                    }
                }
                else if (mode == "edit")
                {
                    if (update() > 0)
                    {
                        MessageBox.Show("Record Updated Successfully");
                        this.Close();
                        els.load_grid();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }

                }
                else
                {
                    MessageBox.Show("mode not set");
                }

            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();
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
                            emp_login_list.Add(el);

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

        // -------------      Get All Employees       --------------------
        public void get_all_employees()
        {
            emp_list = new List<employees>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp ";
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
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                            };
                            if (emp.id != "0")
                            {
                                emp_list.Add(emp);
                            }

                        }
                    }
                    catch
                    {
                        MessageBox.Show("oops! Employees DB not connected");
                    }

                }
            }
        }

        //---------------           Get All Employees types    ----------------------------------
        public void get_all_emp_types()
        {
            emp_types_list = new List<employees_types>();
            try
            {

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {

                        cmd.CommandText = "SELECT* FROM sms_emp_types";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees_types emp_types = new employees_types()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_types = Convert.ToString(reader["emp_types"].ToString()),
                            };
                            emp_types_list.Add(emp_types);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Employees Types DB not connected");
            }
        }

        private void emp_types_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            employees_types et = (employees_types)emp_types_cmb.SelectedItem;
            if(emp_types_cmb.SelectedIndex != 0)
            {
                emp_cmb.Items.Clear();
                emp_cmb.IsEnabled = true;                
                emp_cmb.SelectedIndex = 0;
                emp_cmb.Items.Insert(0, new employees() { emp_name = "---Select Employee---", id = "-1" });
                foreach(employees emp in emp_list)
                {
                    if(emp.emp_type_id == et.id)
                    {
                        emp_cmb.Items.Add(emp);
                    }
                }
                
                //emp_cmb.ItemsSource = emp_list.Where(x => x.emp_type_id == et.id);
            }
            else
            {
                emp_cmb.SelectedIndex = 0;
                emp_cmb.IsEnabled = false;
            }
        }

    }
}
