﻿using System;
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
using SMS.StudentManagement.StudentAttendence;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.StudentManagement.StudentAttendence
{
    /// <summary>
    /// Interaction logic for StudentAttendenceForm.xaml
    /// </summary>
    public partial class StudentAttendenceForm : Window
    {
        StudentAttendence st;
        student_attendence st_obj;
        char att;
        int si_date;
        double total_days = 0;
        double total_abs = 0;
        double total_presents = 0;
        double total_leaves = 0;
        double percentage = 0;
        int index = 0;

        public StudentAttendenceForm(StudentAttendence ST)
        {
            InitializeComponent();
            this.st = ST;
            this.st_obj = st.std_obj;
            if (st_obj != null)
            {
                fill_control();
            }
           // save_btn.IsEnabled = false;
        }

        public void fill_control() 
        {
            std_name_textblock.Text = st_obj.std_name;
            att_cmb.SelectedIndex = 0;

            foreach (DateTime dt in st_obj.att_date_lst)
            {
                att_dates_cmb.Items.Add(dt.ToString("dd MMM"));
            }             
        }

        private void click_save(object sender, RoutedEventArgs e)
        {
            total_abs = 0;
            total_presents = 0;
            total_leaves = 0;
            total_days = 0;
            //count percentage
            if (st_obj != null)
            {
                total_days = Convert.ToInt32(st_obj.total_days);
                total_abs = Convert.ToInt32(st_obj.total_abs);
                total_presents = Convert.ToInt32(st_obj.total_presents);
                total_leaves = Convert.ToInt32(st_obj.total_leaves);
                //-------
                int si = att_cmb.SelectedIndex;
                si_date = att_dates_cmb.SelectedIndex;
                if (si == 0)
                {
                    att = 'P';
                    --total_abs;
                    ++total_presents;
                }
                else if (si == 1)
                {
                    att = 'A';
                    ++total_abs;
                    --total_presents;
                }
                else 
                {
                    att = 'L';
                    ++total_leaves;
                    --total_presents;
                }

                if (total_days != 0)
                {
                    percentage = total_presents / total_days;
                    percentage = percentage * 100;
                }

                //update

                if (update() > 0)
                {
                    if (update_att() > 0)
                    {
                        MessageBox.Show("Successfully Updated");
                        this.Close();
                    }
                }
            }
        }
        public int update()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_student_attendence SET attendence=@att,created_by=@created_by,date_time=@date_time,updation=@updation WHERE std_id = @id && attendence_date=@dt && session_id="+MainWindow.session.id;
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@dt", MySql.Data.MySqlClient.MySqlDbType.Date).Value = st_obj.att_date_lst[si_date];
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st_obj.std_id;
                        cmd.Parameters.Add("@att", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = att;                        
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;

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

        public int update_att()
        {
            int i = 0;
            try
            {               
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_student_attendence SET att_percentage=@att_percentage,total_days=@total_days,total_abs=@total_abs,total_presents=@total_presents, total_leaves=@total_leaves WHERE std_id = @id && session_id=" + MainWindow.session.id;
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = st_obj.std_id;
                            cmd.Parameters.Add("@att_percentage", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = percentage.ToString("0.00");
                            cmd.Parameters.Add("@total_days", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = total_days.ToString();
                            cmd.Parameters.Add("@total_abs", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = total_abs.ToString();
                            cmd.Parameters.Add("@total_presents", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = total_presents.ToString();
                            cmd.Parameters.Add("@total_leaves", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = total_leaves.ToString();

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

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void att_dates_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            att_cmb.IsEnabled = true;
            string dt = (string)att_dates_cmb.SelectedItem;
            for (int i = 0; i < st_obj.att_date_lst.Count;i++ )
            {
                if(st_obj.att_date_lst[i].ToString("dd MMM")== dt)
                {
                    if (st_obj.att_lst[i] == 'P')
                    {
                        att_cmb.SelectedIndex = 0;                        
                    }
                    else if (st_obj.att_lst[i] == 'A')
                    {
                        att_cmb.SelectedIndex = 1;
                    }
                    else 
                    {
                        att_cmb.SelectedIndex = 2;
                    }
                    break;
                }
            }
        }

        private void att_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            save_btn.IsEnabled = false;
            int dsi = att_dates_cmb.SelectedIndex;
            if (dsi >= 0)
            {
                int asi = att_cmb.SelectedIndex;
                char satt;
                char oatt = st_obj.att_lst[dsi];
                if (asi == 0)
                {
                    satt = 'P';
                }
                else if (asi == 1)
                {
                    satt = 'A';
                }
                else 
                {
                    satt = 'L';
                }

                if (satt == oatt)
                {
                    save_btn.IsEnabled = false;
                }
                else
                {
                    save_btn.IsEnabled = true;
                }
            }
          
            
        }
    }
}
