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
using System.Windows.Media.Animation;
using SMS.Web;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for SchoolWeb.xaml
    /// </summary>
    public partial class SchoolWeb : Window
    {

        string notification_text="";

        public SchoolWeb()
        {
            InitializeComponent();
            load_grid();
            institute_name_lbl.Content = MainWindow.ins.institute_name;
        
        }

        public void load_grid() 
        {
            this.web_frame.Navigate(new home());
            get_notification();
            tbmarquee.Text = notification_text;
            notification();
        }

        public void notification() 
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:10"));
            tbmarquee.BeginAnimation(Canvas.RightProperty, doubleAnimation);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            notification();
        }


        private void home_btn_Click(object sender, RoutedEventArgs e)
        {
            this.web_frame.Navigate(new home());
        }

        private void notification_edit_btn_Click(object sender, RoutedEventArgs e)
        {
            EditNotification en = new EditNotification(tbmarquee.Text,this);
            en.ShowDialog();
        }

        public void get_notification() 
        {
            try
            {
                using(MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using(MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_notification";
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            notification_text = Convert.ToString(reader["notification"].ToString());
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void news_btn_Click(object sender, RoutedEventArgs e)
        {
            this.web_frame.Navigate(new NewsMessage());
        }

        private void event_btn_Click(object sender, RoutedEventArgs e)
        {
            this.web_frame.Navigate(new SchoolEvents());
        }

    }
}
