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
using SMS.Models;
using MySql.Data.MySqlClient;
using System.IO;
using SMS.Upload;
using SMS.Messaging.SmsOption;
using SMS.Messaging.BrandedSms;
using System.ComponentModel;

namespace SMS.Messaging
{
    /// <summary>
    /// Interaction logic for FeePaidSms.xaml
    /// </summary>
    public partial class FeePaidSms : Page
    {
        public List<fee> paid_fee_list;
        string months;
        List<fee_history> fee_history_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        fee_history fh;
        int total_fee_paid = 0;
        string class_id = "0";
        DateTime dt;
        List<admission> std_nos_list;
        string message;
        fee f;
        List<fee> total_paid_list;
        public static bool isbranded = false;

        public FeePaidSms()
        {
            InitializeComponent();

            section_cmb.IsEnabled = false;
            class_cmb.IsEnabled = false;
            fill_month_cmb();
            month_cmb.SelectedIndex = 0;
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            get_all_admissions();
            strength_textblock.Text = paid_fee_grid.Items.Count.ToString();
            date_picker.SelectedDate = DateTime.Now;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            fee f;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < paid_fee_grid.Items.Count; i++)
            {
                f = (fee)paid_fee_grid.Items[i];
                f.Checked = checkBox.IsChecked.Value;
            }
            paid_fee_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            paid_fee_grid.SelectedItem = e.Source;
            fee f_obj = new fee();
            f_obj = (fee)paid_fee_grid.SelectedItem;
            foreach (fee ede in paid_fee_list)
            {
                if (ede.id == f_obj.id)
                {
                    f_obj.Checked = checkBox.IsChecked.Value;
                }
            }

        }
        // =======      Fill month cmb        =============================
        public void fill_month_cmb()
        {
            month_cmb.Items.Add("---Select Month---");
            month_cmb.Items.Add("April");
            month_cmb.Items.Add("May");
            month_cmb.Items.Add("June");
            month_cmb.Items.Add("July");
            month_cmb.Items.Add("August");
            month_cmb.Items.Add("September");
            month_cmb.Items.Add("November");
            month_cmb.Items.Add("December");
            month_cmb.Items.Add("January");
            month_cmb.Items.Add("February");
            month_cmb.Items.Add("March");
        }

        //========      Get Fee History       =============================

