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
using MySql.Data.MySqlClient;

namespace SMS.FeeManagement.FeePaidByAmount
{
    /// <summary>
    /// Interaction logic for FeePaidByAmountPrint.xaml
    /// </summary>
    public partial class FeePaidByAmountPrint : Window
    {
        public FeePaidByAmountPrint(fee_voucher fv)
        {
            InitializeComponent();            
            this.DataContext = fv;            
        }

        

    }


}
