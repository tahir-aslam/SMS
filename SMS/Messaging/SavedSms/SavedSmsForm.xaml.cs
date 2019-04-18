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
using SMS.Messaging.SavedSms;
using MySql.Data.MySqlClient;

namespace SMS.Messaging.SavedSms
{
    /// <summary>
    /// Interaction logic for SavedSmsForm.xaml
    /// </summary>
    public partial class SavedSmsForm : Window
    {
        SavedSmsSearch sss;
        List<sms> sms_list;
        sms sms_obj;
        sms obj;
        string mode;

        public SavedSmsForm(string mode, SavedSmsSearch SSS, sms obj)
        {
            InitializeComponent();

            sss = SSS;
            this.obj = obj;
            this.mode = mode;

            sms_name_textbox.Focus();
            get_all_sms();



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
                        cmd.CommandText = "INSERT INTO sms_saved(msg_name,msg,created_by,date_time) Values(@msg_name,@msg,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@msg_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sms_obj.msg_name;
                        cmd.Parameters.Add("@msg", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sms_obj.msg;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sms_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sms_obj.date_time;


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
                        cmd.CommandText = "Update sms_saved SET msg_name=@msg_name,msg=@msg,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@msg_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sms_obj.msg_name;
                        cmd.Parameters.Add("@msg", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sms_obj.msg;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sms_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sms_obj.date_time;
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

        //-----------       Get All SMS     ----------------------

        public void get_all_sms()
        {
            sms_list = new List<sms>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_saved";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms sm = new sms()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                msg_name = Convert.ToString(reader["msg_name"].ToString()),
                                msg = Convert.ToString(reader["msg"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                            };
                            sms_list.Add(sm);

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
            sms_obj = new sms();

            sms_obj.msg_name = sms_name_textbox.Text.Trim();
            sms_obj.msg = sms_textbox.Text.Trim();
            sms_obj.date_time = DateTime.Now;
            sms_obj.created_by = MainWindow.emp_login_obj.emp_user_name;


        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            sms_name_textbox.Text = obj.msg_name;
            sms_textbox.Text = obj.msg;
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (sms_name_textbox.Text.Trim().Length == 0)
            {
                sms_name_textbox.Focus();
                string alertText = "Message Heading Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (sms_textbox.Text.Trim().Length == 0)
            {
                sms_name_textbox.Focus();
                string alertText = "Message  Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else
            {
                return true;
            }

        }

        //------------           Check Sms heading   -------------------

        bool check_exam()
        {
            foreach (sms s in sms_list)
            {
                if (s.msg_name.ToString().ToUpper().Equals(sms_obj.msg_name.ToString().ToUpper()))
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
                        sss.load_grid();
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
                        sss.load_grid();
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

        private void sms_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            count_text_tb.Text = (459 - sms_textbox.Text.Length).ToString();
            if (sms_textbox.Text.Length <= 160)
            {
                sms_no_tb.Text = "1";
            }
            else if (sms_textbox.Text.Length > 160 && sms_textbox.Text.Length <= 306)
            {
                sms_no_tb.Text = "2";
            }
            else 
            {
                sms_no_tb.Text = "3";
            }
            
        }

        private void v_lineBReak_Click(object sender, RoutedEventArgs e)
        {
            //sms_textbox.Text.cu
        }
    }
}
