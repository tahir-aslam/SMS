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

namespace SMS.Messaging.AttendenceSms
{
    /// <summary>
    /// Interaction logic for AttendenceSmsPage.xaml
    /// </summary>
    public partial class AttendenceSmsPage : Page
    {
        List<student_attendence> abs_attendence_list;
        List<student_attendence> attendence_list;
        List<student_attendence> all_attendence_list;
        List<classes> classes_list;
        List<sections> sections_list;
        ObservableCollection<student_attendence> std_vm_list;
        ObservableCollection<student_attendence> std_vm_list1;
        public List<DateTime> group_by_att_dates;
        List<admission> adm_list;
        StudentViewModel svm;
        string sectionid = "";
        public student_attendence std_obj;
        StudentAttendenceForm stf;
        double total_days = 0;
        double total_abs = 0;
        double total_presents = 0;
        double percentage = 0;
        public static bool isbranded = false;
        int totalAbsents = 0;
        int totalLeaves = 0;

        public AttendenceSmsPage()
        {
            InitializeComponent();
            std_vm_list = new ObservableCollection<student_attendence>();
            group_by_att_dates = new List<DateTime>();
            attendence_list = new List<student_attendence>();
            sections_list = new List<sections>();
            classes_list = new List<classes>();
            get_all_classes();

            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            section_cmb.IsEnabled = false;

            class_cmb.ItemsSource = classes_list;
            //attendnce_date.SelectedDate = DateTime.Now;
            strength_textblock.Text = attendence_grid.Items.Count.ToString();

            if (MainWindow.ins.isMultiPartSMSAccess == "Y")
            {
                encodedRB.IsEnabled = true;
            }
            else
            {
                encodedRB.IsEnabled = false;
            }
            default_btn.IsChecked = true;
        }


        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt = (DateTime)attendnce_date.SelectedDate;
            get_all_admissions();
            get_all_attendence(dt);
            populate_emp_vm_list();

