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
using SMS.AccountManagement.Investment;
using MySql.Data.MySqlClient;

namespace SMS.AccountManagement.Investment
{
    /// <summary>
    /// Interaction logic for InvestmentForm.xaml
    /// </summary>
    public partial class InvestmentForm : Window
    {
        InvestmentPage inp;        
        List<sms_investor> investors_list;
        sms_investments investment_obj;
        sms_investments obj;
        string mode;

        public InvestmentForm(string mode,InvestmentPage INP,sms_investments obj)
        {
            InitializeComponent();
            inp = INP;
            this.obj = obj;
            this.mode = mode;

            investor_cmb.Focus();
            get_all_investors();
            investors_list.Insert(0, new sms_investor() { investor_name = "--Select Name--", id = "-1" });
            investor_cmb.ItemsSource = investors_list;
            investor_cmb.SelectedIndex = 0;
            date_textbox.SelectedDate = DateTime.Now;
            if (mode == "edit")
            {
                fill_control();
            }
        }
        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //--------------          Click_Save             -----------------------------------

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        //---------------           Submit Form    ----------------------------------
        public int submit()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_investments(investor_id,investor_name,description,amount,investment_date,cheque_no,created_by,date_time) Values(@investor_id,@investor_name,@description,@amount,@investment_date,@cheque_no,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@investor_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.investor_id;
                        cmd.Parameters.Add("@investor_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.investor_name;
                        cmd.Parameters.Add("@description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.description;
                        cmd.Parameters.Add("@amount", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.amount;
                        cmd.Parameters.Add("@investment_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = investment_obj.investment_date;
                        cmd.Parameters.Add("@cheque_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.cheque_no;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = investment_obj.date_time;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Form        ---------------------------------
        public int update()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_investments SET investor_id=@investor_id,investor_name=@investor_name,description=@description,amount=@amount,investment_date=@investment_date,cheque_no=@cheque_no,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@investor_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.investor_id;
                        cmd.Parameters.Add("@investor_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.investor_name;
                        cmd.Parameters.Add("@description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.description;
                        cmd.Parameters.Add("@amount", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.amount;
                        cmd.Parameters.Add("@investment_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = investment_obj.investment_date;
                        cmd.Parameters.Add("@cheque_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.cheque_no;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investment_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = investment_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //-----------       Get All Investors   ----------------------
        public void get_all_investors()
        {
            investors_list = new List<sms_investor>();

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
                            investors_list.Add(si);
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
        //------------------    Fill Object      ------------------------
        public void fill_object()
        {
            investment_obj = new sms_investments();

            sms_investor ae = (sms_investor)investor_cmb.SelectedItem;
            investment_obj.investor_id = ae.id;
            investment_obj.investor_name = ae.investor_name;
            investment_obj.investment_date = date_textbox.SelectedDate.Value;
            investment_obj.amount = amonut_textbox.Text;
            investment_obj.cheque_no = cheque_textbox.Text;
            investment_obj.description = investor_desc_textbox.Text;
            investment_obj.date_time = DateTime.Now;
            investment_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            investor_cmb.SelectedValue = obj.investor_id;
            investor_desc_textbox.Text = obj.description;
            date_textbox.DisplayDate = obj.investment_date;
            amonut_textbox.Text = obj.amount;
            cheque_textbox.Text = obj.cheque_no;
            
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (investor_cmb.SelectedIndex == 0)
            {
                investor_cmb.Focus();
                string alertText = "Please Select Investor";
                MessageBox.Show(alertText);
                return false;
            }
            else if (date_textbox.SelectedDate == null)
            {
                date_textbox.Focus();
                string alertText = "Please Enter Date";
                MessageBox.Show(alertText);
                return false;
            }
            else if (amonut_textbox.Text.Length == 0)
            {
                date_textbox.Focus();
                string alertText = "Please Enter Amount";
                MessageBox.Show(alertText);
                return false;
            }
            else
            {
                return true;
            }

        }


        //--------------           Save          ----------------------

        public void save()
        {
            fill_object();
            if (validate())
            {
                if (mode == "insert")
                {
                    if (submit() > 0)
                    {
                        MessageBox.Show("Record Added Successfully");
                        this.Close();
                        inp.load_grid();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else if (mode == "edit")
                {
                    if (update() > 0)
                    {
                        MessageBox.Show("Record Updated Successfully");
                        this.Close();
                        inp.load_grid();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }

                }
                else
                {
                    MessageBox.Show("mode not set");
                }

            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();
            }

        }
    }
}
