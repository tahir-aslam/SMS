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
using SMS.EmployeeManagement.ADDEMP;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.Messaging;

namespace SMS.Messaging.EmpSms
{
    /// <summary>
    /// Interaction logic for EmpSmsWindow.xaml
    /// </summary>
    public partial class EmpSmsWindow : Window
    {
        public static List<employees> emp_list;
        GeneralSms gs;

        public EmpSmsWindow(GeneralSms GS)
        {
            InitializeComponent();
            this.gs = GS;
            get_all_employees();
            friends_grid.ItemsSource = emp_list;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            employees emp_obj;
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            for (int i = 0; i < friends_grid.Items.Count; i++)
            {
                emp_obj = (employees)friends_grid.Items[i];
                emp_obj.Checked = checkBox.IsChecked.Value;
            }
            friends_grid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            friends_grid.SelectedItem = e.Source;
            employees fl = new employees();
            fl = (employees)friends_grid.SelectedItem;
            foreach (employees ede in emp_list)
            {
                if (fl.id == ede.id)
                {
                    ede.Checked = checkBox.IsChecked.Value;
                }
            }

        }

        // -------------      Get All Employees       --------------------
        public void get_all_employees()
        {
            emp_list = new List<employees>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp where is_active='Y'";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_father = Convert.ToString(reader["emp_father"].ToString()),                                
                                emp_cell = Convert.ToString(reader["emp_cell"].ToString()),                                
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                            };
                            emp_list.Add(emp);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("oops! Employees DB not connected");
                    }

                }
            }
        }
        private void click_save(object sender, RoutedEventArgs e)
        {
            this.Close();
            int count = 0;
            foreach (employees emp in emp_list.Where(x => x.Checked == true))
            {
                count++;
            }
            gs.emp_count.Text = count.ToString();
        }
    }
}
