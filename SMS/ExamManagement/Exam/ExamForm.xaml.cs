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
using SMS.ExamManagement.Exam;
using MySql.Data.MySqlClient;

namespace SMS.ExamManagement.Exam
{
    /// <summary>
    /// Interaction logic for ExamForm.xaml
    /// </summary>
    public partial class ExamForm : Window
    {
        ExamSearch es;        
        List<exam> exam_list;
        exam exam_obj;
        exam obj;
        string mode;

        public ExamForm(string mode, ExamSearch ES, exam obj)
        {
            InitializeComponent();

            es = ES;
            this.obj = obj;
            this.mode = mode;

            exam_name_textbox.Focus();            
            get_all_exams();

            

            if (mode == "edit")
            {                
                fill_control();
            }
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //--------------          Click_Save             -----------------------------------

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        //---------------           Submit Form    ----------------------------------

        public int submit()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_exam(exam_name,exam_date,created_by,date_time,session_id) Values(@exam_name,@exam_date,@created_by,@date_time,@session_id)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd.Parameters.Add("@exam_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = exam_obj.exam_name;
                        cmd.Parameters.Add("@exam_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = exam_obj.exam_date;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = exam_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = exam_obj.date_time;


                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

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
                        cmd.CommandText = "Update sms_exam SET exam_name=@exam_name,exam_date=@exam_date,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id && session_id="+MainWindow.session.id;
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@exam_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = exam_obj.exam_name;
                        cmd.Parameters.Add("@exam_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = exam_obj.exam_date;                        
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = exam_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = exam_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //-----------       Get All Exam Data    ----------------------

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

        //------------------    Fill Object      ------------------------

        public void fill_object()
        {
            exam_obj = new exam();

            exam_obj.exam_name = exam_name_textbox.Text.Trim();
            if(exam_date_textbox.SelectedDate != null)
            {
                exam_obj.exam_date = exam_date_textbox.SelectedDate.Value;
            }
            
            exam_obj.date_time = DateTime.Now;
            exam_obj.created_by = MainWindow.emp_login_obj.emp_user_name;

           
        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            exam_name_textbox.Text = obj.exam_name;
            exam_date_textbox.SelectedDate = obj.exam_date;

        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (exam_name_textbox.Text.Trim().Length == 0)
            {
                exam_name_textbox.Focus();
                string alertText = "Exam Name Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
           else if (exam_date_textbox.SelectedDate == null)
            {
                exam_date_textbox.Focus();
                string alertText = "Exam Date Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else
            {
                return true;
            }

        }

        //------------           Check Exam Name   -------------------

        bool check_exam()
        {
            foreach (exam s in exam_list)
            {
                if (s.exam_name.ToString().ToUpper().Equals(exam_obj.exam_name.ToString().ToUpper()))
                {                   
                        return true;
                }
            }
            return false;
        }


        //--------------           Save          ----------------------

        public void save()
        {
            fill_object();
            if (validate())
            {
                if (mode == "insert")
                {
                    if (submit() > 0)
                    {
                        MessageBox.Show("Record Added Successfully");
                        this.Close();
                        es.load_grid();
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
                        MessageBox.Show("Record Updated Successfully");
                        this.Close();
                        es.load_grid();
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


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();
            }

        }
    }
}
