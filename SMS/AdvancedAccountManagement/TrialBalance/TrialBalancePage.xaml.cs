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
using SMS.DAL;
using SMS.Models;
using Microsoft.Reporting.WinForms;

namespace SMS.AdvancedAccountManagement.TrialBalance
{
    /// <summary>
    /// Interaction logic for TrialBalancePage.xaml
    /// </summary>
    public partial class TrialBalancePage : Page
    {
        AccountsDAL accountsDAL;

        public TrialBalancePage()
        {
            InitializeComponent();
            accountsDAL = new AccountsDAL();
        }        

        private void date_picker_from_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null )
            {                
                loadReport();                
            }
        }

        void loadReport()
        {            
                
                DateTime eDate = date_picker_from.SelectedDate.Value;

                List<sms_voucher_entries> entries = accountsDAL.getTrialBalance(eDate);
                foreach (var item in entries)
                {                    
                    item.to_date = eDate;
                }

                ReportDataSource voucher = new ReportDataSource();
                voucher.Name = "voucher";
                voucher.Value = entries;

                ReportDataSource ins = new ReportDataSource();
                List<institute> ins_list = new List<institute>();
                MainWindow.ins.date = DateTime.Now;
                MainWindow.ins.page_no = 1;
                ins_list.Add(MainWindow.ins);
                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                ins.Value = ins_list;

                this._reportViewer3.LocalReport.DataSources.Clear();
                this._reportViewer3.LocalReport.DataSources.Add(voucher);
                this._reportViewer3.LocalReport.DataSources.Add(ins);
                this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.AdvancedAccountManagement.TrialBalance.TrialBalanceReport.rdlc";

                _reportViewer3.RefreshReport();
            
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }
    }
}
