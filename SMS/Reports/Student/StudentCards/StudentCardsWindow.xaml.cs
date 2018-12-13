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
using SMS.Models;
using SMS.AdmissionManagement.Admission;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.AdmissionManagement;
using SMS.AdmissionManagement.BulkFeeUpdate;
using System.ComponentModel;
using SMS.DAL;
using Microsoft.Reporting.WinForms;

namespace SMS.Reports.Student.StudentCards

{
    /// <summary>
    /// Interaction logic for StudentCardsWindow.xaml
    /// </summary>
    public partial class StudentCardsWindow : Window
    {
        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;
        public bool check = false;

        ClassesDAL classesDAL;
        AdmissionDAL admDAL;

        public StudentCardsWindow()
        {
            InitializeComponent();

            classesDAL = new ClassesDAL();
            admDAL = new AdmissionDAL();

            try
            {
                SearchTextBox.Focus();
                classes_list = classesDAL.get_all_classes();
                class_cmb.SelectedIndex = 0;
                classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
                class_cmb.ItemsSource = classes_list;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            load_grid();
        }

        public void load_grid()
        {
            classesDAL = new ClassesDAL();
            admDAL = new AdmissionDAL();

            try
            {
                adm_list = admDAL.get_all_admissions();
                adm_grid.ItemsSource = adm_list;
                this.adm_grid.Items.Refresh();
                class_cmb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
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
        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }
        

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            if (search_cmb.SelectedIndex == 0)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.std_name.ToUpper().Contains(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 1)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.father_name.ToUpper().Contains(v_search.ToUpper()) || x.father_name.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 2)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.adm_no.ToUpper().Contains(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 3)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 4)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.cell_no.ToUpper().Contains(v_search.ToUpper()) || x.cell_no.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else
            {
            }
            SearchTextBox.Focus();
        }       

        //============      Classes Selection Change       ===============================
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {


                sections_list = classesDAL.get_all_sections(id);
                adm_grid.ItemsSource = adm_list.Where(x => x.class_id == id);
                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
            }
            else
            {

                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
                adm_grid.ItemsSource = adm_list;
            }
            report_viewer.Visibility = Visibility.Collapsed;
        }

        
        //=======          Sections Selection Changed        ======================================================
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sections sec = (sections)section_cmb.SelectedItem;
            if (sec != null)
            {
                string id = sec.id;
                if (section_cmb.SelectedIndex != 0)
                {
                    adm_grid.ItemsSource = adm_list.Where(x => x.section_id == id);
                }
                else
                {
                    //adm_grid.ItemsSource = null;
                }
                report_viewer.Visibility = Visibility.Collapsed;
            }
        }

        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            check = false;
            foreach (admission adm in adm_list.Where(x => x.Checked == true))
            {
                check = true;
                break;
            }
            if (check)
            {
                foreach (var item in adm_list)
                {
                    item.institute_name = MainWindow.ins.institute_name;
                    item.institute_logo = MainWindow.ins.institute_logo;
                }

                ReportDataSource adm = new ReportDataSource();
                adm.Name = "adm";
                adm.Value = adm_list.Where(x=>x.Checked == true);

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
                this._reportViewer1.LocalReport.ReportEmbeddedResource = "SMS.Reports.Student.StudentCards.studentCardsReport.rdlc";

                _reportViewer1.RefreshReport();

                report_viewer.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student To Update Fee", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }



        private void adm_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
