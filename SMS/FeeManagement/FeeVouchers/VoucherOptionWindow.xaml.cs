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
using SMS.FeeManagement.FeeVouchers;

namespace SMS.FeeManagement.FeeVouchers
{
    /// <summary>
    /// Interaction logic for VoucherOptionWindow.xaml
    /// </summary>
    public partial class VoucherOptionWindow : Window
    {
        bool one_slips = false;
        bool two_slips=false;
        bool three_slips=false;
        bool pending=false;

        FeeVoucherSearch fvs;

        public VoucherOptionWindow(FeeVoucherSearch FVS)
        {
            InitializeComponent();
            this.fvs = FVS;
        }

        private void submit_btn_Click(object sender, RoutedEventArgs e)
        {
            if (add_prev_chkbox.IsChecked == true)
            {
                pending = true;
            }
            else 
            {
                pending = false;
            }

            if (one_radiobtn.IsChecked == true)
            {
                one_slips = true;
            }
            else
            {
                one_slips = false;
            }

            if (two_radiobtn.IsChecked == true)
            {
                two_slips = true;
            }
            else 
            {
                two_slips = false;
            }

            if (three_radiobtn.IsChecked == true)
            {
                three_slips = true;
            }
            else 
            {
                three_slips = false;
            }
            fvs.get_voucher_options(pending,one_slips,two_slips,three_slips);
            this.Close();
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       
    }
}
