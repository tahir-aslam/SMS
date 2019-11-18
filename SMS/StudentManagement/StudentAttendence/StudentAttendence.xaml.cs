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
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using SMS.DAL;
using SMS.AdvancedAccountManagement.VoucherEntry;

namespace SMS.StudentManagement.StudentAttendence
{
    public partial class StudentAttendence : Page
    {
        List<student_attendence> attendence_list;
        List<student_attendence> all_attendence_list;
        List<student_attendence> absents_attendence_list;
        List<classes> classes_list;
        List<sections> sections_list;
        ObservableCollection<student_attendence> std_vm_list;
        public List<DateTime> group_by_att_dates;
        List<admission> adm_list;
        StudentViewModel svm;
        string sectionid = "";
        public student_attendence std_obj;
        StudentAttendenceForm stf;
        double total_days = 0;
        double total_abs = 0;
        double total_presents = 0;
        double total_leaves = 0;
        double percentage = 0;
        public static int count = 0;
        public static DateTime dt;

        public StudentAttendence()
        {
            InitializeComponent();
            attendence_list = new List<student_attendence>();
            sections_list = new List<sections>();
            classes_list = new List<classes>();
            get_all_classes();

            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            section_cmb.IsEnabled = false;
            att_button.IsEnabled = false;
            class_cmb.ItemsSource = classes_list;
            attendnce_date.SelectedDate = DateTime.Now;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            student_attendence st = new student_attendence();
            if (null == checkBox) return;

            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                st = (student_attendence)attendence_grid.Items[i];
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
        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as RadioButton;
            attendence_grid.SelectedItem = e.Source;
            student_attendence st = new student_attendence();
            st = (student_attendence)attendence_grid.SelectedItem;
            foreach (var item in std_vm_list)
            {
                if (item.std_id == st.std_id)
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
            student_attendence st = new student_attendence();
            if (null == checkBox) return;

            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                st = (student_attendence)attendence_grid.Items[i];
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
            student_attendence st = new student_attendence();
            st = (student_attendence)attendence_grid.SelectedItem;
            foreach (var item in std_vm_list)
            {
                if (item.std_id == st.std_id)
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
            student_attendence st = new student_attendence();
            if (null == checkBox) return;

            for (int i = 0; i < attendence_grid.Items.Count; i++)
            {
                st = (student_attendence)attendence_grid.Items[i];
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
            student_attendence st = new student_attendence();

            st = (student_attendence)attendence_grid.SelectedItem;
            foreach (var item in std_vm_list)
            {
                if (item.std_id == st.std_id)
                {
                    item.isPresent = false;
                    item.isAbsent = false;
                    item.isLeave = true;
                }
            }
        }

        //============     Get All Admission data                 ===========================================
        //public void get_all_admissions(string id)
        //{
        //    adm_list = new List<admission>();

        //    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
        //    using (MySqlCommand cmd = new MySqlCommand())
        //    {
        //        cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && section_id=" + id + "&&  session_id=" + MainWindow.session.id + " ORDER BY adm_no ASC";
        //        cmd.Connection = con;
        //        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
        //        try
        //        {
        //            con.Open();
        //            MySqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                admission adm = new admission()
        //                {
        //                    id = Convert.ToString(reader["id"].ToString()),
        //                    std_name = Convert.ToString(reader["std_name"].ToString()),
        //                    father_name = Convert.ToString(reader["father_name"].ToString()),
        //                    class_id = Convert.ToString(reader["class_id"].ToString()),
        //                    class_name = Convert.ToString(reader["class_name"].ToString()),
        //                    section_id = Convert.ToString(reader["section_id"].ToString()),
        //                    section_name = Convert.ToString(reader["section_name"].ToString()),
        //                    roll_no = Convert.ToString(reader["roll_no"].ToString()),
        //                    adm_no = Convert.ToString(reader["adm_no"].ToString()),
        //                };
        //                adm_list.Add(adm);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //        }
        //    }
        //}

        //---------------           Get All Attendences    ----------------------------------
        public void get_all_admissions(List<sections> sec_list)
        {
            adm_list = new List<admission>();
            try
            {

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    foreach (var sec in sec_list)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && section_id=" + sec.id + "&&  session_id=" + MainWindow.session.id + "  ORDER BY adm_no_int ASC";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    

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
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //public void get_all_attendence(string id)
        //{
        //    all_attendence_list = new List<student_attendence>();
        //    group_by_att_dates = new List<DateTime>();
        //    try
        //    {
        //        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand())
        //            {
        //                cmd.CommandText = "SELECT* FROM sms_student_attendence  where section_id = @section_id && session_id=" + MainWindow.session.id + " ORDER BY attendence_date DESC";
        //                cmd.Connection = con;
        //                cmd.Parameters.Add("@section_id", MySqlDbType.String).Value = id.ToString();
        //                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
        //                con.Open();
        //                MySqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    student_attendence att = new student_attendence()
        //                    {
        //                        id = Convert.ToString(reader["id"].ToString()),
        //                        std_id = Convert.ToString(reader["std_id"].ToString()),
        //                        std_name = Convert.ToString(reader["std_name"].ToString()),
        //                        class_id = Convert.ToString(reader["class_id"].ToString()),
        //                        section_id = Convert.ToString(reader["section_id"].ToString()),
        //                        roll_no = Convert.ToString(reader["roll_no"].ToString()),
        //                        att_percentage = Convert.ToString(reader["att_percentage"].ToString()),
        //                        total_days = Convert.ToString(reader["total_days"].ToString()),
        //                        total_abs = Convert.ToString(reader["total_abs"].ToString()),
        //                        total_presents = Convert.ToString(reader["total_presents"].ToString()),
        //                        total_leaves = Convert.ToString(reader["total_leaves"].ToString()),
        //                        attendence = Convert.ToChar(reader["attendence"]),
        //                        attendence_date = Convert.ToDateTime(reader["attendence_date"]),
        //                    };
        //                    all_attendence_list.Add(att);

        //                    if (group_by_att_dates.Exists(x => x.Date == att.attendence_date))
        //                    {
        //                    }
        //                    else
        //                    {
        //                        group_by_att_dates.Add(att.attendence_date);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //CAlculate attendence
        public void get_all_attendence(List<sections> sec_list)
        {
            all_attendence_list = new List<student_attendence>();
            group_by_att_dates = new List<DateTime>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    foreach (var sec in sec_list)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "SELECT* FROM sms_student_attendence as st where st.session_id=" + MainWindow.session.id + " && st.std_id IN (select id from sms_admission as adm where adm.section_id = @section_id && adm.is_active='Y' && adm.session_id=" + MainWindow.session.id + ") ORDER BY st.attendence_date DESC";
                            cmd.Connection = con;
                            cmd.Parameters.Add("@section_id", MySqlDbType.String).Value = sec.id.ToString();
                            //cmd.CommandText = "SELECT* FROM sms_student_attendence as st where DATE(st.attendence_date) >= DATE(@date) && st.session_id=" + MainWindow.session.id + "&& st.std_id IN (select id from sms_admission as adm where adm.section_id = @section_id && adm.session_id=" + MainWindow.session.id + ") ORDER BY st.attendence_date DESC";
                            //cmd.Connection = con;
                            //cmd.Parameters.Add("@section_id", MySqlDbType.String).Value = sec.id.ToString();
                            //cmd.Parameters.Add("@date", MySqlDbType.Date).Value = attendnce_date.SelectedDate.Value.AddDays(-10);
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    

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
                            reader.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void calculateAttendence(string id)
        {
            total_days = 0;
            total_abs = 0;
            total_presents = 0;
            total_leaves = 0;
            percentage = 0;

            foreach (student_attendence att in all_attendence_list.Where(x => x.std_id == id))
            {
                if (group_by_att_dates.Exists(x => x.Date == att.attendence_date))
                {
                    if (att.attendence == 'A')
                    {
                        total_abs++;
                        total_days++;
                    }
                    else if (att.attendence == 'P')
                    {
                        total_presents++;
                        total_days++;
                    }
                    else if (att.attendence == 'L')
                    {
                        total_leaves++;
                        total_days++;
                    }
                    else
                    {
                    }
                }
                else
                {
                }
            }
            if (total_days > 0)
            {
                percentage = total_presents / total_days;
                percentage = percentage * 100;
            }
        }

        //-----------------           Populate Student view model list----------------------
        public void populate_std_vm_list()
        {
            List<char> std_attendence_list;
            List<DateTime> std_attendence_date_list;
            std_vm_list = new ObservableCollection<student_attendence>();


            foreach (admission adm in adm_list.OrderBy(x=>x.adm_no_int))
            {
                student_attendence std_vm = new student_attendence()
                {
                    std_id = adm.id,
                    std_name = adm.std_name,
                    father_name = adm.father_name,
                    roll_no = adm.roll_no,
                    roll_no_int= adm.roll_no_int,
                    class_id = adm.class_id,
                    section_id = adm.section_id,
                    adm_no = adm.adm_no,
                    adm_no_int=adm.adm_no_int,
                };
                calculateAttendence(adm.id);
                std_vm.att_percentage = percentage.ToString("0.00");
                std_vm.total_abs = total_abs.ToString();
                std_vm.total_days = total_days.ToString();
                std_vm.total_presents = total_presents.ToString();
                std_vm.total_leaves = total_leaves.ToString();
                std_attendence_list = new List<char>();
                std_attendence_date_list = new List<DateTime>();

                foreach (student_attendence std_att in all_attendence_list.Where(x => x.std_id == adm.id))
                {
                    std_vm.id = std_att.id;

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
                ObservableCollection<student_attendence> attList = new ObservableCollection<student_attendence>();
                foreach (var item in std_vm_list.OrderBy(x=>x.adm_no_int))
                {
                    attList.Add(item);
                }
                std_vm_list = attList;
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
            show_absents();
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {


                get_all_sections(id);


                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
            }
            else
            {

                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;

            }
        }

        //-----------      Section selection change     -----------------------------------
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //sections sec = (sections)section_cmb.SelectedItem;
            //show_absents();
            //if (sec != null)
            //{
            //    string id = sec.id;
            //    sectionid = id;
            //    if (section_cmb.SelectedIndex != 0)
            //    {
            //        attendence_grid.Visibility = Visibility.Visible;
            //        att_button.IsEnabled = true;
            //        img_grid.Visibility = Visibility.Hidden;
            //        get_all_admissions(id);
            //        get_all_attendence(id);
            //        populate_std_vm_list();

            //        svm = new StudentViewModel(std_vm_list, group_by_att_dates);
            //        attendence_grid.DataContext = svm;
            //    }
            //    else
            //    {
            //        // attendence_grid.ItemsSource = null;
            //        attendence_grid.Visibility = Visibility.Hidden;
            //        att_button.IsEnabled = false;
            //        img_grid.Visibility = Visibility.Visible;
            //        //attendnce_date.SelectedDate = DateTime.Now;
            //    }
            //}
        }

        //-----------       Submit attendence button             -------------------------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            att_button.IsEnabled = false;
            attendence_list = new List<student_attendence>();
            if (check_attendence() == false)
            {
                for (int row = 0; row < attendence_grid.Items.Count; row++)
                {
                    total_days = 0;
                    total_abs = 0;
                    total_presents = 0;
                    total_leaves = 0;
                    percentage = 0;

                    student_attendence att = new student_attendence();
                    att = (student_attendence)(attendence_grid.Items[row]);
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
                        else if (c == 'A')
                        {
                            total_abs++;
                        }
                        else if (c == 'L')
                        {
                            total_leaves++;
                        }
                        else
                        {
                            continue;
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

                MessageBoxResult mbr1 = MessageBox.Show("Do you want to submit attendance?  Presents=" + presents + "  Absents=" + absents + "  Leaves=" + leave, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr1 == MessageBoxResult.Yes)
                {
                    if (presents > 0 || absents > 0 || leave > 0)
                    {
                        if (std_vm_list.Count == attendence_list.Count)
                        {
                            if (submit_attendence() == 0)
                            {
                                if (update() > 0)
                                {
                                    MessageBox.Show("Successfully Added", "Successfully Added", MessageBoxButton.OK, MessageBoxImage.Information);
                                    get_all_attendence(sections_list.Where(x => x.isChecked == true).ToList());
                                    get_all_admissions(sections_list.Where(x => x.isChecked == true).ToList());
                                    populate_std_vm_list();

                                    svm = new StudentViewModel(std_vm_list, group_by_att_dates);
                                    attendence_grid.DataContext = svm;
                                    att_button.IsEnabled = true;
                                    attendence_grid.Items.Refresh();
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Please Mark all students attendance", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please mark attendence first", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }


            }
            else
            {
                MessageBox.Show("You Have Already Submitted Attendence for This Section", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                att_button.IsEnabled = true;
            }

        }

        public int update()
        {
            int i = 0;
            try
            {
                foreach (student_attendence st in attendence_list)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_student_attendence SET att_percentage=@att_percentage,total_days=@total_days,total_abs=@total_abs,total_presents=@total_presents,total_leaves=@total_leaves WHERE std_id = @id && session_id=" + MainWindow.session.id;
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st.std_id;
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

        // ----------    check if already submitted attendence           ---------------------------
        public bool check_attendence()
        {
            foreach (sections s in sections_list.Where(x => x.isChecked == true))
            {
                // sections s = (sections)section_cmb.SelectedItem;
                foreach (student_attendence std in all_attendence_list)
                {
                    if (std.section_id == s.id && std.attendence_date == attendnce_date.SelectedDate.Value)
                    {
                        return true;
                    }
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
                    student_attendence sa = new student_attendence();
                    sa = attendence_list[j];
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_student_attendence(total_presents,total_abs,total_days,att_percentage,std_id,section_id,attendence_date,class_id,roll_no,attendence,std_name,created_by,date_time,session_id,total_leaves)Values(@total_presents,@total_abs,@total_days,@att_percentage,@std_id,@section_id,@attendence_date,@class_id,@roll_no,@attendence,@std_name,@created_by,@date_time,@session_id,@total_leaves)";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.class_id;
                            cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.section_id;
                            cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.std_id;
                            cmd.Parameters.Add("@std_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.std_name;
                            cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.roll_no;
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
            sections s = (sections)section_cmb.SelectedItem;

            foreach (student_attendence st in all_attendence_list)
            {
                if (st.section_id == s.id)
                {

                }
            }
        }

        private void att_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            std_obj = new student_attendence();
            std_obj = (student_attendence)attendence_grid.SelectedItem;
            if (std_obj != null)
            {

                //MessageBox.Show(std_obj.std_name);
                stf = new StudentAttendenceForm(this);
                stf.ShowDialog();
                get_all_attendence(sections_list.Where(x => x.isChecked == true).ToList());
                populate_std_vm_list();

                svm = new StudentViewModel(std_vm_list, group_by_att_dates);
                attendence_grid.DataContext = svm;
                att_button.IsEnabled = true;
            }
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            if (search_cmb.SelectedIndex == 0)
            {
                string v_search = SearchTextBox.Text;
                attendence_grid.ItemsSource = std_vm_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
                attendence_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 1)
            {
                string v_search = SearchTextBox.Text;
                attendence_grid.ItemsSource = std_vm_list.Where(x => x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
                attendence_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 2)
            {
                string v_search = SearchTextBox.Text;
                attendence_grid.ItemsSource = std_vm_list.Where(x => x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
                attendence_grid.Items.Refresh();
            }
            else
            {
            }
            SearchTextBox.Focus();
        }

        // ---------------           Get All Absents    ----------------------------------
        public void get_all_absentsList(DateTime dt)
        {
            count = 0;
            absents_attendence_list = new List<student_attendence>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_student_attendence  where (attendence= 'A' || attendence= 'L')  && attendence_date = @date && session_id=" + MainWindow.session.id;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;
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
                            absents_attendence_list.Add(att);
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public int get_all_absents(DateTime dt)
        {
            int count = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM sms_student_attendence  where attendence= 'A' && attendence_date = @date && session_id=" + MainWindow.session.id;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        count = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return count;
        }
        public void show_absents()
        {
            if (attendnce_date.SelectedDate != null)
            {
                dt = attendnce_date.SelectedDate.Value;

                count_absents.Text = get_all_absents(dt).ToString();
                date_absents.Text = dt.ToString("dd-MMM-yy");
                get_all_admissions();
                get_all_absentsList(dt);
                if (absents_attendence_list.Count > 0)
                {
                    absents_grid.Visibility = Visibility.Visible;
                    foreach (student_attendence sa in absents_attendence_list)
                    {
                        foreach (admission adm in adm_list.Where(x => x.id == sa.std_id))
                        {
                            sa.class_name = adm.class_name;
                            sa.section_name = adm.section_name;
                            sa.adm_no = adm.adm_no;
                            sa.cell_no = adm.cell_no;
                            sa.father_name = adm.father_name;
                            break;
                        }
                    }
                    absents_datagrid.ItemsSource = absents_attendence_list;
                }
                else
                {
                    absents_grid.Visibility = Visibility.Hidden;
                }
            }
        }

        private void attendnce_date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            show_absents();
        }

        //============     Get All Admission data                 ===========================================
        public void get_all_admissions()
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where session_id=" + MainWindow.session.id+" Order By adm_no_int ASC";
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
                            adm_no_int = Convert.ToInt32(reader["adm_no_int"]),
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

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 40, 150, 150, 50, 80, 65, 65, 30, 30, 60 };
            var ht = new SMS.PrintHeaderTemplates.AbsentsStudentsHeader();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }
        private DataTable CreateSampleDataTable()
        {
            int i = 0;
            var dataTable = new DataTable();

            AddColumn(dataTable, "Sr#", typeof(string));
            AddColumn(dataTable, "Student Name", typeof(string));
            AddColumn(dataTable, "Father Name", typeof(string));
            AddColumn(dataTable, "Adm#", typeof(string));
            AddColumn(dataTable, "Cell#", typeof(string));
            AddColumn(dataTable, "Class", typeof(string));
            AddColumn(dataTable, "Section", typeof(string));
            AddColumn(dataTable, "T.A", typeof(string));
            AddColumn(dataTable, "T.P", typeof(string));
            AddColumn(dataTable, "%", typeof(string));

            foreach (student_attendence c in absents_attendence_list)
            {
                i++;
                var dataRow = dataTable.NewRow();
                dataRow[0] = i.ToString();
                dataRow[1] = c.std_name.ToString();
                dataRow[2] = c.father_name.ToString();
                dataRow[3] = c.adm_no.ToString();
                dataRow[4] = c.cell_no.ToString();
                dataRow[5] = c.class_name.ToString();
                dataRow[6] = c.section_name.ToString();
                dataRow[7] = c.total_abs.ToString();
                dataRow[8] = c.total_presents.ToString();
                dataRow[9] = c.att_percentage.ToString();

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            std_obj = new student_attendence();
            std_obj = (student_attendence)attendence_grid.SelectedItem;
            if (std_obj != null)
            {

                StudentAttendenceInsert sai = new StudentAttendenceInsert(std_obj, sectionid);
                sai.ShowDialog();
                int cl = class_cmb.SelectedIndex;
                int sec = section_cmb.SelectedIndex;
                class_cmb.SelectedIndex = 0;
                class_cmb.SelectedIndex = cl;
                section_cmb.SelectedIndex = sec;
            }

        }
        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Do you want to delete all attendance of " + attendnce_date.SelectedDate.Value.ToString("dd-MMM-yy"), "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                sections sec = (sections)section_cmb.SelectedItem;
                DateTime dt = attendnce_date.SelectedDate.Value;
                foreach (student_attendence item in attendence_grid.Items)
                {
                    deleteAttendence(item.std_id);
                }
                //foreach (admission adm in adm_list.Where(x => x.section_id == sec.id))
                //{
                //    deleteAttendence(adm);
                //}
                MessageBox.Show("Successfully Deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                delete_btn.IsEnabled = true;
                get_all_attendence(sections_list.Where(x => x.isChecked == true).ToList());
                get_all_admissions(sections_list.Where(x => x.isChecked == true).ToList());
                populate_std_vm_list();

                svm = new StudentViewModel(std_vm_list, group_by_att_dates);
                attendence_grid.DataContext = svm;

            }
        }

        public void deleteAttendence(string std_id)
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_student_attendence where std_id=" + std_id + " && attendence_date= @dt";
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

        private void CheckBox_Checked_Sections(object sender, RoutedEventArgs e)
        {
            sections s = sender as sections;
            //sections sec = (sections)section_cmb.SelectedItem;
            //show_absents();

            //if (sections_list[0].isChecked == true)
            //{
            //    foreach (var item in sections_list)
            //    {
            //        item.isChecked = true;
            //    }
            //    section_cmb.Items.Refresh();
            //}           

            attendence_grid.Visibility = Visibility.Visible;
            att_button.IsEnabled = true;
            img_grid.Visibility = Visibility.Hidden;
            get_all_admissions(sections_list.Where(x => x.isChecked == true).ToList());
            get_all_attendence(sections_list.Where(x => x.isChecked == true).ToList());
            populate_std_vm_list();

            svm = new StudentViewModel(std_vm_list, group_by_att_dates);
            attendence_grid.DataContext = svm;

            if (attendence_grid.Items.Count > 0)
            {
                search_sp.Visibility = Visibility.Visible;
            }
            else
            {
                search_sp.Visibility = Visibility.Hidden;
            }


            //attendence_grid.Visibility = Visibility.Hidden;
            //att_button.IsEnabled = false;
            //img_grid.Visibility = Visibility.Visible;


        }

        private void print_btn_new_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AttendanceReportWindow window = new AttendanceReportWindow(absents_attendence_list);
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void all_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}


