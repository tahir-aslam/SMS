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
using SMS.Messaging.FriendList;
using MySql.Data.MySqlClient;
using SMS.DAL;



namespace SMS.Messaging.FriendList
{
    /// <summary>
    /// Interaction logic for FriendListForm.xaml
    /// </summary>
    public partial class FriendListForm : Window
    {
        FriendListSearch fls;
        List<friend_list> friends_list;
        List<sms_freind_type> freind_type_list;
        friend_list friend_obj;
        friend_list obj;
        string mode;
        MiscDAL miscDAL;

        public FriendListForm(string mode, FriendListSearch FLS, friend_list obj)
        {
            InitializeComponent();

            fls = FLS;
            this.obj = obj;
            this.mode = mode;

            name_textbox.Focus();
            get_all_friends();
            miscDAL = new MiscDAL();
            freind_type_list = miscDAL.get_all_freind_type();
            freind_type_cmb.ItemsSource = freind_type_list;
            freind_type_cmb.SelectedIndex = 0;

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
                        cmd.CommandText = "INSERT INTO sms_friend_list(friend_name,friend_cell,friend_occupation,created_by,date_time,freind_type_id) Values(@friend_name,@friend_cell,@friend_occupation,@created_by,@date_time,@freind_type_id)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@friend_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.friend_name;
                        cmd.Parameters.Add("@friend_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.friend_cell;
                        cmd.Parameters.Add("@friend_occupation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.friend_occupation;
                        cmd.Parameters.Add("@freind_type_id", MySqlDbType.Int32).Value = friend_obj.freind_type_id;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = friend_obj.date_time;


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
                        cmd.CommandText = "Update sms_friend_list SET friend_name=@friend_name,friend_cell=@friend_cell,friend_occupation=@friend_occupation,created_by=@created_by,date_time=@date_time,updation=@updation, freind_type_id=@freind_type_id WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@friend_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.friend_name;
                        cmd.Parameters.Add("@friend_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.friend_cell;
                        cmd.Parameters.Add("@friend_occupation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.friend_occupation;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = friend_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = friend_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        cmd.Parameters.Add("@freind_type_id", MySqlDbType.Int32).Value = friend_obj.freind_type_id;
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

        //-----------       Get All Friends     ----------------------

        public void get_all_friends()
        {
            friends_list = new List<friend_list>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_friend_list";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            friend_list fl = new friend_list()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                friend_name = Convert.ToString(reader["friend_name"].ToString()),
                                freind_type_id = Convert.ToInt32(reader["freind_type_id"]),
                                friend_cell = Convert.ToString(reader["friend_cell"].ToString()),
                                friend_occupation = Convert.ToString(reader["friend_occupation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                            };
                            friends_list.Add(fl);

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
            friend_obj = new friend_list();

            friend_obj.friend_name = name_textbox.Text.Trim();
            friend_obj.friend_cell = cell_textbox.Text.Trim();
            sms_freind_type type = freind_type_cmb.SelectedItem as sms_freind_type;
            friend_obj.freind_type_id = type.id;
            friend_obj.friend_occupation = occ_textbox.Text.Trim();
            friend_obj.date_time = DateTime.Now;
            friend_obj.created_by = MainWindow.emp_login_obj.emp_user_name;


        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            name_textbox.Text = obj.friend_name;
            cell_textbox.Text = obj.friend_cell;
            occ_textbox.Text = obj.friend_occupation;
            freind_type_cmb.SelectedValue = obj.freind_type_id;
        }

        //------------------    Validation       -------------------------

        public bool validate()
        {
            if (cell_textbox.Text.Trim().Length == 0 || cell_textbox.Text.Length < 10)
            {
                cell_textbox.Focus();
                string alertText = "Cell#  Should Not Be Blank";
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
                        MessageBox.Show("Record Added Successfully","Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        MessageBoxResult mbr = MessageBox.Show("Do You Want To Add More Records ?", "ADD Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (mbr == MessageBoxResult.Yes)
                        {
                            cell_textbox.Text = "";
                            name_textbox.Text = "";
                            occ_textbox.Text = "";

                        }
                        else
                        {
                            this.Close();
                        }
                        fls.load_grid();
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
                        fls.load_grid();
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
