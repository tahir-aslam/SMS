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
using System.IO;
using MySql.Data.MySqlClient;
using SMS.Upload;
using SMS.Messaging.SmsOption;
using SMS.Messaging.BrandedSms;
using System.ComponentModel;
using SMS.DAL;

namespace SMS.Messaging.FeesDefaulter
{
    /// <summary>
    /// Interaction logic for FeesDefaulterSmsPage.xaml
    /// </summary>
    public partial class FeesDefaulterSmsPage : Page
    {
        FeesDAL feesDAL;
        ClassesDAL classDAL;
        MiscDAL miscDAL;

        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_months> months_list;
        List<sms_years> years_list;
        List<sms_fees_category> fees_category_list;
        List<sms_fees> fees_list;
        List<admission> std_nos_list;
        List<sms_fees> defaulter_list;
        string message;
        admission adm_obj;
        public static bool isbranded = false;

        public FeesDefaulterSmsPage()
        {
            InitializeComponent();

            feesDAL = new FeesDAL();
            classDAL = new ClassesDAL();
            miscDAL = new MiscDAL();

            //general_btn.IsChecked = true;
            withoutAmount_btn.IsChecked = true;
            if (MainWindow.ins.isMultiPartSMSAccess == "Y")
            {
                encodedRB.IsEnabled = true;
            }
            else
            {
                encodedRB.IsEnabled = false;
            }

            try
            {
                classes_list = classDAL.get_all_classes();
                months_list = miscDAL.get_all_months();
                years_list = miscDAL.get_all_years();
                fees_category_list = feesDAL.get_all_fees_category();                
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
            setFeesList(fees_list);
            clearAllFilters();
        }

        void setFeesList(List<sms_fees> fees_list) 
        {
            defaulter_list = new List<sms_fees>();
            List<string> months_list;
            List<string> fees_category_list;
            int rem_amount=0;
            string month_name_group = "";
            string fees_category_group = "";
            int rem_amount_group = 0;

            foreach (var obj in fees_list.Select(x=>x.std_id).Distinct())
            {
                rem_amount = 0;
                months_list = new List<string>();
                fees_category_list = new List<string>();
                month_name_group = "";
                fees_category_group = "";
                rem_amount_group = 0;

                foreach (var fees in fees_list.Where(x => x.std_id == obj))
                {                   
                    foreach (var fee in fees_list.Where(x => x.std_id == obj))
                    {
                        months_list.Add(fee.month_name);
                        fees_category_list.Add(fee.fees_category);
                        rem_amount_group = rem_amount_group + fee.rem_amount;
                    }
                    foreach (var month in months_list.Distinct())
                    {
                        month_name_group = month_name_group + " " + month;                        
                    }
                    foreach (var category in fees_category_list.Distinct())
                    {
                        fees_category_group = fees_category_group + " " + category;
                    }

                    fees.month_name_group = month_name_group;
                    fees.fees_category_group = fees_category_group;
                    fees.rem_amount_group = rem_amount_group;
                    defaulter_list.Add(fees);
                    break;
                }
            }
            defaulter_fee_grid.ItemsSource = defaulter_list;           
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sms_fees f;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                f = (sms_fees)defaulter_fee_grid.Items[i];
                f.Checked = checkBox.IsChecked.Value;
            }
            defaulter_fee_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            defaulter_fee_grid.SelectedItem = e.Source;
            sms_fees f_obj = new sms_fees();
            f_obj = (sms_fees)defaulter_fee_grid.SelectedItem;
            foreach (sms_fees f in fees_list)
            {
                if (f.id == f_obj.id)
                {
                    f_obj.Checked = checkBox.IsChecked.Value;
                }
            }

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

        void filter()
        {
            SearchTextBox.Text = "";
            ICollectionView cv = CollectionViewSource.GetDefaultView(defaulter_fee_grid.ItemsSource);
            cv.Filter = o =>
            {
                sms_fees f = o as sms_fees;
                if (getMonth(f) && getYear(f) && getFeesCategory(f) && getClasses(f) && getSections(f))
                {
                    return true;
                }
                return false;
            };           
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
            if (fees_category_cmb.SelectedIndex > 0 && !f.fees_category_group.Contains(category.fees_category))
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
        }
        private void clear_btn_Click(object sender, RoutedEventArgs e)
        {
            clearAllFilters();
        }
      

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (default_btn.IsChecked == true)
            {
                general_grid.Visibility = Visibility.Hidden;
                option_grid.Visibility = Visibility.Visible;
                withoutAmount_btn.IsChecked = true;
                encodedRB.IsChecked = false;
                englishRB.IsChecked = true;
            }
            else
            {
                general_grid.Visibility = Visibility.Visible;
                option_grid.Visibility = Visibility.Hidden;
                englishRB.IsChecked = true;
                encodedRB.IsChecked = false;
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
                dpd.AddValueChanged(defaulter_fee_grid, itemsSourceChanged);
            }
        }
        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = defaulter_fee_grid.Items.Count.ToString();
        }
        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            std_nos_list = new List<admission>();
            //int index = filter_def_cmb.SelectedIndex;
            foreach (sms_fees f in defaulter_list.Where(x => x.Checked == true))
            {
                message = "";
                adm_obj = new admission();
                adm_obj.std_name = f.std_name;
                adm_obj.father_name = f.father_name;
                adm_obj.cell_no = f.cell_no;                
                adm_obj.class_id = f.class_id.ToString();
                adm_obj.class_name = f.class_name;
                adm_obj.section_id = f.section_id.ToString();
                adm_obj.section_name = f.section_name;

                string category = ((sms_fees_category)fees_category_cmb.SelectedItem).fees_category;

                if (default_btn.IsChecked == true)
                {
                    if (withoutAmount_btn.IsChecked == true)
                    {
                        if (fees_category_cmb.SelectedIndex > 0)
                        {                            
                            //show only fees category not all if filter for specific category
                            message = "Respected Parents: Kindly pay " + f.std_name + "'s " + category + " of month(s) " + f.month_name_group + " to the Institute. Thank you. Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                        }
                        else 
                        {
                            message = "Respected Parents: Kindly pay " + f.std_name + "'s " + f.fees_category_group + " of month(s) " + f.month_name_group + " to the Institute. Thank you. Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                        }
                        
                    }
                    else
                    {
                        if (fees_category_cmb.SelectedIndex > 0)
                        {
                            message = "Respected Parents: Kindly pay " + f.std_name + "'s " + category + " of month(s) " + f.month_name_group + " to the Institute."+Environment.NewLine+"Total Remaining Dues=" + f.rem_amount_group +Environment.NewLine+"Thank you. Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                        }
                        else
                        {
                            message = "Respected Parents: Kindly pay " + f.std_name + "'s " + f.fees_category_group + " of month(s) " + f.month_name_group + " Rs " + f.rem_amount_group + " to the Institute. Thank you. Admin " + MainWindow.ins.institute_name + ". " + MainWindow.ins.institute_phone + " " + MainWindow.ins.institute_cell;
                        }
                    }
                    
                }
                else 
                {
                    message = message_textbox.Text;
                }

                adm_obj.sms_message = message;
                adm_obj.sms_type = "Fees Defaulter";
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
                        if (encodedRB.IsChecked == true)
                        {
                            BrandedSmsEngine bse = new BrandedSmsEngine(std_nos_list,true);
                            bse.Show();
                        }
                        else
                        {
                            BrandedSmsEngine bse = new BrandedSmsEngine(std_nos_list,false);
                            bse.Show();
                        }
                        
                    }
                    else
                    {
                        if (encodedRB.IsChecked == true)
                        {
                            UploadWindow uw = new UploadWindow(std_nos_list, true);
                            uw.Show();
                        }
                        else
                        {
                            UploadWindow uw = new UploadWindow(std_nos_list, false);
                            uw.Show();
                        }
                        
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Minimum One Student");
                }
            }
        }

        private void CheckBox_Checked1(object sender, RoutedEventArgs e)
        {
            CheckMonth();
        }

        void CheckMonth()
        {
            int count = 0;
            List<sms_fees> list = new List<sms_fees>();
            foreach (sms_months month in months_list.Where(x => x.isChecked == true))
            {
                foreach (var item in fees_list.Where(x => x.month_name == month.month_name))
                {
                    list.Add(item);
                    count++;
                }
            }
            if (count == 0)
            {
                setFeesList(fees_list);
                //defaulter_fee_grid.Items.Refresh();
            }
            else
            {
                setFeesList(list);
                //defaulter_fee_grid.Items.Refresh();
            }
        }
    }
}
