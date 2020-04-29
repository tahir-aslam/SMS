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
using SMS.DAL;
using SMS.Models;
using System.ComponentModel;
using System.Globalization;

namespace SMS.FeesManagement.FeesDefaulters
{
    /// <summary>
    /// Interaction logic for FeesDefaultersPage.xaml
    /// </summary>
    public partial class FeesDefaultersPage : Page
    {
        DateTime dt;
        int total_amount = 0;
        
        FeesDAL feesDAL;
        ClassesDAL classDAL;
        MiscDAL miscDAL;

        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_months> months_list;
        List<sms_years> years_list;
        List<sms_fees_category> fees_category_list;
        List<sms_fees_sub_category> fees_sub_category_list;
        List<sms_fees> fees_list;

        public FeesDefaultersPage()
        {
            InitializeComponent();

            feesDAL = new FeesDAL();
            classDAL = new ClassesDAL();
            miscDAL = new MiscDAL();

            try
            {
                classes_list = classDAL.get_all_classes();
                months_list = miscDAL.get_all_months();
                years_list = miscDAL.get_all_years();
                fees_category_list = feesDAL.get_all_fees_category();
                fees_sub_category_list = feesDAL.get_all_fees_sub_category();
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

            loadGrid();
        }

        public void loadGrid()
        {
            SearchTextBox.Focus();

            fees_list = feesDAL.getAllUnPaidFees();
            foreach (var item in fees_list)
            {
                sms_fees obj = feesDAL.getLastFeeReceived(item.std_id);
                item.last_fees_received = obj.date;
                item.last_receipt_no = obj.receipt_no_full;
                item.last_amount = obj.total_paid;
            }
            defaulter_fee_grid.ItemsSource = fees_list;
            calculate_amount();
            clearAllFilters();
        }

        public void calculate_amount()
        {
            sms_fees fees;
            total_amount = 0;

            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fees = (sms_fees)defaulter_fee_grid.Items[i];
                total_amount = total_amount + fees.rem_amount;                
            }
            total_fee_tb.Text = total_amount.ToString("C", CultureInfo.CreateSpecificCulture("ur-PKR"));            
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(defaulter_fee_grid.ItemsSource);
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
                clearAllFilters();
            }
            SearchTextBox.Focus();
        }        

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            loadGrid();
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
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
        private void defaulter_type_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (defaulter_type_cmb.SelectedItem != null)
            {
                filter();
            }
        }

        void filter()
        {
            SearchTextBox.Text = "";
            ICollectionView cv = CollectionViewSource.GetDefaultView(defaulter_fee_grid.ItemsSource);
            cv.Filter = o =>
            {
                sms_fees f = o as sms_fees;
                if (getMonth(f) && getYear(f) && getFeesCategory(f) && getClasses(f) && getSections(f) && getDefaulterType(f))
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
        bool getDefaulterType(sms_fees f)
        {
            if (defaulter_type_cmb.SelectedIndex == 1 && f.adm_is_active == "N")
            {
                return true;
            }
            else if (defaulter_type_cmb.SelectedIndex == 2 && f.adm_is_active == "Y")
            {
                return true;
            }
            else if(defaulter_type_cmb.SelectedIndex == 0)
            {
                return true;
            }
            return false;
        }

        void clearAllFilters()
        {
            month_cmb.SelectedIndex = 0;
            year_cmb.SelectedIndex = 0;
            fees_category_cmb.SelectedIndex = 0;
            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;
            defaulter_type_cmb.SelectedIndex = 0;  
        }

        private void clear_btn_Click(object sender, RoutedEventArgs e)
        {
            clearAllFilters();
        }

        private void print_button_Click(object sender, RoutedEventArgs e)
        {
            print(false);
        }
        private void print_group_button_Click(object sender, RoutedEventArgs e)
        {
            print(true);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int count = 0;
            List<sms_fees> list = new List<sms_fees>();
            foreach (sms_months month in months_list.Where(x=>x.isChecked == true))
            {                  
                foreach (var item in fees_list.Where(x=>x.month_name == month.month_name))
                {
                    list.Add(item);
                    count++;
                }                
            }
            if (count == 0)
            {
                defaulter_fee_grid.ItemsSource = fees_list;
                defaulter_fee_grid.Items.Refresh();
            }
            else
            {
                defaulter_fee_grid.ItemsSource = list;
                defaulter_fee_grid.Items.Refresh();
            }
            calculate_amount();
        }

        private void CheckBox_Checked_fees(object sender, RoutedEventArgs e)
        {
            int count = 0;
            List<sms_fees> list = new List<sms_fees>();
            foreach (sms_fees_category fee in fees_category_list.Where(x => x.isChecked == true))
            {
                foreach (var item in fees_list.Where(x => x.fees_category_id == fee.id))
                {
                    list.Add(item);
                    count++;
                }
            }
            if (fees_category_list.Where(x=>x.isChecked == true).Count() == 0)
            {
                defaulter_fee_grid.ItemsSource = fees_list;
                defaulter_fee_grid.Items.Refresh();
            }
            else
            {
                defaulter_fee_grid.ItemsSource = list;
                defaulter_fee_grid.Items.Refresh();
            }
            calculate_amount();
        }

        void print(bool isGrouped) 
        {
            string r_classes = "All";
            string r_sections = "All";
            string r_fees_Category = "All";
            string r_months = "All";
            string r_years = "All";
            string r_total_students = defaulter_fee_grid.Items.OfType<sms_fees>().Select(x=>x.std_id).Distinct().Count().ToString();

            if (class_cmb.SelectedIndex != 0)
            {
                classes cl = (classes)class_cmb.SelectedItem;
                r_classes = cl.class_name;

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


            List<sms_fees> list = new List<sms_fees>();
            List<string> months_list;
            string month_name_group = "";
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                sms_fees obj = defaulter_fee_grid.Items[i] as sms_fees;

                obj.r_classes = r_classes;
                obj.r_sections = r_sections;
                obj.r_months = r_months;
                obj.r_years = r_years;
                obj.r_total_students = r_total_students;
               

                if (isGrouped)
                {
                    months_list = new List<string>();
                    month_name_group = "";
                    foreach (var fee in defaulter_fee_grid.Items.OfType<sms_fees>().Where(x=>x.std_id == obj.std_id).OrderBy(x=>x.month))
                    {               
                       months_list.Add(fee.month_name);                        
                    }
                    foreach (var month in months_list.Distinct())
                    {
                        month_name_group = month_name_group + " " + month;
                    }
                    obj.month_name_group = month_name_group;
                }

                list.Add(obj);
            }            
            FeesDefaulterReportWindow window = new FeesDefaulterReportWindow(list,isGrouped);
            window.Show();
        }

       
    }
}
