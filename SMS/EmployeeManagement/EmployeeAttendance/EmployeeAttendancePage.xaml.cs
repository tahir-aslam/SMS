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
using System.IO;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using SMS.ViewModels;

namespace SMS.EmployeeManagement.EmployeeAttendance
{
    /// <summary>
    /// Interaction logic for EmployeeAttendancePage.xaml
    /// </summary>
    public partial class EmployeeAttendancePage : Page
    {
        List<employee_attendence> attendence_list;
        List<employee_attendence> all_attendence_list;
        ObservableCollection<employee_attendence> emp_vm_list;
        ObservableCollection<employee_attendence> emp_vm_list_temp;
        public List<DateTime> group_by_att_dates;
        List<employees> emp_list;
        EmployeeAttendanceForm eaf;
        
        public employee_attendence emp_obj;
        double total_days = 0;
        double total_abs = 0;
        double total_presents = 0;
        double total_leaves = 0;
        double percentage = 0;
        EmployeeViewModel evm;
        List<employees_types> emp_types_list;

        public EmployeeAttendancePage()
        {
            InitializeComponent();
           
            loadgrid();            
        }

        public void loadgrid() 
        {
            attendence_list = new List<employee_attendence>();          

            get_all_employees();
            get_all_attendence();
            populate_emp_vm_list();
            get_all_emp_types();
            attendnce_date.SelectedDate = DateTime.Now; 
            emp_types_list.Insert(0, new employees_types() { emp_types = "---Select Category---", id = "-1" });
            emp_types_cmb.ItemsSource = emp_types_list;
            emp_types_cmb.SelectedIndex = 0;
            evm = new EmployeeViewModel(emp_vm_list, group_by_att_dates);
            attendence_grid.DataContext = evm;                           
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

        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence()
        {
            all_attendence_list = new List<employee_attendence>();
            group_by_att_dates = new List<DateTime>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_emp_attendence where session_id=@session_id  ORDER BY attendence_date DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {               
                           
                            employee_attendence att = new employee_attendence()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),                                
                                att_percentage = Convert.ToString(reader["att_percentage"].ToString()),
                                total_days = Convert.ToString(reader["total_days"].ToString()),
                                total_abs = Convert.ToString(reader["total_abs"].ToString()),
                                total_presents = Convert.ToString(reader["total_presents"].ToString()),
                                total_leaves = Convert.ToString(reader["total_leaves"].ToString()),
                                attendence = Convert.ToChar(reader["attendence"]),
                                attendence_date = Convert.ToDateTime(reader["attendence_date"]),

                            };
                            all_attendence_list.Add(att);

                            if (group_by_att_dates.Exists(x => x.Date == att.attendence_date))
                            {
                            }
                            else
                            {
                                group_by_att_dates.Add(att.attendence_date);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // isPresent Radio Button
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;           
            employee_attendence st = new employee_attendence();
            if (null == checkBox) return;

            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                st = (employee_attendence)attendence_grid.Items[i];
                if (checkBox.IsChecked.Value == true)
                {
                    st.isLeave = false;
                    st.isAbsent = false;
                    st.isPresent = true;
                }
                else
                {
                    st.isLeave = false;
                    st.isAbsent = false;
                    st.isPresent = false;
                }
            }
            attendence_grid.Items.Refresh();

           // attendence_grid.Items.Refresh();
        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as RadioButton;
            attendence_grid.SelectedItem = e.Source;
            employee_attendence st = new employee_attendence();
            st = (employee_attendence)attendence_grid.SelectedItem;
            foreach (var item in emp_vm_list)
            {
                if (item.emp_id == st.emp_id)
                {
                    item.isPresent = true;
                    item.isAbsent = false;
                    item.isLeave = false;
                }
            }
        }

        // isAbsent
        private void CheckBox_Checked_isAbsent(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            employee_attendence st = new employee_attendence();
            if (null == checkBox) return;

            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                st = (employee_attendence)attendence_grid.Items[i];
                if (checkBox.IsChecked.Value == true)
                {
                    st.isLeave = false;
                    st.isAbsent = true;
                    st.isPresent = false;
                }
                else
                {
                    st.isLeave = false;
                    st.isAbsent = false;
                    st.isPresent = false;
                }
            }
            attendence_grid.Items.Refresh();

