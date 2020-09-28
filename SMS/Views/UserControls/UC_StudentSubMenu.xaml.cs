using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMS.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UC_StudentSubMenu.xaml
    /// </summary>
    public partial class UC_StudentSubMenu : UserControl
    {
        public UC_StudentSubMenu()
        {
            InitializeComponent();
        }

        private void icoBack_Click(object sender, RoutedEventArgs e)
        {
            grdParent.Children.Clear();
            grdParent.Children.Add(new UC_Dashboard());
        }
        private void resetHoveringStyle(int index)
        {
            try
            {
                var solidColorBursh = new SolidColorBrush(Color.FromRgb(0, 195, 179));
                var fontStyle = (Style)FindResource("configurationNoramlTxt");

                switch (index)
                {
                    case 0:
                        bdAdmission.Background = solidColorBursh;
                        txtAdmission.Style = fontStyle;
                        break;
                    case 1:
                        bdWithdrawl.Background = solidColorBursh;
                        txtWithdraw.Style = fontStyle;
                        break;
                    case 2:
                        bdPromote.Background = solidColorBursh;
                        txtPromote.Style = fontStyle;
                        break;
                    case 3:
                        bdEnvelopes.Background = solidColorBursh;
                        txtEnvelopes.Style = fontStyle;
                        break;
                    case 4:
                        bdClasses.Background = solidColorBursh;
                        txtClasses.Style = fontStyle;
                        break;
                    case 5:
                        bdSections.Background = solidColorBursh;
                        txtSections.Style = fontStyle;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //show error
            }
        }
        private void changeHoveringEffect(int index)
        {
            try
            {
                var solidColorBursh = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                var fontStyle = (Style)FindResource("configurationActiveTxt");
                switch (index)
                {
                    case 0:
                        bdAdmission.Background = solidColorBursh;
                        txtAdmission.Style = fontStyle;
                        break;
                    case 1:
                        bdWithdrawl.Background = solidColorBursh;
                        txtWithdraw.Style = fontStyle;
                        break;
                    case 2:
                        bdPromote.Background = solidColorBursh;
                        txtPromote.Style = fontStyle;
                        break;
                    case 3:
                        bdEnvelopes.Background = solidColorBursh;
                        txtEnvelopes.Style = fontStyle;
                        break;
                    case 4:
                        bdClasses.Background = solidColorBursh;
                        txtClasses.Style = fontStyle;
                        break;
                    case 5:
                        bdSections.Background = solidColorBursh;
                        txtSections.Style = fontStyle;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //show error
            }
        }
        private void spController_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdParent.Children.Clear();
            grdParent.Children.Add(new AdmissionManagement.Admission.AdmissionSearchNew());
        }

        private void spController_MouseEnter(object sender, MouseEventArgs e)
        {
            changeHoveringEffect(0);
        }

        private void spController_MouseLeave(object sender, MouseEventArgs e)
        {
            resetHoveringStyle(0);
        }      

        private void spWithdrawl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdParent.Children.Clear();
            grdParent.Children.Add(new AdmissionManagement.WithdrawAdmission.WithdrawAdmissionPage());
        }

        private void spWithdrawl_MouseEnter(object sender, MouseEventArgs e)
        {
            changeHoveringEffect(1);
        }

        private void spWithdrawl_MouseLeave(object sender, MouseEventArgs e)
        {
            resetHoveringStyle(1);
        }

        private void spPromote_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdParent.Children.Clear();
            grdParent.Children.Add(new AdmissionManagement.PromoteStudents.PromoteStudentPage());
        }

        private void spPromote_MouseEnter(object sender, MouseEventArgs e)
        {
            changeHoveringEffect(2);
        }

        private void spPromote_MouseLeave(object sender, MouseEventArgs e)
        {
            resetHoveringStyle(2);
        }

        private void spEnvelopes_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdParent.Children.Clear();
            grdParent.Children.Add(new AdmissionManagement.Envelope.EnvelopePage());
        }

        private void spEnvelopes_MouseEnter(object sender, MouseEventArgs e)
        {
            changeHoveringEffect(3);
        }

        private void spEnvelopes_MouseLeave(object sender, MouseEventArgs e)
        {
            resetHoveringStyle(3);
        }

        private void spClasses_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdParent.Children.Clear();
            grdParent.Children.Add(new ClassManagement.Class.ClassSearch());
        }

        private void spClasses_MouseEnter(object sender, MouseEventArgs e)
        {
            changeHoveringEffect(4);
        }

        private void spClasses_MouseLeave(object sender, MouseEventArgs e)
        {
            resetHoveringStyle(4);
        }

        private void spSections_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdParent.Children.Clear();
            grdParent.Children.Add(new ClassManagement.Section.SectionSearch() );
        }

        private void spSections_MouseEnter(object sender, MouseEventArgs e)
        {
            changeHoveringEffect(5);
        }

        private void spSections_MouseLeave(object sender, MouseEventArgs e)
        {
            resetHoveringStyle(5);
        }

        private void spStudentAttendance_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void spStudentAttendance_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendance_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceHistory_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void spStudentAttendanceHistory_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceHistory_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceDailyReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void spStudentAttendanceDailyReport_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceDailyReport_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceSheet_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void spStudentAttendanceSheet_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceSheet_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceSheetReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void spStudentAttendanceSheetReport_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void spStudentAttendanceSheetReport_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void spComplaintRegister_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void spComplaintRegister_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void spComplaintRegister_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
