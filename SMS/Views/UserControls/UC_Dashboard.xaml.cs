using LiveCharts;
using LiveCharts.Defaults;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMS.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UC_Dashboard.xaml
    /// </summary>
    public partial class UC_Dashboard : UserControl
    {
        #region Fields
        DashboardChartModel DashboardResult;
        DashboardDAL dashboardDAL;

        #region Students
        ObservableValue TotalStudents;
        ObservableValue TotalStudentsInActive;
        ObservableValue TotalBoys;
        ObservableValue TotalGirls;
        ObservableValue TotalStudentPresent;
        ObservableValue TotalStudentAbsent;
        ObservableValue TotalStudentLeave;
        ObservableValue TotalStudentPresentInActive;
        ObservableValue TotalStudentAbsentInActive;
        ObservableValue TotalStudentLeaveInActive;
        #endregion

        #region Employees
        ObservableValue TotalEmp;
        ObservableValue TotalEmpInActive;
        ObservableValue TotalEmpMales;
        ObservableValue TotalEmpFemales;
        ObservableValue TotalEmpPresent;
        ObservableValue TotalEmpAbsent;
        ObservableValue TotalEmpLeave;
        ObservableValue TotalEmpPresentInActive;
        ObservableValue TotalEmpAbsentInActive;
        ObservableValue TotalEmpLeaveInActive;
        #endregion

        #region Fee
        ObservableValue TotalFeeGeneratedForMonth;
        ObservableValue TotalFeeGeneratedForMonthInActive;
        ObservableValue TotalFeePaidForMonth;
        ObservableValue TotalFeePaidForMonthInActive;
        ObservableValue TotalFeeDefaulterForMonth;
        ObservableValue TotalFeeDefaulterForMonthInActive;
        ObservableValue TotalFeeWaveOffForMonth;
        ObservableValue TotalFeeWaveOffForMonthInActive;
        ObservableValue TotalFeePaidInMonth;
        ObservableValue TotalFeePaidInMonthInActive;
        ObservableValue TotalFeeDefaulterTotal;
        ObservableValue TotalFeeDefaulterTotalInActive;
        #endregion

        SolidColorBrush _activeColor = new SolidColorBrush(Color.FromRgb(85, 196, 18));
        SolidColorBrush _activeBorder = Brushes.Transparent;
        SolidColorBrush _inActiveColor = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        SolidColorBrush _inActiveBorder = Brushes.Transparent;

        #endregion

        public UC_Dashboard()
        {
            InitializeComponent();
            dashboardDAL = new DashboardDAL();

            #region Students
            TotalStudents = new ObservableValue(0);
            TotalStudentsInActive = new ObservableValue(0);
            TotalBoys = new ObservableValue(0);
            TotalGirls = new ObservableValue(0);
            TotalStudentPresent = new ObservableValue(0);
            TotalStudentAbsent = new ObservableValue(0);
            TotalStudentLeave = new ObservableValue(0);
            TotalStudentPresentInActive = new ObservableValue(1);
            TotalStudentAbsentInActive = new ObservableValue(1);
            TotalStudentLeaveInActive = new ObservableValue(1);
            #endregion

            #region Employees
            TotalEmp = new ObservableValue(0);
            TotalEmpInActive = new ObservableValue(0);
            TotalEmpMales = new ObservableValue(0);
            TotalEmpFemales = new ObservableValue(0);
            TotalEmpPresent = new ObservableValue(0);
            TotalEmpAbsent = new ObservableValue(0);
            TotalEmpLeave = new ObservableValue(0);
            TotalEmpPresentInActive = new ObservableValue(1);
            TotalEmpAbsentInActive = new ObservableValue(1);
            TotalEmpLeaveInActive = new ObservableValue(1);
            #endregion

            #region Fee
            TotalFeeGeneratedForMonth = new ObservableValue(0);
            TotalFeeGeneratedForMonthInActive = new ObservableValue(0);
            TotalFeePaidForMonth = new ObservableValue(0);
            TotalFeePaidForMonthInActive = new ObservableValue(0);
            TotalFeeDefaulterForMonth = new ObservableValue(0);
            TotalFeeDefaulterForMonthInActive = new ObservableValue(0);
            TotalFeeWaveOffForMonth = new ObservableValue(0);
            TotalFeeWaveOffForMonthInActive = new ObservableValue(0);
            TotalFeePaidInMonth = new ObservableValue(0);
            TotalFeePaidInMonthInActive = new ObservableValue(0);
            TotalFeeDefaulterTotal = new ObservableValue(0);
            TotalFeeDefaulterTotalInActive = new ObservableValue(0);
            #endregion
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            initGraph();
            DashboardResult = dashboardDAL.loadPieChart(Convert.ToInt32(MainWindow.session.id));
            loadTotalReaderStatisticGraphs(DashboardResult);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void initGraph()
        {
            try
            {
                #region Students
                // Total students
                PieStudentsInActive.Values = new ChartValues<ObservableValue> { TotalStudentsInActive };
                PieStudentsActive.Values = new ChartValues<ObservableValue> { TotalStudents };

                // Boys

                PieBoysInActive.Values = new ChartValues<ObservableValue> { TotalGirls };
                PieBoysActive.Values = new ChartValues<ObservableValue> { TotalBoys };

                // Girls
                PieGirlsInActive.Values = new ChartValues<ObservableValue> { TotalBoys };
                PieGirlsActive.Values = new ChartValues<ObservableValue> { TotalGirls };

                // Present                        

                PiePresentInActive.Values = new ChartValues<ObservableValue> { TotalStudentPresentInActive };
                PiePresentActive.Values = new ChartValues<ObservableValue> { TotalStudentPresent };

                // Absent
                PieAbsentInActive.Values = new ChartValues<ObservableValue> { TotalStudentAbsentInActive };
                PieAbsentActive.Values = new ChartValues<ObservableValue> { TotalStudentAbsent };

                // Leave
                PieLeaveInActive.Values = new ChartValues<ObservableValue> { TotalStudentLeaveInActive };
                PieLeaveActive.Values = new ChartValues<ObservableValue> { TotalStudentLeave };
                #endregion

                #region Employees

                // Total students
                PieEmpInActive.Values = new ChartValues<ObservableValue> { TotalEmpInActive };
                PieEmpActive.Values = new ChartValues<ObservableValue> { TotalEmp };

                // Boys

                PieEmpMaleInActive.Values = new ChartValues<ObservableValue> { TotalEmpFemales };
                PieEmpMaleActive.Values = new ChartValues<ObservableValue> { TotalEmpMales };

                // Girls
                PieEmpFemaleInActive.Values = new ChartValues<ObservableValue> { TotalEmpMales };
                PieEmpFemaleActive.Values = new ChartValues<ObservableValue> { TotalEmpFemales };

                // Present                        

                PieEmpPresentInActive.Values = new ChartValues<ObservableValue> { TotalEmpPresentInActive };
                PieEmpPresentActive.Values = new ChartValues<ObservableValue> { TotalEmpPresent };

                // Absent
                PieEmpAbsentInActive.Values = new ChartValues<ObservableValue> { TotalEmpAbsentInActive };
                PieEmpAbsentActive.Values = new ChartValues<ObservableValue> { TotalEmpAbsent };

                // Leave
                PieEmpLeaveInActive.Values = new ChartValues<ObservableValue> { TotalEmpLeaveInActive };
                PieEmpLeaveActive.Values = new ChartValues<ObservableValue> { TotalEmpLeave };
                #endregion

                #region Fee

                // Total students
                PieFeeGeneratedForMonthInActive.Values = new ChartValues<ObservableValue> { TotalFeeGeneratedForMonthInActive };
                PieFeeGeneratedForMonthActive.Values = new ChartValues<ObservableValue> { TotalFeeGeneratedForMonth };

                // Total students
                PieFeePaidForMonthInActive.Values = new ChartValues<ObservableValue> { TotalFeePaidForMonthInActive };
                PieFeePaidForMonthActive.Values = new ChartValues<ObservableValue> { TotalFeePaidForMonth };

                // Total students
                PieFeeDefaulterForMonthInActive.Values = new ChartValues<ObservableValue> { TotalFeeDefaulterForMonthInActive };
                PieFeeDefaulterForMonthActive.Values = new ChartValues<ObservableValue> { TotalFeeDefaulterForMonth };

                // Total students
                PieFeeWaveOffForMonthInActive.Values = new ChartValues<ObservableValue> { TotalFeeWaveOffForMonthInActive };
                PieFeeWaveOffForMonthActive.Values = new ChartValues<ObservableValue> { TotalFeeWaveOffForMonth };

                // Total students
                PieFeePaidInMonthInActive.Values = new ChartValues<ObservableValue> { TotalFeePaidInMonthInActive };
                PieFeePaidInMonthActive.Values = new ChartValues<ObservableValue> { TotalFeePaidInMonth };

                // Total students
                PieFeeDefaulterTotalInActive.Values = new ChartValues<ObservableValue> { TotalFeeDefaulterTotalInActive };
                PieFeeDefaulterTotalActive.Values = new ChartValues<ObservableValue> { TotalFeeDefaulterTotal };


                #endregion
            }
            catch (Exception ex)
            {
                //LogHelper.WriteException(ex.Message.ToString(), source: "Quanika(initGraph)");
            }
        }

        private void loadTotalReaderStatisticGraphs(DashboardChartModel dCount)
        {
            try
            {

                assignChartsData(_activeColor, _activeBorder, _inActiveColor, _inActiveBorder, dCount);

            }
            catch (Exception ex)
            {
                //show error
                //LogHelper.WriteException(ex.Message.ToString(), source: "Quanika");
            }
        }
        private void assignChartsData(SolidColorBrush _activeColor, SolidColorBrush _activeBorder, SolidColorBrush _inActiveColor, SolidColorBrush _inActiveBorder, DashboardChartModel _chatsData)
        {
            #region Students
            // Students
            TotalStudents.Value = _chatsData.TotalStudents;
            TotalStudentsInActive.Value = 0;
            lblTotalStudents.Text = _chatsData.TotalStudents.ToString();

            // Boys
            TotalBoys.Value = _chatsData.TotalBoys;
            lblTotalBoys.Text = _chatsData.TotalBoys.ToString();

            // Girls
            TotalGirls.Value = _chatsData.TotalGirls;
            lblTotalGirls.Text = _chatsData.TotalGirls.ToString();

            // Present
            TotalStudentPresent.Value = _chatsData.TotalStudentPresent;
            TotalStudentPresentInActive.Value = (_chatsData.TotalStudentAbsent + _chatsData.TotalStudentLeave) == 0 ? 1 : _chatsData.TotalStudentAbsent + _chatsData.TotalStudentLeave;
            lblTotalPresent.Text = _chatsData.TotalStudentPresent.ToString();

            // Absent
            TotalStudentAbsent.Value = _chatsData.TotalStudentAbsent;
            TotalStudentAbsentInActive.Value = (_chatsData.TotalStudentLeave + _chatsData.TotalStudentPresent) == 0 ? 1 : _chatsData.TotalStudentLeave + _chatsData.TotalStudentPresent;
            lblTotalAbsent.Text = _chatsData.TotalStudentAbsent.ToString();

            // Leave
            TotalStudentLeave.Value = _chatsData.TotalStudentLeave;
            TotalStudentLeaveInActive.Value = (_chatsData.TotalStudentAbsent + _chatsData.TotalStudentPresent) == 0 ? 1 : _chatsData.TotalStudentAbsent + _chatsData.TotalStudentPresent;
            lblTotalLeave.Text = _chatsData.TotalStudentLeave.ToString();
            #endregion

            #region Employees
            // Employees
            TotalEmp.Value = _chatsData.TotalEmp;
            TotalEmpInActive.Value = 0;
            lblTotalEmp.Text = _chatsData.TotalEmp.ToString();

            // Male
            TotalEmpMales.Value = _chatsData.TotalEmpMale;
            lblTotalEmpMale.Text = _chatsData.TotalEmpMale.ToString();

            // Females
            TotalEmpFemales.Value = _chatsData.TotalEmpFemale;
            lblTotalEmpFemales.Text = _chatsData.TotalEmpFemale.ToString();

            // Present
            TotalEmpPresent.Value = _chatsData.TotalEmpPresent;
            TotalEmpPresentInActive.Value = (_chatsData.TotalEmpAbsent + _chatsData.TotalEmpLeave) == 0 ? 1 : _chatsData.TotalEmpAbsent + _chatsData.TotalEmpLeave;
            lblTotalEmpPresent.Text = _chatsData.TotalEmpPresent.ToString();

            // Absent
            TotalEmpAbsent.Value = _chatsData.TotalEmpAbsent;
            TotalEmpAbsentInActive.Value = (_chatsData.TotalEmpLeave + _chatsData.TotalEmpPresent) == 0 ? 1 : _chatsData.TotalEmpLeave + _chatsData.TotalEmpPresent;
            lblTotalEmpAbsent.Text = _chatsData.TotalEmpAbsent.ToString();

            // Leave
            TotalEmpLeave.Value = _chatsData.TotalStudentLeave;
            TotalEmpLeaveInActive.Value = (_chatsData.TotalEmpAbsent + _chatsData.TotalEmpPresent) == 0 ? 1 : _chatsData.TotalEmpAbsent + _chatsData.TotalEmpPresent;
            lblTotalEmpLeave.Text = _chatsData.TotalEmpLeave.ToString();
            #endregion

            #region Fee
            // Generated
            TotalFeeGeneratedForMonth.Value = _chatsData.FeeGeneratedForMonth;
            TotalFeeGeneratedForMonthInActive.Value = 0;
            lblFeeGeneratedForMonth.Text = _chatsData.FeeGeneratedForMonth.ToString();

            // Paid
            TotalFeePaidForMonth.Value = _chatsData.FeePaidForMonth;
            TotalFeePaidForMonthInActive.Value = (_chatsData.FeeDefaulterForMonth + _chatsData.FeeWaveOffForMonth) == 0 ? 1 : _chatsData.FeeDefaulterForMonth + _chatsData.FeeWaveOffForMonth;
            lblFeePaidForMonth.Text = _chatsData.FeePaidForMonth.ToString();


            // WaveOff
            TotalFeeWaveOffForMonth.Value = _chatsData.FeeWaveOffForMonth;
            TotalFeeWaveOffForMonthInActive.Value = (_chatsData.FeeDefaulterForMonth + _chatsData.FeePaidForMonth) == 0 ? 1 : _chatsData.FeeDefaulterForMonth + _chatsData.FeePaidForMonth;
            lblFeeWaveOffForMonth.Text = _chatsData.FeeWaveOffForMonth.ToString();

            // defaulter
            TotalFeeDefaulterForMonth.Value = _chatsData.FeeDefaulterForMonth;
            TotalFeeDefaulterForMonthInActive.Value = (_chatsData.FeeWaveOffForMonth + _chatsData.FeePaidForMonth) == 0 ? 1 : _chatsData.FeeWaveOffForMonth + _chatsData.FeePaidForMonth;
            lblFeeDefaulterForMonth.Text = _chatsData.FeeDefaulterForMonth.ToString();

            // Paid total
            TotalFeePaidInMonth.Value = _chatsData.FeePaidInMonth;
            TotalFeePaidInMonthInActive.Value = 0;
            lblFeePaidInMonth.Text = _chatsData.FeePaidInMonth.ToString();

            // Total defaulter
            TotalFeeDefaulterTotal.Value = _chatsData.FeeDefaulterTotal;
            TotalFeeDefaulterTotalInActive.Value = 0;
            lblFeeDefaulterTotal.Text = _chatsData.FeeDefaulterTotal.ToString();


            #endregion
        }
        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
