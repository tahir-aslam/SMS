using Microsoft.Reporting.WinForms;
using SMS.Models;
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

namespace SMS.AdmissionManagement.WithdrawAdmission
{
    /// <summary>
    /// Interaction logic for WithdrawlReportWindow.xaml
    /// </summary>
    public partial class WithdrawlReportWindow : Window
    {
        List<admission> adm_list;
        sms_report report_data;
        
        public WithdrawlReportWindow(List<admission> list, sms_report report_data)
        {
            InitializeComponent();
            adm_list = list;
            this.report_data = report_data;
            _reportViewer.Load += _reportViewer_Load;            
        }

        void _reportViewer_Load(object sender, EventArgs e)
        {
            ReportDataSource adm = new ReportDataSource();
            adm.Name = "adm"; //Name of the report dataset in our .RDLC file
            adm.Value = adm_list;

            ReportDataSource ins = new ReportDataSource();
            List<institute> ins_list = new List<institute>();
            MainWindow.ins.date = DateTime.Now;
            MainWindow.ins.page_no = 1;
            ins_list.Add(MainWindow.ins);
            ins.Name = "ins"; //Name of the report dataset in our .RDLC file
            ins.Value = ins_list;

            ReportDataSource report_data = new ReportDataSource();
            List<sms_report> report_list = new List<sms_report>();
            report_list.Add(this.report_data);
            report_data.Name = "report_data"; //Name of the report dataset in our .RDLC file
            report_data.Value = report_list;

            this._reportViewer.LocalReport.DataSources.Add(adm);
            this._reportViewer.LocalReport.DataSources.Add(ins);
            this._reportViewer.LocalReport.DataSources.Add(report_data);

        
            this._reportViewer.LocalReport.ReportEmbeddedResource = "SMS.AdmissionManagement.WithdrawAdmission.WithdrawlRegisterReport.rdlc";
        

            _reportViewer.RefreshReport();

            //_isReportViewerLoaded = true;
        }
    }
}
