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
using SMS.AdmissionManagement.Admission;

namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for GetDateRemarksWindow.xaml
    /// </summary>
    public partial class GetDateRemarksWindow : Window
    {
        AdmissionForm AF;
        AdmissionFormNew AFN;

        public GetDateRemarksWindow(AdmissionForm af)
        {
            InitializeComponent();
            this.AF = af;
            
        }
        public GetDateRemarksWindow(AdmissionFormNew afn)
        {
            InitializeComponent();
            this.AFN = afn;
        }

        private void withdraw_btn_Click(object sender, RoutedEventArgs e)
        {
            if (date_textbox.SelectedDate != null)
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Sure to Withdrawal This Admission ?", "Withdrawal Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    if (AF != null)
                    {
                        DateTime dt = date_textbox.SelectedDate.Value;
                        AF.withdrawal_date = dt;
                        AF.remarks = remarks_tb.Text;
                        AF.withdraw_status = true;
                    }
                    else 
                    {
                        DateTime dt = date_textbox.SelectedDate.Value;
                        AFN.withdrawal_date = dt;
                        AFN.remarks = remarks_tb.Text;
                        AFN.withdraw_status = true;
                    }
                    this.Close();
                }
                else
                {
                    if (AF != null)
                    {
                        AF.withdraw_status = false;
                        
                    }
                    else 
                    {
                        AFN.withdraw_status = false;
                    }
                    this.Close();
                }
            }
            else 
            {
                AFN.withdraw_status = false;
                MessageBox.Show("Please Enter Withdrawal Date");
            }
        }
    }
}
