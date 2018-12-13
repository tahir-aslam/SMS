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
using SMS.ViewModels;

namespace SMS
{
    /// <summary>
    /// Interaction logic for HeaderTemplate.xaml
    /// </summary>
    public partial class HeaderTemplate : UserControl
    {
        HeaderViewModel hvm;
        public HeaderTemplate()
        {
            InitializeComponent();
            //hvm = new HeaderViewModel();
            //this.DataContext = hvm;
            Message = MainWindow.ins.institute_name;
            date = DateTime.Now.ToString("dd-MMM-yyyy");
            
        }

        // institute logo
        public static DependencyProperty institute_logo_Property = DependencyProperty.Register(
            "date", typeof(string), typeof(HeaderTemplate));


        public string date
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
            "Message", typeof(string), typeof(HeaderTemplate));


        public string Message
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
    }
}
