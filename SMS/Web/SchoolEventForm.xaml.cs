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
using SMS.Web;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.Web
{
    /// <summary>
    /// Interaction logic for SchoolEventForm.xaml
    /// </summary>
    public partial class SchoolEventForm : Window
    {
        web_events obj;
        web_events event_obj;
        SchoolEvents se;
        string mode;

        public SchoolEventForm(string mode, SchoolEvents SE, web_events obj)
        {
            InitializeComponent();

            se = SE;
            this.obj = obj;
            this.mode = mode;

            if (mode == "edit")
            {
                fill_control();
            }
        }

        private void submit_btn_Click(object sender, RoutedEventArgs e)
        {
            save();
        }


        //Fill object

        public void fill_object() 
        {
            event_obj = new web_events();
            if(mode == "edit")
            {
                event_obj.id = obj.id;
            }

            if (event_date.SelectedDate != null)
            {
                event_obj.event_date = event_date.SelectedDate.Value;
            }
            event_obj.event_name = event_name.Text.Trim();
            event_obj.event_description = event_description.Text.Trim();
            event_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            event_obj.date_time = DateTime.Now;
        }

        // Fill Control


        public void fill_control() 
        {
            event_date.SelectedDate = obj.event_date;
            event_name.Text = obj.event_name;
            event_description.Text = obj.event_description;
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {           

            if (event_date.SelectedDate == null)
            {
                event_date.Focus();
                string alertText = "Event Date Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (event_name.Text.Length == 0)
            {
                event_name.Focus();
                string alertText = "Event Name Should Not Be Blank";
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
                        se.load_grid();
                        this.Close();
                        
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
                        se.load_grid();
                        this.Close();
                        
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
                        cmd.CommandText = "INSERT INTO web_events(event_date,event_name,event_description,created_by,date_time) Values(@event_date,@event_name,@event_description,@created_by,@date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@event_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = event_obj.event_date;
                        cmd.Parameters.Add("@event_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_name;
                        cmd.Parameters.Add("@event_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_description;                        
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = event_obj.date_time;

                        

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

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
                        cmd.CommandText = "Update web_events SET event_date=@event_date,event_name=@event_name,event_description=@event_description,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.id;
                        cmd.Parameters.Add("@event_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = event_obj.event_date;
                        cmd.Parameters.Add("@event_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_name;
                        cmd.Parameters.Add("@event_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_description;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = event_obj.date_time;

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
    }
}
