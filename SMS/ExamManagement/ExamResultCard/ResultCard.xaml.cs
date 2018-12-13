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
using SUT.PrintEngine.Utils;
using System.IO;

namespace SMS.ExamManagement.ExamResultCard
{
    /// <summary>
    /// Interaction logic for ResultCard.xaml
    /// </summary>
    public partial class ResultCard : Page
    {
        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<subjects> subjects_list;
        List<admission> adm_list;
        List<exam_entry> exam_entry_list;
        List<exam_marks> exam_marks_list;
        exam_marks em_obj;
        int p = 0;

        double percentage = 0;
        double obt = 0;
        double total = 0;
        double total_subj = 0;
        double total_obt = 0;

        public ResultCard()
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

            get_all_admissions();
            
            
            
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
                get_all_subjects(id);
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
                        print_btn.Visibility = Visibility.Visible;
                        exam_entry_grid.Visibility = Visibility.Visible;
                        exam_img_grid.Visibility = Visibility.Hidden;
                        Result_grid_lstbox.ItemsSource = exam_entry_list;
                        
                        

                    }
                    else
                    {

                        print_btn.Visibility = Visibility.Hidden;
                        exam_img_grid.Visibility = Visibility.Visible;
                    }

                }
            }
            else
            {
                print_btn.Visibility = Visibility.Hidden;
                exam_entry_grid.Visibility = Visibility.Hidden;
                exam_img_grid.Visibility = Visibility.Visible;              

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

        public void get_all_admissions()
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && session_id=" + MainWindow.session.id;
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

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
                            father_cnic = Convert.ToString(reader["father_cnic"].ToString()),
                            father_income = Convert.ToString(reader["father_income"].ToString()),
                            religion = Convert.ToString(reader["religion"].ToString()),
                            dob = Convert.ToDateTime(reader["dob"]),
                            b_form = Convert.ToString(reader["b_form"].ToString()),
                            parmanent_adress = Convert.ToString(reader["parmanent_adress"].ToString()),
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            emergency_address = Convert.ToString(reader["emergency_address"].ToString()),
                            previous_school = Convert.ToString(reader["previous_school"].ToString()),
                            boarding = Convert.ToString(reader["boarding"].ToString()),
                            transport = Convert.ToString(reader["transport"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            transport_fee = Convert.ToString(reader["transport_fee"].ToString()),
                            reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                            tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                            exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                            security_fee = Convert.ToString(reader["security_fee"].ToString()),
                            stationary_fee = Convert.ToString(reader["stationary_fee"].ToString()),
                            scholarship = Convert.ToString(reader["scholarship"].ToString()),
                            misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                            other_exp = Convert.ToString(reader["other_exp"].ToString()),
                            adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                            total = Convert.ToString(reader["total"].ToString()),
                            date_time = Convert.ToDateTime(reader["date_time"]),
                            created_by = Convert.ToString(reader["created_by"].ToString()),
                            is_active = Convert.ToString(reader["is_active"].ToString()),
                            image = img,
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


        public void get_exam_entry()
        {
            exam_entry_list = new List<exam_entry>();
            exam e = (exam)exam_cmb.SelectedItem;
            sections s = (sections)section_cmb.SelectedItem;
            p = 0;

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam_entry where exam_id=@exam_id && section_id=@section_id ORDER BY percentage DESC";
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
                            
                            foreach(admission adm in adm_list)
                            {
                                if(adm.id == ee.std_id)
                                {
                                    ee.std_img = adm.image;
                                    ee.std_roll_no = adm.roll_no;
                                    ee.father_name = adm.father_name;
                                    ee.date_time = DateTime.Now.ToString("dd/MM/yyyy");
                                }
                                
                            }
                            //--------------------- Exam  Marks List (subjects)  -----------------------------------
                            exam_marks_list = new List<exam_marks>();                            
                            int i = 0;
                            foreach(subjects subj in subjects_list)
                            {
                                i++;
                                obt = 0;
                                total = 0;
                                percentage = 0;

                                em_obj = new exam_marks();
                                em_obj.subj_id = Convert.ToString(reader["subj"+i+"_id"].ToString());
                                em_obj.subj_name = Convert.ToString(reader["subj" + i + "_name"].ToString());
                                em_obj.subj_total = Convert.ToString(reader["subj" + i + "_total"].ToString());
                                em_obj.subj_obtained = Convert.ToString(reader["subj" + i + "_marks"].ToString());
                                if (Convert.ToInt32(em_obj.subj_total) > 0)
                                {
                                    obt= Convert.ToDouble(em_obj.subj_obtained);
                                    total = Convert.ToDouble(em_obj.subj_total);
                                    percentage = obt / total;
                                    percentage = percentage * 100;
                                    em_obj.percentage = percentage.ToString();
                                }
                                else 
                                {
                                    percentage = 0;
                                    em_obj.percentage = percentage.ToString();
                                }

                                if(percentage >= 90)
                                {
                                    em_obj.grade = "A+";
                                    em_obj.remarks = "Outstanding";
                                }
                                else if (percentage >= 80 && percentage < 90)
                                {
                                    em_obj.grade = "A";
                                    em_obj.remarks = "Excellent";
                                }
                                else if (percentage >= 70 && percentage < 80)
                                {
                                    em_obj.grade = "B";
                                    em_obj.remarks = "V.Good";
                                }
                                else if (percentage >= 60 && percentage < 70)
                                {
                                    em_obj.grade = "C";
                                    em_obj.remarks = "Good";
                                }
                                else if (percentage >= 50 && percentage < 60)
                                {
                                    em_obj.grade = "D";
                                    em_obj.remarks = "Fair";
                                }
                                else if (percentage >= 40 && percentage < 50)
                                {
                                    em_obj.grade = "E";
                                    em_obj.remarks = "Needs Improvement";
                                }
                                else 
                                {
                                    em_obj.grade = "F";
                                    em_obj.remarks = "Fail";
                                }
                                exam_marks_list.Add(em_obj);
                            }

                            //Total marks row

                            em_obj = new exam_marks();
                            em_obj.subj_name = "*Total Marks*";
                            em_obj.subj_total = ee.total_marks;
                            em_obj.subj_obtained = ee.obtained_marks;
                            em_obj.percentage = ee.percentage;
                            if (Convert.ToDouble(ee.percentage) >= 90)
                            {
                                em_obj.grade = "A+";
                                em_obj.remarks = "OutStanding";
                            }
                            else if (Convert.ToDouble(ee.percentage) >= 80 && Convert.ToDouble(ee.percentage) < 90)
                            {
                                em_obj.grade = "A";
                                em_obj.remarks = "Excellent";
                            }
                            else if (Convert.ToDouble(ee.percentage) >= 70 && Convert.ToDouble(ee.percentage) < 80)
                            {
                                em_obj.grade = "B";
                                em_obj.remarks = "V.Good";
                            }
                            else if (Convert.ToDouble(ee.percentage) >= 60 && Convert.ToDouble(ee.percentage) < 70)
                            {
                                em_obj.grade = "C";
                                em_obj.remarks = "Good";
                            }
                            else if (Convert.ToDouble(ee.percentage) >= 50 && Convert.ToDouble(ee.percentage) < 60)
                            {
                                em_obj.grade = "D";
                                em_obj.remarks = "Fair";
                            }
                            else if (Convert.ToDouble(ee.percentage) >= 40 && Convert.ToDouble(ee.percentage) < 50)
                            {
                                em_obj.grade = "E";
                                em_obj.remarks = "Needs Improvement";
                            }
                            else
                            {
                                em_obj.grade = "F";
                                em_obj.remarks = "Fail";
                            }
                            exam_marks_list.Add(em_obj);

                            ee.marks_list = exam_marks_list;
                            ee.institute_name = MainWindow.ins.institute_name;
                            ee.institute_logo = MainWindow.ins.institute_logo;

                            //postition
                            p++;
                            if(p==1)
                            {
                                ee.position = "1st";
                                ee.position_tb = "Position";
                            }
                            else if (p == 2)
                            {
                                ee.position = "2nd";
                                ee.position_tb = "Position";
                            }
                            else if (p == 3)
                            {
                                ee.position = "3rd";
                                ee.position_tb = "Position";
                            }
                            else 
                            {
                                ee.position = "";
                                ee.position_tb = "";
                            }

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

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var visualSize = new Size(Result_grid_lstbox.ActualWidth, Result_grid_lstbox.ActualHeight);
            var printControl = PrintControlFactory.Create(visualSize, Result_grid_lstbox);

            printControl.ShowPrintPreview();
            //PrintDialog printDlg = new PrintDialog();
            //printDlg.PrintVisual(Result_grid_lstbox, "Listbox Printing.");
        }

        
    }
}
