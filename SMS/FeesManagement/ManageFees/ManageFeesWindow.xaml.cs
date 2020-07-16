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
using MySql.Data.MySqlClient;
using SMS.Common;
using SMS.DAL;
using SMS.DAL;
using SMS.FeesManagement.FeesCollection;

namespace SMS.FeesManagement.ManageFees
{
    /// <summary>
    /// Interaction logic for ManageFeesWindow.xaml
    /// </summary>
    public partial class ManageFeesWindow : Window
    {
        public static List<admission> adm_list;
        List<sms_months> months_list;
        List<sms_fees_actual> fees_list_actual;
        List<sms_fees> fees_generated_list;
        List<sms_fees_category> fees_category_list;

        FeesDAL feesDAL;
        AdmissionDAL admissionDAL;
        MiscDAL miscDAL;

        sms_result_engine resultEngineObj;
        int total_success = 0;
        int total_failures = 0;
        int total_warnings = 0;
        int success_count = 0;
        int failure_count = 0;
        List<sms_result_engine> failure_list;
        List<sms_result_engine> success_list;

        string mode = "";
        sms_fees obj;
        ManageFeesPage mfp;
        FeesCollectionForm fcf;

        public ManageFeesWindow(string mode, ManageFeesPage MFP, sms_fees obj)
        {
            InitializeComponent();

            this.mode = mode;
            this.obj = obj;
            this.mfp = MFP;           

            LoadGrid();
            allOption.IsChecked = true;

        }

        public ManageFeesWindow(string mode, FeesCollectionForm FCF, sms_fees obj)
        {
            InitializeComponent();

            this.mode = mode;
            this.obj = obj;
            this.fcf = FCF;

            LoadGrid();

            admission adm = new admission()
            {
                id = obj.std_id.ToString(),
                class_id = obj.class_id.ToString(),
                class_name = obj.class_name,
                section_id = obj.section_id.ToString(),
                section_name = obj.section_name,
            };
            adm_list = new List<admission>();
            adm_list.Add(adm);
        }

