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
using Microsoft.Reporting.WinForms;
using SMS.Models;
using SMS.DAL;

namespace SMS.Reports.Student.AttendanceClassWise
{
    /// <summary>
    /// Interaction logic for AttendanceClassWiseWindow.xaml
    /// </summary>
    public partial class AttendanceClassWiseWindow : Window
    {
        List<student_attendence> lst;
        AttendanceDAL attDAL;

        public AttendanceClassWiseWindow()
        {
            InitializeComponent();
            attDAL = new AttendanceDAL();
            
            
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime sDATE = (DateTime)date_picker_to.SelectedDate;
            lst = attDAL.getStudentAttendanceGroupByClassAndAttendance(sDATE);

            ReportDataSource att = new ReportDataSource();
            att.Name = "att"; //Name of the report dataset in our .RDLC file
            att.Value = lst;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.sDate = sDATE;
            MainWindow.ins.page_no = 1;
            MainWindow.ins.month_name = lst.Count.ToString();
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;


            this._reportViewer3.LocalReport.DataSources.Add(att);
            this._reportViewer3.LocalReport.DataSources.Add(ins);
            this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.Reports.Student.AttendanceClassWise.StudentAttendanceReportClassWise.rdlc";

            _reportViewer3.RefreshReport();
            
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


            this._reportViewer3.LocalReport.DataSources.Add(att);
            this._reportViewer3.LocalReport.DataSources.Add(ins);
            this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.Reports.Student.AttendanceClassWise.StudentAttendanceReportClassWise.rdlc";

            _reportViewer3.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
