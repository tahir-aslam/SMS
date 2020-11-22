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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using SMS.Models;
using SMS.Messaging;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using System.Net;
using SMS.Messaging.ExamSmsNew;
using SMS.Messaging;
using SMS.Messaging.BirthdaySms;
using SMS.Messaging.EmpAttendanceSms;
using SMS.Messaging.TestSms;
using SMS.Messaging;

namespace SMS.Messaging.SmsOption
{
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        public bool IsClosed;
        public OptionWindow()
        {
            InitializeComponent();
            
        }

        //gsm
        private void Button_Click(object sender, RoutedEventArgs e)
        {        
            ExamSmsNew.ExamSmsNew.isbranded = false;
            GeneralSms.isbranded = false;
            BirthdaySmsPage.isbranded = false;
            EmpAttendanceSmsPage.isbranded = false;
            TestSmsPage.isbranded = false;
            FeeDefaulterSms.isbranded = false;
            FeePaidSms.isbranded = false;
            FeesPaid.FeesPaidSMSNew.isbranded = false;
            FeesDefaulter.FeesDefaulterSmsPage.isbranded = false;
            Admission.AdmissionSMSPage.isbranded = false;
            ComplaintRegister.ComplaintSMS.isbranded = false;
            this.Close();

        }

        //branded
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            ExamSmsNew.ExamSmsNew.isbranded = true;
            ExamsSMS.ExamsSMS.isbranded = true;
            GeneralSms.isbranded = true;
            GeneralSms.isFastSMS = false;
            AttendenceSms.AttendenceSmsPage.isbranded = true;
            BirthdaySmsPage.isbranded = true;
            EmpAttendanceSmsPage.isbranded = true;
            TestSmsPage.isbranded = true;
            FeeDefaulterSms.isbranded = true;
            FeePaidSms.isbranded = true;
            FeesDefaulter.FeesDefaulterSmsPage.isbranded = true;
            FeesPaid.FeesPaidSMSNew.isbranded = true;
            Admission.AdmissionSMSPage.isbranded = true;
            ComplaintRegister.ComplaintSMS.isbranded = true;
            this.Close();
        }

        //fast sms
        private void FastSMS_Click(object sender, RoutedEventArgs e)
        {
            ExamSmsNew.ExamSmsNew.isbranded = true;
            GeneralSms.isbranded = true;
            GeneralSms.isFastSMS = true;
            AttendenceSms.AttendenceSmsPage.isbranded = true;
            BirthdaySmsPage.isbranded = true;
            EmpAttendanceSmsPage.isbranded = true;
            TestSmsPage.isbranded = true;
            FeeDefaulterSms.isbranded = true;
            FeePaidSms.isbranded = true;
            FeesDefaulter.FeesDefaulterSmsPage.isbranded = true;
            FeesPaid.FeesPaidSMSNew.isbranded = true;
            Admission.AdmissionSMSPage.isbranded = true;
            ComplaintRegister.ComplaintSMS.isbranded = true;
            this.Close();
        }

        //close
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            IsClosed = true;
            ExamSmsNew.ExamSmsNew.isbranded = false;
            GeneralSms.isbranded = false;
            BirthdaySmsPage.isbranded = false;
            EmpAttendanceSmsPage.isbranded = false;
            TestSmsPage.isbranded = false;
            FeeDefaulterSms.isbranded = false;
            FeePaidSms.isbranded = false;
            FeesDefaulter.FeesDefaulterSmsPage.isbranded = false;
            FeesPaid.FeesPaidSMSNew.isbranded = false;
            Admission.AdmissionSMSPage.isbranded = false;
            ComplaintRegister.ComplaintSMS.isbranded = false;
            this.Close();
        }

      
    }
}
