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
using System.ComponentModel;

namespace SMS.Messaging
{
    /// <summary>
    /// Interaction logic for ExamSms.xaml
    /// </summary>
    public partial class ExamSms : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<subjects> subjects_list;
        List<admission> adm_list;
        List<exam_entry> exam_entry_list;
        List<exam_entry> exam_results_list;
        List<admission> std_nos;
        string message;
        admission adm_obj;
        List<double> col_width;
        public ExamSms()
        {
            InitializeComponent();

            get_all_exams();
            exam_cmb.ItemsSource = exam_list;
            exam_list.Insert(0, new exam() { exam_name = "---Select Exam---", id = "-1" });
            exam_cmb.SelectedIndex = 0;

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            strength_textblock.Text = exam_entry_grid.Items.Count.ToString();
            SearchTextBox.Focus();
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
                sections s = (sections)section_cmb.SelectedItem;
                if (Convert.ToInt32(s.id) > 0)
                {


                    get_exam_entry();
                    if (exam_entry_list.Count > 0)
                    {

                        exam_entry_grid.Visibility = Visibility.Visible;
                        exam_img_grid.Visibility = Visibility.Hidden;
                        get_all_admissions(s.id);

                        exam_entry_grid.ItemsSource = null;
                        exam_entry_grid.Items.Refresh();
                        add_columns();
                        insert_total_marks_row();
                        exam_entry_grid.ItemsSource = exam_entry_list;
                        send_btn.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        send_btn.Visibility = Visibility.Hidden;
                        exam_entry_grid.ItemsSource = null;

                    }

                }
                else 
                {
                    send_btn.Visibility = Visibility.Hidden;
                    exam_entry_grid.ItemsSource = null;
                }
            }
            else
            {
                
                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;

                exam_entry_grid.ItemsSource = null;
                exam_entry_grid.Items.Refresh();
                exam_entry_grid.Columns.Clear();

            }

        }
        public void add_columns()
        {
            int i = 0;
            classes c = (classes)class_cmb.SelectedItem;
            get_all_subjects(c.id);

            DataGridTextColumn textColumn;
            textColumn = new DataGridTextColumn();
            textColumn.Header = "Student Name";
            textColumn.Binding = new Binding("std_name");
            textColumn.IsReadOnly = true;
            textColumn.Width = 200;
            exam_entry_grid.Columns.Add(textColumn);

            foreach (subjects subj in subjects_list)
            {
                i++;
                textColumn = new DataGridTextColumn();
                textColumn.Header = subj.subject_name;
                textColumn.Binding = new Binding("subj" + i + "_marks");
                exam_entry_grid.Columns.Add(textColumn);
            }
            textColumn = new DataGridTextColumn();
            textColumn.Header = "Total";
            textColumn.Binding = new Binding("obtained_marks");
            exam_entry_grid.Columns.Add(textColumn);

            textColumn = new DataGridTextColumn();
            textColumn.Header = "%";
            textColumn.Binding = new Binding("percentage");
            exam_entry_grid.Columns.Add(textColumn);

            textColumn = new DataGridTextColumn();
            textColumn.Header = "Position";
            textColumn.Binding = new Binding("position");
            exam_entry_grid.Columns.Add(textColumn);
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
                    cmd.CommandText = "SELECT* FROM sms_exam";
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

        // ===============     Get All Admissions          ================

        public void get_all_admissions(string id)
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT * FROM sms_admission where section_id=" + id;
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
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
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


        public void get_exam_entry()
        {
            exam_entry_list = new List<exam_entry>();
            exam e = (exam)exam_cmb.SelectedItem;
            sections s = (sections)section_cmb.SelectedItem;

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam_entry where exam_id=@exam_id && section_id=@section_id";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = e.id;
                        cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = s.id;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            exam_entry ee = new exam_entry()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                exam_id = Convert.ToString(reader["exam_id"].ToString()),
                                exam_name = Convert.ToString(reader["exam_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                section_id = Convert.ToString(reader["section_id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                total_marks = Convert.ToString(reader["total_marks"].ToString()),
                                obtained_marks = Convert.ToString(reader["obtained_marks"].ToString()),
                                percentage = Convert.ToString(reader["percentage"].ToString()),
                                remarks = Convert.ToString(reader["remarks"].ToString()),

                                subj1_id = Convert.ToString(reader["subj1_id"].ToString()),
                                subj1_name = Convert.ToString(reader["subj1_name"]).ToString(),
                                subj1_marks = Convert.ToString(reader["subj1_marks"].ToString()),
                                subj1_total = Convert.ToString(reader["subj1_total"].ToString()),

                                subj2_id = Convert.ToString(reader["subj2_id"]).ToString(),
                                subj2_name = Convert.ToString(reader["subj2_name"].ToString()),
                                subj2_marks = Convert.ToString(reader["subj2_marks"].ToString()),
                                subj2_total = Convert.ToString(reader["subj2_total"]).ToString(),

                                subj3_id = Convert.ToString(reader["subj3_id"].ToString()),
                                subj3_name = Convert.ToString(reader["subj3_name"].ToString()),
                                subj3_marks = Convert.ToString(reader["subj3_marks"]).ToString(),
                                subj3_total = Convert.ToString(reader["subj3_total"].ToString()),

                                subj4_id = Convert.ToString(reader["subj4_id"].ToString()),
                                subj4_name = Convert.ToString(reader["subj4_name"].ToString()),
                                subj4_marks = Convert.ToString(reader["subj4_marks"]).ToString(),
                                subj4_total = Convert.ToString(reader["subj4_total"].ToString()),

                                subj5_id = Convert.ToString(reader["subj5_id"].ToString()),
                                subj5_name = Convert.ToString(reader["subj5_name"].ToString()),
                                subj5_marks = Convert.ToString(reader["subj5_marks"]).ToString(),
                                subj5_total = Convert.ToString(reader["subj5_total"].ToString()),

                                subj6_id = Convert.ToString(reader["subj6_id"].ToString()),
                                subj6_name = Convert.ToString(reader["subj6_name"].ToString()),
                                subj6_marks = Convert.ToString(reader["subj6_marks"]).ToString(),
                                subj6_total = Convert.ToString(reader["subj6_total"].ToString()),

                                subj7_id = Convert.ToString(reader["subj7_id"].ToString()),
                                subj7_name = Convert.ToString(reader["subj7_name"].ToString()),
                                subj7_marks = Convert.ToString(reader["subj7_marks"]).ToString(),
                                subj7_total = Convert.ToString(reader["subj7_total"].ToString()),

                                subj8_id = Convert.ToString(reader["subj8_id"].ToString()),
                                subj8_name = Convert.ToString(reader["subj8_name"].ToString()),
                                subj8_marks = Convert.ToString(reader["subj8_marks"]).ToString(),
                                subj8_total = Convert.ToString(reader["subj8_total"].ToString()),

                                subj9_id = Convert.ToString(reader["subj9_id"].ToString()),
                                subj9_name = Convert.ToString(reader["subj9_name"].ToString()),
                                subj9_marks = Convert.ToString(reader["subj9_marks"]).ToString(),
                                subj9_total = Convert.ToString(reader["subj9_total"].ToString()),

                                subj10_id = Convert.ToString(reader["subj10_id"].ToString()),
                                subj10_name = Convert.ToString(reader["subj10_name"].ToString()),
                                subj10_marks = Convert.ToString(reader["subj10_marks"]).ToString(),
                                subj10_total = Convert.ToString(reader["subj10_total"].ToString()),
                            };
                            exam_entry_list.Add(ee);

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

        private void exam_entry_grid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        public void insert_total_marks_row()
        {
            exam_entry ee = new exam_entry();
            exam_entry e2 = new exam_entry();
            e2 = exam_entry_list[0];

            ee.std_name = "**Total Marks**";
            ee.subj1_marks = e2.subj1_total;
            ee.subj2_marks = e2.subj2_total;
            ee.subj3_marks = e2.subj3_total;
            ee.subj4_marks = e2.subj4_total;
            ee.subj5_marks = e2.subj5_total;
            ee.subj6_marks = e2.subj6_total;
            ee.subj7_marks = e2.subj7_total;
            ee.subj8_marks = e2.subj8_total;
            ee.subj9_marks = e2.subj9_total;
            ee.subj10_marks = e2.subj10_total;
            ee.obtained_marks = e2.total_marks;

            exam_entry_list.Insert(0, ee);
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            string v_search;
            v_search = SearchTextBox.Text;
            
            if (by_name.IsChecked == true)
            {
                {
                    exam_entry_grid.ItemsSource = exam_entry_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name == "**Total Marks**");                    
                    exam_entry_grid.Items.Refresh();
                }
            }
            if(v_search.Length == 0)
            {
                exam_entry_grid.ItemsSource = exam_entry_list;
            }
            

        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            
            int i = 1;
            std_nos = new List<admission>();
            exam_results_list = new List<exam_entry>();
            for (int row = 0; row < exam_entry_grid.Items.Count; row++)
            {
                if (row != 0)
                {
                    i = 1;
                    message = "";
                    adm_obj = new admission();
                    exam_entry ee = new exam_entry();
                    ee = (exam_entry)exam_entry_grid.Items[row];
                    message = ee.exam_name+" Results: "+ee.std_name+" has got "+ee.obtained_marks+"/"+ee.total_marks+". Percentage= "+ee.percentage+"%. ";
                    foreach(subjects sub in subjects_list)
                    {
                        
                        if(i == 1)
                        {
                            message = message + ee.subj1_name+"="+ee.subj1_marks+"/"+ee.subj1_total+". ";
                        }
                        if (i == 2)
                        {
                            message = message + ee.subj2_name + "=" + ee.subj2_marks + "/" + ee.subj2_total + ". ";
                        }
                        if (i == 3)
                        {
                            message = message + ee.subj3_name + "=" + ee.subj3_marks + "/" + ee.subj3_total + ". ";
                        }
                        if (i == 4)
                        {
                            message = message + ee.subj4_name + "=" + ee.subj4_marks + "/" + ee.subj4_total + ". ";
                        }
                        if (i == 5)
                        {
                            message = message + ee.subj5_name + "=" + ee.subj5_marks + "/" + ee.subj5_total + ". ";
                        }
                        if (i == 6)
                        {
                            message = message + ee.subj6_name + "=" + ee.subj6_marks + "/" + ee.subj6_total + ". ";
                        }
                        if (i == 7)
                        {
                            message = message + ee.subj7_name + "=" + ee.subj7_marks + "/" + ee.subj7_total + ". ";
                        }
                        if (i == 8)
                        {
                            message = message + ee.subj8_name + "=" + ee.subj8_marks + "/" + ee.subj8_total + ". ";
                        }
                        if (i == 9)
                        {
                            message = message + ee.subj9_name + "=" + ee.subj9_marks + "/" + ee.subj9_total + ". ";
                        }
                        if (i == 10)
                        {
                            message = message + ee.subj10_name + "=" + ee.subj10_marks + "/" + ee.subj10_total + ". ";
                        }
                        i++;
                    }

                    foreach(admission adm in adm_list)
                    {
                        if(adm.id == ee.std_id)
                        {
                            ee.std_cell_no = adm.cell_no;
                            adm_obj = adm;
                        }
                    }
                    message = message + "Admin " + MainWindow.ins.institute_name;
                    adm_obj.cell_no = ee.std_cell_no;
                    adm_obj.sms_message = message;
                    adm_obj.sms_type = "Exam";

                    std_nos.Add(adm_obj);
                }
                
            }
            
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Send    " + std_nos.Count + "  SMS ?", "Send Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (mbr == MessageBoxResult.Yes)
            {
                //this.Close();               
                UploadWindow uw = new UploadWindow(std_nos,false);
                uw.Show();
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
