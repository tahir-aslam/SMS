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
using MySql.Data.MySqlClient;
using SMS.Models;

namespace SMS.StudentManagement.StudentAttendence
{
    /// <summary>
    /// Interaction logic for StudentAttendenceInsert.xaml
    /// </summary>
    public partial class StudentAttendenceInsert : Window
    {
        List<DateTime> group_by_att_dates;
        student_attendence sa;

        public StudentAttendenceInsert(student_attendence sa, string sec_id)
        {
            InitializeComponent();
            get_all_attendence(sec_id);            
            std_name_textblock.Text = sa.std_name;
            this.sa = sa;
            foreach (DateTime dt in group_by_att_dates)
            {
                att_dates_cmb.Items.Add(dt.ToString("dd MMM"));
            }             
        }

        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence(string id)
        {            
            group_by_att_dates = new List<DateTime>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_student_attendence  where section_id = @section_id && session_id=" + MainWindow.session.id + " ORDER BY attendence_date DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@section_id", MySqlDbType.String).Value = id.ToString();
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            student_attendence att = new student_attendence()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                section_id = Convert.ToString(reader["section_id"].ToString()),
                                roll_no = Convert.ToString(reader["roll_no"].ToString()),
                                att_percentage = Convert.ToString(reader["att_percentage"].ToString()),
                                total_days = Convert.ToString(reader["total_days"].ToString()),
                                total_abs = Convert.ToString(reader["total_abs"].ToString()),
                                total_presents = Convert.ToString(reader["total_presents"].ToString()),
                                attendence = Convert.ToChar(reader["attendence"]),
                                attendence_date = Convert.ToDateTime(reader["attendence_date"]),
                            };                            

                            if (group_by_att_dates.Exists(x => x.Date == att.attendence_date))
                            {
                            }
                            else
                            {
                                group_by_att_dates.Add(att.attendence_date);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void click_save(object sender, RoutedEventArgs e)
        {
            submit_attendence();
            MessageBox.Show("Successfully Submitted");
            
        }

        public void submit_attendence()
        {
            DateTime dt = group_by_att_dates[att_dates_cmb.SelectedIndex];
            sa.attendence_date = dt.Date;
            if (att_cmb.SelectedIndex == 0)
            {
                sa.attendence = 'P';
            }
            else 
            {
                sa.attendence = 'A';
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "Delete from sms_student_attendence where std_id=" + sa.std_id + " && attendence_date= @dt";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@dt",MySqlDbType.Date).Value=sa.attendence_date;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }

                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_student_attendence(total_presents,total_abs,total_days,att_percentage,std_id,section_id,attendence_date,class_id,roll_no,attendence,std_name,created_by,date_time,session_id)Values(@total_presents,@total_abs,@total_days,@att_percentage,@std_id,@section_id,@attendence_date,@class_id,@roll_no,@attendence,@std_name,@created_by,@date_time,@session_id)";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.class_id;
                            cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.section_id;
                            cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.std_id;
                            cmd.Parameters.Add("@std_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.std_name;
                            cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.roll_no;
                            cmd.Parameters.Add("@attendence_date", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sa.attendence_date;
                            cmd.Parameters.Add("@attendence", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.attendence;
                            cmd.Parameters.Add("@att_percentage", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd.Parameters.Add("@total_days", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd.Parameters.Add("@total_abs", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd.Parameters.Add("@total_presents", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                            cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.created_by;
                            cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

                            con.Open();
                            cmd.ExecuteScalar();
                            con.Close();
                        }
                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
