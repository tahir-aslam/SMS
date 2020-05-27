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
using SMS.Models;
using SUT.PrintEngine.Utils;
using System.Windows.Markup;
using System.Windows.Threading;

namespace SMS.AdmissionManagement.WithdrawAdmission
{
    /// <summary>
    /// Interaction logic for SLCWindow.xaml
    /// </summary>
    public partial class SLCWindow : Window
    {
        admission adm_obj;
        List<string> list_string;
        FixedDocument fd;
        Border br;
        public SLCWindow( admission adm, Visibility visibility)
        {
            InitializeComponent();
            this.adm_obj = adm;
            adm_obj.adm_fee = adm.adm_date.ToString("dd MMMM yyyy");
            adm_obj.reg_fee = adm.withdrawal_date.Value.ToString("dd MMMM yyyy")+"";
            adm_obj.exam_fee = adm.withdrawal_date.Value.ToString("MMMM yyyy") + ".";
            fill_control();
            this.DataContext = adm_obj;
            v_institute_grid.Visibility = visibility;
            
            //this.RemoveLogicalChild(slc_border);
            //this.RemoveVisualChild(slc_border);
            //PageContent pc = new PageContent();
            //FixedPage fp = new FixedPage();
            
            //fp.Children.Add(slc_border);
            
            //((IAddChild)pc).AddChild(fp);
            //fd_document.Pages.Add(pc);

            //var dispatcher = Dispatcher.CurrentDispatcher;
            //dispatcher.Invoke(
            //   DispatcherPriority.SystemIdle,
            //   new DispatcherOperationCallback(delegate { return null; }),
            //   null);



            
        }
    
        public void fill_control() 
        {
            adm_obj.image = MainWindow.ins.institute_logo;
            adm_obj.previous_school = MainWindow.ins.institute_name;
            adm_obj.tution_fee = MainWindow.session.session_name;
            
            adm_obj.security_fee = adm_obj.dob.ToString("dd MMMM yyyy"+".");

            //institue_logo_img.Source = MainWindow.ByteToImage(MainWindow.ins.institute_logo);
            //institute_name_tb.Text = MainWindow.ins.institute_name;
            //std_image.Source = MainWindow.ByteToImage(adm_obj.image);
            //std_name_tb.Text = adm_obj.std_name;
            //father_name_tb.Text = adm_obj.father_name;
            //class_name_tb.Text = adm_obj.class_name;
            //section_name_tb.Text = adm_obj.section_name;
            //adm_date_tb.Text = adm_obj.adm_date.ToString("dd MMM yyyy");
            //withdrw_date_tb.Text = DateTime.Now.ToString("dd MMM yyyy");
            //dob_tb.Text = adm_obj.dob.ToString("dd MMM yyyy");
            //date_now.Text = DateTime.Now.ToString("dd MMM yyyy");

        
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //print_btn.Visibility = Visibility.Hidden;
            //var visualSize = new Size(slc_grid.ActualWidth, slc_grid.ActualHeight);
            //var printControl = PrintControlFactory.Create(visualSize, slc_grid);
            //printControl.ShowPrintPreview();
            //this.Close();
        }
    }
}
