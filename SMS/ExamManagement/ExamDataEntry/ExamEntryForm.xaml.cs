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


namespace SMS.ExamManagement.ExamDataEntry
{
    /// <summary>
    /// Interaction logic for ExamEntryForm.xaml
    /// </summary>
    public partial class ExamEntryForm : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<subjects> subjects_list;
        List<admission> adm_list;
        List<exam_entry> exam_entry_list;
        List<exam_entry> submit_exam_entry_list;
        exam_entry total_marks_obj;

        public ExamEntryForm()
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
                exam_entry_grid.ItemsSource = null;
                exam_entry_grid.Items.Refresh();
                exam_entry_grid.Columns.Clear();

                sections s = (sections)section_cmb.SelectedItem;
                if (Convert.ToInt32(s.id) > 0)
                {
                    

                    get_exam_entry();
                    if (exam_entry_list.Count > 0)
                    {
                        delete_btn.Visibility = Visibility.Visible;
                        create_btn.Visibility = Visibility.Hidden;
                        exam_entry_grid.Visibility = Visibility.Visible;
                        save_btn.Visibility = Visibility.Visible;
                        exam_img_grid.Visibility = Visibility.Hidden;


                        exam_entry_grid.ItemsSource = null;
                        exam_entry_grid.Items.Refresh();
                        add_columns();
                        insert_total_marks_row();
                        exam_entry_grid.ItemsSource = exam_entry_list;
                        
                    }
                    else 
                    {
                        create_btn.Visibility = Visibility.Visible;
                        delete_btn.Visibility = Visibility.Hidden;
                    }
                    
                }
            }
            else 
            {
                delete_btn.Visibility = Visibility.Hidden;
                create_btn.Visibility = Visibility.Hidden;
                save_btn.Visibility = Visibility.Hidden;
                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;

                exam_entry_grid.ItemsSource = null;
                exam_entry_grid.Items.Refresh();
                exam_entry_grid.Columns.Clear();
                
            }
            
        }
        public void add_columns() 
        {
            int i=0;
            classes c = (classes)class_cmb.SelectedItem;
            get_all_subjects(c.id);

            DataGridTextColumn textColumn ;
            textColumn = new DataGridTextColumn();
            textColumn.Header = "Student Name";
            textColumn.Binding = new Binding("std_name");
            textColumn.IsReadOnly = true;
            textColumn.Width = 200;
            exam_entry_grid.Columns.Add(textColumn); 

            foreach(subjects subj in subjects_list)
            {
                i++;
                textColumn = new DataGridTextColumn();
                textColumn.Header = subj.subject_name;
                textColumn.Binding = new Binding("subj"+i+"_marks");
                exam_entry_grid.Columns.Add(textColumn); 
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
                    catch(Exception ex)
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



                    cmd.CommandText = "SELECT* FROM sms_subject where class_id ="+id;
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
                cmd.CommandText = "SELECT id,std_name,father_name,section_id FROM sms_admission where section_id="+id;
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



        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            exam_entry ee;
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                for (int i = 0; i < exam_entry_grid.Items.Count - 1; i++)
                {
                    try
                    {


                        if (i != 0)
                        {
                            ee = new exam_entry();
                            ee = (exam_entry)exam_entry_grid.Items[i];
                            delete(ee);                            
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        break;
                    }
                }
                MessageBox.Show("Successfully Deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                section_cmb.SelectedIndex = 0;
            }
        }
        //-------------     Delete          ---------------------------

        public int delete(exam_entry ee)
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_exam_entry where id=" + ee.id;
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

        private void create_btn_Click(object sender, RoutedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string c_id = c.id;
            sections s = (sections)section_cmb.SelectedItem;
            string s_id = s.id;

            get_all_subjects(c_id);
            get_all_admissions(s_id);

            create_exam();
            create_btn.Visibility = Visibility.Hidden;
            delete_btn.Visibility = Visibility.Visible;
            exam_img_grid.Visibility = Visibility.Hidden;
            exam_entry_grid.Visibility = Visibility.Visible;
            save_btn.Visibility = Visibility.Visible;
            get_exam_entry();
            add_columns();
            insert_total_marks_row();
            exam_entry_grid.ItemsSource = exam_entry_list;
            
            

            //MessageBox.Show("Successfully created Exam");
            
        }

        public void create_exam() 
        {
            exam ex = (exam)exam_cmb.SelectedItem;
            string ex_id = ex.id;
            string ex_name = ex.exam_name;

            classes c = (classes)class_cmb.SelectedItem;
            string c_id = c.id;
            string c_name = c.class_name;

            sections s = (sections)section_cmb.SelectedItem;
            string s_id = s.id;
            string s_name = s.section_name;

            foreach (admission adm in adm_list)
            {
                    try
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Insert into sms_exam_entry (std_id,std_name,exam_id,exam_name,class_id,class_name,section_id,section_name) Values(@std_id,@std_name,@exam_id,@exam_name,@class_id,@class_name,@section_id,@section_name)";
                                cmd.Connection = con;

                                cmd.Parameters.Add("@std_id", MySqlDbType.VarChar).Value = adm.id;
                                cmd.Parameters.Add("@std_name", MySqlDbType.VarChar).Value = adm.std_name;
                                cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ex_id;
                                cmd.Parameters.Add("@exam_name", MySqlDbType.VarChar).Value = ex_name;
                                cmd.Parameters.Add("@class_id", MySqlDbType.VarChar).Value = c_id;
                                cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = c_name;
                                cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = s_id;
                                cmd.Parameters.Add("@section_name", MySqlDbType.VarChar).Value = s_name;                                

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
            int i = 0;
            foreach (subjects subj in subjects_list) 
            {
                
                try
                {
                    i++;
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_exam_entry SET subj"+i+"_id =@subj_id , subj"+i+"_name =@subj_name where exam_id=@exam_id && section_id=@section_id";
                            cmd.Connection = con;

                            cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ex_id;
                            cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = s_id;

                            cmd.Parameters.Add("@subj_id", MySqlDbType.VarChar).Value = subj.id;
                            cmd.Parameters.Add("@subj_name", MySqlDbType.VarChar).Value = subj.subject_name;

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
                        cmd.Parameters.Add("@exam_id",MySqlDbType.VarChar).Value=e.id;
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


        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            exam_entry ee;
            submit_exam_entry_list = new List<exam_entry>();
            for (int i = 0; i < exam_entry_grid.Items.Count-1; i++ ) 
            {
                
                if (i == 0)
                {
                    ee = new exam_entry();
                    ee = (exam_entry)exam_entry_grid.Items[i];
                    total_marks_obj = new exam_entry();
                    total_marks_obj = ee;
                    
                }
                else
                {
                    ee = new exam_entry();
                    ee = (exam_entry)exam_entry_grid.Items[i];
                    submit_exam_entry_list.Add(ee);
                    total_marks_obj.section_id = ee.section_id;
                    total_marks_obj.exam_id = ee.exam_id;
                }
            }

            calculate_nos();

            save_changes();
            MessageBox.Show("Updated Successfully","Updated",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        public void calculate_nos() 
        {
            foreach(exam_entry ee in submit_exam_entry_list)
            {
                double t_marks = 0;
                double o_marks = 0;
                double percent = 0;

                t_marks = Convert.ToDouble(total_marks_obj.subj1_marks) + Convert.ToDouble(total_marks_obj.subj2_marks) + Convert.ToDouble(total_marks_obj.subj3_marks) + Convert.ToDouble(total_marks_obj.subj4_marks) + Convert.ToDouble(total_marks_obj.subj5_marks) + Convert.ToDouble(total_marks_obj.subj6_marks) + Convert.ToDouble(total_marks_obj.subj7_marks) + Convert.ToDouble(total_marks_obj.subj8_marks) + Convert.ToDouble(total_marks_obj.subj9_marks) + Convert.ToDouble(total_marks_obj.subj10_marks);
                o_marks = Convert.ToDouble(ee.subj1_marks) + Convert.ToDouble(ee.subj2_marks) + Convert.ToDouble(ee.subj3_marks) + Convert.ToDouble(ee.subj4_marks) + Convert.ToDouble(ee.subj5_marks) + Convert.ToDouble(ee.subj6_marks) + Convert.ToDouble(ee.subj7_marks) + Convert.ToDouble(ee.subj8_marks) + Convert.ToDouble(ee.subj9_marks) + Convert.ToDouble(ee.subj10_marks);
                if (t_marks != 0)
                {
                    percent = (o_marks / t_marks) * 100;
                    percent = Math.Round(percent,2);
                }
                ee.total_marks = t_marks.ToString();
                ee.obtained_marks = o_marks.ToString();
                ee.percentage = percent.ToString("F");
                ee.remarks = "Good";
                total_marks_obj.total_marks = t_marks.ToString();
            }
        }

        public void save_changes() 
        {
            int i = 0;
            foreach (exam_entry ee in submit_exam_entry_list)
            {

                try
                {
                    i++;
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_exam_entry SET subj1_marks =@subj1_marks , subj2_marks =@subj2_marks, subj3_marks =@subj3_marks,subj4_marks =@subj4_marks , subj5_marks =@subj5_marks, subj6_marks =@subj6_marks,subj7_marks =@subj7_marks , subj8_marks =@subj8_marks, subj9_marks =@subj9_marks,subj10_marks =@subj10_marks,total_marks =@total_marks,obtained_marks =@obtained_marks,percentage =@percentage,remarks =@remarks where exam_id=@exam_id && std_id=@std_id";
                            cmd.Connection = con;

                            cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = ee.exam_id;
                            cmd.Parameters.Add("@std_id", MySqlDbType.VarChar).Value = ee.std_id;

                            cmd.Parameters.Add("@subj1_marks", MySqlDbType.VarChar).Value = ee.subj1_marks;
                            cmd.Parameters.Add("@subj2_marks", MySqlDbType.VarChar).Value = ee.subj2_marks;
                            cmd.Parameters.Add("@subj3_marks", MySqlDbType.VarChar).Value = ee.subj3_marks;
                            cmd.Parameters.Add("@subj4_marks", MySqlDbType.VarChar).Value = ee.subj4_marks;
                            cmd.Parameters.Add("@subj5_marks", MySqlDbType.VarChar).Value = ee.subj5_marks;
                            cmd.Parameters.Add("@subj6_marks", MySqlDbType.VarChar).Value = ee.subj6_marks;
                            cmd.Parameters.Add("@subj7_marks", MySqlDbType.VarChar).Value = ee.subj7_marks;
                            cmd.Parameters.Add("@subj8_marks", MySqlDbType.VarChar).Value = ee.subj8_marks;
                            cmd.Parameters.Add("@subj9_marks", MySqlDbType.VarChar).Value = ee.subj9_marks;
                            cmd.Parameters.Add("@subj10_marks", MySqlDbType.VarChar).Value = ee.subj10_marks;

                            cmd.Parameters.Add("@total_marks", MySqlDbType.VarChar).Value = ee.total_marks;
                            cmd.Parameters.Add("@obtained_marks", MySqlDbType.VarChar).Value = ee.obtained_marks;
                            cmd.Parameters.Add("@percentage", MySqlDbType.VarChar).Value = ee.percentage;
                            cmd.Parameters.Add("@remarks", MySqlDbType.VarChar).Value = ee.remarks;

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

                try
                {
                    
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_exam_entry SET subj1_total =@subj1_total,subj2_total =@subj2_total,subj3_total =@subj3_total,subj4_total =@subj4_total,subj5_total =@subj5_total,subj6_total =@subj6_total,subj7_total =@subj7_total,subj8_total =@subj8_total,subj9_total =@subj9_total,subj10_total =@subj10_total,total_marks=@total_marks   where exam_id=@exam_id && section_id=@section_id";
                            cmd.Connection = con;

                            cmd.Parameters.Add("@exam_id", MySqlDbType.VarChar).Value = total_marks_obj.exam_id;
                            cmd.Parameters.Add("@section_id", MySqlDbType.VarChar).Value = total_marks_obj.section_id;

                            cmd.Parameters.Add("@subj1_total", MySqlDbType.VarChar).Value = total_marks_obj.subj1_marks;
                            cmd.Parameters.Add("@subj2_total", MySqlDbType.VarChar).Value = total_marks_obj.subj2_marks;
                            cmd.Parameters.Add("@subj3_total", MySqlDbType.VarChar).Value = total_marks_obj.subj3_marks;
                            cmd.Parameters.Add("@subj4_total", MySqlDbType.VarChar).Value = total_marks_obj.subj4_marks;
                            cmd.Parameters.Add("@subj5_total", MySqlDbType.VarChar).Value = total_marks_obj.subj5_marks;
                            cmd.Parameters.Add("@subj6_total", MySqlDbType.VarChar).Value = total_marks_obj.subj6_marks;
                            cmd.Parameters.Add("@subj7_total", MySqlDbType.VarChar).Value = total_marks_obj.subj7_marks;
                            cmd.Parameters.Add("@subj8_total", MySqlDbType.VarChar).Value = total_marks_obj.subj8_marks;
                            cmd.Parameters.Add("@subj9_total", MySqlDbType.VarChar).Value = total_marks_obj.subj9_marks;
                            cmd.Parameters.Add("@subj10_total", MySqlDbType.VarChar).Value = total_marks_obj.subj10_marks;
                            cmd.Parameters.Add("@total_marks", MySqlDbType.VarChar).Value = total_marks_obj.total_marks;
                            

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

        public void insert_total_marks_row()
        {
            
            exam_entry ee = new exam_entry();
            exam_entry e2 = new exam_entry();
            e2=exam_entry_list[0];
            
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
    }
}
