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
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Controls.Primitives;

namespace SMS.FeeManagement.FeePrintSlip
{
    /// <summary>
    /// Interaction logic for FeePrint.xaml
    /// </summary>
    public partial class FeePrint : Window
    {
        private FeeSearch.FeeForm feeForm;
        private RenderTargetBitmap rtbmp;
        private Grid visual;


        public FeePrint(System.Windows.Controls.Image myImage)
        {
            
            // TODO: Complete member initialization
            


            // Add Image to the UI

            //Grid g = new Grid();
            //g.Children.Add(myImage);
            grid_visual1.Children.Add(myImage);
            
            //grid_visual1 = g;
            
            //StackPanel myStackPanel = new StackPanel();
            //myStackPanel.Children.Add(myImage);
            //this.img_visual1 = myStackPanel;
            //this.Content = myStackPanel;
           
            
            //this.img_visual1 = myImage;
        }

        public FeePrint(FeeSearch.FeeForm feeForm)
        {
            // TODO: Complete member initialization
            //this.feeForm = feeForm;
            //this.Content = null;
            //grid_visual1 = feeForm.visual;
            
        }

        public FeePrint(RenderTargetBitmap rtbmp)
        {
            // TODO: Complete member initialization
           
            img_visual1.Source = rtbmp;
            
        }

        public FeePrint()
        {
            // TODO: Complete member initialization
        }

        public FeePrint(Grid visual)
        {
            // TODO: Complete member initialization
            this.visual = visual;
            RenderTargetBitmap rtbmp = new RenderTargetBitmap(300, 650, 96, 96, PixelFormats.Default);
            rtbmp.Render(visual);
            img_visual1.Source = rtbmp;
        }
        public static BitmapSource ConvertBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
