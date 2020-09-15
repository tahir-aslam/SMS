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
using System.Text.RegularExpressions;
using SMS.Models;
using MySql.Data.MySqlClient;
using SMS.FeeManagement.FeePaidByVoucher;

namespace SMS.FeeManagement.FeePaidByVoucher
{
    /// <summary>
    /// Interaction logic for FeePaidByVoucherPage.xaml
    /// </summary>
    public partial class FeePaidByVoucherPage : Page
    {
        List<fee_voucher> voucher_list;
        List<fee_voucher> voucher_list_partial;
        List<fee> fee_list;
        fee_voucher fv_obj;
        fee_voucher fv_obj_grid;
        fee_voucher fee_obj_temp;
        FeePaidByVoucherWindow fpbvw;
        public static string amount="";
        public static string status = "N";
        public static DateTime paidDate{get;set;}

        public FeePaidByVoucherPage()
        {
            InitializeComponent();            
            //SearchTextBox.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(SearchTextBox.Text!="")
                {
                    status = "N";
                    get_voucher(SearchTextBox.Text);
                    if (voucher_list.Count > 0)
                    {
                        fv_obj = voucher_list[0];
                        get_fee_data(fv_obj.std_id);
                        get_std_name();
                        
                        fpbvw = new FeePaidByVoucherWindow(fv_obj_grid);
                        fpbvw.ShowDialog();
                        if (status == "Y")
                        {
                            //std_name_tb.Text = fv_obj_grid.std_name;
                            MessageBoxResult mbr = MessageBox.Show("Are You Want To Pay " + amount + " Rs  ?", "Payment confirmatino Confirmation", MessageBoxButton.YesNo);
                            if (mbr == MessageBoxResult.Yes)
                            {
                                if (amount == fv_obj_grid.total)
                                {
                                    if (check_fees())
                                    {
                                        pay_voucher();
                                        MessageBox.Show("Successfully added");

                                        fee_voucher_grid.Items.Insert(0, fv_obj_grid);
                                        fee_voucher_grid.Items.Refresh();
                                        SearchTextBox.Text = "";
                                        SearchTextBox.Focus();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Have you changed fee Or Added Fine after printing vouchers ??????");
                                    }
                                }
                                else
                                {
                                    set_pay_fee();                                    
                                    pay_voucher();
                                    MessageBox.Show("Successfully added");

                                    fv_obj_grid.total = amount;
                                    fee_voucher_grid.Items.Insert(0, fv_obj_grid);
                                    fee_voucher_grid.Items.Refresh();
                                    SearchTextBox.Text = "";
                                    SearchTextBox.Focus();
                                }
                            }
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Reciept# not available");
                    }
                }
            }
        }

