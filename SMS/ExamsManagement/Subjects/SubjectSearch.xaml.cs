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
using MySql.Data.MySqlClient;
using SMS.Models;
using SMS.ClassManagement.Subject;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;

namespace SMS.ExamsManagement.Subjects
{
    /// <summary>
    /// Interaction logic for SubjectSearch.xaml
    /// </summary>
    public partial class SubjectSearch : Page
    {
        List<subjects> subjects_list;
        SubjectForm SF;
        subjects obj;
        string mode;
        string insertion;
        string updation;

        public SubjectSearch()
        {
            InitializeComponent();
            subjects_list = new List<subjects>();
            obj = new subjects();
            SearchTextBox.Focus();
            load_grid();
        }

        public void load_grid()
        {
            subjects_list.Clear();
            get_all_subjects();
            subjects_grid.ItemsSource = subjects_list;
            this.subjects_grid.Items.Refresh();
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            mode = "insert";
            obj = null;
            SF = new SubjectForm(mode, this, obj);
            SF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SF.ShowDialog();
        }

        //-------------     Editing          ---------------------------

        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void subjects_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            obj = (subjects)subjects_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                SF = new SubjectForm(mode, this, obj);
                SF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                SF.ShowDialog();
            }
        }

        //-------------     Delete          ---------------------------

        private void click_delete(object sender, RoutedEventArgs e)
        {
            obj = (subjects)subjects_grid.SelectedItem;
            if (obj == null)
            {
                MessageBox.Show("Please Select A Row");
            }
            else
            {
                MessageBoxResult mbr = MessageBox.Show("Are You Want To Delete This Record ?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    bool check = check_insertion(obj.id);
                    if (delete() == 1)
                    {
                        if (check == false)
                        {
                            //insert_deleted(obj.id);
                        }
                        load_grid();
                    }
                    else
                    {
                        load_grid();
                        MessageBox.Show("OOPs! Theres is some problem");

                    }
                }
            }
        }

        // ---------     Check Insertion           --------------------------------------

        public bool check_insertion(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select insertion from sms_subject where id = " + id;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            insertion = Convert.ToString(reader["insertion"].ToString());
                            if (insertion == "true")
                            {
                                return true;

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }


        public int delete()
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "Delete from sms_exams_subjects where id=" + obj.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return i;
        }

        // ------------       Refresh        --------------------------------

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
        }

        //-----------       Get All Subjects Data    ----------------------

        public void get_all_subjects()
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {

                    cmd.CommandText = "SELECT* FROM sms_exams_subjects";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            subjects subject = new subjects()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                subject_name = Convert.ToString(reader["subject_name"].ToString()),
                                subject_abb = Convert.ToString(reader["subject_abb"].ToString()),
                                subject_code = Convert.ToString(reader["subject_code"].ToString()),
                                created_date_time = Convert.ToDateTime(reader["created_date_time"].ToString()),
                                updated_date_time = Convert.ToDateTime(reader["updated_date_time"].ToString()),
                                remarks = Convert.ToString(reader["remarks"].ToString()),
                            };
                            subjects_list.Add(subject);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }
        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            subjects_grid.ItemsSource = subjects_list.Where(x => x.subject_name.ToUpper().StartsWith(v_search.ToUpper()));
            subjects_grid.Items.Refresh();

        }
    }
}
