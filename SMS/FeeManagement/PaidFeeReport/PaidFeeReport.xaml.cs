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
using SUT.PrintEngine.Utils;

namespace SMS.FeeManagement.PaidFeeReport
{
    /// <summary>
    /// Interaction logic for PaidFeeReport.xaml
    /// </summary>
    public partial class PaidFeeReport : Page
    {
        public List<fee> paid_fee_list;
        public List<fee> paid_list;
        string months;
        sms_months sm;
        List<admission> adm_list;
        List<fee> total_fee_list;
        List<fee> total_list;
        List<sms_months> months_list;

        int total_annual_fee = 0;
        int total_adm_fee = 0;
        int total_tution_fee = 0;
        int total_other_fee = 0;
        int total_security_fee = 0;
        int total_exam_fee = 0;


        int total_fee_paid = 0;
        int total_annual_paid = 0;
        int total_adm_paid = 0;
        int total_security_paid = 0;
        int total_exam_paid = 0;

        int monthly_tution_paid = 0;
        int monthly_tution_paid_n = 0;
        int monthly_other_paid = 0;
        int monthly_other_paid_n = 0;
        int monthly_annual_paid = 0;
        int monthly_adm_paid = 0;
        int monthly_security_paid = 0;
        int monthly_exam_paid = 0;       

        public PaidFeeReport()
        {
            InitializeComponent();           
            
            month_cmb.SelectedIndex = 0;        
            get_all_admissions();
            get_fee_history();
            get_all_fee();

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "-Select Month-", id = "-1" });
            month_cmb.ItemsSource = months_list;

