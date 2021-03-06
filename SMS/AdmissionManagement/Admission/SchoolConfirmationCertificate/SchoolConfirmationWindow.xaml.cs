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

namespace SMS.AdmissionManagement.Admission.SchoolConfirmationCertificate
{
    /// <summary>
    /// Interaction logic for SchoolConfirmationWindow.xaml
    /// </summary>
    public partial class SchoolConfirmationWindow : Window
    {
        List<admission> adm_list;

        public SchoolConfirmationWindow(List<admission> lst)
        {
            InitializeComponent();
            adm_list = lst;
            _reportViewer.Load += _reportViewer_Load;
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

            this._reportViewer.LocalReport.DataSources.Add(adm);
            this._reportViewer.LocalReport.DataSources.Add(ins);

            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.AdmissionManagement.Admission.SchoolConfirmationCertificate.SchoolConfirmationCertificateReport.rdlc";
            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
