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
using System.Text.RegularExpressions;


namespace SMS.FeesManagement.FeesCollectionByVoucher
{
    /// <summary>
    /// Interaction logic for FeesCollectionByVoucherWindow.xaml
    /// </summary>
    public partial class FeesCollectionByVoucherWindow : Window
    {
        FeesDAL feesDAL;
        public bool isPaid = false;
        public FeesCollectionByVoucherWindow(sms_fees obj)
        {
            InitializeComponent();
            feesDAL = new FeesDAL();
            int paid_amount =  obj.total_amount;
            obj.total_paid = paid_amount;
            this.DataContext = obj;

            try
            {
                place_cmb.ItemsSource = feesDAL.getAllFeesCollectionPlace();
                if (MainWindow.d_FeeCollectionByVocherCollectionPlace == 0)
                {
                    place_cmb.SelectedValue = 22;
                }
                else
                {
                    place_cmb.SelectedValue = MainWindow.d_FeeCollectionByVocherCollectionPlace;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                place_cmb.SelectedIndex = 0;
            }
            date_picker.SelectedDate = DateTime.Now;

            paid_TB.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (!string.IsNullOrEmpty(paid_TB.Text) || !string.IsNullOrWhiteSpace(paid_TB.Text))
                {
                    if (paid_TB.Text != "0")
                    {
                        isPaid = true;
                        this.Close();
                    }
                }
            }
        }

        private void paid_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (paid_TB.Text != null && paid_TB.Text != "")
                {
                    rem_TB.Text = (Convert.ToInt32(total_TB.Text) - Convert.ToInt32(paid_TB.Text)).ToString();
                }
            }
            catch (Exception ex) { };
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void place_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (place_cmb != null && place_cmb.SelectedIndex > 0)
            {
                sms_fees_collection_place obj = (sms_fees_collection_place)place_cmb.SelectedItem;
                MainWindow.d_FeeCollectionByVocherCollectionPlace = obj.id;
            }
        }
    }
}
