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
using SMS.DAL;
using System.Collections.ObjectModel;
using Microsoft.Reporting.WinForms;

namespace SMS.StudentManagement.AttendanceReportClassWise
{
    /// <summary>
    /// Interaction logic for AttendanceReportClassWisePage.xaml
    /// </summary>
    public partial class AttendanceReportClassWisePage : Page
    {
        sections sec;
        classes cl;
        ClassesDAL classDAL;
        AdmissionDAL admDAL;
        AttendanceDAL attDAL;

        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;

        DateTime dt;

        public AttendanceReportClassWisePage()
        {
            InitializeComponent();

            classDAL = new ClassesDAL();
            admDAL = new AdmissionDAL();
            attDAL = new AttendanceDAL();

            adm_list = new List<admission>();

            try
            {
                adm_list = admDAL.get_all_admissions();
                adm_grid.ItemsSource = adm_list;
                classes_list = classDAL.get_all_classes();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            classes_list.Insert(0, new classes() { id = "-1", class_name = "--All Class--" });
            class_cmb.ItemsSource = classes_list;
            class_cmb.SelectedIndex = 0;

            DateTime _date = DateTime.Now;
            var firstDayOfMonth = new DateTime(_date.Year, _date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            date_picker_to.SelectedDate = firstDayOfMonth;
            date_picker_from.SelectedDate = lastDayOfMonth;
        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb.SelectedItem != null)
            {
                cl = (classes)class_cmb.SelectedItem;
                if (class_cmb.SelectedIndex != 0)
                {
                    sections_list = new List<sections>();
                    sections_list = classDAL.get_all_sections(cl.id);
                    sections_list.Insert(0, new sections() { id = "-1", section_name = "--All Sections--" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;
                    section_cmb.IsEnabled = true;
                    adm_grid.ItemsSource = adm_list.Where(x => x.class_id == cl.id);
                }
                else
                {
                }
                ShowAdmissionGrid();
            }
        }
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedItem != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    sec = (sections)section_cmb.SelectedItem;
                    adm_grid.ItemsSource = adm_list.Where(x => x.section_id == sec.id);                    
                }
                else
                {

                }
                ShowAdmissionGrid();
            }
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
            if (adm_list.Where(x => x.Checked == true).Count() > 0)
            {
                List<student_attendence> lst = attDAL.GetStudentAttendanceByDate(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value, adm_list.Where(x => x.Checked == true).OrderBy(x=>x.adm_no_int).ToList());
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
                this._reportViewer1.LocalReport.ReportEmbeddedResource = "SMS.StudentManagement.AttendanceReportClassWise.StudentAttendanceReportClassWiseReport.rdlc";

                _reportViewer1.RefreshReport();
                ShowAttendanceReport();
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private bool validate() 
        {
            if (class_cmb.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Class","Error",MessageBoxButton.OK,MessageBoxImage.Warning);
                class_cmb.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            admission adm_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < adm_grid.Items.Count; i++)
            {
                adm_obj = (admission)adm_grid.Items[i];
                adm_obj.Checked = checkBox.IsChecked.Value;
            }
            adm_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            adm_grid.SelectedItem = e.Source;
            admission adm = new admission();
            adm = (admission)adm_grid.SelectedItem;
            foreach (admission ede in adm_list)
            {
                if (adm.id == ede.id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
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
