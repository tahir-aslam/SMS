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
using MySql.Data.MySqlClient;
using SMS.DAL;

namespace SMS.Common
{
    /// <summary>
    /// Interaction logic for StudentSelectionWindowNew.xaml
    /// </summary>
    /// 
    public interface IGEtStudentdelegate
    {
        void getSelectedStudents(List<admission> selectedStudentsLst);
    }

    public partial class StudentSelectionWindowNew : Window
    {
        public List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;
        public bool check = false;
        public IGEtStudentdelegate idelegate;
        List<sms_months> months_list;
        DateTime dt;
        FeesDAL feesDAL;

        public StudentSelectionWindowNew()
        {
            InitializeComponent();
            SearchTextBox.Focus();
            classes_list = new List<classes>();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---All Classes---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---All Months---", id = "-1" });
            month_cmb.ItemsSource = months_list;

            load_grid();
            feesDAL = new FeesDAL();
        }

        public void load_grid()
        {
            get_all_admissions();
            adm_grid.ItemsSource = adm_list;
            this.adm_grid.Items.Refresh();
            class_cmb.SelectedIndex = 0;
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            admission adm_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < adm_grid.Items.Count; i++)
            {
                adm_obj = (admission)adm_grid.Items[i];
                adm_obj.Checked = checkBox.IsChecked.Value;
            }
            adm_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            adm_grid.SelectedItem = e.Source;
            admission adm = new admission();
            adm = (admission)adm_grid.SelectedItem;
            foreach (admission ede in adm_list)
            {
                if (adm.id == ede.id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
                }
            }
        }
        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }

        // ===============     Get All Admissions          ================
        public void get_all_admissions()
        {
            adm_list = new List<admission>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && session_id=" + MainWindow.session.id;
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                        };
                        adm_list.Add(adm);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            if (search_cmb.SelectedIndex == 0)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 1)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.father_name.ToUpper().StartsWith(v_search.ToUpper()) || x.father_name.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 2)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 3)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 4)
            {
                string v_search = SearchTextBox.Text;
                adm_grid.ItemsSource = adm_list.Where(x => x.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || x.cell_no.ToUpper().Contains(v_search.ToUpper()));
                adm_grid.Items.Refresh();
            }
            else
            {
            }
            SearchTextBox.Focus();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //============      Classes Selection Change       ===============================
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {


                get_all_sections(id);

                adm_grid.ItemsSource = adm_list.Where(x => x.class_id == id);
                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
            }
            else
            {

                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
                adm_grid.ItemsSource = adm_list;
            }
        }

        //------------         Get All Sections   ------------------------
        public void get_all_sections(string id)
        {
            sections_list = new List<sections>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects where class_id = " + id;
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
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
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

        //=======          Sections Selection Changed        ======================================================
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sections sec = (sections)section_cmb.SelectedItem;
            if (sec != null)
            {
                string id = sec.id;
                if (section_cmb.SelectedIndex != 0)
                {
                    adm_grid.ItemsSource = adm_list.Where(x => x.section_id == id);
                }
                else
                {
                    adm_grid.ItemsSource = null;
                }
            }
        }

        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            if (adm_list.Select(y => y.Checked == true).Count() > 0)
            {
                //idelegate.getSelectedStudents(adm_list.Where(x=>x.Checked == true).ToList());
                
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Select Minimum One Student", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (month_cmb.SelectedIndex != 0)
            {
                sms_months sm = (sms_months)month_cmb.SelectedItem;
                List<sms_fees> fees_list = feesDAL.getAllUnPaidFeesByMonth(Convert.ToInt32(sm.month_id));
                    adm_grid.ItemsSource = adm_list.Where(x => fees_list.Any(y => y.std_id.ToString() == x.id));
                
            }
            else 
            {
                adm_grid.ItemsSource = adm_list;
            }
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            if (adm_list.Select(y => y.Checked == true).Count() > 0)
            {                
                
            }
            else
            {
                MessageBox.Show("You Have Not Selected Even A Single Student, Do you want to continue?", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
            }
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selected_date = (DateTime)date_picker.SelectedDate;
            List<sms_fees> paid_fees_list = feesDAL.getFeesPaidByDate(selected_date.Date,selected_date.Date);
            adm_grid.ItemsSource = adm_list.Where(x => paid_fees_list.Any(y => y.std_id.ToString() == x.id));

        }
    }
}
