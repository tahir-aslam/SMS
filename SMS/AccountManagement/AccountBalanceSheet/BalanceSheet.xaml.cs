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
using MySql.Data.MySqlClient;
using SMS.Models;

namespace SMS.AccountManagement.AccountBalanceSheet
{
    /// <summary>
    /// Interaction logic for BalanceSheet.xaml
    /// </summary>
    public partial class BalanceSheet : Page
    {
        List<fee> paid_fee_list;
        int total_paid_fee = 0;
        List<sms_months> months_list;
        List<account> account_list;
        List<account_entry> account_entry_list;
        List<account_entry> expense_list;
        int total_amount = 0;
        int total_account = 0;
        int total_investment = 0;
        int total_amount_investment = 0;
        account_entry ae_obj;
        sms_investments si_obj;
        List<sms_investments> investment_list;
        List<sms_investments> investment_list_new;
        List<sms_investor> investor_list;

        public BalanceSheet()
        {
            InitializeComponent();

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;
            get_all_accounts();
            get_all_investors();
            date_picker.SelectedDate = DateTime.Now;
        }

        //-----------       Get All Investors    ----------------------
        public void get_all_investors()
        {
            investor_list = new List<sms_investor>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_investors";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_investor si = new sms_investor()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                investor_name = Convert.ToString(reader["investor_name"].ToString()),
                                investor_cell = Convert.ToString(reader["investor_cell"].ToString()),
                                investor_address = Convert.ToString(reader["investor_address"].ToString()),
                                investor_description = Convert.ToString(reader["investor_description"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            investor_list.Add(si);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
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

        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            balanceSheet bs = new balanceSheet();
            bs.total_investments = total_amount_investment.ToString();
            bs.total_expenses = total_amount.ToString();
            bs.total_fee = total_paid_fee.ToString();
            bs.total_amount = total_tb.Text;
            bs.institute_name = MainWindow.ins.institute_name;
            bs.image = MainWindow.ins.institute_logo;
            bs.report_name = "Balance Sheet";
            bs.account_entry_list = new List<account_entry>();
            bs.investment_list = new List<sms_investments>();
            bs.account_entry_list = expense_list;
            bs.investment_list = investment_list_new;
            if (month_cmb.SelectedIndex == 0)
            {
                bs.date = date_picker.SelectedDate.Value.ToString("dd MMM yy");               
            }
            else 
            {
                sms_months sm = (sms_months)month_cmb.SelectedItem;
                bs.date = sm.month_name;
            }

            BalanceSheetPrint bsp = new BalanceSheetPrint(bs);
            bsp.ShowDialog();
        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker.SelectedDate != null)
            {
                month_cmb.SelectedIndex = 0;
                DateTime dt = (DateTime)date_picker.SelectedDate;

                get_all_paid_fee_by_date(dt);
                get_total_fee();
                fee_tb.Text = total_paid_fee.ToString();

                get_all_accounts_entry(dt);
                get_total_accounts();
                expense_datagrid.ItemsSource = expense_list;

                get_all_investments(dt);
                get_total_investments();
                investment_datagrid.ItemsSource = investment_list_new;

                fill_total();
                
            }
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sms_months sm = (sms_months)month_cmb.SelectedItem;
            if (month_cmb.SelectedIndex != 0)
            {
                date_picker.SelectedDate = null;

                get_all_paid_fee_by_month(sm.month_id);
                get_total_fee();
                fee_tb.Text = total_paid_fee.ToString();

                get_all_accounts_entry(sm.month_id);
                get_total_accounts();
                expense_datagrid.ItemsSource = expense_list;

                get_all_investments(sm.month_id);
                get_total_investments();
                investment_datagrid.ItemsSource = investment_list_new;

                fill_total();
            }
            else
            {

            }
        }

        public void fill_total() 
        {
            total_fee_tb.Text = total_paid_fee.ToString();
            total_investment_tb.Text = total_amount_investment.ToString();
            total_amount_tb.Text = total_amount.ToString();
            total_tb.Text = (total_paid_fee + total_amount_investment - total_amount).ToString();
        }

        public void get_total_fee() 
        {
            int total = 0;
            total_paid_fee = 0;
            foreach(fee f in paid_fee_list)
            {
                total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee);
                total_paid_fee = total_paid_fee + total;
            }
        }

        public void get_total_accounts() 
        {
            expense_list = new List<account_entry>();
            total_amount = 0;

            foreach (account acc in account_list)
            {
                total_account=0;
                foreach (account_entry ae in account_entry_list.Where(x=>x.account_id == acc.id))
                {
                    total_account = total_account + Convert.ToInt32(ae.amount);
                }
                if(total_account > 0)
                {
                    ae_obj = new account_entry();
                    ae_obj.account_name = acc.account_name;
                    ae_obj.amount = total_account.ToString();

                    total_amount = total_amount + total_account;
                    expense_list.Add(ae_obj);
                }
                
            }
        }

