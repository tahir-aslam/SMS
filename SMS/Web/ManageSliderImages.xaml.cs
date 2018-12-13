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
using MySql.Data.MySqlClient;
using System.IO;
using SMS.Models;
using Microsoft.Win32;

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for ManageSliderImages.xaml
    /// </summary>
    public partial class ManageSliderImages : Window
    {
        public List<web_slider_images> images_list;
        string FileName;
        home H;
        string insertion;

        public ManageSliderImages(home h)
        {
            InitializeComponent();
            H = h;
            load_grid();

            
        }

        public void load_grid() 
        {
            get_all_images();
            images_grid.ItemsSource = images_list;
        }

        //  ----------------------     Get All Images           -----------------------------------
        public void get_all_images() 
        {
            try 
            {
                images_list = new List<web_slider_images>();

                using(MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using(MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_slider_images";
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

                            web_slider_images web = new web_slider_images()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                image = img,
                            };
                            images_list.Add(web);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var frm = (SchoolWeb)this.Owner;
            if (frm != null)
                frm.home_btn.Click += new RoutedEventHandler(home_btn_Click);
        }

        void home_btn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void upload_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.Filter = "Image files | *.jpg";

                if (openFileDialog1.ShowDialog() == true)
                {
                    FileName = openFileDialog1.FileName;
                    
                    if (submit() > 0)
                    {
                        //MessageBox.Show("Added");
                        load_grid();
                        H.load_grid();
                    }

                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

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
                        cmd.CommandText = "insert into web_slider_images (image , created_by , date_time) values (@image , @created_by , @date_time)";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
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

        private void del_btn_click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            web_slider_images w = (web_slider_images)cmd.DataContext;
           // MessageBox.Show(w.id.ToString());
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Photo ?", "Delete Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Information);
            if (mbr == MessageBoxResult.Yes)
            {
                bool check;
                check = check_insertion(w.id);

                if (delete(w.id.ToString()) == 1)
                {
                    if (check == false)
                    {
                        insert_deleted(w.id);
                    }                    
                    load_grid();
                    H.load_grid();


                }
                else
                {
                    load_grid();
                    MessageBox.Show("OOPs! Theres is some problem");

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
                        cmd.CommandText = "insert into web_slider_images_deleted (id) values (@id)";
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
                        cmd.CommandText = "select insertion from web_slider_images where id = " + id;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            insertion = Convert.ToString(reader["insertion"].ToString());
                            if(insertion == "true")
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



                        cmd.CommandText = "Delete from web_slider_images where id=" + id;
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
