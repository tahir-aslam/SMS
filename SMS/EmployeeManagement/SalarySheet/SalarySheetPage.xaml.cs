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
using SMS.DAL;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Reporting.WinForms;
using MySql.Data.MySqlClient;

namespace SMS.EmployeeManagement.SalarySheet
{
    /// <summary>
    /// Interaction logic for SalarySheetPage.xaml
    /// </summary>
    public partial class SalarySheetPage : Page
    {
        MiscDAL miscDAL;
        EmployeesDAL empDAL;

        List<sms_months> months_list;
        List<sms_years> years_list;
        List<employees_types> emp_types_list;

        public SalarySheetPage()
        {
            InitializeComponent();

            miscDAL = new MiscDAL();
            empDAL = new EmployeesDAL();

            try
            {                
                months_list = miscDAL.get_all_months();
                years_list = miscDAL.get_all_years();

                months_list.Insert(0, new sms_months() { id = "-1", month_name = "--Select Month--" });
                month_cmb.ItemsSource = months_list;

                years_list.Insert(0, new sms_years() { id = -1, year = "--Select Year--" });
                year_cmb.ItemsSource = years_list;

                get_all_emp_types();
                emp_types_cmb.SelectedIndex = 0;
                emp_types_list.Insert(0, new employees_types() { emp_types = "---Select Category---", id = "-1" });
                emp_types_cmb.ItemsSource = emp_types_list;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            loadGrid();
        }

        public void loadGrid()
        {
            year_cmb.SelectedValue = DateTime.Now.Year;
            month_cmb.SelectedIndex = 0;

        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (month_cmb.SelectedItem != null)
            {
                loadReport();
            }
        }

        private void year_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (year_cmb.SelectedItem != null)
            {
                loadReport();
            }
        }

        public void loadReport() 
        {
            try
            {
                sms_months month_obj = month_cmb.SelectedItem as sms_months;
                sms_years year_obj = year_cmb.SelectedItem as sms_years;

                if (month_cmb.SelectedIndex != 0 && year_cmb.SelectedIndex != 0 && month_cmb.SelectedItem != null && year_cmb.SelectedItem != null)
                {
                    List<employees> employees_salary_list = empDAL.get_salary_sheet(Convert.ToInt32(month_obj.month_id), year_obj.id);
                    ReportDataSource emp = new ReportDataSource();
                    emp.Name = "emp";
                    if (emp_types_cmb.SelectedItem != null && emp_types_cmb.SelectedIndex != 0)
                    {
                        employees_types et = (employees_types)emp_types_cmb.SelectedItem;
                        emp.Value = employees_salary_list.Where(x => x.emp_type_id.ToString() == et.id.ToString());
                    }
                    else
                    {
                        emp.Value = employees_salary_list;
                    }


                    ReportDataSource ins = new ReportDataSource();
                    List<institute> ins_list = new List<institute>();
                    MainWindow.ins.date = DateTime.Now;
                    MainWindow.ins.month_name = month_obj.month_name;
                    MainWindow.ins.year = year_obj.year;
                    MainWindow.ins.page_no = 1;
                    ins_list.Add(MainWindow.ins);
                    ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                    ins.Value = ins_list;

                    this._reportViewer3.LocalReport.DataSources.Clear();
                    this._reportViewer3.LocalReport.DataSources.Add(emp);
                    this._reportViewer3.LocalReport.DataSources.Add(ins);
                    this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.EmployeeManagement.SalarySheet.SalarySheetReport.rdlc";

                    _reportViewer3.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }

        //---------------           Get All Employees types    ----------------------------------
        public void get_all_emp_types()
        {
            emp_types_list = new List<employees_types>();
            try
            {

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {

                        cmd.CommandText = "SELECT* FROM sms_emp_types";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees_types emp_types = new employees_types()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_types = Convert.ToString(reader["emp_types"].ToString()),
                            };
                            emp_types_list.Add(emp_types);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Employees Types DB not connected");
            }
        }

        private void emp_types_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (emp_types_cmb.SelectedItem != null)
            {
                loadReport();
            }
        }
    }
}
