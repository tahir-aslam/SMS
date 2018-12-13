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
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.Messaging;
using SMS.DAL;

namespace SMS.Messaging.FriendList
{
    /// <summary>
    /// Interaction logic for FriendListSelectedWindow.xaml
    /// </summary>
    public partial class FriendListSelectedWindow : Window
    {
        public static List<friend_list> friends_list;
        GeneralSms gs;
        MiscDAL miscDAL;
        int selected_count = 0;

        public FriendListSelectedWindow(GeneralSms GS)
        {
            InitializeComponent();
            get_all_friends();
           // friends_grid.ItemsSource = friends_list;
            this.gs = GS;
            miscDAL = new MiscDAL();
            List<sms_freind_type> freind_type_list = miscDAL.get_all_freind_type();
            freind_type_list.Insert(0,new sms_freind_type(){id = -1, freind_type="ALL Freinds"});
            freind_type_cmb.ItemsSource = freind_type_list;
            freind_type_cmb.SelectedIndex = 0;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            friend_list friends_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;
            selected_count = 0;

            for (int i = 0; i < friends_grid.Items.Count; i++)
            {
                friends_obj = (friend_list)friends_grid.Items[i];
                friends_obj.Checked = checkBox.IsChecked.Value;                
            }
            friends_grid.Items.Refresh();
            selected_count = friends_list.Where(x => x.Checked == true).Count();
            count_text.Text = selected_count.ToString();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            friends_grid.SelectedItem = e.Source;
            friend_list fl = new friend_list();
            fl = (friend_list)friends_grid.SelectedItem;
            foreach (friend_list ede in friends_list)
            {
                if (fl.id == ede.id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
                    if (checkBox.IsChecked.Value)
                    {
                        selected_count++;
                    }
                    else
                    {
                        selected_count--;
                    }
                }
            }
            count_text.Text = selected_count.ToString();

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
                                friend_cell = Convert.ToString(reader["friend_cell"].ToString()),
                                freind_type_id = Convert.ToInt32(reader["freind_type_id"]),
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

        private void click_save(object sender, RoutedEventArgs e)
        {
            this.Close();
            int count = 0;
            foreach(friend_list fl in friends_list.Where(x=>x.Checked==true))
            {
                count++;
            }
            gs.friends_count.Text = count.ToString();
            if(count == 0)
            {
                gs.frnd_chkbox.IsChecked = false;
            }
        }

        private void freind_type_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(freind_type_cmb.SelectedItem != null)
            {
                if (freind_type_cmb.SelectedIndex != 0)
                {
                    sms_freind_type type = freind_type_cmb.SelectedItem as sms_freind_type;
                    friends_grid.ItemsSource = friends_list.Where(x => x.freind_type_id == type.id);
                    friends_grid.Items.Refresh();
                }
                else 
                {
                    friends_grid.ItemsSource = friends_list;
                    friends_grid.Items.Refresh();
                }
            }
            
        }
    }
}
