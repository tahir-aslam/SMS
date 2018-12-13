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
using SMS.DAL;
using System.Text.RegularExpressions;

namespace SMS.FeesManagement.FeesCollectionByVoucher
{
    /// <summary>
    /// Interaction logic for FeesCollectionByVoucherPage.xaml
    /// </summary>
    public partial class FeesCollectionByVoucherPage : Page
    {
        FeesDAL feesDAL;
        List<sms_fees> fees_list;
        List<sms_fees> unPaidFeesList;
        List<sms_fees> paidFeesList;
        List<sms_fees> feesListToBePaid;
        int unPaidTotalAmount = 0;
        FeesCollectionByVoucherWindow window;
        int total_discount = 0;
        int total_waveOff = 0;

        public FeesCollectionByVoucherPage()
        {
            InitializeComponent();
            feesDAL = new FeesDAL();
            fees_list = new List<sms_fees>();
            unPaidFeesList = new List<sms_fees>();
            paidFeesList = new List<sms_fees>();

            date_picker.SelectedDate = DateTime.Now;
            SearchTextBox.Focus();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(SearchTextBox.Text != "")
                {
                    string text = SearchTextBox.Text;
                    fees_list = feesDAL.getAllFeesVoucherByReceiptNo(Convert.ToInt32(text)).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList();
                    if (fees_list.Count > 0)
                    {
                        window = new FeesCollectionByVoucherWindow(fees_list.First());
                        window.ShowDialog();
                        if (window.isPaid)
                        {
                            unPaidFeesList = feesDAL.getAllUnPaidFeesByStdId(fees_list.First().std_id).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList();

                            if (checkFees())
                            {
                                if (unPaidTotalAmount == Convert.ToInt32(window.total_TB.Text))
                                {
                                    feesListToBePaid = getFeesListToBePaid();
                                    List<sms_fees> historyList = getFeeVoucherHistoryList(feesListToBePaid.First(), fees_list.First());

                                    if (feesDAL.submitFeesVoucher(feesListToBePaid, historyList, paidFeesList.First(), fillVoucherObject(), fillVoucherEntryList()) > 0)
                                    {                                       
                                        MessageBox.Show("SuccessFully Paid");
                                        date_picker.SelectedDate = window.date_picker.SelectedDate;
                                        loadGrid();                                       
                                    }
                                    else
                                    {
                                        MessageBox.Show("Voucher not saved");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Your Fees Total Amount has changed in Fees Generation Form");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Your Fees has changed in Fee Generation Form");
                            }
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Receipt# Not Find");
                    }
                }
            }
        }

        public bool checkFees() 
        {
            bool check = false;
            unPaidTotalAmount = 0;
            paidFeesList = new List<sms_fees>();

            foreach(sms_fees voucher in fees_list)
            {
                check = false;
                foreach(sms_fees fee in unPaidFeesList.Where(x=>x.id == voucher.fees_generated_id))
                {
                    check = true;
                    unPaidTotalAmount = unPaidTotalAmount + fee.rem_amount;

                    fee.receipt_no = voucher.receipt_no;
                    fee.receipt_no_full = voucher.receipt_no_full;
                    paidFeesList.Add(fee);
                }
                if(check==false)
                {
                    return false;
                }
            }
            return true;
        }

        List<sms_fees> getFeesListToBePaid()
        {            
            List<sms_fees> feesList = new List<sms_fees>();
            sms_fees_collection_place place = (sms_fees_collection_place)window.place_cmb.SelectedItem;

            int amount = Convert.ToInt32(window.paid_TB.Text);             
            foreach (sms_fees fee in paidFeesList)
            {
                if (amount > 0)
                { 
                    if (amount >= fee.rem_amount)
                    {
                        //pay whole fee    
                        var date = window.date_picker.SelectedDate.Value;
                        fee.date = date.AddSeconds(DateTime.Now.TimeOfDay.TotalSeconds);                        
                        fee.total_amount = Convert.ToInt32(window.total_TB.Text);
                        fee.total_paid = Convert.ToInt32(window.paid_TB.Text);
                        fee.amount_in_words = feesDAL.NumberToWords(Convert.ToInt32( window.paid_TB.Text));
                        fee.total_remaining = Convert.ToInt32(window.rem_TB.Text);
                        fee.amount_paid = fee.rem_amount;
                        fee.amount = fee.rem_amount;
                        fee.fees_collection_place_id = place.id;
                        fee.fees_collection_place = place.place;

                        fee.date_time = DateTime.Now;
                        fee.created_by = MainWindow.emp_login_obj.emp_user_name;
                        fee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);

                        feesList.Add(fee);
                        amount = amount - fee.amount_paid;
                    }
                    else
                    {
                        // pay partial fee      
                        if (fee.rem_amount != fee.amount)
                        {
                            fee.discount = 0;
                            fee.wave_off = 0;
                        }

                        var date = window.date_picker.SelectedDate.Value;
                        fee.date = date.AddSeconds(DateTime.Now.TimeOfDay.TotalSeconds);                        
                        fee.amount_paid = amount;
                        fee.amount = fee.rem_amount;
                        fee.total_amount = Convert.ToInt32(window.total_TB.Text);
                        fee.total_paid = Convert.ToInt32(window.paid_TB.Text);
                        fee.amount_in_words = feesDAL.NumberToWords(Convert.ToInt32(window.paid_TB.Text));
                        fee.total_remaining = Convert.ToInt32(window.rem_TB.Text);
                        fee.fees_collection_place_id = place.id;
                        fee.fees_collection_place = place.place;

                        fee.date_time = DateTime.Now;
                        fee.created_by = MainWindow.emp_login_obj.emp_user_name;
                        fee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);

                        feesList.Add(fee);
                        amount = 0;
                        break;
                    }
                }
            }            
            return feesList;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {            
            if(date_picker.SelectedDate != null)
            {
                loadGrid();
            }
        }

