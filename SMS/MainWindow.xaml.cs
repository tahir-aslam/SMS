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
using SMS.MainScreen;
using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using SMS.Models;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Configuration;
using SMS.DAL;
using System.Globalization;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net.Cache;

namespace SMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Server { get; set; }
        public static string Port { get; set; }
        public static string Uid { get; set; }

        public static institute ins;
        public static session session;
        public static sms_exam_admin_panel examAdminPanel;
        public static sms_fee_admin_panel feeAdminPanel;
        public static sms_fees_admin_panel feesAdminPanel;
        public static string sms;
        public static string web_sms;
        List<emp_login> emp_login_list;
        public static emp_login emp_login_obj;
        public static List<roles> roles_list;
        public static List<roles> basic_roles_list;
        List<session> session_list;

        //new fees
        public static List<sms_fees_category> fees_category_list;
        public static List<sms_fees_sub_category> fees_sub_category_list;
        public static List<sms_years> years_list;
        public static List<sms_fees_package> fees_package_list;
        public static List<sms_months> months_list;

        //admission
        public static List<prefixNo> adm_no_prefix_list;
        public static List<prefixNo> roll_no_prefix_list;
        public static List<CityArea> area_list;
        public static List<Databases> database_list;

        //Default Values
        // default d
        public static int d_FeeCollectionByVocherCollectionPlace = 0;
        public static int d_FeeCollectionByAmountCollectionPlace = 0;
        public static int d_FeeCollectionByFeeRegisterCollectionPlace = 0;

        FeesDAL feesDAL;
        MiscDAL miscDAL;
        RolesDAL rolesDAL;


        private static string _Database;
        public static string Database
        {
            get
            {
                return _Database;
            }
            set
            {
                _Database = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Database = "sms";
                Server = "localhost";
                Port = "3306";
                Uid = "root";

                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                ReadDatabaseFile();
                usr_name.Focus();
                get_sms_institute();
                institute_name_lbl.Content = ins.institute_name;
                institute_logo_img.Source = ByteToImage(ins.institute_logo);
                //check();
                //var c1 = ConfigurationManager.ConnectionStrings["sms"].ConnectionString;
                //var c2 = ConfigurationManager.ConnectionStrings["web_sms"].ConnectionString;
                //sms = c1;
                //web_sms = c2;            
                get_exam_admin_panel();
                get_fee_admin_panel();
                get_fees_admin_panel();


                //new fees  
                feesDAL = new FeesDAL();
                miscDAL = new MiscDAL();
                rolesDAL = new RolesDAL();

                get_all_sessions();
                session_cmb.ItemsSource = session_list;
                session_cmb.SelectedIndex = session_list.Count - 1;

                database_list = miscDAL.get_all_databases();
                database_cmb.ItemsSource = database_list;
                database_cmb.SelectedIndex = database_list.IndexOf(database_list.Where(x => x.DatabaseName == Database).First());


                fees_category_list = feesDAL.get_all_fees_category();
                fees_sub_category_list = feesDAL.get_all_fees_sub_category();
                years_list = miscDAL.get_all_years();

                fees_package_list = feesDAL.getAllFeesPackage();
                fees_package_list.Insert(0, new sms_fees_package() { id = -1, package_name = "-Select Package-" });

                months_list = miscDAL.get_all_months();
                months_list.Insert(0, new sms_months() { id = "-1", month_name = "-Select Month-" });

                adm_no_prefix_list = miscDAL.get_all_adm_no_prefix();
                roll_no_prefix_list = miscDAL.get_all_roll_no_prefix();
                area_list = miscDAL.get_all_area();



                basic_roles_list = rolesDAL.get_all_roles();

                //WriteLogFile();
                //ReadLogFile();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        public void get_exam_admin_panel()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_exam_admin_panel";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        byte[] teacher_img;
                        byte[] parent_img;
                        byte[] principal_img;

                        while (reader.Read())
                        {
                            if (reader["teacher_sig_image"] == "" || reader["teacher_sig_image"] == null)
                            {
                                string path = "/SMS;component/images/Delete-icon.png";
                                teacher_img = File.ReadAllBytes(path);
                            }
                            else
                            {
                                teacher_img = (byte[])(reader["teacher_sig_image"]);
                            }

                            if (reader["principal_sig_image"] == "" || reader["principal_sig_image"] == null)
                            {
                                string path = "/SMS;component/images/Delete-icon.png";
                                principal_img = File.ReadAllBytes(path);
                            }
                            else
                            {
                                principal_img = (byte[])(reader["principal_sig_image"]);
                            }

                            if (reader["parents_sig_image"] == "" || reader["parents_sig_image"] == null)
                            {
                                string path = "/SMS;component/images/Delete-icon.png";
                                parent_img = File.ReadAllBytes(path);
                            }
                            else
                            {
                                parent_img = (byte[])(reader["parents_sig_image"]);
                            }

                            examAdminPanel = new sms_exam_admin_panel()
                            {
                                position_visibility = Convert.ToString(reader["position_visibility"]),
                                position_text_visibility = Convert.ToString(reader["position_text_visibility"]),
                                position_percentage = Convert.ToString(reader["position_percentage"]),
                                position_limit = Convert.ToString(reader["position_limit"]),
                                attendance_visibility = Convert.ToString(reader["attendance_visibility"]),
                                attendance_text_visibility = Convert.ToString(reader["attendance_text_visibility"]),
                                image_visibility = Convert.ToString(reader["image_visibility"]),
                                remarks_visibility = Convert.ToString(reader["remarks_visibility"]),
                                remarks_text_visibility = Convert.ToString(reader["remarks_text_visibility"]),
                                parents_visibility = Convert.ToString(reader["parents_visibility"]),
                                teacher_visibility = Convert.ToString(reader["teacher_visibility"]),
                                principal_visibility = Convert.ToString(reader["principal_visibility"]),
                                teacher_sig_text = Convert.ToString(reader["teacher_sig_text"]),
                                principal_sig_text = Convert.ToString(reader["principal_sig_text"]),
                                parents_sig_text = Convert.ToString(reader["parents_sig_text"]),
                                teacher_sig_image = teacher_img,
                                principal_sig_image = principal_img,
                                parents_sig_image = parent_img,
                            };

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void get_fee_admin_panel()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fee_admin_panel";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            feeAdminPanel = new sms_fee_admin_panel()
                            {
                                adm_fee = Convert.ToString(reader["adm_fee"]),
                                exam_fee = Convert.ToString(reader["exam_fee"]),
                                other_fee = Convert.ToString(reader["other_fee"]),
                                reg_fee = Convert.ToString(reader["reg_fee"]),
                                tution_fee = Convert.ToString(reader["tution_fee"]),
                                security_fee = Convert.ToString(reader["security_fee"]),
                                fine_fee = Convert.ToString(reader["fine_fee"]),
                                cancel_challan = Convert.ToString(reader["cancel_challan"]),
                                pay_cash_btn = Convert.ToString(reader["pay_cash_btn"]),
                                waveoff = Convert.ToString(reader["waveoff"]),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void get_fees_admin_panel()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_admin_panel";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            feesAdminPanel = new sms_fees_admin_panel()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                bank_logo = (byte[])(reader["bank_logo"]),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void get_all_sessions()
        {
            session_list = new List<session>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_sessions";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            session ses = new session()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                session_name = Convert.ToString(reader["session_name"].ToString()),
                                session_start = Convert.ToDateTime(reader["session_start"]),
                            };
                            session_list.Add(ses);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void check()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            bool check = false;
            foreach (NetworkInterface adapter in nics)
            {

                IPInterfaceProperties properties = adapter.GetIPProperties();
                sMacAddress = adapter.GetPhysicalAddress().ToString();
                if (ins.mac == sMacAddress)
                {
                    check = false;
                }


            }
            if (check == false)
            {
            }
            else
            {
                MessageBox.Show("Application Is Not Licensed");
                Environment.Exit(0);
            }
            //if (ins.mac != sMacAddress)
            //{
            //    MessageBox.Show("Application Is Not Licensed");
            //    Environment.Exit(0);
            //}
            //else 
            //{

            //}
        }

        public static void get_sms_institute()
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_institute";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();

                    Byte[] institute_logo;
                    Byte[] male_image;
                    Byte[] female_image;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    institute_logo = (byte[])(reader["institute_logo"]);
                    male_image = (byte[])(reader["male_image"]);
                    female_image = (byte[])(reader["female_image"]);


                    ins = new institute()
                    {
                        institute_id = Convert.ToInt32(reader["institute_id"]),
                        institute_name = Convert.ToString(reader["institute_name"]),
                        institute_cell = Convert.ToString(reader["institute_cell"]),
                        institute_address = Convert.ToString(reader["institute_address"]),
                        institute_owner_cell = Convert.ToString(reader["institute_owner_cell"]),
                        institute_owner_name = Convert.ToString(reader["institute_owner_name"]),
                        institute_phone = Convert.ToString(reader["institute_phone"]),
                        institute_quote = Convert.ToString(reader["institute_quote"]),
                        expiry_date = Convert.ToDateTime(reader["expiry_date"]),
                        expiry_warning_day = Convert.ToInt32(reader["expiry_warning_day"]),
                        expiry_message = Convert.ToString(reader["expiry_message"]),
                        expiry_warning_message = Convert.ToString(reader["expiry_warning_message"]),
                        expiry_instant = Convert.ToString(reader["expiry_instant"]),
                        installation_date = Convert.ToDateTime(reader["installation_date"]),

                        institute_logo = institute_logo,
                        male_image = male_image,
                        female_image = female_image,
                        mac = Convert.ToString(reader["mac"].ToString()),
                        isMultiPartSMSAccess = Convert.ToString(reader["isMultiPartSMSAccess"]),
                    };

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Environment.Exit(0);
                }
            }
        }


        private void save_click(object sender, RoutedEventArgs e)
        {
            save();
        }
        public void save()
        {
            try
            {
                get_all_emp_login();
                string uid = usr_name.Text.Trim();
                string pwd = usr_pwd.Password;

                if (uid != "" && pwd != "")
                {
                    if (check(uid, pwd))
                    {
                        session = (session)session_cmb.SelectedItem;
                        get_all_roles_assignment();

                        MainScreen.MainScreen main = new MainScreen.MainScreen();
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("UserName Or Password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    usr_name.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Main Window Exception " + ex.Message);
            }
        }

        //-------Get All Roles Assignment ---------

        public void get_all_roles_assignment()
        {
            roles_list = new List<roles>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from sms_roles_assignment where emp_id =" + emp_login_obj.emp_id;
                        //cmd.Parameters.Add("id", MySqlDbType.VarChar).Value = obj.emp_id;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            roles rol = new roles()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                module_name = Convert.ToString(reader["module_name"]),
                                module_pid = Convert.ToString(reader["module_pid"]),
                                module_id = Convert.ToString(reader["module_id"]),
                            };
                            roles_list.Add(rol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        public bool check(string uid, string pwd)
        {
            foreach (emp_login el in emp_login_list)
            {
                if (el.emp_user_name == uid && el.emp_pwd == pwd)
                {
                    emp_login_obj = new emp_login();
                    emp_login_obj = el;
                    return true;
                }
            }
            return false;
        }

        //-----------       Get All Employee Login    ----------------------

        public void get_all_emp_login()
        {
            emp_login_list = new List<emp_login>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_emp_login";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            emp_login el = new emp_login()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_user_name = Convert.ToString(reader["emp_user_name"].ToString()),
                                emp_pwd = Convert.ToString(reader["emp_pwd"].ToString()),
                                branded_url = Convert.ToString(reader["branded_url"].ToString()),
                                branded_url_encoded = Convert.ToString(reader["branded_url_encoded"].ToString()),
                                branded_user_name = Convert.ToString(reader["branded_user_name"].ToString()),
                                branded_pwd = Convert.ToString(reader["branded_pwd"].ToString()),
                                branded_name = Convert.ToString(reader["branded_name"].ToString()),
                                branded_check_remaining_url = Convert.ToString(reader["branded_check_remaining_url"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            emp_login_list.Add(el);

                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Environment.Exit(0);
                    }

                }
            }
        }


        private void click_cancel(object sender, RoutedEventArgs e)
        {
            usr_name.Text = "";
            usr_pwd.Password = "";
            usr_name.Focus();


        }

        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            try
            {

                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                save();
            }

        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static string getIpAddress()
        {
            string ipAddress = "";
            ipAddress = new WebClient().DownloadString("http://icanhazip.com");

            return ipAddress;
        }
        //public static DateTime GetNistTime()
        //{
        //    DateTime dt = DateTime.Today;
        //    try
        //    {
        //        var client = new TcpClient("time.nist.gov", 13);
        //        using (var streamReader = new StreamReader(client.GetStream()))
        //        {
        //            var response = streamReader.ReadToEnd();
        //            var utcDateTimeString = response.Substring(7, 17);
        //            dt=  DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        //        }

        //        //var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
        //        //var response = myHttpWebRequest.GetResponse();
        //        //string todaysDates = response.Headers["date"];
        //        //return DateTime.ParseExact(todaysDates,
        //        //                           "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
        //        //                           CultureInfo.InvariantCulture.DateTimeFormat,
        //        //                           DateTimeStyles.AssumeUniversal);
        //    }
        //    catch (Exception ex) 
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    return dt;
        //}

        public static DateTime GetNistTime()
        {
            DateTime dt = new DateTime();
            try
            {
                var myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.microsoft.com");
                var response = myHttpWebRequest.GetResponse();
                string todaysDates = response.Headers["date"];
                return DateTime.ParseExact(todaysDates,
                                           "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                           CultureInfo.InvariantCulture.DateTimeFormat,
                                           DateTimeStyles.AssumeUniversal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dt;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (CheckForInternetConnection())
            //{
            //    if (GetNistTime().Date.Equals(DateTime.Now.Date))
            //    {

            //    }
            //    else
            //    {
            //        MessageBox.Show("Please Correct Your System Date Time", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    }
            //}
        }

        void WriteLogFile()
        {
            //string sDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyApplicationDir");

            String File_name = "C:\\log.txt";

            string line = "";
            using (StreamReader sr = new StreamReader(File_name))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }
        void ReadLogFile()
        {
            string sDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyApplicationDir");

            if (Directory.Exists(sDirectory))
            {
                StreamReader reader = new StreamReader(sDirectory);
                reader.ReadToEnd();
            }
        }

        void ReadDatabaseFile()
        {
            string line;
            int i = 0;
            var fileName = "Database.txt";
            var spFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var folderPath = Path.Combine(spFolderPath, "ScenarioSystems", "SMS");
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                //using (StreamReader sr = new StreamReader(spFolderPath + "Database.txt"))
                using (StreamReader sr = new StreamReader(filePath))

                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 0 && line.Trim() != "")
                        {
                            Server = line;
                        }
                        if (i == 1 && line.Trim() != "")
                        {
                            Port = line;
                        }
                        if (i == 2 && line.Trim() != "")
                        {
                            Database = line;
                        }
                        if (i == 3 && line.Trim() != "")
                        {
                            Uid = line;
                        }
                        i++;
                    }

                }
            }
            catch (Exception e)
            {
                Directory.CreateDirectory(folderPath);
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(folderPath, fileName)))
                {
                    outputFile.WriteLine("localhost");
                    outputFile.WriteLine("3306");
                    outputFile.WriteLine("sms");
                    outputFile.WriteLine("root");
                }
            }
        }

        private void help_button_Click(object sender, RoutedEventArgs e)
        {
            if (database_cmb.IsEnabled)
            {
                database_cmb.IsEnabled = false;
            }
            else
            {
                database_cmb.IsEnabled = true;
            }
        }

        private void database_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (database_cmb.SelectedItem != null)
            {
                Databases obj = database_cmb.SelectedItem as Databases;
                Database = obj.DatabaseName;
                Connection_String.con_string = "Server=" + Server + "; port=" + Port + "; Database=" + Database + "; Uid=" + Uid + "; Pwd=7120020@123; default command timeout=99999;CHARSET=utf8";
                get_sms_institute();
                institute_name_lbl.Content = ins.institute_name;
                institute_logo_img.Source = ByteToImage(ins.institute_logo);
            }
        }
        public static string GetSelectedDatabaseName()
        {
            return Database;
        }
    }
}