            svm = new StudentViewModel(std_vm_list, group_by_att_dates);
            attendence_grid.DataContext = svm;

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            student_attendence sa;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;
            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                sa = (student_attendence)attendence_grid.Items[i];
                sa.Active = checkBox.IsChecked.Value;
            }
            attendence_grid.Items.Refresh();
        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            attendence_grid.SelectedItem = e.Source;
            student_attendence st = new student_attendence();
            st = (student_attendence)attendence_grid.SelectedItem;
            foreach (var item in std_vm_list)
            {
                if (item.std_id == st.std_id)
                {
                    item.Active = checkBox.IsChecked.Value;
                }
            }
        }

        //============     Get All Admission data                 ===========================================
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
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
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

        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence(DateTime dt)
        {
            all_attendence_list = new List<student_attendence>();
            group_by_att_dates = new List<DateTime>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_student_attendence  where attendence_date = @dt && (attendence='A' || attendence='L') ORDER BY attendence_date DESC";
                        //cmd.CommandText = "SELECT* FROM sms_student_attendence group by std_id  where Month(attendence_date) = Month(@dt) && Year(attendence_date) = Year(@dt) && (attendence='A' || attendence='L')";
                        //cmd.CommandText = "select *, Count(If(st.attendence='A',1,NULL)) as TAbsents  , Count(If(st.attendence='L',1,NULL)) as TLeaves from sms_student_attendence as st" +
                        //"where Month(st.attendence_date)=Month(@dt) && (st.attendence='A' || st.attendence='L') " +
                        //"group by st.std_id";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            student_attendence att = new student_attendence()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                section_id = Convert.ToString(reader["section_id"].ToString()),
                                roll_no = Convert.ToString(reader["roll_no"].ToString()),
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

        //---------------           Get All Absents of current month    ----------------------------------
        public void get_all_attendence_month(DateTime dt, string id)
        {
            totalAbsents = 0;
            totalLeaves = 0;

            abs_attendence_list = new List<student_attendence>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT Count(If(attendence='A',1,NULL)) as TAbsents  , Count(If(attendence='L',1,NULL)) as TLeaves  FROM sms_student_attendence where Month(attendence_date) = Month(@dt) && Year(attendence_date) = Year(@dt) &&  std_id=@id ";

                        cmd.Connection = con;
                        cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                        cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            totalAbsents = Convert.ToInt32(reader[0]);
                            totalLeaves = Convert.ToInt32(reader[1]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //-----------------           Populate Student view model list----------------------
        public void populate_emp_vm_list()
        {
            List<char> std_attendence_list;
            List<DateTime> std_attendence_date_list;
            std_vm_list = new ObservableCollection<student_attendence>();


            foreach (admission adm in adm_list)
            {
                foreach (student_attendence st in all_attendence_list.Where(x => x.std_id == adm.id))
                {
                    student_attendence std_vm = new student_attendence()
                    {
                        std_id = adm.id,
                        std_name = adm.std_name,
                        roll_no = adm.roll_no,
                        class_id = adm.class_id,
                        section_id = adm.section_id,
                        adm_no = adm.adm_no,
                        class_name = adm.class_name,
                        section_name = adm.section_name,
                    };

                    std_attendence_list = new List<char>();
                    std_attendence_date_list = new List<DateTime>();

                    foreach (student_attendence std_att in all_attendence_list.Where(x => x.std_id == adm.id))
                    {
                        std_vm.id = std_att.id;

                        std_vm.att_percentage = std_att.att_percentage;
                        std_vm.total_abs = std_att.total_abs;
                        std_vm.total_days = std_att.total_days;
                        std_vm.total_presents = std_att.total_presents;
                        std_vm.total_leaves = std_att.total_leaves;
                        std_vm.attendence = std_att.attendence;

                        for (int i = 0; i < group_by_att_dates.Count; i++)
                        {
                            if (std_att.attendence_date == group_by_att_dates[i])
                            {
                                std_attendence_list.Insert(i, std_att.attendence);
                                std_attendence_date_list.Insert(i, std_att.attendence_date);

                                break;
                            }
                            else
                            {
                                if (std_attendence_date_list.Count <= i)
                                {
                                    std_attendence_list.Insert(i, '-');
                                    std_attendence_date_list.Insert(i, std_att.attendence_date);
                                }
                            }
                        }
                    }


                    std_vm.att_lst = std_attendence_list;
                    std_vm.att_date_lst = std_attendence_date_list;
                    std_vm_list.Add(std_vm);
                    break;
                }
            }
        }


        //---------------           Get All Classes    ----------------------------------
        public void get_all_classes()
        {

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            };
                            classes_list.Add(classes);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------         Get All Sections   ------------------------
        public void get_all_sections(string id)
        {
            sections_list = new List<sections>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects where class_id = " + id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),


                            };
                            sections_list.Add(section);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("DB not connected");
                    }

                }
            }
        }

        //============      Classes Selection Change       ===============================
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;
            std_vm_list1 = new ObservableCollection<student_attendence>();

            if (class_cmb.SelectedIndex != 0)
            {
                get_all_sections(id);
                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;

                foreach (student_attendence std in std_vm_list.Where(x => x.class_id == id))
                {
                    std_vm_list1.Add(std);
                }
                svm = new StudentViewModel(std_vm_list1, group_by_att_dates);
                attendence_grid.DataContext = svm;
            }
            else
            {
                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;

                svm = new StudentViewModel(std_vm_list, group_by_att_dates);
                attendence_grid.DataContext = svm;


            }
        }

        //-----------      Section selection change     -----------------------------------
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sections sec = (sections)section_cmb.SelectedItem;
            std_vm_list1 = new ObservableCollection<student_attendence>();
            if (sec != null)
            {
                foreach (student_attendence std in std_vm_list.Where(x => x.section_id == sec.id))
                {
                    std_vm_list1.Add(std);
                }
                svm = new StudentViewModel(std_vm_list1, group_by_att_dates);
                attendence_grid.DataContext = svm;
            }
            else
            {
                //svm = new StudentViewModel(std_vm_list, group_by_att_dates);
                //attendence_grid.DataContext = svm;
            }
        }


        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            int absents = 0;
            DateTime dt = (DateTime)attendnce_date.SelectedDate;
            admission adm = new admission();
            List<admission> std_nos = new List<admission>();
            string message = "";
            foreach (student_attendence st in std_vm_list.Where(x => x.Active == true))
            {
                //get_all_attendence_month(dt,st.std_id);

                if (st.attendence == 'A')
                {
                    message = "Respected Parents" + Environment.NewLine + "AoA" + Environment.NewLine + st.std_name + " is absent from Institute on " + dt.ToString("dd MMM yyyy");
                    message = message + Environment.NewLine + " Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                    //message = message + " Total " + dt.ToString("MMM") + " Absentees =" + totalAbsents+" , Leave = "+totalLeaves + ". Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                }
                else
                {
                    message = "Respected Parents, AoA, " + st.std_name + " is on leave from Institute on " + dt.ToString("dd MMM yyyy");
                    message = message + Environment.NewLine + " Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                }

                if (general_btn.IsChecked == true)
                {
                    message = message_textbox.Text;
                }


                foreach (admission adm_obj in adm_list.Where(x => x.id == st.std_id))
                {
                    adm_obj.sms_message = message;
                    adm_obj.sms_type = "Attendance Sms";
                    std_nos.Add(adm_obj);
                    break;
                }
            }
            if (std_nos.Count > 0)
            {
                MessageBoxResult mbr = MessageBox.Show("Do You Want To Send    " + std_nos.Count + "  SMS ?", "Send Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (mbr == MessageBoxResult.Yes)
                {
                    isbranded = false;
                    OptionWindow ow = new OptionWindow();
                    ow.ShowDialog();

                    if (isbranded == true)
                    {
                        if (encodedRB.IsChecked == true)
                        {
                            BrandedSmsEngine bse = new BrandedSmsEngine(std_nos, true);
                            bse.Show();
                        }
                        else
                        {
                            BrandedSmsEngine bse = new BrandedSmsEngine(std_nos, false);
                            bse.Show();
                        }
                    }
                    else
                    {
                        if (encodedRB.IsChecked == true)
                        {
                            UploadWindow uw = new UploadWindow(std_nos, true);
                            uw.Show();
                        }
                        else
                        {
                            UploadWindow uw = new UploadWindow(std_nos, false);
                            uw.Show();
                        }
                    }
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            if (default_btn.IsChecked == true)
            {
                general_grid.Visibility = Visibility.Hidden;
                encodedRB.IsChecked = false;
                englishRB.IsChecked = true;
            }
            else
            {
                general_grid.Visibility = Visibility.Visible;
                englishRB.IsChecked = true;
                encodedRB.IsChecked = false;
            }

        }

        private void encodedRB_Checked(object sender, RoutedEventArgs e)
        {
            //if (encodedRB != null && englishRB != null)
            //{
            //    if (default_btn.IsChecked == true)
            //    {
            //        general_grid.Visibility = Visibility.Hidden;
            //        encodedRB.IsChecked = false;
            //        englishRB.IsChecked = true;
            //    }
            //    else
            //    {
            //        general_grid.Visibility = Visibility.Visible;
            //        englishRB.IsChecked = true;
            //        encodedRB.IsChecked = false;
            //    }
            //}
        }

        private void englishRB_Checked(object sender, RoutedEventArgs e)
        {
            //if (encodedRB != null && englishRB != null)
            //{
            //    if (default_btn.IsChecked == true)
            //    {
            //        general_grid.Visibility = Visibility.Hidden;
            //        encodedRB.IsChecked = false;
            //        englishRB.IsChecked = true;
            //    }
            //    else
            //    {
            //        general_grid.Visibility = Visibility.Visible;
            //        englishRB.IsChecked = true;
            //        encodedRB.IsChecked = false;
            //    }
            //}
        }

        private void message_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            count_text.Text = (306 - message_textbox.Text.Length).ToString();
            if (message_textbox.Text.Length <= 160)
            {
                sms_no_tb.Text = "1";
            }
            else
            {
                sms_no_tb.Text = "2";
            }
        }
    }
}
