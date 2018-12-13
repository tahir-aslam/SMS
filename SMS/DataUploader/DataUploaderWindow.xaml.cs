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

namespace SMS.DataUploader
{
    /// <summary>
    /// Interaction logic for DataUploaderWindow.xaml
    /// </summary>
    public partial class DataUploaderWindow : Window
    {
        public static bool window_open;
        public static DataUploaderWindow duw;
        public DataUploaderWindow()
        {
            InitializeComponent();
            window_open = true;
            duw = this;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            window_open = false;
        }
    }
}
