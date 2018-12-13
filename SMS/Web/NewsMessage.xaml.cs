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
using SMS.Web;
using SMS.Models;
using MySql.Data.MySqlClient;
using System.IO;

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for NewsMessage.xaml
    /// </summary>
    public partial class NewsMessage : Page
    {
        public List<web_messages> messages_list;
        NewsMessageForm nmf;
        web_messages obj;
        string mode;
        string insertion;

        public NewsMessage()
        {
            InitializeComponent();
            load_grid();
        }
        public void load_grid()
        {
         
            get_all_messages();
            message_grid.ItemsSource = messages_list;
            
        }

        //  ----------------------     Get All NEws and messages           -----------------------------------
        public void get_all_messages()
        {
            try
            {
                messages_list = new List<web_messages>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_messages ORDER BY date_time DESC";
                        con.Open();

                        Byte[] img;

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["image"] == "")
                            {
                                string path = "/SMS;component/images/Delete-icon.png";
                                img = File.ReadAllBytes(path);
                            }
                            else
                            {
                                img = (byte[])(reader["image"]);
                            }

                            web_messages wm = new web_messages()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                message_heading = Convert.ToString(reader["message_heading"]),
                                message_description = Convert.ToString(reader["message_description"]),
                                image = img,
                            };
                            messages_list.Add(wm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // add button click

        private void edit_btn_Click(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            nmf = new NewsMessageForm(mode, this, obj);
            nmf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            nmf.ShowDialog();
        }


        public void editing() 
        {
            web_messages obj = (web_messages)message_grid.SelectedItem;
            //Button cmd = (Button)sender;
            //obj = (web_messages)cmd.DataContext;


            if (obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                nmf = new NewsMessageForm(mode, this, obj);
                nmf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                nmf.ShowDialog();
            }
        }

        // edit button
        private void editbtn(object sender, RoutedEventArgs e)
        {
            editing();
        }


        // delete button
        private void del_btn(object sender, RoutedEventArgs e)
        {
            //Button cmd = (Button)sender;
            //web_messages wm = (web_messages)cmd.DataContext;

            obj = (web_messages)message_grid.SelectedItem;
            if (obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {

                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Message ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(obj.id);
                    if (delete() > 0)
                    {
                        if (check == false)
                        {
                            insert_deleted(obj.id);
                            load_grid();
                        }
                        
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
                        cmd.CommandText = "insert into web_messages_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from web_messages where id = " + id;
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
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }


        private void message_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            editing();
        }


        //refresh button
        private void refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            load_grid();
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



                        cmd.CommandText = "Delete from web_messages where id=" + obj.id;
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        try
                        {
                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

    }
}
