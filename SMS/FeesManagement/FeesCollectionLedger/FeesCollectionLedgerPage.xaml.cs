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
using System.ComponentModel;
using System.Globalization;

namespace SMS.FeesManagement.FeesCollectionLedger
{
    /// <summary>
    /// Interaction logic for FeesCollectionLedgerPage.xaml
    /// </summary>
    public partial class FeesCollectionLedgerPage : Page
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

        public FeesCollectionLedgerPage()
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
            
            months_list.Insert(0, new sms_months() {id = "-1", month_name = "--All Months--" });
            month_cmb.ItemsSource = months_list;

            years_list.Insert(0, new sms_years() {id =-1, year="--All Years--" });
            year_cmb.ItemsSource = years_list;

            fees_category_list.Insert(0, new sms_fees_category() {id = -1, fees_category= "--All Category--" });
            fees_category_cmb.ItemsSource = fees_category_list;        

            fees_collection_place_list.Insert(0,new sms_fees_collection_place(){ id =-1, place ="-All Places-"});
            place_cmb.ItemsSource = fees_collection_place_list;

            emp_login_list.Insert(0, new emp_login() { id ="-1", emp_user_name = "-All Employees-" });
            employees_cmb.ItemsSource = emp_login_list;

            loadGrid();
        }

        public void loadGrid() 
        {
            SearchTextBox.Focus();                       
            date_picker_to.SelectedDate = DateTime.Now;
            date_picker_from.SelectedDate = DateTime.Now;
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
                    paid_fee_grid.ItemsSource = feesCollectionList;
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
                    paid_fee_grid.ItemsSource = feesCollectionList;
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
            
            total_fee_paid_tb.Text = total_amount.ToString("C", CultureInfo.CreateSpecificCulture("ur-PKR"));
            discount_tb.Text = total_discount.ToString("C", CultureInfo.CreateSpecificCulture("ur-PKR"));
            waveOff_tb.Text = total_waveoff.ToString("C", CultureInfo.CreateSpecificCulture("ur-PKR"));
            count_text.Text = paid_fee_grid.Items.OfType<sms_fees>().Select(x=>x.receipt_no_full).Distinct().Count().ToString();
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
            SearchTextBox.Focus();
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
            bool check;
            //classes cl = (classes)class_cmb.SelectedItem;
            foreach (var item in classes_list.Where(x=>x.IsChecked == true).Where(x=>x.id != "-1"))
            {
                if (f.class_id.ToString() == item.id)
                {
                    return true;
                }
            }
            return false;
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

        private void print_button_Click(object sender, RoutedEventArgs e)
        {
            print(false); // isgrouped = false;
        }

        private void print_group_button_Click(object sender, RoutedEventArgs e)
        {
            print(true); // isgrouped = true;
        }

        void print(bool isGrouped, bool isClassGrouped = false) 
        {
            if (feesCollectionList.Count > 0)
            {
                List<sms_fees> fees_list = new List<sms_fees>();

                int sort_order = 0;
                string r_classes = "All";
                string r_sections = "All";
                string r_fees_Category = "All";
                string r_users = "All";
                string r_collection_place = "All";
                string r_months = "All";
                string r_years = "All";

                string r_total_receipts = "0";
                string r_toReceipt = "00000";
                string r_fromReceipts = "00000";

                if (class_cmb.SelectedIndex != 0)
                {
                    classes cl = (classes)class_cmb.SelectedItem;
                    r_classes = cl.class_name;
                    sort_order = Convert.ToInt32(cl.reg_fee);

                    if (section_cmb.SelectedIndex != 0)
                    {
                        sections sec = (sections)section_cmb.SelectedItem;
                        r_sections = sec.section_name;
                    }
                }



                if (fees_category_cmb.SelectedIndex != 0)
                {
                    sms_fees_category category = (sms_fees_category)fees_category_cmb.SelectedItem;
                    r_fees_Category = category.fees_category;
                }

                if (employees_cmb.SelectedIndex != 0)
                {
                    emp_login emp = (emp_login)employees_cmb.SelectedItem;
                    r_users = emp.emp_user_name;
                }

                if (place_cmb.SelectedIndex != 0)
                {
                    sms_fees_collection_place place = (sms_fees_collection_place)place_cmb.SelectedItem;
                    r_collection_place = place.place;
                }

                if (month_cmb.SelectedIndex != 0)
                {
                    sms_months month = (sms_months)month_cmb.SelectedItem;
                    r_months = month.month_name;
                }

                if (year_cmb.SelectedIndex != 0)
                {
                    sms_years year = (sms_years)year_cmb.SelectedItem;
                    r_years = year.year;
                }

                //total receipts
                r_total_receipts = feesCollectionList.Select(x => x.receipt_no).Distinct().Count().ToString();
                r_toReceipt = feesCollectionList[feesCollectionList.Count() - 1].receipt_no_full;
                r_fromReceipts = feesCollectionList.First().receipt_no_full;

                string month_name_group="";
                int tution_fee = 0;
                sms_fees obj;
                List<string> months_list;
                for (int i = 0; i < paid_fee_grid.Items.Count; i++)
                {                    
                    sms_fees item = paid_fee_grid.Items[i] as sms_fees;
                    item.toDate = date_picker_to.SelectedDate.Value;
                    item.fromDate = date_picker_from.SelectedDate.Value;

                    item.actual_tution_fee = item.actual_amount;
                    item.r_standard_discount = item.Std_discount_tution_fee.ToString();

                    //if (item.fees_category_id == 113)
                    //{
                    //    item.actual_tution_fee = item.actual_amount;
                    //    item.Std_discount_tution_fee = item.Std_discount_tution_fee;
                    //}
                    //else 
                    //{
                        
                    //}
                    
                    months_list = new List<string>();
                    month_name_group = "";
                    foreach (var fee in paid_fee_grid.Items)
                    {                       
                        obj=  fee as sms_fees;
                        if(obj.receipt_no_full == item.receipt_no_full)
                        {                            
                            months_list.Add(obj.month_name);
                        }
                    }
                    foreach (var month in months_list.Distinct())
                    {
                        month_name_group = month_name_group +" " +month;
                    }
                    item.month_name_group = month_name_group;

                    item.r_classes = r_classes;
                    try
                    {
                        item.session_id = Convert.ToInt32(classes_list.Where(x => x.id == item.class_id.ToString()).First().reg_fee);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.s
                    }
                    item.r_sections = r_sections;
                    item.r_fees_Category = r_fees_Category;
                    item.r_users = r_users;
                    item.r_collection_place = r_collection_place;
                    item.r_months = r_months;
                    item.r_years = r_years;
                    item.r_total_receipts = r_total_receipts;
                    item.r_toReceipt = r_toReceipt;
                    item.r_fromReceipts = r_fromReceipts;

                    fees_list.Add(item);
                }

                FeesCollectionLedgerReportWindow window = new FeesCollectionLedgerReportWindow(fees_list, isGrouped, isClassGrouped);
                    window.Show();
               
            }
            else
            {
                MessageBox.Show("No Record Found");
            }
        }

        private void CheckBox_Checked_fees(object sender, RoutedEventArgs e)
        {
            int count = 0;
            List<sms_fees> list = new List<sms_fees>();
            foreach (sms_fees_category fee in fees_category_list.Where(x => x.isChecked == true))
            {
                foreach (var item in feesCollectionList.Where(x => x.fees_category_id == fee.id))
                {
                    list.Add(item);
                    count++;
                }
            }
            if (fees_category_list.Where(x => x.isChecked == true).Count() == 0)
            {
                paid_fee_grid.ItemsSource = feesCollectionList;
                paid_fee_grid.Items.Refresh();
            }
            else
            {
                paid_fee_grid.ItemsSource = list;
                paid_fee_grid.Items.Refresh();
            }
            calculate_amount();
        }

        private void CheckBox_Checked_classes(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            classes classes = checkBox.DataContext as classes;
            if (classes.id == "-1")
            {
                if (checkBox.IsChecked == true)
                {
                    classes_list.ForEach(x => x.IsChecked = true);
                }
                else
                {
                    classes_list.ForEach(x => x.IsChecked = false);
                }
            }

            //for section cmb
            if (classes_list.Where(x => x.id != "-1").Where(x => x.IsChecked == true).Count() > 1)
            {
                section_cmb.IsEnabled = false;
            }
            else if (classes.id != "-1")
            {
                sections_list = classDAL.get_all_sections(classes.id);
                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
            }
            filter();
        }

        private void print_without_group_button__Click(object sender, RoutedEventArgs e)
        {
            print(true); // isgrouped = true;
        }

        private void Print_withoutclassgroup_button_Click(object sender, RoutedEventArgs e)
        {
            print(true, true); // isgrouped = true;
        }
    }
}
