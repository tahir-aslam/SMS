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
using SMS.ClassManagement.Subject;
using MySql.Data.MySqlClient;

namespace SMS.ClassManagement.Subject
{
    /// <summary>
    /// Interaction logic for SubjectForm.xaml
    /// </summary>
    public partial class SubjectForm : Window
    {
        SubjectSearch SS;
        List<classes> classes_list;
        List<employees> emp_list;
        List<subjects> subjects_list;
        subjects subjects_obj;
        subjects obj;
        string mode;

        public SubjectForm(string m, SubjectSearch ss, subjects ob)
        {
            InitializeComponent();

            SS = ss;
            this.obj = ob;
            this.mode = m;           

            subjects_list = new List<subjects>();
            classes_list = new List<classes>();
            emp_list = new List<employees>();            
        }

       

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //--------------           Save          ----------------------

        public void save()
        {
            fill_object();
            if (validate())
            {
                if (mode == "insert")
                {
                    if (submit() == 0)
                    {
                        SS.load_grid();
                        MessageBox.Show("Record Added Successfully");
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else if (mode == "edit")
                {
                    if (update() > 0)
                    {
                        SS.load_grid();
                        MessageBox.Show("Record Updated Successfully");
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }

                }
                else
                {
                    MessageBox.Show("mode not set");
                }

            }
        }


        //---------------           Submit Form    ----------------------------------

        public int submit()
        {
            int i = 1;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_subject(class_id,class_name,emp_id,emp_name,subject_name,remarks,total_marks,is_active,created_by,date_time) Values(@class_id,@class_name,@emp_id,@emp_name,@subject_name,@remarks,@total_marks,@is_active,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_name;
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_id;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_name;
                        cmd.Parameters.Add("@subject_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_name;
                        cmd.Parameters.Add("@remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.remarks;
                        cmd.Parameters.Add("@total_marks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.total_marks;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.is_Active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = subjects_obj.date_time;


                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Form        ---------------------------------

        public int update()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_subject SET class_id=@class_id,class_name=@class_name,emp_id=@emp_id,emp_name=@emp_name,subject_name=@subject_name,remarks=@remarks,total_marks=@total_marks,is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_name;
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_id;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_name;
                        cmd.Parameters.Add("@subject_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_name;
                        cmd.Parameters.Add("@remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.remarks;
                        cmd.Parameters.Add("@total_marks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.total_marks;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.is_Active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = subjects_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        //------------------    Fill Object      ------------------------

        public void fill_object()
        {
            subjects_obj = new subjects();

            classes cl = (classes)class_cmb.SelectedItem;
            string cl_id = cl.id.ToString();
            string cl_name = cl.class_name.ToString();

            employees emp = (employees)teacher_cmb.SelectedItem;
            string emp_id = emp.id.ToString();
            string emp_name = emp.emp_name.ToString();

            subjects_obj.class_id = cl_id;
            subjects_obj.class_name = cl_name;
            subjects_obj.emp_id = emp_id;
            subjects_obj.emp_name = emp_name;
            subjects_obj.subject_name = subj_name_textbox.Text.Trim();
            subjects_obj.date_time = DateTime.Now;
            subjects_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            subjects_obj.remarks = remarks_textbox.Text.Trim();
            subjects_obj.total_marks = total_marks_textbox.Text.Trim();

            if (is_active_chekbox.IsChecked == true)
            {
                subjects_obj.is_Active = "Y";
            }
            else
            {
                subjects_obj.is_Active = "N";
            }
        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            class_cmb.SelectedValue = obj.class_id;
            teacher_cmb.SelectedValue = obj.emp_id;
            subj_name_textbox.Text = obj.subject_name;
            total_marks_textbox.Text = obj.total_marks;
            remarks_textbox.Text = obj.remarks;

            if (obj.is_Active == "Y")
            {
                is_active_chekbox.IsChecked = true;
            }
            else
            {
                is_active_chekbox.IsChecked = false;
            }
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {



            if (class_cmb.SelectedIndex == 0)
            {
                class_cmb.Focus();
                string alertText = "Class Name Should Not Be Blank.";
                MessageBox.Show(alertText);
                return false;
            }
            else if (teacher_cmb.SelectedIndex == 0)
            {

                string alertText = "Teacher Name Should Not Be Blank";
                teacher_cmb.Focus();
                MessageBox.Show(alertText);
                return false;
            }
            else if (subj_name_textbox.Text.Trim().Length == 0)
            {
                subj_name_textbox.Focus();
                string alertText = "Subject Name Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            
            else if (mode == "insert")
            {
                if (check_subject() == true)
                {
                    string alertText = "Subject Name Already Exists, Please Choose A Different Section Name";
                    subj_name_textbox.Focus();
                    MessageBox.Show(alertText);
                    return false;
                }
                return true;
            }


            else
            {
                return true;
            }

        }

        //------------           Check Section Name   -------------------

        bool check_subject()
        {
            foreach (subjects s in subjects_list)
            {
                if (s.subject_name.ToString().ToUpper().Equals(subjects_obj.subject_name.ToString().ToUpper()))
                {
                    if (s.class_name.ToString().ToUpper().Equals(subjects_obj.class_name.ToString().ToUpper()))
                    {
                        return true;
                    }

                }
            }
            return false;
        }





        //---------------           Get All Classes    ----------------------------------

        public void get_all_classes()
        {

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
            catch
            {
                MessageBox.Show("Classes DB not connected");
            }
        }



        //---------------           Get All Employees    ----------------------------------

        public void get_all_emp()
        {
            try
            {

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_emp where is_active='Y'";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                            };
                            emp_list.Add(emp);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Teacher DB not connected");
            }
        }

        //-----------       Get All Subjects Data    ----------------------

        public void get_all_subjects()
        {


            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {



                    cmd.CommandText = "SELECT* FROM sms_subject";
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                save();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            class_cmb.Focus();
            get_all_subjects();
            get_all_classes();
            get_all_emp();

            class_cmb.SelectedIndex = 0;
            teacher_cmb.SelectedIndex = 0;

            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            emp_list.Insert(0, new employees() { emp_name = "---Select Teacher---", id = "-1" });

            class_cmb.ItemsSource = classes_list;
            teacher_cmb.ItemsSource = emp_list;

            if (mode == "edit")
            {
                //class_cmb.IsEnabled = false;
                //section_name_textbox.IsEnabled = false;
                fill_control();
            }
        }
    }
}
