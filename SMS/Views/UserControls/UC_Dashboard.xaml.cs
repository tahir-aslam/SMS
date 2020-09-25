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
        DashboardChartModel DashboardResult;
        DashboardDAL dashboardDAL;

        ObservableValue TotalStudents;
        ObservableValue TotalStudents2;
        ObservableValue TotalBoys;
        ObservableValue TotalGirls;
        ObservableValue TotalStudentPresent;
        ObservableValue TotalStudentAbsent;
        ObservableValue TotalStudentLeave;
        ObservableValue TotalStudentPresentInActive;
        ObservableValue TotalStudentAbsentInActive;
        ObservableValue TotalStudentLeaveInActive;

        ObservableValue TotalEmployee;
        ObservableValue TotalEmpMale;
        ObservableValue TotalEmpFemale;
        ObservableValue TotalEmpPresent;
        ObservableValue TotalEmpLeave;
        ObservableValue TotalEmpAbsent;

        SolidColorBrush _activeColor = new SolidColorBrush(Color.FromRgb(85, 196, 18));
        SolidColorBrush _activeBorder = Brushes.Transparent;
        SolidColorBrush _inActiveColor = new SolidColorBrush(Color.FromRgb(211, 211, 211));
        SolidColorBrush _inActiveBorder = Brushes.Transparent;

        public UC_Dashboard()
        {
            InitializeComponent();
            dashboardDAL = new DashboardDAL();

            TotalStudents = new ObservableValue(0);
            TotalStudents2 = new ObservableValue(0);
            TotalBoys = new ObservableValue(0);
            TotalGirls = new ObservableValue(0);
            TotalStudentPresent = new ObservableValue(0);
            TotalStudentAbsent = new ObservableValue(0);
            TotalStudentLeave = new ObservableValue(0);
            TotalStudentPresentInActive = new ObservableValue(0);
            TotalStudentAbsentInActive = new ObservableValue(0);
            TotalStudentLeaveInActive = new ObservableValue(0);
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

        private void btnStatistics_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void initGraph()
        {
            try
            {
                // Total students
                PieStudentsInActive.Values = new ChartValues<ObservableValue> { TotalStudents2 };
                PieStudentsActive.Values = new ChartValues<ObservableValue> { TotalStudents };

                // Boys

                PieBoysInActive.Values = new ChartValues<ObservableValue> { TotalGirls };
                PieBoysActive.Values = new ChartValues<ObservableValue> { TotalBoys };

                // Girls
                PieGirlsInActive.Values = new ChartValues<ObservableValue> { TotalBoys };
                PieGirlsActive.Values = new ChartValues<ObservableValue> { TotalGirls};

                // Present                        

                PiePresentInActive.Values = new ChartValues<ObservableValue> { TotalStudentPresentInActive };
                PiePresentActive.Values = new ChartValues<ObservableValue> { TotalStudentPresent };

                // Absent
                PieAbsentInActive.Values = new ChartValues<ObservableValue> { TotalStudentAbsentInActive};
                PieAbsentActive.Values = new ChartValues<ObservableValue> { TotalStudentAbsent };

                // Leave
                PieLeaveInActive.Values = new ChartValues<ObservableValue> { TotalStudentLeaveInActive};
                PieLeaveActive.Values = new ChartValues<ObservableValue> { TotalStudentLeave };
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
            // Students
            TotalStudents.Value = _chatsData.TotalStudents;
            TotalStudents2.Value = 0;
            lblTotalStudents.Text = _chatsData.TotalStudents.ToString();

            // Boys
            TotalBoys.Value = _chatsData.TotalBoys;
            lblTotalBoys.Text = _chatsData.TotalBoys.ToString();

            // Girls
            TotalGirls.Value = _chatsData.TotalGirls;
            lblTotalGirls.Text = _chatsData.TotalGirls.ToString();

            // Present
            TotalStudentPresent.Value = _chatsData.TotalStudentPresent;
            TotalStudentPresentInActive.Value = _chatsData.TotalStudentAbsent + _chatsData.TotalStudentLeave;
            lblTotalPresent.Text = _chatsData.TotalStudentPresent.ToString();

            // Absent
            TotalStudentAbsent.Value = _chatsData.TotalStudentAbsent;
            TotalStudentAbsentInActive.Value = _chatsData.TotalStudentLeave + _chatsData.TotalStudentPresent;
            lblTotalAbsent.Text = _chatsData.TotalStudentAbsent.ToString();

            // Leave
            TotalStudentLeave.Value = _chatsData.TotalStudentLeave;
            TotalStudentLeaveInActive.Value = _chatsData.TotalStudentAbsent + _chatsData.TotalStudentPresent;
            lblTotalLeave.Text = _chatsData.TotalStudentLeave.ToString();
        }

    }
}
