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
using System.Collections.ObjectModel;
using System.ComponentModel;
using SMS.ViewModels;


namespace SMS.EmployeeManagement
{
    
    public partial class EmpAttendence : Page
    {
        
     public List<char> employee_attendence_list;     
     public List<employees> emp_attendence_list;
     EmpViewModel evm;
     employees emp;
     List<employees> emp_submit_attendence_list;
     string emp_id = "";
     List<employees_types> emp_types_list;
     
        public EmpAttendence()
        {
            InitializeComponent();

            attendnce_date.SelectedDate = DateTime.Now;
            load_grid();
            evm = new EmpViewModel();
            emp_attendence_grid.DataContext = evm;
        }


        public void load_grid()
        {            
            get_all_emp_attendence();
            get_all_emp_types();
            emp_types_list.Insert(0, new employees_types() { emp_types = "---Select Category---", id = "-1" });
            emp_types_cmb.SelectedIndex = 0;
            emp_types_cmb.ItemsSource = emp_types_list;
            
            
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;
            foreach (var item in EmpViewModel.emp_vm_list)
            {
                item.Active = checkBox.IsChecked.Value;
                emp_attendence_grid.Items.Refresh();
           
            }

            
        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            emp_attendence_grid.SelectedItem = e.Source;
            emp = new employees();
            emp = (employees)emp_attendence_grid.SelectedItem;
            foreach (employees em in EmpViewModel.emp_vm_list)
            {
                if (emp.id == em.id)
                {
                    em.Active = checkBox.IsChecked.Value;
                }
            }

        }
        //---------------           Get All Employees types    ----------------------------------

        public void get_all_emp_types()
        {
            try
            {
                emp_types_list = new List<employees_types>();
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
        //==============      Get All Employee attendence        ==============================

        public void get_all_emp_attendence()
        {
            try
            {
                emp_attendence_list = new List<employees>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_emp_attendence";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["emp_id"].ToString()),

                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                                attendence = Convert.ToChar(reader["attendence"]),
                                attendence_date = Convert.ToDateTime(reader["attendence_date"]),



                            };
                            emp_attendence_list.Add(emp);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Teacher DB not connected");
            }
        }

        //----------    check if already submitted attendence           ---------------------------
        public bool check_attendence()
        {
            get_all_emp_attendence();
            foreach (employees emp in emp_attendence_list)
            {
                if (emp.attendence_date == attendnce_date.SelectedDate.Value)
                {
                    return true;
                }
            }
            return false;
        }

        //  ---------------           Submit Form    ----------------------------------

        public int submit_attendence()
        {
            int i = 1;
            try
            {
                for (int j = 0; j < emp_submit_attendence_list.Count; j++)
                {
                    employees sa = new employees();
                    sa = emp_submit_attendence_list[j];
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_emp_attendence(emp_id,emp_name,emp_type,attendence_date,attendence,created_by,date_time) Values(@emp_id,@emp_name,@emp_type,@attendence_date,@attendence,@created_by,@date_time)";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.id;
                            cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.emp_name;
                            cmd.Parameters.Add("@emp_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.emp_type;
                            cmd.Parameters.Add("@attendence", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.attendence;
                            cmd.Parameters.Add("@attendence_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = sa.attendence_date;


                            cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.created_by;
                            cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sa.date_time;


                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteScalar());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            emp_submit_attendence_list = new List<employees>();
            for (int row = 0; row < emp_attendence_grid.Items.Count; row++)
            {
                
                employees att = new employees();
                att = (employees)(emp_attendence_grid.Items[row]);
                if (att.Active == true)
                {
                    att.attendence = 'P';
                }
                else
                {
                    att.attendence = 'A';
                }
                att.attendence_date = attendnce_date.SelectedDate.Value;
                att.created_by = MainWindow.emp_login_obj.emp_user_name;
                att.date_time = DateTime.Now;
                emp_submit_attendence_list.Add(att);

            }


            if (check_attendence() == false)
            {
                if (submit_attendence() == 0)
                {
                    MessageBox.Show("Successfully added");
                    this.load_grid();
                    emp_attendence_grid.DataContext = null;
                    evm = new EmpViewModel();
                    emp_attendence_grid.DataContext = evm;
                    emp_attendence_grid.Items.Refresh();
                    
                }
            }
            else
            {
                MessageBox.Show("You Have Already Submitted Attendence for This DATE");
                att_button.IsEnabled = true;
            }
        }

        private void emp_attendence_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void emp_types_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            employees_types et = new employees_types();
            et = (employees_types)emp_types_cmb.SelectedItem;
            
        }

    }

    
    
}
