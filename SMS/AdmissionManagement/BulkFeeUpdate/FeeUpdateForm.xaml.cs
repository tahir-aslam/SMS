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
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace SMS.AdmissionManagement.BulkFeeUpdate
{
    /// <summary>
    /// Interaction logic for FeeUpdateForm.xaml
    /// </summary>
    public partial class FeeUpdateForm : Window
    {
        public List<admission> adm_list_new;
        List<sms_months> months_list;
        List<fee> paid_fee_list;
        List<other_fee_type> fee_type_list;
        List<fee> fee_list;

        int reg = 0;
        int security = 0;
        int exam = 0;
        int other = 0;
        string month_name = "";

        public FeeUpdateForm(List<admission> adm_list)
        {
            InitializeComponent();
            adm_list_new = new List<admission>();
            foreach (admission adm in adm_list.Where(x => x.Checked == true))
            {
                adm_list_new.Add(adm);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void click_save(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                update_tution_fee();
                MessageBox.Show("Updated Successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void fee_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fee_cmb.SelectedIndex == 2)
            {
                other_cmb.IsEnabled = true;
                month_cmb.IsEnabled = true;
                date_picker.IsEnabled = true;
            }
            else
            {
                try
                {
                    month_cmb.IsEnabled = false;
                    other_cmb.IsEnabled = false;
                    date_picker.IsEnabled = false;

                }
                catch (Exception ex) { }

            }
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public bool validate()
        {
            if (fee_cmb.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Fee Type", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                fee_cmb.Focus();
                return false;
            }
            else if (fee_cmb.SelectedIndex == 2 && month_cmb.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Month To Update Other Fee", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                month_cmb.Focus();
                return false;
            }
            else if (fee_cmb.SelectedIndex == 2 && other_cmb.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Other Type To Update Other Fee", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                other_cmb.Focus();
                return false;
            }
            else if (fee_cmb.SelectedIndex == 2 && date_picker.SelectedDate == null)
            {
                MessageBox.Show("Please Select Date To Update Other Fee", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                other_cmb.Focus();
                return false;
            }                            
            else if (amount_option.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Amount Option", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                month_cmb.Focus();
                return false;
            }
            else if (fee_textbox.Text == "")
            {
                MessageBox.Show("Please Enter Amount", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                fee_textbox.Focus();
                return false;
            }
            else
            {
                return true;
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

        //---------------           Get All Fee Types    ----------------------------------
        public void get_all_fee_types()
        {
            fee_type_list = new List<other_fee_type>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_other_fee_types";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            other_fee_type type = new other_fee_type()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                fee_type = Convert.ToString(reader["fee_type"]),
                            };
                            fee_type_list.Add(type);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void amount_option_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (amount_option.SelectedIndex == 0)
            {
                try
                {
                    fee_textbox.IsEnabled = false;
                }
                catch (Exception ex) { }
            }
            else
            {
                try
                {
                    fee_textbox.IsEnabled = true;
                }
                catch (Exception ex) { }
            }
        }

        private void other_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void check_fee_paid(string std_id)
        {
            reg = 0;
            security = 0;
            exam = 0;
            other = 0;


            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where std_id =" + std_id + "&& session_id=" + MainWindow.session.id;

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
                            };
                            paid_fee_list.Add(paid_fee);

                        }
                    }
                }
                foreach (fee f in paid_fee_list)
                {
                    reg = reg + Convert.ToInt32(f.reg_fee);
                    security = security + Convert.ToInt32(f.security_fee);
                    exam = exam + Convert.ToInt32(f.exam_fee);

                    if (fee_cmb.SelectedIndex == 2)
                    {
                        sms_months sm = (sms_months)month_cmb.SelectedItem;
                        if (sm.month_name == f.month)
                        {
                            other = other + Convert.ToInt32(f.other_expenses);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void update_tution_fee()
        {
            foreach (admission adm in adm_list_new)
            {
                check_fee_paid(adm.id);
                get_fee_data(adm.id);

                // for tution fee
                if (fee_cmb.SelectedIndex == 1)
                {
                    try
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Update sms_admission SET tution_fee=@tution_fee, updation=@updation WHERE id = @id && session_id=" + MainWindow.session.id;
                                cmd.Connection = con;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }

                        string[] months = new string[12] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                        for (int j = 0; j < 12; j++)
                        {
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {
                                    cmd_fee.CommandText = "Update sms_fee SET tution_fee=@tution_fee, rem_tution_fee=@rem_tution_fee where std_id = @std_id && month =@months && session_id=" + MainWindow.session.id;
                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                    cmd_fee.Parameters.Add("@months", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = months[j].ToString();

                                    if (amount_option.SelectedIndex == 1)
                                    {
                                        cmd_fee.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                        cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                    }
                                    else
                                    {
                                        fee f = fee_list.Where(x => x.month == months[j]).First();
                                        cmd_fee.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(fee_textbox.Text) + f.rem_tution_fee).ToString();
                                    }

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                // ---------------------for other expenses------------------------------------
                else if (fee_cmb.SelectedIndex == 2)
                {
                    try
                    {
                        sms_months sm = new sms_months();
                        sm = (sms_months)month_cmb.SelectedItem;
                        month_name = sm.month_name;

                        if (other > 0 && amount_option.SelectedIndex == 1)
                        {
                            MessageBox.Show("You Cannot Replace Fee If You Have Already Paid other amount of this month or Please contact with your service provider","Waring",MessageBoxButton.OK,MessageBoxImage.Warning);
                        }
                        else
                        {
                            
                            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_admission SET other_exp=@other_exp, updation=@updation WHERE id = @id && session_id=" + MainWindow.session.id;
                                    cmd.Connection = con;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                    cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;

                                    cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }

                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {
                                    cmd_fee.CommandText = "Update sms_fee SET other_exp=@other_exp, rem_other_exp=@rem_other_exp where std_id = @std_id && month =@months && session_id=" + MainWindow.session.id;
                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                    cmd_fee.Parameters.Add("@months", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month_name;

                                    if (amount_option.SelectedIndex == 1)
                                    {
                                        cmd_fee.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                        cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                    }
                                    else
                                    {
                                        fee f = fee_list.Where(x => x.month == month_name).First();
                                        cmd_fee.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(fee_textbox.Text) + Convert.ToInt32(f.other_expenses)).ToString();
                                        cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = (Convert.ToInt32(fee_textbox.Text) + Convert.ToInt32(f.rem_other_fee)).ToString();
                                    }

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }

                            other_fee_type type = (other_fee_type)other_cmb.SelectedItem;

                            // Insert into sms_other_fee
                            using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_paid = new MySqlCommand())
                                {
                                    cmd_paid.CommandText = "insert into sms_other_fee (std_id,session_id,fee_type_id,fee_type,amount,date,month_id,month_name,date_time,created_by)Values(@std_id,@session_id,@fee_type_id,@fee_type,@amount,@date,@month_id,@month_name,@date_time,@created_by)";
                                    cmd_paid.Connection = con_paid;

                                    cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = adm.id;
                                    cmd_paid.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = MainWindow.session.id;
                                    cmd_paid.Parameters.Add("@fee_type_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = type.id;
                                    cmd_paid.Parameters.Add("@fee_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = type.fee_type;
                                    cmd_paid.Parameters.Add("@amount", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = fee_textbox.Text;
                                    cmd_paid.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = date_picker.SelectedDate;
                                    cmd_paid.Parameters.Add("@month_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = sm.month_id;
                                    cmd_paid.Parameters.Add("@month_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sm.month_name;

                                    cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                                    cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;


                                    con_paid.Open();
                                    cmd_paid.ExecuteScalar();
                                    con_paid.Close();
                                }
                            }
                        
                        }
                    }
                        
                    
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                    // ------------------For reg_fee Annul fund--------------------------------------------------
                else if (fee_cmb.SelectedIndex == 3)
                {
                    try
                    {
                        if (reg > 0)
                        {
                        }
                        else
                        {
                            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_admission SET reg_fee=@reg_fee, updation=@updation WHERE id = @id && session_id=" + MainWindow.session.id;
                                    cmd.Connection = con;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                    cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;                                   

                                    cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {

                                    cmd_fee.CommandText = "Update sms_fee SET reg_fee=@reg_fee,rem_reg_fee=@rem_reg_fee where std_id = @std_id && session_id=" + MainWindow.session.id;

                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;

                                    cmd_fee.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                    cmd_fee.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

                // ------------------For Exam fee--------------------------------------------------
                else if (fee_cmb.SelectedIndex == 4)
                {
                    try
                    {
                        if (exam > 0)
                        {
                        }
                        else
                        {
                            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_admission SET exam_fee=@exam_fee, updation=@updation WHERE id = @id && session_id=" + MainWindow.session.id;
                                    cmd.Connection = con;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                    cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                    //cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;

                                    cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {

                                    cmd_fee.CommandText = "Update sms_fee SET exam_fee=@exam_fee, rem_exam_fee=@rem_exam_fee where std_id = @std_id && session_id=" + MainWindow.session.id;

                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;

                                    cmd_fee.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                    cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                // ------------------For Security fee--------------------------------------------------
                else if (fee_cmb.SelectedIndex == 5)
                {
                    try
                    {
                        if (security > 0)
                        {
                        }
                        else
                        {
                            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_admission SET security_fee=@security_fee, updation=@updation WHERE id = @id && session_id=" + MainWindow.session.id;
                                    cmd.Connection = con;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                    cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;

                                    cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                            {
                                using (MySqlCommand cmd_fee = new MySqlCommand())
                                {

                                    cmd_fee.CommandText = "Update sms_fee SET security_fee=@security_fee, rem_security_fee=@rem_security_fee where std_id = @std_id && session_id=" + MainWindow.session.id;

                                    cmd_fee.Connection = con_fee;
                                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                    cmd_fee.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;
                                    cmd_fee.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fee_textbox.Text;

                                    con_fee.Open();
                                    cmd_fee.ExecuteScalar();
                                    con_fee.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No record updated", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void get_fee_data(string id)
        {
            fee_list = new List<fee>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where std_id = @id && session_id =" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee f = new fee()
                            {
                                rem_security_fee = Convert.ToString(reader["rem_security_fee"].ToString()),
                                rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                                rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),

                                rem_other_fee = Convert.ToString(reader["rem_other_exp"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp"].ToString()),

                                rem_adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString()),
                                rem_reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString()),
                                rem_fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                isActive = Convert.ToChar(reader["isActive"]),
                            };
                            fee_list.Add(f);
                        };

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            std_lbl.Content = adm_list_new.Count;
            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;
            fee_textbox.IsEnabled = false;

            get_all_fee_types();
            other_cmb.SelectedIndex = 0;
            fee_type_list.Insert(0, new other_fee_type() { fee_type = "---Select Type---", id = -1 });
            other_cmb.ItemsSource = fee_type_list;
        }
    }
    
}
