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
using SMS.DAL;
using SMS.Models;
using Microsoft.Reporting.WinForms;

namespace SMS.EmployeeManagement.EmployeeSalarySheetBio
{
    /// <summary>
    /// Interaction logic for EmployeeSalarySheetBioPage.xaml
    /// </summary>
    public partial class EmployeeSalarySheetBioPage : Window
    {
        MiscDAL miscDAL;
        List<sms_months> months_list;
        List<sms_years> years_list;
        AttendanceBioDAL attDAL;

        public EmployeeSalarySheetBioPage()
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
            if (month_cmb.SelectedItem != null && year_cmb.SelectedItem != null) 
            {
                sms_months month = (sms_months)month_cmb.SelectedItem;
                sms_years year = (sms_years)year_cmb.SelectedItem;

                DateTime sDate = new DateTime(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id), 01);
                DateTime eDate = new DateTime(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id), DateTime.DaysInMonth(Convert.ToInt32(year.year), Convert.ToInt32(month.month_id)));

                List<sms_emp_attendance_bio> attList = new List<sms_emp_attendance_bio>();
                List<sms_emp_attendance_bio> AttendanceList = new List<sms_emp_attendance_bio>();
                sms_emp_attendance_bio att_obj;
                int total_days = 0;


                attList = attDAL.get_all_attendance_by_date(sDate, eDate);
                total_days = attList.Select(x => x.date_time.Date).Where(x => x.Date != new DateTime(2001, 01, 01)).Distinct().Count();

                foreach (var emp_id in attList.Select(x => x.emp_id).Distinct())
                {
                    att_obj = new sms_emp_attendance_bio();
                    var att = attList.Where(x => x.emp_id == emp_id).First();

                    //Calculate total absents
                    foreach (var date_time in attList.Select(x => x.date_time.Date).Where(x => x.Date != new DateTime(2001, 01, 01)).Distinct())
                    {
                        if (attList.Where(x => x.emp_id == emp_id).Where(x => x.date_time.Date == date_time.Date).Count() > 0)
                        {
                            att_obj.total_presents++;
                        }
                        else
                        {
                            att_obj.total_absents++;
                        }
                    }
                    att_obj.emp_id = att.emp_id;
                    att_obj.emp_name = att.emp_name;
                    att_obj.father_name = att.father_name;
                    att_obj.designation = att.designation;
                    att_obj.designation_id = att.designation_id;
                    att_obj.total_days = total_days;
                    att_obj.salary = att.salary;
                    att_obj.deduction = att_obj.total_absents * (att_obj.salary / 30);

                    AttendanceList.Add(att_obj);

                }

                ReportDataSource attObj = new ReportDataSource();
                attObj.Name = "att";
                attObj.Value = AttendanceList;

                ReportDataSource ins = new ReportDataSource();
                List<institute> ins_list = new List<institute>();
                MainWindow.ins.date = DateTime.Now;
                MainWindow.ins.month_name = month.month_name;
                MainWindow.ins.year = year.year;
                MainWindow.ins.page_no = 1;
                ins_list.Add(MainWindow.ins);
                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                ins.Value = ins_list;

                this._reportViewer3.LocalReport.DataSources.Clear();
                this._reportViewer3.LocalReport.DataSources.Add(attObj);
                this._reportViewer3.LocalReport.DataSources.Add(ins);
                this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.EmployeeManagement.EmployeeSalarySheetBio.EmployeeSalarySheetReport.rdlc";

                _reportViewer3.RefreshReport();
            }
        }
    }
}
