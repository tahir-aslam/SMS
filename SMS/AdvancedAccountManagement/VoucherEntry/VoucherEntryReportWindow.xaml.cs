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

namespace SMS.AdvancedAccountManagement.VoucherEntry
{
    /// <summary>
    /// Interaction logic for VoucherEntryReportWindow.xaml
    /// </summary>
    public partial class VoucherEntryReportWindow : Window
    {
        List<sms_voucher_entries> voucher_entries;

        public VoucherEntryReportWindow(List<sms_voucher_entries> lst)
        {
            InitializeComponent();
            voucher_entries = lst;
            _reportViewer.Load += _reportViewer_Load;
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource voucher = new ReportDataSource();
            voucher.Name = "voucher"; //Name of the report dataset in our .RDLC file
            voucher.Value = voucher_entries;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            this._reportViewer.LocalReport.DataSources.Add(voucher);
            this._reportViewer.LocalReport.DataSources.Add(ins);
            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.AdvancedAccountManagement.VoucherEntry.VoucherEntryReport.rdlc";

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
