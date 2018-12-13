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
using SMS.FeesManagement.FeesReports;
using SMS.DAL;
using SMS.FeesManagement.FeesRegister;
using SMS.FeesManagement.FeesCollectionByAmount;
using SMS.FeeManagement.FeeSearch;

namespace SMS.FeesManagement.FeesCollection
{
    /// <summary>
    /// Interaction logic for FeesCollectionForm.xaml
    /// </summary>
    public partial class FeesCollectionForm : Window
    {
        admission admObj;
        FeesDAL feeDal;
        AccountsDAL accountsDAL;
        List<sms_fees> unPaidFeesList;       
        List<sms_fees> feesHistoryList;
        List<sms_fees> feesListToBePaid;
        List<sms_fees> originalUnPaidFeesList;
        string fees_note;

        int total = 0;
        int paid = 0;
        int rem = 0;

        public FeesCollectionForm(admission adm)
        {
            InitializeComponent();
            feeDal = new FeesDAL();
            accountsDAL = new AccountsDAL();
            this.admObj = adm;
            try
            {
                place_cmb.ItemsSource = feeDal.getAllFeesCollectionPlace();
                fees_note = feeDal.getFeesNote();
                place_cmb.SelectedValue = 22;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                place_cmb.SelectedIndex = 0;
            }
            FillControl();
            loadGrid();
        }

        void loadGrid()
        {
            try
            {
                unPaidFeesList = feeDal.getAllUnPaidFeesByStdId(Convert.ToInt32(admObj.id)).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList();
                originalUnPaidFeesList = feeDal.getAllUnPaidFeesByStdId(Convert.ToInt32(admObj.id)).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList();
                FeesGrid.ItemsSource = unPaidFeesList;
                FeesGrid.Items.Refresh();
                calculateTotal();
                calculate_discount_waveoff();
                PayByAmountRB.IsChecked = true;

                //fees history
                feesHistoryList = feeDal.getFeesPaidByStdId(Convert.ToInt32(admObj.id));
                FeesHistoryGrid.ItemsSource = feesHistoryList;

                //paid_TB.Text = "0";
                paid_TB.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }       

        void FillControl() 
        {
            std_name_TB.Text = admObj.std_name;
            father_name_TB.Text = admObj.father_name;
            class_name_TB.Text = admObj.class_name;
            section_name_TB.Text = admObj.section_name;
            adm_no_TB.Text = admObj.adm_no;
            cell_TB.Text = admObj.cell_no;
            std_img.Source = MainWindow.ByteToImage(admObj.image);
            date_picker.SelectedDate = DateTime.Now;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sms_fees fees_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < FeesGrid.Items.Count; i++)
            {
                fees_obj = (sms_fees)FeesGrid.Items[i];
                fees_obj.isChecked = checkBox.IsChecked.Value;
            }
            FeesGrid.Items.Refresh();
            calculatePaid();
            calculate_discount_waveoff();
        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            FeesGrid.SelectedItem = e.Source;
            sms_fees fees_obj = new sms_fees();
            fees_obj = (sms_fees)FeesGrid.SelectedItem;
            foreach (sms_fees fee in unPaidFeesList)
            {
                if (fees_obj.id == fee.id)
                {
                    fee.isChecked = checkBox.IsChecked.Value;
                }
            }
            calculatePaid();
            calculate_discount_waveoff();
        }

        void calculateTotal()
        {
            total = 0;
            foreach (sms_fees fees in unPaidFeesList)
            {
                total = total + fees.rem_amount;
            }
            total_TB.Text = total.ToString();
        }

        void calculatePaid()
        {
            paid = 0;
            if (PayBySelectionRB.IsChecked == true)
            {
                foreach (sms_fees fees in unPaidFeesList.Where(x => x.isChecked == true))
                {
                    paid = paid + fees.rem_amount;
                }
            }
            else
            {
                foreach (sms_fees fees in unPaidFeesList)
                {
                    paid = paid + fees.rem_amount;
                }

            }
            paid_TB.Text = paid.ToString();
        }

