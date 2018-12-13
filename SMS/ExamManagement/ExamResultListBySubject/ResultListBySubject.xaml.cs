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
using System.Collections.ObjectModel;
using SMS.ViewModels;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SMS.PrintHeaderTemplates;
using System.Data;
using SUT.PrintEngine.Utils;

namespace SMS.ExamManagement.ExamResultListBySubject
{
    /// <summary>
    /// Interaction logic for ResultListBySubject.xaml
    /// </summary>
    public partial class ResultListBySubject : Page
    {
        List<exam> exam_list;
        List<exam> exam_list_new;
        List<classes> classes_list;
        List<sections> sections_list;
        List<subjects> subjects_list;
        List<admission> adm_list;
        List<exam_data_entry> ede_list;
        List<exam_data_entry> ede_list_report;
        public static List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        ExamViewModel evm;
        List<double> col_width;
        public static string class_name = "";
        public static string section_name = "";
        public static string exam_name = "";

        public sections sec;
        public classes cl;
        public subjects subj;

        public ResultListBySubject()
        {
            InitializeComponent();         

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;            
            
        }        

        //------------------------  Class Selection Changed -------------------------
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            class_name = c.class_name;
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
                section_name = s.section_name;
                classes c = (classes)class_cmb.SelectedItem;
                if (Convert.ToInt32(s.id) > 0)
                {
                    exam_entry_grid.Visibility = Visibility.Hidden;
                    exam_img_grid.Visibility = Visibility.Hidden;
                    subjects_cmb.IsEnabled = true;

                    get_all_admissions(s.id);
                    get_all_subjects(c.id);
                    subjects_cmb.SelectedIndex = 0;
                    subjects_list.Insert(0, new subjects() { subject_name = "---Select Subject---", id = "-1" });
                    subjects_cmb.ItemsSource = subjects_list;
                }
                else 
                {
                    subjects_cmb.IsEnabled = false;
                }
            }
            else
            {

                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;
                print_btn.Visibility = Visibility.Hidden;
                subjects_cmb.IsEnabled = false;
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
                    cmd.CommandText = "SELECT* FROM sms_exam where session_id=" + MainWindow.session.id + " ORDER BY exam_date ASC";
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

        // examination check box
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in exam_list)
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
            foreach (exam ex in exam_list)
            {
                if (ex.id == exa.id)
                {
                    ex.Checked = checkBox.IsChecked.Value;
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
                cmd.CommandText = "SELECT id,std_name,father_name,section_id,adm_no FROM sms_admission where is_active='Y' && section_id=" + id + "&& session_id=" + MainWindow.session.id + " ORDER BY roll_no ASC";
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
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
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

        public void insert_total_marks_row()
        {
            exam_data_entry ee_subj;
            exam_data_entry ee = new exam_data_entry();
            exam_data_entry e2 = new exam_data_entry();
            e2.subj_list = new List<exam_data_entry>();
            if(ede_list_report.Count > 0)
            {
                e2 = ede_list_report[0];
            }
            

            ee.id = "0";
            ee.std_id = "0";
            ee.std_name = "**Total Marks**";
            ee.adm_no = "-";
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
            ede_list_report.Insert(1, ee);
        }
        public void insert_exam_date_row()
        {
            exam_data_entry ee_subj;
            exam_data_entry ee = new exam_data_entry();
            exam_data_entry e2 = new exam_data_entry();
            e2.subj_list = new List<exam_data_entry>();
            if (ede_list_report.Count > 0)
            {
                e2 = ede_list_report[0];
            }

            ee.id = "0";
            ee.std_id = "0";
            ee.std_name = "**Exam Date**";
            ee.adm_no = "-";
            ee.section_id = e2.section_id;
            ee.exam_id = e2.exam_id;
            ee.obtained_marks = e2.total_marks;
            ee.subj_list = new List<exam_data_entry>();
            foreach (exam ex in exam_list_new) 
            {
                ee_subj = new exam_data_entry();
                ee_subj.subject_obtained = ex.exam_date.ToString("dd-MMM");
                ee.subj_list.Add(ee_subj);
            }
            
            ede_list_report.Insert(0, ee);
        }

        public void set_exam_data_entry_list()
        {
            ede_exam_list = new List<exam_data_entry>();

            foreach (admission adm in adm_list)
            {
                foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == adm.id))
                {
                    ede_obj = new exam_data_entry();
                    ede_obj = ede;
                    ede_obj.adm_no = adm.adm_no;
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
                        ede_s_o.adm_no = adm.adm_no;
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
            subjects subj = (subjects)subjects_cmb.SelectedItem;
            //exam ex = (exam)exam_cmb.SelectedItem;

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam_data_entry where subject_id=@subject_id && section_id=@section_id && session_id=" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {                        
                        cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = s.id;
                        cmd.Parameters.Add("@subject_id", MySqlDbType.VarChar).Value = subj.id;

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
        //print 
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = col_width;
            var ht = new GenaralAwardListHeader(this);
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }
        private DataTable CreateSampleDataTable()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            string a;
            var dataTable = new DataTable();
            col_width = new List<double>();

            //Add Headers
            AddColumn(dataTable, "Sr#", typeof(string));
            col_width.Add(30);
            AddColumn(dataTable, "Student Name", typeof(string));
            col_width.Add(120);
            AddColumn(dataTable, "Adm#", typeof(string));
            col_width.Add(60);
            
            foreach (exam ede in exam_list_new)
            {
                AddColumn(dataTable, ede.exam_name, typeof(string));
                col_width.Add(60);
            }
            
            //Add Marks

            foreach (exam_data_entry ee in ede_list_report)
            {
                var dataRow = dataTable.NewRow();
                j = 0;
                dataRow[j] = i.ToString();
                dataRow[++j] = ee.std_name.ToString();
                dataRow[++j] = ee.adm_no.ToString();
                foreach (exam_data_entry ede in ee.subj_list)
                {
                    dataRow[++j] = ede.subject_obtained;
                }
                dataTable.Rows.Add(dataRow);
                i++;
            }
            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        private void subjects_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (subjects_cmb.SelectedIndex > 0)
            {
                exam_img_grid.Visibility = Visibility.Hidden;
                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_grid.Visibility = Visibility.Visible;
                print_btn.Visibility = Visibility.Hidden;
                get_all_exams();
                exam_Datagrid.ItemsSource = exam_list;
            }
            else 
            {
                exam_img_grid.Visibility = Visibility.Visible;
                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_grid.Visibility = Visibility.Hidden;
                print_btn.Visibility = Visibility.Hidden;
            }
        }

         //exams button click
        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            exam_list_new = new List<exam>();
            foreach (exam ex in exam_list.Where(x => x.Checked == true)) 
            {
                exam_list_new.Add(ex);
                check = true;
            }

            if (check)
            {
                print_btn.Visibility = Visibility.Visible;
                sec = (sections)section_cmb.SelectedItem;                
                cl = (classes)class_cmb.SelectedItem;
                subj = (subjects)subjects_cmb.SelectedItem;
                get_all_admissions(cl.id);
                get_all_exams_entry();
                create_report();
            }
            else 
            {
                print_btn.Visibility = Visibility.Hidden;
                MessageBox.Show("Please Select Minimum One Exam");
            }
        }

