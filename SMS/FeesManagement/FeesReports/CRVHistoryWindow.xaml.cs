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

namespace SMS.FeesManagement.FeesReports
{
    /// <summary>
    /// Interaction logic for CRVHistoryWindow.xaml
    /// </summary>
    public partial class CRVHistoryWindow : Window
    {
        List<sms_fees> fees_list;

        public CRVHistoryWindow(List<sms_fees> lst)
        {
            InitializeComponent();

            fees_list = lst;
            _reportViewer.Load += _reportViewer_Load;
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource fees = new ReportDataSource();
            fees.Name = "fees"; //Name of the report dataset in our .RDLC file
            fees.Value = fees_list;

            this._reportViewer.LocalReport.DataSources.Add(fees);
            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesReports.CashReceivedVoucherHistory.rdlc";

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
