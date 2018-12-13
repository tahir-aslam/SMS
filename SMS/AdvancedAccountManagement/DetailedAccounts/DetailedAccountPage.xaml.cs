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

namespace SMS.AdvancedAccountManagement.DetailedAccounts
{
    /// <summary>
    /// Interaction logic for DetailedAccountPage.xaml
    /// </summary>
    public partial class DetailedAccountPage : Page
    {
        AccountsDAL accountsDAL;
        List<chart_of_accounts> accountsList = new List<chart_of_accounts>();
       
        chart_of_accounts obj;
        string mode;
        DetailedAccountsWindow window;

        public DetailedAccountPage()
        {
            InitializeComponent();
            accountsDAL = new AccountsDAL();
            LoadData();
        }

        public void LoadData()
        {
            accountsList = accountsDAL.getAllChartOfAccounts();
            accountsList.Insert(0, new chart_of_accounts() { id = -1, account_name = "--Select Category Account--" });
            account_head_cmb.ItemsSource = accountsList.Where(x =>
            {
                if (x.p_id != 0 && x.account_category_id == 1) { return true; }
                else if (x.id == -1) { return true; }
                else { return false; }
            });
            account_head_cmb.SelectedIndex = 0;
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            window = new DetailedAccountsWindow(mode, this, obj);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            obj = (chart_of_accounts)account_grid.SelectedItem;
            if (obj == null)
            {
                // MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (mbr == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (accountsDAL.deleteAccount(obj.id) > 0)
                        {
                            LoadData();
                        }
                        else
                        {
                            LoadData();
                            MessageBox.Show("Cannot Delete");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {            
            ChartOfAccountReportWindow window = new ChartOfAccountReportWindow(accountsList.Where(x=>x.id != -1).ToList());
            window.ShowDialog();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void account_head_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (account_head_cmb.SelectedItem != null)
            {
                if (account_head_cmb.SelectedIndex == 0)
                {
                    account_grid.ItemsSource = accountsList.Where(x=>x.account_category_id == 2);
                }
                else 
                {
                    chart_of_accounts obj = (chart_of_accounts)account_head_cmb.SelectedItem;
                    account_grid.ItemsSource = accountsList.Where(x=>x.p_id == obj.id);
                }
                
            }
        }

        public void editing()
        {
            obj = (chart_of_accounts)account_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                window = new DetailedAccountsWindow(mode, this, obj);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }

        private void account_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }
    }
}
