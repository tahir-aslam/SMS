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
using System.Text.RegularExpressions;

namespace SMS.FeeManagement.FeePaidByAmount
{
    /// <summary>
    /// Interaction logic for FeePaidByAmountWindow.xaml
    /// </summary>
    public partial class FeePaidByAmountWindow : Window
    {
        admission adm_obj;
        List<sms_months> months_list;
        string last_receipt_no;
        List<fee> fee_list;
        List<sms_months> pending_months;
        List<fee> pending_fee_lst;
        FeePaidByAmountPrint FPBAP;
        fee_voucher fv;

        List<sms_months> fine_months;
        List<fee> fine_fee_lst;

        string pending_desc;        
        sms_months sm;

        string fine_desc;
        int fine_amount;

        string rem_reg_fee = "0";
        string rem_adm_fee = "0";
        string rem_tution_fee = "0";
        string rem_security_fee = "0";
        string rem_transport_fee = "0";
        string rem_exam_fee = "0";
        string rem_other_exp = "0";
        string rem_fine_fee = "0";

        string fine_fee = "0";
        string reg_fee = "0";
        string adm_fee = "0";
        string tution_fee = "0";
        string security_fee = "0";
        string transport_fee = "0";
        string exam_fee = "0";
        string other_exp = "0";

        char isActive = 'Y';
        int amount=0;
        int total_amount = 0;
        int paid_amount = 0;
        int rem_amount = 0;
        int pending_amount = 0;
        FeePaidByAmountPage FeeAmountPage;


        public FeePaidByAmountWindow(admission adm, FeePaidByAmountPage feePage)
        {
            InitializeComponent();
            this.adm_obj = adm;
            get_all_months();
            this.FeeAmountPage = feePage;
            //month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;
            int month = DateTime.Now.Month;
            month++;
            if (month <= 4)
            {
                month_cmb.SelectedIndex = month + 8;
            }
            else
            {
                month_cmb.SelectedIndex = month - 4;
            }
            paid_amount_textbox.Focus();
                        
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sm = (sms_months)month_cmb.SelectedItem;
            if (month_cmb.SelectedIndex != 0)
            {
                get_fee_by_month(sm.month_name,adm_obj.id);
                get_pending_fee(Convert.ToInt32(sm.id));
                get_all_fine(Convert.ToInt32(sm.id));
                fill_fee_control();
            }
            else 
            {
            }
        }