        public void pay_voucher_partial() 
        {
            int total_amount =Convert.ToInt32(amount);
            int i = 0;
            try
            {
                //update sms_fee annual fund 
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i == 0 && fv.total != "0")
                    {
                        foreach (fee f in fee_list.Where(x => x.month == fv.month))
                        {                           
                            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_fee SET rem_reg_fee=@rem_reg_fee,rem_adm_fee=@rem_adm_fee,date_time=@date_time,created_by=@created_by WHERE std_id = @id && session_id ="+MainWindow.session.id;
                                    cmd.Connection = con;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_reg_fee) - Convert.ToInt32(fv.rem_reg_fee);
                                    cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_adm_fee) - Convert.ToInt32(fv.rem_adm_fee);

                                    cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                    cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;

                                    con.Open();
                                    cmd.ExecuteScalar();
                                    con.Close();
                                }
                            }
                        }
                    }
                    i++;
                }

                foreach (fee_voucher fv in voucher_list)
                {
                    if (i != 0)
                    {
                        //set sms_fee for pending

                        using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_fee = new MySqlCommand())
                            {
                                cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_exam_fee=@rem_exam_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id ="+MainWindow.session.id;
                                cmd_fee.Connection = con_fee;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                                con_fee.Open();
                                cmd_fee.ExecuteScalar();
                                con_fee.Close();
                            }
                        }
                    }
                    i++;
                }

                // insert sms_fee_paid for pending
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i != 0)
                    {
                        if (fv.total != "0")
                        {
                            using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_paid = new MySqlCommand())
                                {
                                    cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                                    cmd_paid.Connection = con_paid;

                                    cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                    cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_exam_fee;
                                    cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_transport_fee;
                                    cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_tution_fee;
                                    cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_other_fee;
                                    cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = paidDate;
                                    cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                    cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.reciept_no;

                                    con_paid.Open();
                                    cmd_paid.ExecuteScalar();
                                    con_paid.Close();
                                }
                            }
                        }
                    }
                    i++;
                }


                

                //update sms_fee monthly fee
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i == 0)
                    {
                        foreach (fee f in fee_list.Where(x => x.month == fv.month))
                        {
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {
                                    cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_exam_fee=@rem_exam_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id ="+MainWindow.session.id;
                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_tution_fee) - Convert.ToInt32(fv.rem_tution_fee);
                                    cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_transport_fee) - Convert.ToInt32(fv.rem_transport_fee);
                                    cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_exam_fee) - Convert.ToInt32(fv.rem_exam_fee);
                                    cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_other_fee) - Convert.ToInt32(fv.rem_other_fee);

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }
                        }
                    }
                    i++;
                }

                // fee paid for current month
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i == 0 && fv.total != "0")
                    {
                        using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_paid = new MySqlCommand())
                            {
                                cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id)";
                                cmd_paid.Connection = con_paid;

                                cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_reg_fee;
                                cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_adm_fee;
                                cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_exam_fee;
                                cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_transport_fee;
                                cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_tution_fee;
                                cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_exam_fee;
                                cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = paidDate;
                                cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_name;
                                cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.reciept_no;

                                con_paid.Open();
                                cmd_paid.ExecuteScalar();
                                con_paid.Close();
                            }
                        }
                    }
                    i++;
                }

                // update reciept no isActve=N
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_fee_vouchers SET isActive=@isActive WHERE reciept_no = @reciept_no";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@reciept_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_obj.reciept_no;
                        cmd.Parameters.Add("@isActive", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "N";
                        con.Open();
                        cmd.ExecuteScalar();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool check_fees()
        {
            foreach (fee_voucher fv in voucher_list) 
            {
                foreach (fee f in fee_list.Where(x=>x.month == fv.month)) 
                {
                    if(fv.rem_adm_fee == f.rem_adm_fee && fv.rem_reg_fee==f.rem_reg_fee && fv.rem_tution_fee==f.rem_tution_fee && fv.rem_transport_fee==f.rem_transport_fee && fv.rem_exam_fee==f.rem_exam_fee && fv.rem_other_fee==f.rem_other_fee && fv.rem_security_fee==f.rem_security_fee)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
                 
        public void get_std_name() 
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && id=" + fv_obj.std_id+ "&& session_id="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fv_obj_grid = new fee_voucher()
                            {
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            };
                            fv_obj_grid.total = fv_obj.total;
                            fv_obj_grid.reciept_no = fv_obj.reciept_no;
                            fv_obj_grid.month = fv_obj.month;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public void pay_voucher() 
        {
            int i = 0;
            try
            { 
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i != 0 && fv.rem_fine_fee == "0")
                    {
                        //update sms_fee for pending
                        foreach (fee f in fee_list.Where(x => x.month == fv.month))
                        {
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {
                                    cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id="+MainWindow.session.id;
                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_tution_fee) - Convert.ToInt32(fv.rem_tution_fee);
                                    cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_transport_fee) - Convert.ToInt32(fv.rem_transport_fee);                                    
                                    cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_other_fee) - Convert.ToInt32(fv.rem_other_fee);

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }
                        }
                    }
                    i++;
                }

                // insert sms_fee_paid for pending
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i != 0)
                    {
                        if (fv.total != "0" || fv.rem_tution_fee !="0" || fv.rem_other_fee != "0")
                        {
                            using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_paid = new MySqlCommand())
                                {
                                    cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id,security_fee_paid)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id,@security_fee_paid)";
                                    cmd_paid.Connection = con_paid;

                                    cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                    cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";                                    
                                    cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_transport_fee;
                                    cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_tution_fee;
                                    cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_other_fee;
                                    cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = paidDate;
                                    cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                    cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.reciept_no;

                                    con_paid.Open();
                                    cmd_paid.ExecuteScalar();
                                    con_paid.Close();
                                }
                            }
                        }
                    }
                    i++;
                }

                // Update All Fine
                foreach (fee_voucher fv in voucher_list)
                {
                    if (fv.rem_fine_fee != "0" && !string.IsNullOrEmpty(fv.rem_fine_fee))
                    {
                        foreach (fee f in fee_list.Where(x => x.month == fv.month).Where(y=>y.rem_fine_fee != "0"))
                        {
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {
                                    cmd_fee.CommandText = "Update sms_fee SET rem_fine_fee=@rem_fine_fee WHERE std_id = @id && month = @month && session_id =" + MainWindow.session.id;
                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_fine_fee) - Convert.ToInt32(fv.rem_fine_fee);

                                    con_fee.Open();
                                    i = Convert.ToInt32(cmd_fee.ExecuteScalar());
                                    con_fee.Close();
                                }
                            }
                        }
                    }
                }

                //insert all fine in sms_fee_paid
                foreach (fee_voucher fv in voucher_list)
                {
                    if (fv.rem_fine_fee != "0" && !string.IsNullOrEmpty(fv.rem_fine_fee))
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
                                cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = paidDate;
                                cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.reciept_no;

                                con_paid.Open();
                                cmd_paid.ExecuteScalar();
                                con_paid.Close();
                            }
                        }
                    }
                }

                //update sms_fee annual fee 
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i == 0 && fv.total != "0")
                    {
                        foreach (fee f in fee_list.Where(x=>x.month == fv.month))
                        {
                            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_fee SET rem_reg_fee=@rem_reg_fee,rem_adm_fee=@rem_adm_fee,rem_exam_fee=@rem_exam_fee,rem_security_fee=@rem_security_fee,date_time=@date_time,created_by=@created_by WHERE std_id = @id && session_id=" + MainWindow.session.id;
                                    cmd.Connection = con;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_reg_fee) - Convert.ToInt32(fv.rem_reg_fee);
                                    cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_adm_fee) - Convert.ToInt32(fv.rem_adm_fee);
                                    cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_exam_fee) - Convert.ToInt32(fv.rem_exam_fee);
                                    cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_security_fee) - Convert.ToInt32(fv.rem_security_fee);

                                    cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                    cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;

                                    con.Open();
                                    cmd.ExecuteScalar();
                                    con.Close();
                                }
                            }
                        }
                    }
                    i++;
                }

                //update sms_fee monthly fee current month
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i == 0)
                    {
                        foreach (fee f in fee_list.Where(x => x.month == fv.month))
                        {
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {
                                    cmd_fee.CommandText = "Update sms_fee SET rem_tution_fee=@rem_tution_fee,rem_transport_fee=@rem_transport_fee,rem_other_exp=@rem_other_exp WHERE std_id = @id && month = @month && session_id="+MainWindow.session.id;
                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                    cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                    cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_tution_fee) - Convert.ToInt32(fv.rem_tution_fee);
                                    cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_transport_fee) - Convert.ToInt32(fv.rem_transport_fee);
                                    //cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_exam_fee) - Convert.ToInt32(fv.rem_exam_fee);
                                    cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Convert.ToInt32(f.rem_other_fee) - Convert.ToInt32(fv.rem_other_fee);

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }
                        }
                    }
                    i++;
                }

                // fee paid for current month
                i = 0;
                foreach (fee_voucher fv in voucher_list)
                {
                    if (i == 0 && fv.total != "0")
                    {
                        using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_paid = new MySqlCommand())
                            {
                                cmd_paid.CommandText = "insert into sms_fee_paid (std_id,reg_fee_paid,adm_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,session_id,security_fee_paid)Values(@std_id,@reg_fee_paid,@adm_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@session_id,@security_fee_paid)";
                                cmd_paid.Connection = con_paid;

                                cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                                cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_reg_fee;
                                cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_adm_fee;
                                cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_exam_fee;
                                cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_security_fee;
                                cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_transport_fee;
                                cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_tution_fee;
                                cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_other_fee;
                                cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                                cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = paidDate;
                                cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.reciept_no;

                                con_paid.Open();
                                cmd_paid.ExecuteScalar();
                                con_paid.Close();
                            }
                        }
                    }
                    i++;
                }

                // update reciept no isActve=N
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_fee_vouchers SET isActive=@isActive WHERE reciept_no = @reciept_no";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@reciept_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_obj.reciept_no;
                        cmd.Parameters.Add("@isActive", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "N";
                        con.Open();
                        cmd.ExecuteScalar();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        //========      Get fee data          ====================================
        public void get_fee_data(string id)
        {
            fee_list = new List<fee>();            

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where std_id = @id && isActive='Y' && session_id="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;                        

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {                           
                            fee f = new fee()
                            {
                                rem_adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString()),
                                rem_reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString()),
                                rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                                rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),
                                rem_other_fee = Convert.ToString(reader["rem_other_exp"].ToString()),
                                rem_fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString()),
                                rem_security_fee = Convert.ToString(reader["rem_security_fee"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),                                
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public void get_voucher(string reciept_no) 
        {
            voucher_list = new List<fee_voucher>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_vouchers where reciept_no=@reciept_no && isActive=@isActive";
                        cmd.Parameters.Add("@reciept_no", MySqlDbType.VarChar).Value = reciept_no;
                        cmd.Parameters.Add("@isActive", MySqlDbType.VarChar).Value = 'Y';

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee_voucher fv = new fee_voucher()
                            {
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                reciept_no = Convert.ToString(reader["reciept_no"].ToString()),
                                total = Convert.ToString(reader["total"].ToString()),
                                rem_adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString()),
                                rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),
                                rem_other_fee = Convert.ToString(reader["rem_other_fee"].ToString()),
                                rem_reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                                rem_security_fee = Convert.ToString(reader["rem_security_fee"].ToString()),
                                rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                rem_fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString()),
                            };
                            voucher_list.Add(fv);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void set_pay_fee() 
        {
            int total_amount = Convert.ToInt32(amount);
            voucher_list_partial = new List<fee_voucher>();
            int i = 0;

            foreach (fee_voucher fv in voucher_list) 
            {
                // pay annual feeeses               
                if (i == 0)
                {
                    fee_obj_temp = new fee_voucher();
                    fee_obj_temp = fv;
                    if (total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_adm_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_adm_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_adm_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_adm_fee = "0";
                    }

                    if (total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_reg_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_reg_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_reg_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_reg_fee = "0";
                    }
                    //exam fee
                    if (total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_exam_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_exam_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_exam_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_exam_fee = "0";
                    }
                    //security fee
                    if (total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_security_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_security_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_security_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_security_fee = "0";
                    }
                    voucher_list_partial.Add(fee_obj_temp);
                }
                i++;
                
            }

            // pending monthly feeses
            i = 0;
            foreach (fee_voucher fv in voucher_list) 
            {
                if(i != 0 && total_amount >0)
                {
                    fee_obj_temp = new fee_voucher();
                    fee_obj_temp = fv;

                    // for tution fee
                    if ((total_amount - Convert.ToInt32(fv.rem_tution_fee)) >= 0)
                    {
                        total_amount = total_amount - Convert.ToInt32(fv.rem_tution_fee);
                    }
                    else
                    {
                        fee_obj_temp.rem_tution_fee = total_amount.ToString();
                        total_amount = 0;
                    }

                    // for other
                    if(total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_other_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_other_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_other_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_other_fee = "0";
                    }

                    ////// for Exam fee
                    ////if (total_amount > 0)
                    ////{
                    ////    if ((total_amount - Convert.ToInt32(fv.rem_exam_fee)) >= 0)
                    ////    {
                    ////        total_amount = total_amount - Convert.ToInt32(fv.rem_exam_fee);
                    ////    }
                    ////    else
                    ////    {
                    ////        fee_obj_temp.rem_exam_fee = total_amount.ToString();
                    ////        total_amount = 0;
                    ////    }
                    ////}
                    //else
                    //{
                    //    fee_obj_temp.rem_exam_fee = "0";
                    //}

                    // for transport fee
                    if (total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_transport_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_transport_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_transport_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_transport_fee = "0";
                    }

                    // Fine
                    voucher_list_partial.Add(fee_obj_temp);
                }
                i++;
            }

            // current month feeses            
            foreach (fee_voucher fv in voucher_list)
            {
                if (total_amount > 0 ) 
                {
                    
                    fee_obj_temp = new fee_voucher();
                    fee_obj_temp = fv;

                    // for tution fee
                    if ((total_amount - Convert.ToInt32(fv.rem_tution_fee)) >= 0)
                    {
                        total_amount = total_amount - Convert.ToInt32(fv.rem_tution_fee);
                    }
                    else
                    {
                        fee_obj_temp.rem_tution_fee = total_amount.ToString();
                        total_amount = 0;
                    }

                    // for other
                    if (total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_other_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_other_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_other_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_other_fee = "0";
                    }

                    //// for Exam fee
                    //if (total_amount > 0)
                    //{
                    //    if ((total_amount - Convert.ToInt32(fv.rem_exam_fee)) >= 0)
                    //    {
                    //        total_amount = total_amount - Convert.ToInt32(fv.rem_exam_fee);
                    //    }
                    //    else
                    //    {
                    //        fee_obj_temp.rem_exam_fee = total_amount.ToString();
                    //        total_amount = 0;
                    //    }
                    //}
                    //else
                    //{
                    //    fee_obj_temp.rem_exam_fee = "0";
                    //}

                    // for transport fee
                    if (total_amount > 0)
                    {
                        if ((total_amount - Convert.ToInt32(fv.rem_transport_fee)) >= 0)
                        {
                            total_amount = total_amount - Convert.ToInt32(fv.rem_transport_fee);
                        }
                        else
                        {
                            fee_obj_temp.rem_transport_fee = total_amount.ToString();
                            total_amount = 0;
                        }
                    }
                    else
                    {
                        fee_obj_temp.rem_transport_fee = "0";
                    }

                    //set current month monthly feeses
                    foreach (fee_voucher fv1 in voucher_list_partial)
                    {
                        fv1.rem_tution_fee = fee_obj_temp.rem_tution_fee;
                        fv1.rem_other_fee = fee_obj_temp.rem_other_fee;
                        //fv1.rem_exam_fee = fee_obj_temp.rem_exam_fee;
                        fv1.rem_transport_fee = fee_obj_temp.rem_transport_fee;
                        break;
                    }                    
                }
                else 
                {
                    //set current month monthly feeses
                    foreach (fee_voucher fv1 in voucher_list_partial)
                    {
                        fv1.rem_tution_fee = "0";
                        fv1.rem_other_fee = "0";
                       // fv1.rem_exam_fee = "0";
                        fv1.rem_transport_fee = "0";
                        break;
                    }
                }
                break;
            }            
             // Fine Fee
            i = 0;
            foreach (fee_voucher fv in voucher_list)
            {
                if (i != 0 && total_amount > 0 && fv.rem_fine_fee != "0")
                {
                    fee_obj_temp = new fee_voucher();
                    fee_obj_temp = fv;

                    // for tution fee
                    if ((total_amount - Convert.ToInt32(fv.rem_fine_fee)) >= 0)
                    {
                        total_amount = total_amount - Convert.ToInt32(fv.rem_fine_fee);
                    }
                    else
                    {
                        fee_obj_temp.rem_fine_fee = total_amount.ToString();
                        total_amount = 0;
                    }
                }
                else 
                {
                    fee_obj_temp.rem_fine_fee = "0";
                }
                i++;
            }

            voucher_list = new List<fee_voucher>();
            voucher_list = voucher_list_partial;
        }
    }
}
