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
    /// Interaction logic for ResultCard2.xaml
    /// </summary>
    public partial class ResultCard2 : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        ObservableCollection<exam_data_entry> ede_exam_list_checked;
        List<admission> adm_list;
        List<exam_data_entry> ede_list;
        List<exam_data_entry> ede_class_list;
        List<exam_data_entry> ede_class_positions_list;
        List<exam_data_entry> ede_new_list;
        public static List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        ExamViewModel evm;
        string exam_name = "";
        exam_data_entry ede_total;
        List<sms_exam_requirements> exam_req_list;
        string grade = "";
        string remarks = "";
        string overall_remarks = "";


        double total_days;
        double total_presents;
        double total_absents;
        double att_percentage;

        public ResultCard2()
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
                exam exa = (exam)exam_cmb.SelectedItem;
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
                    cmd.CommandText = "SELECT* FROM sms_exam where session_id=" + MainWindow.session.id;
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
                cmd.CommandText = "SELECT * FROM sms_admission where is_active='Y' && section_id=" + id + "&& session_id=" + MainWindow.session.id+ " ORDER BY adm_no ASC";
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
                    isFail = false;
                    ede_obj = new exam_data_entry();
                    ede_obj = ede;

                    ede_obj.institute_logo = MainWindow.ins.institute_logo;
                    ede_obj.institute_name = MainWindow.ins.institute_name;
                    ede_obj.date = DateTime.Now.ToString("dd-MMM-yyyy");
                    ede_obj.class_name = adm.class_name;
                    ede_obj.section_name = adm.section_name;
                    ede_obj.roll_no = adm.roll_no;
                    ede_obj.adm_no = adm.adm_no;
                    ede_obj.std_img = adm.image;
                    ede_obj.father_name = adm.father_name;
                    ede_obj.exam_name = exam_name;

                    ede_obj.subj_list = new List<exam_data_entry>();
                    foreach (exam_data_entry ede_s in ede_list.Where(x => x.std_id == adm.id))
                    {
                        ede_obj.remarks = ede_s.remarks;
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
                        if (!isFail)
                        {
                            if (ede_s.subject_grade == "F" || ede_s.subject_obtained == "A")
                            {
                                isFail = true;
                            }
                        }
                        ede_obj.subj_list.Add(ede_s_o);
                        
                        //ede_total = new exam_data_entry();
                        //ede_total.subject_name = "Total Marks";
                        //ede_total.subject_obtained = ede_s.obtained_marks;
                        //ede_total.subject_total = ede_s.total_marks;
                        //ede_total.subject_percentage = ede_s.percentage;
                        //ede_total.subject_grade = ede_s.grade;
                        //ede_total.subject_remarks = ede_s.total_remarks;
                    }
                    //ede_obj.subj_list.Add(ede_total);
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
                    cmd.CommandText = "SELECT* FROM sms_exam_data_entry where exam_id=@exam_id && section_id=@section_id && session_id=" + MainWindow.session.id;
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
                if (ee.Checked == true)
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
                set_oral();
                remove_oral();
                insert_total_marks();
                //set_result_Card_list();
                Result_grid_lstbox.ItemsSource = ede_new_list;
            }
        }
        public void set_result_Card_list()
        {
            foreach (exam_data_entry ede in ede_exam_list)
            {
                ede.institute_logo = MainWindow.ins.institute_logo;
                ede.institute_name = MainWindow.ins.institute_name;
                ede.date = DateTime.Now.ToString("dd-MMM-yyyy");

                foreach (admission adm in adm_list.Where(x => x.id == ede.std_id))
                {
                    ede.class_name = adm.class_name;
                    ede.section_name = adm.section_name;
                    ede.roll_no = adm.roll_no;
                    ede.adm_no = adm.adm_no;
                    ede.std_img = adm.image;
                }
            }
        }

        public void set_oral() 
        {
            get_exam_requirements();
            bool check = false;            
            string subj_name;
            string subj1_name;
            ede_new_list = new List<exam_data_entry>();
            double subject_obtained = 0;
            double oral_obtained = 0;
            double max_obtained = 0;
            double max_total = 0;
            double percentage = 0;

            foreach (exam_data_entry ede in ede_exam_list) 
            {                
                foreach (exam_data_entry ede_subj in ede.subj_list) 
                {
                    subj_name = ede_subj.subject_name+"(O)";
                    subj1_name = ede_subj.subject_name + "(P)";
                    check = false;
                    subject_obtained = 0;
                    oral_obtained = 0;
                    max_obtained = 0;
                    max_total = 0;

                    foreach (exam_data_entry ede_check in ede.subj_list) 
                    {
                        if (ede_check.subject_name.ToUpper() == subj_name.ToUpper() || ede_check.subject_name.ToUpper() == subj1_name.ToUpper())
                        {
                            if (ede_subj.subject_total != "" || ede_subj.subject_obtained != "" || ede_check.subject_obtained!="")
                            {
                                check = true;
                                if (ede_subj.subject_obtained == "A")
                                {
                                    subject_obtained = 0;
                                }
                                else 
                                {
                                    subject_obtained = Convert.ToDouble(ede_subj.subject_obtained);
                                }

                                if (ede_check.subject_obtained == "A")
                                {
                                    oral_obtained = 0;
                                }
                                else 
                                {
                                    oral_obtained = Convert.ToDouble(ede_check.subject_obtained);
                                }
                                

                                ede_subj.max_marks = (Convert.ToInt32(ede_subj.subject_total) + Convert.ToInt32(ede_check.subject_total)).ToString();
                                ede_subj.oral_obtained = ede_check.subject_obtained;
                                ede_subj.max_obtained = (subject_obtained + oral_obtained).ToString();
                                percentage = Convert.ToDouble(ede_subj.max_obtained) / Convert.ToDouble(ede_subj.max_marks);
                                percentage = percentage * 100;
                                ede_subj.subject_percentage = percentage.ToString("0.00");
                                get_grade(percentage);
                                ede_subj.subject_grade = grade;
                                ede_subj.subject_remarks = remarks;
                                break;
                            }
                        }
                    }
                    if(check==false)
                    {                       
                        if (ede_subj.subject_total != "" && ede_subj.subject_obtained != "")
                        {
                            if (ede_subj.subject_obtained == "A")
                            {
                                subject_obtained = 0;
                            }
                            else 
                            {
                                subject_obtained = Convert.ToDouble(ede_subj.subject_obtained);
                            }

                            ede_subj.max_marks = ede_subj.subject_total;
                            ede_subj.max_obtained = subject_obtained.ToString();
                            percentage = Convert.ToDouble(ede_subj.max_obtained) / Convert.ToDouble(ede_subj.max_marks);
                            percentage = percentage * 100;
                            ede_subj.subject_percentage = percentage.ToString("0.00");
                            get_grade(percentage);
                            ede_subj.subject_grade = grade;
                            ede_subj.subject_remarks = remarks;
                        }                        
                    }

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
        public void get_exam_requirements()
        {
            exam_req_list = new List<sms_exam_requirements>();           

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM sms_exam_requirements";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        sms_exam_requirements ser = new sms_exam_requirements()
                        {
                            lower_percentage = Convert.ToDouble(reader["lower_percentage"]),
                            upper_percentage = Convert.ToDouble(reader["upper_percentage"]),
                            remarks = Convert.ToString(reader["remarks"].ToString()),
                            overall_remarks = Convert.ToString(reader["overall_remarks"].ToString()),
                            grade = Convert.ToString(reader["grade"].ToString()),
                            fail_subjects = Convert.ToInt32(reader["fail_subjects"]),
                        };
                        exam_req_list.Add(ser);
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        public void remove_oral() 
        {
            ede_new_list = new List<exam_data_entry>();
            exam_data_entry ede_obj;            

            foreach (exam_data_entry ede in ede_exam_list) 
            {
                ede_obj = new exam_data_entry();
                
                ede_obj.Checked = ede.Checked;
                ede_obj.class_id = ede.class_id;                
                ede_obj.date = ede.date;
                ede_obj.exam_id = ede.exam_id;
                ede_obj.exam_name = ede.exam_name;                
                ede_obj.institute_logo = ede.institute_logo;
                ede_obj.institute_name = ede.institute_name;
                ede_obj.std_img = ede.std_img;
                ede_obj.std_name = ede.std_name;
                ede_obj.father_name = ede.father_name;
                ede_obj.section_name = ede.section_name;
                ede_obj.class_name = ede.class_name;
                ede_obj.section_name = ede.section_name;
                ede_obj.adm_no = ede.adm_no;
                ede_obj.std_id = ede.std_id;
                ede_obj.position = ede.position;
                
                

                ede_obj.subj_list = new List<exam_data_entry>();

                foreach (exam_data_entry ede_subj in ede.subj_list) 
                {
                    if (ede_subj.subject_name.Contains("(P)") || ede_subj.subject_name.Contains("(O)"))
                    {
                    }
                    else 
                    {
                        ede_obj.subj_list.Add(ede_subj);
                    }
                }
                ede_new_list.Add(ede_obj);
            }
        }
        public void get_grade(double percentage) 
        {
            foreach (sms_exam_requirements ser in exam_req_list)
            {
                if (percentage >= ser.lower_percentage && percentage <= ser.upper_percentage)
                {
                    grade = ser.grade;
                    remarks = ser.remarks;
                    overall_remarks = ser.overall_remarks;
                }
            }
        }
        public void insert_total_marks()
        {
            sections s = (sections)section_cmb.SelectedItem;
            calculate_class_position();
            int section_strength = ede_class_list.Where(y=>y.section_id == s.id).Select(x => x.std_id).Distinct().Count();
            int class_strength = ede_class_list.Select(x => x.std_id).Distinct().Count();
            try
            {
                bool isFail = false;
                double total_marks = 0;
                double total_theroy = 0;
                double total_oral = 0;
                double total_obtained = 0;
                double percentage = 0;


                foreach (exam_data_entry ede in ede_new_list)
                {
                    isFail = false;
                    total_marks = 0;
                    total_theroy = 0;
                    total_oral = 0;
                    total_obtained = 0;
                    percentage = 0;

                    get_all_attendence(ede.std_id);
                    ede.total_days = total_days.ToString();
                    ede.total_presents = total_presents.ToString();
                    ede.total_absents = total_absents.ToString();
                    ede.att_percentage = att_percentage.ToString("0.0");

                    foreach (exam_data_entry ede_subj in ede.subj_list)
                    {
                        if (ede_subj.subject_obtained != "" || !string.IsNullOrEmpty(ede_subj.subject_obtained) || !string.IsNullOrWhiteSpace(ede_subj.subject_obtained) || ede_subj.oral_obtained != "" || ede_subj.max_obtained != "" || ede_subj.max_marks != "")
                        {
                            if (ede_subj.subject_obtained == "A")
                            {
                                isFail = true;
                            }
                            else
                            {
                                total_theroy = total_theroy + Convert.ToDouble(ede_subj.subject_obtained);
                            }

                            if (ede_subj.oral_obtained == "A")
                            {
                                isFail = true;
                            }
                            else
                            {
                                total_oral = total_oral + Convert.ToDouble(ede_subj.oral_obtained);
                            }

                            total_marks = total_marks + Convert.ToDouble(ede_subj.max_marks);
                            total_obtained = total_obtained + Convert.ToDouble(ede_subj.max_obtained);
                            if(ede_subj.subject_grade == "F")
                            {
                                isFail=true;
                            }
                        }
                    }
                    percentage = total_obtained / total_marks;
                    percentage = percentage * 100;
                    get_grade(percentage);

                    ede_total = new exam_data_entry();
                    ede_total.subject_name = "Total Marks";
                    ede_total.max_marks = total_marks.ToString();
                    ede_total.subject_obtained = total_theroy.ToString();
                    ede_total.oral_obtained = total_oral.ToString();
                    ede_total.max_obtained = total_obtained.ToString();
                    ede_total.subject_percentage = percentage.ToString("0.00");
                    ede_total.subject_grade = grade;
                    ede_total.subject_remarks = remarks;
                    ede.subj_list.Add(ede_total);

                    get_grade(Convert.ToDouble(ede_total.subject_percentage));
                    ede.remarks = overall_remarks;
                    if (isFail == false && percentage >= 40)
                    {
                        ede.position = ede.position + "  out of  " + section_strength.ToString();
                        ede.class_position = ede.class_position + "  out of  " + class_strength.ToString();
                    }
                    else 
                    {
                        ede.position        = "____________";
                        ede.class_position  = "____________";
                    }
                }
            }
            catch (Exception ex) 
            {

            }

        }

        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence(string id)
        {
            total_days = 0;
            total_presents = 0;
            total_absents = 0;
            att_percentage = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_student_attendence  where std_id =@std_id && session_id=" + MainWindow.session.id + "&& attendence_date >= @sessionStartDate ORDER BY attendence_date DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@std_id", MySqlDbType.String).Value = id.ToString();
                        cmd.Parameters.Add("@sessionStartDate", MySqlDbType.Date).Value = MainWindow.session.session_start;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            student_attendence att = new student_attendence()
                            {                                
                                attendence = Convert.ToChar(reader["attendence"]),                             
                            };

                            if (att.attendence == 'A') 
                            {
                                total_absents++;
                                total_days++;
                            }
                            else if (att.attendence == 'P')
                            {
                                total_presents++;
                                total_days++;
                            }
                            else 
                            {
                            }                            
                        }
                        if(total_days > 0)
                        {
                            att_percentage = total_presents / total_days;
                            att_percentage = att_percentage * 100;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //for class position
        public void get_all_class_exams_entry()
        {
            ede_class_list = new List<exam_data_entry>();

            sections s = (sections)section_cmb.SelectedItem;
            classes c = (classes)class_cmb.SelectedItem;
            exam ex = (exam)exam_cmb.SelectedItem;

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam_data_entry where exam_id=@exam_id && class_id=@class_id && session_id=" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ex.id;
                        cmd.Parameters.Add("@class_id", MySqlDbType.VarChar).Value = c.id;

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
                            ede_class_list.Add(ede);
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

        public void calculate_class_position() 
        {
            get_all_class_exams_entry();

            ede_class_positions_list = new List<exam_data_entry>();
            List<exam_data_entry> new_list = new List<exam_data_entry>();
            exam_data_entry edeObj = new exam_data_entry();
            int i = 1;
            int count = 0;

            foreach (var id in ede_class_list.Select(x => x.std_id).Distinct())
            {
                foreach (exam_data_entry ede in ede_class_list.Where(y=>y.std_id == id).Where(z => z.percentage != "-").Where(x => x.percentage != ""))
                {
                    new_list.Add(ede);
                    break;
                }
            }

            var lst = new_list.OrderByDescending(x =>
                {
                    try
                    {
                        return Convert.ToDouble(x.percentage);
                    }
                    catch (Exception ex)
                    {
                        return 0.00;
                    }
                }).ToList();

            foreach (exam_data_entry ede in lst)
                {
                    count++;
                    if (i > 1 && ede.percentage == edeObj.percentage)
                    {
                        ede.class_position = edeObj.class_position;
                    }
                    else
                    {
                        ede.class_position = returnPosition(i);
                        i++;
                    }

                    edeObj = ede;
                    ede_class_positions_list.Add(ede);                    
                }
            

            //embed class position in 

            foreach(exam_data_entry ede_section in ede_new_list)
            {
                foreach(exam_data_entry ede_class in ede_class_positions_list.Where(x=>x.std_id == ede_section.std_id))
                {
                    ede_section.class_position = ede_class.class_position;
                }
            }
        }
        public string returnPosition(int i)
        {
            if (i == 1)
            {
                return i + "st";
            }
            if (i == 2)
            {
                return i + "nd";
            }
            if (i == 3)
            {
                return i + "rd";
            }
            else
            {
                return i + "th";
            }
        }
    }
}
