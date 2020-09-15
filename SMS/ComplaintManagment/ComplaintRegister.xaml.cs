using MySql.Data.MySqlClient;
using SMS.DAL;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SMS.ComplaintManagment
{
    /// <summary>
    /// Interaction logic for ComplaintRegister.xaml
    /// </summary>
    public partial class ComplaintRegister : Page
    {
        List<sms_complaint_register> complaints_list;
        ComplaintForm CF;
        string mode = "";
        sms_complaint_register obj;
        List<classes> classes_list;
        List<sections> sections_list;
        ComplaintDAL ComplaintDAL;

        public ComplaintRegister()
        {
            InitializeComponent();

            //SearchTextBox.Focus();
            classes_list = new List<classes>();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            DateTime _date = DateTime.Now;
            var firstDayOfMonth = new DateTime(_date.Year, _date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            date_picker_to.SelectedDate = firstDayOfMonth;
            date_picker_from.SelectedDate = lastDayOfMonth;

            load_grid();
        }
        public void load_grid()
        {
            ComplaintDAL = new ComplaintDAL();
            complaints_list = new List<sms_complaint_register>();
            complaints_list = ComplaintDAL.getAllComplaintsRegiter(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value).OrderByDescending(x=>x.complaint_date).ToList();
            v_ComplaintGrid.ItemsSource = complaints_list;
            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;
        }        
        private void date_picker_from_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    load_grid();
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
                    load_grid();
                }
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
            class_cmb.SelectedIndex = 0;
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            try
            {
                mode = "insert";
                obj = null;
                CF = new ComplaintForm(mode, this, obj);
                CF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                CF.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //-------------     Editing          ---------------------------
        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void adm_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            obj = (sms_complaint_register)v_ComplaintGrid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                CF = new ComplaintForm(mode, this, obj);
                CF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                CF.ShowDialog();
            }
        }

        //-------------     Delete          ---------------------------      
        private void click_delete(object sender, RoutedEventArgs e)
        {
            obj = (sms_complaint_register)v_ComplaintGrid.SelectedItem;
            obj.updated_by = MainWindow.emp_login_obj.emp_user_name;
            obj.updated_date_time = DateTime.Now;
            obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.id);
            if (obj == null)
            {
                MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    if (ComplaintDAL.deleteComplaintRegister(obj) > 0)
                    {
                        MessageBox.Show("Successfully Deleted");
                        load_grid();
                    }
                }
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
            //strength_textblock.Text = adm_grid.Items.Count.ToString();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(v_ComplaintGrid.ItemsSource);
            if (v_search == null)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = o =>
                {
                    sms_complaint_register x = o as sms_complaint_register;
                    if (search_cmb.SelectedIndex == 0)
                    {
                        return (x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 1)
                    {
                        return (x.father_name.ToUpper().StartsWith(v_search.ToUpper()) || x.father_name.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 2)
                    {
                        return (x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 3)
                    {
                        return (x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 4)
                    {
                        return (x.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || x.cell_no.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 5)
                    {
                        return (x.id.ToString().ToUpper().StartsWith(v_search.ToUpper()) || x.id.ToString().ToUpper().Contains(v_search.ToUpper()));
                    }
                    else
                    {
                        return true;
                    }

                };
            }
            SearchTextBox.Focus();
        }

        //---------------           Get All Classes    ----------------------------------
        public void get_all_classes()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            };
                            classes_list.Add(classes);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //============      Classes Selection Change       ===============================
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {
                get_all_sections(id);
                v_ComplaintGrid.ItemsSource = complaints_list.Where(x => x.class_id == id);
                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
            }
            else
            {

                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
                v_ComplaintGrid.ItemsSource = complaints_list;
            }
        }

        //------------         Get All Sections   ------------------------      
        public void get_all_sections(string id)
        {
            sections_list = new List<sections>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects where class_id = " + id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),


                            };
                            sections_list.Add(section);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("DB not connected");
                    }

                }
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
                    v_ComplaintGrid.ItemsSource = complaints_list.Where(x => x.section_id == id);
                }
                else
                {
                    // adm_grid.ItemsSource = null;
                }
            }
        }

        // ================        Printing         ========================
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            //AdmissionReportWF report = new AdmissionReportWF(adm_list);
            //report.ShowDialog();

            //if (class_cmb.SelectedIndex > 0)
            //{
            //    classes c = (classes)class_cmb.SelectedItem;
            //    class_name = c.class_name;
            //}
            //else
            //{
            //    class_name = "";
            //}

            //if (section_cmb.SelectedIndex > 0)
            //{
            //    sections sec = (sections)section_cmb.SelectedItem;
            //    section_name = sec.section_name;
            //}
            //else
            //{
            //    section_name = "";
            //}

            //get_grid_records();
            //var dataTable = CreateSampleDataTable();
            //var columnWidths = new List<double>() { 50, 140, 130, 60, 60, 60, 60, 90, 90, 90, 140 };
            //var ht = new SMS.PrintHeaderTemplates.AdmissionRegisterHeader();
            //var headerTemplate = XamlWriter.Save(ht);
            //var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            //printControl.ShowPrintPreview();

            //sms_report report_data = new sms_report();
            //report_data.total_strength = adm_grid.Items.OfType<admission>().Select(x => x.id).Distinct().Count();
            //report_data.male_strength = adm_grid.Items.OfType<admission>().Where(x => x.boarding == "Y").Count();
            //report_data.female_strength = adm_grid.Items.OfType<admission>().Where(x => x.boarding == "N").Count();
            //report_data.session = MainWindow.session.session_name;

            //AdmissionRegisterReportWindow window = new AdmissionRegisterReportWindow(adm_grid.Items.OfType<admission>().ToList(), report_data);
            //window.Show();

        }


        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchTextBox.Focus();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(v_ComplaintGrid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            //strength_textblock.Text = adm_grid.Items.Count.ToString();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchTextBox.IsFocused == true)
                {
                    v_ComplaintGrid.SelectedIndex = 0;
                    editing();
                }
            }
        }
       
        private void v_ComplaintGrid_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            editing();
        }  
    }
}
