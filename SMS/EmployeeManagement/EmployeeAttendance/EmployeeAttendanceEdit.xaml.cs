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
using MySql.Data.MySqlClient;

namespace SMS.EmployeeManagement.EmployeeAttendance
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceEdit.xaml
    /// </summary>
    public partial class EmployeeAttendanceEdit : Window
    {
        List<DateTime> group_by_att_dates;
        employee_attendence sa;
        public EmployeeAttendanceEdit(employee_attendence ea)
        {
            InitializeComponent();
            emp_name_textblock.Text = ea.emp_name;
            get_all_attendence();
            this.sa = ea;
            foreach (DateTime dt in group_by_att_dates)
            {
                att_dates_cmb.Items.Add(dt.ToString("dd MMM"));
            }     
        }

        private void click_save(object sender, RoutedEventArgs e)
        {
            submit_attendence();
            MessageBox.Show("Successsfully Submitted","Yes",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                        cmd.CommandText = "INSERT INTO sms_emp_attendence(total_presents,total_abs,total_days,att_percentage,emp_id,attendence_date,attendence,emp_name,created_by,date_time)Values(@total_presents,@total_abs,@total_days,@att_percentage,@emp_id,@attendence_date,@attendence,@emp_name,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add("@attendence_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = sa.attendence_date;
                        cmd.Parameters.Add("@attendence", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.attendence;
                        cmd.Parameters.Add("@att_percentage", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@total_days", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@total_abs", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@total_presents", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.emp_id;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sa.emp_name;

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

        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence()
        {            
            group_by_att_dates = new List<DateTime>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_emp_attendence ORDER BY attendence_date DESC";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            employee_attendence att = new employee_attendence()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
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
    }
}
