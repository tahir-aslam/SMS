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
using SMS.AdmissionManagement.FineManagement.Fine;
using SMS.Models;
using SMS.Common;
using MySql.Data.MySqlClient;
using SMS.Common;

namespace SMS.AdmissionManagement.FineManagement.Fine
{   
    public partial class FineWindow : Window
    {
        public static List<admission> admList;
        List<sms_fine_type> fine_type_list;
        List<sms_months> months_list;

        sms_fine obj;
        FineSearch FS;
        admission admObj;
        string mode;
        string smsFineId;
        int fine = 0;
        int rem_fine = 0;
        int other = 0;
        List<fee> paid_fee_list;
        List<sms_InsertUpdateStatus> statusList;
        sms_InsertUpdateStatus statusObj;

        public FineWindow(string mode, FineSearch fs, sms_fine sf)
        {
            InitializeComponent();

            statusList = new List<sms_InsertUpdateStatus>();
            admList = new List<admission>();            
            this.FS = fs;
            this.mode = mode;
            fineDate.SelectedDate = DateTime.Now;
            

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;

            get_all_fine_types();
            fine_type_cmb.SelectedIndex = 0;
            fine_type_list.Insert(0, new sms_fine_type() { fine_type = "--Select Type--", id = "-1" });
            fine_type_cmb.ItemsSource = fine_type_list;

            if (DateTime.Now.Day > 10)
            {
                month_cmb.SelectedItem = months_list.Where(x => x.month_id == (DateTime.Now.AddMonths(1).Month).ToString()).First();
            }
            else 
            {
                month_cmb.SelectedItem = months_list.Where(x => x.month_id == (DateTime.Now.Month).ToString()).First();
            }

            if (mode == "edit")
            {
                this.obj = sf;
                admObj = new admission();
                admObj.id = obj.std_id;
                admObj.std_name = obj.std_name;
                smsFineId = sf.id;

                fill_control(admObj);
            }
        }

        //-----------       Get All Fine Types    ----------------------
        public void get_all_fine_types()
        {
            fine_type_list = new List<sms_fine_type>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_fine_types";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fine_type acc = new sms_fine_type()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                fine_type = Convert.ToString(reader["fine_type"].ToString()),
                            };
                            fine_type_list.Add(acc);

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

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            StudentSelectionWindow slw = new StudentSelectionWindow();
            slw.ShowDialog();

