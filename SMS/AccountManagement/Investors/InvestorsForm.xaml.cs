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
using SMS.AccountManagement.Investors;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.AccountManagement.Investors
{
    /// <summary>
    /// Interaction logic for InvestorsForm.xaml
    /// </summary>
    public partial class InvestorsForm : Window
    {
        InverstersPage inp;        
        List<sms_investor> investors_list;
        sms_investor investor_obj;
        sms_investor obj;
        string mode;

        public InvestorsForm(string mode,InverstersPage INP,sms_investor obj)
        {
            InitializeComponent();
            inp = INP;
            this.obj = obj;
            this.mode = mode;
            investor_name_textbox.Focus();

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
                        cmd.CommandText = "INSERT INTO sms_investors(investor_name,investor_cell,investor_address,investor_description,created_by,date_time) Values(@investor_name,@investor_cell,@investor_address,@investor_description,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@investor_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_name;
                        cmd.Parameters.Add("@investor_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_cell;
                        cmd.Parameters.Add("@investor_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_address;
                        cmd.Parameters.Add("@investor_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_description;                        
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = investor_obj.date_time;


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
                        cmd.CommandText = "Update sms_investors SET investor_name=@investor_name,investor_cell=@investor_cell,investor_address=@investor_address,investor_description=@investor_description,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@investor_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_name;
                        cmd.Parameters.Add("@investor_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_cell;
                        cmd.Parameters.Add("@investor_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_address;
                        cmd.Parameters.Add("@investor_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.investor_description;                        
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = investor_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = investor_obj.date_time;
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

     
        //------------------    Fill Object      ------------------------

        public void fill_object()
        {
            investor_obj = new sms_investor();

            investor_obj.investor_name = investor_name_textbox.Text.Trim();
            investor_obj.investor_cell = investor_cell_textbox.Text;
            investor_obj.investor_address = investor_address_textbox.Text;
            investor_obj.investor_description = investor_desc_textbox.Text;
            investor_obj.date_time = DateTime.Now;
            investor_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            investor_name_textbox.Text = obj.investor_name;
            investor_cell_textbox.Text = obj.investor_cell;
            investor_address_textbox.Text = obj.investor_address;
            investor_desc_textbox.Text = obj.investor_description;
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (investor_name_textbox.Text.Trim().Length == 0)
            {
                investor_name_textbox.Focus();
                string alertText = "Investor Name Should Not Be Blank";
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