        public void get_fee_by_month(string m,string id) 
        {
            fee_list = new List<fee>();
            rem_reg_fee = "0";
            rem_adm_fee = "0";
            rem_tution_fee = "0";
            rem_security_fee = "0";
            rem_transport_fee = "0";
            rem_exam_fee = "0";
            rem_other_exp = "0";

            reg_fee = "0";
            adm_fee = "0";
            tution_fee = "0";
            security_fee = "0";
            transport_fee = "0";
            exam_fee = "0";
            other_exp = "0";

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where std_id = @id && isActive='Y' && session_id =" + MainWindow.session.id;
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

                                transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString());
                                reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString());
                                tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString());
                                exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString());
                                security_fee = Convert.ToString(reader["rem_security_fee"].ToString());
                                other_exp = Convert.ToString(reader["rem_other_exp"].ToString());
                                adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString());
                                fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString());

                                

                                isActive = Convert.ToChar(reader["isActive"]);
                            }
                           
                                fee f = new fee()
                                {
                                    rem_security_fee = Convert.ToString(reader["rem_security_fee"].ToString()),
                                    rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                                    rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                    rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),
                                    rem_other_fee = Convert.ToString(reader["rem_other_exp"].ToString()),
                                    rem_fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString()),
                                    month = Convert.ToString(reader["month"].ToString()),
                                    isActive = Convert.ToChar(reader["isActive"]),
                                    std_id = Convert.ToString(reader["std_id"].ToString()),
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
        }

        public void get_all_fine(int months_id)
        {
            fine_months = new List<sms_months>();
            fine_fee_lst = new List<fee>();
            int monthly_pending = 0;
            string m_name = "";            
            fine_amount = 0;
            fine_desc = "Fine: ";

            for (int i = 1; i <= months_id; i++)
            {
                //get Month name
                m_name = "";
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
                        monthly_pending = 0;
                        monthly_pending = Convert.ToInt32(f.rem_fine_fee);
                        fine_amount = fine_amount + monthly_pending;
                        if (monthly_pending > 0)
                        {
                            fine_desc = fine_desc + m_name + ",";
                            fine_fee_lst.Add(f);
                        }
                        break;
                    }
                }
            }
        }       

        public void get_pending_fee(int months_id)
        {
            pending_months = new List<sms_months>();
            pending_fee_lst = new List<fee>();
            int monthly_pending = 0;
            string m_name = "";
            pending_amount = 0;
            pending_desc = "Pending Fee:";

            for (int i = 1; i < months_id ; i++)
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

                foreach (fee f in fee_list.Where(x => x.isActive == 'Y'))
                {
                    if (f.month == m_name)
                    {
                        monthly_pending = 0;
                        monthly_pending = Convert.ToInt32(f.rem_tution_fee) + Convert.ToInt32(f.rem_other_fee);
                        pending_amount = pending_amount + monthly_pending;
                        if (monthly_pending > 0)
                        {
                            pending_desc = pending_desc + m_name + ",";
                            pending_fee_lst.Add(f);
                        }
                        break;
                    }
                }

            }

        }

        //========      Fill Fee Control      =============================
        public void fill_fee_control()
        {
            amount = 0;
            total_amount = 0;
            paid_amount = 0;
            rem_amount = 0;

            amount = Convert.ToInt32(rem_reg_fee) + Convert.ToInt32(rem_adm_fee) + Convert.ToInt32(rem_exam_fee) + Convert.ToInt32(rem_tution_fee) + Convert.ToInt32(rem_security_fee) + Convert.ToInt32(rem_transport_fee) + Convert.ToInt32(rem_other_exp);
            annualFund.Text = rem_reg_fee;
            pending_months_textbox.Text = pending_desc;
            amount_textbox.Text = amount.ToString();
            pending_amount_textbox.Text = pending_amount.ToString();
            fine_months_textbox.Text = fine_desc;
            fine_amount_textbox.Text = fine_amount.ToString();
            total_amont_textbox.Text= (amount + pending_amount + fine_amount).ToString();
            paid_amount_textbox.Text = (amount + pending_amount+fine_amount).ToString();
        }

        public void save() 
        {
            if (validate())
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Pay " + paid_amount_textbox.Text + " Rs  ?", "Payment Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    get_last_reciept_no();
                    if (total_amont_textbox.Text == paid_amount_textbox.Text)
                    {
                        update_fee();
                        this.Close();
                        //MessageBox.Show("Successfully Paid","Paid",MessageBoxButton.OK,MessageBoxImage.Information);
                        fill_voucher();
                        FPBAP = new FeePaidByAmountPrint(fv);
                        FPBAP.Show();
                        if (submit(fv) > 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("History not saved, Please Contact Your Service Provider");
                        }
                    }
                    else
                    {
                        ////set_pay_fee();                                    
                        update_fee_partial();
                        //MessageBox.Show(" Successfully added");
                        this.Close();

                        fill_voucher();
                        FPBAP = new FeePaidByAmountPrint(fv);
                        FPBAP.Show();
                        if (submit(fv) > 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("History not saved, Please Contact Your Service Provider");
                        }
                    }
                }
            }
        }

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
           
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();               
            }
        }
        //-----------------     Submit Form        -------------------------------
        public int submit(fee_voucher fv)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_fee_history_by_amount(std_id, receipt_no, std_name, father_name, adm_no, month,class_name, section_name, date, adm_fee, reg_fee, tution_fee, exam_fee, securtiy_fee, other_fee, pending_fee, pending_fee_months, total_fee, paid_fee, remaining_fee, amoun_in_words,received_by, session_id, fine_fee, fine_fee_months ) Values(@std_id, @receipt_no, @std_name, @father_name, @adm_no, @month,@class_name, @section_name, @date, @adm_fee, @reg_fee, @tution_fee, @exam_fee, @securtiy_fee, @other_fee, @pending_fee, @pending_fee_months, @total_fee, @paid_fee, @remaining_fee, @amoun_in_words,@received_by, @session_id, @fine_fee, @fine_fee_months)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.std_id;
                        cmd.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.reciept_no;
                        cmd.Parameters.Add("@std_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_name;
                        cmd.Parameters.Add("@father_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.father_name;
                        cmd.Parameters.Add("@adm_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.adm_no;
                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.class_name;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.section_name;
                        cmd.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.date_time;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.rem_adm_fee;
                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.rem_reg_fee;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.rem_tution_fee;
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.rem_exam_fee;
                        cmd.Parameters.Add("@securtiy_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.rem_security_fee;
                        cmd.Parameters.Add("@other_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.rem_other_fee;
                        cmd.Parameters.Add("@pending_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.pending_amount;
                        cmd.Parameters.Add("@pending_fee_months", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.pending_desc;
                        cmd.Parameters.Add("@fine_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.fine_amount;
                        cmd.Parameters.Add("@fine_fee_months", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.fine_desc;
                        cmd.Parameters.Add("@total_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.total;
                        cmd.Parameters.Add("@paid_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.paid;
                        cmd.Parameters.Add("@remaining_fee", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = fv.remaining;
                        cmd.Parameters.Add("@amoun_in_words", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.total_in_words;
                        cmd.Parameters.Add("@received_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.created_by;
                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.Int16).Value = MainWindow.session.id;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        public bool validate() 
        {
            if(paid_amount_textbox.Text == "" ||  Convert.ToInt32(paid_amount_textbox.Text) > Convert.ToInt32(total_amont_textbox.Text))
            {
                MessageBox.Show("Please fix the Amount Paid Textbox","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                paid_amount_textbox.Focus();
                return false;
            }
            return true;
        }

        public void fill_voucher() 
        {
            fv = new fee_voucher();

            fv.institute_logo = MainWindow.ins.institute_logo;
            fv.institute_name = MainWindow.ins.institute_name;
            fv.reciept_no = last_receipt_no;
            fv.date_time = DateTime.Now.ToString("dd MMM yyyy h:mm");
            fv.std_id = adm_obj.id;
            fv.std_name = adm_obj.std_name;
            fv.adm_no = adm_obj.adm_no;
            fv.father_name = adm_obj.father_name;
            fv.class_name = adm_obj.class_name;
            fv.section_name = adm_obj.section_name;
            fv.month = sm.month_name;

            fv.rem_adm_fee = adm_fee;
            fv.rem_reg_fee = reg_fee;
            fv.rem_security_fee = security_fee;
            fv.rem_exam_fee = exam_fee;
            fv.rem_tution_fee = tution_fee;
            fv.rem_other_fee = other_exp;
            fv.pending_desc = pending_desc;
            fv.pending_amount = pending_amount.ToString();
            fv.fine_desc = fine_months_textbox.Text;
            fv.fine_amount = fine_amount_textbox.Text;

            fv.total = total_amont_textbox.Text;
            fv.paid = paid_amount_textbox.Text;
            fv.remaining = (Convert.ToInt32(total_amont_textbox.Text) - Convert.ToInt32(paid_amount_textbox.Text)).ToString();
            fv.total_in_words = NumberToWords(Convert.ToInt32(paid_amount_textbox.Text));

            fv.created_by = MainWindow.emp_login_obj.emp_user_name;

        }

        public void get_last_reciept_no() 
        {
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
                        last_receipt_no= (Convert.ToInt32(last_receipt_no)+1).ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void paid_amount_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                rem_amount_textbox.Text = (Convert.ToInt32(total_amont_textbox.Text) - Convert.ToInt32(paid_amount_textbox.Text)).ToString();
            }
            catch (Exception ex) 
            {
            }
        }

        public int update_fee()
        {
            int i = 1;
            try
            {
                //For pending fee

                foreach (fee fp in pending_fee_lst)
                {
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {
                            cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id =" + MainWindow.session.id;
                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                            cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                            cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            //cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                            con_fee.Open();
                            i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                        }
                    }
                }

                foreach (fee fp in pending_fee_lst)
                {
                    using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_paid = new MySqlCommand())
                        {
                            cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                            cmd_paid.Connection = con_paid;

                            cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                            cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.rem_tution_fee;
                            cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.rem_other_fee;
                            cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                            cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                            cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                            cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;

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
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                        cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }

                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id=" + MainWindow.session.id;
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                        cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sm.month_name;
                        cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        //cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_exam_fee;
                        cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                        con_fee.Open();

                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                    }
                }

                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_last_receipt_no SET last_receipt_no=@last_receipt_no";
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@last_receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;


                        con_fee.Open();

                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                    }
                }

                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                        cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_reg_fee;
                        cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_adm_fee;
                        cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_security_fee;
                        cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_exam_fee;
                        cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_tution_fee;
                        cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_other_exp;
                        cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sm.month_name;
                        cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;

                        con_paid.Open();
                        cmd_paid.ExecuteScalar();
                    }
                }

                //Fine
                foreach (fee fp in fine_fee_lst)
                {
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {
                            cmd_fee.CommandText = "Update sms_fee SET rem_fine_fee=@rem_fine_fee WHERE std_id = @id && month = @month && session_id =" + MainWindow.session.id;
                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                            cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                            cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";           

                            con_fee.Open();
                            i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                        }
                    }
                }

                foreach (fee fp in fine_fee_lst)
                {
                    using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_paid = new MySqlCommand())
                        {
                            cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id,fine_fee_paid)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id,@fine_fee_paid)";
                            cmd_paid.Connection = con_paid;

                            cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                            cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@fine_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.rem_fine_fee;
                            cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                            cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                            cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                            cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;

                            con_paid.Open();
                            cmd_paid.ExecuteScalar();
                            con_paid.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        public int update_fee_partial()
        {
            int i = 1;
            int total = Convert.ToInt32(paid_amount_textbox.Text);
            try
            {
                //For pending fee
                foreach (fee fp in pending_fee_lst)
                {
                    if (total > 0)
                    {
                        using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_fee = new MySqlCommand())
                            {
                                cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id =" + MainWindow.session.id;
                                cmd_fee.Connection = con_fee;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                                cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                                //tution fee
                                if (total >= Convert.ToInt32(fp.rem_tution_fee))
                                {
                                    cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    total = total - Convert.ToInt32(fp.rem_tution_fee);
                                }
                                else
                                {
                                    cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(fp.rem_tution_fee) - total;
                                    fp.rem_tution_fee = total.ToString();
                                    total = 0;

                                }

                                //transport fee
                                if (total >= Convert.ToInt32(fp.rem_transport_fee))
                                {
                                    cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    total = total - Convert.ToInt32(fp.rem_transport_fee);
                                }
                                else
                                {
                                    cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(fp.rem_transport_fee) - total;
                                    fp.rem_transport_fee = total.ToString();
                                    total = 0;

                                }
                                //cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";


                                //other fee
                                if (total >= Convert.ToInt32(fp.rem_other_fee))
                                {
                                    cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    total = total - Convert.ToInt32(fp.rem_other_fee);
                                }
                                else
                                {
                                    cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(fp.rem_other_fee) - total;
                                    fp.rem_other_fee = total.ToString();
                                    total = 0;

                                }

                                con_fee.Open();
                                i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                            }
                        }
                    }
                    else
                    {
                        fp.rem_other_fee = "0";
                        fp.rem_tution_fee = "0";
                        fp.rem_transport_fee = "0";
                    }
                }
                foreach (fee fp in pending_fee_lst)
                {
                    using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_paid = new MySqlCommand())
                        {
                            cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                            cmd_paid.Connection = con_paid;

                            cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                            cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                            cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.rem_tution_fee;
                            cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.rem_other_fee;
                            cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fp.month;
                            cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                            cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                            cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;

                            con_paid.Open();
                            cmd_paid.ExecuteScalar();
                            con_paid.Close();
                        }
                    }
                }

                //current month
                if (total > 0)
                {
                    // for current month monthly fee
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {
                            cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id=" + MainWindow.session.id;
                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                            cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sm.month_name;
                            //tution fee
                            if (total >= Convert.ToInt32(rem_tution_fee))
                            {
                                cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                total = total - Convert.ToInt32(rem_tution_fee);
                            }
                            else
                            {
                                cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_tution_fee) - total;
                                rem_tution_fee = total.ToString();
                                total = 0;
                            }

                            //transport fee
                            if (total >= Convert.ToInt32(rem_transport_fee))
                            {
                                cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                total = total - Convert.ToInt32(rem_transport_fee);
                            }
                            else
                            {
                                cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_transport_fee) - total;
                                rem_transport_fee = total.ToString();
                                total = 0;
                            }

                            //Other fee
                            if (total >= Convert.ToInt32(rem_other_exp))
                            {
                                cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                total = total - Convert.ToInt32(rem_other_exp);
                            }
                            else
                            {
                                cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_other_exp) - total;
                                rem_other_exp = total.ToString();
                                total = 0;
                            }
                            //cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = paid_exam_fee;


                            con_fee.Open();
                            i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                        }
                    }
                }
                else
                {
                    rem_tution_fee = "0";
                    rem_transport_fee = "0";
                    rem_other_exp = "0";
                }


                // Annual fee
                if (total > 0)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_fee SET rem_reg_fee=@rem_reg_fee,rem_adm_fee=@rem_adm_fee,rem_security_fee=@rem_security_fee,rem_exam_fee=@rem_exam_fee,date_time=@date_time,created_by=@created_by WHERE std_id = @id && session_id=@session_id";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                            cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                            
                            // annual fund
                            if (total >= Convert.ToInt32(rem_reg_fee))
                            {
                                cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                total = total - Convert.ToInt32(rem_reg_fee);
                            }
                            else
                            {
                                cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_reg_fee) - total;
                                rem_reg_fee = total.ToString();
                                total = 0;

                            }

                            if (total >= Convert.ToInt32(rem_adm_fee))
                            {
                                cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                total = total - Convert.ToInt32(rem_adm_fee);
                            }
                            else
                            {
                                cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_adm_fee) - total;
                                rem_adm_fee = total.ToString();
                                total = 0;

                            }

                            // security fee
                            if (total >= Convert.ToInt32(rem_security_fee))
                            {
                                cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                total = total - Convert.ToInt32(rem_security_fee);
                            }
                            else
                            {
                                cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_security_fee) - total;
                                rem_security_fee = total.ToString();
                                total = 0;

                            }


                            //exam fee
                            if (total >= Convert.ToInt32(rem_exam_fee))
                            {
                                cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                total = total - Convert.ToInt32(rem_exam_fee);
                            }
                            else
                            {
                                cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(rem_exam_fee) - total;
                                rem_exam_fee = total.ToString();
                                total = 0;

                            }

                            cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_name;

                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                }
                else 
                {
                    rem_adm_fee = "0";
                    rem_reg_fee = "0";
                    rem_exam_fee = "0";
                    rem_security_fee = "0";
                }
                

               

                // Pay annual feeses and current month
                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                        cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_reg_fee;
                        cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_adm_fee;
                        cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_security_fee;
                        cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_exam_fee;
                        cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_tution_fee;
                        cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_other_exp;
                        cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sm.month_name;
                        cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;

                        con_paid.Open();
                        cmd_paid.ExecuteScalar();
                    }
                }
                      

                //if(total > 0)
                //{
                //    // Fine Fee                    
                //    foreach (fee fv in fine_fee_lst)
                //    {
                //        if (total > 0 && fv.rem_fine_fee != "0")
                //        {
                //            // for tution fee
                //            if ((total - Convert.ToInt32(fv.rem_fine_fee)) >= 0)
                //            {
                //                total = total - Convert.ToInt32(fv.rem_fine_fee);
                //            }
                //            else
                //            {
                //                fv.rem_fine_fee = total.ToString();
                //                total = 0;
                //            }
                //        }
                //        else
                //        {
                //            fv.rem_fine_fee = "0";
                //        }                        
                //    }
                //}

                // Update All Fine  
                if (total > 0)
                {
                    foreach (fee fv in fine_fee_lst)
                    {
                        if (fv.rem_fine_fee != "0" || !string.IsNullOrEmpty(fv.rem_fine_fee))
                        {
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {
                                    cmd_fee.CommandText = "Update sms_fee SET rem_fine_fee=@rem_fine_fee WHERE std_id = @id && month = @month && session_id =" + MainWindow.session.id;
                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    if (total >= Convert.ToInt32(fv.rem_fine_fee))
                                    {
                                        cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                        total = total - Convert.ToInt32(fv.rem_fine_fee);
                                    }
                                    else
                                    {
                                        cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(fv.rem_fine_fee) - total;
                                        fv.rem_fine_fee = total.ToString();
                                        total = 0;

                                    }
                                    cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    //cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_fine_fee) - Convert.ToInt32(fv.rem_fine_fee);

                                    con_fee.Open();
                                    i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                                    con_fee.Close();
                                }
                            }

                        }
                    }

                    //insert all fine in sms_fee_paid
                    foreach (fee fv in fine_fee_lst)
                    {
                        if (fv.rem_fine_fee != "0" || !string.IsNullOrEmpty(fv.rem_fine_fee))
                        {
                            using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_paid = new MySqlCommand())
                                {
                                    cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id,fine_fee_paid)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id,@rem_fine_fee)";
                                    cmd_paid.Connection = con_paid;

                                    cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                    cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_fine_fee;
                                    cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                    cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                    cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;

                                    con_paid.Open();
                                    cmd_paid.ExecuteScalar();
                                    con_paid.Close();
                                }
                            }
                        }
                    }

                }

               


                //regulare fee               

               

                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_last_receipt_no SET last_receipt_no=@last_receipt_no";
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@last_receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_receipt_no;


                        con_fee.Open();

                        i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //===========      Digits to words              ================================
        public string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "And ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FeePaidByAmountHistory fpah = new FeePaidByAmountHistory(adm_obj);
            fpah.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            FeeAmountPage.SearchTextBox.Clear();
        }     
    }
}
