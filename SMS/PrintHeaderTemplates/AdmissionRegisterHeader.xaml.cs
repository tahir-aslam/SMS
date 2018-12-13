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
    /// Interaction logic for AdmissionRegisterHeader.xaml
    /// </summary>
    public partial class AdmissionRegisterHeader : UserControl
    {
        public AdmissionRegisterHeader()
        {
            InitializeComponent();
            Messages = MainWindow.ins.institute_name;
            logo = @"/SMS;component/images/ghazali.jpg";
            dates = DateTime.Now.ToString("dd-MMM-yyyy");
            date_texts = "Strength:  ";
            date_exps = AdmissionManagement.Admission.AdmissionSearch.strength.ToString();
            report_names = " Admission Register ";
            amount_texts = AdmissionManagement.Admission.AdmissionSearch.class_name;
            amounts = AdmissionManagement.Admission.AdmissionSearch.section_name;
        }

        // date time now
        public static DependencyProperty institute_logo_Property = DependencyProperty.Register(
            "dates", typeof(string), typeof(AdmissionRegisterHeader));

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
            "Messages", typeof(string), typeof(AdmissionRegisterHeader));
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
        // institute logo
        public static DependencyProperty logoProperty = DependencyProperty.Register(
            "logo", typeof(string), typeof(AdmissionRegisterHeader));
        public string logo
        {
            get
            {
                return (string)GetValue(logoProperty);

            }
            set
            {
                SetValue(logoProperty, value);
            }

        }

        // DAte text
        public static DependencyProperty date_textProperty = DependencyProperty.Register(
            "date_texts", typeof(string), typeof(AdmissionRegisterHeader));
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
            "date_exps", typeof(string), typeof(AdmissionRegisterHeader));
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
            "report_names", typeof(string), typeof(AdmissionRegisterHeader));
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
            "amount_texts", typeof(string), typeof(AdmissionRegisterHeader));
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
            "amounts", typeof(string), typeof(AdmissionRegisterHeader));
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
