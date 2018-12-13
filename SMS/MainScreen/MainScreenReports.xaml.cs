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
using SMS.Reports.Student.StudentCards;

namespace SMS.MainScreen
{
    /// <summary>
    /// Interaction logic for MainScreenReports.xaml
    /// </summary>
    public partial class MainScreenReports : Page
    {
        public MainScreenReports(string mode)
        {
            InitializeComponent();

            if(mode == "student")
            {
                student_GB.Visibility = Visibility.Visible;
            }
            else if(mode == "employee")
            {
                employee_GB.Visibility = Visibility.Visible;
            }
            else if (mode == "exam")
            {
                exam_GB.Visibility = Visibility.Visible;
            }
        }

        private void std_cards_Click(object sender, RoutedEventArgs e)
        {
            StudentCardsWindow window = new StudentCardsWindow();
            window.Show();

        }

        private void emp_cards_Click(object sender, RoutedEventArgs e)
        {
            SMS.Reports.Employee.EmployeeCardWindow window = new Reports.Employee.EmployeeCardWindow();
            window.Show();
        }

        private void std_list_Click(object sender, RoutedEventArgs e)
        {
            SMS.Reports.MiscReports.StudentListBySection.StudentListBySectionWindow window = new Reports.MiscReports.StudentListBySection.StudentListBySectionWindow();
            window.Show();
        }

        private void emp_attendance_bio_Click(object sender, RoutedEventArgs e)
        {
            EmployeeManagement.EmployeeAttendanceBio.EmployeeAttendanceBioPage window = new EmployeeManagement.EmployeeAttendanceBio.EmployeeAttendanceBioPage();
            window.Show();
        }

        private void emp_attendance_sheet_bio_Click(object sender, RoutedEventArgs e)
        {
            EmployeeManagement.EmployeeAttendanceSheet.EmployeeAttendanceSheetPage window = new EmployeeManagement.EmployeeAttendanceSheet.EmployeeAttendanceSheetPage();
            window.Show();
        }

        private void emp_salary_sheet_bio_Click(object sender, RoutedEventArgs e)
        {
            EmployeeManagement.EmployeeSalarySheetBio.EmployeeSalarySheetBioPage window = new EmployeeManagement.EmployeeSalarySheetBio.EmployeeSalarySheetBioPage();
            window.Show();
        }

        private void result_cards_graded_Click(object sender, RoutedEventArgs e)
        {
            Reports.Exam.ResultCardByGrade.ResultCardByGradeWindow window = new Reports.Exam.ResultCardByGrade.ResultCardByGradeWindow();
            window.Show();
        }

        private void std_attendance_report_Click(object sender, RoutedEventArgs e)
        {
            SMS.Reports.Student.Attendance.StudentAttendanceReportWindow window = new Reports.Student.Attendance.StudentAttendanceReportWindow();
            window.Show();
        }

        private void result_cards_conducted_Click(object sender, RoutedEventArgs e)
        {
            Reports.Exam.ResultCardByCoduct.ResultCardByConductWindow window = new Reports.Exam.ResultCardByCoduct.ResultCardByConductWindow();
            window.Show();
        }

        private void std_attendance_report_classwise_Click(object sender, RoutedEventArgs e)
        {
            SMS.Reports.Student.AttendanceClassWise.AttendanceClassWiseWindow window = new Reports.Student.AttendanceClassWise.AttendanceClassWiseWindow();
            window.Show();
        }

        private void twoexamreportbtn_Click(object sender, RoutedEventArgs e)
        {
            Reports.Exam.TwoExamReport.TwoExamReport window = new Reports.Exam.TwoExamReport.TwoExamReport();
            window.Show();
        }

        private void threeexamreportbtn_Click(object sender, RoutedEventArgs e)
        {
            Reports.Exam.ThreeExamReport.ThreeExamReportWindow window = new Reports.Exam.ThreeExamReport.ThreeExamReportWindow();
            window.Show();
        }

        private void fourexamreportbtn_Click(object sender, RoutedEventArgs e)
        {
            Reports.Exam.FourExamReport.FourExamReportWindow window = new Reports.Exam.FourExamReport.FourExamReportWindow();
            window.Show();
        }

        private void v_GeneralAwardListByPosition_Click(object sender, RoutedEventArgs e)
        {
            Reports.Exam.GeneralAwardListByPosition.GeneralAwardListByPositionWindow window = new Reports.Exam.GeneralAwardListByPosition.GeneralAwardListByPositionWindow();
            window.Show();
        }

        private void result_cards_custom_Click(object sender, RoutedEventArgs e)
        {
            Reports.Exam.ResultCardCustom.ResultCardCustomWindow window = new Reports.Exam.ResultCardCustom.ResultCardCustomWindow();
            window.Show();
        }
    }
}
