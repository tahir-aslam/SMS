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

namespace SMS.AdvancedAccountManagement.MainAccounts
{
    /// <summary>
    /// Interaction logic for MainAccountsPage.xaml
    /// </summary>
    public partial class MainAccountsPage : Page
    {
        AccountsDAL accountsDAL;
        List<chart_of_accounts> accountsList;
        chart_of_accounts obj;
        string mode;
        MainAccountWindow window;

        public MainAccountsPage()
        {
            InitializeComponent();
            accountsDAL = new AccountsDAL();
            LoadData();
        }

        public void LoadData() 
        {
            try
            {
                accountsList = accountsDAL.getAllChartOfAccounts();
                account_grid.ItemsSource = accountsList.Where(x => x.p_id != 0).Where(x=>x.account_category_id == 1);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            window = new MainAccountWindow(mode, this, obj);
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

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

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
                window = new MainAccountWindow(mode, this, obj);
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
