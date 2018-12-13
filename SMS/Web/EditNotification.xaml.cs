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

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for EditNotification.xaml
    /// </summary>
    public partial class EditNotification : Window
    {
        string notification;
        SchoolWeb SW;
        public EditNotification(string n , SchoolWeb sw)
        {
            InitializeComponent();
            notification = n;
            notification_textbox.Text = n;
            notification_textbox.Focus();
            SW = sw;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (update() > 0)
            {
                SW.load_grid();
                this.Close();               
            }
            else
            {
                MessageBox.Show("OOPs! There's some thing wrong, Please try again");
            }
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
                        cmd.CommandText = "Update web_notification SET notification=@notification,created_by=@created_by,date_time=@date_time,updation=@updation ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@notification", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = notification_textbox.Text;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
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
    }
}
