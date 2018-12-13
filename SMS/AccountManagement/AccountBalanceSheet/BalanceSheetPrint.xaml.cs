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

namespace SMS.AccountManagement.AccountBalanceSheet
{
    /// <summary>
    /// Interaction logic for BalanceSheetPrint.xaml
    /// </summary>
    public partial class BalanceSheetPrint : Window
    {
        balanceSheet bs;
        public BalanceSheetPrint(balanceSheet BS)
        {
            InitializeComponent();
            this.bs = BS;
            this.DataContext = bs;
        }
    }
}
