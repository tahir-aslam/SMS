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
using SMS.DAL;
using System.Text.RegularExpressions;

namespace SMS.ClassManagement.Class
{
    /// <summary>
    /// Interaction logic for ClassChargesForm.xaml
    /// </summary>
    public partial class ClassChargesForm : Window
    {
        ClassesDAL classesDAL;
        public List<classes> classes_list;

        sms_fees_actual obj;
        sms_fees_actual classes_obj;
        string mode;
        ClassSearch CS;

        public ClassChargesForm(string m, ClassSearch cs, sms_fees_actual ob)
        {
            InitializeComponent();
            classesDAL = new ClassesDAL();   

            mode = m;
            CS = cs;            
            this.obj = ob;

            classes_list = classesDAL.getAllClasses();
            classes_list.Insert(0, new classes() { id = "-1", class_name ="--Select Class--" });
            class_cmb.ItemsSource = classes_list;
            class_cmb.SelectedIndex = 0;
            

            fees_category_cmb.ItemsSource = MainWindow.fees_category_list.Where(x => x.is_active == "Y");
            fees_category_cmb.SelectedIndex = 0;

            if (mode == "edit")
            {                
                fill_control();
            }
        }

        public void fill_control() 
        {
            class_cmb.SelectedValue = obj.class_id;
            fees_category_cmb.SelectedValue = obj.fees_category_id;           
            amount_textbox.Text = obj.amount.ToString();
        }

        public void fill_object() 
        {
            classes_obj = new sms_fees_actual();
            if(mode == "edit")
            {
                classes_obj.id = obj.id;
            }
            
            sms_fees_category feesCategory = (sms_fees_category)fees_category_cmb.SelectedItem;
            
            classes cl = (classes)class_cmb.SelectedItem;

            classes_obj.class_id = Convert.ToInt32(cl.id);
            classes_obj.class_name = cl.class_name;
            classes_obj.fees_category_id = feesCategory.id;
            classes_obj.fees_category = feesCategory.fees_category;          
           
            classes_obj.amount = Convert.ToInt32(amount_textbox.Text);

            classes_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            classes_obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
            classes_obj.date_time = DateTime.Now;
        }

        bool validate()
        {
            if (class_cmb.SelectedIndex == 0)
            {
                class_cmb.Focus();
                string alertText = "Please Select Class";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (amount_textbox.Text.Count() == 0)
            {
                class_cmb.Focus();
                string alertText = "Amount should be greater than zero";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }               
            else
            {
                return true;
            }
        }

        //--------------    Save        ------------------------
        public void save()
        {            
            if (validate())
            {
                fill_object();
                if (mode == "insert")
                {
                    if (!CS.checkDuplicateEntry(classes_obj))
                    {
                        if (classesDAL.insertFeesClasses(classes_obj) > 0)
                        {
                            MessageBox.Show("Record Added Successfully");
                            CS.load_charges_grid();
                            MessageBoxResult mbr = MessageBox.Show("Are You Want To Add More Records ?", "Delete Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Warning);
                            if (mbr == MessageBoxResult.Yes)
                            {
                                amount_textbox.Text = "";
                                fees_category_cmb.SelectedIndex = 0;
                                fees_category_cmb.Focus();
                            }
                            else 
                            {
                                this.Close();
                            }
                            
                            
                        }
                        else
                        {
                            MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Duplicate entry");
                    }
                }
                else if (mode == "edit")
                {

                    if (classesDAL.updateFeesClasses(classes_obj) > 0)
                    {
                        MessageBox.Show("Record Updated Successfully");

                        MessageBoxResult mbr = MessageBox.Show("Do You Want To Apply This Fix To All Students Of This Class ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (mbr == MessageBoxResult.Yes)
                        {
                            
                        }                        

                        
                        this.Close();
                        CS.load_charges_grid();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else
                {
                    MessageBox.Show("mode not set");
                }

            }
        }

        

        private void submit_btn_Click(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                save();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
