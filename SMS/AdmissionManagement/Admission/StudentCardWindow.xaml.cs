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
using SUT.PrintEngine.Utils;
using SMS.Models;
using System.IO;

namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for StudentCardWindow.xaml
    /// </summary>
    public partial class StudentCardWindow : Window
    {
        admission adm_obj;
        public StudentCardWindow(admission obj)
        {
            InitializeComponent();
            adm_obj = obj;
            fill_control();
                
        }

        public void fill_control() 
        {
            MainWindow mw = new MainWindow();
            ins_logo.Source = ByteToImage(MainWindow.ins.institute_logo);
            institute_name.Text = MainWindow.ins.institute_name;
            std_img.Source = ByteToImage(adm_obj.image);
            std_name_tb.Text = adm_obj.std_name;
            father_name_tb.Text = adm_obj.father_name;
            class_tb.Text = adm_obj.class_name;
            section_tb.Text = adm_obj.section_name;
            address_tb.Text = adm_obj.parmanent_adress;
            adm_no_tb.Text = adm_obj.adm_no;
            stdBarCode.Code = adm_obj.adm_no;
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var visualSize = new Size(std_card_sp.ActualWidth, std_card_sp.ActualHeight);
            var printControl = PrintControlFactory.Create(visualSize, std_card_sp);
            printControl.ShowPrintPreview();
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
    }
}
