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

namespace SMS.FeeManagement.FeeSearch
{
    /// <summary>
    /// Interaction logic for ReceiptNoWindow.xaml
    /// </summary>
    public partial class ReceiptNoWindow : Window
    {
        public string receipt_no = "";
        public ReceiptNoWindow()
        {
            InitializeComponent();
            receipt_textbox.Focus();
        }

        private void save() 
        {
            if (receipt_textbox.Text != "")
            {
                receipt_no = receipt_textbox.Text;
                FeeForm.cancelReceiptNumber = receipt_textbox.Text;
                this.Close();
            }
            else 
            {
                MessageBox.Show("Enter Correct Receipt#","Error",MessageBoxButton.OK,MessageBoxImage.Stop);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();   
            }
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            save();
        }
    }
}
