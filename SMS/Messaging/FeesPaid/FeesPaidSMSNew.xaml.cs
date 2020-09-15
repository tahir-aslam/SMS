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
using SMS.Upload;
using SMS.Messaging.SmsOption;
using SMS.Messaging.BrandedSms;
using System.ComponentModel;
using SMS.DAL;

namespace SMS.Messaging.FeesPaid
{
    /// <summary>
    /// Interaction logic for FeesPaidSMSNew.xaml
    /// </summary>
    public partial class FeesPaidSMSNew : Page
    {

        DateTime dt;
        int total_amount = 0;
        int total_discount = 0;
        int total_waveoff = 0;

        FeesDAL feesDAL;
        ClassesDAL classDAL;
        MiscDAL miscDAL;
        EmployeesDAL empDAL;

        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_months> months_list;
        List<sms_years> years_list;
        List<sms_fees_category> fees_category_list;
        List<sms_fees_sub_category> fees_sub_category_list;
        List<sms_fees_collection_place> fees_collection_place_list;
        List<emp_login> emp_login_list;
        List<sms_fees> feesCollectionList;
        List<sms_fees> paid_list_group;

        List<admission> std_nos_list;
        string message;
        admission adm_obj;
        public static bool isbranded = false;

        public FeesPaidSMSNew()
        {
            InitializeComponent();
            feesDAL = new FeesDAL();
            classDAL = new ClassesDAL();
            miscDAL = new MiscDAL();
            empDAL = new EmployeesDAL();

            try
            {
                classes_list = classDAL.get_all_classes();
                months_list = miscDAL.get_all_months();
                years_list = miscDAL.get_all_years();
                fees_category_list = feesDAL.get_all_fees_category();
                fees_collection_place_list = feesDAL.getAllFeesCollectionPlace();
                emp_login_list = empDAL.get_all_emp_login();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


            classes_list.Insert(0, new classes() { id = "-1", class_name = "--All Classes--" });
            class_cmb.ItemsSource = classes_list;

            months_list.Insert(0, new sms_months() { id = "-1", month_name = "--All Months--" });
            month_cmb.ItemsSource = months_list;

            years_list.Insert(0, new sms_years() { id = -1, year = "--All Years--" });
            year_cmb.ItemsSource = years_list;

            fees_category_list.Insert(0, new sms_fees_category() { id = -1, fees_category = "--All Category--" });
            fees_category_cmb.ItemsSource = fees_category_list;

            fees_collection_place_list.Insert(0, new sms_fees_collection_place() { id = -1, place = "-All Places-" });
            place_cmb.ItemsSource = fees_collection_place_list;

            emp_login_list.Insert(0, new emp_login() { id = "-1", emp_user_name = "-All Employees-" });
            employees_cmb.ItemsSource = emp_login_list;

            loadGrid();
    
        }

        public void loadGrid()
        {
            //SearchTextBox.Focus();
            date_picker_to.SelectedDate = DateTime.Now;
            date_picker_from.SelectedDate = DateTime.Now;
            default_btn.IsChecked = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sms_fees f;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < paid_fee_grid.Items.Count; i++)
            {
                f = (sms_fees)paid_fee_grid.Items[i];
                f.Checked = checkBox.IsChecked.Value;
            }
            paid_fee_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            paid_fee_grid.SelectedItem = e.Source;
            sms_fees f_obj = new sms_fees();
            f_obj = (sms_fees)paid_fee_grid.SelectedItem;
            foreach (sms_fees ede in paid_list_group)
            {
                if (ede.id == f_obj.id)
                {
                    f_obj.Checked = checkBox.IsChecked.Value;
                }
            }

        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (default_btn.IsChecked == true)
            {
                general_grid.Visibility = Visibility.Hidden;
                option_grid.Visibility = Visibility.Visible;
                withoutAmount_btn.IsChecked = true;
            }
            else
            {
                general_grid.Visibility = Visibility.Visible;
                option_grid.Visibility = Visibility.Hidden;
            }
        }
        private void message_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

