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

        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;

        DateTime dt;

        public AttendanceReportClassWisePage()
        {
            InitializeComponent();

            classDAL = new ClassesDAL();
            admDAL = new AdmissionDAL();

            adm_list = new List<admission>();

            try
            {
                adm_list = admDAL.get_all_admissions();
                classes_list = classDAL.get_all_classes();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            classes_list.Insert(0, new classes() { id = "-1", class_name = "--All Class--" });
            class_cmb.ItemsSource = classes_list;
            class_cmb.SelectedIndex = 0;
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
                }
                else
                {
                }
            }
        }
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedItem != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    sec = (sections)section_cmb.SelectedItem;

                    ReportDataSource adm = new ReportDataSource();
                    adm.Name = "adm";
                    adm.Value = adm_list.Where(x => x.section_id == sec.id);

                    ReportDataSource ins = new ReportDataSource();
                    List<institute> ins_list = new List<institute>();
                    MainWindow.ins.date = DateTime.Now;
                    MainWindow.ins.page_no = 1;
                    ins_list.Add(MainWindow.ins);
                    ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                    ins.Value = ins_list;

                    this._reportViewer1.LocalReport.DataSources.Clear();
                    this._reportViewer1.LocalReport.DataSources.Add(adm);
                    this._reportViewer1.LocalReport.DataSources.Add(ins);
                    this._reportViewer1.LocalReport.ReportEmbeddedResource = "SMS.StudentManagement.AttendanceReportClassWise.StudentAttendanceReportClassWiseReport.rdlc";

                    _reportViewer1.RefreshReport();
                }
                else
                {

                }
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
                }
            }
        }

        private void generate_report_btn_Click(object sender, RoutedEventArgs e)
        {
            if(class_cmb.SelectedItem != null && section_cmb.SelectedItem != null)
            {

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
    }

}