        public void get_all_paid_fee_by_date(DateTime dt) 
        {
            
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where session_id="+MainWindow.session.id;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee paid_fee = new fee()
                            {
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            if (paid_fee.date_time.ToString("yyyy-MM-dd") == dt.ToString("yyyy-MM-dd"))
                            {
                                paid_fee_list.Add(paid_fee);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void get_all_paid_fee_by_month(string id)
        {

            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where session_id=" + MainWindow.session.id;                       

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
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
                            if (paid_fee.date_time.Month == Convert.ToInt32(id))
                            {
                                paid_fee_list.Add(paid_fee);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }        

        //-----------       Get All Accounts Data    ----------------------
        public void get_all_accounts()
        {
            account_list = new List<account>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_accounts";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            account acc = new account()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                account_name = Convert.ToString(reader["account_name"].ToString()),
                                account_desc = Convert.ToString(reader["account_desc"].ToString()),
                                account_holder_name = Convert.ToString(reader["account_holder_name"].ToString()),
                                account_holder_cell = Convert.ToString(reader["account_holder_cell"].ToString()),
                                account_holder_phn = Convert.ToString(reader["account_holder_phn"].ToString()),

                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            account_list.Add(acc);

                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //-----------       Get All Accounts Data entry    ----------------------
        public void get_all_accounts_entry(DateTime dt)
        {
            total_amount = 0;
            account_entry_list = new List<account_entry>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_account_entry where date=@dt ORDER BY account_id";
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
        }

        //-----------       Get All Accounts Data entry    ----------------------
        public void get_all_accounts_entry(string id)
        {
            total_amount = 0;
            account_entry_list = new List<account_entry>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_account_entry ORDER BY account_id";                    
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
                            if(acc.date.Month == Convert.ToInt32(id))
                            {
                                account_entry_list.Add(acc);
                                total_amount = total_amount + Convert.ToInt32(acc.amount);
                            }
                            
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //-----------       Get All Investments    ----------------------
        public void get_all_investments(DateTime dt)
        {
            total_investment = 0;
            investment_list = new List<sms_investments>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_investments where investment_date=@dt ORDER BY investor_id";
                    cmd.Parameters.Add("@dt", MySqlDbType.Date).Value = dt;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_investments si = new sms_investments()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                investor_id = Convert.ToString(reader["investor_id"].ToString()),
                                investor_name = Convert.ToString(reader["investor_name"].ToString()),
                                description = Convert.ToString(reader["description"].ToString()),
                                cheque_no = Convert.ToString(reader["cheque_no"].ToString()),
                                investment_date = Convert.ToDateTime(reader["investment_date"]),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            investment_list.Add(si);
                            total_investment = total_investment + Convert.ToInt32(si.amount);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //-----------       Get All Investments    ----------------------
        public void get_all_investments(string id)
        {
            total_investment = 0;
            investment_list = new List<sms_investments>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_investments ORDER BY investor_id";                   
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_investments si = new sms_investments()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                investor_id = Convert.ToString(reader["investor_id"].ToString()),
                                investor_name = Convert.ToString(reader["investor_name"].ToString()),
                                description = Convert.ToString(reader["description"].ToString()),
                                cheque_no = Convert.ToString(reader["cheque_no"].ToString()),
                                investment_date = Convert.ToDateTime(reader["investment_date"]),
                                amount = Convert.ToString(reader["amount"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            if (si.investment_date.Month == Convert.ToInt32(id))
                            {
                                investment_list.Add(si);
                                total_investment = total_investment + Convert.ToInt32(si.amount);
                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //-----------       Get total Investment  -------------------
        public void get_total_investments()
        {
            investment_list_new = new List<sms_investments>();
            total_investment = 0;
            total_amount_investment = 0;

            foreach (sms_investor si in investor_list)
            {
                total_investment = 0;
                foreach (sms_investments sii in investment_list.Where(x => x.investor_id == si.id))
                {
                    total_investment = total_investment + Convert.ToInt32(sii.amount);
                }
                if (total_investment > 0)
                {
                    si_obj = new sms_investments();
                    si_obj.investor_name = si.investor_name;
                    si_obj.amount = total_investment.ToString();

                    total_amount_investment = total_amount_investment + total_investment;
                    investment_list_new.Add(si_obj);
                }

            }
        }

    }
}
