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
using SMS.Upload;
using System.Collections.ObjectModel;
using SMS.ViewModels;
using SUT.PrintEngine.Utils;
using System.Windows.Markup;
using System.Data;
using SMS.Messaging.BrandedSms;
using SMS.Messaging.SmsOption;
using System.ComponentModel;

namespace SMS.Messaging.ExamSmsNew
{
    /// <summary>
    /// Interaction logic for ExamSmsNew.xaml
    /// </summary>
    public partial class ExamSmsNew : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        ObservableCollection<subjects> subjects_list;
        List<admission> adm_list;
        List<exam_data_entry> ede_list;
        public static List<exam_data_entry> ede_exam_list;
        exam_data_entry ede_obj;
        ExamViewModel evm;
        List<double> col_width;
        exam_data_entry ede_obj1;
        List<admission> std_nos;
        string exam_name;
        string class_name;
        public static bool isbranded = false;

        public ExamSmsNew()
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
            strength_textblock.Text = exam_entry_grid.Items.Count.ToString();
            ede_exam_list = new List<exam_data_entry>();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in ede_exam_list)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            exam_entry_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            exam_entry_grid.SelectedItem = e.Source;
            ede_obj1 = new exam_data_entry();
            ede_obj1 = (exam_data_entry)exam_entry_grid.SelectedItem;
            foreach (exam_data_entry ede in ede_list)
            {
                if (ede_obj1.id == ede.id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
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
                exam exe = (exam)exam_cmb.SelectedItem;
                exam_name = exe.exam_name;
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
                classes cl = (classes)class_cmb.SelectedItem;
                class_name = cl.class_name;
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
                    exam_entry_grid.Visibility = Visibility.Hidden;
                    exam_img_grid.Visibility = Visibility.Hidden;

                    get_all_admissions(s.id);
                    get_all_subjects(c.id);
                    get_all_exams_entry();

                    if (ede_list.Count > 0)
                    {
                        exam_img_grid.Visibility = Visibility.Hidden;
                        exam_entry_grid.Visibility = Visibility.Visible;
                        send_btn.Visibility = Visibility.Visible;
                        set_exam_data_entry_list();
                        insert_total_marks_row();
                        evm = new ExamViewModel(ede_exam_list);
                        exam_entry_grid.DataContext = evm;

                    }
                    else
                    {
                        exam_entry_grid.Visibility = Visibility.Hidden;
                        exam_img_grid.Visibility = Visibility.Visible;
                        
                    }
                }
            }
            else
            {

                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;
                
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

        //-----------              Get All Subjects     ----------------------
        public void get_all_subjects(string id)
        {

            subjects_list = new ObservableCollection<subjects>();
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

        // ===============     Get All Admissions          ================
        public void get_all_admissions(string id)
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM sms_admission where session_id=@session_id && section_id=" + id;
                cmd.Parameters.Add("@session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;
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
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
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
        public void set_exam_data_entry_list()
        {

            ede_exam_list = new List<exam_data_entry>();

            foreach (admission adm in adm_list)
            {
                foreach (exam_data_entry ede in ede_list.Where(x => x.std_id == adm.id))
                {
                    ede_obj = new exam_data_entry();
                    ede_obj = ede;
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
                    cmd.CommandText = "SELECT* FROM sms_exam_data_entry where exam_id=@exam_id && section_id=@section_id && session_id=@session_id";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ex.id;
                        cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = s.id;
                        cmd.Parameters.Add("@session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;

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
            var ht = new HeaderTemplate();
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
            foreach (exam_data_entry ee in ede_exam_list)
            {
                foreach (exam_data_entry ede in ee.subj_list)
                {
                    AddColumn(dataTable, ede.subject_name, typeof(string));
                    col_width.Add(60);
                }
                break;
            }
            AddColumn(dataTable, "Total", typeof(string));
            col_width.Add(60);
            AddColumn(dataTable, "%", typeof(string));
            col_width.Add(50);
            AddColumn(dataTable, "Grade", typeof(string));
            col_width.Add(50);

            //Add Marks

            foreach (exam_data_entry ee in ede_exam_list)
            {
                var dataRow = dataTable.NewRow();
                j = 0;
                dataRow[j] = i.ToString();
                dataRow[++j] = ee.std_name.ToString();
                foreach (exam_data_entry ede in ee.subj_list)
                {
                    dataRow[++j] = ede.subject_obtained;
                }
                dataRow[++j] = ee.obtained_marks;
                dataRow[++j] = ee.percentage;
                dataRow[++j] = ee.grade;

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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {            
            admission adm = new admission();
            std_nos = new List<admission>();
            string message = "";
            foreach(exam_data_entry ede in ede_exam_list.Where(x=>x.Checked == true))
            {
                
                message = exam_name+ " Results. Class "+class_name+". ";
                message = message + ede.std_name + " has got " + ede.obtained_marks+"/"+ede.total_marks+" with "+ede.percentage+"%. ";
                foreach(exam_data_entry ede_subj in ede.subj_list)
                {
                    message=message+ ede_subj.subject_name+"="+ede_subj.subject_obtained+"/"+ede_subj.subject_total+". ";
                }
                message = message + " Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                foreach (admission adm_obj in adm_list.Where(x => x.id == ede.std_id)) 
                {
                    adm_obj.sms_message = message;
                    adm_obj.sms_type = "Exam Sms";
                    std_nos.Add(adm_obj);
                    break;
                }   
            }
            if (std_nos.Count > 0)
            {
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
                dpd.AddValueChanged(exam_entry_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = exam_entry_grid.Items.Count.ToString();
        }

    }
}
