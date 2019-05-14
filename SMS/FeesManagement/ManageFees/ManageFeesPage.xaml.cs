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
using SMS.DAL;
using SMS.DAL;
using System.ComponentModel;
using System.Globalization;
using SMS.Common;
using SMS.HelperClasses;

namespace SMS.FeesManagement.ManageFees
{
    /// <summary>
    /// Interaction logic for ManageFeesPage.xaml
    /// </summary>
    public partial class ManageFeesPage : Page, INotifyPropertyChanged
    {
        ListCollectionView collectionView;
        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_months> months_list;
        List<sms_years> years_list;
        List<sms_fees_category> category_list;

        List<sms_fees> fees_list;
        List<sms_fees> filter_list;
        sms_fees filter_obj;
        DateTime dt = DateTime.Now;
        List<admission> adm_list;       

        FeesDAL feesDAL;
        ClassesDAL classesDAL;
        AdmissionDAL admissionDAL;
        MiscDAL miscDAL;        

        int total_amount = 0;
        string mode;
        sms_fees obj;
        ManageFeesWindow mfw;

        private bool _panelLoading;
        private string _panelMainMessage = "Main Loading Message";
        private string _panelSubMessage = "Sub Loading Message";

        public ManageFeesPage()
        {
            InitializeComponent();

            PanelLoading = true;
            feesDAL = new FeesDAL();
            classesDAL = new ClassesDAL();
            admissionDAL = new AdmissionDAL();
            miscDAL = new DAL.MiscDAL();
            DataContext = this;

            try
            {
                classes_list = classesDAL.get_all_classes();
                months_list = miscDAL.get_all_months();
                years_list = miscDAL.get_all_years();
                category_list = feesDAL.get_all_fees_category();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            classes_list.Insert(0, new classes() { class_name = "---All Classes---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            
            category_list.Insert(0, new sms_fees_category() { id = -1, fees_category = "--All Categories" });
            fees_category_cmb.ItemsSource = category_list;

            months_list.Insert(0, new sms_months() { id = "-1", month_name = "--All Months--" });
            month_cmb.ItemsSource = months_list;

            years_list.Insert(0, new sms_years() { id = -1, year = "--All Years--" });
            year_cmb.ItemsSource = years_list;          
        }

        public void load_grid()
        {
            
            //_panelLoading = true;
            SearchTextBox.Focus();
            
            try
            {
                adm_list = admissionDAL.get_all_admissions();
                fees_list = feesDAL.get_all_fees_generated();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }            

            FeesGrid.ItemsSource = fees_list;
            calculate_amount();

            fees_category_cmb.SelectedIndex = 0;
            month_cmb.SelectedIndex = 0;
            year_cmb.SelectedIndex = 0;
            class_cmb.SelectedIndex = 0;

            date_picker_to.SelectedDate = MainWindow.session.session_start;
            date_picker_from.SelectedDate = DateTime.Now.AddDays(20);
            //year_cmb.SelectedValue = DateTime.Now.Year;

            //loadingPanel(false, "", "");
        }
        
        public void calculate_amount()
        {
            sms_fees fees;
            total_amount = 0;
            for (int i = 0; i < FeesGrid.Items.Count; i++ )
            {
                fees = (sms_fees)FeesGrid.Items[i];
                total_amount = total_amount + fees.amount;
            }
            amount_TB.Text = total_amount.ToString("C", CultureInfo.CreateSpecificCulture("ur-PKR"));
            count_text.Text = FeesGrid.Items.Count.ToString() ;
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(FeesGrid.ItemsSource);
            if (v_search == null)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = o =>
                {
                    sms_fees x = o as sms_fees;
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
                    //else if (search_cmb.SelectedIndex == 5)
                    //{
                    //    return (x.father_cnic.ToUpper().StartsWith(v_search.ToUpper()) || x.father_cnic.ToUpper().Contains(v_search.ToUpper()));
                    //}
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


        // click Events -------------------------------------------

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            mfw = new ManageFeesWindow(mode, this, obj);
            mfw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mfw.ShowDialog();           

            load_grid();
        }
        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void click_delete(object sender, RoutedEventArgs e)
        {            
            List<sms_fees> checkedFeesList = fees_list.Where(x=>x.isChecked == true).ToList();
            List<sms_fees> feesListToBeDeleted = checkedFeesList.Where(x => x.amount == x.rem_amount).ToList();
            List<sms_fees> feesListToBeNotDeleted = checkedFeesList.Where(x => x.amount != x.rem_amount).ToList();
            int count = checkedFeesList.Count();
            if (count > 0)
            {
                
                    MessageBoxResult mbr = MessageBox.Show("Do You Want To Delete " + count.ToString() + " Record(s) ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        
                        count =  feesDAL.deleteGeneratedFees(feesListToBeDeleted);

                        //Result Window                        
                        sms_result_engine result_obj;
                        List<sms_result_engine> failure_list = new List<sms_result_engine>();
                        List<sms_result_engine> success_list = new List<sms_result_engine>();
                        int total_success = 0;
                        int total_failure = 0;

                        foreach (var item in checkedFeesList)
                        {
                            result_obj = new sms_result_engine();
                            result_obj.serial_no = item.id;
                            result_obj.id = item.adm_no;
                            result_obj.action = item.std_name + " [" + item.class_name + "-" + item.section_name + "]";

                            if (item.amount == item.rem_amount)
                            {
                                total_success++;
                                result_obj.reason = "Successfully Deleted";
                                success_list.Add(result_obj);
                            }
                            else
                            {
                                total_failure++;
                                result_obj.reason = item.fees_category + " Has Already Paid, It cannot be deleted";
                                failure_list.Add(result_obj);
                            }
                        }
                        sms_result_engine resultEngineObj = new sms_result_engine();
                        resultEngineObj.success_count = total_success;
                        resultEngineObj.failure_count = total_failure;
                        resultEngineObj.success_list = success_list;
                        resultEngineObj.failure_list = failure_list;

                        if (count > 0)
                        {
                           
                            MessageBox.Show("You Have Successfully Deleted "+count.ToString()+" Records", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                            load_grid();
                        }                        

                        ResultWindow RW = new ResultWindow(resultEngineObj);
                        RW.ShowDialog();

                        calculate_selected();
                        calculate_amount();
                    }                
            }
            else 
            {
                MessageBox.Show("Please Select Minimum One Record", "Information", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            string r_classes = "All";
            string r_sections = "All";
            string r_fees_Category = "All";            
            string r_months = "All";
            string r_years = "All";

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
            for (int i = 0; i < FeesGrid.Items.Count; i++ )
            {
                sms_fees obj = FeesGrid.Items[i] as sms_fees;
                obj.toDate = date_picker_to.SelectedDate.Value;
                obj.fromDate = date_picker_from.SelectedDate.Value;
                obj.r_classes = r_classes;
                obj.r_sections = r_sections;
                obj.r_months = r_months;
                obj.r_years = r_years;
                
                list.Add(obj);
            }
            ManageFeesReportWindow window = new ManageFeesReportWindow(list);
            window.Show();
        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    PanelLoading = true;
                    dt = date_picker_from.SelectedDate.Value;
                    fees_list = feesDAL.get_all_fees_generated_by_date(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    collectionView = new ListCollectionView(fees_list);                  
                    FeesGrid.ItemsSource = collectionView;
                    calculate_amount();

                    clearAllFilters();
                    SearchTextBox.Text = "";
                    PanelLoading = false;
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
                    PanelLoading = true;
                    dt = date_picker_from.SelectedDate.Value;
                    fees_list = feesDAL.get_all_fees_generated_by_date(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    collectionView = new ListCollectionView(fees_list);
                    FeesGrid.ItemsSource = collectionView;
                    calculate_amount();

                    clearAllFilters();
                    SearchTextBox.Text = "";
                    PanelLoading = false;
                }
            }
        }       

        public void editing() 
        {
            obj = (sms_fees)FeesGrid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                mfw = new ManageFeesWindow(mode, this, obj);
                mfw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                mfw.ShowDialog();               
            }
        }

        // Filters ---------------------------------------

        private void fees_category_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fees_category_cmb.SelectedItem != null) 
            {                
                if (fees_category_cmb.SelectedIndex != 0)
                {
                    filter();
                }
                else 
                {
                    filter();
                }
            }
        }
        private void year_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (year_cmb.SelectedItem != null)
            {
                if (year_cmb.SelectedIndex != 0)
                {                    
                    filter();
                }
                else
                {                    
                    filter();
                }
            }
        }
        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (month_cmb.SelectedItem != null)
            {
                if (month_cmb.SelectedIndex != 0)
                {                    
                    filter();
                }                    
                else
                {
                    filter();
                }
            }
        }
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb.SelectedItem != null)
            {
                classes cl = (classes)class_cmb.SelectedItem;

                if (class_cmb.SelectedIndex != 0)
                {

                    filter();

                    section_cmb.IsEnabled = true;
                    sections_list = new List<sections>();
                    sections_list = classesDAL.get_all_sections(cl.id);
                    sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;
                }
                else
                {
                    filter();
                }
            }
        }
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedItem != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    filter();
                }
                else
                {
                    filter();
                }
            }
        }

        void filter() 
        {
            SearchTextBox.Text = "";
            ICollectionView cv = CollectionViewSource.GetDefaultView(FeesGrid.ItemsSource);
            cv.Filter = o =>
            {
                sms_fees f = o as sms_fees;            
                if(getMonth(f) && getYear(f) && getFeesCategory(f) && getClasses(f) && getSections(f))
                {
                    return true;
                }                            
                return false;            
            };
            calculate_amount();
        }

        bool getMonth(sms_fees f) 
        {
            sms_months month = (sms_months)month_cmb.SelectedItem;
            if(month_cmb.SelectedIndex > 0 && f.month.ToString() != month.month_id)
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

        void clearAllFilters() 
        {
            month_cmb.SelectedIndex = 0;
            year_cmb.SelectedIndex = 0;
            fees_category_cmb.SelectedIndex = 0;
            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;            
        }

        //check box
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            if (checkBox.IsChecked.Value)
            {
                foreach (var item in FeesGrid.Items)
                {
                    sms_fees fee = item as sms_fees;
                    fee.isChecked = checkBox.IsChecked.Value;
                }
            }
            else 
            {
                foreach (sms_fees item in fees_list)
                {
                    item.isChecked = false;                 
                }
            }
            
            FeesGrid.Items.Refresh();
            calculate_selected();
        }

        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            FeesGrid.SelectedItem = e.Source;
            sms_fees fee = new sms_fees();
            fee = (sms_fees)FeesGrid.SelectedItem;
            foreach (sms_fees item in fees_list)
            {
                if (fee.id == item.id)
                {
                    item.isChecked = checkBox.IsChecked.Value;
                }
            }
            calculate_selected();
        }
        void calculate_selected() 
        {
            int count = 0;
            count = fees_list.Where(x=>x.isChecked == true).Count();
            selected_text.Text = count.ToString();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            load_grid();
        }

        public void loadingPanel(bool visibility, string mainMessage, string subMessage)
        {
            SMS.Models.loadingPanel panel = new loadingPanel();
            panel.PanelLoading = visibility;
            panel.PanelMainMessage = mainMessage;
            panel.PanelSubMessage = subMessage;           
            smsLoadingPanel.DataContext = panel;
        }






        //================================================================================================================================================
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether [panel loading].
        /// </summary>
        /// <value>
        /// <c>true</c> if [panel loading]; otherwise, <c>false</c>.
        /// </value>
        public bool PanelLoading
        {
            get
            {
                return _panelLoading;
            }
            set
            {
                _panelLoading = value;
                RaisePropertyChanged("PanelLoading");
            }
        }

        /// <summary>
        /// Gets or sets the panel main message.
        /// </summary>
        /// <value>The panel main message.</value>
        public string PanelMainMessage
        {
            get
            {
                return _panelMainMessage;
            }
            set
            {
                _panelMainMessage = value;
                RaisePropertyChanged("PanelMainMessage");
            }
        }

        /// <summary>
        /// Gets or sets the panel sub message.
        /// </summary>
        /// <value>The panel sub message.</value>
        public string PanelSubMessage
        {
            get
            {
                return _panelSubMessage;
            }
            set
            {
                _panelSubMessage = value;
                RaisePropertyChanged("PanelSubMessage");
            }
        }

        /// <summary>
        /// Gets the panel close command.
        /// </summary>
        public ICommand PanelCloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    // Your code here.
                    // You may want to terminate the running thread etc.
                    PanelLoading = false;
                });
            }
        }

        /// <summary>
        /// Gets the show panel command.
        /// </summary>
        public ICommand ShowPanelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    PanelLoading = true;
                });
            }
        }

        /// <summary>
        /// Gets the hide panel command.
        /// </summary>
        public ICommand HidePanelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    PanelLoading = false;
                });
            }
        }

        /// <summary>
        /// Gets the change sub message command.
        /// </summary>
        public ICommand ChangeSubMessageCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    PanelSubMessage = string.Format("Message: {0}", DateTime.Now);
                });
            }
        }

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
