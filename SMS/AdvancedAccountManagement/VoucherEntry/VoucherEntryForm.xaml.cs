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
using SMS.DAL;

namespace SMS.AdvancedAccountManagement.VoucherEntry
{
    /// <summary>
    /// Interaction logic for VoucherEntryForm.xaml
    /// </summary>
    public partial class VoucherEntryForm : Window
    {

        List<sms_voucher_types> voucherTypeList;
        List<chart_of_accounts> chartOfAccountList;
        AccountsDAL accountsDAL;

        sms_voucher sms_voucher_obj;
        sms_voucher_entries sms_voucher_entries_obj;
        int voucher_id = 0;

        string mode = "";
        VoucherEntryPage VEP;

        double total_debit = 0;
        double total_credit = 0;

        List<sms_voucher_entries> voucher_entries_list;

        public VoucherEntryForm(string mode, VoucherEntryPage vep, sms_voucher obj)
        {
            InitializeComponent();
            this.VEP = vep;
            this.mode = mode;
            this.sms_voucher_obj = obj;

            accountsDAL = new AccountsDAL();
            LoadGrid();

            if(mode == "edit")
            {
                fill_control(sms_voucher_obj);

                if (sms_voucher_obj.is_posted == "Y")
                {
                    add_btn.IsEnabled = false;
                    post_btn.Visibility = Visibility.Collapsed;
                    posted_TB.Text = "POSTED";
                    posted_TB.Foreground = Brushes.Green;
                    voucher_entry_SP.Visibility = Visibility.Collapsed;
                    print_btn.Visibility = Visibility.Visible;
                    voucher_entries_grid.Columns[5].Visibility = Visibility.Collapsed;
                }                
            }            
        }

