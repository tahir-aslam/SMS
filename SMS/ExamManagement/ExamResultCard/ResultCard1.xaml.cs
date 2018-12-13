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
using SMS.ViewModels;
using SUT.PrintEngine.Utils;
using System.Windows.Markup;
using System.Data;
using System.IO;

namespace SMS.ExamManagement.ExamResultCard
{
    /// <summary>
    /// Interaction logic for ResultCard1.xaml
    /// </summary>
    public partial class ResultCard1 : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        ObservableCollection<exam_data_entry> ede_exam_list_checked;
        List<admission> adm_list;
        List<exam_data_entry> ede_list;
        public static List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        ExamViewModel evm;
        string exam_name = "";
        exam_data_entry ede_total;
        string total_days =     "_______";
        string total_presents = "_______";
        string total_absents =  "_______";
        string att_percentage = "_______";

        public ResultCard1()
        {
            InitializeComponent();

            get_all_exams();
            exam_cmb.Focus();
            exam_cmb.ItemsSource = exam_list;
            exam_list.Insert(0, new exam() { exam_name = "---Select Exam---", id = "-1" });
            exam_cmb.SelectedIndex = 0;

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            ede_exam_list = new List<exam_data_entry>();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in ede_exam_list_checked)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            exam_entry_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            exam_entry_grid.SelectedItem = e.Source;
            ede_obj = new exam_data_entry();
            ede_obj = (exam_data_entry)exam_entry_grid.SelectedItem;
            foreach (exam_data_entry s in ede_exam_list_checked)
            {
                if (ede_obj.id == s.id)
                {
                    s.Checked = checkBox.IsChecked.Value;
                }
            }

        }

        // ---------------------- Exam Selection Changed ---------------------------
        private void exam_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exam_cmb.SelectedIndex > 0)
            {
                class_cmb.IsEnabled = true;
                class_cmb.SelectedIndex = 0;
                exam exa =  (exam)exam_cmb.SelectedItem;
                exam_name = exa.exam_name;
            }
            else
            {
                class_cmb.IsEnabled = false;
                class_cmb.SelectedIndex = 0;
            }

        }

        //------------------------  Class Selection Changed -------------------------
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {
                section_cmb.IsEnabled = true;
                get_all_sections(id);
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

        // ----------------------  Sections Selection changed---------------------------
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedIndex > 0)
            {
                //exam_entry_grid.ItemsSource = null;
                //exam_entry_grid.Items.Refresh();
                //exam_entry_grid.Columns.Clear();

                sections s = (sections)section_cmb.SelectedItem;
                classes c = (classes)class_cmb.SelectedItem;
                if (Convert.ToInt32(s.id) > 0)
                {
                    exam_entry_grid.Visibility = Visibility.Hidden;
                    exam_img_grid.Visibility = Visibility.Hidden;

                    get_all_admissions(s.id);                    
                    get_all_exams_entry();

                    if (ede_list.Count > 0)
                    {
                        exam_result_card_grid.Visibility = Visibility.Hidden;
                        exam_img_grid.Visibility = Visibility.Hidden;
                        exam_entry_grid.Visibility = Visibility.Visible;
                        exam_entry_grid_g.Visibility = Visibility.Visible;
                        create_btn.Visibility = Visibility.Visible;
                        print_btn.Visibility = Visibility.Hidden;

                        set_exam_data_entry_list();

                        ede_exam_list_checked = new ObservableCollection<exam_data_entry>();
                        foreach (exam_data_entry ede in ede_exam_list) 
                        {
                            ede_exam_list_checked.Add(ede);
                        }

                        evm = new ExamViewModel(ede_exam_list_checked);
                        exam_entry_grid.DataContext = evm;

                    }
                    else
                    {
                        exam_entry_grid.Visibility = Visibility.Hidden;
                        exam_img_grid.Visibility = Visibility.Visible;
                        print_btn.Visibility = Visibility.Hidden;                        
                        create_btn.Visibility = Visibility.Hidden;
                        exam_result_card_grid.Visibility = Visibility.Hidden;
                        print_btn.Visibility = Visibility.Hidden;
                    }
                }
            }
            else
            {

                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;
                print_btn.Visibility = Visibility.Hidden;
                create_btn.Visibility = Visibility.Hidden;
                exam_result_card_grid.Visibility = Visibility.Hidden;
                print_btn.Visibility = Visibility.Hidden;
                //exam_entry_grid.ItemsSource = null;
                //exam_entry_grid.Items.Refresh();
                //exam_entry_grid.Columns.Clear();

            }

        }

        //-----------              Get All Exam     ----------------------
        public void get_all_exams()
        {
            exam_list = new List<exam>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam where session_id="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            exam ex = new exam()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                exam_name = Convert.ToString(reader["exam_name"].ToString()),
                                exam_date = Convert.ToDateTime(reader["exam_date"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            exam_list.Add(ex);

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

        //---------------           Get All Classes    ----------------------------------
        public void get_all_classes()
        {
            classes_list = new List<classes>();
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

        //------------              Get All Sections   ------------------------
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
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }
        
        // ===============     Get All Admissions          ================
        public void get_all_admissions(string id)
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM sms_admission where is_active='Y' && section_id=" + id + "&& session_id=" + MainWindow.session.id + " ORDER BY adm_no ASC";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    Byte[] img;
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["image"] == "")
                        {
                            string path = "/SMS;component/images/Delete-icon.png";
                            img = File.ReadAllBytes(path);
                        }
                        else
                        {
                            img = (byte[])(reader["image"]);
                        }
                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            image = img,

                        };
                        adm_list.Add(adm);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
       
        public void set_exam_data_entry_list()
        {
            bool isFail = false;
            ede_exam_list = new List<exam_data_entry>();
            foreach (admission adm in adm_list)
            {               
                foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == adm.id))
                {
                    ede_obj = new exam_data_entry();
                    ede_obj = ede;
                    isFail = false;

                    ede_obj.institute_logo = MainWindow.ins.institute_logo;
                    ede_obj.institute_name = MainWindow.ins.institute_name;
                    ede_obj.date = DateTime.Now.ToString("dd-MMM-yyyy");
                    ede_obj.class_name = adm.class_name+"-"+adm.section_name;
                    ede_obj.section_name = adm.section_name;
                    ede_obj.roll_no = adm.roll_no;
                    if (adm.cell_no.Length == 10)
                    {
                        ede_obj.cell_no = "0" + adm.cell_no.Insert(3, "-");
                    }
                    else 
                    {
                        ede_obj.cell_no = "";
                    }
                    
                    ede_obj.comm_adress = adm.comm_adress;
                    ede_obj.adm_no = adm.adm_no;
                    ede_obj.std_img = adm.image;
                    ede_obj.father_name = adm.father_name;
                    ede_obj.exam_name = exam_name;
                    ede_obj.std_id = adm.id;
                    if(MainWindow.examAdminPanel.attendance_text_visibility == "Y")
                    {
                        get_all_attendence(ede.std_id); // get all attendecne
                    }                    
                    ede_obj.total_days = total_days;
                    ede_obj.total_presents = total_presents;
                    ede_obj.total_absents = total_absents;
                    ede_obj.att_percentage = att_percentage;

                    ede_obj.teacher_sig_text = MainWindow.examAdminPanel.teacher_sig_text;
                    ede_obj.principal_sig_text = MainWindow.examAdminPanel.principal_sig_text;
                    ede_obj.parents_sig_text = MainWindow.examAdminPanel.parents_sig_text;
                    

                    ede_obj.subj_list = new List<exam_data_entry>();
                    foreach (exam_data_entry ede_s in ede_list.Where(x => x.std_id == adm.id))
                    {
                        if (MainWindow.examAdminPanel.remarks_text_visibility == "Y")
                        {
                            ede_obj.remarks = ede_s.remarks;
                        }
                        else
                        {
                            ede_obj.remarks = "___________________________________________________________________________________________________________";
                        }
                        
                        exam_data_entry ede_s_o = new exam_data_entry();
                        // ede_s_o = ede_obj;
                        ede_s_o.class_id = ede_s.class_id;
                        ede_s_o.id = ede_s.id;
                        ede_s_o.section_id = ede_s.section_id;
                        ede_s_o.exam_id = ede_s.exam_id;
                        ede_s_o.std_id = ede_s.std_id;
                        ede_s_o.std_name = ede_s.std_name;

                        ede_s_o.subject_id = ede_s.subject_id;
                        ede_s_o.subject_name = ede_s.subject_name;
                        ede_s_o.subject_obtained = ede_s.subject_obtained;
                        ede_s_o.subject_total = ede_s.subject_total;
                        ede_s_o.subject_percentage = ede_s.subject_percentage;
                        ede_s_o.subject_remarks = ede_s.subject_remarks;
                        ede_s_o.subject_grade = ede_s.subject_grade; 
                        if(!isFail)
                        {
                            if (ede_s.subject_grade == "F" || ede_s.subject_obtained == "A") 
                            {
                                isFail = true;
                            }
                        }

                        ede_obj.subj_list.Add(ede_s_o);

                        ede_total = new exam_data_entry();
                        ede_total.subject_name = "Total Marks";
                        ede_total.subject_obtained = ede_s.obtained_marks;
                        ede_total.subject_total = ede_s.total_marks;
                        ede_total.subject_percentage = ede_s.percentage;
                        ede_total.subject_grade = ede_s.grade;
                        ede_total.subject_remarks = ede_s.total_remarks;
                        
                        
                        try
                        {
                            if (Convert.ToDouble(ede_total.subject_percentage) < Convert.ToDouble(MainWindow.examAdminPanel.position_percentage) || MainWindow.examAdminPanel.position_text_visibility== "N")
                            {
                                isFail = true;
                            }
                        }
                        catch (Exception ex) { }
                    }
                    if(isFail)
                    {
                        ede_obj.position = "          ";
                    }
                    ede_obj.subj_list.Add(ede_total);
                    ede_exam_list.Add(ede_obj);

                    break;
                }
            }            
        }
        public void get_all_exams_entry()
        {
            ede_list = new List<exam_data_entry>();

            sections s = (sections)section_cmb.SelectedItem;
            classes c = (classes)class_cmb.SelectedItem;
            exam ex = (exam)exam_cmb.SelectedItem;

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam_data_entry where exam_id=@exam_id && section_id=@section_id && session_id="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ex.id;
                        cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = s.id;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            exam_data_entry ede = new exam_data_entry()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                exam_id = Convert.ToString(reader["exam_id"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                section_id = Convert.ToString(reader["section_id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                std_name = Convert.ToString(reader["std_name"].ToString()),

                                subject_id = Convert.ToString(reader["subject_id"].ToString()),
                                subject_name = Convert.ToString(reader["subject_name"].ToString()),
                                subject_total = Convert.ToString(reader["subject_total"].ToString()),
                                subject_obtained = Convert.ToString(reader["subject_obtained"].ToString()),
                                subject_percentage = Convert.ToString(reader["subject_percentage"].ToString()),
                                subject_grade = Convert.ToString(reader["subject_grade"].ToString()),
                                subject_remarks = Convert.ToString(reader["subject_remarks"].ToString()),

                                total_marks = Convert.ToString(reader["total_marks"].ToString()),
                                obtained_marks = Convert.ToString(reader["obtained_marks"].ToString()),
                                grade = Convert.ToString(reader["grade"].ToString()),
                                remarks = Convert.ToString(reader["remarks"].ToString()),
                                percentage = Convert.ToString(reader["percentage"].ToString()),
                                position = Convert.ToString(reader["position"].ToString()),
                                total_remarks = Convert.ToString(reader["total_remarks"].ToString()),
                            };
                            ede_list.Add(ede);
                        }
                        con.Close();
                    }
                    catch (Exception exs)
                    {
                        MessageBox.Show(exs.Message);
                    }

                }
            }


        }
        

        private void create_btn_Click(object sender, RoutedEventArgs e)
        {
            ede_exam_list = new List<exam_data_entry>();
            foreach (exam_data_entry ee in ede_exam_list_checked) 
            {
                if(ee.Checked == true)
                {
                    ede_exam_list.Add(ee);
                }
            }

            if (ede_exam_list.Count < 1)
            {
                MessageBox.Show("Please Select Minimum One Record", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else 
            {
                create_btn.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Hidden;
                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_result_card_grid.Visibility = Visibility.Visible;
                exam_entry_grid_g.Visibility = Visibility.Hidden;
                create_btn.Visibility = Visibility.Hidden;
                print_btn.Visibility = Visibility.Visible;

                //set_result_Card_list();
                Result_grid_lstbox.ItemsSource = ede_exam_list;
                
            }
        }
        public void set_result_Card_list() 
        {
            foreach (exam_data_entry ede in ede_exam_list) 
            {
                ede.institute_logo = MainWindow.ins.institute_logo;
                ede.institute_name = MainWindow.ins.institute_name;
                ede.date = DateTime.Now.ToString("dd-MMM-yyyy");

                foreach (admission adm in adm_list.Where(x=>x.id == ede.std_id)) 
                {
                    ede.class_name = adm.class_name;
                    ede.section_name = adm.section_name;
                    ede.roll_no = adm.roll_no;
                    ede.adm_no = adm.adm_no;
                    ede.std_img = adm.image;                    
                }                
            }
        }

        // Print result card
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var visualSize = new Size(Result_grid_lstbox.ActualWidth, Result_grid_lstbox.ActualHeight);
            var printControl = PrintControlFactory.Create(visualSize, Result_grid_lstbox);
            printControl.ShowPrintPreview();
        }
        public void insert_total_marks_row()
        {
            exam_data_entry ee_subj;
            exam_data_entry ee = new exam_data_entry();
            exam_data_entry e2 = new exam_data_entry();
            e2 = ede_exam_list[0];

            ee.id = "0";
            ee.std_id = "0";
            ee.std_name = "**Total Marks**";
            ee.section_id = e2.section_id;
            ee.exam_id = e2.exam_id;
            ee.obtained_marks = e2.total_marks;
            ee.subj_list = new List<exam_data_entry>();
            foreach (exam_data_entry ede in e2.subj_list)
            {
                ee_subj = new exam_data_entry();

                ee_subj.subject_obtained = ede.subject_total;
                ee_subj.subject_id = ede.subject_id;
                ee_subj.subject_name = ede.subject_name;
                ee_subj.section_id = ede.section_id;
                ee_subj.exam_id = ede.exam_id;
                ee_subj.id = "0";
                ee_subj.obtained_marks = ede.total_marks;
                ee.subj_list.Add(ee_subj);
            }
            ede_exam_list.Insert(0, ee);
        }
        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence(string id)
        {
            double days = 0;
            double absents = 0;
            double presents = 0;
            double leaves = 0;
            double percentage = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select count(*) as total, attendence from sms_student_attendence where session_id="+MainWindow.session.id+" && std_id=@std_id group by attendence";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@std_id", MySqlDbType.String).Value = id.ToString();
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {                            
                            if (reader["attendence"].ToString() == "P")
                            {
                                presents = Convert.ToDouble(reader["total"]);
                            }
                            else if (reader["attendence"].ToString() == "A")
                            {
                                absents = Convert.ToDouble(reader["total"]);
                            }
                            else if (reader["attendence"].ToString() == "L")
                            {
                                leaves = Convert.ToDouble(reader["total"]);
                            }                            
                        }
                        days = presents + absents + leaves;
                    }
                }

                if (days > 0)
                {
                    percentage = presents / days;
                    percentage = percentage * 100;
                }
                att_percentage = percentage.ToString("0.00");
                total_days = days.ToString();
                total_absents = absents.ToString();
                total_presents = presents.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //public void get_all_attendence(string id)
        //{
        //    double days = 0;
        //    double absents = 0;
        //    double presents = 0;
        //    double percentage = 0;
        //    try
        //    {
        //        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand())
        //            {
        //                cmd.CommandText = "SELECT* FROM sms_student_attendence  where std_id =@std_id && session_id=" + MainWindow.session.id + " ORDER BY attendence_date DESC";
        //                cmd.Connection = con;
        //                cmd.Parameters.Add("@std_id", MySqlDbType.String).Value = id.ToString();
        //                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
        //                con.Open();
        //                MySqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    days++;
        //                    if (reader["attendence"].ToString() == "P")
        //                    {
        //                        presents++;
        //                    }
        //                    else 
        //                    {
        //                        absents++;
        //                    }
        //                    //att_percentage = Convert.ToString(reader["att_percentage"].ToString());
        //                    //total_days = Convert.ToString(reader["total_days"].ToString());
        //                    //total_absents = Convert.ToString(reader["total_abs"].ToString());
        //                    //total_presents = Convert.ToString(reader["total_presents"].ToString());
        //                    //break;
        //                }
        //            }
        //        }

        //        if (days > 0)
        //        {
        //            percentage = presents / days ;
        //            percentage = percentage * 100;
        //        }
        //        att_percentage = percentage.ToString("0.00");
        //        total_days = days.ToString();
        //        total_absents = absents.ToString();
        //        total_presents = presents.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
    }
}
