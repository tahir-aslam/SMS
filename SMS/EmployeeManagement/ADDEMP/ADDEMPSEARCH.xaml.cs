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
using SMS.EmployeeManagement.ADDEMP;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using System.ComponentModel;
using SMS.DAL;

namespace SMS.EmployeeManagement.ADDEMP
{
    /// <summary>
    /// Interaction logic for ADDEMPSEARCH.xaml
    /// </summary>
    public partial class ADDEMPSEARCH : Page
    {
        List<employees> emp_list;
        AddEmpForm AEF;
        employees obj;
        string mode;
        string insertion;
        EmployeesDAL empDAL;

        public ADDEMPSEARCH()
        {
            InitializeComponent();

            empDAL = new EmployeesDAL();            
            obj = new employees();
            //SearchTextBox.Focus();
            load_grid();
            strength_textblock.Text = emp_grid.Items.Count.ToString();
        }

        public void load_grid() 
        {
            try
            {                
                emp_list = new List<employees>();
                emp_list = empDAL.get_all_active_employees();
                emp_grid.ItemsSource = emp_list;
                this.emp_grid.Items.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            AEF = new AddEmpForm(mode, this, obj);
            AEF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AEF.ShowDialog();
        }

        //-------------     Editing          ---------------------------

        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }

        private void emp_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            obj = (employees)emp_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                AEF = new AddEmpForm(mode, this, obj);
                AEF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                AEF.ShowDialog();
            }
        }

        //-------------     Delete          ---------------------------

        private void click_delete(object sender, RoutedEventArgs e)
        {
            obj = (employees)emp_grid.SelectedItem;
            if (obj == null)
            {
                MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(obj.id);
                    if (delete() == 1)
                    {
                        if (check == false)
                        {
                            insert_deleted(obj.id);
                        }
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

        //  ----------------------insert deleted------------------------------

        public void insert_deleted(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into sms_emp_deleted (id) values (@id)";
                        cmd.Connection = con;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;


                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // ---------     Check Insertion           --------------------------------------

        public bool check_insertion(string id)
        {
            try
            {


                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select insertion from sms_emp where id = " + id;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            insertion = Convert.ToString(reader["insertion"].ToString());
                            if (insertion == "true")
                            {
                                return true;

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }


        public int delete()
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {



                    cmd.CommandText = "Delete from sms_emp where id=" + obj.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return i;
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
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

        // ==============        Printing        ==============================================
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var dataTable = CreateSampleDataTable();
            //var columnWidths = new List<double>() {40, 150, 150, 100, 100, 130, 100, 200 };
            //var ht = new HeaderTemplate();
            //var headerTemplate = XamlWriter.Save(ht);
            //var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            //printControl.ShowPrintPreview();
            EmployeeReportWindow window = new EmployeeReportWindow(emp_list);
            window.Show();
        }

        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();
            int i = 0;

            AddColumn(dataTable, "Sr#", typeof(string));
            AddColumn(dataTable, "Employee Name", typeof(string));
            AddColumn(dataTable, "Father Name", typeof(string));
            AddColumn(dataTable, "Emp Type", typeof(string));
            AddColumn(dataTable, "Cell#", typeof(string));
            AddColumn(dataTable, "Qualification", typeof(string));
            AddColumn(dataTable, "CNIC", typeof(string));
            AddColumn(dataTable, "Address", typeof(string));
            
            foreach (employees c in emp_list)
            {
                i++;
                var dataRow = dataTable.NewRow();
                dataRow[0] = i.ToString();
                dataRow[1] = c.emp_name.ToString();
                dataRow[2] = c.emp_father.ToString();
                dataRow[3] = c.emp_type.ToString();                
                dataRow[4] = c.emp_cell.ToString();
                dataRow[5] = c.emp_qual.ToString();
                dataRow[6] = c.emp_cnic.ToString();
                dataRow[7] = c.emp_address.ToString();
                

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(emp_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = emp_grid.Items.Count.ToString();
        }
    }
}