        void LoadGrid() 
        {
            try
            {
                voucherTypeList = new List<sms_voucher_types>();                
                voucherTypeList = accountsDAL.getAllVoucherTypes();
                voucherTypeList.Insert(0, new sms_voucher_types() { id= -1, voucher_type="--Select Vocher Type--" });
                voucher_types_CMB.ItemsSource = voucherTypeList;
                voucher_types_CMB.SelectedIndex = 0;

                voucher_Date_TB.SelectedDate = DateTime.Today;

                chartOfAccountList = new List<chart_of_accounts>();
                chartOfAccountList = accountsDAL.getAllChartOfAccounts();
                chartOfAccountList.Insert(0, new chart_of_accounts() { id=-1, account_name= "--Select Head Account--" });
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

        public void fill_control(sms_voucher obj) 
        {
            voucher_Date_TB.SelectedDate = obj.voucher_date;
            voucher_no_TB.Text = obj.voucher_no;
            voucher_types_CMB.SelectedValue = obj.voucher_type_id;
            voucher_Description_TB.Text = obj.voucher_description;

            loadVoucherEntryGrid(obj.id);
            calculate_Debit_Credit();

            voucher_Date_TB.IsEnabled = false;
            voucher_types_CMB.IsEnabled = false;
            voucher_Date_TB.Foreground = Brushes.Black;
            voucher_types_CMB.Foreground = Brushes.Black;
        }

        private void voucher_types_CMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        //Add Vocher entry
        private void add_btn_Click(object sender, RoutedEventArgs e)
        {
            addEntry();
        }

        public void loadVoucherEntryGrid(int voucher_id)
        {
            try
            {
                voucher_entries_list = accountsDAL.getAllVoucherEntriesByVoucherID(voucher_id);
                voucher_entries_grid.Items.Clear();
                foreach (var item in voucher_entries_list)
                {
                    voucher_entries_grid.Items.Add(item);
                }
                
                //voucher_entries_grid.ItemsSource = voucher_entries_list;
                voucher_entries_grid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void calculate_Debit_Credit() 
        {
            total_credit = 0;
            total_debit = 0;

            for (int i = 0; i < voucher_entries_grid.Items.Count; i++)
            {
                sms_voucher_entries obj = (sms_voucher_entries)voucher_entries_grid.Items[i];
                total_credit = total_credit + obj.credit;
                total_debit = total_debit + obj.debit;
            }

            total_credit_TB.Text = total_credit.ToString();
            total_debit_TB.Text = total_debit.ToString();
        }

        public void fillVoucherObject() 
        {
            try
            {
                sms_voucher_obj = new sms_voucher();
                sms_voucher_types type = (sms_voucher_types)voucher_types_CMB.SelectedItem;

                sms_voucher_obj.voucher_type = type.voucher_type;
                sms_voucher_obj.voucher_type_id = type.id;
                sms_voucher_obj.voucher_date = voucher_Date_TB.SelectedDate.Value;
                sms_voucher_obj.voucher_description = voucher_Description_TB.Text;
                sms_voucher_obj.voucher_no_int = accountsDAL.getLastVoucherNo(type.id) + 1;
                sms_voucher_obj.voucher_no = type.voucher_type + "-" + voucher_Date_TB.SelectedDate.Value.ToString("yy") + "-" + sms_voucher_obj.voucher_no_int.ToString("D6");

                sms_voucher_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                sms_voucher_obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                sms_voucher_obj.date_time = DateTime.Now;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void fillVoucherEntryObject() 
        {
            sms_voucher_entries_obj = new sms_voucher_entries();

            chart_of_accounts head = (chart_of_accounts)account_head_cmb.SelectedItem;
            sms_voucher_entries_obj.account_head_id = head.id;
            sms_voucher_entries_obj.account_head = head.account_name;

            chart_of_accounts detail = (chart_of_accounts)account_detail_cmb.SelectedItem;
            sms_voucher_entries_obj.account_detail_id = detail.id;
            sms_voucher_entries_obj.account_detail = detail.account_name;

            if (voucher_no_TB.Text == "")
            {
                sms_voucher_entries_obj.voucher_id = voucher_id;
            }
            else 
            {
                sms_voucher_entries_obj.voucher_id = sms_voucher_obj.id;
            }
            
            sms_voucher_entries_obj.voucher_no = sms_voucher_obj.voucher_no;
            sms_voucher_entries_obj.voucher_no_int = sms_voucher_obj.voucher_no_int ;

            sms_voucher_types type = (sms_voucher_types)voucher_types_CMB.SelectedItem;
            sms_voucher_entries_obj.voucher_type = type.voucher_type;
            sms_voucher_entries_obj.voucher_type_id = type.id;

            sms_voucher_entries_obj.description = voucher_entry_description_TB.Text;
            sms_voucher_entries_obj.credit = Convert.ToDouble(credit_TB.Text);
            sms_voucher_entries_obj.debit = Convert.ToDouble( debit_TB.Text);

            sms_voucher_entries_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            sms_voucher_entries_obj.emp_id = Convert.ToInt32 (MainWindow.emp_login_obj.emp_id);
            sms_voucher_entries_obj.date_time = DateTime.Now;

        }

        public bool validate()
        {
            if (voucher_types_CMB.SelectedIndex == 0)
            {
                voucher_types_CMB.Focus();
                string alertText = "Please Select Voucher Type";
                MessageBox.Show(alertText);
                return false;
            }
            else if (voucher_Description_TB.Text.Length == 0)
            {
                voucher_Description_TB.Focus();
                string alertText = "Please Enter Voucher Description";
                MessageBox.Show(alertText);
                return false;
            }
            if (account_head_cmb.SelectedIndex == 0)
            {
                account_head_cmb.Focus();
                string alertText = "Please Select Account Head";
                MessageBox.Show(alertText);
                return false;
            }
            if (account_detail_cmb.SelectedIndex == 0)
            {
                account_detail_cmb.Focus();
                string alertText = "Please Select Account Detail";
                MessageBox.Show(alertText);
                return false;
            }            
            else if (Convert.ToDouble(credit_TB.Text) <= 0 && Convert.ToDouble(debit_TB.Text) <= 0)
            {
                credit_TB.Focus();
                string alertText = "Please Enter Credit/Debit Amount";
                MessageBox.Show(alertText);
                return false;
            }
            else if (Convert.ToDouble(credit_TB.Text) <= 0 && voucher_entry_description_TB.Text.Length == 0)
            {
                voucher_entry_description_TB.Focus();
                string alertText = "Please Enter Voucher Entry Description";
                MessageBox.Show(alertText);
                return false;
            }
            else
            {
                return true;
            }
        }

        public void reserEntryFields()
        {
            account_head_cmb.SelectedIndex = 0;
            account_detail_cmb.SelectedIndex = 0;
            voucher_entry_description_TB.Text = "";
            debit_TB.Text = "0.00";
            credit_TB.Text = "0.00";

            voucher_Date_TB.IsEnabled = false;            
            voucher_types_CMB.IsEnabled = false;
            voucher_Date_TB.Foreground = Brushes.Black;
            voucher_types_CMB.Foreground = Brushes.Black;
        }

        private void post_btn_Click(object sender, RoutedEventArgs e)
        {
            if (total_credit > 0 && total_credit == total_debit)
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Post This Voucher ?", "POST Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Question);
                if (mbr == MessageBoxResult.Yes)
                {
                    if (accountsDAL.postVoucher(sms_voucher_obj) > 0)
                    {
                        add_btn.IsEnabled = false;
                        post_btn.Visibility = Visibility.Collapsed;
                        posted_TB.Text = "POSTED";
                        posted_TB.Foreground = Brushes.Green;
                        voucher_entry_SP.Visibility = Visibility.Collapsed;
                        print_btn.Visibility = Visibility.Visible;
                        voucher_entries_grid.Columns[5].Visibility = Visibility.Collapsed;
                        MessageBox.Show("Successfully Posted");
                    }
                    else
                    {
                        MessageBox.Show("There is error in posting voucher");
                    }
                }
            }
            else 
            {
               MessageBox.Show("Debit Credit Should Be Equal And Greater Than Zero");
            }
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sms_voucher_types type = voucher_types_CMB.SelectedItem as sms_voucher_types;
                chart_of_accounts acc = account_detail_cmb.SelectedItem as chart_of_accounts;
                FeesDAL feesDAL = new FeesDAL();
                string amonut_in_words = feesDAL.NumberToWords(Convert.ToInt32(total_debit_TB.Text));
                foreach (var item in voucher_entries_list)
                {
                    item.voucher_date = sms_voucher_obj.voucher_date;
                    item.voucher_description = sms_voucher_obj.voucher_description;
                    item.voucher_no = sms_voucher_obj.voucher_no;
                    item.voucher_type_description = type.description;
                    item.account_code = acc.account_full_code;
                    item.amount_in_words = amonut_in_words;
                }

                VoucherEntryReportWindow window = new VoucherEntryReportWindow(voucher_entries_list);
                window.Show();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                addEntry();
            }
        }

        void addEntry() 
        {
            if (validate())
            {
                // for submit voucher 
                if (voucher_no_TB.Text == "")
                {
                    voucher_id = 0;
                    fillVoucherObject();
                    voucher_id = accountsDAL.submitVoucher(sms_voucher_obj);
                    if (voucher_id > 0)
                    {
                        sms_voucher_obj.id = voucher_id;
                        voucher_no_TB.Text = sms_voucher_obj.voucher_no;

                        fillVoucherEntryObject();
                        List<sms_voucher_entries> lst = new List<sms_voucher_entries>();
                        lst.Add(sms_voucher_entries_obj);
                        if (accountsDAL.submitVoucherEntries(lst) > 0)
                        {
                            //voucher_entries_grid.Items.Insert(0, sms_voucher_entries_obj);
                            loadVoucherEntryGrid(sms_voucher_obj.id);
                            calculate_Debit_Credit();
                            reserEntryFields();

                            sms_voucher_obj.voucher_description = voucher_Description_TB.Text;
                            sms_voucher_obj.amount = total_debit;
                            accountsDAL.updateVoucher(sms_voucher_obj);

                            VEP.LoadData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("OOps! There is some problem in inserting record");
                    }
                }
                else
                {
                    fillVoucherEntryObject();
                    List<sms_voucher_entries> lst = new List<sms_voucher_entries>();
                    lst.Add(sms_voucher_entries_obj);
                    if (accountsDAL.submitVoucherEntries(lst) > 0)
                    {
                        //voucher_entries_grid.Items.Insert(0, sms_voucher_entries_obj);
                        loadVoucherEntryGrid(sms_voucher_obj.id);
                        calculate_Debit_Credit();
                        reserEntryFields();

                        sms_voucher_obj.voucher_description = voucher_Description_TB.Text;
                        sms_voucher_obj.amount = total_debit;

                        accountsDAL.updateVoucher(sms_voucher_obj);
                        VEP.LoadData();
                    }
                }
            }
        }
      
        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Voucher ?", "Delete Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (mbr == MessageBoxResult.Yes)
            {
                sms_voucher_entries obj = voucher_entries_grid.SelectedItem as sms_voucher_entries;
                if (accountsDAL.deleteVoucherEntry(obj.id) > 0)
                {
                    loadVoucherEntryGrid(sms_voucher_obj.id);
                    calculate_Debit_Credit();
                }
            }
        }
    }
}