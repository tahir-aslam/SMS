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
using System.Collections.ObjectModel;
using Microsoft.Reporting.WinForms;
using System.ComponentModel;
using SMS.AdmissionManagement.Admission;

namespace SMS.FeesManagement.FeesVoucher
{
    /// <summary>
    /// Interaction logic for FeesVoucherPage.xaml
    /// </summary>
    public partial class FeesVoucherPage : Page
    {
        FeesDAL feesDAL;
        ClassesDAL classDAL;
        MiscDAL miscDAL;
        AdmissionDAL admDAL;
        AccountsDAL accountsDAL;

        List<admission> adm_list;
        List<admission> checked_adm_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_fees> fees_list;
        List<sms_fees> vouchers_list;
        List<sms_months> months_list;
        List<sms_fees> voucher_history_list;

        // history
        admission obj;
        List<fee> fee_list;
        List<admission> adm_grid_list;

        sections sec;
        classes cl;

        public FeesVoucherPage()
        {
            InitializeComponent();

            feesDAL = new FeesDAL();
            classDAL = new ClassesDAL();
            miscDAL = new MiscDAL();
            admDAL = new AdmissionDAL();
            accountsDAL = new AccountsDAL();

            adm_list = new List<admission>();

            try
            {
                adm_list = admDAL.get_all_admissions();
                classes_list = classDAL.get_all_classes();
                months_list = miscDAL.get_all_months();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            adm_datagrid.ItemsSource = adm_list;
            classes_list.Insert(0, new classes() { id = "-1", class_name = "--All Class--" });
            class_cmb.ItemsSource = classes_list;
            class_cmb_history.ItemsSource = classes_list;

            voucher_type_cmb.SelectedIndex = 0;
            voucher_type_history_cmb.SelectedIndex = 0;
            default_RB.IsChecked = true;
        }

        private void voucher_type_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (voucher_type_cmb.SelectedItem != null)
            {
                if (voucher_type_cmb.SelectedIndex != 0)
                {
                    reset();
                    class_cmb.IsEnabled = true;
                    class_cmb.SelectedIndex = 0;
                    SearchTextBox1.Focus();
                }
                else
                {
                    class_cmb.IsEnabled = false;
                    class_cmb.SelectedIndex = 0;

                    adm_grid.Visibility = Visibility.Hidden;
                    months_grid.Visibility = Visibility.Hidden;
                    voucher1_grid.Visibility = Visibility.Hidden;
                    voucher2_grid.Visibility = Visibility.Hidden;
                    voucher3_grid.Visibility = Visibility.Hidden;
                    voucherFine_grid.Visibility = Visibility.Hidden;
                    voucher_grid.Visibility = Visibility.Visible;
                }
            }
        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb.SelectedItem != null)
            {
                cl = (classes)class_cmb.SelectedItem;
                if (class_cmb.SelectedIndex != 0)
                {
                    sections_list = new List<sections>();
                    sections_list = classDAL.get_all_sections(cl.id);
                    sections_list.Insert(0, new sections() { id = "-1", section_name = "--All Sections--" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;
                    section_cmb.IsEnabled = true;

                    reset();
                    adm_datagrid.ItemsSource = adm_list.Where(x => x.class_id == cl.id);
                }
                else
                {
                    reset();
                    adm_datagrid.ItemsSource = adm_list;
                }
            }
        }
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedItem != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    adm_datagrid.Visibility = Visibility.Visible;
                    sec = (sections)section_cmb.SelectedItem;
                    adm_datagrid.ItemsSource = adm_list.Where(x => x.section_id == sec.id);
                    reset();
                }
                else
                {
                    voucher1_grid.Visibility = Visibility.Hidden;
                    voucher2_grid.Visibility = Visibility.Hidden;
                    voucher3_grid.Visibility = Visibility.Hidden;
                    voucher_grid.Visibility = Visibility.Hidden;
                    voucherFine_grid.Visibility = Visibility.Hidden;

                    adm_datagrid.ItemsSource = adm_list.Where(x => x.class_id == cl.id);
                }
            }
        }



        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in adm_list)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            adm_datagrid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            adm_datagrid.SelectedItem = e.Source;
            admission adm = new admission();
            adm = (admission)adm_datagrid.SelectedItem;
            foreach (admission s in adm_list)
            {
                if (adm.id == s.id)
                {
                    s.Checked = checkBox.IsChecked.Value;
                }
            }
        }

        private void CheckBox_Checked_months(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in months_list)
            {
                item.isChecked = checkBox.IsChecked.Value;
            }
            months_datagrid.Items.Refresh();

        }
        private void CheckBox_Checked_sub_months(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            months_datagrid.SelectedItem = e.Source;
            sms_months month = new sms_months();
            month = (sms_months)months_datagrid.SelectedItem;
            foreach (sms_months s in months_list)
            {
                if (month.id == s.id)
                {
                    s.isChecked = checkBox.IsChecked.Value;
                }
            }
        }

        public void get_all_checked_admission()
        {
            checked_adm_list = new List<admission>();
            admission adm;
            for (int i = 0; i < adm_datagrid.Items.Count; i++)
            {
                adm = adm_datagrid.Items[i] as admission;
                if (adm.Checked == true)
                {
                    checked_adm_list.Add(adm);
                }
            }
        }

        private void show_months_btn_Click(object sender, RoutedEventArgs e)
        {
            default_RB.IsChecked = true;
            get_all_checked_admission();
            if (checked_adm_list.Count > 0)
            {
                adm_grid.Visibility = Visibility.Collapsed;
                months_grid.Visibility = Visibility.Visible;
                show_vouchers_btn.Visibility = Visibility.Visible;

                months_datagrid.ItemsSource = months_list;
            }
            else
            {
                MessageBox.Show("Please select minimum one student");
            }
        }

        private void show_vouchers_btn_Click(object sender, RoutedEventArgs e)
        {
            List<admission> _siblingsList = new List<admission>();
            bool check = false;
            bool check_receipt = false;
            List<sms_months> checkedMonthsList = months_list.Where(x => x.isChecked == true).ToList();
            if (default_RB.IsChecked == true)
            {
                check = true;
            }
            else
            {
                if (checkedMonthsList.Count > 0)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            if (check)
            {
                string amount_in_words = "";
                string fees_note = "";
                sms_fees bank_obj;

                if (checked_adm_list.Count > 0)
                {

                    List<sms_fees> saveVoucherList = new List<sms_fees>();
                    vouchers_list = new List<sms_fees>();

                    int count = 0;
                    int total_amount = 0;
                    sms_fees fee;

                    int last_receipt_no = 0;
                    try
                    {
                        fees_note = feesDAL.getFeesNote();
                        bank_obj = feesDAL.get_bank_details();
                        last_receipt_no = accountsDAL.getLastVoucherNo(4);

                        if (voucher_type_cmb.SelectedIndex == 5)
                        {
                            checked_adm_list.Select(x=>x.father_cnic).Distinct();
                        }
                        foreach (admission adm in checked_adm_list)
                        {                           

                            total_amount = 0;
                            fees_list = new List<sms_fees>();
                            if (default_RB.IsChecked == true)
                            {
                                if (voucher_type_cmb.SelectedIndex==5)
                                {                                    
                                    _siblingsList = admDAL.get_all_siblings(adm);
                                    fees_list = feesDAL.getAllUnPaidFeesByStdId(_siblingsList).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList();
                                    if (fees_list.Count > 0)
                                    {
                                        last_receipt_no = last_receipt_no + 1;
                                    }

                                    foreach (var item in fees_list)
                                    {
                                        item.adm_list = _siblingsList;
                                    }
                                }
                                else
                                {
                                    fees_list = feesDAL.getAllUnPaidFeesByStdId(Convert.ToInt32(adm.id)).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList();
                                    if (fees_list.Count > 0)
                                    {
                                        last_receipt_no = last_receipt_no + 1;
                                    }
                                    foreach (var item in fees_list)
                                    {
                                        item.class_id = Convert.ToInt32(adm.class_id);
                                        item.class_name = adm.class_name;
                                        item.section_id = Convert.ToInt32(adm.section_id);
                                        item.section_name = adm.section_name;
                                    }
                                }
                            }
                            else
                            {
                                check_receipt = false;
                                foreach (var item in feesDAL.getAllUnPaidFeesByStdId(Convert.ToInt32(adm.id)).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList())
                                {
                                    foreach (var month in checkedMonthsList)
                                    {
                                        if (item.month.ToString() == month.month_id)
                                        {
                                            item.class_id = Convert.ToInt32(adm.class_id);
                                            item.class_name = adm.class_name;
                                            item.section_id = Convert.ToInt32(adm.section_id);
                                            item.section_name = adm.section_name;
                                            fees_list.Add(item);
                                        }
                                    }

                                    if (check_receipt == false)
                                    {
                                        last_receipt_no = last_receipt_no + 1;
                                        check_receipt = true;
                                    }

                                }
                                //var list = from fees in feesDAL.getAllUnPaidFeesByStdId(Convert.ToInt32(adm.id))
                                //           where checkedMonthsList.Select(x => x.month_id).Contains(fees.month.ToString())
                                //           select fees;

                            }

                            if (fees_list != null)
                            {

                                total_amount = fees_list.Select(x => x.rem_amount).Sum();

                                amount_in_words = feesDAL.NumberToWords(total_amount);

                                foreach (sms_fees fees in fees_list)
                                {
                                    if (voucher_type_cmb.SelectedIndex == 5)
                                    {
                                        admission _adm = _siblingsList.Where(x => x.id == fees.std_id.ToString()).FirstOrDefault();
                                        fees.std_name = _adm.std_name;
                                        fees.father_name = _adm.father_name;
                                        fees.father_cnic = _adm.father_cnic;
                                        fees.adm_no = _adm.adm_no;
                                        fees.class_name = _adm.class_name;
                                    }
                                    else
                                    {
                                        fees.std_name = adm.std_name;
                                        fees.father_name = adm.father_name;
                                        fees.father_cnic = adm.father_cnic;
                                        fees.adm_no = adm.adm_no;
                                    }
                                    fees.fees_generated_id = fees.id;                                    
                                    fees.date = DateTime.Now;
                                    fees.total_amount = total_amount;
                                    fees.total_paid = total_amount;
                                    fees.total_remaining = 0;
                                    fees.receipt_no = last_receipt_no;
                                    fees.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + last_receipt_no.ToString("D6");
                                    fees.created_by = MainWindow.emp_login_obj.emp_user_name;
                                    fees.date_time = DateTime.Now;
                                    fees.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                                    fees.fees_note = fees_note;
                                    fees.amount_in_words = amount_in_words;
                                    fees.bank_name = bank_obj.bank_name;
                                    fees.branch_address = bank_obj.branch_address;
                                    fees.bank_logo = bank_obj.bank_logo;
                                    fees.account_no = bank_obj.account_no;
                                    fees.account_title = bank_obj.account_title;
                                    fees.due_date = fees_list.OrderByDescending(x => x.month).OrderByDescending(x => x.year).First().due_date;
                                    // fees.month_name = fees_list.OrderBy(x => x.month).OrderBy(x => x.year).First().month_name;


                                    vouchers_list.Add(fees);
                                    saveVoucherList.Add(fees);
                                }
                            }
                            if (fees_list != null) // add extra rows
                            {
                                if (voucher_type_cmb.SelectedIndex == 5)
                                {
                                    count = fees_list.Where(x => x.father_cnic == adm.father_cnic).Count();
                                }
                                else
                                {
                                    count = fees_list.Where(x => x.std_id == Convert.ToInt32(adm.id)).Count();
                                }

                                int limit = 6;
                                if (voucher_type_cmb.SelectedIndex==5)
                                {
                                    limit = 10;
                                }
                                for (int i = count; i < limit; i++)
                                {
                                    fee = new sms_fees();
                                    fee.std_name = "";
                                    fee.father_name = "";
                                    fee.adm_no = "";
                                    fee.date = DateTime.Now;
                                    fee.std_id = Convert.ToInt32(adm.id);
                                    fee.father_cnic = adm.father_cnic;
                                    fee.cell_no = adm.cell_no;
                                    fee.fees_note = fees_note;
                                    vouchers_list.Add(fee);
                                }

                                if (count > 6)
                                {
                                    //for (int i = 6; i < count; i++)
                                    //{
                                    //    vouchers_list.Where(x => x.std_id == Convert.ToInt32(adm.id)).ToList().RemoveAt(i);
                                    //}
                                    //vouchers_list.Where(x => x.std_id == Convert.ToInt32(adm.id)).ToList().RemoveRange(6, count - 6);
                                    //List<sms_fees> lst = vouchers_list.Where(x => x.std_id == Convert.ToInt32(adm.id)).ToList();
                                    //lst.RemoveRange(6, count - 6);
                                }
                            }
                        }


                        if (saveVoucherList.Count > 0)
                        {
                            if (voucher_type_cmb.SelectedIndex == 5)
                            {
                                voucherFamily_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                ReportDataSource ins = new ReportDataSource();
                                List<institute> ins_list = new List<institute>();
                                MainWindow.ins.date = DateTime.Now;
                                MainWindow.ins.page_no = 1;
                                ins_list.Add(MainWindow.ins);
                                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                ins.Value = ins_list;

                                this._reportViewerFamily.LocalReport.DataSources.Clear();
                                this._reportViewerFamily.LocalReport.DataSources.Add(fees);
                                this._reportViewerFamily.LocalReport.DataSources.Add(ins);
                                this._reportViewerFamily.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReportFamily.rdlc";

                                _reportViewerFamily.RefreshReport();
                            }

                            if (voucher_type_cmb.SelectedIndex == 4)
                            {
                                voucherFine_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                ReportDataSource ins = new ReportDataSource();
                                List<institute> ins_list = new List<institute>();
                                MainWindow.ins.date = DateTime.Now;
                                MainWindow.ins.page_no = 1;
                                ins_list.Add(MainWindow.ins);
                                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                ins.Value = ins_list;

                                this._reportViewerFine.LocalReport.DataSources.Clear();
                                this._reportViewerFine.LocalReport.DataSources.Add(fees);
                                this._reportViewerFine.LocalReport.DataSources.Add(ins);
                                this._reportViewerFine.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReport3Fine.rdlc";

                                _reportViewerFine.RefreshReport();
                            }

                            if (voucher_type_cmb.SelectedIndex == 3)
                            {
                                voucher3_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                ReportDataSource ins = new ReportDataSource();
                                List<institute> ins_list = new List<institute>();
                                MainWindow.ins.date = DateTime.Now;
                                MainWindow.ins.page_no = 1;
                                ins_list.Add(MainWindow.ins);
                                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                ins.Value = ins_list;

                                this._reportViewer3.LocalReport.DataSources.Clear();
                                this._reportViewer3.LocalReport.DataSources.Add(fees);
                                this._reportViewer3.LocalReport.DataSources.Add(ins);
                                this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReport3.rdlc";

                                _reportViewer3.RefreshReport();
                            }
                            else if (voucher_type_cmb.SelectedIndex == 2)
                            {
                                voucher2_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                ReportDataSource ins = new ReportDataSource();
                                List<institute> ins_list = new List<institute>();
                                MainWindow.ins.date = DateTime.Now;
                                MainWindow.ins.page_no = 1;
                                ins_list.Add(MainWindow.ins);
                                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                ins.Value = ins_list;

                                this._reportViewer2.LocalReport.DataSources.Clear();
                                this._reportViewer2.LocalReport.DataSources.Add(fees);
                                this._reportViewer2.LocalReport.DataSources.Add(ins);
                                this._reportViewer2.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReport2.rdlc";

                                _reportViewer2.RefreshReport();
                            }

                            else if (voucher_type_cmb.SelectedIndex == 1)
                            {
                                voucher1_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                vouchers_list.ToList().ForEach(x => x.institute_name = MainWindow.ins.institute_name);
                                vouchers_list.ToList().ForEach(x => x.institute_logo = MainWindow.ins.institute_logo);
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                //ReportDataSource ins = new ReportDataSource();
                                //List<institute> ins_list = new List<institute>();
                                //MainWindow.ins.date = DateTime.Now;
                                //MainWindow.ins.page_no = 1;
                                //ins_list.Add(MainWindow.ins);
                                //ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                //ins.Value = ins_list;

                                this._reportViewer1.LocalReport.DataSources.Clear();
                                this._reportViewer1.LocalReport.DataSources.Add(fees);
                                //this._reportViewer1.LocalReport.DataSources.Add(ins);
                                this._reportViewer1.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReport1.rdlc";

                                _reportViewer1.RefreshReport();
                            }

                            // ------------------------------------------------------------------------------------------------------------------------
                            #region Save Voucher in database


                            if (feesDAL.insertFeesVoucher(saveVoucherList, last_receipt_no) > 0)
                            {
                            }
                            else
                            {
                                MessageBox.Show("ERROR in  Saved Vouchers, Please Dont print these vouchers");
                                voucher_type_cmb.SelectedIndex = 0;
                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        voucher_type_cmb.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Please Check Minimum One Student");
                }
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Month");
            }

        }

        void reset()
        {
            adm_grid.Visibility = Visibility.Visible;

            months_grid.Visibility = Visibility.Hidden;
            voucher_grid.Visibility = Visibility.Collapsed;
            voucher1_grid.Visibility = Visibility.Hidden;
            voucher2_grid.Visibility = Visibility.Hidden;
            voucher3_grid.Visibility = Visibility.Hidden;

        }

        private void default_RB_Checked(object sender, RoutedEventArgs e)
        {
            if (default_RB.IsChecked == true)
            {
                months_datagrid.Visibility = Visibility.Collapsed;
            }
        }
        private void selected_RB_Checked(object sender, RoutedEventArgs e)
        {
            if (selected_RB.IsChecked == true)
            {
                months_datagrid.Visibility = Visibility.Visible;
            }
        }


        //--------------------------------------- Voucher History ---------------------------------------------------------------------------------

        private void voucher_type_history_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (voucher_type_history_cmb.SelectedItem != null)
            {
                if (voucher_type_history_cmb.SelectedIndex != 0)
                {
                    class_cmb_history.IsEnabled = true;
                    class_cmb_history.SelectedIndex = 0;
                    SearchTextBox.Focus();
                }
                else
                {
                    class_cmb_history.IsEnabled = false;
                    class_cmb_history.SelectedIndex = 0;

                    voucher11_grid.Visibility = Visibility.Hidden;
                    voucher22_grid.Visibility = Visibility.Hidden;
                    voucher33_grid.Visibility = Visibility.Hidden;
                    voucherFine1_grid.Visibility = Visibility.Hidden;
                    voucher_history_grid.Visibility = Visibility.Visible;
                }
            }
        }
        private void class_cmb_SelectionChanged_history(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb_history.SelectedItem != null)
            {
                cl = (classes)class_cmb_history.SelectedItem;
                if (class_cmb_history.SelectedIndex != 0)
                {
                    sections_list = new List<sections>();
                    sections_list = classDAL.get_all_sections(cl.id);
                    sections_list.Insert(0, new sections() { id = "-1", section_name = "--All Sections--" });
                    section_cmb_history.ItemsSource = sections_list;
                    section_cmb_history.SelectedIndex = 0;
                    section_cmb_history.IsEnabled = true;

                    voucher_history_grid.ItemsSource = voucher_history_list.Where(x => x.class_id.ToString() == cl.id);
                }
                else
                {
                    voucher_history_grid.ItemsSource = voucher_history_list;
                }
            }
        }
        private void section_cmb_SelectionChanged_history(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb_history.SelectedItem != null)
            {
                if (section_cmb_history.SelectedIndex != 0)
                {
                    sec = (sections)section_cmb_history.SelectedItem;
                    voucher_history_grid.ItemsSource = voucher_history_list.Where(x => x.section_id.ToString() == sec.id);
                    voucher_history_grid.Visibility = Visibility.Visible;
                }
                else
                {
                    voucher11_grid.Visibility = Visibility.Hidden;
                    voucher22_grid.Visibility = Visibility.Hidden;
                    voucher33_grid.Visibility = Visibility.Hidden;
                    voucherFine1_grid.Visibility = Visibility.Hidden;
                    voucher_history_grid.Visibility = Visibility.Visible;

                    voucher_history_grid.ItemsSource = voucher_history_list.Where(x => x.class_id.ToString() == cl.id);
                }
            }
        }


        private void click_refresh(object sender, RoutedEventArgs e)
        {
        }

        // ================        Printing         ========================
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }
        public void search_box()
        {
            try
            {
                string v_search = SearchTextBox.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(voucher_history_grid.ItemsSource);
                if (v_search == null)
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = o =>
                    {
                        sms_fees f = o as sms_fees;

                        if (search_cmb.SelectedIndex == 0)
                        {
                            return (f.std_name.ToUpper().StartsWith(v_search.ToUpper()) || f.std_name.ToUpper().Contains(v_search.ToUpper()));
                        }
                        else if (search_cmb.SelectedIndex == 1)
                        {
                            return (f.father_name.ToUpper().StartsWith(v_search.ToUpper()) || f.father_name.ToUpper().Contains(v_search.ToUpper()));
                        }
                        if (search_cmb.SelectedIndex == 2)
                        {
                            return (f.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || f.adm_no.ToUpper().Contains(v_search.ToUpper()));
                        }                        
                        if (search_cmb.SelectedIndex == 3)
                        {
                            return (f.roll_no.ToUpper().StartsWith(v_search.ToUpper()));
                        }
                        else if (search_cmb.SelectedIndex == 4)
                        {
                            return (f.receipt_no.ToString().Contains(v_search.ToUpper()));
                        }
                        else
                        {
                            return true;
                        }
                    };
                }
                SearchTextBox.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void search_box1()
        {
            try
            {
                
                    string v_search = SearchTextBox1.Text;
                    ICollectionView cv = CollectionViewSource.GetDefaultView(adm_datagrid.ItemsSource);
                    if (v_search == null)
                    {
                        cv.Filter = null;
                    }
                    else
                    {
                        cv.Filter = o =>
                        {
                            admission adm = o as admission;

                            if (search_cmb1.SelectedIndex == 0)
                            {
                                return (adm.std_name.ToUpper().StartsWith(v_search.ToUpper()) || adm.std_name.ToUpper().Contains(v_search.ToUpper()));
                            }
                            else if (search_cmb1.SelectedIndex == 1)
                            {
                                return (adm.father_name.ToUpper().StartsWith(v_search.ToUpper()) || adm.father_name.ToUpper().Contains(v_search.ToUpper()));
                            }
                            if (search_cmb1.SelectedIndex == 2)
                            {
                                return (adm.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || adm.adm_no.ToUpper().Contains(v_search.ToUpper()));
                            }
                            else if (search_cmb1.SelectedIndex == 3)
                            {
                                return (adm.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || adm.cell_no.ToUpper().Contains(v_search.ToUpper()));
                            }
                            else
                            {
                                return true;
                            }
                        };
                    }
                    SearchTextBox1.Focus();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //History

        private void CheckBox_Checked_history(object sender, RoutedEventArgs e)
        {
            try
            {
                var checkBox = sender as CheckBox;
                if (null == checkBox) return;

                foreach (sms_fees item in voucher_history_grid.Items)
                {
                    item.Checked = checkBox.IsChecked.Value;
                }
                voucher_history_grid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckBox_Checked_sub_history(object sender, RoutedEventArgs e)
        {
            try
            {

                var checkBox = sender as CheckBox;
                voucher_history_grid.SelectedItem = e.Source;
                //IEnumerable<sms_fees> feelist = new IEnumerable<sms_fees>();
                sms_fees fees = (sms_fees)voucher_history_grid.SelectedItem;
                foreach (sms_fees s in voucher_history_list.Where(x => x.receipt_no_full == fees.receipt_no_full))
                {

                    s.Checked = checkBox.IsChecked.Value;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void load_voucher_history()
        {
            //voucher_history_list = feesDAL.getAllUnPaidFeesVoucher();
            voucher_history_list = feesDAL.getAllUnPaidFeesVoucherWithAmount();
            voucher_history_grid.ItemsSource = voucher_history_list.GroupBy(x => x.receipt_no_full).Select(y => y.First()).ToList();
            //voucher_history_grid.ItemsSource = voucher_history_list.Select(x=>x.receipt_no_full).Distinct().ToList();
        }

        private void TabControlClasses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (TabControlClasses.SelectedItem != null)
                {
                    if (TabControlClasses.SelectedIndex == 0)
                    {

                    }
                    else
                    {
                        load_voucher_history();
                    }
                }
            }
        }

        private void print_voucher_history_Click(object sender, RoutedEventArgs e)
        {
            vouchers_list = new List<sms_fees>();

            if (voucher_type_history_cmb.SelectedIndex > 0)
            {
                string amount_in_words = "";
                string fees_note = "";
                sms_fees bank_obj;

                if (voucher_history_list.Where(x => x.Checked).Count() > 0)
                {
                    int count = 0;
                    int total_amount = 0;
                    sms_fees fee;

                    try
                    {
                        fees_note = feesDAL.getFeesNote();
                        bank_obj = feesDAL.get_bank_details();



                        foreach (var receipt_no_full in voucher_history_list.Where(x => x.Checked == true).Select(x => x.receipt_no_full).Distinct())
                        {
                            fees_list = new List<sms_fees>();
                            fees_list = voucher_history_list.Where(x => x.receipt_no_full == receipt_no_full).OrderBy(x => x.month).OrderBy(x => x.year).OrderByDescending(x => x.fees_category_id == 113).ToList();

                            total_amount = 0;
                            total_amount = fees_list.Select(x => x.amount).Sum();
                            amount_in_words = feesDAL.NumberToWords(total_amount);

                            if (total_amount == fees_list[0].total_amount)
                            {
                                foreach (sms_fees fees in fees_list)
                                {
                                    //fees.fees_generated_id = fees.id;
                                    //fees.std_name = adm.std_name;
                                    //fees.father_name = adm.father_name;
                                    //fees.adm_no = adm.adm_no;
                                    //fees.date = DateTime.Now;
                                    fees.total_amount = total_amount;
                                    fees.total_paid = total_amount;
                                    fees.total_remaining = 0;
                                    //fees.receipt_no = last_receipt_no;
                                    //fees.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + last_receipt_no.ToString("D6");
                                    fees.created_by = MainWindow.emp_login_obj.emp_user_name;
                                    fees.date_time = fees.date;
                                    fees.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                                    fees.fees_note = fees_note;
                                    fees.amount_in_words = amount_in_words;
                                    fees.bank_name = bank_obj.bank_name;
                                    fees.bank_logo = bank_obj.bank_logo;
                                    fees.branch_address = bank_obj.branch_address;
                                    fees.account_no = bank_obj.account_no;
                                    fees.account_title = bank_obj.account_title;
                                    fees.due_date = fees_list.OrderByDescending(x => x.month).OrderByDescending(x => x.year).First().due_date;
                                    // fees.month_name = fees_list.OrderBy(x => x.month).OrderBy(x => x.year).First().month_name;
                                    vouchers_list.Add(fees);
                                }
                                if (fees_list != null) // add extra rows
                                {
                                    count = fees_list.Count;
                                    for (int i = count; i < 6; i++)
                                    {
                                        fee = new sms_fees();
                                        fee.std_name = fees_list[0].std_name;
                                        fee.father_name = fees_list[0].father_name;
                                        fee.adm_no = fees_list[0].adm_no;
                                        fee.date = DateTime.Now;
                                        fee.std_id = Convert.ToInt32(fees_list[0].std_id);
                                        fee.fees_note = fees_note;
                                        vouchers_list.Add(fee);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Original fees has been changed or paid already. please compare with fees register. student name=" + fees_list[0].std_name + " Adm_no=" + fees_list[0].adm_no + Environment.NewLine + "Voucher total amount=" + fees_list[0].total_amount + Environment.NewLine + " Fees register total amount=" + total_amount);
                                continue;
                            }
                        }

                        //Print vouchers
                        if (vouchers_list.Count > 0)
                        {
                            if (voucher_type_history_cmb.SelectedIndex == 4)
                            {
                                voucherFine1_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                ReportDataSource ins = new ReportDataSource();
                                List<institute> ins_list = new List<institute>();
                                MainWindow.ins.date = DateTime.Now;
                                MainWindow.ins.page_no = 1;
                                MainWindow.ins.bank_logo = MainWindow.feesAdminPanel.bank_logo;
                                ins_list.Add(MainWindow.ins);
                                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                ins.Value = ins_list;

                                this._reportViewerFine1.LocalReport.DataSources.Clear();
                                this._reportViewerFine1.LocalReport.DataSources.Add(fees);
                                this._reportViewerFine1.LocalReport.DataSources.Add(ins);
                                this._reportViewerFine1.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReport3Fine.rdlc";

                                _reportViewerFine1.RefreshReport();
                            }

                            if (voucher_type_history_cmb.SelectedIndex == 3)
                            {
                                voucher33_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                ReportDataSource ins = new ReportDataSource();
                                List<institute> ins_list = new List<institute>();
                                MainWindow.ins.date = DateTime.Now;
                                MainWindow.ins.page_no = 1;
                                MainWindow.ins.bank_logo = MainWindow.feesAdminPanel.bank_logo;
                                ins_list.Add(MainWindow.ins);
                                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                ins.Value = ins_list;

                                this._reportViewer33.LocalReport.DataSources.Clear();
                                this._reportViewer33.LocalReport.DataSources.Add(fees);
                                this._reportViewer33.LocalReport.DataSources.Add(ins);
                                this._reportViewer33.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReport3.rdlc";

                                _reportViewer33.RefreshReport();
                            }
                            else if (voucher_type_history_cmb.SelectedIndex == 2)
                            {
                                voucher22_grid.Visibility = Visibility.Visible;

                                ReportDataSource fees = new ReportDataSource();
                                fees.Name = "fees";
                                fees.Value = vouchers_list;

                                ReportDataSource ins = new ReportDataSource();
                                List<institute> ins_list = new List<institute>();
                                MainWindow.ins.date = DateTime.Now;
                                MainWindow.ins.page_no = 1;
                                MainWindow.ins.bank_logo = MainWindow.feesAdminPanel.bank_logo;
                                ins_list.Add(MainWindow.ins);
                                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                                ins.Value = ins_list;

                                this._reportViewer22.LocalReport.DataSources.Clear();
                                this._reportViewer22.LocalReport.DataSources.Add(fees);
                                this._reportViewer22.LocalReport.DataSources.Add(ins);
                                this._reportViewer22.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesVoucher.FeesVoucherReport2.rdlc";

                                _reportViewer22.RefreshReport();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        voucher_type_history_cmb.SelectedIndex = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Please select Minimum One fee voucher");
                }
            }
            else
            {
                MessageBox.Show("Please Select Voucher type, e-g  1,2 or 3 fee slips");
            }
        }

        private void SearchTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box1();
        }

        private void search_cmb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox1.Focus();
        }
    }
}
