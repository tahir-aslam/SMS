using Microsoft.Reporting.WinForms;
using SMS.DAL;
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

namespace SMS.Reports.Exams.TeacherEvaluation
{
    /// <summary>
    /// Interaction logic for TeacherEvaluationReportWindow.xaml
    /// </summary>
    public partial class TeacherEvaluationReportWindow : Window
    {
        List<exam_data_entry> exam_data_entry_list;
        ExamsDAL examsDAL;

        public TeacherEvaluationReportWindow()
        {
            InitializeComponent();
            examsDAL = new ExamsDAL();           
        }

        void loadReport()
        {
            if (exam_data_entry_list.Count > 0)
            {
                ReportDataSource exam = new ReportDataSource()
                {
                    Name = "exam",
                    Value = exam_data_entry_list
                };

                ReportDataSource ins = new ReportDataSource();
                List<institute> ins_list = new List<institute>();
                MainWindow.ins.date = DateTime.Now;
                MainWindow.ins.page_no = 1;
                ins_list.Add(MainWindow.ins);
                ins.Name = "ins"; //Name of the report dataset in our .RDLC file
                ins.Value = ins_list;

                this._reportViewer3.LocalReport.DataSources.Clear();
                this._reportViewer3.LocalReport.DataSources.Add(exam);
                this._reportViewer3.LocalReport.DataSources.Add(ins);
                this._reportViewer3.LocalReport.ReportEmbeddedResource = "SMS.Reports.Exams.TeacherEvaluation.TeacherEvaluationReport.rdlc";
                //this._reportViewer3.LocalReport.ReportEmbeddedResource = "bin.ExamsManagement.GeneralAwardList.ExamGeneralAwardListReport.rdlc";

                _reportViewer3.RefreshReport();
            }
        }

       
        private void v_showreport_Click(object sender, RoutedEventArgs e)
        {
            report_grid.Visibility = Visibility.Visible;
            exam_data_entry_list = examsDAL.GetAllExamDataEntryBySession(Convert.ToInt32(MainWindow.session.id));
            loadReport();
        }
    }
}
