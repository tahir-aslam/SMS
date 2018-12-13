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
using SMS.Controls;
using SMS.Models;

namespace SMS.Upload
{
    /// <summary>
    /// Interaction logic for UploadWindow.xaml
    /// </summary>
    public partial class UploadWindow : Window
    {
        public static List<admission> std_nos_sms;
        public static bool isEncoded = false;

        public UploadWindow(List<admission> std_nos, bool isEncode)
        {            
            InitializeComponent();
            std_nos_sms = new List<admission>();
            std_nos_sms = std_nos;
            isEncoded = isEncode;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                smsEngineObj.comm.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
