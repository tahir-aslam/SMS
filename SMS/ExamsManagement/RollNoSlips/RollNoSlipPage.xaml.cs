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
using Microsoft.Reporting.WinForms;

namespace SMS.ExamsManagement.RollNoSlips
{
    /// <summary>
    /// Interaction logic for RollNoSlipPage.xaml
    /// </summary>
    public partial class RollNoSlipPage : Page
    {
        List<exam> exam_list;
        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;
        public bool check = false;

        AdmissionDAL admDAL;
        ClassesDAL classesDAL;
        ExamsDAL examsDAL;

        public RollNoSlipPage()
        {
            InitializeComponent();

            admDAL = new AdmissionDAL();
            classesDAL = new ClassesDAL();
            examsDAL = new ExamsDAL();

            //SearchTextBox.Focus();

            classes_list = classesDAL.getAllClasses();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            exam_list = examsDAL.get_all_exams();
            exam_cmb.Focus();
            exam_cmb.ItemsSource = exam_list;
            exam_list.Insert(0, new exam() { exam_name = "---Select Exam---", id = "-1" });
            exam_cmb.SelectedIndex = 0;

            load_grid();
        }

        public void load_grid()
        {
            adm_list = admDAL.get_all_admissions();
            adm_grid.ItemsSource = adm_list;
            this.adm_grid.Items.Refresh();
            class_cmb.SelectedIndex = 0;
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
            //SearchTextBox.Focus();
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
                adm_grid.ItemsSource = adm_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 1)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.father_name.ToUpper().StartsWith(v_search.ToUpper()) || x.father_name.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 2)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
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
                adm_grid.ItemsSource = adm_list.Where(x => x.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || x.cell_no.ToUpper().Contains(v_search.ToUpper()));
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
                sections_list = classesDAL.get_all_sections(c.id);
                
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
                    studentGrid.Visibility = Visibility.Visible;
                    envelopeGrid.Visibility = Visibility.Hidden;
                }
                else
                {
                    classes c = (classes)class_cmb.SelectedItem;
                    adm_grid.ItemsSource = adm_list.Where(x => x.class_id == c.id);
                    studentGrid.Visibility = Visibility.Visible;
                    envelopeGrid.Visibility = Visibility.Hidden;
                }
            }
        }
        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            List<admission> roll_no_slip_list;
            admission adm_slip;
            if (exam_cmb.SelectedIndex != 0)
            {                
                check = false;
                foreach (admission adm in adm_list.Where(x => x.Checked == true))
                {
                    check = true;
                    break;
                }
                if (check)
                {
                    exam ex = exam_cmb.SelectedItem as exam;
                    List<sms_exams_date_sheet> date_sheet_list = examsDAL.get_all_date_sheet();
                    roll_no_slip_list = new List<admission>();
                    foreach (var adm in adm_list.Where(x=>x.Checked == true))
                    {
                        foreach (var datesheet in date_sheet_list.Where(x=>x.exam_id.ToString() == ex.id).Where(x=>x.section_id.ToString() == adm.section_id))
                        {
                            adm_slip = new admission();
                            adm_slip.id = adm.id;
                            adm_slip.std_name = adm.std_name;
                            adm_slip.father_name = adm.father_name;
                            adm_slip.class_name = adm.class_name;
                            adm_slip.section_name = adm.section_name;
                            adm_slip.cell_no = adm.cell_no;
                            adm_slip.parmanent_adress = adm.parmanent_adress;
                            adm_slip.std_image = adm.std_image;
                            adm_slip.adm_no = adm.adm_no;
                            adm_slip.roll_no = adm.roll_no;

                            adm_slip.exam_name = datesheet.exam_name;
                            adm_slip.exam_date = datesheet.exam_date;
                            adm_slip.exam_time = datesheet.exam_time;
                            adm_slip.exam_remarks = datesheet.remarks;
                            adm_slip.subject_name = datesheet.subject_name;

                            adm_slip.institute_logo = MainWindow.ins.institute_logo;
                            adm_slip.institute_name = MainWindow.ins.institute_name;

                            roll_no_slip_list.Add(adm_slip);
                        }
                    }
                    
                    //MessageBox.Show("Printing............");
                    studentGrid.Visibility = Visibility.Hidden;
                    envelopeGrid.Visibility = Visibility.Visible;
                    loadReport(roll_no_slip_list);
                    // Form1 form = new Form1(adm_list.Where(x => x.Checked == true).ToList());
                    //form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please Select Minimum One Student", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            else
            {
                MessageBox.Show("Please Select Exam", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        private void adm_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {            
        }
        private void exam_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            exam exam = (exam)exam_cmb.SelectedItem;
            if (exam_cmb.SelectedItem != null && adm_list != null)
            {
                foreach (var item in adm_list.Where(x => x.Checked == true))
                {
                    item.exam_fee = exam.exam_name;
                }
            }
        }
        void loadReport(List<admission> adm_list)
        {
            if (adm_list.Count > 0)
            {
                ReportDataSource adm = new ReportDataSource()
                {
                    Name = "adm",
                    Value = adm_list
                };
                
                this._reportViewer3.LocalReport.DataSources.Clear();
                this._reportViewer3.LocalReport.DataSources.Add(adm);
                
                this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.ExamsManagement.RollNoSlips.RollNoSlipReport.rdlc";
                //this._reportViewer3.LocalReport.ReportEmbeddedResource = "bin.ExamsManagement.GeneralAwardList.ExamGeneralAwardListReport.rdlc";

                _reportViewer3.RefreshReport();
            }
        }
    }
}
