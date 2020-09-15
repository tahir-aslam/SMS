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
using SMS.AdmissionManagement.Admission;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.AdmissionManagement;
using System.ComponentModel;


namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for AdmissionSearch.xaml
    /// </summary>
    public partial class AdmissionSearch : Page
    {
        List<admission> adm_grid_list;
        public List<admission> adm_list;
        AdmissionForm AF;
        admission obj;
        string mode;
        string insertion;
        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_months> months_list;
        public static int strength = 0;
        public static string class_name = "";
        public static string section_name = "";

        public AdmissionSearch()
        {
            InitializeComponent();
            adm_list = new List<admission>();
            obj = new admission();
            //SearchTextBox.Focus();
            classes_list = new List<classes>();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "-Select Month-", id = "-1" });
            month_cmb.ItemsSource = months_list;

            load_grid();
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

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sms_months sm = (sms_months)month_cmb.SelectedItem;
            if (month_cmb.SelectedIndex != 0)
            {
                adm_grid.ItemsSource = adm_list.Where(x=>x.adm_date.Month == Convert.ToInt32(sm.month_id)).Where(y=>y.adm_date >= MainWindow.session.session_start);
                adm_grid.Items.Refresh();
            }
            else
            {
                
            }

        }

        public void load_grid()
        {
            adm_list.Clear();
            get_all_admissions();
            adm_grid.ItemsSource = adm_list;
            class_cmb.SelectedIndex = 0;
            section_cmb.SelectedIndex = 0;
            strength_textblock.Text = adm_grid.Items.Count.ToString();
            this.adm_grid.Items.Refresh();
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            load_grid();
            class_cmb.SelectedIndex = 0;
            month_cmb.SelectedIndex = 0;
        }

        private void click_new(object sender, RoutedEventArgs e)
        {
            try
            {
                mode = "insert";
                obj = null;
                AF = new AdmissionForm(mode, this, obj);
                AF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                AF.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //-------------     Editing          ---------------------------
        private void click_edit(object sender, RoutedEventArgs e)
        {
            editing();
        }
        private void adm_grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editing();
        }

        public void editing()
        {
            obj = (admission)adm_grid.SelectedItem;
            if (obj == null)
            {
                //MessageBox.Show("plz select a row");
            }
            else
            {
                mode = "edit";
                AF = new AdmissionForm(mode, this, obj);
                AF.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                AF.ShowDialog();
            }
        }

        //-------------     Delete          ---------------------------      
        private void click_delete(object sender, RoutedEventArgs e)
        {
            obj = (admission)adm_grid.SelectedItem;
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
                            insert_deleted(obj.id);
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

        //  ----------------------insert deleted------------------------------
        public void insert_deleted(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into sms_admission_deleted (id) values (@id)";
                        cmd.Connection = con;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = id;


                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        cmd.CommandText = "select insertion from sms_admission where id = " + id +"&& session_id="+MainWindow.session.id;
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



                    cmd.CommandText = "Delete from sms_admission where id=" + obj.id+"&& session_id="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                    catch
                    {
                        MessageBox.Show(" Subject DB not connected");
                    }

                }
            }
            return i;
        }

        // ===============     Get All Admissions          ================
        public void get_all_admissions()
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && session_id="+MainWindow.session.id+" ORDER BY adm_no ASC";
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

                                fees_package_id = Convert.ToInt32(reader["fees_package_id"]),
                                fees_package = Convert.ToString(reader["fees_package"].ToString()),
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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
            strength_textblock.Text = adm_grid.Items.Count.ToString();
        }

        public void search_box()
        {
            string v_search = SearchTextBox.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(adm_grid.ItemsSource);
            if (v_search == null)
            {
                cv.Filter = null;
            }
            else
            {
                cv.Filter = o =>
                {
                    admission x = o as admission;
                    if (search_cmb.SelectedIndex == 0)
                    {
                        return (x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 1)
                    {
                        return (x.father_name.ToUpper().StartsWith(v_search.ToUpper()) || x.father_name.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 2)
                    {
                        return (x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 3)
                    {
                        return (x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
                    }
                    else if (search_cmb.SelectedIndex == 4)
                    {
                        return (x.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || x.cell_no.ToUpper().Contains(v_search.ToUpper()));
                    }
                    else
                    {
                        return true;
                    }

                };
            }
            SearchTextBox.Focus();

            //if (search_cmb.SelectedIndex == 0)
            //{
            //    string v_search = SearchTextBox.Text;
            //    adm_grid.ItemsSource = adm_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()) || x.std_name.ToUpper().Contains(v_search.ToUpper()));
            //    adm_grid.Items.Refresh();
            //}
            //else if (search_cmb.SelectedIndex == 1)
            //{
            //    string v_search = SearchTextBox.Text;
            //    adm_grid.ItemsSource = adm_list.Where(x => x.father_name.ToUpper().StartsWith(v_search.ToUpper()) || x.father_name.ToUpper().Contains(v_search.ToUpper()));
            //    adm_grid.Items.Refresh();
            //}
            //else if (search_cmb.SelectedIndex == 2)
            //{
            //    string v_search = SearchTextBox.Text;
            //    adm_grid.ItemsSource = adm_list.Where(x => x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));
            //    adm_grid.Items.Refresh();
            //}
            //else if (search_cmb.SelectedIndex == 3)
            //{
            //    string v_search = SearchTextBox.Text;
            //    adm_grid.ItemsSource = adm_list.Where(x => x.roll_no.Equals(v_search.ToUpper()) || x.roll_no.Equals(v_search.ToUpper()));
            //    adm_grid.Items.Refresh();
            //}
            //else if (search_cmb.SelectedIndex == 4)
            //{
            //    string v_search = SearchTextBox.Text;
            //    adm_grid.ItemsSource = adm_list.Where(x => x.cell_no.ToUpper().StartsWith(v_search.ToUpper()) || x.cell_no.ToUpper().Contains(v_search.ToUpper()));
            //    adm_grid.Items.Refresh();
            //}
            //else 
            //{
            //}
            //SearchTextBox.Focus();
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
                class_name = c.class_name;
                adm_grid.ItemsSource = adm_list.Where(x => x.class_id == id);
                section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
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
                    section_name = sec.section_name;
                }
                else
                {
                    adm_grid.ItemsSource = null;
                }
            }
        }

        // ================        Printing         ========================
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            //AdmissionReportWF report = new AdmissionReportWF(adm_list);
            //report.ShowDialog();

            //if (class_cmb.SelectedIndex > 0)
            //{
            //    classes c = (classes)class_cmb.SelectedItem;
            //    class_name = c.class_name;
            //}
            //else
            //{
            //    class_name = "";
            //}

            //if (section_cmb.SelectedIndex > 0)
            //{
            //    sections sec = (sections)section_cmb.SelectedItem;
            //    section_name = sec.section_name;
            //}
            //else
            //{
            //    section_name = "";
            //}

            //get_grid_records();
            //var dataTable = CreateSampleDataTable();
            //var columnWidths = new List<double>() { 50, 140, 130, 60, 60, 60, 60, 90, 90, 90, 140 };
            //var ht = new SMS.PrintHeaderTemplates.AdmissionRegisterHeader();
            //var headerTemplate = XamlWriter.Save(ht);
            //var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            //printControl.ShowPrintPreview();

            sms_report report_data = new sms_report();
            report_data.total_strength = adm_grid.Items.OfType<admission>().Select(x => x.id).Distinct().Count();
            AdmissionRegisterReportWindow window = new AdmissionRegisterReportWindow(adm_grid.Items.OfType<admission>().ToList(),report_data);
            window.Show();
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
            AddColumn(dataTable, "Contact#", typeof(string));            
            AddColumn(dataTable, "DOB", typeof(string));
            AddColumn(dataTable, "Adm Date", typeof(string));
            AddColumn(dataTable, "Address", typeof(string));
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
                dataRow[7] = c.cell_no.ToString();                
                dataRow[8] = c.dob.ToString("dd-MMM-yyyy");
                dataRow[9] = c.adm_date.ToString("dd-MMM-yyyy");
                dataRow[10] = c.parmanent_adress;                

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
            strength = adm_grid_list.Count;
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchTextBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            adm_grid.ItemsSource = adm_list.Where(x=>x.adm_date >= MainWindow.session.session_start);
            adm_grid.Items.Refresh();
            month_cmb.SelectedIndex = 0;
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

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (SearchTextBox.IsFocused == true)
                {
                    adm_grid.SelectedIndex = 0;
                    editing();
                }
            }
        }
        
    }
}
