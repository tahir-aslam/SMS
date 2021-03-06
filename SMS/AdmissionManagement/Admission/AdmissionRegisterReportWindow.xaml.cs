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

namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for AdmissionRegisterReportWindow.xaml
    /// </summary>
    public partial class AdmissionRegisterReportWindow : Window
    {
        List<admission> adm_list;
        sms_report report_data;
        bool isAdm;

        public AdmissionRegisterReportWindow(List<admission> list, sms_report report_data, bool _isAdm = true)
        {
            InitializeComponent();
            adm_list = list;
            this.report_data = report_data;
            _reportViewer.Load += _reportViewer_Load;
            this.isAdm = _isAdm;
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource adm = new ReportDataSource();
            adm.Name = "adm"; //Name of the report dataset in our .RDLC file
            adm.Value = adm_list;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            ReportDataSource report_data = new ReportDataSource();
            List<sms_report> report_list = new List<sms_report>();
            report_list.Add(this.report_data);
            report_data.Name = "report_data"; //Name of the report dataset in our .RDLC file
            report_data.Value = report_list;

            this._reportViewer.LocalReport.DataSources.Add(adm);
            this._reportViewer.LocalReport.DataSources.Add(ins);
            this._reportViewer.LocalReport.DataSources.Add(report_data);

            if (isAdm)
            {
                this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.AdmissionManagement.Admission.AdmissionRegisterReport.rdlc";
            }
            else
            {
                this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.AdmissionManagement.Admission.AdmissionWithdrawlRegister.AdmissionWithdrawlRegisterReport.rdlc";
            }

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
