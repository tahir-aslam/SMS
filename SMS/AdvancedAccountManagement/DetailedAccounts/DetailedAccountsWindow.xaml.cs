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

namespace SMS.AdvancedAccountManagement.DetailedAccounts
{
    /// <summary>
    /// Interaction logic for DetailedAccountsWindow.xaml
    /// </summary>
    public partial class DetailedAccountsWindow : Window
    {
        AccountsDAL accountsDAL;
        List<chart_of_accounts> accounts_list;
        List<chart_of_accounts> acc_types_list;
        List<chart_of_accounts> acc_category_list;
        chart_of_accounts obj;
        chart_of_accounts row_obj;
        string mode;
        DetailedAccountPage MAP;
        int code = 0;

        public DetailedAccountsWindow(string m, DetailedAccountPage map, chart_of_accounts ob)
        {
            InitializeComponent();
            accountsDAL = new AccountsDAL();

            mode = m;
            this.MAP = map;
            this.obj = ob;

            accounts_list = accountsDAL.getAllChartOfAccounts();

            acc_types_list = new List<chart_of_accounts>();
            acc_types_list = accounts_list.Where(x => x.p_id == 0).ToList();
            acc_types_list.Insert(0, new chart_of_accounts() { id = -1, account_name = "--Select Account Type--" });
            account_type_cmb.ItemsSource = acc_types_list;
            account_type_cmb.SelectedIndex = 0;

            if (mode == "edit")
            {
                fill_control();
            }
        }

        public void fill_control()
        {

            account_type_cmb.SelectedValue = accounts_list.Where(x=>x.id == obj.p_id).First().p_id;
            category_account_cmb.SelectedValue = obj.p_id;
            account_textbox.Text = obj.account_name;
            account_code_textbox.Text = obj.account_full_code;
        }

        public void fill_object()
        {
            row_obj = new chart_of_accounts();
            if (mode == "edit")
            {
                row_obj.id = obj.id;
            }
            else
            {
                row_obj.id = 0;
            }


            chart_of_accounts type = (chart_of_accounts)account_type_cmb.SelectedItem;
            chart_of_accounts category = (chart_of_accounts)category_account_cmb.SelectedItem;

            row_obj.account_name = account_textbox.Text;
            row_obj.p_id = category.id;
            row_obj.account_code = code;
            row_obj.account_full_code = account_code_textbox.Text;
            row_obj.account_type_id = type.account_type_id;
            row_obj.account_category_id = 2;

            row_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            row_obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
            row_obj.date_time = DateTime.Now;
        }

        bool validate()
        {
            if (account_type_cmb.SelectedIndex == 0)
            {
                account_type_cmb.Focus();
                string alertText = "Please Select Account Head";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (category_account_cmb.SelectedIndex == 0)
            {
                category_account_cmb.Focus();
                string alertText = "Please Select Category Account";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (account_textbox.Text.Count() == 0)
            {
                account_textbox.Focus();
                string alertText = "Please Enter Main Account Name";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (account_code_textbox.Text.Count() == 0)
            {
                account_code_textbox.Focus();
                string alertText = "Please Enter Main Account Code";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        //--------------    Save        ------------------------
        public void save()
        {
            if (validate())
            {
                fill_object();
                if (mode == "insert")
                {
                    if (!accountsDAL.checkAccountName(row_obj.account_name, row_obj.p_id, row_obj.id))
                    {
                        if (accountsDAL.insertAccount(row_obj) > 0)
                        {
                            MessageBox.Show("Record Added Successfully");
                            MAP.LoadData();
                            MessageBoxResult mbr = MessageBox.Show("Are You Want To Add More Records ?", "Again  Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (mbr == MessageBoxResult.Yes)
                            {
                                account_textbox.Text = "";
                                account_type_cmb.SelectedIndex = 0;
                                account_code_textbox.Text = "0";
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Duplicate entry");
                    }
                }
                else if (mode == "edit")
                {

                    if (!accountsDAL.checkAccountName(row_obj.account_name, row_obj.p_id, row_obj.id))
                    {
                        if (accountsDAL.updateAccount(row_obj) > 0)
                        {
                            MessageBox.Show("Record Updated Successfully");
                            this.Close();
                            MAP.LoadData();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Account Name Already Exist");
                    }
                }
                else
                {
                    MessageBox.Show("mode not set");
                }

            }
        }

        private void submit_btn_Click(object sender, RoutedEventArgs e)
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

        private void account_category_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            code = 1;
            if (category_account_cmb.SelectedItem != null)
            {
                if (category_account_cmb.SelectedIndex != 0)
                {
                    chart_of_accounts TypeObj = (chart_of_accounts)account_type_cmb.SelectedItem;
                    chart_of_accounts obj = (chart_of_accounts)category_account_cmb.SelectedItem;                    
                    try
                    {
                        code = accountsDAL.getLastAccountNo(obj.id) + 1;
                        account_code_textbox.Text = TypeObj.account_code.ToString("D2")+"-"+ obj.account_code.ToString("D2") + "-" + code.ToString("D3");
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        account_code_textbox.Text = TypeObj.account_code.ToString("D2") + "-" + obj.account_code.ToString("D2") + "-" + code.ToString("D3");
                    }
                }
                else
                {
                    account_code_textbox.Text = "";
                }
            }
        }

        private void account_type_cmb_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if(account_type_cmb.SelectedItem!= null)
            {
                chart_of_accounts selectedOBJ = (chart_of_accounts)account_type_cmb.SelectedItem;
                if(account_type_cmb.SelectedIndex != 0)
                {
                    acc_category_list = new List<chart_of_accounts>();
                    acc_category_list = accounts_list.Where(x => x.p_id == selectedOBJ.id).ToList();
                    acc_category_list.Insert(0, new chart_of_accounts() { id = -1, account_name = "--Select Category Account--" });
                    category_account_cmb.ItemsSource = acc_category_list;
                    category_account_cmb.SelectedIndex = 0;
                }
            }
        }
    }
}
