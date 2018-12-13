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

namespace SMS.AccountManagement.Account
{
    /// <summary>
    /// Interaction logic for AccountForm.xaml
    /// </summary>
    public partial class AccountForm : Window
    {
        AccountSearch ass;
        List<account> account_list;
        account account_obj;
        account obj;
        string mode;

        public AccountForm(string mode , AccountSearch AS , account obj)
        {
            InitializeComponent();

            ass = AS;
            this.obj = obj;
            this.mode = mode;

            acc_name_textbox.Focus();
            get_all_accounts();



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
                        cmd.CommandText = "INSERT INTO sms_accounts(account_name,account_desc,account_holder_name,account_holder_cell,account_holder_phn,created_by,date_time) Values(@account_name,@account_desc,@account_holder_name,@account_holder_cell,@account_holder_phn,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@account_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_name;
                        cmd.Parameters.Add("@account_desc", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_desc;
                        cmd.Parameters.Add("@account_holder_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_holder_name;
                        cmd.Parameters.Add("@account_holder_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_holder_cell;
                        cmd.Parameters.Add("@account_holder_phn", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_holder_phn;                        
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = account_obj.date_time;


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
                        cmd.CommandText = "Update sms_accounts SET account_name=@account_name,account_desc=@account_desc,account_holder_name=@account_holder_name,account_holder_cell=@account_holder_cell,account_holder_phn=@account_holder_phn,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@account_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_name;
                        cmd.Parameters.Add("@account_desc", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_desc;
                        cmd.Parameters.Add("@account_holder_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_holder_name;
                        cmd.Parameters.Add("@account_holder_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_holder_cell;
                        cmd.Parameters.Add("@account_holder_phn", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.account_holder_phn;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = account_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = account_obj.date_time;
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
            account_obj = new account();

            account_obj.account_name = acc_name_textbox.Text;
            account_obj.account_desc = acc_desc_textbox.Text;
            account_obj.account_holder_name = acc_holder_name_textbox.Text;
            account_obj.account_holder_cell = acc_cell_textbox.Text;
            account_obj.account_holder_phn = acc_phone_textbox.Text;
            account_obj.date_time = DateTime.Now;
            account_obj.created_by = MainWindow.emp_login_obj.emp_user_name;


        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            acc_name_textbox.Text = obj.account_name;
            acc_desc_textbox.Text = obj.account_desc;
            acc_holder_name_textbox.Text = obj.account_holder_name;
            acc_cell_textbox.Text = obj.account_holder_cell;
            acc_phone_textbox.Text = obj.account_holder_phn;
            
                
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (acc_name_textbox.Text.Trim().Length == 0)
            {
                acc_name_textbox.Focus();
                string alertText = "Account Name Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }            
            else
            {
                return true;
            }

        }

        //------------           Check Exam Name   -------------------

        bool check_exam()
        {
            foreach (account s in account_list)
            {
                if (s.account_name.ToString().ToUpper().Equals(account_obj.account_name.ToString().ToUpper()))
                {
                    return true;
                }
            }
            return false;
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
                        MessageBox.Show("Record Added Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        ass.load_grid();
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
                        MessageBox.Show("Record Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        ass.load_grid();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }

                }
                else
                {
                    MessageBox.Show("mode not set", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
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