        private void paid_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(paid_TB.Text))
            {
                rem_TB.Text = (total - Convert.ToInt32(paid_TB.Text)).ToString();
            }
        }

        private void PayBySelectionRB_Checked(object sender, RoutedEventArgs e)
        {
            FeesGrid.Columns[7].IsReadOnly = false;
            paid_TB.IsEnabled = false;
            FeesGrid.Columns[8].Visibility = Visibility.Visible;
            calculatePaid();
            calculate_discount_waveoff();
        }

        private void PayByAmountRB_Checked(object sender, RoutedEventArgs e)
        {
            FeesGrid.Columns[7].IsReadOnly = true;
            paid_TB.IsEnabled = true;

            foreach (sms_fees fee in unPaidFeesList)
            {
                fee.isChecked = false;
            }
            calculatePaid();
            calculate_discount_waveoff();

            FeesGrid.Columns[8].Visibility = Visibility.Collapsed;
        }

        List<sms_fees> getFeesListToBePaid()
        {
            int last_receipt_no = 0;
            List<sms_fees> feesList = new List<sms_fees>();
            sms_fees_collection_place place = (sms_fees_collection_place)place_cmb.SelectedItem;

            try
            {
                last_receipt_no = accountsDAL.getLastVoucherNo(4) + 1; // CRV


                int amount = Convert.ToInt32(paid_TB.Text);
                if (PayBySelectionRB.IsChecked == true)
                {
                    foreach (sms_fees fee in unPaidFeesList.Where(x => x.isChecked == true))
                    {
                        var date = date_picker.SelectedDate.Value;
                        fee.date = date.AddSeconds(DateTime.Now.TimeOfDay.TotalSeconds);
                        fee.date_time = DateTime.Now;
                        fee.receipt_no = last_receipt_no;
                        fee.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + last_receipt_no.ToString("D6");
                        fee.total_amount = Convert.ToInt32(total_TB.Text);
                        fee.total_paid = Convert.ToInt32(paid_TB.Text);
                        fee.amount_in_words = feeDal.NumberToWords(Convert.ToInt32(paid_TB.Text));
                        fee.total_remaining = Convert.ToInt32(rem_TB.Text);
                        int rem_amount = fee.rem_amount;
                        fee.amount_paid = rem_amount;
                        fee.rem_amount = originalUnPaidFeesList.Where(x => x.id == fee.id).First().rem_amount;
                        fee.amount = originalUnPaidFeesList.Where(x => x.id == fee.id).First().rem_amount;
                        fee.created_by = MainWindow.emp_login_obj.emp_user_name;
                        fee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                        fee.fees_collection_place_id = place.id;
                        fee.fees_collection_place = place.place;
                        feesList.Add(fee);
                    }
                    feesList = unPaidFeesList.Where(x => x.isChecked == true).ToList();
                }
                else
                {
                    foreach (sms_fees fee in unPaidFeesList)
                    {
                        if (amount > 0)
                        {
                            fee.receipt_no = last_receipt_no;
                            if (amount >= fee.rem_amount)
                            {
                                //pay whole fee    
                                var date = date_picker.SelectedDate.Value;
                                fee.date = date.AddSeconds(DateTime.Now.TimeOfDay.TotalSeconds);
                                fee.date_time = DateTime.Now;
                                fee.receipt_no = last_receipt_no;
                                fee.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + last_receipt_no.ToString("D6");
                                fee.total_amount = Convert.ToInt32(total_TB.Text);
                                fee.total_paid = Convert.ToInt32(paid_TB.Text);
                                fee.amount_in_words = feeDal.NumberToWords(Convert.ToInt32(paid_TB.Text));
                                fee.total_remaining = Convert.ToInt32(rem_TB.Text);
                                fee.amount_paid = fee.rem_amount;
                                fee.amount = fee.rem_amount;
                                fee.fees_collection_place_id = place.id;
                                fee.fees_collection_place = place.place;
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

                                var date = date_picker.SelectedDate.Value;
                                fee.date = date.AddSeconds(DateTime.Now.TimeOfDay.TotalSeconds);
                                fee.date_time = DateTime.Now;
                                fee.amount_paid = amount;
                                fee.amount = fee.rem_amount;
                                fee.receipt_no = last_receipt_no;
                                fee.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + last_receipt_no.ToString("D6");
                                fee.total_amount = Convert.ToInt32(total_TB.Text);
                                fee.total_paid = Convert.ToInt32(paid_TB.Text);
                                fee.amount_in_words = feeDal.NumberToWords(Convert.ToInt32(paid_TB.Text));
                                fee.total_remaining = Convert.ToInt32(rem_TB.Text);
                                fee.fees_collection_place_id = place.id;
                                fee.fees_collection_place = place.place;
                                fee.created_by = MainWindow.emp_login_obj.emp_user_name;
                                fee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                                feesList.Add(fee);
                                amount = 0;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return feesList;
        }

        private void pay_cash_btn_Click(object sender, RoutedEventArgs e)
        {
            submit();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                submit();
            }
        }

        private void submit()
        {
            if (PayBySelectionRB.IsChecked == true)
            {
                calculatePaid();
            }
            MessageBoxResult mbr = MessageBox.Show("Do You Want To Pay    " + paid_TB.Text + " Rs ?", "Payment Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                int count = 0;
                sms_fees fee;
                try
                {
                    if (Convert.ToInt32(paid_TB.Text) > 0)
                    {
                        feesListToBePaid = getFeesListToBePaid();
                        if (Convert.ToInt32(paid_TB.Text) <= Convert.ToInt32(total_TB.Text))
                        {
             
                            List<sms_fees> feesVoucherHistoryList = getFeeVoucherHistoryList(feesListToBePaid.First(), admObj);

                            if (feeDal.submitFees(feesListToBePaid, feesListToBePaid.First().receipt_no, feesVoucherHistoryList, fillVoucherObject(), fillVoucherEntryList()) > 0)
                            {
                                MessageBoxResult mbr1 = MessageBox.Show("Do You Want To Print Cashed Received Voucher ?", "Print Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (mbr1 == MessageBoxResult.Yes)
                                {
                                    List<admission> admList = new List<admission>();
                                    List<FeesReportData> feesReportList = new List<FeesReportData>();

                                    foreach (sms_fees fees in feesVoucherHistoryList)
                                    {
                                        //fees.std_name = admObj.std_name;
                                        //fees.father_name = admObj.father_name;
                                        //fees.adm_no = admObj.adm_no;                                        

                                        fees.institute_logo = MainWindow.ins.institute_logo;
                                        fees.institute_name = MainWindow.ins.institute_name;
                                        fees.fees_note = fees_note;

                                    }
                                    count = feesVoucherHistoryList.Count;
                                    for (int i = count; i < 6; i++)
                                    {
                                        fee = new sms_fees();
                                        fee.std_name = admObj.std_name;
                                        fee.father_name = admObj.father_name;
                                        fee.adm_no = admObj.adm_no;
                                        fee.date = DateTime.Now;
                                        fee.std_id = Convert.ToInt32(admObj.id);
                                        feesVoucherHistoryList.Add(fee);
                                    }
                                    FeesCollectionByAmountReportWindow window = new FeesCollectionByAmountReportWindow(feesVoucherHistoryList);
                                    window.ShowDialog();
                                }
                                loadGrid();
                                MessageBox.Show("Successfully Paid", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Paid Amount Should Be Less Or Equal To Total Amount", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Paid Amount Should Be Greater Than Zero", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public List<sms_fees> getFeeVoucherHistoryList(sms_fees obj, admission adm)
        {
            List<sms_fees> historyList = new List<sms_fees>();

            foreach (var item in unPaidFeesList)
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
                item.discount = Convert.ToInt32(disc_TB.Text);
                item.wave_off = Convert.ToInt32(waveOFF_TB.Text);
                item.fees_collection_place = obj.fees_collection_place;
                item.emp_id = obj.emp_id;

                historyList.Add(item);
            }

            return historyList;
        }

        private void feesHistory_btn_Click(object sender, RoutedEventArgs e)
        {
            List<sms_fees> fees_list;
            List<sms_fees> finalFeesHistoryList = new List<sms_fees>();
            List<sms_fees> feesHistoryList = feeDal.getFeesVoucherHistoryByStdId(Convert.ToInt32(admObj.id));
            int count = 0;
            sms_fees fee;

            foreach (var receiptNo in feesHistoryList.Select(x => x.receipt_no).Distinct())
            {
                count = 0;
                fees_list = new List<sms_fees>();
                fees_list = feesHistoryList.Where(x => x.receipt_no == receiptNo).ToList();
                foreach (sms_fees fees in fees_list)
                {
                    //fees.std_name = admObj.std_name;
                    //fees.father_name = admObj.father_name;
                    //fees.adm_no = admObj.adm_no;
                    fees.fees_note = fees_note;

                    fees.institute_logo = MainWindow.ins.institute_logo;
                    fees.institute_name = MainWindow.ins.institute_name;

                    finalFeesHistoryList.Add(fees);
                }
                count = fees_list.Count;
                for (int i = count; i < 6; i++)
                {
                    fee = new sms_fees();
                    fee.std_name = admObj.std_name;
                    fee.father_name = admObj.father_name;
                    fee.adm_no = admObj.adm_no;
                    fee.std_id = Convert.ToInt32(admObj.id);
                    fee.receipt_no = receiptNo;

                    finalFeesHistoryList.Add(fee);
                }
            }

            if (finalFeesHistoryList.Count > 0)
            {
                FeesVoucherHistoryReportWindow window = new FeesVoucherHistoryReportWindow(finalFeesHistoryList);
                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("No History available");
            }

        }

        public void calculate_discount_waveoff()
        {
            int total_discount = 0;
            int total_waveOff = 0;

            if (PayBySelectionRB.IsChecked == true)
            {
                foreach (sms_fees fee in unPaidFeesList.Where(x => x.isChecked == true))
                {
                    if (fee.amount == fee.rem_amount)
                    {
                        total_discount = total_discount + fee.discount;
                        total_waveOff = total_waveOff + fee.wave_off;
                    }
                }
            }
            else
            {
                foreach (sms_fees fee in unPaidFeesList)
                {
                    if (fee.amount == fee.rem_amount)
                    {
                        total_discount = total_discount + fee.discount;
                        total_waveOff = total_waveOff + fee.wave_off;
                    }
                }
            }

            disc_TB.Text = total_discount.ToString();
            waveOFF_TB.Text = total_waveOff.ToString();
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
                sms_voucher_obj.voucher_description = admObj.std_name + "[" + admObj.adm_no + "]";
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

                sms_voucher_entries_obj.description = admObj.std_name + "[" + admObj.adm_no + "]- " + item.month_name + "-" + item.fees_category;
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

                sms_voucher_entries_obj.description = admObj.std_name + "[" + admObj.adm_no + "]- " + item.month_name + "-" + item.fees_category;
                sms_voucher_entries_obj.credit = 0;
                sms_voucher_entries_obj.debit = item.amount_paid;

                sms_voucher_entries_obj.created_by = item.created_by;
                sms_voucher_entries_obj.emp_id = item.emp_id;
                sms_voucher_entries_obj.date_time = item.date_time;

                list.Add(sms_voucher_entries_obj);
            }

            return list;
        }

        private void cance_challan_btn_Click(object sender, RoutedEventArgs e)
        {           
            ReceiptNoWindow window = new ReceiptNoWindow();
            window.ShowDialog();
            if(!string.IsNullOrEmpty(window.receipt_textbox.Text) || !string.IsNullOrWhiteSpace(window.receipt_textbox.Text))
            {
                if (feesHistoryList.Select(x => x.receipt_no_full).Contains(window.receipt_textbox.Text))
                {
                    try
                    {
                        if (feeDal.cancelFees(feesHistoryList.Where(x => x.receipt_no_full == window.receipt_textbox.Text).ToList()) > 0)
                        {                            
                            MessageBox.Show("Successfully Canceled", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            loadGrid();
                            calculatePaid();
                        }
                        else 
                        {
                            MessageBox.Show("There is some error to cancel fees", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else 
                {
                    MessageBox.Show("Receipt# Not Available", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }            
        }

        private void student_fee_report_btn_Click(object sender, RoutedEventArgs e)
        {
            StudentFeeIndividualReport window = new StudentFeeIndividualReport(admObj);
            window.Show();
        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
