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
using System.ComponentModel;

namespace SMS.AdvancedAccountManagement.VoucherEntry
{
    /// <summary>
    /// Interaction logic for VoucherEntryPage.xaml
    /// </summary>
    public partial class VoucherEntryPage : Page
    {
        AccountsDAL accountsDAL;
        List<sms_voucher> vouchersList;

        VoucherEntryForm vef;
        sms_voucher row_obj;
        string mode;
        double total_amount = 0;

        List<chart_of_accounts> chartOfAccountList;
        List<sms_voucher_types> voucherTypeList;

        public VoucherEntryPage()
        {
            InitializeComponent();
            accountsDAL = new AccountsDAL();
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                date_picker_to.SelectedDate = DateTime.Now;
                date_picker_from.SelectedDate = DateTime.Now;

                voucherTypeList = new List<sms_voucher_types>();
                voucherTypeList = accountsDAL.getAllVoucherTypes();
                voucherTypeList.Insert(0, new sms_voucher_types() { id = -1, voucher_type = "--All Vouchers--" });
                voucher_types_CMB.ItemsSource = voucherTypeList;
                voucher_types_CMB.SelectedIndex = 0;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            vef = new VoucherEntryForm(mode, this, row_obj);
            vef.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            vef.ShowDialog();            
        }

        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            row_obj = (sms_voucher)vouchers_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                vef = new VoucherEntryForm(mode, this, row_obj);
                vef.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                vef.ShowDialog();
            }
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (sms_voucher)vouchers_grid.SelectedItem;
            if (row_obj != null)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete","Confirmation",MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (accountsDAL.deleteVoucher(row_obj.id) > 0)
                        {
                            MessageBox.Show("Successfully Deleted");
                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else 
            {
                MessageBox.Show("plz select a row");
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime sDate = date_picker_from.SelectedDate.Value;
                DateTime eDate = date_picker_to.SelectedDate.Value;
                sms_account_type type;
                List<sms_voucher_entries> lst;

                if (voucher_types_CMB.SelectedItem != null && voucher_types_CMB.SelectedIndex > 0)
                {
                    type = (sms_account_type)voucher_types_CMB.SelectedItem;
                    lst = accountsDAL.getAllVoucherEntriesByVoucherDateAndType(sDate, eDate).Where(x => x.account_type_id == type.id).ToList();
                }
                else
                {
                    lst = accountsDAL.getAllVoucherEntriesByVoucherDateAndType(sDate, eDate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }       
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void vouchers_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();

        }

        private void date_picker_from_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {                    
                    vouchersList = accountsDAL.getAllVoucherByDate(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    vouchers_grid.ItemsSource = vouchersList;
                    calculate_amount();
                }
            }
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;

                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    vouchersList = accountsDAL.getAllVoucherByDate(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    vouchers_grid.ItemsSource = vouchersList;
                    calculate_amount();                    
                }
            }
        }

        public void calculate_amount()
        {
            sms_voucher voucher;
            total_amount = 0;            

            for (int i = 0; i < vouchers_grid.Items.Count; i++)
            {
                voucher = (sms_voucher)vouchers_grid.Items[i];
                total_amount = total_amount + voucher.amount;                
            }
            amount_TB.Text = total_amount.ToString();
            count_TB.Text = vouchers_grid.Items.Count.ToString();
        }

        //private void account_head_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (account_head_cmb.SelectedItem != null)
        //    {
        //        if (account_head_cmb.SelectedIndex != 0)
        //        {
        //            chart_of_accounts acc = (chart_of_accounts)account_head_cmb.SelectedItem;

        //            account_detail_cmb.ItemsSource = chartOfAccountList.Where(x =>
        //            {
        //                if (x.p_id == acc.id) { return true; }
        //                else if (x.id == -2) { return true; }
        //                else { return false; }
        //            });
        //            account_detail_cmb.SelectedIndex = 0;
        //            calculate_amount();
                    
        //            ICollectionView cv = CollectionViewSource.GetDefaultView(vouchers_grid.ItemsSource);
        //            cv.Filter = o =>
        //            {
        //                sms_voucher f = o as sms_voucher;
        //                return (f.v == emp.emp_user_name);
        //            };
        //            calculate_amount();
        //        }
        //        else
        //        {
        //            clear_all_filter();
        //        }
                
        //    }
        //}

        //private void account_detail_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (account_detail_cmb.SelectedItem != null)
        //    {
        //        chart_of_accounts acc = (chart_of_accounts)account_head_cmb.SelectedItem;

        //        account_detail_cmb.ItemsSource = chartOfAccountList.Where(x =>
        //        {
        //            if (x.p_id == acc.id) { return true; }
        //            else if (x.id == -2) { return true; }
        //            else { return false; }
        //        });
        //        calculate_amount();
        //    }
        //}

        private void voucher_types_CMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(voucher_types_CMB.SelectedItem != null)
            {
                sms_voucher_types types = voucher_types_CMB.SelectedItem as sms_voucher_types;
                ICollectionView cv = CollectionViewSource.GetDefaultView(vouchers_grid.ItemsSource);
                if (voucher_types_CMB.SelectedIndex != 0)
                {                    
                    cv.Filter = o =>
                    {
                        sms_voucher f = o as sms_voucher;
                        return (f.voucher_type_id == types.id);
                    };                    
                }
                else 
                {
                    if (cv.Filter != null)
                    {
                        cv.Filter = null;
                    }                    
                }
                calculate_amount();
            }
        }
    }
}
