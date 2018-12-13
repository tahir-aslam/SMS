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

namespace SMS.EmployeeManagement.EmployeeAttendanceBio
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceBioPage.xaml
    /// </summary>
    public partial class EmployeeAttendanceBioPage : Window
    {
        AttendanceBioDAL attDAL;
        List<sms_emp_attendance_bio> attList;
        List<sms_emp_attendance_bio> AttendanceList;
        public EmployeeAttendanceBioPage()
        {
            InitializeComponent();            
            attDAL = new AttendanceBioDAL();
            date_picker_to.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month,01);
            date_picker_from.SelectedDate = DateTime.Today;
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            DG1.ItemsSource = AttendanceList.Where(x => x.emp_name.ToUpper().StartsWith(v_search.ToUpper()) || x.emp_name.ToUpper().Contains(v_search.ToUpper()));
            DG1.Items.Refresh();
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;

                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    try
                    {
                        loadReport();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
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
                    try
                    {
                        loadReport();
                    }
                    catch (Exception ex)                    
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        void loadReport()
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                DateTime sDate = date_picker_to.SelectedDate.Value;
                DateTime eDate = date_picker_from.SelectedDate.Value;
                AttendanceList = new List<sms_emp_attendance_bio>();
                sms_emp_attendance_bio att_obj;
                int total_days = 0;

                
                attList = attDAL.get_all_attendance_by_date(sDate,eDate);
                total_days = attList.Select(x => x.date_time.Date).Where(x=>x.Date != new DateTime(2001,01,01)).Distinct().Count();
                
                foreach (var emp_id in attList.Select(x => x.emp_id).Distinct())
                {
                    att_obj = new sms_emp_attendance_bio();
                    var att = attList.Where(x => x.emp_id == emp_id).First();
                    
                    //Calculate total absents
                    foreach (var date_time in attList.Select(x => x.date_time.Date).Where(x=>x.Date != new DateTime(2001,01,01)).Distinct())
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
                    try
                    {
                        if (attList.Where(x => x.emp_id == emp_id).Where(x => x.mode == "checkin").Select(x => x.date_time).Count() > 0)
                        {

                            att_obj.check_in = attList.Where(x => x.emp_id == emp_id).Where(x => x.mode == "checkin").Select(x => x.date_time).Min();
                            att_obj.check_out = attList.Where(x => x.emp_id == emp_id).Where(x => x.mode == "checkout").Select(x => x.date_time).Max();
                            att_obj.total_hours = Math.Round((att_obj.check_out - att_obj.check_in).TotalHours, 2);
                            att_obj.total_days = total_days;
                        }
                        
                    }
                    catch(Exception ex)
                    {
                    }
                        
                    AttendanceList.Add(att_obj);
                    
                }
                EmpCountTB.Text = attList.Select(x => x.emp_id).Distinct().Count().ToString();                
                AbsentCountTB.Text = AttendanceList.Where(x => x.total_absents > 0).Count().ToString();
                DG1.ItemsSource = AttendanceList;
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeCombo.SelectedItem != null && date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null) 
            {
                if(EmployeeCombo.SelectedIndex == 0)
                {
                    DG1.ItemsSource = AttendanceList;
                }
                else if(EmployeeCombo.SelectedIndex == 1)
                {
                    DG1.ItemsSource = AttendanceList.Where(x=>x.total_presents > 0);
                }
                else if (EmployeeCombo.SelectedIndex == 2)
                {
                    DG1.ItemsSource = AttendanceList.Where(x => x.total_absents > 0);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void printbtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime sDate = date_picker_to.SelectedDate.Value;
            DateTime eDate = date_picker_from.SelectedDate.Value;
            EmployeeAttendanceBioReportWindow window = new EmployeeAttendanceBioReportWindow(AttendanceList,sDate,eDate);
            window.Show();
        }
    }
}