            institue_name.Text = MainWindow.ins.institute_name;
            institute_logo.Source = MainWindow.ByteToImage(MainWindow.ins.institute_logo);
            session_tb.Text = MainWindow.session.session_name;
        }
        //---------------           Get All Months    ----------------------------------
        public void get_all_months()
        {
            months_list = new List<sms_months>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_months";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_months sm = new sms_months()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                month_id = Convert.ToString(reader["month"].ToString()),
                            };
                            months_list.Add(sm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                        cmd.CommandText = "select * from sms_fee_paid where session_id ="+MainWindow.session.id;
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

        //===============    Get All Fee Data            ==================
        public void get_all_fee() 
        {
            total_fee_list = new List<fee>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where isActive='Y' && session_id ="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            fee f = new fee()
                            {
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                            };
                            total_fee_list.Add(f);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
       
        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (month_cmb.SelectedIndex != 0)
            {
                report_grid.Visibility = Visibility.Visible;
                img_grid.Visibility = Visibility.Hidden;


                total_fee_paid = 0;
                sm = (sms_months)month_cmb.SelectedItem;
                months = sm.month_name;
                set_paid_list();
                calculate_paid_fee();
                fill_control();
                month_charges_label.Content = months;

                
            }
            else
            {
                
                total_fee_paid = 0;
                total_annual_paid = 0;
                total_adm_paid = 0;
                monthly_tution_paid = 0;
                monthly_other_paid = 0;
                monthly_annual_paid = 0;
                monthly_adm_paid = 0;

                report_grid.Visibility = Visibility.Hidden;
                img_grid.Visibility = Visibility.Visible;
            }

        }
       

        public void set_paid_list()
        {
            total_list = new List<fee>();
            paid_list = new List<fee>();
            fee paid_fee_obj;

            foreach (fee f in paid_fee_list)
            {
                foreach (admission adm in adm_list)
                {
                    if (f.std_id == adm.id)
                    {
                        paid_fee_obj = new fee();

                        f.std_name = adm.std_name;
                        f.image = adm.image;
                        f.class_name = adm.class_name;
                        f.class_id = adm.class_id;
                        f.section_id = adm.section_id;
                        f.section_name = adm.section_name;

                        paid_fee_obj = f;
                        paid_list.Add(paid_fee_obj);
                    }
                }
                
            }

            // For total fee
            foreach(fee f in total_fee_list)
            {
                foreach(admission adm in adm_list)
                {
                    if(f.std_id == adm.id)
                    {
                        total_list.Add(f);
                    }
                }
            
            }
        }

        public void calculate_paid_fee() 
        {
            int total = 0;
            int total_annual = 0;
            int total_adm = 0;
            int total_security = 0;
            int total_exam = 0;

            int monthly_tution = 0;
            int monthly_tution_n = 0;
            int monthly_annual = 0;
            int monthly_adm = 0;
            int monthly_other = 0;
            int monthly_other_n = 0;
            int monthly_security = 0;
            int monthly_exam = 0;
            

            total_fee_paid = 0;
            total_annual_fee = 0;
            total_adm_fee = 0;
            total_tution_fee = 0;
            total_other_fee = 0;
            total_security_fee = 0;
            total_exam_fee = 0;

            monthly_tution_paid = 0;
            monthly_tution_paid_n = 0;
            monthly_other_paid = 0;
            monthly_other_paid_n = 0;
            monthly_annual_paid = 0;
            monthly_adm_paid = 0;
            monthly_security_paid = 0;
            monthly_exam_paid = 0;

            total_annual_paid = 0;
            total_adm_paid = 0;
            total_security_paid = 0;
            total_exam_paid = 0;

            foreach(fee f in paid_list)
            {
                if(f.month==months)
                {
                    monthly_tution = Convert.ToInt32(f.tution_fee);
                    monthly_tution_paid = monthly_tution_paid + monthly_tution;

                    monthly_other = Convert.ToInt32(f.other_expenses);
                    monthly_other_paid = monthly_other_paid + monthly_other;

                    
                }
                if(f.date_time.Month == Convert.ToInt32(sm.month_id))
                {
                    monthly_tution_n = Convert.ToInt32(f.tution_fee);
                    monthly_tution_paid_n = monthly_tution_paid_n + monthly_tution_n;

                    monthly_other_n = Convert.ToInt32(f.other_expenses);
                    monthly_other_paid_n = monthly_other_paid_n + monthly_other_n;

                    monthly_annual = Convert.ToInt32(f.reg_fee);
                    monthly_annual_paid = monthly_annual_paid + monthly_annual;

                    monthly_adm = Convert.ToInt32(f.adm_fee);
                    monthly_adm_paid = monthly_adm_paid + monthly_adm;

                    monthly_security = Convert.ToInt32(f.security_fee);
                    monthly_security_paid = monthly_security_paid + monthly_security;

                    monthly_exam = Convert.ToInt32(f.exam_fee);
                    monthly_exam_paid = monthly_exam_paid + monthly_exam;

                    total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee);
                    total_fee_paid = total_fee_paid + total;
                }

                total_annual = Convert.ToInt32(f.reg_fee);
                total_annual_paid = total_annual_paid + total_annual;

                total_adm = Convert.ToInt32(f.adm_fee);
                total_adm_paid = total_adm_paid + total_adm;

                total_security = Convert.ToInt32(f.security_fee);
                total_security_paid = total_security_paid + total_security;

                total_exam = Convert.ToInt32(f.exam_fee);
                total_exam_paid = total_exam_paid + total_exam;

            }

            foreach (admission adm in adm_list)
            {
                foreach (fee f in total_list)
                {
                    if(adm.id == f.std_id)
                    {
                        if(f.month == months)
                        {
                            total_annual_fee = total_annual_fee + Convert.ToInt32(f.reg_fee);
                            total_adm_fee = total_adm_fee + Convert.ToInt32(f.adm_fee);
                            total_tution_fee = total_tution_fee + Convert.ToInt32(f.tution_fee);
                            total_other_fee = total_other_fee + Convert.ToInt32(f.other_expenses);
                            total_security_fee = total_security_fee + Convert.ToInt32(f.security_fee);
                            total_exam_fee = total_exam_fee + Convert.ToInt32(f.exam_fee);
                        }                        
                    }
                }
            }
        }

        public void fill_control() 
        {
            total_annual_tb.Text = total_annual_fee.ToString();
            paid_annual_tb.Text = total_annual_paid.ToString();
            rem_annual_tb.Text = Convert.ToString(total_annual_fee - total_annual_paid);

            total_adm_tb.Text = total_adm_fee.ToString();
            paid_adm_tb.Text = total_adm_paid.ToString();
            rem_adm_tb.Text = Convert.ToString(total_adm_fee-total_adm_paid);

            total_tution_tb.Text = total_tution_fee.ToString();
            paid_tution_tb.Text = monthly_tution_paid.ToString();
            rem_tution_tb.Text = Convert.ToString(total_tution_fee - monthly_tution_paid);            

            total_other_tb.Text = total_other_fee.ToString();
            paid_other_tb.Text = monthly_other_paid.ToString();
            rem_other_tb.Text = Convert.ToString(total_other_fee-monthly_other_paid);

            total_security_tb.Text = total_security_fee.ToString();
            paid_security_tb.Text = monthly_security_paid.ToString();
            rem_security_tb.Text = Convert.ToString(total_security_fee - monthly_security_paid);

            total_exam_tb.Text = total_exam_fee.ToString();
            paid_exam_tb.Text = monthly_exam_paid.ToString();
            rem_exam_tb.Text = Convert.ToString(total_exam_fee - monthly_exam_paid);

            monthly_annual_paid_tb.Text = monthly_annual_paid.ToString();
            monthly_adm_paid_tb.Text = monthly_adm_paid.ToString();
            monthly_security_paid_tb.Text = monthly_security_paid.ToString();
            monthly_exam_paid_tb.Text = monthly_exam_paid.ToString();
            monthly_tution_paid_tb.Text = monthly_tution_paid_n.ToString();
            monthly_other_paid_tb.Text = monthly_other_paid_n.ToString();

            total_paid_tb.Text = total_fee_paid.ToString();


        }

        public void clear_control() 
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var visualSize = new Size(report_grid.ActualWidth, report_grid.ActualHeight);
            var printControl = PrintControlFactory.Create(visualSize, report_grid);

            printControl.ShowPrintPreview();
        }

        

        
    }
}

