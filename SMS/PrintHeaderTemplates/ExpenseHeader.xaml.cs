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
using System.IO;

namespace SMS.PrintHeaderTemplates
{
    /// <summary>
    /// Interaction logic for ExpenseHeader.xaml
    /// </summary>
    public partial class ExpenseHeader : UserControl
    {
        public ExpenseHeader()
        {
            InitializeComponent();
            Message = MainWindow.ins.institute_name;
            date = DateTime.Now.ToString("dd-MMM-yyyy");
            date_text = "Expense Date:  ";
            date_exp = AccountManagement.AccountDataEntry.AccountDataSearch.dt.ToString("dd-MMM-yyyy");
            report_name = " Daily Expenditure Report ";
            amount_text = "Total Expenses:  ";
            amount = AccountManagement.AccountDataEntry.AccountDataSearch.total_amount.ToString()+" Rs";
            byte[] im = MainWindow.ins.institute_logo;
            //logo = ByteToImage(im);
            
        }

        // date time now
        public static DependencyProperty institute_logo_Property = DependencyProperty.Register(
            "date", typeof(string), typeof(ExpenseHeader));

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
            "Message", typeof(string), typeof(ExpenseHeader));
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

        // DAte text
        public static DependencyProperty date_textProperty = DependencyProperty.Register(
            "date_text", typeof(string), typeof(ExpenseHeader));
        public string date_text
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
            "date_exp", typeof(string), typeof(ExpenseHeader));
        public string date_exp
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
            "report_name", typeof(string), typeof(ExpenseHeader));
        public string report_name
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
            "amount_text", typeof(string), typeof(ExpenseHeader));
        public string amount_text
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
            "amount", typeof(string), typeof(ExpenseHeader));
        public string amount
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

        // Institue logo
        public static DependencyProperty logo_Property = DependencyProperty.Register(
            "logo", typeof(ImageSource), typeof(ExpenseHeader));
        public ImageSource logo
        {
            get
            {
                return (ImageSource)GetValue(logo_Property);

            }
            set
            {
                SetValue(logo_Property, value);
            }

        }

        public static  ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            try
            {

                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }
    }
}
