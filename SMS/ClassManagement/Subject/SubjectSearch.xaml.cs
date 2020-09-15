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
using SMS.ClassManagement.Subject;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;

namespace SMS.ClassManagement.Subject
{
    /// <summary>
    /// Interaction logic for SubjectSearch.xaml
    /// </summary>
    public partial class SubjectSearch : Page
    {
        List<subjects> subjects_list;
        SubjectForm SF;
        subjects obj;
        string mode;
        string insertion;
        string updation;

        public SubjectSearch()
        {
            InitializeComponent();
            subjects_list = new List<subjects>();
            obj = new subjects();
           
        }

        public void load_grid()
        {
            subjects_list.Clear();
            get_all_subjects();
            subjects_grid.ItemsSource = subjects_list;
            this.subjects_grid.Items.Refresh();
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            SF = new SubjectForm(mode, this, obj);
            SF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SF.ShowDialog();
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
            obj = (subjects)subjects_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                SF = new SubjectForm(mode, this, obj);
                SF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                SF.ShowDialog();
            }
        }

        //-------------     Delete          ---------------------------

        private void click_delete(object sender, RoutedEventArgs e)
        {
            obj = (subjects)subjects_grid.SelectedItem;
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
                        cmd.CommandText = "insert into sms_subject_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_subject where id = " + id;
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


                    
                    cmd.CommandText = "Delete from sms_subject where id=" + obj.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return i;
        }

        // ------------       Refresh        --------------------------------

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
        }



        //-----------       Get All Subjects Data    ----------------------

        public void get_all_subjects()
        {


            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    
                    cmd.CommandText = "SELECT* FROM sms_subject";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            subjects subject = new subjects()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                subject_name = Convert.ToString(reader["subject_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                is_Active = Convert.ToString(reader["is_Active"].ToString()),
                                total_marks = Convert.ToString(reader["total_marks"].ToString()),
                                remarks = Convert.ToString(reader["remarks"].ToString()),


                            };
                            subjects_list.Add(subject);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("Subjects DB not connected");
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
                subjects_grid.ItemsSource = subjects_list.Where(x => x.subject_name.ToUpper().StartsWith(v_search.ToUpper()));
                subjects_grid.Items.Refresh();
            
        }

        // =======================         printing        ======================================

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 100, 100, 150, 100, 150 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }
        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Class Name", typeof(string));
            AddColumn(dataTable, "Subject Name", typeof(string));
            AddColumn(dataTable, "Teacher Name", typeof(string));
            AddColumn(dataTable, "Created By", typeof(string));
            AddColumn(dataTable, "DateTime", typeof(string));



            foreach (subjects c in subjects_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = c.class_name.ToString();
                dataRow[1] = c.subject_name.ToString();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Focus();
            load_grid();
        }
    }
}
