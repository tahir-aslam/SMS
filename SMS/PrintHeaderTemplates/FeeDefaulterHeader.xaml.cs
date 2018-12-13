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
    /// Interaction logic for FeeDefaulterHeader.xaml
    /// </summary>
    public partial class FeeDefaulterHeader : UserControl
    {
        public FeeDefaulterHeader()
        {
            InitializeComponent();
            Messages = MainWindow.ins.institute_name;
            dates = DateTime.Now.ToString("dd-MMM-yyyy");
            date_texts = "Month:  ";
            date_exps = FeeManagement.FeeDefaulters.FeeDefaultersList.months;
            report_names = " Fee Defaulters List ";           
        }

        // date time now
        public static DependencyProperty institute_logo_Property = DependencyProperty.Register(
            "dates", typeof(string), typeof(FeeDefaulterHeader));

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
            "Messages", typeof(string), typeof(FeeDefaulterHeader));
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
            "date_texts", typeof(string), typeof(FeeDefaulterHeader));
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
            "date_exps", typeof(string), typeof(FeeDefaulterHeader));
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
            "report_names", typeof(string), typeof(FeeDefaulterHeader));
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

       
    }
}
