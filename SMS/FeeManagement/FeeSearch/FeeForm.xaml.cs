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
using SMS.FeeManagement.FeeSearch;
using SMS.Models;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using SUT.PrintEngine;
using SUT.PrintEngine.Utils;
using SMS.FeeManagement.FeePrintSlip;



namespace SMS.FeeManagement.FeeSearch
{
    /// <summary>
    /// Interaction logic for FeeForm.xaml
    /// </summary>
    public partial class FeeForm : Window
    {
        FeeSearch FS;
        admission obj;
        //string month;

        string rem_reg_fee="0";
        string rem_adm_fee="0";
        string rem_tution_fee="0";
        string rem_security_fee="0";
        string rem_transport_fee="0";
        string rem_exam_fee="0";
        string rem_other_exp="0";
        string rem_fine_fee = "0";

        string last_receipt_no = "0";
        int receipt_no = 0;
        string paid_receipt_no = "0";

        int total_fee = 0;
        int reg_fee = 0;
        int adm_fee = 0;
        int tutuion_fee = 0;        
        int exam_fee = 0;
        int sec_fee = 0;        
        int transport_fee = 0;
        int other_exp = 0;
        int fine_fee = 0;

        int paid_reg_fee = 0;
        int paid_adm_fee = 0;
        int paid_tutuion_fee = 0;
        int paid_exam_fee = 0;
        int paid_sec_fee = 0;
        int paid_transport_fee = 0;
        int paid_other_exp = 0;
        int paid_fine_fee = 0;
        char isActive = 'Y';

        public List<fee_history> fee_history_list;
        public List<fee> paid_fee_list;
        fee_history fh;
        List<sms_months> months_list;
        List<fee> fee_list;
        int months_id;
        int month = 0;
        int selected_index = 0;

        List<sms_months> pending_months;
        int pending_amount = 0;        
        string pending_desc;        
        List<fee> pending_fee_lst;    
    
        int fine_amount = 0;
        string fine_desc = "";
        List<sms_months> fine_months;
        List<fee> fine_fee_list;

        int other_amount = 0;
        string other_desc = "";
        List<sms_months> other_months;
        List<fee> other_fee_list;
        List<other_fee> other_desc_list;
        List<other_fee> paid_desc_list;

        public static string cancelReceiptNumber;

        public FeeForm(FeeSearch fs, admission adm)
        {
            InitializeComponent();

            FS = fs;
            this.obj = adm;
            month = DateTime.Now.Month;
            if(month < 4)
            {
                months_tabcontrol.SelectedIndex = month+8;
            }
            else
            {
                months_tabcontrol.SelectedIndex = month - 4;
            }
            
            fill_control();
            get_fee_history();
            institute_name_lbl.Content = MainWindow.ins.institute_name;
            institute_logo.Source = MainWindow.ByteToImage(MainWindow.ins.institute_logo);
        }

        public void fill_control() 
        {
            date.Text = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
            std_name.Text = obj.std_name.ToString();
            fname.Text = obj.father_name.ToString();
            adm_no.Text = obj.adm_no.ToString();
            class_name.Text = obj.class_name.ToString();
        }
        public void fill_object() 
        {

        }
        private void months_tabcontrol_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            
            reg_fee_textbox.IsReadOnly = false;
            adm_fee_texbox.IsReadOnly = false;
            security_fee_textbox.IsReadOnly = false;
            tution_fee_textbox.IsReadOnly = false;
            transport_fee_textbox.IsReadOnly = false;
            exam_fee_textbox.IsReadOnly = false;
            other_fee_textbox.IsReadOnly = false;
            fine_fee_textbox.IsReadOnly = true;

            pay_cash_btn.IsEnabled = true;
           
            reset_btn.IsEnabled = true;
            paid_grid.Visibility = Visibility.Hidden;
            disable_grid.Visibility = Visibility.Hidden;
            tution_Waveoff.IsEnabled = true;
            other_Waveoff.IsEnabled = true;
            fine_Waveoff.IsEnabled = true;

            TabItem selectedTab = e.AddedItems[0] as TabItem;  // Gets selected tab

            month_textblock.Text = selectedTab.Name.ToString();
            months_id = months_tabcontrol.SelectedIndex;
            //MessageBox.Show(months_tabcontrol.SelectedIndex.ToString());
            get_fee_data(selectedTab.Name.ToString() , obj.id.ToString());

            manage_paid_fee();

            fee_history_grid.ItemsSource = fee_history_list;
            get_all_months();
            get_pending_fee();
            get_all_fine();
            get_all_other();
            fill_fee_control();            
            check_status();           
        }

