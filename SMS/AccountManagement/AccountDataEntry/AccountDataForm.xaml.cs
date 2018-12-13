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
using SMS.AccountManagement.Account;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.AccountManagement.AccountDataEntry
{
    /// <summary>
    /// Interaction logic for AccountDataForm.xaml
    /// </summary>
    public partial class AccountDataForm : Window
    {
        AccountDataSearch ads;
        List<account> account_list;
        account_entry account_entry_obj;
        account_entry obj;
        string mode;

        public AccountDataForm(string mode, AccountDataSearch ADS, account_entry obj)
        {
            InitializeComponent();
            ads = ADS;
            this.obj = obj;
            this.mode = mode;

            account_cmb.Focus();
            get_all_accounts();
            account_list.Insert(0, new account() { account_name = "--Select Account--", id = "-1" });
            account_cmb.ItemsSource = account_list;
            account_cmb.SelectedIndex = 0;
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
                        cmd.CommandText = "INSERT INTO sms_account_entry(account_id,account_name,expenditure,amount,date,cheque_no,created_by,date_time) Values(@account_id,@account_name,@expenditure,@amount,@date,@cheque_no,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@account_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.account_id;
                        cmd.Parameters.Add("@account_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.account_name;
                        cmd.Parameters.Add("@expenditure", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.expenditure;
                        cmd.Parameters.Add("@amount", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.amount;
                        cmd.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = account_entry_obj.date;
                        cmd.Parameters.Add("@cheque_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.cheque_no;  
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = account_entry_obj.date_time;


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
                        cmd.CommandText = "Update sms_account_entry SET account_id=@account_id,account_name=@account_name,expenditure=@expenditure,amount=@amount,date=@date,cheque_no=@cheque_no,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@account_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.account_id;
                        cmd.Parameters.Add("@account_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.account_name;
                        cmd.Parameters.Add("@expenditure", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.expenditure;
                        cmd.Parameters.Add("@amount", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.amount;
                        cmd.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = account_entry_obj.date;
                        cmd.Parameters.Add("@cheque_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.cheque_no;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_entry_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = account_entry_obj.date_time;
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
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),

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

        //------------------    Fill Object      ------------------------
        public void fill_object()
        {
            account_entry_obj = new account_entry();

            account ae = (account)account_cmb.SelectedItem;
            account_entry_obj.account_id = ae.id;
            account_entry_obj.account_name = ae.account_name;
            account_entry_obj.expenditure = exp_desc_textbox.Text;
            account_entry_obj.date = date_textbox.SelectedDate.Value;
            account_entry_obj.cheque_no = cheque_textbox.Text;
            account_entry_obj.amount = amonut_textbox.Text;
            account_entry_obj.date_time = DateTime.Now;
            account_entry_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            account_cmb.SelectedValue= obj.account_id;
            exp_desc_textbox.Text = obj.expenditure;
            date_textbox.DisplayDate = obj.date;
            amonut_textbox.Text = obj.amount;
            cheque_textbox.Text = obj.cheque_no;
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (account_cmb.SelectedIndex == 0)
            {
                account_cmb.Focus();
                string alertText = "Please Select Account";
                MessageBox.Show(alertText);
                return false;
            }
            else if(date_textbox.SelectedDate == null)
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
                        ads.load_grid();
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
                        ads.load_grid();
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
