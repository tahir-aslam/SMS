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
using SMS.MainScreen;
using SMS.ClassManagement.Class;
using SMS.ClassManagement.Subject;
using SMS.ClassManagement.Section;
using MahApps.Metro.Controls;
using System.Windows.Media.Effects;
using SMS.AdmissionManagement.Admission;
using SMS.EmployeeManagement.ADDEMP;
using System.Data.SqlClient;
using SMS.FeeManagement.FeeSearch;
using SMS.StudentManagement.StudentAttendence;
using SMS.EmployeeManagement;
using SMS.EmployeeManagement.EmpLogin;
using SMS.EmployeeManagement.EmpPayment;
using SMS.Messaging;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using SMS.Web;
using SMS.DataUploader;
using SMS.ExamManagement.Exam;
using SMS.ExamManagement.ExamDataEntry;
using SMS.ExamManagement.ExamResultList;
using SMS.ExamManagement.ExamResultCard;
using SMS.FeeManagement.PaidFeeList;
using SMS.FeeManagement.FeeDefaulters;
using SMS.FeeManagement.PaidFeeReport;
using SMS.AccountManagement.Account;
using SMS.EmployeeManagement.EmpRoles;
using SMS.EmployeeManagement.UnitTest;
using SMS.Models;
using SMS.FeeManagement.FeeVouchers;
using SMS.AdmissionManagement.WithdrawAdmission;
using SMS.FeeManagement.FeePaidByVoucher;
using SMS.EmployeeManagement.EmployeeAttendance;
using SMS.AdvancedAccountManagement;
using System.ComponentModel;
using System.Net;
using SMS.DAL;
using SMS.ComplaintManagment;
using SMS.ExamsManagement.ExamDataEntry;
using SMS.ExamsManagement.GeneralAwardList;

namespace SMS.MainScreen
{
    /// <summary>
    /// Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : Window
    {
        BlurEffect blur = new BlurEffect();
        bool check = false;
        public bool IsInternetConnection = false;
        public DateTime OnlineDate;
        private MainWindow m_MainWindow;

        public MainScreen()
        {
            //this.m_MainWindow = mw;
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            dispatcherTimer.Start();
            InitializeComponent();

            this.session_name_tb.Text = MainWindow.session.session_name;
            day_textblock.Text = DateTime.Now.ToString("D");
            this.mainFrame.Navigate(new Start());
            institute_name_lbl.Content = MainWindow.ins.institute_name;
            institute_logo_img.Source = MainWindow.ByteToImage(MainWindow.ins.institute_logo);


        }
        public MainScreen(int i)
        {
            InitializeComponent();
        }

        public void loadingPanel(bool visibility, string mainMessage, string subMessage)
        {
            SMS.Models.loadingPanel panel = new loadingPanel();
            panel.PanelLoading = visibility;
            panel.PanelMainMessage = mainMessage;
            panel.PanelSubMessage = subMessage;

            smsLoadingPanel.DataContext = panel;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            time_textblock.Text = DateTime.Now.ToString("HH:mm:ss");
        }


        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            blur.Radius = 0;
            this.Background = new SolidColorBrush(Colors.White);

        }

        private void MetroWindow_Deactivated(object sender, EventArgs e)
        {


            blur.Radius = 2;
            this.Background = new SolidColorBrush(Colors.White);

            this.Effect = blur;



        }


