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
using SMS.StudentManagement.StudentAttendence;
using SMS.Messaging.SmsOption;
using SMS.Messaging.BrandedSms;
using SMS.Upload;

namespace SMS.Messaging.EmpAttendanceSms
{
    /// <summary>
    /// Interaction logic for EmpAttendanceSmsPage.xaml
    /// </summary>
    public partial class EmpAttendanceSmsPage : Page
    {
        List<employee_attendence> attendence_list;
        List<employee_attendence> all_attendence_list;
        List<employee_attendence> abs_attendence_list;
        ObservableCollection<employee_attendence> emp_vm_list;       
        public List<DateTime> group_by_att_dates;
        List<employees> emp_list;
        public static bool isbranded = false;
        public employee_attendence emp_obj;        
        EmployeeViewModel evm;        

        public EmpAttendanceSmsPage()
        {
            InitializeComponent();            
            get_all_employees();
            strength_textblock.Text = attendence_grid.Items.Count.ToString();
            attendnce_date.SelectedDate = DateTime.Now;
            
            //loadgrid();            
        }
        public void loadgrid()
        {
            attendence_list = new List<employee_attendence>();
            attendnce_date.SelectedDate = DateTime.Now;            
            populate_emp_vm_list();                        
            
            evm = new EmployeeViewModel(emp_vm_list, group_by_att_dates);
            attendence_grid.DataContext = evm;
        }
        
        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence(DateTime dt)
        {
            all_attendence_list = new List<employee_attendence>();
            group_by_att_dates = new List<DateTime>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_emp_attendence where attendence_date = @dt && attendence='A' ORDER BY attendence_date DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
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

        //---------------           Get All Absents of current month    ----------------------------------
        public void get_all_attendence_month(DateTime dt, string id)
        {
            abs_attendence_list = new List<employee_attendence>();            
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_emp_attendence where emp_id=@id && attendence='A' ORDER BY attendence_date DESC";
                        
                        cmd.Connection = con;
                        cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                        cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
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
                                attendence = Convert.ToChar(reader["attendence"]),
                                attendence_date = Convert.ToDateTime(reader["attendence_date"]),

                            };
                            abs_attendence_list.Add(att);                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            //if (null == checkBox) return;
            //foreach (var item in emp_vm_list)
            //{
            //    item.Active = checkBox.IsChecked.Value;
            //}

            employee_attendence st = new employee_attendence();
            if (null == checkBox) return;

            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                st = (employee_attendence)attendence_grid.Items[i];
                st.Active = checkBox.IsChecked.Value;
            }
            attendence_grid.Items.Refresh();

            // attendence_grid.Items.Refresh();
        }

        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            attendence_grid.SelectedItem = e.Source;
            employee_attendence st = new employee_attendence();
            st = (employee_attendence)attendence_grid.SelectedItem;
            foreach (var item in emp_vm_list)
            {
                if (item.emp_id == st.emp_id)
                {
                    item.Active = checkBox.IsChecked.Value;
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
                    catch
                    {
                        MessageBox.Show("oops! Employees DB not connected");
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
                    emp_type_id = emp.emp_type_id,
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
                    emp_vm.att_lst = emp_attendence_list;
                    emp_vm.att_date_lst = emp_attendence_date_list;
                    emp_vm_list.Add(emp_vm);
                    break;
                }
                
            }
        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt = (DateTime)attendnce_date.SelectedDate;
            if (dt != null)
            {
                attendence_list = new List<employee_attendence>();
                get_all_attendence(dt);                                
                populate_emp_vm_list();
                evm = new EmployeeViewModel(emp_vm_list, group_by_att_dates);
                attendence_grid.DataContext = evm;
                
            }
        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            DateTime dt = (DateTime)attendnce_date.SelectedDate;
            admission adm = new admission();
            List<admission> std_nos = new List<admission>();
            string message = "";
            foreach (employee_attendence st in emp_vm_list.Where(x => x.Active == true))
            {
                count = 0;
                get_all_attendence_month(dt,st.emp_id);
                foreach(employee_attendence abs in abs_attendence_list)
                {
                    if(dt.Month == abs.attendence_date.Month)
                    {
                        count++;
                    }
                }

                message = "Respected " + st.emp_name + ", You Are Absent From Institute On " + dt.ToString("dd MMM yyyy") + ". ";
                message = message + "Your Total " + dt.ToString("MMM") + " Absents are " + count + ". Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;

                foreach (employees emp in emp_list.Where(x => x.id == st.emp_id))
                {
                    adm = new admission();
                    adm.std_name = emp.emp_name;
                    adm.father_name = emp.emp_father;
                    adm.class_name = "-";
                    adm.section_name = "-";
                    adm.cell_no = emp.emp_cell;                    
                    adm.sms_message = message;
                    adm.sms_type = "Emp Attendance Sms";
                    std_nos.Add(adm);
                    break;
                }
            }
            if (std_nos.Count > 0)
            {
                isbranded = false;
                OptionWindow ow = new OptionWindow();
                ow.ShowDialog();

                if (isbranded == true)
                {
                    BrandedSmsEngine bse = new BrandedSmsEngine(std_nos);
                    bse.Show();
                }
                else
                {
                    UploadWindow uw = new UploadWindow(std_nos,false);
                    uw.Show();
                }

            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(attendence_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = attendence_grid.Items.Count.ToString();
        }
    }
}
