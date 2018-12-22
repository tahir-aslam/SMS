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
using SMS.ExamManagement.ExamResultList;
using SMS.Reports.Exam.GeneralAwardListByPosition;

namespace SMS.PrintHeaderTemplates
{
    /// <summary>
    /// Interaction logic for GenaralAwardListHeader.xaml
    /// </summary>
    public partial class GenaralAwardListHeader : UserControl
    {
        public GenaralAwardListHeader()
        {
            InitializeComponent();
            Messages = MainWindow.ins.institute_name;
            dates = DateTime.Now.ToString("dd-MMM-yyyy");
            date_texts = ResultList1.exam_name;
            date_exps = MainWindow.session.session_name;
            report_names = " General Award List ";
            amount_texts = ResultList1.class_name;
            amounts = ResultList1.section_name;
        }

        public GenaralAwardListHeader(SMS.Reports.Exam.GeneralAwardListByPosition.GeneralAwardListByPositionWindow obj)
        {
            InitializeComponent();
            Messages = MainWindow.ins.institute_name;
            dates = DateTime.Now.ToString("dd-MMM-yyyy");
            date_texts = "Subject ";
            date_exps = GeneralAwardListByPositionWindow.exam_name;
            report_names = "Subject Result List - " + MainWindow.session.session_name;
            amount_texts = GeneralAwardListByPositionWindow.class_name;
            amounts = GeneralAwardListByPositionWindow.section_name;
        }

        public GenaralAwardListHeader(SMS.ExamManagement.ExamResultListBySubject.ResultListBySubject RLBS)
        {
            InitializeComponent();
            Messages = MainWindow.ins.institute_name;
            dates = DateTime.Now.ToString("dd-MMM-yyyy");
            date_texts = "Subject ";
            date_exps = RLBS.subj.subject_name;
            report_names = "Subject Result List - " + MainWindow.session.session_name;
            amount_texts = RLBS.cl.class_name;
            amounts = RLBS.sec.section_name;
        }

        // date time now
        public static DependencyProperty institute_logo_Property = DependencyProperty.Register(
            "dates", typeof(string), typeof(GenaralAwardListHeader));

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
            "Messages", typeof(string), typeof(GenaralAwardListHeader));
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
            "date_texts", typeof(string), typeof(GenaralAwardListHeader));
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
            "date_exps", typeof(string), typeof(GenaralAwardListHeader));
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
            "report_names", typeof(string), typeof(GenaralAwardListHeader));
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
            "amount_texts", typeof(string), typeof(GenaralAwardListHeader));
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
            "amounts", typeof(string), typeof(GenaralAwardListHeader));
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
