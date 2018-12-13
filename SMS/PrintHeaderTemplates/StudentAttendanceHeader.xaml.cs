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

namespace SMS.PrintHeaderTemplates
{
    /// <summary>
    /// Interaction logic for StudentAttendanceHeader.xaml
    /// </summary>
    public partial class StudentAttendanceHeader : UserControl
    {
        public StudentAttendanceHeader()
        {
            InitializeComponent();
            Messages = MainWindow.ins.institute_name;
            dates = DateTime.Now.ToString("dd-MMM-yyyy");
            date_texts = "Attendance Date:  ";
            date_exps = SMS.StudentManagement.AttendanceReport.DailyAttendanceReportPage.dt.ToString("dd-MMM-yy");
            report_names = " Attendance Report ";
            amount_texts = "T.Strenght ="+ SMS.StudentManagement.AttendanceReport.DailyAttendanceReportPage.t_strenght+ ", T.Absents ="+SMS.StudentManagement.AttendanceReport.DailyAttendanceReportPage.t_absents;
            amounts = ", T.Presents =" + SMS.StudentManagement.AttendanceReport.DailyAttendanceReportPage.t_presents;
        }
        // date time now
        public static DependencyProperty institute_logo_Property = DependencyProperty.Register(
            "dates", typeof(string), typeof(StudentAttendanceHeader));

        public string dates
        {
            get
            {
                return (string)GetValue(institute_logo_Property);

            }
            set
            {
                SetValue(institute_logo_Property, value);
            }

        }

        // institute name
        public static DependencyProperty MessageProperty = DependencyProperty.Register(
            "Messages", typeof(string), typeof(StudentAttendanceHeader));
        public string Messages
        {
            get
            {
                return (string)GetValue(MessageProperty);

            }
            set
            {
                SetValue(MessageProperty, value);
            }

        }

        // DAte text
        public static DependencyProperty date_textProperty = DependencyProperty.Register(
            "date_texts", typeof(string), typeof(StudentAttendanceHeader));
        public string date_texts
        {
            get
            {
                return (string)GetValue(date_textProperty);

            }
            set
            {
                SetValue(date_textProperty, value);
            }

        }

        // date_exp
        public static DependencyProperty date_expProperty = DependencyProperty.Register(
            "date_exps", typeof(string), typeof(StudentAttendanceHeader));
        public string date_exps
        {
            get
            {
                return (string)GetValue(date_expProperty);

            }
            set
            {
                SetValue(date_expProperty, value);
            }

        }

        // Report Name
        public static DependencyProperty report_nameProperty = DependencyProperty.Register(
            "report_names", typeof(string), typeof(StudentAttendanceHeader));
        public string report_names
        {
            get
            {
                return (string)GetValue(report_nameProperty);

            }
            set
            {
                SetValue(report_nameProperty, value);
            }

        }

        // Amonut_Text
        public static DependencyProperty amount_textProperty = DependencyProperty.Register(
            "amount_texts", typeof(string), typeof(StudentAttendanceHeader));
        public string amount_texts
        {
            get
            {
                return (string)GetValue(amount_textProperty);

            }
            set
            {
                SetValue(amount_textProperty, value);
            }

        }

        // Amonut
        public static DependencyProperty amount_Property = DependencyProperty.Register(
            "amounts", typeof(string), typeof(StudentAttendanceHeader));
        public string amounts
        {
            get
            {
                return (string)GetValue(amount_Property);

            }
            set
            {
                SetValue(amount_Property, value);
            }

        }     
    }
}
