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

namespace SMS.FeeManagement.FeePaidByVoucher
{
    /// <summary>
    /// Interaction logic for FeePaidByVoucherWindow.xaml
    /// </summary>
    public partial class FeePaidByVoucherWindow : Window
    {
        public FeePaidByVoucherWindow(fee_voucher fv)
        {
            InitializeComponent();

            reciept_no_lbl.Content = fv.reciept_no;
            std_name_lbl.Content = fv.std_name;
            adm_no_lbl.Content = fv.adm_no;
            months_lbl.Content = fv.month;
            total_tb.Text = fv.total;
            date.SelectedDate = DateTime.Now;

            total_tb.Focus();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (total_tb.Text != "")
                {
                    FeePaidByVoucherPage.amount = total_tb.Text;
                    FeePaidByVoucherPage.status = "Y";
                    FeePaidByVoucherPage.paidDate = date.SelectedDate.Value;
                    this.Close();
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //FeePaidByVoucherPage.status = "N";
        }
    }
}
