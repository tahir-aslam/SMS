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
using SMS.DAL;
using SMS.Models;

namespace SMS.AdvancedAccountManagement.AccountsLedger
{
    /// <summary>
    /// Interaction logic for AccountsLedgerPage.xaml
    /// </summary>
    public partial class AccountsLedgerPage : Page
    {
        AccountsDAL accountsDAL;
        List<chart_of_accounts> chartOfAccountList;
        List<sms_voucher_entries> voucherEntryList = new List<sms_voucher_entries>();

        public AccountsLedgerPage()
        {
            InitializeComponent();
            accountsDAL = new AccountsDAL();
            LoadGrid();
        }

        void LoadGrid()
        {
            try
            {

                date_picker_to.SelectedDate = DateTime.Now;
                date_picker_from.SelectedDate = DateTime.Now;

                chartOfAccountList = new List<chart_of_accounts>();
                chartOfAccountList = accountsDAL.getAllChartOfAccounts();
                chartOfAccountList.Insert(0, new chart_of_accounts() { id = -1, account_name = "--Select Head Account--" });
                chartOfAccountList.Insert(0, new chart_of_accounts() { id = -2, account_name = "--Select Detailed Account--" });

                account_head_cmb.ItemsSource = chartOfAccountList.Where(x =>
                {
                    if (x.p_id != 0 && x.account_category_id == 1) { return true; }
                    else if (x.id == -1) { return true; }
                    else { return false; }
                });
                account_head_cmb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            AccountsLedgerReportWindow window = new AccountsLedgerReportWindow(vouchers_grid.Items.OfType<sms_voucher_entries>().ToList(), opening_TB.Text);
            window.ShowDialog();
        }

        private void account_head_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (account_head_cmb.SelectedItem != null)
            {
                chart_of_accounts acc = (chart_of_accounts)account_head_cmb.SelectedItem;

                account_detail_cmb.ItemsSource = chartOfAccountList.Where(x =>
                {
                    if (x.p_id == acc.id) { return true; }
                    else if (x.id == -2) { return true; }
                    else{return false;}});
                account_detail_cmb.SelectedIndex = 0;
            }                     
        }

        private void account_detail_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double balance = 0;
            if(account_detail_cmb.SelectedItem != null)
            {
                chart_of_accounts acc = (chart_of_accounts)account_detail_cmb.SelectedItem;
                if(account_detail_cmb.SelectedIndex != 0)               
                {
                    voucherEntryList = accountsDAL.getAllVoucherEntriesByAccountDetailID(acc.id);
                    setLedger();
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
                    setLedger();
                }
            }
        }

        private void date_picker_from_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    setLedger();
                }
            }
        }

        void setLedger()
        {
            if (account_detail_cmb.SelectedItem != null)
            {
                double balance = 0;
                DateTime dt = date_picker_to.SelectedDate.Value;
                DateTime dt1 = date_picker_from.SelectedDate.Value;
                if (voucherEntryList.Count > 0)
                {
                    vouchers_grid.ItemsSource = voucherEntryList.Where(x => x.voucher_date >= dt.Date).Where(x => x.voucher_date <= dt1);
                    try
                    {
                        opening_TB.Text = voucherEntryList.Where(x => x.voucher_date < dt.Date).Last().balance.ToString();

                    }
                    catch (Exception ex)
                    {
                        opening_TB.Text = "0";
                        //vouchers_grid.ItemsSource = null;
                        //vouchers_grid.Items.Refresh();
                    }
                }
                else 
                {
                    opening_TB.Text = "0";
                    vouchers_grid.ItemsSource = null;
                    vouchers_grid.Items.Refresh();
                }
            }
        }
    }
}
