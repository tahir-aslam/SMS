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
using Microsoft.Reporting.WinForms;

namespace SMS.AdvancedAccountManagement.DetailedAccounts
{
    /// <summary>
    /// Interaction logic for ChartOfAccountReportWindow.xaml
    /// </summary>
    public partial class ChartOfAccountReportWindow : Window
    {
        List<chart_of_accounts> accounts_list;

        public ChartOfAccountReportWindow(List<chart_of_accounts> lst)
        {
            InitializeComponent();
            accounts_list = lst;
            _reportViewer.Load += _reportViewer_Load;
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource accounts = new ReportDataSource();
            accounts.Name = "accounts"; //Name of the report dataset in our .RDLC file
            accounts.Value = accounts_list;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            this._reportViewer.LocalReport.DataSources.Add(accounts);
            this._reportViewer.LocalReport.DataSources.Add(ins);
            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.AdvancedAccountManagement.DetailedAccounts.ChartOfAccountsReport.rdlc";

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
