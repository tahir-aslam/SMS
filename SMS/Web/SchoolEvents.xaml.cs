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
using Microsoft.Win32;

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for SchoolEvents.xaml
    /// </summary>
    public partial class SchoolEvents : Page
    {
        List<web_events> events_list;
        List<web_event_image> events_images_list;
        List<web_events> events_images_mashup_list;
        string FileName = "";
        SchoolEventForm sef;
        web_events obj;
        string mode;
        int selected_id = 0;
        string insertion;

        public SchoolEvents()
        {
            InitializeComponent();
            load_grid();
            
        }
        public void load_grid() 
        {
            get_all_events();
            get_all_images();
            mashup();

            events_grid.ItemsSource = events_images_mashup_list;
            events_grid.SelectedIndex = 0;
            events_grid.SelectedIndex = selected_id;
        }
        

        // Mashup

        public void mashup() 
        {
            events_images_mashup_list = new List<web_events>();
            foreach(web_events we in events_list)
            {
                web_events wem = new web_events();
                wem.id = we.id;
                wem.event_date = we.event_date;
                wem.event_name = we.event_name;
                wem.event_description = we.event_description;
                wem.created_by = we.created_by;
                wem.date_time = we.date_time;

                wem.event_images_lst = new List<web_event_image>();
                foreach(web_event_image wei in events_images_list.Where(x=>x.id == we.id))
                {
                    web_event_image w = new web_event_image();
                    w.image = wei.image;
                    //we.event_images_lst.Add(wei);
                    wem.event_images_lst.Add(w);

                }
                events_images_mashup_list.Add(wem);
            }
        }

        //  ----------------------     Get All Events            -----------------------------------
        public void get_all_events()
        {
            try
            {
                events_list = new List<web_events>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_events";
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            web_events w_e = new web_events()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                event_date=Convert.ToDateTime(reader["event_date"]),
                                event_name = Convert.ToString(reader["event_name"].ToString()),
                                event_description = Convert.ToString(reader["event_description"].ToString()),
                                date_time = Convert.ToDateTime(reader["event_date"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                               
                            };
                            events_list.Add(w_e);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        //  ----------------------     Get All Images           -----------------------------------
        public void get_all_images()
        {
            try
            {
                events_images_list = new List<web_event_image>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_events_images";
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

                            web_event_image web = new web_event_image()
                            {
                                id = Convert.ToString(reader["id"].ToString()),

                                image = img,
                                image_id = Convert.ToString(reader["image_id"].ToString())
                            };
                            events_images_list.Add(web);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void add_btn_Click(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            sef = new SchoolEventForm(mode, this, obj);
            sef.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            sef.ShowDialog();
        }

        private void edit_btn_Click(object sender, RoutedEventArgs e)
        {
            editing();
        }

        // Event Deleted
        private void del_btn_Click(object sender, RoutedEventArgs e)
        {
            web_events we;
            try
            {

                //Button cmd = (Button)sender;
                //web_event_image wei = (web_event_image)cmd.DataContext;

                we = (web_events)events_grid.SelectedItem;                
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Photo ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion_events(we.id);

                    if (delete_events(we.id.ToString()) == 1)
                    {
                        if (check == false)
                        {
                            insert_deleted_events(we.id);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //insert deleted events

        public void insert_deleted_events(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into web_events_deleted (id) values (@id)";
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
        

        //-------------     Delete  events        ---------------------------

        public int delete_events(string id)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {



                        cmd.CommandText = "Delete from web_events where id=" + id;
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


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        // ---------     Check Insertion events          --------------------------------------

        public bool check_insertion_events(string id)
        {
            try
            {


                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select insertion from web_events where id = " + id;
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

        private void refresh_btn_Click(object sender, RoutedEventArgs e)
        {
            load_grid();
        }

        // editing

        public void editing()
        {
            obj = (web_events)events_grid.SelectedItem;
            if (obj == null)
            {
                MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                sef = new SchoolEventForm(mode, this, obj);
                sef.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                sef.ShowDialog();
            }
        }


        //===========================        Add Photos events images           ==================================================

        private void add_photos_Click(object sender, RoutedEventArgs e)
        {
            //Button cmd = (Button)sender;
            //obj = (web_events)cmd.DataContext;
            obj = (web_events)events_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //MessageBox.Show(obj.id);
                //event_images_grid.ItemsSource = events_images_list.Where(x => x.id == obj.id);

                //MessageBox.Show(w.id);
                selected_id = Convert.ToInt32(obj.id);
                try
                {

                    OpenFileDialog openFileDialog1 = new OpenFileDialog();

                    openFileDialog1.Filter = "Image files | *.jpg";

                    if (openFileDialog1.ShowDialog() == true)
                    {
                        FileName = openFileDialog1.FileName;
                        int i = submit();
                        if (i > 0)
                        {
                            //MessageBox.Show("Added");
                            load_grid();
                            //events_grid.SelectedIndex = 0;
                            events_grid.SelectedIndex = Convert.ToInt32(selected_id);
                            //event_images_grid.SelectedValue = i;
                            
                        }
                        else
                        {
                            load_grid();
                        }

                    }

                }

                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);

                }
            }
        }

        public int submit()
        {
            int i = 0;
            try
            {
                FileStream fs;
                BinaryReader br;
                byte[] ImageData;

                if (FileName != "")
                {
                    fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);
                    ImageData = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();
                }
                else
                {
                    MessageBox.Show("There is some Error");
                    ImageData = File.ReadAllBytes(FileName);
                }

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into web_events_images (id,image) values (@id , @image); SELECT LAST_INSERT_ID()";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;
                        

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        

        private void events_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            obj = (web_events)events_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //MessageBox.Show(obj.id);
                event_images_grid.ItemsSource = events_images_list.Where(x=>x.id == obj.id);
            }
        }


        // ==================         Photos delete events images             =====================================================
        private void photo_deleted_click(object sender, RoutedEventArgs e)
        {
            web_event_image wei;
            try
            {

                //Button cmd = (Button)sender;
                //web_event_image wei = (web_event_image)cmd.DataContext;
                
                wei = (web_event_image)event_images_grid.SelectedItem;
               // MessageBox.Show(wei.image_id);
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Photo ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(wei.image_id);

                    if (delete(wei.image_id.ToString()) == 1)
                    {
                        if (check == false)
                        {
                            insert_deleted(wei.image_id);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //insert deleted

        public void insert_deleted(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into web_events_images_deleted (id) values (@id)";
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




        //-------------     Delete          ---------------------------

        public int delete(string id)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {



                        cmd.CommandText = "Delete from web_events_images where image_id=" + id;
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


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
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
                        cmd.CommandText = "select insertion from web_events_images where image_id = " + id;
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

    }
}
