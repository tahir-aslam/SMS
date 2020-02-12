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
using SMS.DAL;

namespace SMS.ExamsManagement.ExamDataEntry
{
    /// <summary>
    /// Interaction logic for ExamDataEntryPage.xaml
    /// </summary>
    public partial class ExamDataEntryPage : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        ObservableCollection<sms_exams_subjects> subjects_list;
        List<admission> adm_list;
        List<exam_data_entry> ede_list;
        List<exam_data_entry> ede_class_list;
        public static List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        exam_data_entry ede_obj_total;
        sms_exams_subjects subj_obj;
        ExamViewModel evm;
        List<exam_data_entry> submit_exam_entry_list;
        List<sms_exam_requirements> exam_req_list;
        SubjectsDAL subjectsDAL;
        AdmissionDAL admDAL;

        double subject_percentage = 0;
        double subject_total = 0;
        double subject_obtained = 0;
        double subject_tmp = 0;

        double percentage = 0;
        double total_marks = 0;
        double obtained_marks = 0;
        bool absent = false;
        bool leave = false;
        public ExamDataEntryPage()
        {
            InitializeComponent();

            subjectsDAL = new SubjectsDAL();
            admDAL = new AdmissionDAL();
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

        // ---------------------- Exam Selection Changed ---------------------------
        private void exam_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (exam_cmb.SelectedIndex > 0)
            {
                class_cmb.IsEnabled = true;
                class_cmb.SelectedIndex = 0;
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
                    save_btn.Visibility = Visibility.Hidden;
                    exam_entry_grid.Visibility = Visibility.Hidden;
                    exam_img_grid.Visibility = Visibility.Hidden;

                    //get_all_admissions(s.id);
                    adm_list = admDAL.getAdmissionsBySectionID(Convert.ToInt32(s.id));
                    if (adm_list.Count > 0)
                    {                        
                            subjects_list = new ObservableCollection<sms_exams_subjects>();
                            subjectsDAL.GetAllSubjectsAssignmentOfSection(Convert.ToInt32(s.id)).ForEach(x=>subjects_list.Add(x));
                        get_all_exams_entry();

                        if (ede_list.Count > 0)
                        {
                            create_btn.Visibility = Visibility.Visible;
                            delete_btn.Visibility = Visibility.Visible;
                            subjects_grid.Visibility = Visibility.Hidden;
                            save_btn.Visibility = Visibility.Visible;
                            exam_entry_grid.Visibility = Visibility.Visible;

                            set_exam_data_entry_list();
                            populate_exams_entry_grid();
                            insert_total_marks_row();
                            evm = new ExamViewModel(ede_exam_list);
                            exam_entry_grid.DataContext = evm;
                        }
                        else
                        {
                            delete_btn.Visibility = Visibility.Hidden;
                            subjects_grid.Visibility = Visibility.Visible;
                            exam_entry_grid.Visibility = Visibility.Hidden;
                            populate_subjects();
                        }
                    }
                    else
                    {
                        create_btn.Visibility = Visibility.Collapsed;
                        MessageBox.Show("There are not any student in this section. There should be minimum one student to create exam");
                    }


                }
            }
            else
            {
                delete_btn.Visibility = Visibility.Hidden;
                save_btn.Visibility = Visibility.Hidden;
                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;

                //exam_entry_grid.ItemsSource = null;
                //exam_entry_grid.Items.Refresh();
                //exam_entry_grid.Columns.Clear();

            }

        }

        public void populate_subjects()
        {
            this.subjects_Datagrid.ItemsSource = subjects_list;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in subjects_list)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            subjects_Datagrid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            subjects_Datagrid.SelectedItem = e.Source;
            subj_obj = new sms_exams_subjects();
            subj_obj = (sms_exams_subjects)subjects_Datagrid.SelectedItem;
            foreach (sms_exams_subjects s in subjects_list)
            {
                if (subj_obj.id == s.id)
                {
                    s.Checked = checkBox.IsChecked.Value;
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
                    cmd.CommandText = "SELECT* FROM sms_exam where session_id =" + MainWindow.session.id;
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
               
        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            update_exam_list();
        }

        private void create_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Create This Exam? Please Check Your Selected Subjects Again, Subjects Will Not Change Later.", "Create Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                fill_create_list();
                save_create_list();
                subjects_grid.Visibility = Visibility.Hidden;
                exam_entry_grid.Visibility = Visibility.Visible;

                set_exam_data_entry_list();
                populate_exams_entry_grid();
                save_btn.Visibility = Visibility;
                insert_total_marks_row();
                delete_btn.Visibility = Visibility.Visible;
                evm = new ExamViewModel(ede_exam_list);
                exam_entry_grid.DataContext = evm;
            }
        }

        public void populate_exams_entry_grid()
        {
            //DataGridTextColumn textColumn;
            //textColumn = new DataGridTextColumn();
            //textColumn.Header = "Student Name";
            //textColumn.Binding = new Binding("std_name");
            //textColumn.IsReadOnly = true;
            //textColumn.Width = 200;
            //exam_entry_grid.Columns.Add(textColumn);


            //foreach (exam_data_entry ede in ede_exam_list)
            //{
            //    foreach (exam_data_entry edes in ede.subj_list)
            //    {
            //        DataGridTemplateColumn dgtc = new DataGridTemplateColumn();
            //        DataTemplate dt = new DataTemplate();

            //        textColumn = new DataGridTextColumn();
            //        textColumn.Header = edes.subject_name;
            //        textColumn.Binding = new Binding("subject_obtained");
            //        exam_entry_grid.Columns.Add(textColumn);
            //    }
            //    break;
            //}

            //exam_entry_grid.ItemsSource = null;
            //exam_entry_grid.ItemsSource = ede_exam_list;
            //exam_entry_grid.Items.Refresh();            
        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                exam_data_entry ee;
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                    //for (int i = 0; i < exam_entry_grid.Items.Count ; i++)
                    //{
                    //    try
                    //    {


                    //        if (i != 0)
                    //        {
                    //            ee = new exam_data_entry();
                    //            ee = (exam_data_entry)exam_entry_grid.Items[i];
                    //            delete(ee);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MessageBox.Show(ex.Message);
                    //        break;
                    //    }
                    //}
                    ee = new exam_data_entry();
                    ee = (exam_data_entry)exam_entry_grid.Items[1];
                    delete(ee);
                    MessageBox.Show("Successfully Deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    section_cmb.SelectedIndex = 0;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //-------------     Delete          ---------------------------
        public int delete(exam_data_entry ee)
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_exams_data_entry where exam_id=" + ee.exam_id + " && section_id=" + ee.section_id + "&& session_id =" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);

                    }

                }
            }
            return i;
        }

        public void insert_total_marks_row()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void set_exam_data_entry_list()
        {
            ede_exam_list = new List<exam_data_entry>();
            foreach (admission adm in adm_list.OrderBy(x=>x.adm_no_int))
            {
                foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == adm.id))
                {
                    ede_obj = new exam_data_entry();
                    ede_obj = ede;
                    ede_obj.adm_no = adm.adm_no;
                    ede_obj.father_name = adm.father_name;
                    ede_obj.roll_no = adm.roll_no;
                    ede_obj.subj_list = new List<exam_data_entry>();
                    foreach (exam_data_entry ede_s in ede_list.Where(x => x.std_id == adm.id))
                    {
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


                        ede_obj.subj_list.Add(ede_s_o);
                    }
                    ede_exam_list.Add(ede_obj);

                    break;
                }
            }
            //exam_entry_grid.ItemsSource = null;
            //foreach (exam_data_entry ede in ede_exam_list)
            //{
            //    exam_entry_grid.Items.Add(ede);
            //}
            //exam_entry_grid.ItemsSource = ede_exam_list;
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
                    cmd.CommandText = "SELECT* FROM sms_exams_data_entry where exam_id=@exam_id && section_id=@section_id && session_id=" + MainWindow.session.id;
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

        public void save_create_list()
        {
            try
            {
                foreach (exam_data_entry ede in ede_list)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {

                            cmd.CommandText = "insert into sms_exams_data_entry (class_id,section_id,exam_id,subject_id,std_id,std_name,subject_name,created_by,date_time,session_id)Values(@class_id,@section_id,@exam_id,@subject_id,@std_id,@std_name,@subject_name,@created_by,@date_time,@session_id)";

                            cmd.Parameters.Add("session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd.Parameters.Add("class_id", MySqlDbType.VarChar).Value = ede.class_id;
                            cmd.Parameters.Add("section_id", MySqlDbType.VarChar).Value = ede.section_id;
                            cmd.Parameters.Add("exam_id", MySqlDbType.VarChar).Value = ede.exam_id;
                            cmd.Parameters.Add("subject_id", MySqlDbType.VarChar).Value = ede.subject_id;
                            cmd.Parameters.Add("std_id", MySqlDbType.VarChar).Value = ede.std_id;
                            cmd.Parameters.Add("std_name", MySqlDbType.VarChar).Value = ede.std_name;
                            cmd.Parameters.Add("subject_name", MySqlDbType.VarChar).Value = ede.subject_name;
                            cmd.Parameters.Add("created_by", MySqlDbType.VarChar).Value = ede.created_by;
                            cmd.Parameters.Add("date_time", MySqlDbType.DateTime).Value = ede.date_time;

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void fill_create_list()
        {
            sections s = (sections)section_cmb.SelectedItem;
            classes c = (classes)class_cmb.SelectedItem;
            exam ex = (exam)exam_cmb.SelectedItem;


            ede_list = new List<exam_data_entry>();

            foreach (admission adm in adm_list)
            {
                foreach (sms_exams_subjects subj in subjects_list.Where(x => x.Checked == true))
                {
                    ede_obj = new exam_data_entry();

                    ede_obj.class_id = c.id;
                    ede_obj.section_id = s.id;
                    ede_obj.exam_id = ex.id;
                    ede_obj.std_id = adm.id;
                    ede_obj.std_name = adm.std_name;
                    ede_obj.subject_id = subj.id.ToString();
                    ede_obj.subject_name = subj.subject_name;
                    ede_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                    ede_obj.date_time = DateTime.Now;

                    ede_list.Add(ede_obj);

                }
            }
        }
        //Update Exam Marks
        public void update_exam_list()
        {
            exam_data_entry ee;
            exam_data_entry ee_total = new exam_data_entry();
            submit_exam_entry_list = new List<exam_data_entry>();

            for (int i = 0; i < exam_entry_grid.Items.Count; i++)
            {

                if (i == 0)
                {
                    ee_total = (exam_data_entry)exam_entry_grid.Items[i];
                }
                else
                {
                    ee = new exam_data_entry();
                    ee = (exam_data_entry)exam_entry_grid.Items[i];

                    foreach (exam_data_entry ede_total in ee_total.subj_list)
                    {
                        foreach (exam_data_entry ede_ee in ee.subj_list)
                        {
                            if (ede_total.subject_id == ede_ee.subject_id)
                            {
                                ede_ee.subject_total = ede_total.subject_obtained;
                                ede_ee.date_time = DateTime.Now;
                                ede_ee.created_by = MainWindow.emp_login_obj.emp_user_name;
                                break;
                            }
                        }
                    }

                    submit_exam_entry_list.Add(ee);
                }
            }
            calculate_nos();
            calculate_position();
            save_changes();
            MessageBox.Show("Updated Successfully", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void calculate_nos()
        {
            string abc = "Concentrate On ";
            string fail_subjects = "";
            get_exam_requirements();
            foreach (exam_data_entry ee in submit_exam_entry_list)
            {
                subject_percentage = 0;
                subject_total = 0;
                subject_obtained = 0;
                subject_tmp = 0;

                percentage = 0;
                total_marks = 0;
                obtained_marks = 0;
                absent = true;
                leave = true;
                fail_subjects = "";
                foreach (exam_data_entry ede in ee.subj_list)
                {
                    // Calcaulate Obtained Marks
                    if (String.IsNullOrEmpty(ede.subject_obtained))
                    {
                        ede.subject_grade = "";
                        ede.subject_percentage = "";
                        ede.subject_remarks = "";
                        ede_obj.subject_obtained = "";
                        ede_obj.subject_obtained_int = 0;

                        if (String.IsNullOrEmpty(ede.subject_total) || ede_obj.subject_total == "0")
                        {
                        }
                        else
                        {
                            subject_total = Convert.ToDouble(ede.subject_total);    
                            ede.subject_total_int = Convert.ToInt32(ede.subject_total);
                            total_marks = total_marks + subject_total;
                        }

                    }
                    else if (ede.subject_obtained.ToUpper() == "A")
                    {
                        ede.subject_grade = "-";
                        ede.subject_percentage = "-";
                        ede.subject_remarks = "Absent";
                        ede_obj.subject_obtained = "-";
                        ede_obj.subject_obtained_int = 0;
                        if (String.IsNullOrEmpty(ede.subject_total) || ede_obj.subject_total == "0")
                        {
                        }
                        else
                        {
                            subject_total = Convert.ToDouble(ede.subject_total);
                            ede.subject_total_int = Convert.ToInt32(ede.subject_total);
                            total_marks = total_marks + subject_total;
                        }
                    }
                    else if (ede.subject_obtained.ToUpper() == "L")
                    {
                        ede.subject_grade = "-";
                        ede.subject_percentage = "-";
                        ede.subject_remarks = "Leave";
                        ede_obj.subject_obtained = "-";
                        ede_obj.subject_obtained_int = 0;
                        if (String.IsNullOrEmpty(ede.subject_total) || ede_obj.subject_total == "0")
                        {
                        }
                        else
                        {
                            subject_total = Convert.ToDouble(ede.subject_total);
                            ede.subject_total_int = Convert.ToInt32(ede.subject_total);
                            total_marks = total_marks + subject_total;
                        }
                    }
                    else
                    {
                        absent = false;
                        leave = false;
                        subject_obtained = Convert.ToDouble(ede.subject_obtained);
                        ede.subject_obtained_int = Convert.ToInt32(ede.subject_obtained);
                        obtained_marks = obtained_marks + subject_obtained;

                        if (String.IsNullOrEmpty(ede.subject_total) || ede_obj.subject_total == "0")
                        {                          
                        }
                        else
                        {
                            subject_total = Convert.ToDouble(ede.subject_total);
                            ede.subject_total_int = Convert.ToInt32(ede.subject_total);
                            subject_percentage = subject_obtained / subject_total;
                            subject_percentage = subject_percentage * 100;

                            total_marks = total_marks + subject_total;

                            ede.subject_percentage = subject_percentage.ToString("0.00");
                            //apply grade and remarks using database script

                            foreach (sms_exam_requirements ser in exam_req_list)
                            {
                                if (subject_percentage >= ser.lower_percentage && subject_percentage <= ser.upper_percentage)
                                {
                                    ede.subject_grade = ser.grade;
                                    ede.subject_remarks = ser.remarks;
                                    break;
                                }
                            }
                        }
                    }
                    if (ede.subject_grade == "F")
                    {
                        fail_subjects = fail_subjects + ", " + ede.subject_name;
                    }

                }
                if (absent == true)
                {
                    ee.total_marks = total_marks.ToString();
                    ee.obtained_marks = "Absent";
                    ee.grade = "-";
                    ee.position = "-";
                    ee.percentage = "-";
                    ee.remarks = "Absent";
                }
                else if (leave == true)
                {
                    ee.total_marks = total_marks.ToString();
                    ee.obtained_marks = "Leave";
                    ee.grade = "-";
                    ee.position = "-";
                    ee.percentage = "-";
                    ee.remarks = "Leave";
                }
                else
                {
                    ee.total_marks = total_marks.ToString();
                    ee.obtained_marks = obtained_marks.ToString();

                    if (total_marks > 0)
                    {
                        percentage = obtained_marks / total_marks;
                        percentage = percentage * 100;
                        ee.percentage = percentage.ToString("0.00");
                        //apply grade and remarks

                        foreach (sms_exam_requirements ser in exam_req_list)
                        {
                            if (percentage >= ser.lower_percentage && percentage <= ser.upper_percentage)
                            {
                                ee.grade = ser.grade;
                                ee.remarks = ser.overall_remarks;
                                ee.total_remarks = ser.remarks;
                                break;
                            }
                        }

                    }
                }


            }

            // calculate total marks

            foreach (exam_data_entry ee in submit_exam_entry_list)
            {
                foreach (exam_data_entry ede in ee.subj_list)
                {
                    ede.position = ee.position;
                    ede.percentage = ee.percentage;
                    ede.obtained_marks = ee.obtained_marks;
                    ede.total_marks = ee.total_marks;
                    ede.grade = ee.grade;
                    ede.remarks = ee.remarks;
                    ede.total_remarks = ee.total_remarks;

                    if (ede.subject_grade == "F")
                    {
                        fail_subjects = fail_subjects + ", " + ede.subject_name;
                    }
                }
            }
        }
        public void save_changes()
        {
            foreach (exam_data_entry ede in submit_exam_entry_list)
            {
                foreach (exam_data_entry ede_s in ede.subj_list)
                {

                    try
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Update sms_exams_data_entry SET grade=@grade, position=@position,  remarks=@remarks, obtained_marks=@obtained_marks, percentage=@percentage, total_marks=@total_marks,total_remarks=@total_remarks, subject_percentage=@subject_percentage, subject_grade=@subject_grade, subject_remarks=@subject_remarks, subject_total =@subject_total , subject_obtained =@subject_obtained, subject_total_int =@subject_total_int , subject_obtained_int =@subject_obtained_int where exam_id=@exam_id && section_id=@section_id  && std_id=@std_id && subject_id=@subject_id && session_id=" + MainWindow.session.id;
                                cmd.Connection = con;

                                cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ede_s.exam_id;
                                cmd.Parameters.Add("@std_id", MySqlDbType.VarChar).Value = ede_s.std_id;
                                cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = ede_s.section_id;
                                cmd.Parameters.Add("@subject_id", MySqlDbType.VarChar).Value = ede_s.subject_id;

                                cmd.Parameters.Add("@subject_remarks", MySqlDbType.VarChar).Value = ede_s.subject_remarks;
                                cmd.Parameters.Add("@subject_grade", MySqlDbType.VarChar).Value = ede_s.subject_grade;
                                cmd.Parameters.Add("@subject_percentage", MySqlDbType.VarChar).Value = ede_s.subject_percentage;
                                cmd.Parameters.Add("@subject_total", MySqlDbType.VarChar).Value = ede_s.subject_total;
                                cmd.Parameters.Add("@subject_obtained", MySqlDbType.VarChar).Value = ede_s.subject_obtained;
                                cmd.Parameters.Add("@subject_total_int", MySqlDbType.Int32).Value = ede_s.subject_total_int;
                                cmd.Parameters.Add("@subject_obtained_int", MySqlDbType.Int32).Value = ede_s.subject_obtained_int;

                                cmd.Parameters.Add("@total_marks", MySqlDbType.VarChar).Value = ede_s.total_marks;
                                cmd.Parameters.Add("@total_remarks", MySqlDbType.VarChar).Value = ede_s.total_remarks;
                                cmd.Parameters.Add("@percentage", MySqlDbType.VarChar).Value = ede_s.percentage;
                                cmd.Parameters.Add("@obtained_marks", MySqlDbType.VarChar).Value = ede_s.obtained_marks;
                                cmd.Parameters.Add("@remarks", MySqlDbType.VarChar).Value = ede_s.remarks;
                                cmd.Parameters.Add("@position", MySqlDbType.VarChar).Value = ede.position;
                                cmd.Parameters.Add("@grade", MySqlDbType.VarChar).Value = ede_s.grade;


                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }

                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
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
        public void calculate_position()
        {
            bool isFail = false;
            exam_data_entry edeObj = new exam_data_entry();
            int i = 1;
            foreach (exam_data_entry ede in submit_exam_entry_list.OrderByDescending(x =>
            {
                try
                {
                    return Convert.ToDouble(x.percentage);
                }
                catch (Exception ex) { return 0.00; }
            }))
            {
                isFail = false;
                foreach (exam_data_entry ede_s in ede.subj_list)
                {
                    if (ede_s.subject_grade == "F" || ede_s.subject_obtained == "A" || ede_s.subject_obtained == "L")
                    {
                        isFail = true;
                        break;
                    }
                }

                if (isFail == false)
                {
                    if (i <= Convert.ToInt32(MainWindow.examAdminPanel.position_limit) && Convert.ToInt32(MainWindow.examAdminPanel.position_limit) != 0)
                    {
                        if (i > 1 && ede.percentage == edeObj.percentage)
                        {
                            ede.position = edeObj.position;
                        }
                        else
                        {
                            ede.position = returnPosition(i);
                            i++;
                        }

                        edeObj = ede;
                    }
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
