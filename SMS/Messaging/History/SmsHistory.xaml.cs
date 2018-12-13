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
using SMS.Models;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using SMS.DAL;

namespace SMS.Messaging.History
{
    /// <summary>
    /// Interaction logic for SmsHistory.xaml
    /// </summary>
    public partial class SmsHistory : Page
    {

        DateTime dt;
        List<sms_history> history_lst;
        List<classes> classes_list;
        List<sections> sections_list;
        ClassesDAL classesDAL;

        public SmsHistory()
        {
            InitializeComponent();
            classesDAL = new ClassesDAL();
            classes_list = classesDAL.get_all_classes();
            classes_list.Insert(0, new classes() { class_name = "---All Classes---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            date_picker_to.SelectedDate = DateTime.Now;
            date_picker_from.SelectedDate = DateTime.Now;

            class_cmb.SelectedIndex = 0;
        }

        //private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (date_picker.SelectedDate != null)
        //    {
        //        grid.Visibility = Visibility.Visible;
        //        img_grid.Visibility = Visibility.Hidden;
        //        //dt = date_picker.SelectedDate.Value;
        //        get_sms_history();
        //        sms_history_grid.ItemsSource = history_lst;
        //    }
        //    else
        //    {
        //        sms_history_grid.ItemsSource = null;
        //        sms_history_grid.Items.Refresh();
        //        grid.Visibility = Visibility.Hidden;
        //        img_grid.Visibility = Visibility.Visible;
        //    }
        //}

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {

                    dt = date_picker_from.SelectedDate.Value;
                    grid.Visibility = Visibility.Visible;
                    img_grid.Visibility = Visibility.Hidden;
                    get_sms_history(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    sms_history_grid.ItemsSource = history_lst;
                    SearchTextBox.Text = "";
                    calculate_message();
                }                
            }
            else
            {
                sms_history_grid.ItemsSource = null;
                sms_history_grid.Items.Refresh();
                grid.Visibility = Visibility.Hidden;
                img_grid.Visibility = Visibility.Visible;
            }
        }
        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker_from.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker_from.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker_from.SelectedDate;

                if (date_picker_to.SelectedDate <= date_picker_from.SelectedDate)
                {
                    dt = date_picker_from.SelectedDate.Value;
                    grid.Visibility = Visibility.Visible;
                    img_grid.Visibility = Visibility.Hidden;
                    get_sms_history(date_picker_to.SelectedDate.Value, date_picker_from.SelectedDate.Value);
                    sms_history_grid.ItemsSource = history_lst;
                    SearchTextBox.Text = "";
                    calculate_message();
                }
            }
            else
            {
                sms_history_grid.ItemsSource = null;
                sms_history_grid.Items.Refresh();
                grid.Visibility = Visibility.Hidden;
                img_grid.Visibility = Visibility.Visible;
            }
        }

        void filter()
        {
            SearchTextBox.Text = "";
            ICollectionView cv = CollectionViewSource.GetDefaultView(sms_history_grid.ItemsSource);
            cv.Filter = o =>
            {
                sms_fees f = o as sms_fees;
                if (getClasses(f) && getSections(f))
                {
                    return true;
                }
                return false;
            };
            calculate_message();
        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (class_cmb.SelectedItem != null)
            {
                classes cl = (classes)class_cmb.SelectedItem;

                if (class_cmb.SelectedIndex != 0)
                {

                    filter();

                    section_cmb.IsEnabled = true;
                    sections_list = new List<sections>();
                    sections_list = classesDAL.get_all_sections(cl.id);
                    sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;
                }
                else
                {
                    filter();
                }
            }
        }
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedItem != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    filter();
                }
                else
                {
                    filter();
                }
            }
        }

        bool getClasses(sms_fees f)
        {
            classes cl = (classes)class_cmb.SelectedItem;
            if (class_cmb.SelectedIndex > 0 && f.class_id.ToString() != cl.id)
            {
                return false;
            }
            return true;
        }
        bool getSections(sms_fees f)
        {
            sections sec = (sections)section_cmb.SelectedItem;
            if (section_cmb.SelectedIndex > 0 && f.section_id.ToString() != sec.id)
            {
                return false;
            }
            return true;
        }
        void clearAllFilters()
        {            
            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }

        public void search_box()
        {
            try
            {
                string v_search = SearchTextBox.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(sms_history_grid.ItemsSource);
                if (v_search == null)
                {
                    cv.Filter = null;
                }
                else
                {
                    cv.Filter = o =>
                    {
                        sms_history f = o as sms_history;
                        if (search_cmb.SelectedIndex == 0)
                        {
                            return (f.sender_name.ToUpper().Contains(v_search.ToUpper()));
                        }                       
                        else if (search_cmb.SelectedIndex == 1)
                        {
                            return (f.cell.ToUpper().Contains(v_search.ToUpper()));
                        }                        
                        else
                        {
                            return true;
                        }
                    };
                    calculate_message();
                    clearAllFilters();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
                SearchTextBox.Focus();
            
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void calculate_message()
        {

            int message_count = 0;
            int specimen_count = 0;
            sms_history sh;

            for (int i = 0; i < sms_history_grid.Items.Count; i++)
            {
                sh = (sms_history)sms_history_grid.Items[i];
                specimen_count++;
                if(sh.msg.Count() <= 160)
                {
                    message_count++;
                }
                else if(sh.msg.Count() <= 306)
                {
                    message_count = message_count + 2;
                }
                else if(sh.msg.Count() <= 459)
                {
                    message_count = message_count + 3;
                }
            }

            specimen_count_tb.Text = specimen_count.ToString();
            sms_count_tb.Text = message_count.ToString();
        }

        //-------------- Get SMS History --------------------------------------

        public void get_sms_history(DateTime sDate, DateTime eDate) 
        {
            history_lst = new List<sms_history>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_history where DATE(date_time) >= @sDate && DATE(date_time) <= @eDate ORDER BY date_time DESC";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_history sh = new sms_history()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                sender_id = Convert.ToString(reader["sender_id"].ToString()),
                                sender_name = Convert.ToString(reader["sender_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                section_id = Convert.ToString(reader["section_id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                cell = Convert.ToString(reader["cell"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                msg = Convert.ToString(reader["msg"].ToString()),
                                sms_type = Convert.ToString(reader["sms_type"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            //if (sh.date_time.ToString("yyyy-MM-dd") == dt.ToString("yyyy-MM-dd"))
                            //{
                                history_lst.Add(sh);
                            //}


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            SMSHistoryPrint window = new SMSHistoryPrint(sms_history_grid.Items.OfType<sms_history>().ToList());
            window.Show();
            
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }

        


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(sms_history_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
           // strength_textblock.Text = sms_history_grid.Items.Count.ToString();
        }

       
    }
}