        void LoadGrid()
        {
            try
            {
                feesDAL = new FeesDAL();
                admissionDAL = new AdmissionDAL();
                miscDAL = new MiscDAL();
                resultEngineObj = new sms_result_engine();

                months_list = miscDAL.get_all_months();
                fees_category_list = feesDAL.get_all_fees_category();
                fees_category_list.Insert(0, new sms_fees_category() { id = -1, fees_category = "--Select Fees--" });
                fees_category_cmb.ItemsSource = fees_category_list;
                fees_category_cmb.SelectedIndex = 0;

                month_cmb.SelectedIndex = 0;
                months_list.Insert(0, new sms_months() { month_name = "---Month---", id = "-1" });
                month_cmb.ItemsSource = months_list;

                
                monthlyOption.IsChecked = true;
                date.SelectedDate = DateTime.Today;
                year_cmb.ItemsSource = MainWindow.years_list;
                year_cmb.SelectedValue = DateTime.Now.Year;
                withoutAmountOption.IsChecked = true;


                if (mode == "insert")
                {
                    if (this.obj != null)
                    {
                        std_name_TB.Visibility = Visibility.Visible;
                        std_SP.Visibility = Visibility.Collapsed;
                        std_count_SP.Visibility = Visibility.Collapsed;
                        std_name_TB.Text = obj.std_name;
                    }
                    else
                    {
                        std_name_TB.Visibility = Visibility.Collapsed;
                        std_SP.Visibility = Visibility.Visible;
                        std_count_SP.Visibility = Visibility.Visible;
                    }
                    
                    waveoff_SP.Visibility = Visibility.Collapsed;
                    feeType_SP.Visibility = Visibility.Collapsed;          
                }
                else
                {
                    std_count_SP.Visibility = Visibility.Collapsed;
                    std_SP.Visibility = Visibility.Collapsed;
                    feeType_SP.Visibility = Visibility.Collapsed;
                    waveoff_SP.Visibility = Visibility.Visible;
                    std_name_TB.Visibility = Visibility.Visible;
                    amount_textbox.Visibility = Visibility.Visible;
                    amount_type_SP.Visibility = Visibility.Collapsed;

                    fees_category_cmb.IsEnabled = false;
                    amount_textbox.IsReadOnly = true;
                    year_cmb.IsEnabled = false;
                    month_cmb.IsEnabled = false;
                    date.IsEnabled = false;

                    fees_category_cmb.SelectedValue = obj.fees_category_id;
                    amount_textbox.Text = obj.amount.ToString();
                    year_cmb.SelectedValue = obj.year;
                    month_cmb.SelectedValue = obj.month;
                    waveoff_textbox.Text = obj.wave_off.ToString();
                    std_name_TB.Text = obj.std_name;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void allOption_Checked(object sender, RoutedEventArgs e)
        {
            adm_list = admissionDAL.get_all_admissions();
            selectedStudentCountTB.Text = adm_list.Count.ToString();
        }
        private void selectedOption_Checked(object sender, RoutedEventArgs e)
        {
            StudentSelectionWindowNew ssw = new StudentSelectionWindowNew();
            ssw.ShowDialog();
            adm_list = ssw.adm_list.Where(x => x.Checked == true).ToList();
            selectedStudentCountTB.Text = adm_list.Count.ToString();
        }


        //Annual/Monthly Radio buttons
        private void AnnualOption_Checked(object sender, RoutedEventArgs e)
        {
            //fees_category_cmb.ItemsSource = feesDAL.get_all_fees_category();
            //fees_category_cmb.ItemsSource = MainWindow.fees_category_list.Where(x=>x.fees_type_id == 1).Where(x=>x.is_active=="Y");
            //fees_category_cmb.SelectedIndex = 0;
        }

        private void monthlyOption_Checked(object sender, RoutedEventArgs e)
        {
            //fees_category_cmb.ItemsSource = feesDAL.get_all_fees_category();
            //fees_category_cmb.ItemsSource = MainWindow.fees_category_list.Where(x => x.fees_type_id == 2).Where(x => x.is_active == "Y");
            //fees_category_cmb.SelectedIndex=0;
        }


        //save-----------------------------
        public void save()
        {
            if (mode == "insert")
            {
                if (validate())
                {
                    try
                    {
                        List<sms_fees> generated_list = populate_generated_fees(adm_list);

                        int count = feesDAL.insertFeesGenerated(generated_list);

                        if (count > 0)
                        {
                            MessageBox.Show("You have successfully generated " + count + " Students Fees", "Generated", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Not Any Record Updated", "Generated ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        ResultWindow RW = new ResultWindow(resultEngineObj);
                        RW.ShowDialog();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }

                    this.Close();
                }
            }
            else
            {
                if (obj.rem_amount > 0 && obj.rem_amount >= Convert.ToInt32(waveoff_textbox.Text))
                {
                    try
                    {
                        obj.amount = obj.amount;
                        obj.rem_amount = obj.rem_amount - Convert.ToInt32(waveoff_textbox.Text);
                        obj.wave_off = obj.wave_off + Convert.ToInt32(waveoff_textbox.Text);
                        obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                        obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                        obj.date_time = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        obj.wave_off = 0;
                    }
                    try
                    {
                        if (feesDAL.updateFeesGenerated(obj) > 0)
                        {
                            MessageBox.Show(" Successfully Updated Students Fees", " Updated Generated", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                        else
                        {
                            MessageBox.Show("There is Some Error, Please Try Again", " Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        this.Close();
                        //mfp.load_grid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


                else
                {
                    MessageBox.Show("Remaining amount should be greater than zero and greater than wave off amount.", " Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        List<sms_fees> populate_generated_fees(List<admission> admList)
        {
            bool check_actual = false;
            bool check_generated = false;
            failure_list = new List<sms_result_engine>();
            success_list = new List<sms_result_engine>();
            failure_count = 0;
            success_count = 0;
            sms_result_engine result_obj = new sms_result_engine();

            List<sms_fees> generatedList = new List<sms_fees>();
            sms_fees generatedFee;
            sms_years sms_year = (sms_years)year_cmb.SelectedItem;
            sms_fees_category feesCategory = (sms_fees_category)fees_category_cmb.SelectedItem;
            sms_months sms_months = (sms_months)month_cmb.SelectedItem;

            foreach (admission adm in admList)
            {
                check_actual = false;
                check_generated = false;

                result_obj = new sms_result_engine();

                check_actual = false;
                check_generated = false;

                if (!feesDAL.isFeesGenerated(Convert.ToInt32(adm.id), feesCategory.id, 0, Convert.ToInt32(sms_months.month_id), sms_year.id))
                {
                    check_generated = true;

                    generatedFee = new sms_fees();

                    generatedFee.std_id = Convert.ToInt32(adm.id);
                    generatedFee.fees_category_id = feesCategory.id;
                    generatedFee.fees_category = feesCategory.fees_category;

                    generatedFee.month = Convert.ToInt32(sms_months.month_id);
                    generatedFee.month_name = sms_months.month_name;

                    generatedFee.class_id = Convert.ToInt32(adm.class_id);
                    generatedFee.class_name = adm.class_name;
                    generatedFee.section_id = Convert.ToInt32(adm.section_id);
                    generatedFee.section_name = adm.section_name;

                    generatedFee.year = sms_year.id;
                    generatedFee.date = date.SelectedDate.Value;
                    generatedFee.due_date = due_date.SelectedDate.Value;
                    generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                    generatedFee.created_by = MainWindow.emp_login_obj.emp_user_name;
                    generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);

                    generatedFee.date_time = DateTime.Now;


                    //For Fix amount, no actual fees needed
                    if (amountOption.IsChecked == true)
                    {
                        check_actual = true;
                        generatedFee.amount = Convert.ToInt32(amount_textbox.Text);
                        generatedFee.rem_amount = Convert.ToInt32(amount_textbox.Text);
                        generatedFee.discount = 0;

                        generatedList.Add(generatedFee);  // add object
                    }
                    else
                    {
                        sms_fees_actual actualobj = new sms_fees_actual();
                        List<sms_fees_actual> actualList = feesDAL.getActualFeesByStdID(Convert.ToInt32(adm.id));
                        foreach (sms_fees_actual fee in actualList.Where(x => x.fees_category_id == feesCategory.id))
                        {
                            actualobj = fee;
                            check_actual = true;
                        }

                        if (check_actual)
                        {
                            generatedFee.amount = actualobj.amount;
                            generatedFee.actual_amount = actualobj.actual_amount;
                            generatedFee.rem_amount = actualobj.amount;
                            generatedFee.discount = actualobj.discount;

                            generatedList.Add(generatedFee);  // add object
                        }
                    }
                }


                // Result Engine
                result_obj.id = adm.adm_no;
                result_obj.action = adm.std_name + " [" + adm.class_name + "-" + adm.section_name + "]";

                if (check_actual && check_generated)
                {
                    total_success++;
                    success_count++;
                    result_obj.serial_no = success_count;
                    result_obj.reason = "Successfully Added";

                    success_list.Add(result_obj);
                }
                else //failure
                {
                    total_failures++;
                    failure_count++;
                    result_obj.serial_no = failure_count;
                    if (!check_generated)
                    {
                        result_obj.reason = feesCategory.fees_category + " Has Been Alrady Generated For " + sms_months.month_name + " " + sms_year.year.ToString();
                    }
                    else
                    {
                        result_obj.reason = feesCategory.fees_category + " Not Set In Admission Form, Please Set Actual Fees First";
                    }
                    failure_list.Add(result_obj);
                }

            }
            resultEngineObj = new sms_result_engine();
            resultEngineObj.success_count = total_success;
            resultEngineObj.failure_count = total_failures;
            resultEngineObj.success_list = success_list;
            resultEngineObj.failure_list = failure_list;

            return generatedList;
        }

        bool validate()
        {
            if (adm_list.Count == 0)
            {
                string alertText = "Selected Student Count Should Not Be Blank";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            else if ((amount_sp.Visibility == Visibility.Visible && amount_textbox.Text == "0") || (amount_sp.Visibility == Visibility.Visible && amount_textbox.Text == ""))
            {
                amount_textbox.Focus();
                string alertText = "Amount Should Not Be Blank Or Zero.";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (fees_category_cmb.SelectedIndex == 0)
            {
                fees_category_cmb.Focus();
                string alertText = "Fees Category Should Not Be Blank.";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (month_cmb.SelectedIndex == 0)
            {
                month_cmb.Focus();
                string alertText = "Month Name Should Not Be Blank.";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            else if (month_cmb.SelectedIndex == 0)
            {
                month_cmb.Focus();
                string alertText = "Month Name Should Not Be Blank.";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void fees_category_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fees_category_cmb.SelectedItem != null)
            {
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();
            }
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date.SelectedDate != null)
            {
                due_date.SelectedDate = date.SelectedDate.Value.AddDays(10);
            }
        }


        private void withoutAmountOption_Checked(object sender, RoutedEventArgs e)
        {
            amount_sp.Visibility = Visibility.Collapsed;
        }

        private void amountOption_Checked(object sender, RoutedEventArgs e)
        {
            amount_sp.Visibility = Visibility.Visible;
        }


    }
}
