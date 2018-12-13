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
using System.Windows.Threading;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System.IO;
using SMS.Web;
using SMS.Models;

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for home.xaml
    /// </summary>
    public partial class home : Page
    {
        DispatcherTimer timer;
        int ctr = 0;
        string FileName;
        public List<web_slider_images> images_list;

        public home()
        {
            InitializeComponent();

            load_grid();
            
            
            
        }

        public void load_grid() 
        {
            
            get_all_images();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = images_list.Count-1;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, images_list.Count);
            timer.Tick += new EventHandler(timer_Tick);


            chkAutoPlay.IsChecked = true;
            timer.IsEnabled = chkAutoPlay.IsChecked.Value;
            ctr = 0;
            if(images_list.Count>0)
            {
                PlaySlideShow(ctr);
            }
            
        }

       private void timer_Tick(object sender, EventArgs e)
        {
            ctr++;
            if (ctr > images_list.Count-1)
            {
                ctr = 0;
            }
            if (images_list.Count > 0)
            {
                PlaySlideShow(ctr);
            }
            
        }

       private void PlaySlideShow(int ctr)
       {
           //BitmapImage image = new BitmapImage();
           //image.BeginInit();
           //string filename = ((ctr < 10) ? "/SMS;component/sliderimages/Plane0" + ctr + ".jpeg" : "/SMS;component/sliderimages/Plane" + ctr + ".jpeg");
           web_slider_images w = (web_slider_images)images_list[ctr];
           //string filename = w.image.ToString();
           //image.UriSource = new Uri(filename, UriKind.Relative);
           //image.EndInit();
           MemoryStream ms = new MemoryStream(w.image);
           BitmapImage image = new BitmapImage();
           image.BeginInit();
           image.StreamSource = ms;
           image.EndInit();

           myImage.Source = image;
           myImage.Stretch = Stretch.Uniform;
           progressBar1.Value = ctr;
       }

       private void btnFirst_Click(object sender, RoutedEventArgs e)
       {
           if (images_list.Count > 0)
           {
               ctr = 0;
               PlaySlideShow(ctr);
           }
           
       }

       private void btnPrevious_Click(object sender, RoutedEventArgs e)
       {
           if (images_list.Count > 0)
           {
               ctr--;
               if (ctr < 0)
               {
                   ctr = images_list.Count - 1;
               }
               PlaySlideShow(ctr);
           }
       }

       private void btnNext_Click(object sender, RoutedEventArgs e)
       {
           if (images_list.Count > 0)
           {
               ctr++;
               if (ctr > images_list.Count - 1)
               {
                   ctr = 0;
               }
               PlaySlideShow(ctr);
           }
       }

       private void btnLast_Click(object sender, RoutedEventArgs e)
       {
           if (images_list.Count > 0)
           {
               ctr = images_list.Count - 1;
               PlaySlideShow(ctr);
           }
       }
       private void chkAutoPlay_Click(object sender, RoutedEventArgs e)
       {
           auto_play();
       }

       private void auto_play() 
       {
           timer.IsEnabled = chkAutoPlay.IsChecked.Value;
           //btnFirst.Visibility = (btnFirst.IsVisible == true) ? Visibility.Hidden : Visibility.Visible;
           //btnPrevious.Visibility = (btnPrevious.IsVisible == true) ? Visibility.Hidden : Visibility.Visible;
           //btnNext.Visibility = (btnNext.IsVisible == true) ? Visibility.Hidden : Visibility.Visible;
           //btnLast.Visibility = (btnLast.IsVisible == true) ? Visibility.Hidden : Visibility.Visible;
       }
       private void Grid_Loaded(object sender, RoutedEventArgs e)
       {
           
       }

       private void slider_img_btn_Click(object sender, RoutedEventArgs e)
       {
           
       }
      
       private void manage_slider_btn_Click(object sender, RoutedEventArgs e)
       {
           ManageSliderImages msi = new ManageSliderImages(this);
           msi.ShowDialog();
       }

       //  ----------------------     Get All Images           -----------------------------------
       public void get_all_images()
       {
           try
           {
               images_list = new List<web_slider_images>();

               using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
               {
                   using (MySqlCommand cmd = new MySqlCommand())
                   {
                       cmd.Connection = con;
                       cmd.CommandText = "select * from web_slider_images ";
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

                               image = img,
                           };
                           images_list.Add(web);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
       }
    }
}
