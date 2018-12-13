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
using SMS.AdmissionManagement.WithdrawAdmission;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using System.ComponentModel;


namespace SMS.AdmissionManagement.WithdrawAdmission
{
    /// <summary>
    /// Interaction logic for WithdrawAdmissionPage.xaml
    /// </summary>
    public partial class WithdrawAdmissionPage : Page
    {
        List<admission> adm_list;
        WithdrawAdmissionPage WAP;
        admission obj;
        List<session> session_list;
        public static session sess;
        List<admission> adm_grid_list;

        public WithdrawAdmissionPage()
        {
            InitializeComponent();
            load_grid();
            get_all_sessions();
            session_cmb.ItemsSource = session_list;
            session_cmb.SelectedIndex = Convert.ToInt32(MainWindow.session.id)-1;
            strength_textblock.Text = adm_grid.Items.Count.ToString();
        }
        public void load_grid()
        {            
        }
        public void get_all_sessions()
        {
            session_list = new List<session>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_sessions";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            session ses = new session()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                session_name = Convert.ToString(reader["session_name"].ToString()),
                                session_start = Convert.ToDateTime(reader["session_start"]),
                            };
                            session_list.Add(ses);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void click_edit(object sender, RoutedEventArgs e)
        {
            obj = (admission)adm_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {

                MessageBoxResult mbr = MessageBox.Show("Are You Want To Restore This Student ?", " Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    update_restore();                    
                    MessageBox.Show("Admission Restore Successfully");
                    load_grid();
                }
            }
        }

        public void update_restore()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_admission SET is_active=@is_active WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = 'Y';


                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void click_refresh(object sender, RoutedEventArgs e)
        {

        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            obj = (admission)adm_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                SLCWindow slc = new SLCWindow(obj);
                slc.ShowDialog();
                
            }
        }
        // ===============     Get All Admissions          ================

        public void get_all_admissions(string id)
        {
            adm_list = new List<admission>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='N' && session_id=" + id;
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["image"] == "")
                        {
                            string path = "/SMS;component/images/Delete-icon.png";
                            img = File.ReadAllBytes(path);
                        }
                        else
                        {
                            img = (byte[])(reader["image"]);
                        }

                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            father_cnic = Convert.ToString(reader["father_cnic"].ToString()),
                            father_income = Convert.ToString(reader["father_income"].ToString()),
                            religion = Convert.ToString(reader["religion"].ToString()),
                            dob = Convert.ToDateTime(reader["dob"]),
                            b_form = Convert.ToString(reader["b_form"].ToString()),
                            parmanent_adress = Convert.ToString(reader["parmanent_adress"].ToString()),
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            emergency_address = Convert.ToString(reader["emergency_address"].ToString()),
                            previous_school = Convert.ToString(reader["previous_school"].ToString()),
                            boarding = Convert.ToString(reader["boarding"].ToString()),
                            transport = Convert.ToString(reader["transport"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            transport_fee = Convert.ToString(reader["transport_fee"].ToString()),
                            reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                            tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                            exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                            security_fee = Convert.ToString(reader["security_fee"].ToString()),
                            stationary_fee = Convert.ToString(reader["stationary_fee"].ToString()),
                            scholarship = Convert.ToString(reader["scholarship"].ToString()),
                            misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                            other_exp = Convert.ToString(reader["other_exp"].ToString()),
                            adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                            total = Convert.ToString(reader["total"].ToString()),
                            date_time = Convert.ToDateTime(reader["date_time"]),
                            created_by = Convert.ToString(reader["created_by"].ToString()),
                            is_active = Convert.ToString(reader["is_active"].ToString()),
                            remarks = Convert.ToString(reader["remarks"].ToString()),
                            withdrawal_date = Convert.ToDateTime(reader["withdrawal_date"]),

                            image = img,
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

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        private void session_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sess = (session)session_cmb.SelectedItem;  
            get_all_admissions(sess.id);
            adm_grid.ItemsSource = adm_list;
        }

        private void print_btn_list_Click(object sender, RoutedEventArgs e)
        {
            get_grid_records();
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 30, 80, 60, 110, 90, 110, 80, 100, 70, 90, 90, 60 };
            var ht = new PrintHeaderTemplates.WithdrawHeader();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }

        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Sr#", typeof(string));
            AddColumn(dataTable, "Adm Date", typeof(string));
            AddColumn(dataTable, "Adm #", typeof(string));
            AddColumn(dataTable, "Student Name", typeof(string));
            AddColumn(dataTable, "DOB", typeof(string));
            AddColumn(dataTable, "Father Name", typeof(string));
            AddColumn(dataTable, "Contact#", typeof(string));
            AddColumn(dataTable, "Residence", typeof(string));
            AddColumn(dataTable, "Adm Class", typeof(string));
            AddColumn(dataTable, "With.Class", typeof(string));
            AddColumn(dataTable, "Withdrawal Date", typeof(string));
            AddColumn(dataTable, "Remarks", typeof(string));            
            
            int i = 0;
            foreach (admission c in adm_grid_list)
            {
                i++;
                var dataRow = dataTable.NewRow();

                dataRow[0] = i.ToString();
                dataRow[1] = c.adm_date.ToString("dd-MMM-yyyy");
                dataRow[2] = c.adm_no.ToString();
                dataRow[3] = c.std_name.ToString();
                dataRow[4] = c.dob.ToString("dd-MMM-yyyy");
                dataRow[5] = c.father_name.ToString();
                dataRow[6] = c.cell_no.ToString();
                dataRow[7] = c.parmanent_adress;
                dataRow[8] = c.class_name.ToString();
                dataRow[9] = c.class_name.ToString();
                dataRow[10] = c.withdrawal_date.ToString("dd-MMM-yyyy");
                dataRow[11] = c.remarks.ToString();

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }
        public void get_grid_records()
        {
            adm_grid_list = new List<admission>();
            for (int row = 0; row < adm_grid.Items.Count; row++)
            {
                admission adm = new admission();
                adm = (admission)(adm_grid.Items[row]);
                adm_grid_list.Add(adm);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(adm_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = adm_grid.Items.Count.ToString();
        }

    }
}
