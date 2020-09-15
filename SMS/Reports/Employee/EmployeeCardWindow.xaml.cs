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
using SMS.EmployeeManagement.ADDEMP;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using System.ComponentModel;
using SMS.DAL;
using Microsoft.Reporting.WinForms;


namespace SMS.Reports.Employee
{
    /// <summary>
    /// Interaction logic for EmployeeCardWindow.xaml
    /// </summary>
    public partial class EmployeeCardWindow : Window
    {
        List<employees> emp_list;     
        EmployeesDAL empDAL;
        bool check = false;

        public EmployeeCardWindow()
        {
            InitializeComponent();

            empDAL = new EmployeesDAL();            
            //SearchTextBox.Focus();
            load_grid();
            strength_textblock.Text = emp_grid.Items.Count.ToString();
        }
        public void load_grid()
        {
            try
            {
                emp_list = empDAL.get_all_active_employees();
                emp_grid.ItemsSource = emp_list;
                this.emp_grid.Items.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            emp_grid.ItemsSource = emp_list.Where(x => x.emp_name.ToUpper().StartsWith(v_search.ToUpper()));
            emp_grid.Items.Refresh();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            employees emp_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < emp_grid.Items.Count; i++)
            {
                emp_obj = (employees)emp_grid.Items[i];
                emp_obj.Checked = checkBox.IsChecked.Value;
            }
            emp_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            emp_grid.SelectedItem = e.Source;
            employees emp = new employees();
            emp = (employees)emp_grid.SelectedItem;
            foreach (employees ede in emp_list)
            {
                if (emp.id == ede.id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
                }
            }
        }

        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            check = false;
            foreach (employees emp in emp_list.Where(x => x.Checked == true))
            {
                check = true;
                break;
            }
            if (check)
            {
                foreach (var item in emp_list)
                {
                    item.institute_name = MainWindow.ins.institute_name;
                    item.institute_logo = MainWindow.ins.institute_logo;
                }

                ReportDataSource emp = new ReportDataSource();
                emp.Name = "emp";
                emp.Value = emp_list.Where(x => x.Checked == true);              
               

                this._reportViewer1.LocalReport.DataSources.Clear();
                this._reportViewer1.LocalReport.DataSources.Add(emp);
                this._reportViewer1.LocalReport.ReportEmbeddedResource = "SMS.Reports.Employee.EmployeeCardReport.rdlc";

                _reportViewer1.RefreshReport();

                report_viewer.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student To Update Fee", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
    }
}
