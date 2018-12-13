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
using SMS.DAL;
using Microsoft.Reporting.WinForms;

namespace SMS.FeesManagement.FeesCollection
{
    /// <summary>
    /// Interaction logic for StudentFeeIndividualReport.xaml
    /// </summary>
    public partial class StudentFeeIndividualReport : Window
    {
        FeesDAL feeDAL;
        admission admObj;

        public StudentFeeIndividualReport(admission adm)
        {
            InitializeComponent();

            feeDAL = new FeesDAL();
            this.admObj = adm;
            LoadData();
        }        

        void LoadData()
        {
            List<sms_fees> feeList = feeDAL.get_all_fees_by_StdID(Convert.ToInt32(admObj.id),MainWindow.session.id);
            if (feeList.Count > 0)
            {                
                ReportDataSource adm = new ReportDataSource();
                List<admission> adm_list = new List<admission>();
                adm_list.Add(admObj);
                adm.Name = "adm";
                adm.Value = adm_list;

                ReportDataSource fees = new ReportDataSource();
                fees.Name = "fees";
                fees.Value = feeList;

                ReportDataSource ins = new ReportDataSource();
                List<institute> ins_list = new List<institute>();
                MainWindow.ins.date = DateTime.Now;
                MainWindow.ins.page_no = 1;
                ins_list.Add(MainWindow.ins);
                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                ins.Value = ins_list;

                this._reportViewer3.LocalReport.DataSources.Clear();
                this._reportViewer3.LocalReport.DataSources.Add(fees);
                this._reportViewer3.LocalReport.DataSources.Add(adm);
                this._reportViewer3.LocalReport.DataSources.Add(ins);
                this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.FeesManagement.FeesCollection.StduentFeeReportIndividual.rdlc";

                _reportViewer3.RefreshReport();
            }
            else
            {
                MessageBox.Show("Not Any record exist for this month");
            }
        }
    }
}
