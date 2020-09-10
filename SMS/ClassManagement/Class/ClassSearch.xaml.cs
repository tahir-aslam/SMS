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
using SMS.ClassManagement.Class;
using SMS.MainScreen;
using MySql.Data.MySqlClient;
using SMS.Models;
using SUT.PrintEngine;
using SUT.PrintEngine.Utils;
using System.Data;
using System.Windows.Markup;
using SMS.DAL;


namespace SMS.ClassManagement.Class
{
    /// <summary>
    /// Interaction logic for ClassSearch.xaml
    /// </summary>
    public partial class ClassSearch : Page
    {
       List<classes> classes_list;
       classes obj;
       sms_fees_actual classes_fees_obj;
       string mode;
       //ClassForm cf;
       ClassFormNew cfn;
       ClassChargesForm ccf;
       string insertion;
       string updation;
       ClassesDAL classesDAL;
       public List<sms_fees_actual> classes_fees_list;

        public ClassSearch()
        {
            InitializeComponent();

            
            
            
        }

        public void load_gird() 
        {
            classes_list.Clear();
            get_all_classes();
            classes_grid.ItemsSource = classes_list;
            this.classes_grid.Items.Refresh();
            
        }

        public void load_charges_grid() 
        {
            classes_fees_list = classesDAL.getAllFeesClasses();
            chargesGrid.ItemsSource = classes_fees_list.OrderByDescending(x=>x.class_id);
            //chargesGrid.Items.Refresh();

            List<classes> classList = classesDAL.getAllClasses();
            classList.Insert(0, new classes() { id = "-1", class_name = "--Select Class--" });
            class_cmb.ItemsSource = classList;
            class_cmb.SelectedIndex = 0;
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            cfn = new ClassFormNew(mode, this, obj);
            cfn.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            cfn.ShowDialog();
            
        }
        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void click_delete(object sender, RoutedEventArgs e)
        {
            obj = (classes)classes_grid.SelectedItem;
            if (obj == null)
            {
                MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Warning);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(obj.id);
                    if (delete() == 1)
                    {
                        if (check == false)
                        {
                            insert_deleted(obj.id);
                        }
                        load_gird();
                    }
                    else
                    {
                        load_gird();
                        MessageBox.Show("OOPs! Theres is some problem");

                    }
                }
            }
        }
        private void click_refresh(object sender, RoutedEventArgs e)
        {

            load_gird();
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
                        cmd.CommandText = "insert into sms_classes_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_classes where id = " + id;
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


        //-------------     Delete          ---------------------------

        public int delete()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {



                        cmd.CommandText = "Delete from sms_classes where id=" + obj.id;
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
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }
    //------------            Editing        ----------------------------

        public void editing()
        {
            obj = (classes)classes_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                cfn = new ClassFormNew(mode, this, obj);
                cfn.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                cfn.ShowDialog();
            }
        }

        public void editing_charges()
        {
            classes_fees_obj = (sms_fees_actual)chargesGrid.SelectedItem;
            if (classes_fees_obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                ccf = new ClassChargesForm(mode, this, classes_fees_obj);
                ccf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ccf.ShowDialog();
            }
        }

//---------------      Get All Classes            -------------------------------

        public void get_all_classes() 
        {
            
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                transport_charges = Convert.ToString(reader["transport_charges"].ToString()),
                                boarding_fee = Convert.ToString(reader["boarding_fee"].ToString()),
                                misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                stationary_charges = Convert.ToString(reader["stationary_charges"].ToString()),
                                books_charges = Convert.ToString(reader["books_charges"].ToString()),
                                other_exp = Convert.ToString(reader["other_exp"].ToString()),
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            

                            };
                            classes_list.Add(classes);
                        
                        }

                       
                    }
                }
            }
            catch
            {
                MessageBox.Show("Classes DB not connected");
            }
        }

        public void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            search_box();
        }
        public void search_box() 
        {
            string v_search = SearchTextBox.Text;
            classes_grid.ItemsSource = classes_list.Where(x => x.class_name.ToUpper().StartsWith(v_search.ToUpper()));
            classes_grid.Items.Refresh();
        }

        private void classes_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }


        //===========         Printing           ==============================================

        private void print_button_Click(object sender, RoutedEventArgs e)
        {
            //classReport cr = new classReport();
            //ClassReportWF crwf = new ClassReportWF(classes_list);
            //crwf.ShowDialog();
            //crystalReportViewer1.ReportSource = cr;
           // cr.SetDataSource(GetStudentList());

            //classReportViewerForm crvf = new classReportViewerForm();
            //crvf.Show();
            //var visualSize = new Size(visual.ActualWidth, visual.ActualHeight);
            //var printControl = PrintControlFactory.Create(visualSize, visual);

            //printControl.ShowPrintPreview();

            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 100, 100, 100, 100, 100, 100 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }

        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Class Name", typeof(string));
            AddColumn(dataTable, "Annual Fund", typeof(string));
            AddColumn(dataTable, "Admission Fee", typeof(string));
            AddColumn(dataTable, "Other", typeof(string));
            AddColumn(dataTable, "Tution Fee", typeof(string));
            AddColumn(dataTable, "Roll No Format", typeof(string));
            

            foreach (classes c in classes_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = c.class_name.ToString();
                dataRow[1] = c.reg_fee.ToString();
                dataRow[2] = c.adm_fee.ToString();
                dataRow[3] = c.other_exp.ToString();
                dataRow[4] = c.tution_fee.ToString();
                dataRow[5] = c.roll_no_format.ToString();
                

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        //-----------------------------------------------------------------
       
        public void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void add_record_charges_Click(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            classes_fees_obj = null;
            ccf = new ClassChargesForm(mode, this, classes_fees_obj);
            ccf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ccf.ShowDialog();
        }

        private void edit_record_charges_Click(object sender, RoutedEventArgs e)
        {
            editing_charges();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refresh_record_charges_Click(object sender, RoutedEventArgs e)
        {
            load_charges_grid();
        }

        private void print_button_charges_Click(object sender, RoutedEventArgs e)
        {
            ClassReportWindow window = new ClassReportWindow(classes_fees_list);
            window.Show();
        }

        private void chargesGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing_charges();
        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb.SelectedItem != null)
            {
                classes cl = (classes)class_cmb.SelectedItem;
                if (class_cmb.SelectedIndex == 0)
                {
                    chargesGrid.ItemsSource = classes_fees_list;
                    chargesGrid.Items.Refresh();
                }
                else 
                {
                    chargesGrid.ItemsSource = classes_fees_list.Where(x => x.class_id == Convert.ToInt32(cl.id));
                    chargesGrid.Items.Refresh();
                }             
            }
        } 

        public bool checkDuplicateEntry(sms_fees_actual obj)
        {
            if(classes_fees_list.Where(x=>x.class_id == obj.class_id).Where(x=>x.fees_category_id == obj.fees_category_id).Where(x=>x.fees_sub_category_id == obj.fees_sub_category_id).Count() > 0)
            {
                return true;
            }
            return false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            classes_list = new List<classes>();
            SearchTextBox.Focus();
            classesDAL = new ClassesDAL();

            load_gird();
            load_charges_grid();
        }
    }
    
}
