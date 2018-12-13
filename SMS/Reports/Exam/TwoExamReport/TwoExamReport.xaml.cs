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
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using SMS.ViewModels;
using System.IO;
using SUT.PrintEngine.Utils;

namespace SMS.Reports.Exam.TwoExamReport
{
    /// <summary>
    /// Interaction logic for TwoExamReport.xaml
    /// </summary>
    public partial class TwoExamReport : Window
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        List<subjects> subjects_list;
        List<subjects> subjects_list_new;
        bool check = false;
        bool isFail = false;
        List<exam> exam_list_new;
        List<exam_data_entry> ede_list;
        List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        exam_data_entry ede_total;
        exam_data_entry ede_percent;
        ExamReportViewModel ervm;
        exam_data_entry ede_temp;
        List<sms_exam_requirements> exam_req_list;
        exam_data_entry ede_remarks;
        exam_data_entry ede_grade;
        exam_data_entry ede_position;
        string position = "";
        string total_days = "_______";
        string total_presents = "_______";
        string total_absents = "_______";
        string att_percentage = "_______";
        public TwoExamReport()
        {
            InitializeComponent();

            get_all_exams();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            get_exam_requirements();
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
                sections s = (sections)section_cmb.SelectedItem;
                classes c = (classes)class_cmb.SelectedItem;
                if (Convert.ToInt32(s.id) > 0)
                {
                    get_all_subjects(c.id);
                    exam_list_new = new List<exam>();
                    exam_result_card_grid.Visibility = Visibility.Hidden;
                    exam_img_grid.Visibility = Visibility.Hidden;
                    exam_std_grid.Visibility = Visibility.Hidden;
                    exam_grid.Visibility = Visibility.Visible;
                    foreach (exam exa in exam_list)
                    {
                        if (check_exams(s.id, exa.id))
                        {
                            exam_list_new.Add(exa);
                        }
                    }
                    exam_Datagrid.ItemsSource = exam_list_new;
                }
            }
            else
            {
                exam_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;
                exam_std_grid.Visibility = Visibility.Hidden;
                subjects_grid.Visibility = Visibility.Hidden;
                exam_result_card_grid.Visibility = Visibility.Hidden;
                print_btn.Visibility = Visibility.Hidden;
            }
        }

        // examination check box
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in exam_list_new)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            exam_Datagrid.Items.Refresh();
        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            exam_Datagrid.SelectedItem = e.Source;
            exam exa = new exam();
            exa = (exam)exam_Datagrid.SelectedItem;
            foreach (exam ex in exam_list_new)
            {
                if (ex.id == exa.id)
                {
                    ex.Checked = checkBox.IsChecked.Value;
                }
            }

        }

        // subjects check box
        private void CheckBox_Checked_subjects(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in subjects_list_new)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            subjects_Datagrid.Items.Refresh();
        }
        private void CheckBox_Checked_sub_subjects(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            subjects_Datagrid.SelectedItem = e.Source;
            subjects sub = new subjects();
            sub = (subjects)subjects_Datagrid.SelectedItem;
            foreach (subjects s in subjects_list_new)
            {
                if (sub.id == s.id)
                {
                    s.Checked = checkBox.IsChecked.Value;
                }
            }
        }

        // students check box
        private void CheckBox_Checked_std(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in adm_list)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            exam_std_Datagrid.Items.Refresh();
        }
        private void CheckBox_Checked_sub_std(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            exam_std_Datagrid.SelectedItem = e.Source;
            admission adm = new admission();
            adm = (admission)exam_std_Datagrid.SelectedItem;
            foreach (admission ad in adm_list)
            {
                if (adm.id == ad.id)
                {
                    ad.Checked = checkBox.IsChecked.Value;
                }
            }
        }

        //-----------              Get All Subjects     ----------------------
        public void get_all_subjects(string id)
        {

            subjects_list = new List<subjects>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_subject where class_id =" + id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            subjects subject = new subjects()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                subject_name = Convert.ToString(reader["subject_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                is_Active = Convert.ToString(reader["is_Active"].ToString()),
                                total_marks = Convert.ToString(reader["total_marks"].ToString()),
                                remarks = Convert.ToString(reader["remarks"].ToString()),


                            };
                            subjects_list.Add(subject);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Subjects DB not connected");
                    }

                }
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
                cmd.CommandText = "SELECT * FROM sms_admission where is_active='Y' && section_id=" + id + "&& session_id=" + MainWindow.session.id;
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
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
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

        //check exams in exam data entry
        public bool check_exams(string sec_id, string exam_id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_exam_data_entry where section_id=@section_id && exam_id=@exam_id && session_id=" + MainWindow.session.id;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = sec_id;
                        cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = exam_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        //check subjects
        public bool check_subjects(string sec_id, string subj_id, string exam_id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_exam_data_entry where section_id=@section_id && subject_id=@subject_id && exam_id=@exam_id && session_id=" + MainWindow.session.id;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = sec_id;
                        cmd.Parameters.Add("@subject_id", MySqlDbType.VarChar).Value = subj_id;
                        cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = exam_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        //exams button click
        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            check = false;
            subjects_list_new = new List<subjects>();
            sections s = (sections)section_cmb.SelectedItem;
            foreach (exam exa in exam_list_new.Where(x => x.Checked == true))
            {
                check = true;
            }

            if (check)
            {
                exam_std_grid.Visibility = Visibility.Hidden;
                exam_grid.Visibility = Visibility.Hidden;
                subjects_grid.Visibility = Visibility.Visible;


                foreach (subjects sub in subjects_list)
                {
                    foreach (exam exa in exam_list_new.Where(x => x.Checked == true))
                    {
                        if (check_subjects(s.id, sub.id, exa.id))
                        {
                            subjects_list_new.Add(sub);
                            break;
                        }
                    }
                }
                subjects_Datagrid.ItemsSource = subjects_list_new;
            }
            else
            {
                MessageBox.Show("Please Select Exams To Create Exam Report", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //subject button click
        private void subjects_btn_Click(object sender, RoutedEventArgs e)
        {
            check = false;
            sections s = (sections)section_cmb.SelectedItem;
            foreach (subjects sub in subjects_list_new.Where(x => x.Checked == true))
            {
                check = true;
            }

            if (check)
            {
                exam_std_grid.Visibility = Visibility.Visible;
                exam_grid.Visibility = Visibility.Hidden;
                subjects_grid.Visibility = Visibility.Hidden;
                get_all_admissions(s.id);
                exam_std_Datagrid.ItemsSource = adm_list;
            }
            else
            {
                MessageBox.Show("Please Select Subjects To Create Exam Report", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //proceed to Exam Report
        private void create_report_btn_Click(object sender, RoutedEventArgs e)
        {
            check = false;
            foreach (admission adm in adm_list.Where(x => x.Checked == true))
            {
                check = true;
            }

            if (check)
            {
                exam_std_grid.Visibility = Visibility.Hidden;
                exam_grid.Visibility = Visibility.Hidden;
                subjects_grid.Visibility = Visibility.Hidden;

                create_report();
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student To Create Exam Report", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        //Create report
        public void create_report()
        {
            get_all_exams_entry();
            set_exam_data_entry_list();
            set_result_Card_list();

            ervm = new ExamReportViewModel(ede_exam_list, exam_list_new);
            Result_grid_lstbox.DataContext = ervm;
            //Result_grid_lstbox.ItemsSource = ede_exam_list;

            exam_std_grid.Visibility = Visibility.Hidden;
            exam_result_card_grid.Visibility = Visibility.Visible;
            print_btn.Visibility = Visibility.Visible;
        }

        // For Getting Exam data entry 
        public void get_all_exams_entry()
        {
            ede_list = new List<exam_data_entry>();
            sections s = (sections)section_cmb.SelectedItem;
            exam ex = exam_list[0];

            //foreach(exam exa in exam_list)
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam_data_entry where section_id=@section_id && session_id=" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = s.id;
                        cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ex.id;
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
        public void set_exam_data_entry_list()
        {
            exam_data_entry edeTemp;
            string temp;
            string exam_name = "Exam Report";
            ede_exam_list = new List<exam_data_entry>();
            foreach (admission adm in adm_list.Where(x => x.Checked == true))
            {
                foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == adm.id))
                {
                    ede_obj = new exam_data_entry();
                    ede_obj = ede;
                    isFail = false;

                    ede_obj.institute_logo = MainWindow.ins.institute_logo;
                    ede_obj.institute_name = MainWindow.ins.institute_name;
                    ede_obj.date = DateTime.Now.ToString("dd-MMM-yyyy");
                    ede_obj.class_name = adm.class_name + "-" + adm.section_name;
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
                    if (MainWindow.examAdminPanel.attendance_text_visibility == "Y")
                    {
                        get_all_attendence(ede.std_id); // get all attendecne
                    }
                    ede_obj.total_days = total_days;
                    ede_obj.total_presents = total_presents;
                    ede_obj.total_absents = total_absents;
                    ede_obj.att_percentage = att_percentage;

                    ede_obj.subj_list = new List<exam_data_entry>();
                    foreach (subjects subject in subjects_list_new.Where(x => x.Checked == true))
                    {
                        exam_data_entry ede_s_o = new exam_data_entry();
                        ede_s_o.subject_name = subject.subject_name;

                        // inner subject list according to exams
                        ede_s_o.subj_list = new List<exam_data_entry>();
                        foreach (exam ex_inner in exam_list_new.Where(x => x.Checked == true))
                        {
                            check = false;
                            foreach (exam_data_entry ede_s_o_i in ede_list.Where(x => x.std_id == adm.id))
                            {
                                if (subject.id == ede_s_o_i.subject_id && ex_inner.id == ede_s_o_i.exam_id)
                                {
                                    if (ede_s_o_i.subject_obtained == "-")
                                    {
                                        ede_s_o_i.subject_total = "-";
                                    }
                                    edeTemp = new exam_data_entry();
                                    edeTemp.subject_obtained = ede_s_o_i.subject_obtained + "/" + ede_s_o_i.subject_total;
                                    ede_s_o.subj_list.Add(edeTemp);
                                    check = true;
                                    break;
                                }
                            }
                            if (check == false)
                            {
                                ede_temp = new exam_data_entry();
                                ede_temp.subject_obtained = "";
                                ede_temp.subject_total = "";
                                ede_s_o.subj_list.Add(ede_temp);
                            }
                        }
                        ede_obj.subj_list.Add(ede_s_o);
                    }
                }
                ede_obj.subj_list.Add(get_total_marks(ede_obj.std_id));
                ede_obj.subj_list.Add(ede_percent);
                ede_obj.subj_list.Add(ede_grade);
                ede_obj.subj_list.Add(ede_position);
                ede_obj.subj_list.Add(ede_remarks);
                ede_exam_list.Add(ede_obj);

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
        public exam_data_entry get_total_marks(string std_id)
        {
            double total_obt = 0;
            double total = 0;
            double percentage = 0;
            exam_data_entry ede_obj;
            bool isFail = false;

            ede_percent = new exam_data_entry();
            ede_percent.subj_list = new List<exam_data_entry>();
            ede_percent.subject_name = "PERCENTAGE";

            ede_total = new exam_data_entry();
            ede_total.subject_name = "TOTAL MARKS";
            ede_total.subj_list = new List<exam_data_entry>();

            ede_remarks = new exam_data_entry();
            ede_remarks.subject_name = "REMARKS";
            ede_remarks.subj_list = new List<exam_data_entry>();

            ede_grade = new exam_data_entry();
            ede_grade.subject_name = "GRADE";
            ede_grade.subj_list = new List<exam_data_entry>();

            ede_position = new exam_data_entry();
            ede_position.subject_name = "POSITION";
            ede_position.subj_list = new List<exam_data_entry>();

            foreach (exam exa in exam_list_new.Where(x => x.Checked == true))
            {
                total = 0;
                total_obt = 0;
                percentage = 0;
                isFail = false;

                //total
                foreach (subjects subj in subjects_list_new.Where(x => x.Checked == true))
                {
                    foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == std_id))
                    {
                        if (ede.subject_id == subj.id && exa.id == ede.exam_id)
                        {
                            if (ede.subject_obtained != "" || ede.subject_obtained != null || !string.IsNullOrWhiteSpace(ede.subject_obtained) || !string.IsNullOrEmpty(ede.subject_obtained))
                            {
                                if (ede.subject_obtained == "A")
                                {
                                    total = total + Convert.ToDouble(ede.subject_total);
                                    isFail = true;
                                }
                                else
                                {
                                    try
                                    {
                                        total_obt = total_obt + Convert.ToDouble(ede.subject_obtained);
                                        total = total + Convert.ToDouble(ede.subject_total);
                                        if (!isFail)
                                        {
                                            isFail = isFailSubject(ede.subject_total, ede.subject_obtained);
                                        }
                                    }
                                    catch (Exception ex) { total = 0; }
                                }
                            }
                        }
                    }
                }

                // total marks
                ede_obj = new exam_data_entry();
                ede_obj.subject_obtained = total_obt.ToString() + "/" + total.ToString();
                //ede_obj.subject_total = total.ToString();                
                ede_total.subj_list.Add(ede_obj);

                // percentage
                ede_obj = new exam_data_entry();
                if (total != 0)
                {
                    percentage = total_obt / total;
                    percentage = percentage * 100;
                }
                ede_obj.subject_obtained = percentage.ToString("0.00") + "%";
                ede_obj.subject_total = "%";
                ede_percent.percentage = percentage.ToString("0.00");
                ede_percent.subj_list.Add(ede_obj);

                // remarks  grade
                foreach (sms_exam_requirements ser in exam_req_list)
                {
                    if (percentage >= ser.lower_percentage && percentage <= ser.upper_percentage)
                    {
                        ede_obj = new exam_data_entry();
                        ede_obj.subject_obtained = ser.remarks;
                        ede_obj.subject_total = "";
                        ede_remarks.subj_list.Add(ede_obj);

                        ede_obj = new exam_data_entry();
                        ede_obj.subject_obtained = ser.grade;
                        ede_obj.subject_total = "";
                        ede_grade.subj_list.Add(ede_obj);
                        break;
                    }
                }

                // position
                foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == std_id).Where(y => y.exam_id == exa.id))
                {
                    ede_obj = new exam_data_entry();
                    if (percentage >= 40 && isFail == false)
                    {
                        ede_obj.subject_obtained = ede.position;
                    }
                    else
                    {
                        ede_obj.subject_obtained = "-";
                    }

                    ede_obj.subject_total = "";
                    ede_position.subj_list.Add(ede_obj);
                    break;
                }

            }
            return ede_total;
        }

        //printing
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var visualSize = new Size(Result_grid_lstbox.ActualWidth, Result_grid_lstbox.ActualHeight);
            var printControl = PrintControlFactory.Create(visualSize, Result_grid_lstbox);

            printControl.ShowPrintPreview();
        }

        public bool isFailSubject(string tota, string obtained)
        {
            try
            {
                double total = Convert.ToDouble(tota);
                double obtain = Convert.ToDouble(obtained);
                double percentage = 0;
                if (total != 0)
                {
                    percentage = obtain / total;
                    percentage = percentage * 100;

                    // remarks  grade
                    foreach (sms_exam_requirements ser in exam_req_list)
                    {
                        if (percentage >= ser.lower_percentage && percentage <= ser.upper_percentage)
                        {
                            if (ser.grade == "F")
                            {
                                return true;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return false;
        }

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
                        cmd.CommandText = "select count(*) as total, attendence from sms_student_attendence where session_id=" + MainWindow.session.id + " && std_id=@std_id group by attendence";
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
    }
}
