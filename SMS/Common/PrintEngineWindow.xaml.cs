using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SUT.PrintEngine.Utils;

namespace SMS.Common
{
    /// <summary>
    /// Interaction logic for PrintEngineWindow.xaml
    /// </summary>
    public partial class PrintEngineWindow : Window
    {
        Size Size;
        Visual Visual;
        public PrintEngineWindow(Size size, Visual visual)
        {
            InitializeComponent();
            this.Visual = visual;
            this.Size = size;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            var printControl = PrintControlFactory.Create(Size, Visual);
            printControl.ShowPrintPreview();
        }
    }
}