            count_text.Text = (306 - message_textbox.Text.Length).ToString();
            if (message_textbox.Text.Length <= 160)
            {
                sms_no_tb.Text = "1";
            }
            else
            {
                sms_no_tb.Text = "2";
            }

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(paid_fee_grid, itemsSourceChanged);
            }
        }
        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = paid_fee_grid.Items.Count.ToString();
        }
        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            std_nos_list = new List<admission>();
            //int index = filter_def_cmb.SelectedIndex;
            foreach (sms_fees f in feesCollectionList.Where(x => x.Checked == true))
            {
                message = "";
                adm_obj = new admission();
                adm_obj.std_name = f.std_name;
                adm_obj.father_name = f.father_name;
                adm_obj.cell_no = f.cell_no;
                adm_obj.cell_no = f.cell_no;
                adm_obj.class_id = f.class_id.ToString();
                adm_obj.class_name = f.class_name;
                adm_obj.section_id = f.section_id.ToString();
                adm_obj.section_name = f.section_name;

                if (default_btn.IsChecked == true)
                {
                    if (withoutAmount_btn.IsChecked == true)
                    {
                        message = "Respected Parents, AoA, It is to inform you that " + f.std_name + "'s " + f.fees_category_group + " of month(s) " + f.month_name_group + " under Receipt No. " + f.receipt_no_group + " has been deposited to the Institute office. Thank you, Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                    }
                    else
                    {
                        message = "Respected Parents, AoA, It is to inform you that " + f.std_name + "'s " + f.fees_category_group + " of month(s) " + f.month_name_group + " Rs " + f.total_paid + "/ under Receipt No. " + f.receipt_no_group + " has been deposited to the Institute office. Thank you, Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                    }

                }
                else
                {
                    message = message_textbox.Text;
                }

                adm_obj.sms_message = message;
                adm_obj.sms_type = "Fees Paid";
                std_nos_list.Add(adm_obj);
            }
            MessageBoxResult mbr = MessageBox.Show("Do You Want To Send    " + std_nos_list.Count + "  SMS ?", "Send Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (mbr == MessageBoxResult.Yes)
            {
                if (std_nos_list.Count > 0)
                {
                    OptionWindow ow = new OptionWindow();
                    ow.ShowDialog();

                    if (isbranded == true)
                    {
                        BrandedSmsEngine bse = new BrandedSmsEngine(std_nos_list);
                        bse.Show();
                    }
                    else
                    {
                        UploadWindow uw = new UploadWindow(std_nos_list, false);
                        uw.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Minimum One Student");
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
                    feesCollectionList = feesDAL.getFeesPaidByDate(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    setFeesList(feesCollectionList);
                    calculate_amount();
                    clearAllFilters();
                    SearchTextBox.Text = "";
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
                    feesCollectionList = feesDAL.getFeesPaidByDate(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    setFeesList(feesCollectionList);
                    calculate_amount();
                    clearAllFilters();
                    SearchTextBox.Text = "";
                }
            }
        }

        public void calculate_amount()
        {
            sms_fees fees;
            total_amount = 0;
            total_discount = 0;
            total_waveoff = 0;

            for (int i = 0; i < paid_fee_grid.Items.Count; i++)
            {
                fees = (sms_fees)paid_fee_grid.Items[i];
                total_amount = total_amount + fees.amount_paid;
                total_discount = total_discount + fees.discount;
                total_waveoff = total_waveoff + fees.wave_off;
            }

           
            count_text.Text = paid_fee_grid.Items.OfType<sms_fees>().Select(x => x.receipt_no_full).Distinct().Count().ToString();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(paid_fee_grid.ItemsSource);
            if (v_search == null)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = o =>
                {
                    sms_fees f = o as sms_fees;

                    if (search_cmb.SelectedIndex == 0)
                    {
                        return (f.adm_no.ToUpper().StartsWith(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 1)
                    {
                        return (f.receipt_no_full.ToUpper().StartsWith(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 2)
                    {
                        return (f.receipt_no.ToString().StartsWith(v_search.ToUpper()));
                    }
                    if (search_cmb.SelectedIndex == 3)
                    {
                        return (f.std_name.ToUpper().StartsWith(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 4)
                    {
                        return (f.father_name.ToUpper().StartsWith(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 5)
                    {
                        return (f.cell_no.ToUpper().StartsWith(v_search.ToUpper()));
                    }
                    else
                    {
                        return true;
                    }
                };
                calculate_amount();
            }
            SearchTextBox.Focus();
        }



        private void click_refresh(object sender, RoutedEventArgs e)
        {
            loadGrid();
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchTextBox.Focus();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        //Filters

        private void fees_category_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fees_category_cmb.SelectedItem != null)
            {
                filter();
            }
        }
        private void year_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (year_cmb.SelectedItem != null)
            {
                filter();
            }
        }
        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (month_cmb.SelectedItem != null)
            {
                filter();
            }
        }
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb.SelectedItem != null)
            {
                classes cl = (classes)class_cmb.SelectedItem;

                if (class_cmb.SelectedIndex != 0)
                {

                    section_cmb.IsEnabled = true;
                    sections_list = new List<sections>();
                    sections_list = classDAL.get_all_sections(cl.id);
                    sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;
                }
                else
                {
                    sections_list = classDAL.get_all_sections(cl.id);
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;
                }
                filter();
            }
        }
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedItem != null)
            {
                filter();
            }
        }
        private void place_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (place_cmb.SelectedItem != null)
            {
                filter();
            }
        }
        private void employees_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (employees_cmb.SelectedItem != null)
            {
                filter();
            }
        }

        void filter()
        {
            SearchTextBox.Text = "";
            ICollectionView cv = CollectionViewSource.GetDefaultView(paid_fee_grid.ItemsSource);
            cv.Filter = o =>
            {
                sms_fees f = o as sms_fees;
                if (getMonth(f) && getYear(f) && getFeesCategory(f) && getClasses(f) && getSections(f) && getemployees(f) && getFeesCollectionPlace(f))
                {
                    return true;
                }
                return false;
            };
            calculate_amount();
        }

        bool getClasses(sms_fees f)
        {
            classes cl = (classes)class_cmb.SelectedItem;
            if (class_cmb.SelectedIndex > 0 && f.class_id.ToString() != cl.id)
            {
                return false;
            }
            return true;
        }
        bool getSections(sms_fees f)
        {
            sections sec = (sections)section_cmb.SelectedItem;
            if (section_cmb.SelectedIndex > 0 && f.section_id.ToString() != sec.id)
            {
                return false;
            }
            return true;
        }
        bool getMonth(sms_fees f)
        {
            sms_months month = (sms_months)month_cmb.SelectedItem;
            if (month_cmb.SelectedIndex > 0 && f.month.ToString() != month.month_id)
            {
                return false;
            }
            return true;
        }
        bool getYear(sms_fees f)
        {
            sms_years year = (sms_years)year_cmb.SelectedItem;
            if (year_cmb.SelectedIndex > 0 && f.year != year.id)
            {
                return false;
            }
            return true;
        }
        bool getFeesCategory(sms_fees f)
        {
            sms_fees_category category = (sms_fees_category)fees_category_cmb.SelectedItem;
            if (fees_category_cmb.SelectedIndex > 0 && f.fees_category_id != category.id)
            {
                return false;
            }
            return true;
        }
        bool getFeesCollectionPlace(sms_fees f)
        {
            sms_fees_collection_place place = (sms_fees_collection_place)place_cmb.SelectedItem;
            if (place_cmb.SelectedIndex > 0 && f.fees_collection_place_id != place.id)
            {
                return false;
            }
            return true;
        }
        bool getemployees(sms_fees f)
        {
            emp_login emp = (emp_login)employees_cmb.SelectedItem;
            if (employees_cmb.SelectedIndex > 0 && f.emp_id.ToString() != emp.emp_id)
            {
                return false;
            }
            return true;
        }


        void clearAllFilters()
        {
            month_cmb.SelectedIndex = 0;
            year_cmb.SelectedIndex = 0;
            fees_category_cmb.SelectedIndex = 0;
            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;
            place_cmb.SelectedIndex = 0;
            employees_cmb.SelectedIndex = 0;
        }

        private void clear_btn_Click(object sender, RoutedEventArgs e)
        {
            clearAllFilters();
        }

        void setFeesList(List<sms_fees> fees_list)
        {
            paid_list_group = new List<sms_fees>();
            List<string> months_list;
            List<string> fees_category_list;
            List<string> receipt_list;
            int total_paid = 0;
            string month_name_group = "";
            string fees_category_group = "";
            string receipt_group = "";

            foreach (var obj in fees_list.Select(x => x.std_id).Distinct())
            {
                total_paid = 0;
                months_list = new List<string>();
                fees_category_list = new List<string>();
                receipt_list = new List<string>();
                month_name_group = "";
                fees_category_group = "";
                receipt_group = "";

                foreach (var fees in fees_list.Where(x => x.std_id == obj))
                {
                    foreach (var fee in fees_list.Where(x => x.std_id == obj))
                    {
                        months_list.Add(fee.month_name);
                        fees_category_list.Add(fee.fees_category);
                        receipt_list.Add(fee.receipt_no_full);
                        total_paid = total_paid + fee.amount_paid;                        
                    }                   

                    fees.month_name_group = String.Join(", ",months_list.Distinct());
                    fees.fees_category_group = String.Join(", ",fees_category_list.Distinct());
                    fees.receipt_no_group = String.Join(", ", receipt_list.Distinct());

                    fees.total_paid = total_paid;
                    paid_list_group.Add(fees);
                    break;
                }
            }
            paid_fee_grid.ItemsSource = paid_list_group;
        }
    }
}
