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

namespace SMS.EmployeeManagement.EmployeeAttendanceBio
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceBioReportWindow.xaml
    /// </summary>
    public partial class EmployeeAttendanceBioReportWindow : Window
    {
        DateTime sDate;
        DateTime eDate;
        List<sms_emp_attendance_bio> att_list;
        public EmployeeAttendanceBioReportWindow(List<sms_emp_attendance_bio> lst,DateTime sDate, DateTime eDate)
        {
            InitializeComponent();
            att_list = new List<sms_emp_attendance_bio>();
            att_list = lst;
            this.sDate = sDate;
            this.eDate = eDate;
            _reportViewer.Load += _reportViewer_Load;
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
           

            ReportDataSource att = new ReportDataSource();
            att.Name = "att"; //Name of the report dataset in our .RDLC file
            att.Value = att_list;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            MainWindow.ins.sDate = sDate;
            MainWindow.ins.eDate = eDate;
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            this._reportViewer.LocalReport.DataSources.Add(att);
            this._reportViewer.LocalReport.DataSources.Add(ins);


            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.EmployeeManagement.EmployeeAttendanceBio.EmployeeAttendanceBioReport.rdlc";
           


            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
