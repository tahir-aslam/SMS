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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SMS.Models;
using SMS.DAL;
using Microsoft.Reporting.WinForms;

namespace SMS.EmployeeManagement.EmployeeAttendanceSheet
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceSheetPage.xaml
    /// </summary>
    public partial class EmployeeAttendanceSheetPage : Window
    {
        MiscDAL miscDAL;
        List<sms_months> months_list;
        List<sms_years> years_list;
        AttendanceBioDAL attDAL;

        public EmployeeAttendanceSheetPage()
        {
            InitializeComponent();

            miscDAL = new MiscDAL();
            attDAL = new AttendanceBioDAL();
            months_list = miscDAL.get_all_months();
            years_list = miscDAL.get_all_years();

            
            month_cmb.ItemsSource = months_list;            
            year_cmb.ItemsSource = years_list;

            year_cmb.SelectedValue = DateTime.Now.Year;
            month_cmb.SelectedValue = DateTime.Now.Month;
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void year_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            if (month_cmb.SelectedItem != null && year_cmb.SelectedItem!=null) 
            {
                sms_months month = (sms_months)month_cmb.SelectedItem;
                sms_years year = (sms_years)year_cmb.SelectedItem;

               
                DateTime sDate = new DateTime(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id),01);
                DateTime eDate = new DateTime(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id), DateTime.DaysInMonth(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id)));
                List<sms_emp_attendance_bio> AttendanceList = new List<sms_emp_attendance_bio>();
                List<sms_emp_attendance_bio> AttendanceListBio = new List<sms_emp_attendance_bio>();
                sms_emp_attendance_bio attObj;
                AttendanceList = attDAL.get_all__checkin_attendance_by_month(sDate, eDate).Where(x=>x.date_time != new DateTime(2001,01,01)).ToList();
                if (AttendanceList.Count > 0)
                {
                    for (int i = 1; i <= DateTime.DaysInMonth(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id)); i++)
                    {
                        if (AttendanceList.Where(x => x.date_time.Day == i).Count() == 0)
                        {
                            attObj = new sms_emp_attendance_bio();
                            attObj.date_time = new DateTime(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id), i);
                            attObj.emp_id = AttendanceList[0].emp_id;

                            AttendanceList.Add(attObj);
                        }
                    }



                    ReportDataSource att = new ReportDataSource();
                    att.Name = "att";
                    att.Value = AttendanceList;

                    ReportDataSource ins = new ReportDataSource();
                    List<institute> ins_list = new List<institute>();
                    MainWindow.ins.date = DateTime.Now;
                    MainWindow.ins.page_no = 1;
                    ins_list.Add(MainWindow.ins);
                    ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                    ins.Value = ins_list;

                    this._reportViewer3.LocalReport.DataSources.Clear();
                    this._reportViewer3.LocalReport.DataSources.Add(att);
                    this._reportViewer3.LocalReport.DataSources.Add(ins);
                    this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.EmployeeManagement.EmployeeAttendanceSheetBio.EmployeeAttendanceSheetReport.rdlc";

                    _reportViewer3.RefreshReport();
                }
                else 
                {
                    MessageBox.Show("Not Any record exist for this month");
                }
            }

        }
    }
}
