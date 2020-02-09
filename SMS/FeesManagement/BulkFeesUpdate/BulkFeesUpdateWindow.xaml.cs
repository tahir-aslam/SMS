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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using SMS.DAL;

namespace SMS.FeesManagement.BulkFeesUpdate
{
    /// <summary>
    /// Interaction logic for BulkFeesUpdateWindow.xaml
    /// </summary>
    public partial class BulkFeesUpdateWindow : Window
    {
        public List<admission> adm_list_new;
        List<sms_fees_category> fees_category_list;
        FeesDAL feesDAL;
        
        public BulkFeesUpdateWindow(List<admission> adm_list)
        {
            InitializeComponent();
            adm_list_new = new List<admission>();
            foreach (admission adm in adm_list.Where(x => x.Checked == true))
            {
                adm_list_new.Add(adm);
            }
            // mark adm_list unchecked
            adm_list.ForEach(x=>x.Checked = false);
            std_lbl.Content = adm_list_new.Count;            
            fee_textbox.IsEnabled = false;

            feesDAL = new FeesDAL();            
            fees_category_list = feesDAL.get_all_fees_category();
            fees_category_list.Insert(0, new sms_fees_category() { id = -1, fees_category = "--Select Fees--" });
            fees_category_cmb.ItemsSource = fees_category_list;
            fees_category_cmb.SelectedIndex = 0;            
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void click_save(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Have You Backup your data before doing this operation, It will change all student fee ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mbr == MessageBoxResult.Yes)
            {
                if (validate())
                {
                    if (updateActualFees())
                    {
                        MessageBox.Show("Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();                        
                    }
                    else
                    {
                        MessageBox.Show("Error", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                        this.Close();
                    }
                }
            }
            else
            {
                this.Close();
            }
            
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 

        public bool validate()
        {
            if (fees_category_cmb.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Fees Category", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                fees_category_cmb.Focus();
                return false;
            }
           
            else if (amount_option.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Amount Option", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                amount_option.Focus();
                return false;
            }
            else if (fee_textbox.Text == "")
            {
                MessageBox.Show("Please Enter Amount", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                fee_textbox.Focus();
                return false;
            }
            else
            {
                return true;
            }

        }
        
        private void amount_option_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (amount_option.SelectedIndex == 0)
            {
                try
                {
                    fee_textbox.IsEnabled = false;
                }
                catch (Exception ex) { }
            }
            else
            {
                try
                {
                    fee_textbox.IsEnabled = true;
                }
                catch (Exception ex) { }
            }
        }

        bool updateActualFees() 
        {
            int amount = 0;
            sms_fees_category fees_category = fees_category_cmb.SelectedItem as sms_fees_category;   
            try{
           amount = Convert.ToInt32(fee_textbox.Text);}catch(Exception ex){MessageBox.Show(ex.Message);}
            List<sms_fees_actual> actualFeesList = feesDAL.get_all_actual_fees();
            List<sms_fees_actual> actualFeesListSubmit = new List<sms_fees_actual>();
            sms_fees_actual feesActualObj;
            bool check = false;
           
                
            foreach (var adm in adm_list_new)
            {
                feesActualObj = new sms_fees_actual();
                check = false;

                if (actualFeesList.Exists(x => x.std_id.ToString() == adm.id))
                {
                    foreach (var actual in actualFeesList.Where(x=>x.std_id.ToString() == adm.id).Where(x=>x.fees_category_id == fees_category.id))
                    {
                        feesActualObj = actual;
                        if (amount_option.SelectedIndex == 1)
                        {
                            if (fee_type_cmb.SelectedIndex == 0)
                            {
                                feesActualObj.amount = amount;
                            }
                            else
                            {
                                feesActualObj.actual_amount = amount;
                            }
                        }
                        else 
                        {
                            if (fee_type_cmb.SelectedIndex == 0)
                            {
                                feesActualObj.amount = feesActualObj.amount + amount;
                            }
                            else
                            {
                                feesActualObj.actual_amount = feesActualObj.actual_amount + amount;
                            }
                        }

                        //for negative
                        if (feesActualObj.actual_amount - feesActualObj.amount < 0)
                        {
                            feesActualObj.actual_amount = feesActualObj.amount;
                        }

                        feesActualObj.discount = feesActualObj.actual_amount - feesActualObj.amount;
                        feesActualObj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.id);
                        feesActualObj.date_time = DateTime.Today;
                        feesActualObj.created_by = MainWindow.emp_login_obj.emp_user_name;
                        check = true;

                            
                    }                       
                }
                if(check == false)
                {
                    feesActualObj.std_id = Convert.ToInt32(adm.id);
                    feesActualObj.fees_category_id = fees_category.id;
                    feesActualObj.fees_category = fees_category.fees_category;
                    feesActualObj.actual_amount = amount;
                    feesActualObj.amount = amount;
                    feesActualObj.discount = 0;
                    feesActualObj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.id);
                    feesActualObj.date_time = DateTime.Today;
                    feesActualObj.created_by = MainWindow.emp_login_obj.emp_user_name;
                }
                actualFeesListSubmit.Add(feesActualObj);
            }

            if (feesDAL.insertActualFees(actualFeesListSubmit) > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }
            
        }
      
    }
}
