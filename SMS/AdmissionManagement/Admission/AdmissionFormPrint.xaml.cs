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
using System.Windows.Markup;

namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for AdmissionFormPrint.xaml
    /// </summary>
    public partial class AdmissionFormPrint : Window
    {

        admission adm_obj;
        public AdmissionFormPrint(admission adm)
        {
            InitializeComponent();
            adm_obj = new admission();
            adm_obj.institute_logo = MainWindow.ins.institute_logo;
            adm_obj.date_time = DateTime.Now;
            adm_obj.institute_name = MainWindow.ins.institute_name;           

            if (adm != null)
            {                
                this.adm_obj = adm;
                adm_obj.institute_logo = MainWindow.ins.institute_logo;
                adm_obj.date_time = DateTime.Now;
                adm_obj.institute_name = MainWindow.ins.institute_name;           
            }                
            this.DataContext = adm_obj;            
        }

        public void fill_control()
        {  
        }
    }
}
