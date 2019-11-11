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
        List<sms_exams_subjects> subjects_list;
        DateSheetWindow window;
        sms_exams_subjects obj;
        string mode;
        SubjectsDAL subjectsDAL;
        string insertion;
        string updation;

        public DateSheetPage()
        {
            InitializeComponent();

            subjects_list = new List<sms_exams_subjects>();
            subjectsDAL = new SubjectsDAL();
            obj = new sms_exams_subjects();

            SearchTextBox.Focus();
            load_grid();
        }

        public void load_grid()
        {
            subjects_list = new List<sms_exams_subjects>();
            subjects_list = subjectsDAL.GetAllSubjectsAssignment();
            subjects_grid.ItemsSource = subjects_list;
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
            obj = (sms_exams_subjects)subjects_grid.SelectedItem;
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
            obj = (sms_exams_subjects)subjects_grid.SelectedItem;
            if (obj == null)
            {
                MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Do You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    if (subjectsDAL.DeleteSubjectAssignment(subjects_list.Where(x => x.id == obj.id).Where(x => x.section_id == obj.section_id).ToList()) == 1)
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
        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
        }
    }
}
