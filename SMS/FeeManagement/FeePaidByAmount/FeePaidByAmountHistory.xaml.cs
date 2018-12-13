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

namespace SMS.FeeManagement.FeePaidByAmount
{
    /// <summary>
    /// Interaction logic for FeePaidByAmountHistory.xaml
    /// </summary>
    public partial class FeePaidByAmountHistory : Window
    {
        List<fee_voucher> feeHistoryList;
        List<fee> paid_fee_list;
        fee_voucher feeVoucher;
        admission admObj;

        public FeePaidByAmountHistory(admission adm)
        {
            InitializeComponent();

            this.admObj = adm;
            getAllFeeHistory(adm);
            get_fee_history(adm);
            setPaidFeeHistory();
             
            voucherHistoryLB.ItemsSource = feeHistoryList.OrderByDescending(x => 
            {
                try
                {
                    return Convert.ToInt32(x.reciept_no);
                }
                catch(Exception ex)
                {
                    return 0;
                }
            });
        }
        public void getAllFeeHistory(admission adm) 
        {
            feeHistoryList = new List<fee_voucher>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_history_by_amount where std_id="+adm.id+" && session_id="+MainWindow.session.id;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee_voucher fv = new fee_voucher()
                            {
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                reciept_no = Convert.ToString(reader["receipt_no"].ToString()),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                father_name = Convert.ToString(reader["father_name"].ToString()),
                                adm_no = Convert.ToString(reader["adm_no"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                date_time = Convert.ToString(reader["date"].ToString()),
                                rem_adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                rem_exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                rem_other_fee = Convert.ToString(reader["other_fee"].ToString()),
                                rem_reg_fee = Convert.ToString(reader["reg_fee"].ToString()),                                
                                rem_security_fee = Convert.ToString(reader["securtiy_fee"].ToString()),
                                rem_tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                pending_amount = Convert.ToString(reader["pending_fee"].ToString()),
                                pending_desc = Convert.ToString(reader["pending_fee_months"].ToString()),
                                fine_amount = Convert.ToString(reader["fine_fee"].ToString()),
                                fine_desc = Convert.ToString(reader["fine_fee_months"].ToString()),
                                total = Convert.ToString(reader["total_fee"].ToString()),
                                paid = Convert.ToString(reader["paid_fee"].ToString()),
                                remaining = Convert.ToString(reader["remaining_fee"].ToString()),
                                total_in_words = Convert.ToString(reader["amoun_in_words"].ToString()),
                                created_by = Convert.ToString(reader["received_by"].ToString()),  
                                institute_name= MainWindow.ins.institute_name,
                                institute_logo= MainWindow.ins.institute_logo,
                            };
                            feeHistoryList.Add(fv);

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
        public void get_fee_history(admission adm)
        {
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where std_id =" + adm.id + "&& session_id = " + MainWindow.session.id;

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
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                            };
                            if (feeHistoryList.Select(x => x.reciept_no).Contains(paid_fee.receipt_no))
                            {

                            }
                            else 
                            {
                                paid_fee_list.Add(paid_fee);
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

        public void setPaidFeeHistory() 
        {
            int total = 0;
            int tutionFee = 0;
            int otherFee = 0;
            int pendingAmount = 0;
            int fine_amount = 0;
            int count = 0;
            int check = 1;
            
            foreach(fee f in paid_fee_list.GroupBy(x=>x.receipt_no).Select(y=>y.FirstOrDefault()))
            {
                int amount = 0;
                tutionFee = 0;
                otherFee = 0;
                pendingAmount = 0;
                fine_amount = 0;
                feeVoucher = new fee_voucher();
                total = 0;
                count = 0;
                check = 1;

                feeVoucher.institute_logo = MainWindow.ins.institute_logo;
                feeVoucher.institute_name = MainWindow.ins.institute_name;
                feeVoucher.reciept_no = f.receipt_no;
                feeVoucher.created_by = f.created_by;
                feeVoucher.std_name = admObj.std_name;
                feeVoucher.father_name = admObj.father_name;
                feeVoucher.class_name = admObj.class_name;
                feeVoucher.section_name = admObj.section_name;
                feeVoucher.adm_no = admObj.adm_no;
                feeVoucher.pending_desc = "Pending Fee:";
                feeVoucher.fine_desc = "Fine:";                

                count = paid_fee_list.Where(x => x.receipt_no == f.receipt_no).Count();
                foreach(fee paidFee in paid_fee_list.Where(x=>x.receipt_no == f.receipt_no))
                {

                    if (count == check)
                    {
                        feeVoucher.month = paidFee.month;
                        feeVoucher.rem_reg_fee = paidFee.reg_fee;
                        feeVoucher.rem_adm_fee = paidFee.adm_fee;
                        feeVoucher.rem_exam_fee = paidFee.exam_fee;
                        feeVoucher.rem_security_fee = paidFee.security_fee;
                        feeVoucher.rem_tution_fee = paidFee.tution_fee;
                        feeVoucher.rem_other_fee = paidFee.other_expenses;                        
                    }
                    else if (Convert.ToInt32(paidFee.tution_fee) + Convert.ToInt32(paidFee.other_expenses) > 0)
                    {
                        amount = Convert.ToInt32(paidFee.tution_fee) + Convert.ToInt32(paidFee.other_expenses);
                        pendingAmount = pendingAmount + amount;
                        if(amount > 0)
                        {
                            feeVoucher.pending_desc = feeVoucher.pending_desc + " " + paidFee.month;
                        }                    
                    }
                    else if(paidFee.fine_fee != "0")
                    {
                        feeVoucher.fine_desc = feeVoucher.fine_desc + " " + paidFee.month;
                        fine_amount = fine_amount +  Convert.ToInt32(paidFee.fine_fee);
                    }
                    

                    try
                    {
                        total = total + Convert.ToInt32(paidFee.reg_fee) + Convert.ToInt32(paidFee.adm_fee) + Convert.ToInt32(paidFee.exam_fee) + Convert.ToInt32(paidFee.security_fee) + Convert.ToInt32(paidFee.tution_fee) + Convert.ToInt32(paidFee.other_expenses) + Convert.ToInt32(paidFee.fine_fee);                        
                    }
                    catch(Exception ex)
                    {
                    }

                    feeVoucher.date_time = paidFee.date_time.ToString("dd-MMM-yy");
                    feeVoucher.total = total.ToString();
                    feeVoucher.paid = total.ToString();
                    feeVoucher.remaining = "0";
                    feeVoucher.pending_amount = pendingAmount.ToString();
                    feeVoucher.fine_amount = fine_amount.ToString();

                    check++;
                }
                feeHistoryList.Add(feeVoucher);
            }
        }

        private void print_button_Click(object sender, RoutedEventArgs e)
        {
            fee_voucher fv = ((FrameworkElement)sender).DataContext as fee_voucher;
            this.Close();
            FeePaidByAmountPrint fpbap = new FeePaidByAmountPrint(fv);
            fpbap.ShowDialog();
            
        }

        
    }
}
