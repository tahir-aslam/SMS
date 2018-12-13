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
using SMS.Messaging.FriendList;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using System.ComponentModel;

namespace SMS.Messaging.FriendList
{
    /// <summary>
    /// Interaction logic for FriendListSearch.xaml
    /// </summary>
    public partial class FriendListSearch : Page
    {
        List<friend_list> friends_list;
        FriendListForm flf;
        friend_list row_obj;
        string mode;
        string insertion;
        string updation;

        public FriendListSearch()
        {
            InitializeComponent();

            friends_list = new List<friend_list>();
            row_obj = new friend_list();
            SearchTextBox.Focus();
            load_grid();
            strength_textblock.Text = friends_grid.Items.Count.ToString();
        }

        public void load_grid()
        {
            friends_list.Clear();
            get_all_friends();
            friends_grid.ItemsSource = friends_list;
            this.friends_grid.Items.Refresh();
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            row_obj = null;
            flf = new FriendListForm(mode, this, row_obj);
            flf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            flf.ShowDialog();
        }

        private void click_delete(object sender, RoutedEventArgs e)
        {
            row_obj = (friend_list)friends_grid.SelectedItem;
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
                        cmd.CommandText = "insert into sms_friend_list_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from sms_friend_list where id = " + id;
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
                    cmd.CommandText = "Delete from sms_friend_list where id=" + row_obj.id;
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
            row_obj = (friend_list)friends_grid.SelectedItem;
            if (row_obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                flf = new FriendListForm(mode, this, row_obj);
                flf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                flf.ShowDialog();
            }
        }

        //-----------       Get All Friends     ----------------------

        public void get_all_friends()
        {
            friends_list = new List<friend_list>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_friend_list";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            friend_list fl = new friend_list()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                friend_name = Convert.ToString(reader["friend_name"].ToString()),
                                friend_cell = Convert.ToString(reader["friend_cell"].ToString()),
                                friend_occupation = Convert.ToString(reader["friend_occupation"].ToString()),
                                freind_type_id = Convert.ToInt32(reader["freind_type_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                            };
                            friends_list.Add(fl);

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
            friends_grid.ItemsSource = friends_list.Where(x => x.friend_name.ToUpper().StartsWith(v_search.ToUpper()));
            friends_grid.Items.Refresh();
        }



        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 300, 150, 200 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }


        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Name", typeof(string));
            AddColumn(dataTable, "cell#", typeof(string));
            AddColumn(dataTable, "Occupation", typeof(string));




            foreach (friend_list c in friends_list)
            {
                var dataRow = dataTable.NewRow();
                dataRow[0] = c.friend_name.ToString();
                dataRow[1] = c.friend_cell.ToString();
                dataRow[2] = c.friend_occupation.ToString();
               


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
                dpd.AddValueChanged(friends_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = friends_grid.Items.Count.ToString();
        }
    }
}
