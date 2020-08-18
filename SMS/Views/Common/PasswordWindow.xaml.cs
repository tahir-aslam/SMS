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

namespace SMS.Views.Common
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
            txtPassword.Focus();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            MatchPassword();
        }

        private void btnCance_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).DialogResult = false;
            Window.GetWindow(this).Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MatchPassword();
            }
        }

        void MatchPassword()
        {
            if (txtPassword.Password == MainWindow.emp_login_obj.emp_pwd)
            {
                Window.GetWindow(this).DialogResult = true;
                Window.GetWindow(this).Close();
            }
            else
            {
                MessageBox.Show("Wrong Password.");
            }
        }
    }
}