        public void apply_emp_roles_list()
        {
            try
            {
                if (MainWindow.emp_login_obj.emp_id == "0")
                {
                    foreach (var rol in MainWindow.basic_roles_list)
                    {
                        // Class Management
                        if (rol.id == "10" && rol.is_active == "Y")
                        {
                            class_mgmt_tab.Visibility = Visibility.Visible;
                        }

                        if (rol.id == "11" && rol.is_active == "Y")
                        {
                            button1.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "12" && rol.is_active == "Y")
                        {
                            section_button.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "13" && rol.is_active == "Y")
                        {
                            subject_button.Visibility = Visibility.Visible;
                        }

                        // Admission Management
                        if (rol.id == "20" && rol.is_active == "Y")
                        {
                            adm_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "21" && rol.is_active == "Y")
                        {
                            Admission_new.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "22" && rol.is_active == "Y")
                        {
                            Admission_withdraw.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "23" && rol.is_active == "Y")
                        {
                            envelope_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "24" && rol.is_active == "Y")
                        {
                            std_promote_btn.Visibility = Visibility.Visible;
                        }

                        // Fees Management
                        if (rol.id == "30" && rol.is_active == "Y")
                        {
                            fees_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "31" && rol.is_active == "Y")
                        {
                            ManageFeesBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "32" && rol.is_active == "Y")
                        {
                            FeesCollectionBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "33" && rol.is_active == "Y")
                        {
                            FeesCollectionReportBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "34" && rol.is_active == "Y")
                        {
                            FeesDefaulterBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "35" && rol.is_active == "Y")
                        {
                            FeesVouchersBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "36" && rol.is_active == "Y")
                        {
                            FeesByAmountBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "37" && rol.is_active == "Y")
                        {
                            FeesByVouchersBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "38" && rol.is_active == "Y")
                        {
                            bulkFeesButton.Visibility = Visibility.Visible;
                        }

                        //Employee Management
                        if (rol.id == "40" && rol.is_active == "Y")
                        {
                            emp_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "41" && rol.is_active == "Y")
                        {
                            add_emp.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "42" && rol.is_active == "Y")
                        {
                            withdraw_emp.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "43" && rol.is_active == "Y")
                        {
                            emp_attnd.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "44" && rol.is_active == "Y")
                        {
                            emp_login.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "45" && rol.is_active == "Y")
                        {
                            emp_roles_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "46" && rol.is_active == "Y")
                        {
                            emp_salary_sheet_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "47" && rol.is_active == "Y")
                        {
                            emp_payment.Visibility = Visibility.Visible;
                        }
                        //Student Management
                        if (rol.id == "50" && rol.is_active == "Y")
                        {
                            student_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "51" && rol.is_active == "Y")
                        {
                            std_attendence.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "52" && rol.is_active == "Y")
                        {
                            daily_attendance_report_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "53" && rol.is_active == "Y")
                        {
                            attendance_sheet_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "54" && rol.is_active == "Y")
                        {
                            attendance_report_class_wise.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "55" && rol.is_active == "Y")
                        {
                            std_attendence_history.Visibility = Visibility.Visible;
                        }
                        // Exam Management
                        if (rol.id == "60" && rol.is_active == "Y")
                        {
                            exam_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "61" && rol.is_active == "Y")
                        {
                            exams_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "62" && rol.is_active == "Y")
                        {
                            exam_entry.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "63" && rol.is_active == "Y")
                        {
                            result_list_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "64" && rol.is_active == "Y")
                        {
                            result_card_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "65" && rol.is_active == "Y")
                        {
                            result_card_btn_new.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "66" && rol.is_active == "Y")
                        {
                            exam_report_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "67" && rol.is_active == "Y")
                        {
                            result_list_subject.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "68" && rol.is_active == "Y")
                        {
                            midterm_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "69" && rol.is_active == "Y")
                        {
                            roll_no_slip_btn.Visibility = Visibility.Visible;
                        }

                        // A.Account Management
                        if (rol.id == "70" && rol.is_active == "Y")
                        {
                            adv_account_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "71" && rol.is_active == "Y")
                        {
                            main_accounts_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "72" && rol.is_active == "Y")
                        {
                            detailed_accounts_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "73" && rol.is_active == "Y")
                        {
                            voucher_entry_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "74" && rol.is_active == "Y")
                        {
                            account_ledger_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "75" && rol.is_active == "Y")
                        {
                            income_statement_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "76" && rol.is_active == "Y")
                        {
                            cashFlow_statement_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "77" && rol.is_active == "Y")
                        {
                            trial_balance_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "78" && rol.is_active == "Y")
                        {
                            balance_sheet_btn.Visibility = Visibility.Visible;
                        }

                        // Exams Management  // New exams management
                        if (rol.id == "110" && rol.is_active == "Y")
                        {
                            exams_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "111" && rol.is_active == "Y")
                        {
                            subjects_management_button.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "112" && rol.is_active == "Y")
                        {
                            define_exams_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "113" && rol.is_active == "Y")
                        {
                            exams_entry.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "114" && rol.is_active == "Y")
                        {
                            exams_entry.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "115" && rol.is_active == "Y")
                        {
                            general_award_list.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "116" && rol.is_active == "Y")
                        {
                            results_card_btn.Visibility = Visibility.Visible;
                        }

                        // SMS Management
                        if (rol.id == "80" && rol.is_active == "Y")
                        {
                            smsSend.Visibility = Visibility.Visible;
                        }

                        // Reports Management
                        if (rol.id == "100" && rol.is_active == "Y")
                        {
                            reports_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "101" && rol.is_active == "Y")
                        {
                            student_reports_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "102" && rol.is_active == "Y")
                        {
                            emp_reports_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "103" && rol.is_active == "Y")
                        {
                            exam_reports_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.id == "104" && rol.is_active == "Y")
                        {
                            finance_reports_btn.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    foreach (roles rol in MainWindow.roles_list)
                    {
                        // Class Management
                        if (rol.module_id == "10")
                        {
                            class_mgmt_tab.Visibility = Visibility.Visible;
                        }

                        if (rol.module_id == "11")
                        {
                            button1.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "12")
                        {
                            section_button.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "13")
                        {
                            subject_button.Visibility = Visibility.Visible;
                        }

                        // Admission Management
                        if (rol.module_id == "20")
                        {
                            adm_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "21")
                        {
                            Admission_new.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "22")
                        {
                            Admission_withdraw.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "23")
                        {
                            envelope_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "24")
                        {
                            std_promote_btn.Visibility = Visibility.Visible;
                        }

                        // Fees Management
                        if (rol.module_id == "30")
                        {
                            fees_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "31")
                        {
                            ManageFeesBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "32")
                        {
                            FeesCollectionBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "33")
                        {
                            FeesCollectionReportBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "34")
                        {
                            FeesDefaulterBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "35")
                        {
                            FeesVouchersBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "36")
                        {
                            FeesByAmountBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "37")
                        {
                            FeesByVouchersBTN.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "38")
                        {
                            bulkFeesButton.Visibility = Visibility.Visible;
                        }

                        //Employee Management
                        if (rol.module_id == "40")
                        {
                            emp_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "41")
                        {
                            add_emp.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "42")
                        {
                            withdraw_emp.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "43")
                        {
                            emp_attnd.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "44")
                        {
                            emp_login.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "45")
                        {
                            emp_roles_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "46")
                        {
                            emp_salary_sheet_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "47")
                        {
                            emp_payment.Visibility = Visibility.Visible;
                        }
                        //Student Management
                        if (rol.module_id == "50")
                        {
                            student_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "51")
                        {
                            std_attendence.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "52")
                        {
                            daily_attendance_report_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "53")
                        {
                            attendance_sheet_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "54")
                        {
                            attendance_report_class_wise.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "55")
                        {
                            std_attendence_history.Visibility = Visibility.Visible;
                        }
                        // Exam Management
                        if (rol.module_id == "60")
                        {
                            exam_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "61")
                        {
                            exams_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "62")
                        {
                            exam_entry.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "63")
                        {
                            result_list_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "64")
                        {
                            result_card_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "65")
                        {
                            result_card_btn_new.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "66")
                        {
                            exam_report_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "67")
                        {
                            result_list_subject.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "68")
                        {
                            midterm_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "69")
                        {
                            roll_no_slip_btn.Visibility = Visibility.Visible;
                        }

                        // A.Account Management
                        if (rol.module_id == "70")
                        {
                            adv_account_mgmt_tab.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "71")
                        {
                            main_accounts_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "72")
                        {
                            detailed_accounts_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "73")
                        {
                            voucher_entry_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "74")
                        {
                            account_ledger_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "75")
                        {
                            income_statement_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "76")
                        {
                            cashFlow_statement_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "77")
                        {
                            trial_balance_btn.Visibility = Visibility.Visible;
                        }
                        if (rol.module_id == "78")
                        {
                            balance_sheet_btn.Visibility = Visibility.Visible;
                        }

                        // SMS Management
                        if (rol.module_id == "80")
                        {
                            smsSend.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        //--------------------------------------   Clicks       ---------------------------------------------------------------


        private void classMgmt_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ClassSearch());
            this.mainFrame.Content = new ClassSearch();
            button1.Background = Brushes.Purple;
            button1.Foreground = Brushes.White;

            subjects_management_button.Background = Brushes.White;
            subjects_management_button.Foreground = Brushes.Black;

            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }

        private void sectionMgmt_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new SectionSearch());
            this.mainFrame.Content = new SectionSearch();
            section_button.Background = Brushes.Purple;
            section_button.Foreground = Brushes.White;

            subjects_management_button.Background = Brushes.White;
            subjects_management_button.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }

        private void SubjectMgmt_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new SubjectSearch());
            this.mainFrame.Content = new SubjectSearch();
            subject_button.Background = Brushes.Purple;
            subject_button.Foreground = Brushes.White;

            subjects_management_button.Background = Brushes.White;
            subjects_management_button.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }

       

        private void add_emp_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ADDEMPSEARCH());
            this.mainFrame.Content = new ADDEMPSEARCH();
            add_emp.Background = Brushes.Purple;
            add_emp.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }

        // Amission Form

        private void Admission_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AdmissionSearch());
            this.mainFrame.Content = new AdmissionSearch();
            Admission.Background = Brushes.Purple;
            Admission.Foreground = Brushes.White;

            Admission_new.Background = Brushes.White;
            Admission_new.Foreground = Brushes.Black;
            Admission_withdraw.Background = Brushes.White;
            Admission_withdraw.Foreground = Brushes.Black;
            fine_btn.Background = Brushes.White;
            fine_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }

        private void Admission_new_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new AdmissionSearchNew();
            Admission_new.Background = Brushes.Purple;
            Admission_new.Foreground = Brushes.White;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            Admission_withdraw.Background = Brushes.White;
            Admission_withdraw.Foreground = Brushes.Black;
            fine_btn.Background = Brushes.White;
            fine_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }

        private void Admission_withdraw_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new WithdrawAdmissionPage());
            this.mainFrame.Content = new WithdrawAdmissionPage();
            Admission_withdraw.Background = Brushes.Purple;
            Admission_withdraw.Foreground = Brushes.White;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }
        private void bulk_fee_update_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AdmissionManagement.BulkFeeUpdate.FeeUpdatePage());
            this.mainFrame.Content = new AdmissionManagement.BulkFeeUpdate.FeeUpdatePage();
            bulk_fee_update_btn.Background = Brushes.Purple;
            bulk_fee_update_btn.Foreground = Brushes.White;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            Admission_withdraw.Background = Brushes.White;
            Admission_withdraw.Foreground = Brushes.Black;
            fine_btn.Background = Brushes.White;
            fine_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }
        private void std_promote_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AdmissionManagement.PromoteStudents.PromoteStudentPage());
            this.mainFrame.Content = new AdmissionManagement.PromoteStudents.PromoteStudentPage();
            std_promote_btn.Background = Brushes.Purple;
            std_promote_btn.Foreground = Brushes.White;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            Admission_withdraw.Background = Brushes.White;
            Admission_withdraw.Foreground = Brushes.Black;
            bulk_fee_update_btn.Background = Brushes.White;
            bulk_fee_update_btn.Foreground = Brushes.Black;
            fine_btn.Background = Brushes.White;
            fine_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }
        private void fine_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AdmissionManagement.FineManagement.Fine.FineSearch());
            this.mainFrame.Content = new AdmissionManagement.FineManagement.Fine.FineSearch();
            fine_btn.Background = Brushes.Purple;
            fine_btn.Foreground = Brushes.White;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            Admission_withdraw.Background = Brushes.White;
            Admission_withdraw.Foreground = Brushes.Black;
            bulk_fee_update_btn.Background = Brushes.White;
            bulk_fee_update_btn.Foreground = Brushes.Black;
            std_promote_btn.Background = Brushes.White;
            std_promote_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
        }
        private void envelope_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AdmissionManagement.FineManagement.Fine.FineSearch());
            this.mainFrame.Content = new AdmissionManagement.Envelope.EnvelopePage();
            envelope_btn.Background = Brushes.Purple;
            envelope_btn.Foreground = Brushes.White;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            Admission_withdraw.Background = Brushes.White;
            Admission_withdraw.Foreground = Brushes.Black;
            bulk_fee_update_btn.Background = Brushes.White;
            bulk_fee_update_btn.Foreground = Brushes.Black;
            std_promote_btn.Background = Brushes.White;
            std_promote_btn.Foreground = Brushes.Black;
        }
        private void std_list_section_Click(object sender, RoutedEventArgs e)
        {
            SMS.Reports.MiscReports.StudentListBySection.StudentListBySectionWindow window = new Reports.MiscReports.StudentListBySection.StudentListBySectionWindow();
            window.Show();
        }

        private void fee_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new FeeSearch());
            this.mainFrame.Content = new FeeSearch();
            fee.Background = Brushes.Purple;
            fee.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

        }


        //------------------------------   Student Management  -----------------------------------------

        private void std_attendence_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new StudentAttendence());
            this.mainFrame.Content = new StudentAttendence();
            std_attendence.Background = Brushes.Purple;
            std_attendence.Foreground = Brushes.White;

            std_attendence_history.Background = Brushes.White;
            std_attendence_history.Foreground = Brushes.Black;
            attendance_sheet_btn.Background = Brushes.White;
            attendance_sheet_btn.Foreground = Brushes.Black;
            attendance_report_class_wise.Background = Brushes.White;
            attendance_report_class_wise.Foreground = Brushes.Black;
            daily_attendance_report_btn.Background = Brushes.White;
            daily_attendance_report_btn.Foreground = Brushes.Black;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new StudentManagement.AttendanceReport.DailyAttendanceReportPage();
            daily_attendance_report_btn.Background = Brushes.Purple;
            daily_attendance_report_btn.Foreground = Brushes.White;

            attendance_sheet_btn.Background = Brushes.White;
            attendance_sheet_btn.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            attendance_report_class_wise.Background = Brushes.White;
            attendance_report_class_wise.Foreground = Brushes.Black;
        }
        private void attendance_sheet_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new StudentManagement.AttendanceReport.StudentAttendanceSheetPage();
            attendance_sheet_btn.Background = Brushes.Purple;
            attendance_sheet_btn.Foreground = Brushes.White;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            daily_attendance_report_btn.Background = Brushes.White;
            daily_attendance_report_btn.Foreground = Brushes.Black;
            attendance_report_class_wise.Background = Brushes.White;
            attendance_report_class_wise.Foreground = Brushes.Black;
        }
        private void attendance_report_class_wise_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new StudentManagement.AttendanceReportClassWise.AttendanceReportClassWisePage();
            attendance_report_class_wise.Background = Brushes.Purple;
            attendance_report_class_wise.Foreground = Brushes.White;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            attendance_sheet_btn.Background = Brushes.White;
            attendance_sheet_btn.Foreground = Brushes.Black;
            daily_attendance_report_btn.Background = Brushes.White;
            daily_attendance_report_btn.Foreground = Brushes.Black;
        }

        private void std_attendence_history_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new StudentAttendanceNew();
            std_attendence_history.Background = Brushes.Purple;
            std_attendence_history.Foreground = Brushes.White;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            attendance_sheet_btn.Background = Brushes.White;
            attendance_sheet_btn.Foreground = Brushes.Black;
            daily_attendance_report_btn.Background = Brushes.White;
            daily_attendance_report_btn.Foreground = Brushes.Black;
        }

        private void V_ComplaintRegiter_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new ComplaintRegister();
            V_ComplaintRegiter.Background = Brushes.Purple;
            V_ComplaintRegiter.Foreground = Brushes.White;

            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            attendance_sheet_btn.Background = Brushes.White;
            attendance_sheet_btn.Foreground = Brushes.Black;
            daily_attendance_report_btn.Background = Brushes.White;
            daily_attendance_report_btn.Foreground = Brushes.Black;
        }

        private void emp_login_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new EmployeeLoginSearch());
            this.mainFrame.Content = new EmployeeLoginSearch();
            emp_login.Background = Brushes.Purple;
            emp_login.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
            emp_salary_sheet_btn.Background = Brushes.White;
            emp_salary_sheet_btn.Foreground = Brushes.Black;
        }

        private void mainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void emp_attnd_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new EmployeeAttendancePage());
            this.mainFrame.Content = new EmployeeAttendancePage();
            emp_attnd.Background = Brushes.Purple;
            emp_attnd.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_salary_sheet_btn.Background = Brushes.White;
            emp_salary_sheet_btn.Foreground = Brushes.Black;
        }

        private void emp_payment_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new EmpPayment());
            this.mainFrame.Content = new EmpPayment();
            emp_payment.Background = Brushes.Purple;
            emp_payment.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;

            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
            emp_salary_sheet_btn.Background = Brushes.White;
            emp_salary_sheet_btn.Foreground = Brushes.Black;
        }
        private void emp_roles_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new EmployeeRolesSearch());
            this.mainFrame.Content = new EmployeeRolesSearch();
            emp_roles_btn.Background = Brushes.Purple;
            emp_roles_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;


            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;

            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
            emp_payment.Background = Brushes.White;
            emp_payment.Foreground = Brushes.Black;
            emp_salary_sheet_btn.Background = Brushes.White;
            emp_salary_sheet_btn.Foreground = Brushes.Black;
        }

        private void withdraw_emp_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new EmployeeManagement.WithdrawEmployees.WithdrawEmployeesPage());
            this.mainFrame.Content = new EmployeeManagement.WithdrawEmployees.WithdrawEmployeesPage();
            withdraw_emp.Background = Brushes.Purple;
            withdraw_emp.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;

            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
            emp_payment.Background = Brushes.White;
            emp_payment.Foreground = Brushes.Black;
            emp_roles_btn.Background = Brushes.White;
            emp_roles_btn.Foreground = Brushes.Black;
            emp_salary_sheet_btn.Background = Brushes.White;
            emp_salary_sheet_btn.Foreground = Brushes.Black;
        }

        private void emp_salary_sheet_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new EmployeeManagement.SalarySheet.SalarySheetPage();
            emp_salary_sheet_btn.Background = Brushes.Purple;
            emp_salary_sheet_btn.Foreground = Brushes.White;

            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;
            withdraw_emp.Background = Brushes.White;
            withdraw_emp.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;
            emp_payment.Background = Brushes.White;
            emp_payment.Foreground = Brushes.Black;
            emp_roles_btn.Background = Brushes.White;
            emp_roles_btn.Foreground = Brushes.Black;
        }

        private Dispatcher _dispatcher;
        public void SetDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        private void smsSend_Click(object sender, RoutedEventArgs e)
        {

            SendMessage sm = new SendMessage();
            sm.Show();
        }


        private void web_btn_Click(object sender, RoutedEventArgs e)
        {
            SchoolWeb sw = new SchoolWeb();
            sw.Show();
        }

        private void data_uploader_btn_Click(object sender, RoutedEventArgs e)
        {
            DataUploaderWindow duw;

            if (DataUploaderWindow.window_open)
            {
                DataUploaderWindow.duw.Activate();
            }
            else
            {
                duw = new DataUploaderWindow();
                duw.Show();
            }

        }
        private void home_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new Start());
            this.mainFrame.Content = new Start();
        }

        private void institute_name_lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.mainFrame.Navigate(new Start());
            this.mainFrame.Content = new Start();
        }



        //---------------------------- Exam Management --------------------------------------------------------

        private void exams_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ExamSearch());
            this.mainFrame.Content = new ExamSearch();
            exams_btn.Background = Brushes.Purple;
            exams_btn.Foreground = Brushes.White;

            roll_no_slip_btn.Background = Brushes.White;
            roll_no_slip_btn.Foreground = Brushes.Black;
            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
            result_card_btn_new.Background = Brushes.White;
            result_card_btn_new.Foreground = Brushes.Black;
            exam_report_btn.Background = Brushes.White;
            exam_report_btn.Foreground = Brushes.Black;
            result_list_subject.Background = Brushes.White;
            result_list_subject.Foreground = Brushes.Black;
        }
        private void exam_entry_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ExamDataEntry1());
            this.mainFrame.Content = new ExamDataEntry1();
            exam_entry.Background = Brushes.Purple;
            exam_entry.Foreground = Brushes.White;

            roll_no_slip_btn.Background = Brushes.White;
            roll_no_slip_btn.Foreground = Brushes.Black;
            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
            result_card_btn_new.Background = Brushes.White;
            result_card_btn_new.Foreground = Brushes.Black;
            exam_report_btn.Background = Brushes.White;
            exam_report_btn.Foreground = Brushes.Black;
            result_list_subject.Background = Brushes.White;
            result_list_subject.Foreground = Brushes.Black;
        }
        private void result_list_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ResultList1());
            this.mainFrame.Content = new ResultList1();
            result_list_btn.Background = Brushes.Purple;
            result_list_btn.Foreground = Brushes.White;

            roll_no_slip_btn.Background = Brushes.White;
            roll_no_slip_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
            result_card_btn_new.Background = Brushes.White;
            result_card_btn_new.Foreground = Brushes.Black;
            exam_report_btn.Background = Brushes.White;
            exam_report_btn.Foreground = Brushes.Black;
            result_list_subject.Background = Brushes.White;
            result_list_subject.Foreground = Brushes.Black;
        }
        private void result_card_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ResultCard1());
            this.mainFrame.Content = new ResultCard1();
            result_card_btn.Background = Brushes.Purple;
            result_card_btn.Foreground = Brushes.White;

            roll_no_slip_btn.Background = Brushes.White;
            roll_no_slip_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            exam_report_btn.Background = Brushes.White;
            exam_report_btn.Foreground = Brushes.Black;
            result_card_btn_new.Background = Brushes.White;
            result_card_btn_new.Foreground = Brushes.Black;
            result_list_subject.Background = Brushes.White;
            result_list_subject.Foreground = Brushes.Black;

        }
        private void result_card_btn_new_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ResultCard2());
            this.mainFrame.Content = new ResultCard2();
            result_card_btn_new.Background = Brushes.Purple;
            result_card_btn_new.Foreground = Brushes.White;

            roll_no_slip_btn.Background = Brushes.White;
            roll_no_slip_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            exam_report_btn.Background = Brushes.White;
            exam_report_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
            result_list_subject.Background = Brushes.White;
            result_list_subject.Foreground = Brushes.Black;
        }
        private void exam_report_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ExamManagement.ExamReport.ExamReportPage());
            this.mainFrame.Content = new ExamManagement.ExamReport.ExamReportPage();
            exam_report_btn.Background = Brushes.Purple;
            exam_report_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
            result_card_btn_new.Background = Brushes.White;
            result_card_btn_new.Foreground = Brushes.Black;
            result_list_subject.Background = Brushes.White;
            result_list_subject.Foreground = Brushes.Black;
        }
        private void result_list_subject_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ExamManagement.ExamResultListBySubject.ResultListBySubject());
            this.mainFrame.Content = new ExamManagement.ExamResultListBySubject.ResultListBySubject();
            result_list_subject.Background = Brushes.Purple;
            result_list_subject.Foreground = Brushes.White;

            roll_no_slip_btn.Background = Brushes.White;
            roll_no_slip_btn.Foreground = Brushes.Black;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
            result_card_btn_new.Background = Brushes.White;
            result_card_btn_new.Foreground = Brushes.Black;
            exam_report_btn.Background = Brushes.White;
            exam_report_btn.Foreground = Brushes.Black;

        }
        private void midterm_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new ExamManagement.ExamMultipleResultCards.MultipleResultCardsPage());
            this.mainFrame.Content = new ExamManagement.ExamMultipleResultCards.MultipleResultCardsPage();
            midterm_btn.Background = Brushes.Purple;
            midterm_btn.Foreground = Brushes.White;
        }
        private void roll_no_slip_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new ExamManagement.RollNoSlip.RollNoSlipPage();
            roll_no_slip_btn.Background = Brushes.Purple;
            roll_no_slip_btn.Foreground = Brushes.White;

            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
        }

        // -------------------------- Exams management new exam system --------------------------------

        private void Subjects_Mgmt_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new SubjectSearch());
            this.mainFrame.Content = new ClassManagement.Subjects.SubjectManagementPage();
            subjects_management_button.Background = Brushes.Purple;
            subjects_management_button.Foreground = Brushes.White;

            exams_entry.Background = Brushes.White;
            exams_entry.Foreground = Brushes.Purple;
            define_exams_btn.Background = Brushes.White;
            define_exams_btn.Foreground = Brushes.Purple;
            general_award_list.Background = Brushes.White;
            general_award_list.Foreground = Brushes.Purple;
        }
        private void define_exams_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new ExamSearch();
            define_exams_btn.Background = Brushes.Purple;
            define_exams_btn.Foreground = Brushes.White;

            subjects_management_button.Background = Brushes.White;
            subjects_management_button.Foreground = Brushes.Purple;
            exams_entry.Background = Brushes.White;
            exams_entry.Foreground = Brushes.Purple;
            general_award_list.Background = Brushes.White;
            general_award_list.Foreground = Brushes.Purple;
        }

        private void exams_entry_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new ExamDataEntryPage();
            exams_entry.Background = Brushes.Purple;
            exams_entry.Foreground = Brushes.White;

            subjects_management_button.Background = Brushes.White;
            subjects_management_button.Foreground = Brushes.Purple;
            define_exams_btn.Background = Brushes.White;
            define_exams_btn.Foreground = Brushes.Purple;
            general_award_list.Background = Brushes.White;
            general_award_list.Foreground = Brushes.Purple;
        }

        private void general_award_list_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new GeneralAwardListPage();
            general_award_list.Background = Brushes.Purple;
            general_award_list.Foreground = Brushes.White;

            exams_entry.Background = Brushes.White;
            exams_entry.Foreground = Brushes.Purple;
            subjects_management_button.Background = Brushes.White;
            subjects_management_button.Foreground = Brushes.Purple;
            define_exams_btn.Background = Brushes.White;
            define_exams_btn.Foreground = Brushes.Purple;
        }

        private void results_card_btn_Click(object sender, RoutedEventArgs e)
        {

        }


        // fees management------------------------------------------------------------------------------
        private void paidfee_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new PaidFee());
            this.mainFrame.Content = new PaidFee();
            paidfee_btn.Background = Brushes.Purple;
            paidfee_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;
            fee_vouchers_btn.Background = Brushes.White;
            fee_vouchers_btn.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;


            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
            exam_report_btn.Background = Brushes.White;
            exam_report_btn.Foreground = Brushes.Black;

        }

        private void Defaulterfee_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new FeeDefaultersList());
            this.mainFrame.Content = new FeeDefaultersList();
            Defaulterfee_btn.Background = Brushes.Purple;
            Defaulterfee_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;
            fee_vouchers_btn.Background = Brushes.White;
            fee_vouchers_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
        }

        private void paidfeeReport_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new PaidFeeReport());
            this.mainFrame.Content = new PaidFeeReport();
            paidfeeReport_btn.Background = Brushes.Purple;
            paidfeeReport_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            fee_vouchers_btn.Background = Brushes.White;
            fee_vouchers_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

        }

        private void fee_vouchers_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new FeeVoucherSearch());
            this.mainFrame.Content = new FeeVoucherSearch();
            fee_vouchers_btn.Background = Brushes.Purple;
            fee_vouchers_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;
            fee_vouchers_btn.Background = Brushes.White;
            fee_vouchers_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
        }

        private void fee_vouchers_paid_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new FeePaidByVoucherPage());
            this.mainFrame.Content = new FeePaidByVoucherPage();
            fee_vouchers_btn.Background = Brushes.Purple;
            fee_vouchers_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;
            fee_vouchers_btn.Background = Brushes.White;
            fee_vouchers_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

        }

        private void fee_amount_paid_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new FeeManagement.FeePaidByAmount.FeePaidByAmountPage());
            this.mainFrame.Content = new FeeManagement.FeePaidByAmount.FeePaidByAmountPage();
            fee_amount_paid_btn.Background = Brushes.Purple;
            fee_amount_paid_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;
            paidfeeReport_btn.Background = Brushes.White;
            paidfeeReport_btn.Foreground = Brushes.Black;
            fee_vouchers_btn.Background = Brushes.White;
            fee_vouchers_btn.Foreground = Brushes.Black;
            fee_vouchers_btn.Background = Brushes.White;
            fee_vouchers_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;
        }

        private void accounts_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AccountSearch());
            this.mainFrame.Content = new AccountSearch();
            accounts_btn.Background = Brushes.Purple;
            accounts_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

            account_entry_btn.Background = Brushes.White;
            account_entry_btn.Foreground = Brushes.Black;
            investors_btn.Background = Brushes.White;
            investors_btn.Foreground = Brushes.Black;
            investments_btn.Background = Brushes.White;
            investments_btn.Foreground = Brushes.Black;
            cash_detail_btn.Background = Brushes.White;
            cash_detail_btn.Foreground = Brushes.Black;

        }
        private void account_entry_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AccountManagement.AccountDataEntry.AccountDataSearch());
            this.mainFrame.Content = new AccountManagement.AccountDataEntry.AccountDataSearch();
            account_entry_btn.Background = Brushes.Purple;
            account_entry_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

            accounts_btn.Background = Brushes.White;
            accounts_btn.Foreground = Brushes.Black;
            investors_btn.Background = Brushes.White;
            investors_btn.Foreground = Brushes.Black;
            investments_btn.Background = Brushes.White;
            investments_btn.Foreground = Brushes.Black;
            cash_detail_btn.Background = Brushes.White;
            cash_detail_btn.Foreground = Brushes.Black;
        }

        private void account_balance_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AccountManagement.AccountBalanceSheet.BalanceSheet());
            this.mainFrame.Content = new AccountManagement.AccountBalanceSheet.BalanceSheet();
            account_balance_btn.Background = Brushes.Purple;
            account_balance_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

            accounts_btn.Background = Brushes.White;
            accounts_btn.Foreground = Brushes.Black;
            account_entry_btn.Background = Brushes.White;
            account_entry_btn.Foreground = Brushes.Black;
            investors_btn.Background = Brushes.White;
            investors_btn.Foreground = Brushes.Black;
            investments_btn.Background = Brushes.White;
            investments_btn.Foreground = Brushes.Black;
            cash_detail_btn.Background = Brushes.White;
            cash_detail_btn.Foreground = Brushes.Black;

        }

        private void investors_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AccountManagement.Investors.InverstersPage());
            this.mainFrame.Content = new AccountManagement.Investors.InverstersPage();
            investors_btn.Background = Brushes.Purple;
            investors_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

            accounts_btn.Background = Brushes.White;
            accounts_btn.Foreground = Brushes.Black;
            account_entry_btn.Background = Brushes.White;
            account_entry_btn.Foreground = Brushes.Black;
            account_balance_btn.Background = Brushes.White;
            account_balance_btn.Foreground = Brushes.Black;
            investments_btn.Background = Brushes.White;
            investments_btn.Foreground = Brushes.Black;
            cash_detail_btn.Background = Brushes.White;
            cash_detail_btn.Foreground = Brushes.Black;
        }

        private void investments_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AccountManagement.Investment.InvestmentPage());
            this.mainFrame.Content = new AccountManagement.Investment.InvestmentPage();
            investments_btn.Background = Brushes.Purple;
            investments_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

            accounts_btn.Background = Brushes.White;
            accounts_btn.Foreground = Brushes.Black;
            account_entry_btn.Background = Brushes.White;
            account_entry_btn.Foreground = Brushes.Black;
            account_balance_btn.Background = Brushes.White;
            account_balance_btn.Foreground = Brushes.Black;
            investors_btn.Background = Brushes.White;
            investors_btn.Foreground = Brushes.Black;
            cash_detail_btn.Background = Brushes.White;
            cash_detail_btn.Foreground = Brushes.Black;
        }

        private void cash_detail_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new AccountManagement.CashDetail.CashDetailPage());
            this.mainFrame.Content = new AccountManagement.CashDetail.CashDetailPage();
            cash_detail_btn.Background = Brushes.Purple;
            cash_detail_btn.Foreground = Brushes.White;

            button1.Background = Brushes.White;
            button1.Foreground = Brushes.Black;
            subject_button.Background = Brushes.White;
            subject_button.Foreground = Brushes.Black;
            section_button.Background = Brushes.White;
            section_button.Foreground = Brushes.Black;
            add_emp.Background = Brushes.White;
            add_emp.Foreground = Brushes.Black;

            fee.Background = Brushes.White;
            fee.Foreground = Brushes.Black;
            paidfee_btn.Background = Brushes.White;
            paidfee_btn.Foreground = Brushes.Black;
            Defaulterfee_btn.Background = Brushes.White;
            Defaulterfee_btn.Foreground = Brushes.Black;

            Admission.Background = Brushes.White;
            Admission.Foreground = Brushes.Black;
            std_attendence.Background = Brushes.White;
            std_attendence.Foreground = Brushes.Black;
            emp_login.Background = Brushes.White;
            emp_login.Foreground = Brushes.Black;
            emp_attnd.Background = Brushes.White;
            emp_attnd.Foreground = Brushes.Black;

            exam_entry.Background = Brushes.White;
            exam_entry.Foreground = Brushes.Black;
            exams_btn.Background = Brushes.White;
            exams_btn.Foreground = Brushes.Black;
            result_list_btn.Background = Brushes.White;
            result_list_btn.Foreground = Brushes.Black;
            result_card_btn.Background = Brushes.White;
            result_card_btn.Foreground = Brushes.Black;

            accounts_btn.Background = Brushes.White;
            accounts_btn.Foreground = Brushes.Black;
            account_entry_btn.Background = Brushes.White;
            account_entry_btn.Foreground = Brushes.Black;
            account_balance_btn.Background = Brushes.White;
            account_balance_btn.Foreground = Brushes.Black;
            investors_btn.Background = Brushes.White;
            investors_btn.Foreground = Brushes.Black;
            investments_btn.Background = Brushes.White;
            investments_btn.Foreground = Brushes.Black;
        }


        private void unit_test_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.mainFrame.Navigate(new UnitTestPage());
            this.mainFrame.Content = new UnitTestPage();
        }

        private void logout_btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void setting_btn_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel.AdminPasswordWindow apw = new AdminPanel.AdminPasswordWindow();
            apw.ShowDialog();
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }



        // Advance Account System       

        private void main_accounts_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.MainAccounts.MainAccountsPage());
            main_accounts_btn.Background = Brushes.Purple;
            main_accounts_btn.Foreground = Brushes.White;

            detailed_accounts_btn.Background = Brushes.White;
            detailed_accounts_btn.Foreground = Brushes.Black;
            voucher_entry_btn.Background = Brushes.White;
            voucher_entry_btn.Foreground = Brushes.Black;
            balance_sheet_btn.Background = Brushes.White;
            balance_sheet_btn.Foreground = Brushes.Black;
            account_ledger_btn.Background = Brushes.White;
            account_ledger_btn.Foreground = Brushes.Black;
            income_statement_btn.Background = Brushes.White;
            income_statement_btn.Foreground = Brushes.Black;
            cashFlow_statement_btn.Background = Brushes.White;
            cashFlow_statement_btn.Foreground = Brushes.Black;
            trial_balance_btn.Background = Brushes.White;
            trial_balance_btn.Foreground = Brushes.Black;
        }
        private void detailed_accounts_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.DetailedAccounts.DetailedAccountPage());
            detailed_accounts_btn.Background = Brushes.Purple;
            detailed_accounts_btn.Foreground = Brushes.White;

            main_accounts_btn.Background = Brushes.White;
            main_accounts_btn.Foreground = Brushes.Black;
            voucher_entry_btn.Background = Brushes.White;
            voucher_entry_btn.Foreground = Brushes.Black;
            balance_sheet_btn.Background = Brushes.White;
            balance_sheet_btn.Foreground = Brushes.Black;
            account_ledger_btn.Background = Brushes.White;
            account_ledger_btn.Foreground = Brushes.Black;
            income_statement_btn.Background = Brushes.White;
            income_statement_btn.Foreground = Brushes.Black;
            cashFlow_statement_btn.Background = Brushes.White;
            cashFlow_statement_btn.Foreground = Brushes.Black;
            trial_balance_btn.Background = Brushes.White;
            trial_balance_btn.Foreground = Brushes.Black;
        }
        private void voucher_entry_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.VoucherEntry.VoucherEntryPage());
            voucher_entry_btn.Background = Brushes.Purple;
            voucher_entry_btn.Foreground = Brushes.White;

            main_accounts_btn.Background = Brushes.White;
            main_accounts_btn.Foreground = Brushes.Black;
            detailed_accounts_btn.Background = Brushes.White;
            detailed_accounts_btn.Foreground = Brushes.Black;
            balance_sheet_btn.Background = Brushes.White;
            balance_sheet_btn.Foreground = Brushes.Black;
            account_ledger_btn.Background = Brushes.White;
            account_ledger_btn.Foreground = Brushes.Black;
            income_statement_btn.Background = Brushes.White;
            income_statement_btn.Foreground = Brushes.Black;
            cashFlow_statement_btn.Background = Brushes.White;
            cashFlow_statement_btn.Foreground = Brushes.Black;
            trial_balance_btn.Background = Brushes.White;
            trial_balance_btn.Foreground = Brushes.Black;
        }
        private void account_ledger_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.AccountsLedger.AccountsLedgerPage());
            account_ledger_btn.Background = Brushes.Purple;
            account_ledger_btn.Foreground = Brushes.White;

            main_accounts_btn.Background = Brushes.White;
            main_accounts_btn.Foreground = Brushes.Black;
            detailed_accounts_btn.Background = Brushes.White;
            detailed_accounts_btn.Foreground = Brushes.Black;
            voucher_entry_btn.Background = Brushes.White;
            voucher_entry_btn.Foreground = Brushes.Black;
            balance_sheet_btn.Background = Brushes.White;
            balance_sheet_btn.Foreground = Brushes.Black;
            income_statement_btn.Background = Brushes.White;
            income_statement_btn.Foreground = Brushes.Black;
            cashFlow_statement_btn.Background = Brushes.White;
            cashFlow_statement_btn.Foreground = Brushes.Black;
            trial_balance_btn.Background = Brushes.White;
            trial_balance_btn.Foreground = Brushes.Black;

        }
        private void balance_sheet_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.BalanceSheet.BalanceSheetPage());
            balance_sheet_btn.Background = Brushes.Purple;
            balance_sheet_btn.Foreground = Brushes.White;

            main_accounts_btn.Background = Brushes.White;
            main_accounts_btn.Foreground = Brushes.Black;
            detailed_accounts_btn.Background = Brushes.White;
            detailed_accounts_btn.Foreground = Brushes.Black;
            voucher_entry_btn.Background = Brushes.White;
            voucher_entry_btn.Foreground = Brushes.Black;
            account_ledger_btn.Background = Brushes.White;
            account_ledger_btn.Foreground = Brushes.Black;
            income_statement_btn.Background = Brushes.White;
            income_statement_btn.Foreground = Brushes.Black;
            cashFlow_statement_btn.Background = Brushes.White;
            cashFlow_statement_btn.Foreground = Brushes.Black;
            trial_balance_btn.Background = Brushes.White;
            trial_balance_btn.Foreground = Brushes.Black;
        }
        private void income_statement_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.IncomeStatement.IncomeStatementPage());
            income_statement_btn.Background = Brushes.Purple;
            income_statement_btn.Foreground = Brushes.White;

            main_accounts_btn.Background = Brushes.White;
            main_accounts_btn.Foreground = Brushes.Black;
            detailed_accounts_btn.Background = Brushes.White;
            detailed_accounts_btn.Foreground = Brushes.Black;
            voucher_entry_btn.Background = Brushes.White;
            voucher_entry_btn.Foreground = Brushes.Black;
            account_ledger_btn.Background = Brushes.White;
            account_ledger_btn.Foreground = Brushes.Black;
            balance_sheet_btn.Background = Brushes.White;
            balance_sheet_btn.Foreground = Brushes.Black;
            cashFlow_statement_btn.Background = Brushes.White;
            cashFlow_statement_btn.Foreground = Brushes.Black;
            trial_balance_btn.Background = Brushes.White;
            trial_balance_btn.Foreground = Brushes.Black;
        }
        private void cashFlow_statement_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.CashFlowStatement.CashFlowStatementPage());
            cashFlow_statement_btn.Background = Brushes.Purple;
            cashFlow_statement_btn.Foreground = Brushes.White;

            main_accounts_btn.Background = Brushes.White;
            main_accounts_btn.Foreground = Brushes.Black;
            detailed_accounts_btn.Background = Brushes.White;
            detailed_accounts_btn.Foreground = Brushes.Black;
            voucher_entry_btn.Background = Brushes.White;
            voucher_entry_btn.Foreground = Brushes.Black;
            account_ledger_btn.Background = Brushes.White;
            account_ledger_btn.Foreground = Brushes.Black;
            balance_sheet_btn.Background = Brushes.White;
            balance_sheet_btn.Foreground = Brushes.Black;
            income_statement_btn.Background = Brushes.White;
            income_statement_btn.Foreground = Brushes.Black;
            trial_balance_btn.Background = Brushes.White;
            trial_balance_btn.Foreground = Brushes.Black;
        }
        private void trial_balance_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new AdvancedAccountManagement.TrialBalance.TrialBalancePage());
            trial_balance_btn.Background = Brushes.Purple;
            trial_balance_btn.Foreground = Brushes.White;

            main_accounts_btn.Background = Brushes.White;
            main_accounts_btn.Foreground = Brushes.Black;
            detailed_accounts_btn.Background = Brushes.White;
            detailed_accounts_btn.Foreground = Brushes.Black;
            voucher_entry_btn.Background = Brushes.White;
            voucher_entry_btn.Foreground = Brushes.Black;
            account_ledger_btn.Background = Brushes.White;
            account_ledger_btn.Foreground = Brushes.Black;
            balance_sheet_btn.Background = Brushes.White;
            balance_sheet_btn.Foreground = Brushes.Black;
            income_statement_btn.Background = Brushes.White;
            income_statement_btn.Foreground = Brushes.Black;
            cashFlow_statement_btn.Background = Brushes.White;
            cashFlow_statement_btn.Foreground = Brushes.Black;
        }


        // Fees Management
        private void ManageFees_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new SMS.FeesManagement.ManageFees.ManageFeesPage();
            ManageFeesBTN.Background = Brushes.Purple;
            ManageFeesBTN.Foreground = Brushes.White;

            FeesCollectionBTN.Background = Brushes.White;
            FeesCollectionBTN.Foreground = Brushes.Black;
            FeesCollectionReportBTN.Background = Brushes.White;
            FeesCollectionReportBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void FeesCollectionBTN_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new SMS.FeesManagement.FeesCollection.FeesCollectionPage();
            FeesCollectionBTN.Background = Brushes.Purple;
            FeesCollectionBTN.Foreground = Brushes.White;

            FeesCollectionReportBTN.Background = Brushes.White;
            FeesCollectionReportBTN.Foreground = Brushes.Black;
            ManageFeesBTN.Background = Brushes.White;
            ManageFeesBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void FeesCollectionReportBTN_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new FeesManagement.FeesCollectionLedger.FeesCollectionLedgerPage());
            FeesCollectionReportBTN.Background = Brushes.Purple;
            FeesCollectionReportBTN.Foreground = Brushes.White;

            ManageFeesBTN.Background = Brushes.White;
            ManageFeesBTN.Foreground = Brushes.Black;
            FeesCollectionBTN.Background = Brushes.White;
            FeesCollectionBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void FeesDefaulterBTN_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new FeesManagement.FeesDefaulters.FeesDefaultersPage());
            FeesDefaulterBTN.Background = Brushes.Purple;
            FeesDefaulterBTN.Foreground = Brushes.White;

            FeesCollectionReportBTN.Background = Brushes.White;
            FeesCollectionReportBTN.Foreground = Brushes.Black;
            ManageFeesBTN.Background = Brushes.White;
            ManageFeesBTN.Foreground = Brushes.Black;
            FeesCollectionBTN.Background = Brushes.White;
            FeesCollectionBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void sampleReportsBTN_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Content = new SMS.FeesManagement.SampleReports.SampleReportsPage();
            sampleReportsBTN.Background = Brushes.Purple;
            sampleReportsBTN.Foreground = Brushes.White;

            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void FeesVouchersBTN_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new FeesManagement.FeesVoucher.FeesVoucherPage());
            FeesVouchersBTN.Background = Brushes.Purple;
            FeesVouchersBTN.Foreground = Brushes.White;

            FeesCollectionReportBTN.Background = Brushes.White;
            FeesCollectionReportBTN.Foreground = Brushes.Black;
            ManageFeesBTN.Background = Brushes.White;
            ManageFeesBTN.Foreground = Brushes.Black;
            FeesCollectionBTN.Background = Brushes.White;
            FeesCollectionBTN.Foreground = Brushes.Black;
            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void FeesByAmountBTN_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new FeesManagement.FeesCollectionByAmount.FeesCollectionByAmountPage());
            FeesByAmountBTN.Background = Brushes.Purple;
            FeesByAmountBTN.Foreground = Brushes.White;

            FeesCollectionReportBTN.Background = Brushes.White;
            FeesCollectionReportBTN.Foreground = Brushes.Black;
            ManageFeesBTN.Background = Brushes.White;
            ManageFeesBTN.Foreground = Brushes.Black;
            FeesCollectionBTN.Background = Brushes.White;
            FeesCollectionBTN.Foreground = Brushes.Black;
            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void FeesByVouchersBTN_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new FeesManagement.FeesCollectionByVoucher.FeesCollectionByVoucherPage());
            FeesByVouchersBTN.Background = Brushes.Purple;
            FeesByVouchersBTN.Foreground = Brushes.White;

            FeesCollectionReportBTN.Background = Brushes.White;
            FeesCollectionReportBTN.Foreground = Brushes.Black;
            ManageFeesBTN.Background = Brushes.White;
            ManageFeesBTN.Foreground = Brushes.Black;
            FeesCollectionBTN.Background = Brushes.White;
            FeesCollectionBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            bulkFeesButton.Background = Brushes.White;
            bulkFeesButton.Foreground = Brushes.Black;
        }

        private void bulkFeesButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new FeesManagement.BulkFeesUpdate.BulkFeesUpdatePage());
            bulkFeesButton.Background = Brushes.Purple;
            bulkFeesButton.Foreground = Brushes.White;

            FeesCollectionReportBTN.Background = Brushes.White;
            FeesCollectionReportBTN.Foreground = Brushes.Black;
            ManageFeesBTN.Background = Brushes.White;
            ManageFeesBTN.Foreground = Brushes.Black;
            FeesCollectionBTN.Background = Brushes.White;
            FeesCollectionBTN.Foreground = Brushes.Black;
            FeesByAmountBTN.Background = Brushes.White;
            FeesByAmountBTN.Foreground = Brushes.Black;
            FeesVouchersBTN.Background = Brushes.White;
            FeesVouchersBTN.Foreground = Brushes.Black;
            sampleReportsBTN.Background = Brushes.White;
            sampleReportsBTN.Foreground = Brushes.Black;
            FeesDefaulterBTN.Background = Brushes.White;
            FeesDefaulterBTN.Foreground = Brushes.Black;
            FeesByVouchersBTN.Background = Brushes.White;
            FeesByVouchersBTN.Foreground = Brushes.Black;
        }



        // --------------------   Reports Managment  ----------------------------------


        // Student Reports
        private void student_reports_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new MainScreenReports("student"));
        }
        private void emp_reports_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new MainScreenReports("employee"));
        }
        private void exam_reports_btn_Click(object sender, RoutedEventArgs e)
        {
            this.mainFrame.Navigate(new MainScreenReports("exam"));
        }

        private void finance_reports_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            apply_emp_roles_list();
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();


        }

        public void apply_license()
        {
            try
            {
                //get latet institute
                MainWindow.get_sms_institute();

                // check date time online
                if (IsInternetConnection)
                {
                    if (!OnlineDate.Date.Equals(DateTime.Now.Date))
                    {
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();

                        MessageBox.Show("Please Correct Your System Date Time", "Stop", MessageBoxButton.OK, MessageBoxImage.Error);
                        Environment.Exit(0);
                    }
                }


                if (MainWindow.ins.expiry_instant == "Y")
                {                    
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                    MessageBox.Show(MainWindow.ins.expiry_message, "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                    Environment.Exit(0);
                }
                else
                {
                    if (DateTime.Now.Date >= MainWindow.ins.expiry_date)
                    {                       

                        LicenseDAL dal = new LicenseDAL();
                        institute obj = new institute();
                        obj.expiry_date = MainWindow.ins.expiry_date;
                        obj.expiry_message = MainWindow.ins.expiry_message;
                        obj.expiry_warning_day = MainWindow.ins.expiry_warning_day;
                        obj.expiry_warning_message = MainWindow.ins.expiry_warning_message;
                        obj.expiry_instant = "Y";
                        dal.update_sms_institute_local(obj);

                        MainWindow mw = new MainWindow();
                        mw.Show();
                        this.Close();
                        MessageBox.Show(MainWindow.ins.expiry_message, "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                        Environment.Exit(0);
                    }
                    else if (DateTime.Now.Date >= MainWindow.ins.expiry_date.AddDays(-MainWindow.ins.expiry_warning_day) && DateTime.Now.Date <= MainWindow.ins.expiry_date)
                    {
                        MessageBox.Show(MainWindow.ins.expiry_warning_message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                       //Run and enjoy software
                    }
                }
            }
            catch (Exception ex)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
                MessageBox.Show(ex.Message, "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                Environment.Exit(0);
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            apply_license();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (MainWindow.CheckForInternetConnection())
                {
                    IsInternetConnection = true;                    

                    LicenseDAL dal = new LicenseDAL();
                    try
                    {
                        try
                        {
                            OnlineDate = MainWindow.GetNistTime();
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Failed To get online date ex: " + ex.Message);
                            OnlineDate = DateTime.Now;
                            //throw ex;
                        }

                        dal.inser_login_log_OnlineDB();
                        institute ins = dal.get_expiry_OnlineDB();
                        if (ins.check)
                        {
                            dal.update_sms_institute_local(ins);
                        }
                        else
                        {
                            institute obj = new institute();
                            obj.expiry_date = DateTime.Now;
                            obj.expiry_message = MainWindow.ins.expiry_message;
                            obj.expiry_warning_day = MainWindow.ins.expiry_warning_day;
                            obj.expiry_warning_message = MainWindow.ins.expiry_warning_message;
                            obj.expiry_instant = "Y";
                            dal.update_sms_institute_local(obj);
                            //MessageBox.Show("Failed To Get Intitute Information Online");                            
                        }                        
                    }
                    catch (Exception ex)
                    {
                        institute obj = new institute();
                        obj.expiry_date = DateTime.Now;
                        obj.expiry_message = MainWindow.ins.expiry_message;
                        obj.expiry_warning_day = MainWindow.ins.expiry_warning_day;
                        obj.expiry_warning_message = MainWindow.ins.expiry_warning_message;
                        obj.expiry_instant = "Y";
                        dal.update_sms_institute_local(obj);

                        //MessageBox.Show("InternetConnection=true ex: "+ex.Message);
                        throw ex;
                    }
                }
                else
                {
                    //apply_license();
                }
            }
            catch (Exception ex)
            {
                //MainWindow mw = new MainWindow();
                //mw.Show();
                //this.Close();
                //MessageBox.Show(ex.Message, "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                //Environment.Exit(0);
            }
        }

        
    }
}
