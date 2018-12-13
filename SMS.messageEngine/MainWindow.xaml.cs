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
using System.IO.Ports;
using MySql.Data.MySqlClient;
using System.IO;
using SMS.messageEngine.Models;

namespace SMS.messageEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        List<admission> adm_list;
        List<admission> std_nos;

        public MainWindow()
        {
            InitializeComponent();

            adm_list = new List<admission>();
            SearchTextBox.Focus();
            load_grid();
            try
            {
                this.port = objclsSMS.OpenPort("COM15", 115200, 8, 300, 300);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MainWindow))
            //    {
            //        (window as MainWindow).smsSend.IsEnabled = false;
            //    }
            //}
        }


        public void load_grid()
        {
            adm_list.Clear();
            get_all_admissions();
            adm_grid.ItemsSource = adm_list;
            this.adm_grid.Items.Refresh();
        }

        public void get_all_admissions()
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission";
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
                            phone_no = Convert.ToString(reader["phone_no"].ToString()),
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            if (by_name.IsChecked == true)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (by_roll_no.IsChecked == true)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.roll_no.ToUpper().StartsWith(v_search.ToUpper()) || x.roll_no.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }

        }

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            sms_prog_bar.Visibility = Visibility.Visible;
            std_nos = new List<admission>();
            for (int row = 0; row < adm_grid.Items.Count; row++)
            {
                admission adm = new admission();
                adm = (admission)adm_grid.Items[row];
                std_nos.Add(adm);
            }
            try
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Send    " + std_nos.Count + "  SMS ?", "Send Confirmation", MessageBoxButton.YesNo);

                if (mbr == MessageBoxResult.Yes)
                {

                    //Task.Factory.StartNew(() => objclsSMS.sendMsg(this.port, std_nos, message_textbox.Text));
                    //MessageBox.Show("sent successfully");
                    if (objclsSMS.sendMsg(this.port, std_nos, message_textbox.Text))
                    {

                        //MessageBox.Show("Message has sent successfully");
                        sms_prog_bar.Visibility = Visibility.Hidden;
                        MessageBox.Show("Message has sent successfully");


                    }
                    else
                    {
                        //MessageBox.Show("Failed to send message");
                        sms_prog_bar.Visibility = Visibility.Hidden;
                        MessageBox.Show("Failed to send message main");


                    }
                }
                else
                {
                    sms_prog_bar.Visibility = Visibility.Hidden;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void disc_btn_Click(object sender, RoutedEventArgs e)
        {
            objclsSMS.ClosePort(this.port);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            objclsSMS.ClosePort(this.port);
            //foreach (Window window in Application.Current.Windows)
            //{
            //    if (window.GetType() == typeof(MainScreen.MainScreen))
            //    {
            //        (window as MainScreen.MainScreen).smsSend.IsEnabled = true;
            //    }
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
