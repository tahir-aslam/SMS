﻿using System;
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

namespace SMS.FeesManagement.FeesRegister
{
    /// <summary>
    /// Interaction logic for FeesVoucherHistoryReportWindow.xaml
    /// </summary>
    public partial class FeesVoucherHistoryReportWindow : Window
    {
        List<sms_fees> fees_list;

        public FeesVoucherHistoryReportWindow(List<sms_fees> lst)
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

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            this._reportViewer.LocalReport.DataSources.Add(fees);
            this._reportViewer.LocalReport.DataSources.Add(ins);
            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesRegister.FeesVoucherHistoryReport.rdlc";

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
