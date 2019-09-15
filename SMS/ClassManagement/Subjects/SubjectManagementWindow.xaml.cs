using SMS.DAL;
using SMS.Models;
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

namespace SMS.ClassManagement.Subjects
{
    /// <summary>
    /// Interaction logic for SubjectManagementWindow.xaml
    /// </summary>
    public partial class SubjectManagementWindow : Window
    {
        SubjectManagementPage SP;
        List<classes> classes_list;
        List<employees> emp_list;
        List<sms_exams_subjects> subjects_list;
        subjects subjects_obj;
        subjects obj;
        string mode;
        SubjectsDAL subjectsDAL;
        EmployeesDAL empDAL;
        SubjectsDataGrid subjectsGrid;
        public SubjectManagementWindow(string m, SubjectManagementPage sp, subjects ob)
        {
            InitializeComponent();

            SP = sp;
            this.obj = ob;
            this.mode = m;

            subjectsDAL = new SubjectsDAL();
            empDAL = new EmployeesDAL();            

            v_class_cmb.Focus();
            LoadData();
            
        }

        void LoadData()
        {
            try
            {
                subjects_list = subjectsDAL.GetAllSubjects();                
                emp_list = empDAL.get_all_active_employees();

                subjectsGrid = new SubjectsDataGrid();
                subjectsGrid.emp_list = emp_list;
                subjectsGrid.subjects_list = subjects_list;

                v_subjectsSelection_Datagrid.DataContext = subjectsGrid;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void click_save(object sender, RoutedEventArgs e)
        {

        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {

        }
    }
}