            if (admList.Count > 0)
            {
                stdListview.ItemsSource = admList;
                selectedCountTB.Text = admList.Count.ToString();
            }
            else 
            {
                selectedCountTB.Text = "0";
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

        //---------------           Submit Form    ----------------------------------
        public int submit()
        {
            int i = 0;
            try
            {
                foreach (admission adm in admList)
                {
                    statusObj = new sms_InsertUpdateStatus();
                    statusObj.adm_no = adm.adm_no;
                    statusObj.std_name = adm.std_name;
                    statusObj.operation = "Fine";

                    if (check_fee_paid(adm.id, obj.month))
                    {
                        statusObj.status = "Error";
                    }
                    else
                    {
                        statusObj.status = "Success";
                        getFeeData(obj.month, adm.id);
                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_fine(std_id,session_id,fine_type_id,fine_type,fine_description,amount,created_by,fine_date,month,monthId,date_time) Values(@std_id,@session_id,@fine_type_id,@fine_type,@fine_description,@amount,@created_by,@fine_date,@month,@monthId,@date_time)";
                                cmd.Connection = con;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                cmd.Parameters.Add("@fine_type_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.fine_type_id;
                                cmd.Parameters.Add("@fine_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.fine_type;
                                cmd.Parameters.Add("@fine_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.fine_description;
                                cmd.Parameters.Add("@amount", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.amount;
                                cmd.Parameters.Add("@fine_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = obj.fine_date;
                                cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.month;
                                cmd.Parameters.Add("@monthId", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.monthId;
                                cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.created_by;
                                cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = obj.date_time;

                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                con.Close();
                            }
                        }

                        using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_fee = new MySqlCommand())
                            {
                                cmd_fee.CommandText = "Update sms_fee SET fine_fee=@fine_fee, rem_fine_fee=@rem_fine_fee where std_id = @std_id && month=@months && session_id=" + MainWindow.session.id;
                                cmd_fee.Connection = con_fee;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                                cmd_fee.Parameters.Add("@months", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.month;

                                fine = fine + Convert.ToInt32(obj.amount);
                                rem_fine = rem_fine + Convert.ToInt32(obj.amount);

                                cmd_fee.Parameters.Add("@fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine.ToString();
                                cmd_fee.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = rem_fine.ToString();

                                con_fee.Open();
                                cmd_fee.ExecuteScalar();
                                con_fee.Close();
                            }
                        }
                    }
                    statusList.Add(statusObj);
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
            obj = new sms_fine();         

            admission adm = (admission)stdListview.Items[0];
            sms_fine_type sft = (sms_fine_type)fine_type_cmb.SelectedItem;
            sms_months sm = (sms_months)month_cmb.SelectedItem;

            obj.id = smsFineId;
            obj.std_name = adm.std_name;
            obj.adm_no = adm.adm_no;
            obj.std_id = adm.id;
            obj.fine_type_id = sft.id;
            obj.fine_type = sft.fine_type;
            obj.fine_description = fineDescriptionTB.Text;
            obj.fine_date = fineDate.SelectedDate.Value;
            obj.amount = amount_textbox.Text;
            obj.month = sm.month_name;
            obj.monthId = sm.month_id;
            obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            obj.date_time = DateTime.Now;
        }

        //------------------    Fill Control     -------------------------
        public void fill_control(admission adm)
        {
            try
            {
                stdListview.Items.Clear();
                stdListview.Items.Add(adm);
                stdListview.IsEnabled = false;
                selectButton.Visibility = Visibility.Hidden;

                fine_type_cmb.SelectedIndex = Convert.ToInt32(obj.fine_type_id);
                fineDescriptionTB.Text = obj.fine_description;
                amount_textbox.Text = obj.amount;
                month_cmb.SelectedItem = months_list.Where(x=>x.month_id == obj.monthId).First();
                //month_cmb.SelectedIndex = Convert.ToInt32(obj.monthId);
                fineDate.SelectedDate = obj.fine_date;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------    Validation       -------------------------
        public bool validate()
        {
            if(stdListview.Items.Count < 1)
            {
                stdListview.Focus();
                string alertText = "Select Minimum One Student";
                MessageBox.Show(alertText, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            else if (fine_type_cmb.SelectedIndex == 0)
            {
                fine_type_cmb.Focus();
                string alertText = "Fine Type Should Not Be Blank";
                MessageBox.Show(alertText,"Error",MessageBoxButton.OK,MessageBoxImage.Stop);
                return false;
            }
            else if (amount_textbox.Text == "")
            {
                amount_textbox.Focus();
                string alertText = "Amount Should Not Be Blank";
                MessageBox.Show(alertText, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            else if (fineDate.SelectedDate == null)
            {
                fineDate.Focus();
                string alertText = "Date Should Not Be Blank";
                MessageBox.Show(alertText, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            else if (month_cmb.SelectedIndex == 0)
            {
                month_cmb.Focus();
                string alertText = "Month Should Not Be Blank";
                MessageBox.Show(alertText, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
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
                        //MessageBox.Show("Record Added Successfully");
                        StatusWindow sw = new Common.StatusWindow(statusList);
                        sw.ShowDialog();
                        this.Close();
                        FS.load_grid();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else if (mode == "edit")
                {                    
                }
                else
                {
                    MessageBox.Show("mode not set", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

            }
        }

        //get sms_fee
        public void getFeeData(string month, string id) 
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where std_id = @id && month=@month && session_id =" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;
                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {                            
                            fine = Convert.ToInt32(reader["fine_fee"]);
                            rem_fine = Convert.ToInt32(reader["rem_fine_fee"]);            
                        };

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public bool check_fee_paid(string std_id,string month)
        {    
            int total=0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where std_id=@id && month=@month && session_id=" + MainWindow.session.id;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = std_id;
                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month;
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
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),                                
                            };
                            try
                            {
                                //total = Convert.ToInt32(paid_fee.reg_fee) +Convert.ToInt32(paid_fee.adm_fee) +Convert.ToInt32(paid_fee.security_fee) +Convert.ToInt32(paid_fee.exam_fee) +Convert.ToInt32(paid_fee.transport_fee) +Convert.ToInt32(paid_fee.tution_fee) +Convert.ToInt32(paid_fee.fine_fee) +Convert.ToInt32(paid_fee.other_expenses);
                                total = total +  Convert.ToInt32(paid_fee.fine_fee);
                            }
                            catch(Exception ex)
                            {
                                total = 1;
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
            if (total > 0)
            {
                return true;
            }
            else
            {
                return false;
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
