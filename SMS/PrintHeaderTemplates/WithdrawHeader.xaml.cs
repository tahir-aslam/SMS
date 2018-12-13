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
    /// Interaction logic for WithdrawHeader.xaml
    /// </summary>
    public partial class WithdrawHeader : UserControl
    {
        public WithdrawHeader()
        {
            InitializeComponent();
            Messages = MainWindow.ins.institute_name;
            dates = DateTime.Now.ToString("dd-MMM-yyyy");                        
            report_names = "Withdrawal Register";            
            amounts = AdmissionManagement.WithdrawAdmission.WithdrawAdmissionPage.sess.session_name;
        }
        // date time now
        public static DependencyProperty institute_logo_Property = DependencyProperty.Register(
            "dates", typeof(string), typeof(WithdrawHeader));

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
            "Messages", typeof(string), typeof(WithdrawHeader));
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
        

        // date_exp
        public static DependencyProperty date_expProperty = DependencyProperty.Register(
            "date_exps", typeof(string), typeof(WithdrawHeader));
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
            "report_names", typeof(string), typeof(WithdrawHeader));
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
        
        // Amonut
        public static DependencyProperty amount_Property = DependencyProperty.Register(
            "amounts", typeof(string), typeof(WithdrawHeader));
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
