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
using SMS.ClassManagement.Section;
using MahApps.Metro.Controls;
using SMS.ClassManagement.Class;
using MySql.Data.MySqlClient;
using SMS.Models;


namespace SMS.ClassManagement.Section
{
    /// <summary>
    /// Interaction logic for SectionForm.xaml
    /// </summary>
    public partial class SectionForm : Window
    {
        
        SectionSearch ss;
        List<classes> classes_list;
        List<employees> emp_list;
        List<sections> sections_list;
        sections sections_obj;
        sections obj;
        string mode;

        public SectionForm(string mode, SectionSearch SS , sections obj)
        {
            InitializeComponent();
            ss = SS;
            this.obj = obj;
            this.mode = mode;

           
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
                        cmd.CommandText = "INSERT INTO sms_subjects(class_id,class_name,section_name,is_active,created_by,date_time,roll_no_format) Values(@class_id,@class_name,@section_name,@is_active,@created_by,@date_time,@roll_no_format)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_name;
                        cmd.Parameters.Add("@roll_no_format", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.roll_no_format;
                        //cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_id;
                        //cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_name;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.section_name;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sections_obj.date_time;      

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch
            {
                MessageBox.Show("Sections DB not connected");
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
                        cmd.CommandText = "Update sms_subjects SET class_id=@class_id,class_name=@class_name,section_name=@section_name,is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation,roll_no_format=@roll_no_format WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_name;
                        //cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_id;
                        //cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_name;
                        cmd.Parameters.Add("@roll_no_format", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.roll_no_format;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.section_name;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sections_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        con.Open();                        
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Get All Classes    ----------------------------------
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
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                             };
                            classes_list.Add(classes);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Classes DB not connected");
            }
        }

        //---------------           Get All Teachers    ----------------------------------
        public void get_all_emp()
        {
            try
            {

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_emp where is_active='Y'";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                            };
                            emp_list.Add(emp);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Teacher DB not connected");
            }
        }

        //------------         Get All Sections   ------------------------

        public void get_all_sections()
        {


            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                //emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                //emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),


                            };
                            sections_list.Add(section);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("DB not connected");
                    }

                }
            }
        }
        
        //------------------    Fill Object      ------------------------
        public void fill_object() 
        {
            sections_obj = new sections();

            classes cl = (classes)class_cmb.SelectedItem;
            string cl_id = cl.id.ToString();
            string cl_name = cl.class_name.ToString();

            //employees emp = (employees)teacher_cmb.SelectedItem;
            //string emp_id = emp.id.ToString();
            //string emp_name = emp.emp_name.ToString();

            sections_obj.roll_no_format = roll_no_textbox.Text;
            sections_obj.class_id = cl_id;
            sections_obj.class_name = cl_name;
            //sections_obj.emp_id = emp_id;
            //sections_obj.emp_name = emp_name;
            sections_obj.section_name = section_name_textbox.Text.Trim();
            sections_obj.date_time = DateTime.Now;
            sections_obj.created_by = MainWindow.emp_login_obj.emp_user_name;

            if (is_active_chekbox.IsChecked == true)
            {
                sections_obj.is_active = "Y";
            }
            else 
            {
                sections_obj.is_active = "N";
            }
        }

        //------------------    Fill Control     -------------------------
        public void fill_control() 
        {
            class_cmb.SelectedValue=obj.class_id;
            section_name_textbox.Text = obj.section_name;
            roll_no_textbox.Text = obj.roll_no_format;
            //teacher_cmb.SelectedValue = obj.emp_id;
            if (obj.is_active == "Y")
            {
                is_active_chekbox.IsChecked = true;
            }
            else 
            {
                is_active_chekbox.IsChecked = false;
            }
        }

        //------------------    Validation       -------------------------
        public bool validate()
        {
            if (class_cmb.SelectedIndex==0)
            {
                class_cmb.Focus();
                string alertText = "Class Name Should Not Be Blank.";
                MessageBox.Show(alertText);
                return false;
            }

            else if (section_name_textbox.Text.Trim().Length==0)
            {
                section_name_textbox.Focus();
                string alertText = "Section Name Should Not Be Blank"; 
                MessageBox.Show(alertText);
                return false;
            }
            else if (roll_no_textbox.Text.Trim().Length == 0)
            {
                section_name_textbox.Focus();
                string alertText = "Roll# Format Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if(mode=="insert")
            {
                if (check_section() == true)
                {
                    string alertText = "Section Name Already Exists, Please Choose A Different Section Name";
                    section_name_textbox.Focus();
                    MessageBox.Show(alertText);
                    return false;
                }
                return true;
            }
            
            
            else
            {
                return true;
            }

        }

        //------------           Check Section Name   -------------------
        bool check_section() 
        {
            foreach(sections s in sections_list)
            {
                if(s.section_name.ToString().ToUpper().Equals(sections_obj.section_name.ToString().ToUpper()))
                {
                    if (s.class_name.ToString().ToUpper().Equals(sections_obj.class_name.ToString().ToUpper())) 
                    {
                        return true;
                    }
                    
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
                        MessageBox.Show("Record Added Successfully");
                        this.Close();
                        ss.load_grid();
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
                        ss.load_grid();
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
            if(e.Key==Key.Enter)
            {
                save();
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            class_cmb.Focus();
            sections_list = new List<sections>();
            get_all_sections();

            classes_list = new List<classes>();
            emp_list = new List<employees>();

            get_all_classes();
            get_all_emp();

            class_cmb.SelectedIndex = 0;
            //teacher_cmb.SelectedIndex = 0;

            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            emp_list.Insert(0, new employees() { emp_name = "---Select Teacher---", id = "-1" });

            class_cmb.ItemsSource = classes_list;
            //teacher_cmb.ItemsSource = emp_list;

            if (mode == "edit")
            {
                // class_cmb.IsEnabled =false;
                // section_name_textbox.IsEnabled = false;
                fill_control();
            }
        }
    }
}
