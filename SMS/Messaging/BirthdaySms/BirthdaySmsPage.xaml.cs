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
using MySql.Data.MySqlClient;
using SMS.Models;
using SMS.Upload;
using System.IO;
using SMS.Messaging.BrandedSms;
using SMS.Messaging.SmsOption;
using System.ComponentModel;

namespace SMS.Messaging.BirthdaySms
{
    /// <summary>
    /// Interaction logic for BirthdaySmsPage.xaml
    /// </summary>
    public partial class BirthdaySmsPage : Page
    {
        int count = 0;

        public static string text_message;
        List<admission> adm_list;
        List<admission> std_nos;
        List<classes> classes_list;
        List<sections> sections_list;
        List<sms> sms_list;
        List<friend_list> friends_list;
        public static bool isbranded = false;

        public BirthdaySmsPage()
        {
            InitializeComponent();
            count = 0;
            load_grid();
            strength_textblock.Text = adm_grid.Items.Count.ToString();
        }

        public void load_grid()
        {            
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            attendnce_date.SelectedDate = DateTime.Now;      
            

        }        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            admission adm_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < adm_grid.Items.Count; i++)
            {
                adm_obj = (admission)adm_grid.Items[i];
                adm_obj.Checked = checkBox.IsChecked.Value;
            }
            adm_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            adm_grid.SelectedItem = e.Source;
            admission adm = new admission();
            adm = (admission)adm_grid.SelectedItem;
            foreach (admission ede in adm_list)
            {
                if (adm.id == ede.id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
                }
            }

        }
        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt = (DateTime)attendnce_date.SelectedDate.Value;
            get_all_admissions(dt);
            adm_grid.ItemsSource = adm_list;

        }
        //---------------           Get All Classes    ----------------------------------

        public void get_all_classes()
        {
            classes_list = new List<classes>();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void get_all_admissions(DateTime dt)
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where session_id=" + MainWindow.session.id;
                cmd.Connection = con;
                cmd.Parameters.Add("@dt",MySqlDbType.Date).Value=dt;
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
                            dob = Convert.ToDateTime(reader["dob"]),                          
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),                           
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),                            
                            image = img,
                        };
                        if (adm.dob.Month == dt.Month && adm.dob.Day == dt.Day)
                        {
                            adm_list.Add(adm);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = (DateTime)attendnce_date.SelectedDate;
            admission adm = new admission();
            List<admission> std_nos = new List<admission>();
            string message = "";
            foreach (admission adm_obj in adm_list.Where(x => x.Checked == true))
            {
                message = "Respected Parents, AoA, Happy BirthDay to Dear "+adm_obj.std_name+". May your life be the best. We wish you a successfull future. ";
                message = message + " Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                adm_obj.sms_message = message;
                adm_obj.sms_type = "Birthday";
                std_nos.Add(adm_obj);       
            }
            if (std_nos.Count > 0)
            {
                isbranded = false;
                OptionWindow ow = new OptionWindow();
                ow.ShowDialog();

                if (isbranded == true)
                {
                    BrandedSmsEngine bse = new BrandedSmsEngine(std_nos);
                    bse.Show();
                }
                else
                {
                    UploadWindow uw = new UploadWindow(std_nos,false);
                    uw.Show();
                }

            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student");
            }
        }
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                classes c = (classes)class_cmb.SelectedItem;
                string id = c.id;

                if (class_cmb.SelectedIndex != 0)
                {

                    adm_grid.ItemsSource = adm_list.Where(x => x.class_id == id);

                    get_all_sections(id);


                    section_cmb.IsEnabled = true;
                    sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;
                }
                else
                {

                    section_cmb.IsEnabled = false;
                    section_cmb.SelectedIndex = 0;
                    adm_grid.ItemsSource = adm_list;
                    // load_grid();

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show();
            }
        }

        //------------         Get All Sections   ------------------------
        public void get_all_sections(string id)
        {
            sections_list = new List<sections>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects where class_id = " + id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),


                            };
                            sections_list.Add(section);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("DB not connected");
                    }

                }
            }
        }

        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                sections s = (sections)section_cmb.SelectedItem;
                if (section_cmb.SelectedIndex > 0)
                {

                    adm_grid.ItemsSource = adm_list.Where(x => x.section_id == s.id);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(adm_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = adm_grid.Items.Count.ToString();
        }
        

    }
}
