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
using MahApps.Metro.Controls;
using SMS.Models;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace SMS.ClassManagement.Class
{
    /// <summary>
    /// Interaction logic for ClassFormNew.xaml
    /// </summary>
    public partial class ClassFormNew : Window
    {
        classes obj;
        classes class_obj;
        string mode;
        ClassSearch CS;
        List<classes> classes_list;

        public ClassFormNew(string m, ClassSearch cs, classes ob)
        {
            InitializeComponent();
            obj = new classes();
            mode = m;
            CS = cs;
            this.obj = ob;

        }
        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }


        //--------------    Save        ------------------------
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
                        CS.load_gird();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else if (mode == "edit")
                {

                    if (update() == 0)
                    {
                        MessageBox.Show("Record Updated Successfully");
                        this.Close();
                        CS.load_gird();
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

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // ---------- ----    Fill Control        -----------------------------
        public void fill_control()
        {
            class_name_textbox.Text = obj.class_name;
            
            reg_fee_textbox.Text = obj.reg_fee;
            adm_fee_textbox.Text = obj.adm_fee;
            
            tutuion_fee_textbox.Text = obj.tution_fee;
            transport_chrgs_textbox.Text = obj.transport_charges;
            
            
            exam_fee_textbox.Text = obj.exam_fee;
            
            other_exp_textbox.Text = obj.other_exp;

            

        }

        // ---------- ----    Get all Classes        -----------------------------
        public void get_all_classes()
        {

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                transport_charges = Convert.ToString(reader["transport_charges"].ToString()),
                                boarding_fee = Convert.ToString(reader["boarding_fee"].ToString()),
                                misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                stationary_charges = Convert.ToString(reader["stationary_charges"].ToString()),
                                books_charges = Convert.ToString(reader["books_charges"].ToString()),
                                other_exp = Convert.ToString(reader["other_exp"].ToString()),
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),


                            };
                            classes_list.Add(classes);

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //-----------------     Submit Form        -------------------------------
        public int submit()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_classes(class_name,reg_fee,adm_fee,security_fee,tution_fee,transport_charges,boarding_fee,misc_charges,exam_fee,stationary_charges,books_charges,other_exp,roll_no_format,is_active,date_time,created_by) Values(@class_name,@reg_fee,@adm_fee,@security_fee,@tution_fee,@transport_charges,@boarding_fee,@misc_charges,@exam_fee,@stationary_charges,@books_charges,@other_exp,@roll_no_format,@is_active,@date_time,@created_by)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.class_name;
                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.adm_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = transport_chrgs_textbox.Text;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.tution_fee;
                        cmd.Parameters.Add("@transport_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@boarding_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.exam_fee;
                        cmd.Parameters.Add("@stationary_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@books_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.other_exp;
                        cmd.Parameters.Add("@roll_no_format", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.is_active;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = class_obj.date_time;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.created_by;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());

                    }
                }
            }
            catch
            {
                MessageBox.Show("Classes DB not connected");
            }
            return i;
        }

        //---------------           Update Form        ---------------------------------
        public int update()
        {
            int i = 1;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_classes SET class_name=@class_name,reg_fee=@reg_fee,adm_fee=@adm_fee,security_fee=@security_fee,tution_fee=@tution_fee,transport_charges=@transport_charges,boarding_fee=@boarding_fee,misc_charges=@misc_charges,exam_fee=@exam_fee,stationary_charges=@stationary_charges,books_charges=@books_charges,other_exp=@other_exp,roll_no_format=@roll_no_format,is_active=@is_active,date_time=@date_time,created_by=@created_by,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.adm_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = transport_chrgs_textbox.Text;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.tution_fee;
                        cmd.Parameters.Add("@transport_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@boarding_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.exam_fee;
                        cmd.Parameters.Add("@stationary_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@books_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.other_exp;
                        cmd.Parameters.Add("@roll_no_format", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.is_active;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = class_obj.date_time;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.created_by;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.class_name;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch
            {
                MessageBox.Show("classes DB no Connected");
            }
            return i;
        }

        //-----------------     Fill Object       --------------------------------
        public void fill_object()
        {
            class_obj = new classes();

            class_obj.class_name = class_name_textbox.Text.Trim();
            class_obj.reg_fee = reg_fee_textbox.Text.Trim();
            class_obj.adm_fee = adm_fee_textbox.Text.Trim();
            
            class_obj.tution_fee = tutuion_fee_textbox.Text.Trim();
            class_obj.transport_charges = transport_chrgs_textbox.Text.Trim();
            
            class_obj.exam_fee = exam_fee_textbox.Text.Trim();
            
            class_obj.other_exp = other_exp_textbox.Text.Trim();
            
            class_obj.date_time = DateTime.Now;
            class_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
        }

        //------------------------   Validation     ----------------------------
        public bool validate()
        {
            if (class_name_textbox.Text.Trim().Length == 0)
            {
                class_name_textbox.Focus();
                string alertText = "Class Name Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }

            else if (mode == "insert")
            {
                if (check_class_name() == true)
                {
                    string alertText = "Class Name Already Exists, Please Choose A Different Class Name";
                    class_name_textbox.Focus();
                    MessageBox.Show(alertText);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            else
            {
                return true;
            }

        }

        //--------------------    check class name         ---------------------
        public bool check_class_name()
        {

            foreach (classes cl in classes_list)
            {

                if (cl.class_name.ToUpper().ToString().Equals(class_obj.class_name.ToUpper().ToString()))
                {
                    return true;
                }


            }

            return false;
        }

        //--------------------    check Roll No Format         ---------------------
        public bool check_rollno_format()
        {

            foreach (classes cl in classes_list)
            {

                if (cl.roll_no_format.ToUpper().ToString().Equals(class_obj.roll_no_format.ToUpper().ToString()))
                {
                    return true;
                }


            }

            return false;
        }


        //--------------------    check Roll No Format on edit mode        ---------------------
        public bool check_rollno_format_edit()
        {

            foreach (classes cl in classes_list)
            {
                if (cl.id != obj.id)
                {
                    if (cl.roll_no_format.ToUpper().ToString().Equals(class_obj.roll_no_format.ToUpper().ToString()))
                    {
                        return true;
                    }
                }


            }

            return false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            class_name_textbox.Focus();
           

            classes_list = new List<classes>();
            get_all_classes();


            if (mode == "edit")
            {
                class_name_textbox.IsEnabled = true;
                fill_control();
            }
        }
    }
}
