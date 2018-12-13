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
using Microsoft.Reporting.WinForms;
using SMS.Models;

namespace SMS.StudentManagement.StudentAttendence
{
    /// <summary>
    /// Interaction logic for AttendanceReportWindow.xaml
    /// </summary>
    
    public partial class AttendanceReportWindow : Window
    {
        List<student_attendence> lst;

        public AttendanceReportWindow(List<student_attendence> lst)
        {
            InitializeComponent();
            this.lst = lst;
            _reportViewer.Load += _reportViewer_Load;
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource att = new ReportDataSource();
            att.Name = "att"; //Name of the report dataset in our .RDLC file
            att.Value = lst;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            MainWindow.ins.month_name = lst.Count.ToString();
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;
            

            this._reportViewer.LocalReport.DataSources.Add(att);
            this._reportViewer.LocalReport.DataSources.Add(ins);
            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.StudentManagement.StudentAttendence.StudentAttendanceReport.rdlc";

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