        public void get_all_other() 
        {
            other_amount = 0;
            other_desc = "Other:";
            string m_name = "";
            int monthly_other = 0;
            other_fee_list = new List<fee>();
            other_months = new List<sms_months>();
            other_desc_list = new List<other_fee>();
            other_desc_list = get_other_expense(obj.id);
            paid_desc_list = new List<other_fee>();
            bool isPaid = false;

            for (int i = 1; i <= months_id + 1; i++)
            {
                //get Month name
                foreach (sms_months sm in months_list)
                {
                    if (i.ToString() == sm.id)
                    {
                        m_name = sm.month_name;
                        other_months.Add(sm);
                        break;
                    }
                }

                foreach (fee f in fee_list.Where(x => x.isActive == 'Y'))
                {
                    try
                    {
                        if (f.month == m_name)
                        {
                            monthly_other = 0;
                            monthly_other = Convert.ToInt32(f.rem_other_fee);
                            other_amount = other_amount + Convert.ToInt32(f.rem_other_fee);

                            if (monthly_other > 0)
                            {
                                other_desc = other_desc + m_name + "(";
                                foreach (other_fee other in other_desc_list.Where(x => x.month_name == m_name))
                                {
                                    isPaid = false;
                                    foreach(fee paid in paid_fee_list.Where(x=>x.other_exp_id == other.id))
                                    {
                                        if (Convert.ToInt32(paid.other_expenses) == other.amount)
                                        {
                                            isPaid = true;
                                        }                                        
                                    }       
                                    if(!isPaid)
                                    {
                                        other_desc = other_desc + other.fee_type + ",";
                                        paid_desc_list.Add(other);
                                    }
                                }
                                other_desc = other_desc + ")";
                                other_fee_list.Add(f);
                            }
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public void get_all_fine() 
        {
            fine_amount = 0;
            fine_desc = "Fine:";
            string m_name = "";
            int monthly_fine = 0;
            fine_fee_list = new List<fee>();
            fine_months = new List<sms_months>();

            for (int i = 1; i <= months_id + 1; i++)
            {
                //get Month name
                foreach (sms_months sm in months_list)
                {
                    if (i.ToString() == sm.id)
                    {
                        m_name = sm.month_name;
                        fine_months.Add(sm);
                        break;
                    }
                }

                foreach (fee f in fee_list.Where(x => x.isActive == 'Y'))
                {
                    
                    if (f.month == m_name)
                    {
                        monthly_fine = 0;
                        monthly_fine = Convert.ToInt32(f.rem_fine_fee);
                        fine_amount = fine_amount + Convert.ToInt32(f.rem_fine_fee);

                        if (monthly_fine > 0)
                        {
                            fine_desc = fine_desc + m_name + ",";
                            fine_fee_list.Add(f);
                        }
                        break;
                    }
                }
            }
        }

        public void get_pending_fee()
        {
            pending_months = new List<sms_months>();           
            pending_fee_lst = new List<fee>();
            int monthly_pending = 0;            
            string m_name = "";
            pending_amount = 0;            
            pending_desc = "Pending Fee:";           

            for (int i = 1; i < months_id+1; i++)
            {
                //get Month name
                foreach (sms_months sm in months_list)
                {
                    if (i.ToString() == sm.id)
                    {
                        m_name = sm.month_name;
                        pending_months.Add(sm);
                        break;
                    }
                }

                foreach (fee f in fee_list.Where(x=>x.isActive=='Y'))
                {
                    try
                    {
                        if (f.month == m_name)
                        {                            
                            monthly_pending = 0;
                            monthly_pending = Convert.ToInt32(f.rem_tution_fee);
                            pending_amount = pending_amount + monthly_pending;
                            
                            if (monthly_pending > 0)
                            {
                                pending_desc = pending_desc + m_name + ",";
                                pending_fee_lst.Add(f);
                            }                            
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }

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

        public void check_status() 
        {
            if (isActive == 'Y')
            {
                if (total_fee == 0)
                {
                    if (paid_fee_list.Count > 0)
                    {
                        paid_reg_fee = 0;
                        paid_adm_fee = 0;
                        paid_sec_fee = 0;
                        paid_tutuion_fee = 0;
                        paid_transport_fee = 0;
                        paid_other_exp = 0;
                        paid_exam_fee = 0;
                        paid_fine_fee = 0;

                        reg_fee_textbox.IsReadOnly = true;
                        adm_fee_texbox.IsReadOnly = true;
                        security_fee_textbox.IsReadOnly = true;
                        tution_fee_textbox.IsReadOnly = true;
                        transport_fee_textbox.IsReadOnly = true;
                        exam_fee_textbox.IsReadOnly = true;
                        other_fee_textbox.IsReadOnly = true;
                        fine_fee_textbox.IsReadOnly = true;

                        pay_cash_btn.IsEnabled = false;

                        reset_btn.IsEnabled = false;
                        paid_grid.Visibility = Visibility.Visible;
                        disable_grid.Visibility = Visibility.Hidden;
                        tution_Waveoff.IsEnabled = false;
                        other_Waveoff.IsEnabled = false;
                        fine_Waveoff.IsEnabled = false;

                        foreach (fee f in paid_fee_list.Where(x => x.month.Equals(month_textblock.Text)))
                        {
                            paid_reg_fee = paid_reg_fee + Convert.ToInt32(f.reg_fee);
                            paid_adm_fee = paid_adm_fee + Convert.ToInt32(f.adm_fee);
                            paid_sec_fee = paid_sec_fee + Convert.ToInt32(f.security_fee);
                            paid_tutuion_fee = paid_tutuion_fee + Convert.ToInt32(f.tution_fee);
                            paid_transport_fee = paid_transport_fee + Convert.ToInt32(f.transport_fee);
                            paid_other_exp = paid_other_exp + Convert.ToInt32(f.other_expenses);
                            paid_exam_fee = paid_exam_fee + Convert.ToInt32(f.exam_fee);
                            paid_fine_fee = paid_fine_fee + Convert.ToInt32(f.fine_fee);
                            paid_receipt_no = f.receipt_no.ToString();
                        }

                        reg_fee_textbox.Text = paid_reg_fee.ToString();
                        adm_fee_texbox.Text = paid_adm_fee.ToString();
                        security_fee_textbox.Text = paid_sec_fee.ToString();
                        tution_fee_textbox.Text = paid_tutuion_fee.ToString();
                        transport_fee_textbox.Text = paid_sec_fee.ToString();
                        other_fee_textbox.Text = paid_other_exp.ToString();
                        exam_fee_textbox.Text = paid_exam_fee.ToString();
                        receipt_no_textblock.Text = paid_receipt_no.ToString();
                        fine_fee_textbox.Text = paid_fine_fee.ToString();
                    }
                }
            }
            else 
            {
                reg_fee_textbox.IsReadOnly = true;
                reg_fee_textbox.Text = "";
                adm_fee_texbox.IsReadOnly = true;
                adm_fee_texbox.Text = "";
                security_fee_textbox.IsReadOnly = true;
                security_fee_textbox.Text = "";
                tution_fee_textbox.IsReadOnly = true;
                tution_fee_textbox.Text = "";
                transport_fee_textbox.IsReadOnly = true;
                transport_fee_textbox.Text="";
                exam_fee_textbox.IsReadOnly = true;
                exam_fee_textbox.Text="";
                other_fee_textbox.IsReadOnly = true;
                other_fee_textbox.Text="";                                
                pending_fee_textbox.Text = "";
                pending_fee_label.Content = "Pending";
                total_textbox.Text = "";
                fine_fee_textbox.IsReadOnly = true;
                fine_fee_label.Content = "Fine";
                fine_fee_textbox.Text = "";
                words.Text = "";
                pay_cash_btn.IsEnabled = false;

                reset_btn.IsEnabled = false;
                paid_grid.Visibility = Visibility.Hidden;
                disable_grid.Visibility = Visibility.Visible;
                tution_Waveoff.IsEnabled = false;
                other_Waveoff.IsEnabled = false;
                fine_Waveoff.IsEnabled = false;
            }
        }

        //======== Manage Paid Feeses         ============================
        public void manage_paid_fee() 
        {
            get_fee_history();

            fee_history_list = new List<fee_history>();
            foreach(fee f in paid_fee_list)
            {
                
                if(f.reg_fee != "0")
                {
                    fh = new fee_history() 
                    {
                        particulars="Annual Fund",
                        month = f.month,
                        amount=f.reg_fee,
                        date_time=f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }

                if (f.adm_fee != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Admission Fee",
                        month = f.month,
                        amount = f.adm_fee,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }

                if (f.security_fee != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Security Fee",
                        month = f.month,
                        amount = f.security_fee,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }

                if (f.tution_fee != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Tution Fee",
                        month = f.month,
                        amount = f.tution_fee,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }

                if (f.other_expenses != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = f.other_exp_type,
                        month = f.month,
                        amount = f.other_expenses,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }

                if (f.transport_fee != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Transport Fee",
                        month = f.month,
                        amount = f.transport_fee,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }

                if (f.exam_fee != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Exam Fee",
                        month = f.month,
                        amount = f.exam_fee,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }

                if (f.fine_fee != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Fine",
                        month = f.month,
                        amount = f.fine_fee,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }
                if (f.fine_fee_wave_off != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Fine WaveOff",
                        month = f.month,
                        amount = f.fine_fee_wave_off,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }
                if (f.tution_fee_wave_off != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Tution WaveOff",
                        month = f.month,
                        amount = f.tution_fee_wave_off,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }
                if (f.other_fee_wave_off != "0")
                {
                    fh = new fee_history()
                    {
                        particulars = "Other WaveOff",
                        month = f.month,
                        amount = f.other_fee_wave_off,
                        date_time = f.date_time,
                        receipt_no = f.receipt_no
                    };
                    fee_history_list.Add(fh);
                }
            }
        }

        //========      Get Fee History       =============================
        public void get_fee_history() 
        {
            paid_fee_list = new List<fee>();
            try
            {
                using(MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using(MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where std_id ="+obj.id+"&& session_id = "+MainWindow.session.id;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
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
                                other_exp_type = Convert.ToString(reader["other_exp_type"].ToString()),
                                other_exp_type_id = Convert.ToString(reader["other_exp_type_id"].ToString()),
                                other_exp_id = Convert.ToInt32(reader["other_exp_id"]),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                fine_fee_wave_off = Convert.ToString(reader["fine_fee_wave_off"].ToString()),
                                other_fee_wave_off = Convert.ToString(reader["other_fee_wave_off"].ToString()),
                                tution_fee_wave_off = Convert.ToString(reader["tution_fee_wave_off"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                
                                 

                            };
                            paid_fee_list.Add(paid_fee);
                            
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //========      Fill Fee Control      =============================
        public void fill_fee_control() 
        {
            try
            {
                adm_fee_texbox.Text = rem_adm_fee;
                reg_fee_textbox.Text = rem_reg_fee;
                tution_fee_textbox.Text = rem_tution_fee;
                exam_fee_textbox.Text = rem_exam_fee;
                transport_fee_textbox.Text = rem_security_fee;
               // other_fee_textbox.Text = rem_other_exp;
                security_fee_textbox.Text = "0";
                receipt_no = Convert.ToInt32(last_receipt_no);
                receipt_no++;
                receipt_no_textblock.Text = receipt_no.ToString("D7");

                reg_fee = Convert.ToInt32(rem_reg_fee);
                adm_fee = Convert.ToInt32(rem_adm_fee);
                tutuion_fee = Convert.ToInt32(rem_tution_fee);

                fine_fee_label.Content = fine_desc;
                fine_fee_textbox.Text = fine_amount.ToString(); ;

                other_fee_label.Content = other_desc;
                other_fee_textbox.Text = other_amount.ToString(); ;

                pending_fee_textbox.Text = pending_amount.ToString();
                pending_fee_label.Content = pending_desc;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        //========      Get fee data          ====================================
        public void get_fee_data(string m,string id) 
        {
            fee_list = new List<fee>();
            rem_reg_fee = "0";
            rem_adm_fee = "0";
            rem_tution_fee = "0";
            rem_security_fee = "0";
            rem_transport_fee = "0";
            rem_exam_fee = "0";
            rem_other_exp = "0";
            rem_fine_fee = "0";

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where std_id = @id && session_id ="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;
                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = m;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {                               
                            if (m == reader["month"].ToString())
                            {
                                rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString());
                                rem_reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString());
                                rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString());
                                rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString());
                                rem_security_fee = Convert.ToString(reader["rem_security_fee"].ToString());
                                rem_other_exp = Convert.ToString(reader["rem_other_exp"].ToString());
                                rem_adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString());
                                rem_fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString());
                                isActive = Convert.ToChar(reader["isActive"]);
                            }
                            fee f = new fee()
                            {
                                rem_security_fee = Convert.ToString(reader["rem_security_fee"].ToString()),
                                rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                                rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),                                
                                rem_other_fee = Convert.ToString(reader["rem_other_exp"].ToString()),
                                rem_adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString()),
                                rem_reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString()),
                                rem_fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                isActive = Convert.ToChar(reader["isActive"]), 
                            };
                            fee_list.Add(f);
                        };
                                               
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_last_receipt_no ";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();

                        last_receipt_no = Convert.ToString(reader["last_receipt_no"].ToString());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //===========    Number Validation   =============================
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //==============           Total                ===========================
        public void total()
        {

            total_fee = 0;
            adm_fee = 0;
            tutuion_fee = 0;            
            exam_fee = 0;
            sec_fee = 0;            
            transport_fee = 0;
            other_exp = 0;
            fine_fee = 0;
            try
            {
                if (reg_fee_textbox.Text.Trim() == "" || reg_fee_textbox == null)
                {
                    reg_fee_textbox.Text = "0";
                }
                if (adm_fee_texbox.Text.Trim() == "" || adm_fee_texbox == null)
                {
                    adm_fee_texbox.Text = "0";
                }
                if (tution_fee_textbox.Text.Trim() == "" || tution_fee_textbox == null)
                {
                    tution_fee_textbox.Text = "0";
                }
                if (exam_fee_textbox.Text.Trim() == "" || exam_fee_textbox == null)
                {
                    exam_fee_textbox.Text = "0";
                }
                if (security_fee_textbox.Text.Trim() == "" || security_fee_textbox == null)
                {
                    security_fee_textbox.Text = "0";
                }
                if (transport_fee_textbox.Text.Trim() == "" || transport_fee_textbox == null)
                {
                    transport_fee_textbox.Text = "0";
                }
                if (other_fee_textbox.Text.Trim() == "" || other_fee_textbox == null)
                {
                    other_fee_textbox.Text = "0";
                }
                if (fine_fee_textbox.Text.Trim() == "" || fine_fee_textbox == null)
                {
                    fine_fee_textbox.Text = "0";
                }

                reg_fee = Convert.ToInt32(reg_fee_textbox.Text.Trim());
                adm_fee = Convert.ToInt32(adm_fee_texbox.Text.Trim());
                tutuion_fee = Convert.ToInt32(tution_fee_textbox.Text.Trim());                
                exam_fee = Convert.ToInt32(exam_fee_textbox.Text.Trim());
                sec_fee = Convert.ToInt32(transport_fee_textbox.Text.Trim());
                transport_fee = 0;
                other_exp = Convert.ToInt32(other_fee_textbox.Text.Trim());
                fine_fee = Convert.ToInt32(fine_fee_textbox.Text.Trim());
                
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            total_fee = reg_fee + adm_fee + tutuion_fee  + exam_fee + sec_fee  + other_exp + fine_fee + pending_amount;

            if (total_fee != 0)
            {
                total_textbox.Text = total_fee.ToString();
                words.Text = NumberToWords(total_fee);
            }
            else 
            {
                
                try
                {
                    total_textbox.Text = "0";
                    words.Text = "Zero";
                }
                catch (Exception ex)
                { }
                
            }

        }

        //========    Fee Text box selection change events      ==========================
        private void adm_fee_texbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(adm_fee_texbox != null)
            {
                total();
            }
            
        }
        private void reg_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }
        private void pending_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }
        private void tution_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {           
            total();
            if (tution_fee_textbox.Text == "0")
            {
                //tution_Waveoff.IsEnabled = false;
            }
            else
            {
                //tution_Waveoff.IsEnabled = true;
            }
        }
        private void exam_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }
        private void transport_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
           total();
        }
        private void security_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }
        private void other_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {            
            total();
            if (other_fee_textbox.Text == "0")
            {
                //other_Waveoff.IsEnabled = false;
            }
            else
            {
                //other_Waveoff.IsEnabled = true;
            }
        }
        private void fine_fee_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {            
            total();
            if (fine_fee_textbox.Text == "0")
            {
                //fine_Waveoff.IsEnabled = false;
            }
            else
            {
                //fine_Waveoff.IsEnabled = true;
            }
        }

        //===========      Digits to words              ================================

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }


        // Pay Cash
        private void pay_cash_btn_Click(object sender, RoutedEventArgs e)
        {
                calculate_fee();
                if (validate())
                {
                    MessageBoxResult mbr = MessageBox.Show("Are You Want To Pay This Fee As A Cash ? \n \n \n Total Fee:  " + total_fee + " Rs", "Payment Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Information);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        if (update_fee() == 0)
                        {
                            selected_index = months_tabcontrol.SelectedIndex;
                            if (selected_index != 0)
                            {
                                months_tabcontrol.SelectedIndex = selected_index - 1;
                            }
                            else 
                            {
                                months_tabcontrol.SelectedIndex = selected_index + 1;

                            }
                            
                            months_tabcontrol.SelectedIndex = selected_index;
                        }
                    }
                }
        }

        public int update_fee()
        {            
            int i = 1;
            try
            {
                //for Fine
                if(Convert.ToInt32(fine_fee_textbox.Text) > 0)
                {
                    foreach (fee fp in fine_fee_list)
                    {                        
                        using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_fee = new MySqlCommand())
                            {
                                cmd_fee.CommandText = "Update sms_fee SET rem_fine_fee=@rem_fine_fee WHERE std_id = @id && month = @month && session_id =" + MainWindow.session.id;
                                cmd_fee.Connection = con_fee;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                                cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                                cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";                              

                                con_fee.Open();
                                i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                                con_fee.Close();
                            }
                        }
                    }
                }   
             
                // sms_fee_paid All Fine
                if (Convert.ToInt32(fine_fee_textbox.Text) > 0)
                {
                    foreach (fee fp in fine_fee_list)
                    {
                        using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_paid = new MySqlCommand())
                            {
                                cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id,fine_fee_paid)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id,@rem_fine_fee)";
                                cmd_paid.Connection = con_paid;

                                cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                                cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";                              
                                cmd_paid.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.rem_fine_fee;
                                cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                                cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = receipt_no_textblock.Text;

                                con_paid.Open();
                                cmd_paid.ExecuteScalar();
                                con_paid.Close();
                            }
                        }
                    }
                }

                //for Other
                if(Convert.ToInt32(other_fee_textbox.Text) > 0)
                {
                    foreach (fee fp in other_fee_list)
                    {                        
                        using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_fee = new MySqlCommand())
                            {
                                cmd_fee.CommandText = "Update sms_fee SET rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id =" + MainWindow.session.id;
                                cmd_fee.Connection = con_fee;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                                cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                                cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";                              

                                con_fee.Open();
                                i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                                con_fee.Close();
                            }
                        }
                    }
                }   
             
                // sms_fee_paid Other
                if (Convert.ToInt32(other_fee_textbox.Text) > 0)
                {
                    foreach (fee fp in other_fee_list)
                    {
                        foreach (other_fee other in paid_desc_list.Where(x => x.month_name == fp.month))
                        {
                            using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_paid = new MySqlCommand())
                                {
                                    cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id,fine_fee_paid,other_exp_type,other_exp_type_id, other_exp_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id,@rem_fine_fee,@other_exp_type,@other_exp_type_id,@other_exp_id)";
                                    cmd_paid.Connection = con_paid;

                                    cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                    cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                                    cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other.amount;
                                    cmd_paid.Parameters.Add("@other_exp_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other.fee_type;
                                    cmd_paid.Parameters.Add("@other_exp_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = other.id;
                                    cmd_paid.Parameters.Add("@other_exp_type_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other.fee_type_id;
                                    cmd_paid.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                                    cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                    cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                    cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = receipt_no_textblock.Text;

                                    con_paid.Open();
                                    cmd_paid.ExecuteScalar();
                                    con_paid.Close();
                                }
                            }
                        }
                    }
                }

                //For pending fee

                foreach (fee fp in pending_fee_lst)
                {
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {
                            cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee WHERE std_id = @id && month = @month && session_id ="+MainWindow.session.id;
                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                            cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                            cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            //cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            //cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                            con_fee.Open();
                            i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                            con_fee.Close();
                        }
                    }
                }


                // sms_fee_paid Previous Month
                foreach (fee fp in pending_fee_lst)
                {
                    using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_paid = new MySqlCommand())
                        {
                            cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                            cmd_paid.Connection = con_paid;

                            cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                            cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.rem_tution_fee;
                            cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                            cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                            cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                            cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = receipt_no_textblock.Text;                         

                            con_paid.Open();
                            cmd_paid.ExecuteScalar();
                            con_paid.Close();
                        }
                    }
                }


                //regulare fee
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_fee SET rem_reg_fee=@rem_reg_fee,rem_adm_fee=@rem_adm_fee,rem_security_fee=@rem_security_fee,rem_exam_fee=@rem_exam_fee,date_time=@date_time,created_by=@created_by WHERE std_id = @id && session_id=@session_id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_reg_fee;
                        cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_adm_fee;
                        cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_sec_fee;
                        cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_exam_fee;                       
                        
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                    }
                }

                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee WHERE std_id = @id && month = @month && session_id="+MainWindow.session.id;
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_fee.Parameters.Add("@month" ,MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text.Trim().ToString();
                        cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_tutuion_fee;                        
                        //cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_exam_fee;
                        //cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_other_exp;
                        //cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_other_exp;

                        con_fee.Open();
                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                        con_fee.Close();
                    }
                }

                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_last_receipt_no SET last_receipt_no=@last_receipt_no";
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@last_receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = receipt_no_textblock.Text;
                        

                        con_fee.Open();

                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                    }
                }
                
                // sms_fee_paid Current Month
                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                {
                    using(MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@std_id",MySql.Data.MySqlClient.MySqlDbType.VarChar).Value=obj.id;
                        cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = reg_fee_textbox.Text.Trim();
                        cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_fee_texbox.Text.Trim();
                        cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = transport_fee_textbox.Text.Trim();
                        cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = exam_fee_textbox.Text.Trim();
                        cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = tution_fee_textbox.Text.Trim();
                        cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        //if (Convert.ToInt32(fine_fee_textbox.Text) > 0 && fine_fee_list.Last().month == month_textblock.Text)
                        //{
                        //    cmd_paid.Parameters.Add("@rem_fine_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine_fee_list.Last().rem_fine_fee;
                        //}
                        //else 
                        //{
                        //    cmd_paid.Parameters.Add("@rem_fine_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        //}
                        
                        cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text;
                        cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = receipt_no_textblock.Text;

                        con_paid.Open();
                        cmd_paid.ExecuteScalar();
                    }
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //Get Other Expense
        public List<other_fee> get_other_expense(string stdId) 
        {
            List<other_fee> other_fee_list = new List<other_fee>();
            try
            {
                using(MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using(MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_other_fee where std_id ="+stdId+"&& session_id = "+MainWindow.session.id;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            other_fee other = new other_fee()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                fee_type_id = Convert.ToInt32(reader["fee_type_id"]),
                                fee_type = Convert.ToString(reader["fee_type"].ToString()),
                                 month_name = Convert.ToString(reader["month_name"].ToString()),
                                 amount = Convert.ToInt32(reader["amount"]),
                            };
                            other_fee_list.Add(other);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return other_fee_list;
        }

        public void calculate_fee() 
        {
            paid_reg_fee = Convert.ToInt32(rem_reg_fee)-reg_fee;
            paid_adm_fee = Convert.ToInt32(rem_adm_fee) - adm_fee;
            paid_exam_fee = Convert.ToInt32(rem_exam_fee)-exam_fee;
            paid_sec_fee = Convert.ToInt32(rem_security_fee)-sec_fee;
            paid_transport_fee = Convert.ToInt32(rem_transport_fee)-transport_fee;
            paid_tutuion_fee = Convert.ToInt32(rem_tution_fee) - tutuion_fee;
            paid_other_exp = Convert.ToInt32(rem_other_exp) - other_exp;
        }

        //------------------    Validation       -------------------------
        public bool validate()
        {
            try
            {


                if (Convert.ToInt32(reg_fee_textbox.Text) > Convert.ToInt32(rem_reg_fee))
                {
                    reg_fee_textbox.Focus();
                    string alertText = "Annual should not Exceed than original registration fee mentinoed in Admission Form";
                    MessageBox.Show(alertText);
                    return false;
                }
                else if (Convert.ToInt32(adm_fee_texbox.Text) > Convert.ToInt32(rem_adm_fee))
                {
                    adm_fee_texbox.Focus();
                    string alertText = "Admission Fee should not Exceed than original admission fee mentinoed in Admission Form";
                    MessageBox.Show(alertText);
                    return false;
                }

                else if (Convert.ToInt32(tution_fee_textbox.Text) > Convert.ToInt32(rem_tution_fee))
                {
                    tution_fee_textbox.Focus();
                    string alertText = "Tution Fee should not Exceed than original tution fee mentinoed in Admission Form";
                    MessageBox.Show(alertText);
                    return false;
                }
                else if (Convert.ToInt32(exam_fee_textbox.Text) > Convert.ToInt32(rem_exam_fee))
                {
                    exam_fee_textbox.Focus();
                    string alertText = "Exam Fee should not Exceed than original Exam fee mentinoed in Admission Form";
                    MessageBox.Show(alertText);
                    return false;
                }
                else if (Convert.ToInt32(transport_fee_textbox.Text) > Convert.ToInt32(rem_security_fee))
                {
                    transport_fee_textbox.Focus();
                    string alertText = "Security Fee should not Exceed than original Transport fee mentinoed in Admission Form";
                    MessageBox.Show(alertText);
                    return false;
                }
                //else if (Convert.ToInt32(other_fee_textbox.Text) > Convert.ToInt32(rem_other_exp))
                //{
                //    other_fee_textbox.Focus();
                //    string alertText = "Other Fee should not Exceed than original Other fee mentinoed in Admission Form";
                //    MessageBox.Show(alertText);
                //    return false;
                //}

                else
                {
                    return true;
                }
                //return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return true;
            }
            //return true;
        }

        //=========       Print Slip          ===============================
        private void print_slip_btn_Click(object sender, RoutedEventArgs e)
        {
                System.Windows.Controls.Image myImage = new System.Windows.Controls.Image();
                int width = (int)Math.Ceiling(visual.ActualWidth);
                int height = (int)Math.Ceiling(visual.ActualHeight);

                width = width == 0 ? 1 : width;
                height = height == 0 ? 1 : height;

                RenderTargetBitmap rtbmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
                rtbmp.Render(visual);
                img_visual_1.Source = rtbmp;
                img_visual_2.Source = rtbmp;
                img_visual_3.Source = rtbmp;
            
                var visualSize = new Size(print_slips.ActualWidth, print_slips.ActualHeight);
                var printControl = PrintControlFactory.Create(visualSize, print_slips);                
                printControl.ShowPrintPreview();
            
            
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        public void reset() 
        {
            reg_fee_textbox.Text = rem_reg_fee.ToString();
            adm_fee_texbox.Text = rem_adm_fee.ToString();
            security_fee_textbox.Text = rem_security_fee;
            tution_fee_textbox.Text = rem_tution_fee;
            other_fee_textbox.Text = rem_other_exp;
            exam_fee_textbox.Text = rem_exam_fee;
            transport_fee_textbox.Text = rem_transport_fee;
        }

        //Wave off 
        private void tution_Waveoff_Click(object sender, RoutedEventArgs e)
        {
            if (WaveoffTutionFee()==0) 
            {
                MessageBox.Show("Successfully Waved off Tution Fee", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                int month = months_tabcontrol.SelectedIndex;
                months_tabcontrol.SelectedIndex = 0;
                months_tabcontrol.SelectedIndex = month;
                get_fee_history();
                fee_history_grid.ItemsSource = fee_history_list;
                fee_history_grid.Items.Refresh();
            }
        }

        private void fine_Waveoff_Click(object sender, RoutedEventArgs e)
        {
            if (WaveoffFineFee() == 0)
            {
                MessageBox.Show("Successfully Waved off Fine", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                int month = months_tabcontrol.SelectedIndex;
                months_tabcontrol.SelectedIndex = 0;
                months_tabcontrol.SelectedIndex = month;
                get_fee_history();
                fee_history_grid.ItemsSource = fee_history_list;
                fee_history_grid.Items.Refresh();
            }
        }

        private void other_Waveoff_Click(object sender, RoutedEventArgs e)
        {
            if (WaveoffOtherFee() == 0)
            {
                MessageBox.Show("Successfully Waved off Other Charges", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                int month = months_tabcontrol.SelectedIndex;
                months_tabcontrol.SelectedIndex = 0;
                months_tabcontrol.SelectedIndex = month;
                get_fee_history();
                fee_history_grid.ItemsSource = fee_history_list;
                fee_history_grid.Items.Refresh();
            }
        }


        private int WaveoffTutionFee() 
        {
            int i = 0;
            try 
            {
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee WHERE std_id = @id && month = @month && session_id=" + MainWindow.session.id;
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text.Trim().ToString();
                        cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_tution_fee)-Convert.ToInt32(tution_fee_textbox.Text);                        

                        con_fee.Open();
                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                        con_fee.Close();
                    }
                }

                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "insert into sms_fee_paid (std_id,tution_fee_wave_off,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@tution_fee_wave_off,@month,@date_time,@created_by,@receipt_no,@session_id)";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd_paid.Parameters.Add("@tution_fee_wave_off", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = tution_fee_textbox.Text;

                        cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text;
                        cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0000";

                        con_paid.Open();
                        cmd_paid.ExecuteScalar();
                        con_paid.Close();
                    }
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return i;

        }

        private int WaveoffFineFee()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_fee SET rem_fine_fee=@rem_fine_fee WHERE std_id = @id && month = @month && session_id=" + MainWindow.session.id;
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text.Trim().ToString();
                        cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                        con_fee.Open();
                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                        con_fee.Close();
                    }
                }

                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "insert into sms_fee_paid (std_id,fine_fee_wave_off,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@fine_fee_wave_off,@month,@date_time,@created_by,@receipt_no,@session_id)";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd_paid.Parameters.Add("@fine_fee_wave_off", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine_fee_textbox.Text.Trim();

                        cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text;
                        cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0000";

                        con_paid.Open();
                        cmd_paid.ExecuteScalar();
                        con_paid.Close();
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;


        }

        private int WaveoffOtherFee()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_fee SET rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id=" + MainWindow.session.id;
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text.Trim().ToString();
                        cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_other_exp) - Convert.ToInt32(other_fee_textbox.Text);

                        con_fee.Open();
                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                        con_fee.Close();
                    }
                }
     

                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                {
                    using(MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "insert into sms_fee_paid (std_id,other_fee_wave_off,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@other_fee_wave_off,@month,@date_time,@created_by,@receipt_no,@session_id)";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@std_id",MySql.Data.MySqlClient.MySqlDbType.VarChar).Value=obj.id;
                        cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd_paid.Parameters.Add("@other_fee_wave_off", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other_fee_textbox.Text;
                   
                        cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_textblock.Text;
                        cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0000";

                        con_paid.Open();
                        cmd_paid.ExecuteScalar();
                        con_paid.Close();
                    }
                }                
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return i;
        }

        private void cance_challan_btn_Click(object sender, RoutedEventArgs e)
        {
            ReceiptNoWindow window = new ReceiptNoWindow();
            window.ShowDialog();
            if (fee_history_list.Select(x => x.receipt_no).Contains(cancelReceiptNumber))
            {
                if( update_sms_fee() > 0)
                {
                    if(delete_sms_fee_paid(cancelReceiptNumber) > 0)
                    {
                        MessageBox.Show("Successfully Cancelled Challan", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        get_fee_history();
                        selected_index = months_tabcontrol.SelectedIndex;
                        if (selected_index != 0)
                        {
                            months_tabcontrol.SelectedIndex = selected_index - 1;
                        }
                        else
                        {
                            months_tabcontrol.SelectedIndex = selected_index + 1;
                        }

                        months_tabcontrol.SelectedIndex = selected_index;
                        fee_history_grid.Items.Refresh();
                    }
                }
            }
            else 
            {
                MessageBox.Show("Receipt# Not Available", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private int update_sms_fee()
        {
            int i=0;
            try
            {
                foreach (fee paidFee in paid_fee_list.Where(x=>x.receipt_no == cancelReceiptNumber))
                {
                    foreach (fee f in fee_list.Where(x=>x.month == paidFee.month))
                    {
                        using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_fee = new MySqlCommand())
                            {
                                cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_other_exp=@rem_other_exp,rem_fine_fee=@rem_fine_fee,date_time=@date_time,created_by=@created_by WHERE std_id = @id && month = @month && session_id=" + MainWindow.session.id;
                                cmd_fee.Connection = con_fee;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                                cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paidFee.month;
                                //cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_tution_fee) + Convert.ToInt32(paidFee.tution_fee)).ToString();
                                //cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                //cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_other_fee) + Convert.ToInt32(paidFee.other_expenses)).ToString();
                                //cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_fine_fee) + Convert.ToInt32(paidFee.fine_fee)).ToString();

                                cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_tution_fee)).ToString();
                                cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_other_fee)).ToString();
                                cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_fine_fee)).ToString();

                                cmd_fee.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                cmd_fee.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;

                                con_fee.Open();
                                i = Convert.ToInt32(cmd_fee.ExecuteNonQuery());
                                con_fee.Close();
                            }
                        }

                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Update sms_fee SET rem_reg_fee=@rem_reg_fee,rem_adm_fee=@rem_adm_fee,rem_security_fee=@rem_security_fee,rem_exam_fee=@rem_exam_fee,date_time=@date_time,created_by=@created_by WHERE std_id = @id && session_id=@session_id";
                                cmd.Connection = con;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                                //cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_reg_fee) + Convert.ToInt32(paidFee.reg_fee)).ToString();
                                //cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_adm_fee) + Convert.ToInt32(paidFee.adm_fee)).ToString();
                                //cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_security_fee) + Convert.ToInt32(paidFee.security_fee)).ToString();
                                //cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_exam_fee) + Convert.ToInt32(paidFee.exam_fee)).ToString();

                                cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_reg_fee) ).ToString();
                                cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_adm_fee) ).ToString();
                                cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_security_fee) ).ToString();
                                cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(f.rem_exam_fee)).ToString();

                                cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;

                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                con.Close();
                            }
                        }
                    }
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        private int delete_sms_fee_paid(string receiptno) 
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_fee_paid where receipt_no=" + cancelReceiptNumber;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return i;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            FS.SearchTextBox.Clear();
        } 
    }
}
