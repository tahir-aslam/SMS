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
using Microsoft.Reporting.WinForms;

namespace SMS.ClassManagement.Class
{
    /// <summary>
    /// Interaction logic for ClassReportWindow.xaml
    /// </summary>
    public partial class ClassReportWindow : Window
    {
        List<sms_fees_actual> classes_list;

        public ClassReportWindow(List<sms_fees_actual> lst)
        {
            InitializeComponent();
            classes_list = lst;
            _reportViewer.Load += _reportViewer_Load;
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource classes = new ReportDataSource();
            classes.Name = "classes"; //Name of the report dataset in our .RDLC file
            classes.Value = classes_list;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            this._reportViewer.LocalReport.DataSources.Add(classes);
            this._reportViewer.LocalReport.DataSources.Add(ins);
            this._reportViewer.LocalReport.ReportPath = "ClassManagement/Class/ClassReport.rdlc";

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
