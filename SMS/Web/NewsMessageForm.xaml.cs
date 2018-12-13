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
using Microsoft.Win32;
using SMS.Web;
using SMS.Models;
using System.IO;
using MySql.Data.MySqlClient;

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for NewsMessageForm.xaml
    /// </summary>
    public partial class NewsMessageForm : Window
    {
        string FileName = "";
        web_messages obj;
        web_messages message_obj;
        string mode;
        NewsMessage nm;

        public NewsMessageForm(string mode, NewsMessage NM, web_messages obj)
        {
            InitializeComponent();

            nm = NM;
            this.obj = obj;
            this.mode = mode;

            if (mode == "edit")
            {             
                fill_control();            
            }
        }

        // fill control

        public void fill_control() 
        {
            message_heading_tb.Text = obj.message_heading.ToString();
            message_desc_tb.Text = obj.message_description.ToString();
            MemoryStream stream = new MemoryStream(obj.image);
            message_img.Source = ByteToImage(obj.image);
        }

        public ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            try
            {

                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }



        // fill object

        public void fill_object() 
        {
            message_obj = new web_messages();
            if(mode == "edit")
            {
                message_obj.id = obj.id;
            }
            
            message_obj.message_heading = message_heading_tb.Text.Trim();
            message_obj.message_description = message_desc_tb.Text.Trim();
            message_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            message_obj.date_time = DateTime.Now;
            

        }


        // validation

        //------------------    Validation       -------------------------

        public bool validate()
        {


            if (message_heading_tb.Text.Trim().Length == 0)
            {
                message_heading_tb.Focus();
                string alertText = "Message Heading Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (message_desc_tb.Text.Trim().Length == 0)
            {
                message_desc_tb.Focus();
                string alertText = "Message Description Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }           

            else
            {
                return true;
            }

        }



        private void browse_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.Filter = "Image files | *.jpg";

                if (openFileDialog1.ShowDialog() == true)
                {
                    FileName = openFileDialog1.FileName;
                    message_img.Source = new BitmapImage(new Uri(openFileDialog1.FileName));

                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }


        //--------------           Save          ----------------------

        public void save()
        {
            fill_object();
            if (validate())
            {
                if (mode == "insert")
                {
                    if (submit() > 0)
                    {
                        
                        nm.load_grid();
                        MessageBox.Show("Record Added Successfully","Successfully Added",MessageBoxButton.OK,MessageBoxImage.Information);
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else if (mode == "edit")
                {
                    if (update() > 0)
                    {
                        
                        nm.load_grid();
                        MessageBox.Show("Record Updated Successfully");
                        this.Close();

                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }

                }
                else
                {
                    MessageBox.Show("mode not set");
                }

            }
        }




        //---------------           Submit Form    ----------------------------------

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
                    string path = "/SMS;component/images/Delete-icon.png";
                    ImageData = File.ReadAllBytes(FileName);
                }


                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO web_messages(message_heading,message_description,image,created_by,date_time) Values(@message_heading,@message_description,@image,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@message_heading", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_heading;
                        cmd.Parameters.Add("@message_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_description;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = message_obj.date_time;

                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;

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



            //---------------           Update Form        ---------------------------------

        public int update()
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
                    ImageData = obj.image;
                }
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_messages SET message_heading=@message_heading,message_description=@message_description,created_by=@created_by,date_time=@date_time,image=@image,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.id;
                        cmd.Parameters.Add("@message_heading", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_heading;
                        cmd.Parameters.Add("@message_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_description;

                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = message_obj.date_time;

                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        

        // save button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            save();
        }
    }
}
