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

namespace SMS.ExamsManagement.Subjects
{
    /// <summary>
    /// Interaction logic for SubjectForm.xaml
    /// </summary>
    public partial class SubjectForm : Window
    {
        SubjectSearch SS;        
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
            subj_name_textbox.Focus();

            subjects_list = new List<subjects>();           

            get_all_subjects();           

            if (mode == "edit")
            {
                //class_cmb.IsEnabled = false;
                //section_name_textbox.IsEnabled = false;
                fill_control();
            }
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
                        cmd.CommandText = "INSERT INTO sms_exams_subjects(subject_name, subject_abb, subject_code, created_date_time, updated_date_time) Values(@subject_name, @subject_abb, @subject_code, @created_date_time, @updated_date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@subject_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_name;
                        cmd.Parameters.Add("@subject_abb", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_abb;
                        cmd.Parameters.Add("@subject_code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_code;
                        cmd.Parameters.Add("@created_date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@updated_date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

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
                        cmd.CommandText = "Update sms_exams_subjects SET subject_name=@subject_name, subject_abb=@subject_abb, subject_code=@subject_code, updated_date_time=@updated_date_time   WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;                        
                        cmd.Parameters.Add("@subject_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_name;
                        cmd.Parameters.Add("@subject_abb", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_abb;
                        cmd.Parameters.Add("@subject_code", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_code;
                        cmd.Parameters.Add("@updated_date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

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

            subjects_obj.subject_name = subj_name_textbox.Text.Trim();
            subjects_obj.subject_abb = subj_abb_textbox.Text.Trim();
            subjects_obj.subject_code = subj_code_textbox.Text.Trim();

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
            subj_name_textbox.Text = obj.subject_name;
            subj_abb_textbox.Text = obj.subject_abb;
            subj_code_textbox.Text = obj.subject_code;
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
            if (subj_name_textbox.Text.Trim().Length == 0)
            {
                subj_name_textbox.Focus();
                string alertText = "Subject Name Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (subj_abb_textbox.Text.Trim().Length == 0)
            {
                subj_abb_textbox.Focus();
                string alertText = "Subject Abb Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (subj_code_textbox.Text.Trim().Length == 0)
            {
                subj_code_textbox.Focus();
                string alertText = "Subject Code Should Not Be Blank";
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
                    return true;
                }
            }
            return false;
        }





     
        //-----------       Get All Subjects Data    ----------------------

        public void get_all_subjects()
        {


            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_exams_subjects";
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
                                is_Active = Convert.ToString(reader["is_Active"].ToString()),                                
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

    }
}
