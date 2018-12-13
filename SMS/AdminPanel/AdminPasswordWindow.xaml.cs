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

namespace SMS.AdminPanel
{
    /// <summary>
    /// Interaction logic for AdminPasswordWindow.xaml
    /// </summary>
    public partial class AdminPasswordWindow : Window
    {
        public AdminPasswordWindow()
        {
            InitializeComponent();
            pass_textbox.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (pass_textbox.Password == "7120020123")
                {
                    this.Close();
                    AdminWindow aw = new AdminWindow();
                    aw.ShowDialog();
                }
                else 
                {
                    MessageBox.Show("Please Enter Correct Password","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
        }
    }
}
