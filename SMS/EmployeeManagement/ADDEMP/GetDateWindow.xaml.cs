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
using SMS.EmployeeManagement.ADDEMP;

namespace SMS.EmployeeManagement.ADDEMP
{
    /// <summary>
    /// Interaction logic for GetDateWindow.xaml
    /// </summary>
    public partial class GetDateWindow : Window
    {
        AddEmpForm AEF;
        public GetDateWindow(AddEmpForm aef)
        {
            InitializeComponent();
            this.AEF = aef;
            date_textbox.SelectedDate = DateTime.Now;
        }

        private void withdraw_btn_Click(object sender, RoutedEventArgs e)
        {
             MessageBoxResult mbr = MessageBox.Show("Are You Sure ?", "Withdraw Confirmation", MessageBoxButton.YesNo);
             if (mbr == MessageBoxResult.Yes)
             {
                 DateTime dt = date_textbox.SelectedDate.Value;
                 AEF.leaving_date = dt;
                 AEF.isYes = true;
                 this.Close();
             }
             else 
             {
                 AEF.isYes = false;
                 this.Close();
             }
        }
    }
}