            // attendence_grid.Items.Refresh();
        }
        private void CheckBox_Checked_sub_isAbsent(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as RadioButton;
            attendence_grid.SelectedItem = e.Source;
            employee_attendence st = new employee_attendence();
            st = (employee_attendence)attendence_grid.SelectedItem;
            foreach (var item in emp_vm_list)
            {
                if (item.emp_id == st.emp_id)
                {
                    item.isPresent = false;
                    item.isAbsent = true;
                    item.isLeave = false;
                }
            }
        }

        // isLeave
        private void CheckBox_Checked_isLeave(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            employee_attendence st = new employee_attendence();
            if (null == checkBox) return;

            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                st = (employee_attendence)attendence_grid.Items[i];
                if (checkBox.IsChecked.Value == true)
                {
                    st.isLeave = true;
                    st.isAbsent = false;
                    st.isPresent = false;
                }
                else                 
                {
                    st.isLeave = false;
                    st.isAbsent = false;
                    st.isPresent = false;
                }
                
            }
            attendence_grid.Items.Refresh();

            // attendence_grid.Items.Refresh();
        }
        private void CheckBox_Checked_sub_isLeave(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as RadioButton;
            attendence_grid.SelectedItem = e.Source;
            employee_attendence st = new employee_attendence();

            st = (employee_attendence)attendence_grid.SelectedItem;
            foreach (var item in emp_vm_list)
            {
                if (item.emp_id == st.emp_id)
                {
                    item.isPresent = false;
                    item.isAbsent = false;
                    item.isLeave = true;
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
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //-----------------           Populate Student view model list----------------------
        public void populate_emp_vm_list()
        {
            List<char> emp_attendence_list;
            List<DateTime> emp_attendence_date_list;
            emp_vm_list = new ObservableCollection<employee_attendence>();


            foreach (employees emp in emp_list)
            {
                employee_attendence emp_vm = new employee_attendence()
                {
                    emp_id = emp.id,
                    emp_name = emp.emp_name,                
                    emp_type_id= emp.emp_type_id,
                };

                emp_attendence_list = new List<char>();
                emp_attendence_date_list = new List<DateTime>();

                foreach (employee_attendence emp_att in all_attendence_list.Where(x => x.emp_id == emp.id))
                {
                    emp_vm.id = emp_att.id;

                    emp_vm.att_percentage = emp_att.att_percentage;
                    emp_vm.total_abs = emp_att.total_abs;
                    emp_vm.total_days = emp_att.total_days;
                    emp_vm.total_presents = emp_att.total_presents;
                    emp_vm.total_leaves = emp_att.total_leaves;

                    for (int i = 0; i < group_by_att_dates.Count; i++)
                    {
                        if (emp_att.attendence_date == group_by_att_dates[i])
                        {
                            emp_attendence_list.Insert(i, emp_att.attendence);
                            emp_attendence_date_list.Insert(i, emp_att.attendence_date);

                            break;
                        }
                        else
                        {
                            if (emp_attendence_date_list.Count <= i)
                            {
                                emp_attendence_list.Insert(i, '-');
                                emp_attendence_date_list.Insert(i, emp_att.attendence_date);
                            }
                        }
                    }
                }


                emp_vm.att_lst = emp_attendence_list;
                emp_vm.att_date_lst = emp_attendence_date_list;
                emp_vm_list.Add(emp_vm);
            }
        }

        //-----------       Submit attendence button             -------------------------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            attendence_list = new List<employee_attendence>();
            if (check_attendence() == false)
            {
                foreach (employee_attendence ea in emp_vm_list)
                {
                    total_days = 0;
                    total_abs = 0;
                    total_presents = 0;
                    total_leaves = 0;
                    percentage = 0;

                    employee_attendence att = new employee_attendence();
                    att = ea;
                    if (att.isPresent == true)
                    {
                        att.attendence = 'P';
                        total_presents++;
                    }
                    else if (att.isAbsent == true)
                    {
                        att.attendence = 'A';
                        total_abs++;
                    }
                    else if (att.isLeave == true)
                    {
                        att.attendence = 'L';
                        total_leaves++;
                    }
                    else
                    {
                        continue;
                    }


                    total_days = att.att_date_lst.Count;
                    foreach (char c in att.att_lst)
                    {
                        if (c == 'P')
                        {
                            total_presents++;
                        }
                        else if(c=='L')
                        {
                            total_leaves++;   
                        }
                        else if (c == 'A')
                        {
                            total_abs++;
                        }
                        else 
                        {
                        }
                    }
                    total_days++;
                    if (total_days != 0)
                    {
                        percentage = total_presents / total_days;
                        percentage = percentage * 100;
                    }

                    att.att_percentage = percentage.ToString("0.00");
                    att.total_abs = total_abs.ToString();
                    att.total_days = total_days.ToString();
                    att.total_presents = total_presents.ToString();
                    att.total_leaves = total_leaves.ToString();
                    att.attendence_date = attendnce_date.SelectedDate.Value;
                    att.created_by = MainWindow.emp_login_obj.emp_user_name;
                    att.date_time = DateTime.Now;
                    attendence_list.Add(att);

                }

                int presents = attendence_list.Where(x => x.isPresent == true).Count();
                int absents = attendence_list.Where(x => x.isAbsent == true).Count();
                int leave = attendence_list.Where(x => x.isLeave == true).Count();

                MessageBoxResult mbr1 = MessageBox.Show("Do you want to submit attendance? Presents=" + presents + "  Absents=" + absents + "  Leaves=" + leave, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr1 == MessageBoxResult.Yes)
                {
                    if (emp_vm_list.Count == attendence_list.Count)
                    {
                        if (submit_attendence() == 0)
                        {
                            if (update() > 0)
                            {
                                MessageBox.Show("Successfully Added", "Successfully Added", MessageBoxButton.OK, MessageBoxImage.Information);
                                loadgrid();
                                delete_button.IsEnabled = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxResult mbr = MessageBox.Show("Do you want to skip  attendance?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mbr == MessageBoxResult.Yes)
                        {
                            if (submit_attendence() == 0)
                            {
                                if (update() > 0)
                                {
                                    MessageBox.Show("Successfully Added", "Successfully Added", MessageBoxButton.OK, MessageBoxImage.Information);
                                    loadgrid();
                                    delete_button.IsEnabled = true;
                                }

                            }
                        }
                        else
                        {
                        }
                    }
                }
                else 
                {
                }            
            }
            else
            {
                MessageBox.Show("You Have Already Submitted Attendence for This Date", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                att_button.IsEnabled = true;
                selectionChanged();
            }

        }

        // ----------    check if already submitted attendence           ---------------------------
        public bool check_attendence()
        {
            DateTime selected_date = attendnce_date.SelectedDate.Value;

            foreach (employee_attendence emp in all_attendence_list)
            {
                if (emp.attendence_date.Date == selected_date.Date)
                {
                    return true;
                }
            }
            return false;
        }

        //---------------           Submit Form    ----------------------------------
        public int submit_attendence()
        {
            int i = 1;
            try
            {
                for (int j = 0; j < attendence_list.Count; j++)
                {
                    employee_attendence sa = new employee_attendence();
                    sa = attendence_list[j];
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_emp_attendence(total_presents,total_abs,total_days,att_percentage,emp_id,attendence_date,attendence,emp_name,created_by,date_time,total_leaves,session_id)Values(@total_presents,@total_abs,@total_days,@att_percentage,@emp_id,@attendence_date,@attendence,@emp_name,@created_by,@date_time, @total_leaves,@session_id)";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            
                            cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.emp_id;
                            cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.emp_name;                            
                            cmd.Parameters.Add("@attendence_date", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sa.attendence_date;
                            cmd.Parameters.Add("@attendence", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.attendence;
                            cmd.Parameters.Add("@att_percentage", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.att_percentage;
                            cmd.Parameters.Add("@total_days", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.total_days;
                            cmd.Parameters.Add("@total_abs", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.total_abs;
                            cmd.Parameters.Add("@total_presents", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.total_presents;
                            cmd.Parameters.Add("@total_leaves", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.total_leaves;

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

        //-----------         Load previous attendence          ------------------------------------------------
        public void load_attendence()
        {
            //sections s = (sections)section_cmb.SelectedItem;

            //foreach (student_attendence st in all_attendence_list)
            //{
            //    if (st.section_id == s.id)
            //    {

            //    }
            //}
        }

        private void att_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            emp_obj = new employee_attendence();
            emp_obj = (employee_attendence)attendence_grid.SelectedItem;
            //MessageBox.Show(std_obj.std_name);
            eaf = new EmployeeAttendanceForm(this);
            eaf.ShowDialog();
            loadgrid();
        }

        public int update()
        {
            int i = 0;
            try
            {
                foreach (employee_attendence st in attendence_list)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_emp_attendence SET att_percentage=@att_percentage,total_days=@total_days,total_abs=@total_abs,total_presents=@total_presents,total_leaves=@total_leaves WHERE emp_id = @id && session_id=@session_id";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st.emp_id;
                            cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd.Parameters.Add("@att_percentage", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st.att_percentage;
                            cmd.Parameters.Add("@total_days", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st.total_days;
                            cmd.Parameters.Add("@total_abs", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st.total_abs;
                            cmd.Parameters.Add("@total_presents", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st.total_presents;
                            cmd.Parameters.Add("@total_leaves", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st.total_leaves;

                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            con.Close();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void emp_types_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            emp_vm_list_temp = new ObservableCollection<employee_attendence>();
            employees_types et = (employees_types)emp_types_cmb.SelectedItem;
            if(et != null)
            {
                if (Convert.ToInt32(et.id) > 0)
                {
                    foreach (employee_attendence ea in emp_vm_list.Where(x=>x.emp_type_id == et.id))
                    {
                        emp_vm_list_temp.Add(ea);
                    }
                    evm = new EmployeeViewModel(emp_vm_list_temp, group_by_att_dates);
                    attendence_grid.DataContext = evm;      
                }
                else 
                {
                    evm = new EmployeeViewModel(emp_vm_list, group_by_att_dates);
                    attendence_grid.DataContext = evm;
                }
            }
        }

        private void edit_button_Click(object sender, RoutedEventArgs e)
        {
            emp_obj = new employee_attendence();
            if(attendence_grid.SelectedItem != null)
            {
                emp_obj = (employee_attendence)attendence_grid.SelectedItem;
                EmployeeAttendanceEdit eae = new EmployeeAttendanceEdit(emp_obj);
                eae.ShowDialog();
                loadgrid();
            }       
            //MessageBox.Show(std_obj.std_name);            
        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are you want to delete all attendance of "+attendnce_date.SelectedDate.Value.ToString("dd-MMM-yy"),"Delete Confirmation",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {

                foreach (employees emp in emp_list)
                {
                    deleteAttendence(emp);
                }
                MessageBox.Show("Successfully Deleted","Deleted",MessageBoxButton.OK,MessageBoxImage.Information);
                loadgrid();
            }

        }

        public void deleteAttendence(employees emp) 
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_emp_attendence where emp_id=" + emp.id + " && attendence_date= @dt";
                    cmd.Connection = con;
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = attendnce_date.SelectedDate.Value;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        private void attendnce_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionChanged();
        }

        public void selectionChanged() 
        {
            DateTime dt = attendnce_date.SelectedDate.Value;
            get_all_attendence();
            if (all_attendence_list.Select(x => x.attendence_date).Contains(dt))
            {
                delete_button.IsEnabled = true;
                //loadgrid();                
            }
            else
            {
                delete_button.IsEnabled = false;
            }
        }
    }
}
