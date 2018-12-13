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
using SMS.Messaging.SavedSms;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using System.ComponentModel;

namespace SMS.Messaging.SavedSms
{
    /// <summary>
    /// Interaction logic for SavedSmsSearch.xaml
    /// </summary>
    public partial class SavedSmsSearch : Page
    {
        List<sms> sms_list;
        SavedSmsForm ssf;
        sms row_obj;
        string mode;
        string insertion;
        string updation;

        public SavedSmsSearch()
        {
            InitializeComponent();

            sms_list = new List<sms>();
            row_obj = new sms();
            SearchTextBox.Focus();
            load_grid();
            strength_textblock.Text = sms_grid.Items.Count.ToString();
        }
        public void load_grid()
        {
            sms_list.Clear();
            get_all_sms();
            sms_grid.ItemsSource = sms_list;
            this.sms_grid.Items.Refresh();
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            ssf = new SavedSmsForm(mode, this, row_obj);
            ssf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ssf.ShowDialog();
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (sms)sms_grid.SelectedItem;
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
                        cmd.CommandText = "insert into sms_saved_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_saved where id = " + id;
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
                    cmd.CommandText = "Delete from sms_saved where id=" + row_obj.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                    catch (Exception ex)
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
            row_obj = (sms)sms_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                ssf = new SavedSmsForm(mode, this, row_obj);
                ssf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ssf.ShowDialog();
            }
        }

        //-----------       Get All SMS     ----------------------

        public void get_all_sms()
        {
            sms_list = new List<sms>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_saved";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms sm = new sms()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                msg_name = Convert.ToString(reader["msg_name"].ToString()),
                                msg = Convert.ToString(reader["msg"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                            };
                            sms_list.Add(sm);

                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
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
            sms_grid.ItemsSource = sms_list.Where(x => x.msg_name.ToUpper().StartsWith(v_search.ToUpper()));
            sms_grid.Items.Refresh();
        }



        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 200, 450 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }


        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Message Heading", typeof(string));
            AddColumn(dataTable, "Message", typeof(string));
            



            foreach (sms c in sms_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = c.msg_name.ToString();
                dataRow[1] = c.msg.ToString();
                

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
                dpd.AddValueChanged(sms_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = sms_grid.Items.Count.ToString();
        }
    }
}
