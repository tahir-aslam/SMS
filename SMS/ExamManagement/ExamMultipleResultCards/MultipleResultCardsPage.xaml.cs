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
using System.IO;
using SUT.PrintEngine.Utils;

namespace SMS.ExamManagement.ExamMultipleResultCards
{
    /// <summary>
    /// Interaction logic for MultipleResultCardsPage.xaml
    /// </summary>
    public partial class MultipleResultCardsPage : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        List<subjects> subjects_list;
        List<subjects> subjects_list_new;
        bool check = false;
        List<exam> exam_list_new;
        List<exam_data_entry> ede_list;
        List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        exam_data_entry ede_total;
        exam_data_entry ede_percent;
        ExamReportViewModel ervm;
        exam_data_entry ede_temp;

        public MultipleResultCardsPage()
        {
            InitializeComponent();
            get_all_exams();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
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
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
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
                exam_std_grid.Visibility = Visibility.Visible;
                exam_grid.Visibility = Visibility.Hidden;                
                get_all_admissions(s.id);
                exam_std_Datagrid.ItemsSource = adm_list;
            }
            else
            {
                MessageBox.Show("Please Select Exams To Create Exam Report", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
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
            ede_exam_list = new List<exam_data_entry>();
            foreach (admission adm in adm_list)
            {
                foreach(exam examObj in exam_list_new.Where(x=>x.Checked== true))
                {
                    foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == adm.id).Where(y=>y.exam_id == examObj.id))
                    {
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
                        ede_obj.exam_name = examObj.exam_name;
                        ede_obj.std_id = adm.id;
                        //get_all_attendence(ede.std_id);
                        //ede_obj.total_days = total_days;
                        //ede_obj.total_presents = total_presents;
                        //ede_obj.total_absents = total_absents;
                        //ede_obj.att_percentage = att_percentage;

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

                            ede_obj.subj_list.Add(ede_s_o);

                            ede_total = new exam_data_entry();
                            ede_total.subject_name = "Total Marks";
                            ede_total.subject_obtained = ede_s.obtained_marks;
                            ede_total.subject_total = ede_s.total_marks;
                            ede_total.subject_percentage = ede_s.percentage;
                            ede_total.subject_grade = ede_s.grade;
                            ede_total.subject_remarks = ede_s.total_remarks;
                        }
                        ede_obj.subj_list.Add(ede_total);
                        ede_exam_list.Add(ede_obj);

                        break;
                    }
                }
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

        public exam_data_entry get_total_marks(string std_id)
        {
            double total_obt = 0;
            double total = 0;
            double percentage = 0;
            exam_data_entry ede_obj;

            ede_percent = new exam_data_entry();
            ede_percent.subj_list = new List<exam_data_entry>();
            ede_percent.subject_name = "Percentage";

            ede_total = new exam_data_entry();
            ede_total.subject_name = "Total Marks";
            ede_total.subj_list = new List<exam_data_entry>();


            foreach (exam exa in exam_list_new.Where(x => x.Checked == true))
            {
                total = 0;
                total_obt = 0;
                percentage = 0;
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
                                }
                                else
                                {
                                    try
                                    {
                                        total_obt = total_obt + Convert.ToDouble(ede.subject_obtained);
                                        total = total + Convert.ToDouble(ede.subject_total);
                                    }
                                    catch (Exception ex) { total = 0; }
                                }
                            }
                        }
                    }
                }
                ede_obj = new exam_data_entry();

                ede_obj.subject_obtained = total_obt.ToString();
                ede_obj.subject_total = total.ToString();
                ede_total.subj_list.Add(ede_obj);


                ede_obj = new exam_data_entry();
                if (total != 0)
                {
                    percentage = total_obt / total;
                    percentage = percentage * 100;
                }


                ede_obj.subject_obtained = percentage.ToString("0.00");
                ede_obj.subject_total = "%";
                ede_percent.subj_list.Add(ede_obj);
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
    }
}
