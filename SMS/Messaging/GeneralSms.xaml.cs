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
using SMS.Messaging.FriendList;
using SMS.Messaging.EmpSms;
using System.ComponentModel;

namespace SMS.Messaging
{
    /// <summary>
    /// Interaction logic for GeneralSms.xaml
    /// </summary>
    public partial class GeneralSms : Page
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

        public GeneralSms()
        {
            InitializeComponent();            
            SearchTextBox.Focus();
            count = 0;
            load_grid();
            strength_textblock.Text = adm_grid.Items.Count.ToString();
            if (MainWindow.ins.isMultiPartSMSAccess == "Y")
            {
                encodedRB.IsEnabled = true;
            }
            else 
            {
                encodedRB.IsEnabled = false;
            }

            string message = "This sms is powered by Scenario Management Solution......."+Environment.NewLine;
            message = message + " Admin " + MainWindow.ins.institute_name + "." + Environment.NewLine
            + MainWindow.ins.institute_cell + Environment.NewLine
            + MainWindow.ins.institute_phone+Environment.NewLine
            + "Principal: "+ MainWindow.ins.institute_owner_name;
            message_textbox.Text = message;
        }

        public void load_grid()
        {
            
            get_all_admissions();
            adm_grid.ItemsSource = adm_list;
            this.adm_grid.Items.Refresh();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            get_all_sms();
            sms_types_cmb.SelectedIndex = 0;
            sms_list.Insert(0, new sms(){msg_name = "---Saved SMS---", id = "-1" });
            sms_types_cmb.ItemsSource = sms_list;
            
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            admission adm_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < adm_grid.Items.Count;i++)
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

        public void get_all_admissions()
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && session_id=" + MainWindow.session.id;
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
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
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
                adm_grid.ItemsSource = adm_list.Where(x => x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
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
            std_nos = new List<admission>();
            admission adm;
            text_message = message_textbox.Text;            
            //foreach(int row = 0; row < adm_grid.Items.Count; row++)
            foreach(admission adms in adm_list.Where(x=>x.Checked == true))
            {
                adm = new admission();
                adm = adms;
                adm.sms_message = message_textbox.Text.Trim();
                adm.sms_type = "General";
                std_nos.Add(adm);
            }

            if (frnd_chkbox.IsChecked == true)
            {
                get_all_friends();               
                foreach (friend_list fl in FriendListSelectedWindow.friends_list.Where(x=>x.Checked == true))
                {
                    adm = new admission();
                    adm.id = fl.id;
                    adm.std_name = fl.friend_name;
                    adm.cell_no = fl.friend_cell;
                    adm.sms_message = text_message;
                    adm.father_name = fl.friend_occupation;
                    adm.sms_type = "General";
                    adm.class_name = "Friends";
                    adm.section_name = "Friends";
                    std_nos.Add(adm);
                }
            }
            if (emp_chkbox.IsChecked == true)
            {               
                foreach (employees fl in EmpSmsWindow.emp_list.Where(x => x.Checked == true))
                {
                    adm = new admission();
                    adm.id = fl.id;
                    adm.std_name = fl.emp_name;
                    adm.cell_no = fl.emp_cell;
                    adm.sms_message = text_message;
                    adm.father_name =  fl.emp_father;
                    adm.sms_type = "General";
                    adm.class_name = "Employee";
                    adm.section_name = "Employee";
                    std_nos.Add(adm);
                }

            }
            MessageBoxResult mbr = MessageBox.Show("Do You Want To Send    " + std_nos.Count + "  SMS ?", "Send Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (mbr == MessageBoxResult.Yes)
            {
                if (std_nos.Count > 0)
                {
                    OptionWindow ow = new OptionWindow();
                    ow.ShowDialog();

                    if (isbranded == true)
                    {
                        if (encodedRB.IsChecked == true)
                        {
                            BrandedSmsEngine bse = new BrandedSmsEngine(std_nos, true);
                            bse.Show();
                        }
                        else
                        {
                            BrandedSmsEngine bse = new BrandedSmsEngine(std_nos, false);
                            bse.Show();
                        }
                    }
                    else
                    {
                        if (encodedRB.IsChecked == true)
                        {
                            UploadWindow uw = new UploadWindow(std_nos, true);
                            uw.Show();
                        }
                        else 
                        {
                            UploadWindow uw = new UploadWindow(std_nos, false);
                            uw.Show();
                        }
                        
                        
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Minimum One Student");
                }              
            }
        }

        //-----------       Get All Friends     ----------------------
        public void get_all_friends()
        {
            friends_list = new List<friend_list>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_friend_list";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            friend_list fl = new friend_list()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                friend_name = Convert.ToString(reader["friend_name"].ToString()),
                                friend_cell = Convert.ToString(reader["friend_cell"].ToString()),
                                friend_occupation = Convert.ToString(reader["friend_occupation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                            };
                            friends_list.Add(fl);

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

        private void message_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            count_text.Text = (459 - message_textbox.Text.Length).ToString();
            if (message_textbox.Text.Length <= 160)
            {
                sms_no_tb.Text = "1";
            }
            else if (message_textbox.Text.Length > 160 && message_textbox.Text.Length <= 306)
            {
                sms_no_tb.Text = "2";
            }
            else
            {
                sms_no_tb.Text = "3";
            }
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

        private void sms_types_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sms_types_cmb.SelectedIndex != 0)
            {
                sms s = (sms)sms_types_cmb.SelectedItem;
                foreach(sms sm in sms_list)
                {
                    if(sm.id == s.id)
                    {
                        message_textbox.Text = sm.msg;
                        break;
                    }
                }
            }
            else 
            {
                message_textbox.Text = "This Sms Is Powered By Scenario Management Solution......";
            }
        }

        private void frnd_chkbox_Checked(object sender, RoutedEventArgs e)
        {
            if (frnd_chkbox.IsChecked == true)
            {
                FriendListSelectedWindow flsw = new FriendListSelectedWindow(this);
                flsw.ShowDialog();
            }
            else 
            {
                friends_count.Text = "0";
                foreach (friend_list fl in FriendListSelectedWindow.friends_list.Where(x => x.Checked == true))
                {
                    fl.Checked=false;
                }
            }

        }

        private void emp_chkbox_Checked(object sender, RoutedEventArgs e)
        {
            if (emp_chkbox.IsChecked == true)
            {
                EmpSmsWindow esw = new EmpSmsWindow(this);
                esw.ShowDialog();
            }
            else
            {
                emp_count.Text = "0";
                foreach (employees emp in EmpSmsWindow.emp_list.Where(x => x.Checked == true))
                {
                    emp.Checked = false;
                }
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

        private void frnd_chkbox_Click(object sender, RoutedEventArgs e)
        {            
        }

    }
}
