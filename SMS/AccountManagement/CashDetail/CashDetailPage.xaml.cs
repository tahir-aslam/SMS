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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SMS.Models;
using MySql.Data.MySqlClient;
using SUT.PrintEngine.Utils;

namespace SMS.AccountManagement.CashDetail
{
    /// <summary>
    /// Interaction logic for CashDetailPage.xaml
    /// </summary>
    public partial class CashDetailPage : Page
    {
        List<sms_months> months_list;
        List<cashDetails> cashDetailsList;
        List<account_entry> account_entry_list;
        List<fee> paid_fee_list;
        int totalCash = 0;
        int totalExpense = 0;

        public CashDetailPage()
        {
            InitializeComponent();

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "-Select Month-", id = "-1" });
            month_cmb.ItemsSource = months_list;

            institue_name.Text = MainWindow.ins.institute_name;
            institute_logo.Source = MainWindow.ByteToImage(MainWindow.ins.institute_logo);
            session_tb.Text = MainWindow.session.session_name;
        }
             //---------------           Get All Months    ----------------------------------
        public void get_all_months()
        {
            months_list = new List<sms_months>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_months";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_months sm = new sms_months()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                month_id = Convert.ToString(reader["month"].ToString()),
                            };
                            months_list.Add(sm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (month_cmb.SelectedIndex != 0)
            {
                sms_months sm = (sms_months)month_cmb.SelectedItem;
                report_grid.Visibility = Visibility.Visible;
                img_grid.Visibility = Visibility.Hidden;

                getCashDetails(sm.month_id);
            }
            else
            {
                report_grid.Visibility = Visibility.Hidden;
                img_grid.Visibility = Visibility.Visible;
            }
        }

        public void getCashDetails(string month) 
        {           
            int selectedMonth= Convert.ToInt32(month);
            DateTime sDate;
            DateTime eDate;
            sms_months sm = (sms_months)month_cmb.SelectedItem;            

            string[] years= MainWindow.session.session_name.Split('-');
            if (selectedMonth < 13 && selectedMonth > 03)
            {                
                sDate = Convert.ToDateTime(years[0] + "/" + selectedMonth + "/01");
                eDate = sDate.AddMonths(1);
                session_tb.Text = sm.month_name+"-"+years[0];
            }
            else 
            {
                sDate = Convert.ToDateTime(years[1] + "/" + selectedMonth + "/01");
                eDate = sDate.AddMonths(1);
                session_tb.Text = sm.month_name + "-" + years[1];
            }

            PopulateCashDetailList(sDate,eDate);
            cashDetailsDataGrid.ItemsSource = cashDetailsList;
            cashTB.Text = totalCash.ToString() + " Rs" ;
            expenseTB.Text = totalExpense.ToString() + " Rs";
        }

        public void PopulateCashDetailList(DateTime sDate, DateTime eDate)
        {
            cashDetailsList = new List<cashDetails>();
            cashDetails cd;
            int cash = 0;
            int expense = 0;
            totalCash = 0;
            totalExpense = 0;
            for (int i = 0; i < 32; i++) 
            {
                if(sDate < eDate)
                {
                    cd = new cashDetails();
                    cd.monthDay = sDate.Day;
                    cd.weekDay = sDate.ToString("dddd");
                    //cash detail
                    cash = get_fee_history_by_date(sDate);
                    totalCash = totalCash + cash;
                    if (cash > 0)
                    {
                        cd.cashAmount = cash.ToString();
                    }
                    else 
                    {
                        cd.cashAmount = "";
                    }

                    // expense detail
                    expense = get_all_accounts_entry(sDate);
                    totalExpense = totalExpense + expense;
                    if (expense > 0)
                    {
                        cd.expenseAmount = expense.ToString();
                    }
                    else 
                    {
                        cd.expenseAmount = "";
                    }                    

                    cashDetailsList.Add(cd);
                }
                sDate= sDate.AddDays(1);
            }
        }

        public int get_fee_history_by_date(DateTime dt)
        {           
            int total = 0;
            int total_paid = 0;
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where session_id=" + MainWindow.session.id;
                        cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = dt;

                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            total_paid = 0;
                            fee paid_fee = new fee()
                            {
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            if (paid_fee.date_time.ToString("yyyy-MM-dd") == dt.ToString("yyyy-MM-dd"))
                            {
                                total_paid = Convert.ToInt32(paid_fee.reg_fee) + Convert.ToInt32(paid_fee.adm_fee) + Convert.ToInt32(paid_fee.tution_fee) + Convert.ToInt32(paid_fee.other_expenses) + Convert.ToInt32(paid_fee.security_fee) + Convert.ToInt32(paid_fee.exam_fee);
                                total = total + total_paid;
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return total;
        }

        //-----------       Get All Accounts Data entry    ----------------------
        public int get_all_accounts_entry(DateTime dt)
        {           
            int total_amount = 0;
            account_entry_list = new List<account_entry>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_account_entry where date=@dt";
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            account_entry acc = new account_entry()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                account_id = Convert.ToString(reader["account_id"].ToString()),
                                account_name = Convert.ToString(reader["account_name"].ToString()),
                                cheque_no = Convert.ToString(reader["cheque_no"].ToString()),
                                expenditure = Convert.ToString(reader["expenditure"].ToString()),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                date = Convert.ToDateTime(reader["date"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            account_entry_list.Add(acc);
                            total_amount = total_amount + Convert.ToInt32(acc.amount);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return total_amount;
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var visualSize = new Size(report_grid.ActualWidth, report_grid.ActualHeight);
            var printControl = PrintControlFactory.Create(visualSize, report_grid);
            printControl.ShowPrintPreview();
        }
    }
}
