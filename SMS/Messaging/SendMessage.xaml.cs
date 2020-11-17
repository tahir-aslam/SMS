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
using SMS.Models;
using MySql.Data.MySqlClient;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using SMS.Messaging;
using SMS.Upload;
using SMS.Messaging.SavedSms;
using SMS.Messaging.FriendList;
using SMS.Messaging.History;
using SMS.Messaging.ExamSmsNew;
using SMS.Messaging.BrandedSms;
using SMS.Messaging.AttendenceSms;
using SMS.Messaging.BirthdaySms;

namespace SMS.Messaging
{
    /// <summary>
    /// Interaction logic for SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {
        

        public SendMessage()
        {
            InitializeComponent();
            institute_name_lbl.Content = MainWindow.ins.institute_name;
            institute_logo_img.Source = ByteToImage(MainWindow.ins.institute_logo);
            this.sms_frame.Navigate(new GeneralSms());
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void general_sms_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new GeneralSms());
        }

        private void exam_sms_Click(object sender, RoutedEventArgs e)
        {
            //this.sms_frame.Navigate(new ExamSms());
            this.sms_frame.Navigate(new ExamSmsNew.ExamSmsNew());
        }

        private void fee_defaulter_sms_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new FeeDefaulterSms());
        }

        private void fee_paid_sms_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new FeePaidSms());
        }

        private void saved_sms_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new SavedSmsSearch());
        }
        public static ImageSource ByteToImage(byte[] imageData)
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

        private void friends_sms_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new FriendListSearch());
        }

        private void history_btn_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new SmsHistory());
        }

        private void att_sms_btn_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new AttendenceSmsPage());
        }

        private void birthday_btn_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new BirthdaySmsPage());
        }

        private void emp_attendence_btn_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new EmpAttendanceSms.EmpAttendanceSmsPage());
        }

        private void test_sms_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new TestSms.TestSmsPage());
        }

        private void fee_defaulter_sms_new_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new FeesDefaulter.FeesDefaulterSmsPage());
        }

        private void fee_paid_sms_new_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new FeesPaid.FeesPaidSMSNew());
        }

        private void adm_info_sms_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new Admission.AdmissionSMSPage());
        }

        private void complaint_btn_Click(object sender, RoutedEventArgs e)
        {
            this.sms_frame.Navigate(new Messaging.ComplaintRegister.ComplaintSMS());
        }

        private void exam_sms_new_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}