        public void get_fee_history()
        {
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where month =@months && session_id="+MainWindow.session.id;
                        cmd.Parameters.Add("@months", MySqlDbType.VarChar).Value = months;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee paid_fee = new fee()
                            {
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                            };
                            paid_fee_list.Add(paid_fee);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        // ===============     Get All Admissions          ================

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
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
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
        
        private void click_edit(object sender, RoutedEventArgs e)
        {

        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (month_cmb.SelectedIndex != 0)
            {
                date_picker.SelectedDate = null;
                class_cmb.IsEnabled = true;
                class_cmb.SelectedIndex = 0;
                total_fee_paid = 0;
                paid_fee_grid.ItemsSource = null;
                months = month_cmb.SelectedItem.ToString();
                get_fee_history();
                set_paid_list();
                paid_fee_grid.ItemsSource = paid_fee_list;
                strength_textblock.Text = paid_fee_grid.Items.Count.ToString();
            }
            else
            {
                class_cmb.IsEnabled = false;
                section_cmb.IsEnabled = false;
                class_cmb.SelectedIndex = 0;


                
                total_fee_paid = 0;
                paid_fee_grid.ItemsSource = null;
            }

        }

        public void set_paid_list()
        {
            int total = 0;
            total_fee_paid = 0;
            total_paid_list = new List<fee>();
            List<string> months_list;
            List<string> receipt_list;
            fee total_paid_obj;


            int regFee = 0;
            int admFee = 0;
            int secFee = 0;
            int examFee = 0;
            int tutionFee = 0;
            int otherFee = 0;
            int fineFee = 0;

            
                foreach (var id in paid_fee_list.Select(x => x.std_id).Distinct())
                {
                    regFee = 0;
                    admFee = 0;
                    secFee = 0;
                    examFee = 0;
                    tutionFee = 0;
                    otherFee = 0;
                    fineFee = 0;
                    total_paid_obj = new fee();
                    months_list = new List<string>();
                    receipt_list = new List<string>();

                    foreach (fee fee in paid_fee_list.Where(x => x.std_id == id))
                    {
                        regFee = regFee + Convert.ToInt32(fee.reg_fee);
                        admFee = admFee + Convert.ToInt32(fee.adm_fee);
                        secFee = secFee + Convert.ToInt32(fee.security_fee);
                        examFee = examFee + Convert.ToInt32(fee.exam_fee);
                        tutionFee = tutionFee + Convert.ToInt32(fee.tution_fee);
                        otherFee = otherFee + Convert.ToInt32(fee.other_expenses);
                        fineFee = fineFee + Convert.ToInt32(fee.fine_fee);

                        months_list.Add(fee.month);
                        receipt_list.Add(fee.receipt_no);
                        total_paid_obj = fee;
                    }

                    total_paid_obj.total_paid = (regFee + admFee + secFee + examFee + tutionFee + otherFee + fineFee).ToString();
                    total_paid_obj.reg_fee = regFee.ToString();
                    total_paid_obj.adm_fee = admFee.ToString();
                    total_paid_obj.security_fee = secFee.ToString();
                    total_paid_obj.exam_fee = examFee.ToString();
                    total_paid_obj.tution_fee = tutionFee.ToString();
                    total_paid_obj.other_expenses = otherFee.ToString();
                    total_paid_obj.fine_fee = fineFee.ToString();

                    //months
                    total_paid_obj.month = "";
                    foreach (string month in months_list.Distinct())
                    {
                        total_paid_obj.month = total_paid_obj.month + " " + month;
                    }

                    //receipt
                    total_paid_obj.receipt_no = "";
                    foreach (string receipt in receipt_list.Distinct())
                    {
                        total_paid_obj.receipt_no = total_paid_obj.receipt_no + " " + receipt;
                    }

                    total_paid_list.Add(total_paid_obj);
                }

                foreach (fee f in total_paid_list)
                {
                    foreach (admission adm in adm_list)
                    {
                        if (f.std_id == adm.id)
                        {
                            total_paid_obj = new fee();

                            f.std_name = adm.std_name;
                            f.image = adm.image;
                            f.class_name = adm.class_name;
                            f.class_id = adm.class_id;
                            f.section_id = adm.section_id;
                            f.section_name = adm.section_name;
                            f.adm_no = adm.adm_no;
                            f.father_name = adm.father_name;
                        }
                    }

                }         
        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            class_id = c.id;
            if (class_cmb.SelectedIndex != 0)
            {

                section_cmb.IsEnabled = true;

                get_all_sections(c.id);
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;

                paid_fee_grid.ItemsSource = paid_fee_list.Where(x => x.class_id == c.id);
            }
            else
            {
                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
                paid_fee_grid.ItemsSource = paid_fee_list;
            }
        }

        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sections s = (sections)section_cmb.SelectedItem;
            if (s != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    paid_fee_grid.ItemsSource = paid_fee_list.Where(x => x.section_id == s.id);

                }
                else
                {
                    paid_fee_grid.ItemsSource = paid_fee_list.Where(x => x.class_id == class_id);
                }
            }


        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker.SelectedDate != null)
            {
                month_cmb.SelectedIndex = 0;
                total_fee_paid = 0;
                dt = date_picker.SelectedDate.Value;
                get_fee_history_by_date();
                set_paid_list();
                paid_fee_grid.ItemsSource = total_paid_list;
                strength_textblock.Text = paid_fee_grid.Items.Count.ToString();
            }
        }

        public void get_fee_history_by_date()
        {
            int total_paid = 0;
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where session_id="+MainWindow.session.id;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee paid_fee = new fee()
                            {
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"]),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            if (paid_fee.date_time.ToString("yyyy-MM-dd") == dt.ToString("yyyy-MM-dd"))
                            {
                                total_paid = Convert.ToInt32(paid_fee.reg_fee) + Convert.ToInt32(paid_fee.adm_fee) + Convert.ToInt32(paid_fee.tution_fee) + Convert.ToInt32(paid_fee.other_expenses) + Convert.ToInt32(paid_fee.security_fee) + Convert.ToInt32(paid_fee.exam_fee) + Convert.ToInt32(paid_fee.fine_fee);
                                if (total_paid > 0)
                                {
                                    paid_fee.total_paid = total_paid.ToString();
                                    paid_fee_list.Add(paid_fee);
                                }                                
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

        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            std_nos_list = new List<admission>();
            admission adm_obj ;
            
            foreach(fee f in paid_fee_list.Where(x=>x.Checked == true))
            {
                message = "";
                
                adm_obj = new admission();                
                foreach(admission adm in adm_list)
                {
                    if(f.std_id == adm.id)
                    {                        
                        adm_obj = adm;
                        adm_obj.sms_type = "Fee Paid";
                        if (Without_btn.IsChecked == true)
                        {
                            adm_obj.sms_message = "Respected Parents, AoA, It is to inform you that " + adm_obj.std_name + "'s fee for the month of " + f.month + " " + MainWindow.session.session_name + " under Receipt# " + f.receipt_no + " has been deposited to the Institute office. Thank you, Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                        }
                        else 
                        {
                            adm_obj.sms_message = "Respected Parents, AoA, It is to inform you that " + adm_obj.std_name + "'s fee for the month of " + f.month + " " + MainWindow.session.session_name + " under Receipt# " + f.receipt_no + " with Amount " + f.total_paid + "/Rs has been deposited to the Institute office. Thank you, Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                        }
                        break;
                    }
                }
                std_nos_list.Add(adm_obj);
            }
            //MessageBoxResult mbr = MessageBox.Show("Are You Want To Send    " + std_nos_list.Count + "  SMS ?", "Send Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);

            //if (mbr == MessageBoxResult.Yes)
            //{
            //    //this.Close();
            //    UploadWindow uw = new UploadWindow(std_nos_list);
            //    uw.Show();
            //}

            if (std_nos_list.Count > 0)
            {
                OptionWindow ow = new OptionWindow();
                ow.ShowDialog();

                if (isbranded == true)
                {
                    BrandedSmsEngine bse = new BrandedSmsEngine(std_nos_list);
                    bse.Show();
                }
                else
                {
                    UploadWindow uw = new UploadWindow(std_nos_list,false);
                    uw.Show();
                }

            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student");
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(paid_fee_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = paid_fee_grid.Items.Count.ToString();
        }
        
    }
}
