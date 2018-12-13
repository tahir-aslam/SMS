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
using SMS.DAL;
using Microsoft.Reporting.WinForms;
using System.ComponentModel;

namespace SMS.Reports.Student.Attendance
{
    /// <summary>
    /// Interaction logic for StudentAttendanceReportWindow.xaml
    /// </summary>
    public partial class StudentAttendanceReportWindow : Window
    {
        List<student_attendence> lst;
        AttendanceDAL attendanceDAL;
        List<classes> classes_list;
        List<sections> sections_list;
        ClassesDAL classesDAL;

        public StudentAttendanceReportWindow()
        {
            InitializeComponent();
            lst = new List<student_attendence>();
            attendanceDAL = new AttendanceDAL();
            classesDAL = new ClassesDAL();
            

            try
            {
                classes_list = classesDAL.get_all_classes();                
            

            classes_list.Insert(0, new classes() { class_name = "---All Classes---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            class_cmb.SelectedIndex = 0;
            attendance_cmb.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;

                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    loadReport();
                }
            }
        }

        private void date_picker_from_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    loadReport();
                }
            }
        }

        void generateReport(List<student_attendence> lst) 
        {
            DateTime sDate = date_picker_to.SelectedDate.Value;
            DateTime eDate = date_picker_from.SelectedDate.Value;                    

            ReportDataSource att = new ReportDataSource();
            att.Name = "att";
            att.Value = lst;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            MainWindow.ins.sDate = sDate;
            MainWindow.ins.eDate = eDate;
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            this._reportViewer3.LocalReport.DataSources.Clear();
            this._reportViewer3.LocalReport.DataSources.Add(att);
            this._reportViewer3.LocalReport.DataSources.Add(ins);
            this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.Reports.Student.Attendance.StudentAttendanceReport.rdlc";

            _reportViewer3.RefreshReport();
        }

        void loadReport()
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                try
                {
                    DateTime sDate = date_picker_to.SelectedDate.Value;
                    DateTime eDate = date_picker_from.SelectedDate.Value;                    
                    
                    lst = attendanceDAL.getStudentAttendanceByDate(sDate, eDate);
                    generateReport(lst);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void attendance_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (attendance_cmb.SelectedItem != null && lst.Count > 0)
            {
                
                if (attendance_cmb.SelectedIndex == 0)
                {
                    generateReport(lst);
                }
                else if (attendance_cmb.SelectedIndex == 1)
                {
                    generateReport(lst.Where(x => x.attendence == 'P').Where(x => x.total_attendance > 0).ToList());
                }
                else if (attendance_cmb.SelectedIndex == 2)
                {
                    generateReport(lst.Where(x => x.attendence == 'A').Where(x => x.total_attendance > 0).ToList());
                }
                else if (attendance_cmb.SelectedIndex == 3)
                {
                    generateReport(lst.Where(x => x.attendence == 'L').Where(x => x.total_attendance > 0).ToList());
                }
            }
        }
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb.SelectedItem != null && lst.Count > 0)
            {
                classes cl = (classes)class_cmb.SelectedItem;

                if (class_cmb.SelectedIndex != 0)
                {                    
                    section_cmb.IsEnabled = true;
                    sections_list = new List<sections>();
                    sections_list = classesDAL.get_all_sections(cl.id);
                    sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;

                    if (class_cmb.SelectedIndex != 0)
                    {
                        generateReport(lst.Where(x => x.class_id == cl.id).ToList());
                    }
                    else
                    {
                        generateReport(lst.Where(x => x.class_id != "0").ToList());
                    }
                }
                else
                {
                }
            }
        }
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sections sec = (sections)section_cmb.SelectedItem;
            if (section_cmb.SelectedItem != null && lst.Count > 0)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    generateReport(lst.Where(x=>x.section_id == sec.id).ToList());
                }
                else
                {
                    generateReport(lst.Where(x => x.section_id != "0").ToList());
                }
            }
        }        
    }
}
