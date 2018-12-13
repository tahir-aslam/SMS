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
using MySql.Data.MySqlClient;
using SMS.Models;
using SMS.ClassManagement.Section;
using System.Data;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;

namespace SMS.ClassManagement.Section
{
    /// <summary>
    /// Interaction logic for SectionSearch.xaml
    /// </summary>
    public partial class SectionSearch : Page
     {
        List<sections> sections_list;
        SectionForm sf;
        sections row_obj;
        string mode;
        string insertion;
        string updation;

        public SectionSearch()
        {
            InitializeComponent();
            sections_list = new List<sections>();
            row_obj = new sections();
            SearchTextBox.Focus();
            load_grid();
        }

        public void load_grid()
        {
            sections_list.Clear();
            get_all_sections();
            section_grid.ItemsSource = sections_list;
            this.section_grid.Items.Refresh();
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            sf = new SectionForm(mode, this, row_obj);
            sf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            sf.ShowDialog();
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (sections)section_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(row_obj.id);
                    if (delete() == 1)
                    {
                        if (check == false)
                        {
                            insert_deleted(row_obj.id);
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

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
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
                        cmd.CommandText = "insert into sms_subjects_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_subjects where id = " + id;
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
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_subjects where id=" + row_obj.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i=Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return i;
        }

        //-------------     Editing          ---------------------------

        private void section_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        public void editing() 
        {
            row_obj = (sections)section_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                sf = new SectionForm(mode, this, row_obj);
                sf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                sf.ShowDialog();
            }
        }
        
        //-----------       Get All Sections Data    ----------------------

        public void get_all_sections()
        {


            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                    
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
                            };
                            sections_list.Add(section);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("DB not connected");
                    }

                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }
        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            section_grid.ItemsSource = sections_list.Where(x => x.section_name.ToUpper().StartsWith(v_search.ToUpper()));
            section_grid.Items.Refresh();
        }



        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 100, 100, 150 , 100 , 150 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }


        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Class Name", typeof(string));
            AddColumn(dataTable, "Section Name", typeof(string));
            AddColumn(dataTable, "Teacher Name", typeof(string));
            AddColumn(dataTable, "Created By", typeof(string));
            AddColumn(dataTable, "DateTime", typeof(string));
            


            foreach (sections c in sections_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = c.class_name.ToString();
                dataRow[1] = c.section_name.ToString();
                dataRow[2] = c.emp_name.ToString();
                dataRow[3] = c.created_by.ToString();
                dataRow[4] = c.date_time.ToString();

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }
        
    }
}
