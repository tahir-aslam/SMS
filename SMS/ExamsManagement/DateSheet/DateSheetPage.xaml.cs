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

namespace SMS.ExamsManagement.DateSheet
{
    /// <summary>
    /// Interaction logic for DateSheetPage.xaml
    /// </summary>
    public partial class DateSheetPage : Page
    {
        List<sms_exams_date_sheet> date_sheet_list;
        DateSheetWindow window;
        sms_exams_date_sheet obj;
        string mode;
        ExamsDAL examsDAL;

        public DateSheetPage()
        {
            InitializeComponent();

            date_sheet_list = new List<sms_exams_date_sheet>();
            examsDAL = new ExamsDAL();
            obj = new sms_exams_date_sheet();

            SearchTextBox.Focus();
            load_grid();
        }

        public void load_grid()
        {
            date_sheet_list = examsDAL.get_all_date_sheet();
            subjects_grid.ItemsSource = date_sheet_list;
            this.subjects_grid.Items.Refresh();
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            window = new DateSheetWindow(mode, this, obj);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        //-------------     Editing          ---------------------------

        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void subjects_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            obj = (sms_exams_date_sheet)subjects_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                window = new DateSheetWindow(mode, this, obj);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }

        //-------------     Delete          ---------------------------

        private void click_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                if (date_sheet_list.Where(x => x.IsChecked == true).Count() == 0)
                {
                    MessageBox.Show("Please Select A Row");
                }
                else
                {
                    MessageBoxResult mbr = MessageBox.Show("Do You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        if (examsDAL.DeleteDateSheet(date_sheet_list.Where(x => x.IsChecked == true).Select(x => x.id).ToList(), Convert.ToInt32(MainWindow.emp_login_obj.emp_id)) > 0)
                        {
                            MessageBox.Show("Succesfully Deleted");
                            load_grid();
                        }
                        else
                        {
                            load_grid();
                            MessageBox.Show("OOPs! Theres is some problem");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
        }

        //check box
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            if (checkBox.IsChecked.Value)
            {
                foreach (var item in subjects_grid.Items)
                {
                    sms_exams_date_sheet fee = item as sms_exams_date_sheet;
                    fee.IsChecked = checkBox.IsChecked.Value;
                }
            }
            else
            {
                foreach (sms_exams_date_sheet item in date_sheet_list)
                {
                    item.IsChecked = false;
                }
            }

            subjects_grid.Items.Refresh();            
        }

        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            subjects_grid.SelectedItem = e.Source;
            sms_exams_date_sheet fee = new sms_exams_date_sheet();
            fee = (sms_exams_date_sheet)subjects_grid.SelectedItem;
            foreach (sms_exams_date_sheet item in date_sheet_list)
            {
                if (fee.id == item.id)
                {
                    item.IsChecked = checkBox.IsChecked.Value;
                }
            }            
        }
    }
}