        public void create_report() 
        {
            ede_list_report = new List<exam_data_entry>();
            exam_data_entry ede_obj;
            exam_data_entry ede_obj_sub;

            foreach(admission adm in adm_list)
            {
                ede_obj = new exam_data_entry();                
                ede_obj.std_id = adm.id;
                ede_obj.std_name = adm.std_name;
                ede_obj.adm_no = adm.adm_no;
                ede_obj.father_name = adm.father_name;
                ede_obj.subj_list = new List<exam_data_entry>();

                foreach (exam ex in exam_list_new) 
                {
                    foreach (exam_data_entry ede in ede_list.Where(x => x.exam_id == ex.id)) 
                    {                        
                        if(ede.std_id == adm.id)
                        {
                            ede_obj_sub = new exam_data_entry();
                            ede_obj_sub.exam_name = ex.exam_name;
                            ede_obj_sub.subject_obtained = ede.subject_obtained;
                            ede_obj_sub.subject_total = ede.subject_total;
                            ede_obj_sub.exam_id = ede.exam_id;
                            ede_obj.subj_list.Add(ede_obj_sub);
                        }
                        
                    }
                }
                ede_list_report.Add(ede_obj);
            }

            insert_exam_date_row();
            insert_total_marks_row();
            

            ExamReportBySubjectVM erbsvm;
            erbsvm = new ExamReportBySubjectVM(ede_list_report,exam_list_new);
            exam_entry_grid.DataContext = erbsvm;

            exam_grid.Visibility = Visibility.Hidden;
            exam_entry_grid.Visibility = Visibility.Visible;


        }
    }
}
