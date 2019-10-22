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
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using SMS.DAL;

namespace SMS.FeesManagement.FeesCollection
{
    /// <summary>
    /// Interaction logic for FeesCollectionPage.xaml
    /// </summary>
    public partial class FeesCollectionPage : Page
    {

        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;        
        admission obj;
        AdmissionDAL admDAL;
        List<fee> fee_list;
        List<admission> adm_grid_list;
        FeesDAL feesDAL;

        public FeesCollectionPage()
        {
            InitializeComponent();

            adm_list = new List<admission>();
            feesDAL = new FeesDAL();
            admDAL = new AdmissionDAL();

            classes_list = new List<classes>();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            SearchTextBox.Focus();
            load_grid();
            strength_textblock.Text = fee_grid.Items.Count.ToString();
           
        }

        public void load_grid()
        {
            section_cmb.IsEnabled = false;
            class_cmb.SelectedIndex = 0;
            adm_list.Clear();
            adm_list = admDAL.get_all_admissions();
            List<sms_fees_actual> fees_list = feesDAL.get_all_actual_fees();
            foreach (var adm in adm_list)
            {
                foreach (var fee in fees_list.Where(x=>x.std_id.ToString() == adm.id))
                {
                    if(fee.fees_category_id == 111)
                    {
                        adm.adm_fee = fee.amount.ToString();
                    }
                    if (fee.fees_category_id == 112)
                    {
                        adm.reg_fee = fee.amount.ToString();
                    }
                    if (fee.fees_category_id == 113)
                    {
                        adm.tution_fee = fee.amount.ToString();
                    }                    
                }
            }
            fee_grid.ItemsSource = adm_list;
            this.fee_grid.Items.Refresh();
        }
        private void click_new(object sender, RoutedEventArgs e)
        {

        }

        //-------------     Editing          ---------------------------
        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void fee_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            obj = (admission)fee_grid.SelectedItem;
            if (obj == null)
            {
                // MessageBox.Show("plz select a row");
            }
            else
            {
                FeesCollectionForm fcf = new FeesCollectionForm(obj);
                fcf.ShowDialog();
            }
        }


        private void click_delete(object sender, RoutedEventArgs e)
        {

        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            //load_grid();
            int i = 1;
            //load_grid();

            foreach (admission adm in adm_list)
            {
                get_fee_data(adm.id);
                if (fee_list.Count > 12)
                {
                    i = 1;
                    foreach (fee f in fee_list)
                    {
                        if (i > 12)
                        {
                            delete_extra_fee(f.id);
                        }
                        i++;
                    }
                }
            }
        }
        //-------------     Delete          ---------------------------

        public int delete_extra_fee(string id)
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_fee where id=" + id + "&& session_id=" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            return i;
        }

        public void get_fee_data(string id)
        {
            fee_list = new List<fee>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where std_id = " + id + "&& session_id=" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        // cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = months;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee f = new fee()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                rem_reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString()),
                                rem_adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString()),
                                rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                rem_other_fee = Convert.ToString(reader["rem_other_exp"].ToString()),
                                rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),
                                rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                            };
                            fee_list.Add(f);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //============     Get All Admission data                 ===========================================
        public void get_all_admissions()
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && session_id=" + MainWindow.session.id + " ORDER BY adm_no ASC";
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


                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
            }
            else
            {

                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;

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
                    fee_grid.ItemsSource = adm_list.Where(x => x.section_id == id);
                }
                else
                {
                    fee_grid.ItemsSource = null;
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
                fee_grid.ItemsSource = adm_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 1)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.father_name.ToUpper().StartsWith(v_search.ToUpper()) || x.father_name.ToUpper().Contains(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 2)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 3)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 4)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || x.cell_no.ToUpper().Contains(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 5)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.adm_fee.ToUpper().StartsWith(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 6)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.reg_fee.ToUpper().StartsWith(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 7)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.tution_fee.ToUpper().StartsWith(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 8)
            {
                string v_search = SearchTextBox.Text;
                fee_grid.ItemsSource = adm_list.Where(x => x.father_cnic.ToUpper().StartsWith(v_search.ToUpper()) || x.father_cnic.ToUpper().Contains(v_search.ToUpper()));
                fee_grid.Items.Refresh();
            }
            else
            {
            }
            SearchTextBox.Focus();

        }

        // ================        Printing         ========================
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {

            get_grid_records();
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 50, 120, 120, 60, 60, 70, 70, 70, 70, 70, 70, 70, 70 };
            var ht = new HeaderTemplate();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }
        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Sr#", typeof(string));
            AddColumn(dataTable, "Student Name", typeof(string));
            AddColumn(dataTable, "Father Name", typeof(string));
            AddColumn(dataTable, "Adm #", typeof(string));
            AddColumn(dataTable, "Roll#", typeof(string));
            AddColumn(dataTable, "Class", typeof(string));
            AddColumn(dataTable, "Section", typeof(string));
            AddColumn(dataTable, "Annual Fund", typeof(string));
            AddColumn(dataTable, "Adm Fee", typeof(string));
            AddColumn(dataTable, "Tution Fee", typeof(string));
            AddColumn(dataTable, "Other", typeof(string));
            AddColumn(dataTable, "Security", typeof(string));
            AddColumn(dataTable, "Exam Fee", typeof(string));


            int i = 0;
            foreach (admission c in adm_grid_list)
            {

                i++;
                var dataRow = dataTable.NewRow();

                dataRow[0] = i.ToString();
                dataRow[1] = c.std_name.ToString();
                dataRow[2] = c.father_name.ToString();
                dataRow[3] = c.adm_no.ToString();
                dataRow[4] = c.roll_no.ToString();
                dataRow[5] = c.class_name.ToString();
                dataRow[6] = c.section_name.ToString();
                dataRow[7] = c.reg_fee.ToString();
                dataRow[8] = c.adm_fee.ToString();
                dataRow[9] = c.tution_fee.ToString();
                dataRow[10] = c.other_exp.ToString();
                dataRow[11] = c.security_fee.ToString();
                dataRow[12] = c.exam_fee.ToString();

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }
        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }

        public void get_grid_records()
        {
            adm_grid_list = new List<admission>();
            for (int row = 0; row < fee_grid.Items.Count; row++)
            {

                admission adm = new admission();
                adm = (admission)(fee_grid.Items[row]);

                adm_grid_list.Add(adm);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(fee_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = fee_grid.Items.Count.ToString();
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchTextBox.IsFocused == true)
                {
                    fee_grid.SelectedIndex = 0;
                    editing();
                }
            }
        }
    }
}
