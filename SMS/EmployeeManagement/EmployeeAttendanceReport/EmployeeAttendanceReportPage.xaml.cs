using Microsoft.Reporting.WinForms;
using SMS.DAL;
using SMS.Models;
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

namespace SMS.EmployeeManagement.EmployeeAttendanceReport
{
    /// <summary>
    /// Interaction logic for EmployeeAttendanceReportPage.xaml
    /// </summary>
    public partial class EmployeeAttendanceReportPage : Page
    {
        EmployeesDAL empDAL;
        EmployeeAttendanceDAL attDAL;
        List<employees> emp_list;
        DateTime dt;

        public EmployeeAttendanceReportPage()
        {
            InitializeComponent();

            attDAL = new EmployeeAttendanceDAL();
            empDAL = new EmployeesDAL();      

            try
            {
                emp_list = empDAL.get_all_active_employees();
                adm_grid.ItemsSource = emp_list;                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }           

            DateTime _date = DateTime.Now;
            var firstDayOfMonth = new DateTime(_date.Year, _date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            date_picker_to.SelectedDate = firstDayOfMonth;
            date_picker_from.SelectedDate = lastDayOfMonth;
        }
        private void date_picker_from_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    dt = date_picker_from.SelectedDate.Value;
                    ShowAdmissionGrid();
                }
            }
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;

                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    dt = date_picker_from.SelectedDate.Value;
                    ShowAdmissionGrid();
                }
            }
        }

        private void generate_report_btn_Click(object sender, RoutedEventArgs e)
        {
            if (emp_list.Where(x => x.Checked == true).Count() > 0)
            {
                List<employee_attendence> lst = attDAL.GetStudentAttendanceByDate(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value, emp_list.Where(x => x.Checked == true).ToList());
                foreach (var item in lst)
                {

                }
                ReportDataSource att = new ReportDataSource();
                att.Name = "att";
                att.Value = lst;

                ReportDataSource ins = new ReportDataSource();
                List<institute> ins_list = new List<institute>();
                MainWindow.ins.sDate = date_picker_to.SelectedDate.Value;
                MainWindow.ins.eDate = date_picker_from.SelectedDate.Value;
                MainWindow.ins.page_no = 1;
                ins_list.Add(MainWindow.ins);
                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                ins.Value = ins_list;

                this._reportViewer1.LocalReport.DataSources.Clear();
                this._reportViewer1.LocalReport.DataSources.Add(att);
                this._reportViewer1.LocalReport.DataSources.Add(ins);
                this._reportViewer1.LocalReport.ReportEmbeddedResource = "SMS.EmployeeManagement.EmployeeAttendanceReport.EmployeeAttendanceReport.rdlc";

                _reportViewer1.RefreshReport();
                ShowAttendanceReport();
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            employees  adm_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < adm_grid.Items.Count; i++)
            {
                adm_obj = (employees)adm_grid.Items[i];
                adm_obj.Checked = checkBox.IsChecked.Value;
            }
            adm_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            adm_grid.SelectedItem = e.Source;
            employees obj = new employees();
            obj = (employees)adm_grid.SelectedItem;
            foreach (employees item in emp_list)
            {
                if (obj.id == item.id)
                {
                    item.Checked = checkBox.IsChecked.Value;
                }
            }
        }
        void ShowAdmissionGrid()
        {
            windowsFormsHost1.Visibility = Visibility.Collapsed;
            adm_grid.Visibility = Visibility.Visible;
        }
        void ShowAttendanceReport()
        {
            windowsFormsHost1.Visibility = Visibility.Visible;
            adm_grid.Visibility = Visibility.Collapsed;
        }
    }
}
