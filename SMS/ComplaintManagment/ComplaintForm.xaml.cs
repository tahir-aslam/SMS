using SMS.Common;
using SMS.DAL;
using SMS.Models;
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

namespace SMS.ComplaintManagment
{
    /// <summary>
    /// Interaction logic for ComplaintForm.xaml
    /// </summary>
    public partial class ComplaintForm : Window
    {
        public static List<admission> adm_list;
        AdmissionDAL admissionDAL;

        public ComplaintForm(string mode, ComplaintRegister search, sms_complaint_register obj)
        {
            InitializeComponent();

            admissionDAL = new AdmissionDAL();

            if (mode == "insert")
            {
                std_name_TB.Visibility = Visibility.Collapsed;                
            }
            else
            {
                std_count_SP.Visibility = Visibility.Collapsed;
                std_SP.Visibility = Visibility.Collapsed;                
                std_name_TB.Visibility = Visibility.Visible;                                
                std_name_TB.Text = obj.std_name;
            }
        }

        private void allOption_Checked(object sender, RoutedEventArgs e)
        {
            adm_list = admissionDAL.get_all_admissions();
            selectedStudentCountTB.Text = adm_list.Count.ToString();
        }
        private void selectedOption_Checked(object sender, RoutedEventArgs e)
        {
            StudentSelectionWindowNew ssw = new StudentSelectionWindowNew();
            ssw.ShowDialog();
            adm_list = ssw.adm_list.Where(x => x.Checked == true).ToList();
            selectedStudentCountTB.Text = adm_list.Count.ToString();
        }

    }
}