        void loadGrid() 
        {
            DateTime date = (DateTime)date_picker.SelectedDate;
            List<sms_fees> vouchers_list = feesDAL.getAllFeesVoucherByDate(date);
            fee_voucher_grid.ItemsSource = vouchers_list;

            count_TB.Text = vouchers_list.Select(x => x.receipt_no).Count().ToString();
            paid_TB.Text = vouchers_list.Select(x => x.total_paid).Sum().ToString();
        }

        public List<sms_fees> getFeeVoucherHistoryList(sms_fees obj, sms_fees adm)
        {
            List<sms_fees> historyList = new List<sms_fees>();
            calculate_discount_waveoff();

            foreach (var item in paidFeesList)
            {
                item.std_name = adm.std_name;
                item.father_name = adm.father_name;
                item.adm_no = adm.adm_no;
                item.class_name = adm.class_name;
                item.section_name = adm.section_name;

                item.receipt_no = obj.receipt_no;
                item.receipt_no_full = obj.receipt_no_full;
                item.date = obj.date;
                item.total_amount = obj.total_amount;
                item.total_paid = obj.total_paid;
                item.total_remaining = obj.total_remaining;
                item.amount_in_words = obj.amount_in_words;
                item.discount = total_discount;
                item.wave_off = total_waveOff;
                item.fees_collection_place = obj.fees_collection_place;
                item.emp_id = obj.emp_id;

                historyList.Add(item);
            }

            return historyList;
        }

        public void calculate_discount_waveoff()
        {
            total_discount = 0;
            total_waveOff = 0;

            foreach (sms_fees fee in feesListToBePaid)
            {
                if (fee.amount == fee.rem_amount)
                {
                    total_discount = total_discount + fee.discount;
                    total_waveOff = total_waveOff + fee.wave_off;
                }
            }    
        }

        public sms_voucher fillVoucherObject()
        {
            sms_voucher sms_voucher_obj = new sms_voucher();
            
            try
            {
                sms_voucher_obj.voucher_type = "CRV";
                sms_voucher_obj.voucher_type_id = 4;
                sms_voucher_obj.is_posted = "Y";

                sms_voucher_obj.voucher_date = feesListToBePaid.First().date;
                sms_voucher_obj.voucher_description = fees_list.First().std_name + "[" + fees_list.First().adm_no + "]";
                sms_voucher_obj.voucher_no_int = feesListToBePaid.First().receipt_no;
                sms_voucher_obj.voucher_no = feesListToBePaid.First().receipt_no_full;
                sms_voucher_obj.amount = feesListToBePaid.First().total_paid;

                sms_voucher_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                sms_voucher_obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                sms_voucher_obj.date_time = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return sms_voucher_obj;
        }

        public List<sms_voucher_entries> fillVoucherEntryList()
        {
            List<sms_voucher_entries> list = new List<sms_voucher_entries>();
            sms_voucher_entries sms_voucher_entries_obj;

            foreach (var item in feesListToBePaid)
            {
                //debit
                sms_voucher_entries_obj = new sms_voucher_entries();
                sms_voucher_entries_obj.account_head_id = 49;
                sms_voucher_entries_obj.account_head = "Fee";

                sms_voucher_entries_obj.account_detail_id = item.fees_category_id;
                sms_voucher_entries_obj.account_detail = item.fees_category;

                sms_voucher_entries_obj.voucher_no = item.receipt_no_full;
                sms_voucher_entries_obj.voucher_no_int = item.receipt_no;

                sms_voucher_entries_obj.voucher_type = "CRV";
                sms_voucher_entries_obj.voucher_type_id = 4;

                sms_voucher_entries_obj.description = fees_list.First().std_name + "[" + fees_list.First().adm_no + "]- " + item.month_name + "-" + item.fees_category;
                sms_voucher_entries_obj.credit = item.amount_paid;
                sms_voucher_entries_obj.debit = 0;

                sms_voucher_entries_obj.created_by = item.created_by;
                sms_voucher_entries_obj.emp_id = item.emp_id;
                sms_voucher_entries_obj.date_time = item.date_time;

                list.Add(sms_voucher_entries_obj);

                //credit
                sms_voucher_entries_obj = new sms_voucher_entries();
                sms_voucher_entries_obj.account_head_id = 15;
                sms_voucher_entries_obj.account_head = "Current Assets";

                sms_voucher_entries_obj.account_detail_id = item.fees_collection_place_id;
                sms_voucher_entries_obj.account_detail = item.fees_collection_place;

                sms_voucher_entries_obj.voucher_no = item.receipt_no_full;
                sms_voucher_entries_obj.voucher_no_int = item.receipt_no;

                sms_voucher_entries_obj.voucher_type = "CRV";
                sms_voucher_entries_obj.voucher_type_id = 4;

                sms_voucher_entries_obj.description = fees_list.First().std_name + "[" + fees_list.First().adm_no + "]- " + item.month_name + "-" + item.fees_category;
                sms_voucher_entries_obj.credit = 0;
                sms_voucher_entries_obj.debit = item.amount_paid;

                sms_voucher_entries_obj.created_by = item.created_by;
                sms_voucher_entries_obj.emp_id = item.emp_id;
                sms_voucher_entries_obj.date_time = item.date_time;

                list.Add(sms_voucher_entries_obj);
            }

            return list;
        }
    }
}
