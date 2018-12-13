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
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.PrintHeaderTemplates;
using System.ComponentModel;

namespace SMS.FeeManagement.FeeDefaulters
{
    /// <summary>
    /// Interaction logic for FeeDefaultersList.xaml
    /// </summary>
    public partial class FeeDefaultersList : Page
    {
        public static string months;
        List<sms_months> sms_months_list;
        List<fee> fee_list;
        List<fee> total_def_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        List<fee> filter_lst;
        fee f;
        List<double> col_width;

        string class_id = "0";

        public FeeDefaultersList()
        {
            InitializeComponent();

            get_all_months();
            section_cmb.IsEnabled = false;
            class_cmb.IsEnabled = false;
            fill_month_cmb();
            month_cmb.SelectedIndex = 0;
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            get_all_admissions();
            filter_def_cmb.SelectedIndex = 0;
            strength_textblock.Text = defaulter_fee_grid.Items.Count.ToString();
        }

        // =======      Fill month cmb        =============================
        public void fill_month_cmb()
        {
            month_cmb.Items.Add("---Select Month---");
            month_cmb.Items.Add("April");
            month_cmb.Items.Add("May");
            month_cmb.Items.Add("June");
            month_cmb.Items.Add("July");
            month_cmb.Items.Add("August");
            month_cmb.Items.Add("September");
            month_cmb.Items.Add("October");
            month_cmb.Items.Add("November");
            month_cmb.Items.Add("December");
            month_cmb.Items.Add("January");
            month_cmb.Items.Add("February");
            month_cmb.Items.Add("March");

            filter_def_cmb.Items.Add("--Select Filter--");
            filter_def_cmb.Items.Add("Annual Fund");
            filter_def_cmb.Items.Add("Tution Fee");
            filter_def_cmb.Items.Add("Other Fee");
            filter_def_cmb.Items.Add("Admission Fee");
            filter_def_cmb.Items.Add("Exam Fee");
            filter_def_cmb.Items.Add("Security Fee");
            filter_def_cmb.Items.Add("Fine Fee");

        }

        //---------------           Get All Months    ----------------------------------
        public void get_all_months()
        {
            sms_months_list = new List<sms_months>();
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
                            sms_months_list.Add(sm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //---------------           Get All Classes    ----------------------------------
        public void get_all_classes()
        {
            classes_list = new List<classes>();
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
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
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

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter_def_cmb.SelectedIndex = 0;
            if (month_cmb.SelectedIndex != 0)
            {
                filter_def_cmb.IsEnabled = true;
                class_cmb.IsEnabled = true;
                class_cmb.SelectedIndex = 0;                
                months = month_cmb.SelectedItem.ToString();
                get_fee_data();
                set_fee_data();
                defaulter_fee_grid.ItemsSource = total_def_list.OrderBy(x=>x.class_id);
                calculate_amount();
            }
            else
            {
                class_cmb.IsEnabled = false;
                section_cmb.IsEnabled = false;
                class_cmb.SelectedIndex = 0;
                defaulter_fee_grid.ItemsSource = null;
                filter_def_cmb.IsEnabled = false;
                calculate_amount();
            }

        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            class_id = c.id;
            filter_def_cmb.SelectedIndex = 0;
            if (class_cmb.SelectedIndex != 0)
            {

                section_cmb.IsEnabled = true;

                get_all_sections(c.id);
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                section_cmb.ItemsSource = sections_list;
                section_cmb.SelectedIndex = 0;
                defaulter_fee_grid.ItemsSource = total_def_list.Where(x => x.class_id == c.id);
                calculate_amount();
            }
            else
            {
                defaulter_fee_grid.ItemsSource = null;
                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
                defaulter_fee_grid.ItemsSource = total_def_list;
                calculate_amount();
            }
        }

        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter_def_cmb.SelectedIndex = 0;
            sections s = (sections)section_cmb.SelectedItem;

            if (s != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    defaulter_fee_grid.ItemsSource = total_def_list.Where(x => x.section_id == s.id);
                    calculate_amount();
                }
                else
                {
                    defaulter_fee_grid.ItemsSource = total_def_list.Where(x => x.class_id == class_id);
                    calculate_amount();
                }
            }


        }

         //========      Get fee data          ====================================
        public void get_fee_data()
        {            
            fee_list = new List<fee>();
            if(month_cmb.SelectedIndex > 0)
            {
                for (int i = 1; i <= month_cmb.SelectedIndex; i++ ) 
                {                    
                    months = month_cmb.Items[i].ToString();
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "SELECT* FROM sms_fee where month = @month && isActive='Y' && session_id=" + MainWindow.session.id;
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                            try
                            {
                                cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = months;
                                con.Open();
                                MySqlDataReader reader = cmd.ExecuteReader();
                                while (reader.Read())
                                {         
                                    fee f = new fee()
                                    {
                                        transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                                        reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString()),
                                        tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                        exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),
                                        security_fee = Convert.ToString(reader["rem_security_fee"].ToString()),
                                        other_expenses = Convert.ToString(reader["rem_other_exp"].ToString()),
                                        adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString()),
                                        fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString()),
                                        month = Convert.ToString(reader["month"].ToString()),
                                        std_id = Convert.ToString(reader["std_id"].ToString()),
                                    };
                                    
                                    if (Convert.ToInt32(f.transport_fee) > 0 )
                                    {
                                        fee_list.Add(f);
                                    }
                                    else if (Convert.ToInt32(f.reg_fee) > 0)
                                    {
                                        fee_list.Add(f);
                                    }
                                    else if (Convert.ToInt32(f.tution_fee) > 0)
                                    {
                                        fee_list.Add(f);
                                    }
                                    else if (Convert.ToInt32(f.exam_fee) > 0)
                                    {
                                        fee_list.Add(f);
                                    }
                                    else if (Convert.ToInt32(f.security_fee) > 0)
                                    {
                                        fee_list.Add(f);
                                    }
                                    else if (Convert.ToInt32(f.other_expenses) > 0)
                                    {
                                        fee_list.Add(f);
                                    }
                                    else if (Convert.ToInt32(f.fine_fee) > 0)
                                    {
                                        fee_list.Add(f);
                                    }
                                    else
                                    {
                                    }                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
            }            
        }

        public void set_fee_data()
        {
            total_def_list = new List<fee>();

            int regFee = 0;
            int admFee = 0;
            int secFee = 0;
            int examFee = 0;
            int tutionFee = 0;
            int otherFee = 0;
            int fineFee = 0;
            int count = 0;

            fee fee_defaulter;
            List<string> months_list;

            foreach (var id in fee_list.Select(x => x.std_id).Distinct())
            {
                count = 0;
                regFee = 0;
                admFee = 0;
                secFee = 0;
                examFee = 0;
                tutionFee = 0;
                otherFee = 0;
                fineFee = 0;
                months_list = new List<string>();
                fee_defaulter = new fee();

                int totalEntries = fee_list.Where(x => x.std_id == id).Count();
                foreach (fee fee in fee_list.Where(x => x.std_id == id))
                {
                    count++;
                    if (count == totalEntries)
                    {
                        regFee = regFee + Convert.ToInt32(fee.reg_fee);
                        admFee = admFee + Convert.ToInt32(fee.adm_fee);
                        secFee = secFee + Convert.ToInt32(fee.security_fee);
                        examFee = examFee + Convert.ToInt32(fee.exam_fee);
                        tutionFee = tutionFee + Convert.ToInt32(fee.tution_fee);
                        otherFee = otherFee + Convert.ToInt32(fee.other_expenses);
                        fineFee = fineFee + Convert.ToInt32(fee.fine_fee);                        
                        months_list.Add(fee.month);
                    }
                    else 
                    {
                        int total = Convert.ToInt32(fee.tution_fee) + Convert.ToInt32(fee.other_expenses) + Convert.ToInt32(fee.fine_fee);
                        if(total > 0)
                        {
                            tutionFee = tutionFee + Convert.ToInt32(fee.tution_fee);
                            otherFee = otherFee + Convert.ToInt32(fee.other_expenses);
                            fineFee = fineFee + Convert.ToInt32(fee.fine_fee);
                            months_list.Add(fee.month);
                        }                        
                    }
                    
                    fee_defaulter = fee;                    
                }

                fee_defaulter.total_balance = (regFee + admFee + secFee + examFee + tutionFee + otherFee + fineFee).ToString();
                fee_defaulter.reg_fee = regFee.ToString();
                fee_defaulter.adm_fee = admFee.ToString();
                fee_defaulter.security_fee = secFee.ToString();
                fee_defaulter.exam_fee = examFee.ToString();
                fee_defaulter.tution_fee = tutionFee.ToString();
                fee_defaulter.other_expenses = otherFee.ToString();
                fee_defaulter.fine_fee = fineFee.ToString();

                //months
                fee_defaulter.month = "";
                foreach (string month in months_list.Distinct())
                {
                    sms_months sm = sms_months_list.Where(x=>x.month_name == month).First();
                    int month_id = Convert.ToInt32(sm.month_id);
                    DateTime dt = new DateTime(2015, month_id, 1);
                    fee_defaulter.month = fee_defaulter.month + " " + dt.ToString("MMM");
                }

                //
                foreach (admission adm in adm_list.Where(x => x.id == fee_defaulter.std_id))
                {
                    fee_defaulter.father_name = adm.father_name;
                    fee_defaulter.std_name = adm.std_name;
                    fee_defaulter.image = adm.image;
                    fee_defaulter.class_id = adm.class_id;
                    fee_defaulter.class_name = adm.class_name;
                    fee_defaulter.section_id = adm.section_id;
                    fee_defaulter.section_name = adm.section_name;
                    fee_defaulter.adm_no = adm.adm_no;
                    fee_defaulter.std_cell_no = adm.cell_no;
                
                    total_def_list.Add(fee_defaulter);
                }
            }           
        }
            //count=0;
            //foreach (fee f in fee_list.Where(x=>x.std_id == adm.id))
            //{       
            //        count++;
            //        def_obj = new fee();

            //        f.std_name = adm.std_name;
            //        f.image = adm.image;
            //        f.class_id = adm.class_id;
            //        f.class_name = adm.class_name;
            //        f.section_id = adm.section_id;
            //        f.section_name = adm.section_name;
            //        f.adm_no = adm.adm_no;
            //        f.std_cell_no = adm.cell_no;

            //        if(count > 1)
            //        {
            //            f.reg_fee="0";
            //            f.adm_fee="0";
            //            f.exam_fee="0";
            //            f.security_fee="0";
            //        }
            //        def_obj = f;
            //        total_def_list.Add(def_obj);
            //}
            //foreach(fee f in fee_list)
            //{
            //    foreach(admission adm in adm_list)
            //    {
            //        if(f.std_id==adm.id)
            //        {
            //            def_obj = new fee();

            //            f.std_name = adm.std_name;
            //            f.image = adm.image;
            //            f.class_id = adm.class_id;
            //            f.class_name = adm.class_name;
            //            f.section_id=adm.section_id;
            //            f.section_name = adm.section_name;
            //            f.adm_no = adm.adm_no;
            //            f.std_cell_no = adm.cell_no;

            //            def_obj = f;
            //            total_def_list.Add(def_obj);
            //        }
            //    }
            //}
        

        private void filter_def_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = filter_def_cmb.SelectedIndex;
            filter_lst = new List<fee>();

            if (month_cmb.SelectedIndex != 0 && filter_def_cmb.SelectedIndex != 0)
            {   
                if (section_cmb.SelectedIndex > 0)
                {             
                    sections sec = (sections)section_cmb.SelectedItem;
                    foreach (fee f in total_def_list.Where(x => x.section_id == sec.id))
                    {
                        filter_lst.Add(f);
                    }              
                }
                else if (class_cmb.SelectedIndex > 0)
                {
                    classes cl = (classes)class_cmb.SelectedItem;
                    foreach (fee f in total_def_list.Where(x => x.class_id == cl.id))
                    {
                        filter_lst.Add(f);
                    }
                }
                else 
                {
                    filter_lst = total_def_list;
                }
               
                    if (index == 1)
                    {                        
                        defaulter_fee_grid.ItemsSource = filter_lst.Where(x=>x.reg_fee != "0");
                        calculate_total_annaul();
                    }
                    if (index == 2)
                    {
                        defaulter_fee_grid.ItemsSource = filter_lst.Where(x=>x.tution_fee!="0");
                        calculate_total_tution();
                    }
                    if (index == 3)
                    {
                        defaulter_fee_grid.ItemsSource = filter_lst.Where(x=>x.other_expenses != "0");
                        calculate_total_other();
                    }
                    if (index == 4)
                    {
                        defaulter_fee_grid.ItemsSource = filter_lst.Where(x=>x.adm_fee != "0");
                        calculate_total_admission();
                    }
                    if (index == 5)
                    {
                        defaulter_fee_grid.ItemsSource = filter_lst.Where(x=>x.exam_fee != "0");                        
                        calculate_total_exam();
                    }
                    if (index == 6)
                    {
                        defaulter_fee_grid.ItemsSource = filter_lst.Where(x=>x.security_fee != "0");
                        calculate_total_security();
                    }
                    if (index == 7)
                    {
                        defaulter_fee_grid.ItemsSource = filter_lst.Where(x => x.fine_fee != "0");
                        calculate_total_fine();
                    }
                }      
            
            else 
            {
                //month_cmb.SelectedIndex = 0;
            }
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            
            var dataTable = CreateSampleDataTable();
            //var columnWidths = new List<double>() {40, 150, 60, 90, 75, 65, 70, 70, 70, 70, 70, 70, 70};
            var columnWidths = col_width;
            var ht = new FeeDefaulterHeader();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();
        }
        private DataTable CreateSampleDataTable()
        {
            DataTable dataTable = new DataTable();
            try
            {
                col_width = new List<double>();
                

                AddColumn(dataTable, "Sr#", typeof(string));
                col_width.Add(40);
                AddColumn(dataTable, "Student Name", typeof(string));
                col_width.Add(150);
                AddColumn(dataTable, "Adm#", typeof(string));
                col_width.Add(60);
                AddColumn(dataTable, "Cell#", typeof(string));
                col_width.Add(80);
                AddColumn(dataTable, "Class", typeof(string));
                col_width.Add(65);
                AddColumn(dataTable, "Section", typeof(string));
                col_width.Add(65);
                AddColumn(dataTable, "Month", typeof(string));
                col_width.Add(80);
                if (MainWindow.feeAdminPanel.reg_fee == "Y")
                {
                    AddColumn(dataTable, "Annual", typeof(string));
                    col_width.Add(45);
                }
                if (MainWindow.feeAdminPanel.adm_fee == "Y")
                {
                    AddColumn(dataTable, "Adm", typeof(string));
                    col_width.Add(45);
                }
                if (MainWindow.feeAdminPanel.tution_fee == "Y")
                {
                    AddColumn(dataTable, "Tution", typeof(string));
                    col_width.Add(45);
                }
                if (MainWindow.feeAdminPanel.other_fee == "Y")
                {
                    AddColumn(dataTable, "Other", typeof(string));
                    col_width.Add(45);
                }
                if (MainWindow.feeAdminPanel.security_fee == "Y")
                {
                    AddColumn(dataTable, "Security", typeof(string));
                    col_width.Add(45);
                }
                if (MainWindow.feeAdminPanel.exam_fee == "Y")
                {
                    AddColumn(dataTable, "Exam", typeof(string));
                    col_width.Add(45);
                }
                if (MainWindow.feeAdminPanel.fine_fee == "Y")
                {
                    AddColumn(dataTable, "Fine", typeof(string));
                    col_width.Add(45);
                }


                int j = 0;
                int k = 6;
                int reg = 0;
                int adm = 0;
                int sec = 0;
                int tution = 0;
                int exam = 0;
                int other = 0;
                int fine = 0;

                for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
                {
                    j++;
                    fee f = (fee)defaulter_fee_grid.Items[i];
                    var dataRow = dataTable.NewRow();

                    dataRow[0] = j.ToString();
                    dataRow[1] = f.std_name.ToString();
                    dataRow[2] = f.adm_no.ToString();
                    dataRow[3] = f.std_cell_no.ToString();
                    dataRow[4] = f.class_name.ToString();
                    dataRow[5] = f.section_name.ToString();
                    dataRow[6] = f.month;
                    if (MainWindow.feeAdminPanel.reg_fee == "Y")
                    {
                        if (i == 0)
                        {
                            reg = ++k;
                        }
                        dataRow[reg] = f.reg_fee.ToString();
                    }
                    if (MainWindow.feeAdminPanel.adm_fee == "Y")
                    {
                        if (i == 0)
                        {
                            adm = ++k;
                        }
                        dataRow[adm] = f.adm_fee.ToString();
                    }
                    if (MainWindow.feeAdminPanel.tution_fee == "Y")
                    {
                        if (i == 0)
                        {
                            tution = ++k;
                        }
                        dataRow[tution] = f.tution_fee.ToString();
                    }
                    if (MainWindow.feeAdminPanel.other_fee == "Y")
                    {
                        if (i == 0)
                        {
                            other = ++k;
                        }
                        dataRow[other] = f.other_expenses.ToString();
                    }
                    if (MainWindow.feeAdminPanel.security_fee == "Y")
                    {
                        if (i == 0)
                        {
                            sec = ++k;
                        }
                        dataRow[sec] = f.security_fee.ToString();
                    }
                    if (MainWindow.feeAdminPanel.exam_fee == "Y")
                    {
                        if (i == 0)
                        {
                            exam = ++k;
                        }
                        dataRow[exam] = f.exam_fee.ToString();
                    }
                    if (MainWindow.feeAdminPanel.fine_fee == "Y")
                    {
                        if (i == 0)
                        {
                            fine = ++k;
                        }
                        dataRow[fine] = f.fine_fee.ToString();
                    }
                    dataTable.Rows.Add(dataRow);
                }

                return dataTable;
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message);
                return dataTable;
            }
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        public void calculate_amount() 
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++ ) 
            {
               fee f= (fee)defaulter_fee_grid.Items[i];
               total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee) + Convert.ToInt32(f.fine_fee);
               total_fee = total + total_fee;
            }

            total_fee_paid_tb.Text = total_fee.ToString();
        }

        private void defaulter_fee_grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void calculate_total_annaul() 
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fee f = (fee)defaulter_fee_grid.Items[i];
                total = Convert.ToInt32(f.reg_fee) ;
                total_fee = total + total_fee;
            }
            total_fee_paid_tb.Text = total_fee.ToString();
        }
        public void calculate_total_tution()
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fee f = (fee)defaulter_fee_grid.Items[i];
                total = Convert.ToInt32(f.tution_fee);
                total_fee = total + total_fee;
            }
            total_fee_paid_tb.Text = total_fee.ToString();
        }
        public void calculate_total_other()
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fee f = (fee)defaulter_fee_grid.Items[i];
                total = Convert.ToInt32(f.other_expenses);
                total_fee = total + total_fee;
            }
            total_fee_paid_tb.Text = total_fee.ToString();
        }
        public void calculate_total_admission()
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fee f = (fee)defaulter_fee_grid.Items[i];
                total = Convert.ToInt32(f.adm_fee);
                total_fee = total + total_fee;
            }
            total_fee_paid_tb.Text = total_fee.ToString();
        }
        public void calculate_total_security()
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fee f = (fee)defaulter_fee_grid.Items[i];
                total = Convert.ToInt32(f.security_fee);
                total_fee = total + total_fee;
            }
            total_fee_paid_tb.Text = total_fee.ToString();
        }
        public void calculate_total_exam()
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fee f = (fee)defaulter_fee_grid.Items[i];
                total = Convert.ToInt32(f.exam_fee);
                total_fee = total + total_fee;
            }
            total_fee_paid_tb.Text = total_fee.ToString();
        }
        public void calculate_total_fine()
        {
            int total = 0;
            int total_fee = 0;
            for (int i = 0; i < defaulter_fee_grid.Items.Count; i++)
            {
                fee f = (fee)defaulter_fee_grid.Items[i];
                total = Convert.ToInt32(f.fine_fee);
                total_fee = total + total_fee;
            }
            total_fee_paid_tb.Text = total_fee.ToString();
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTextBox.Focus();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
            calculate_amount();
        }

        public void search_box()
        {            
                string v_search = SearchTextBox.Text;
                ICollectionView cv = CollectionViewSource.GetDefaultView(defaulter_fee_grid.ItemsSource);
                if (v_search == null)
                {                    
                    cv.Filter = null;
                }
                else
                {                    
                    cv.Filter = o =>
                    {
                        fee f = o as fee;
                        if (search_cmb.SelectedIndex == 0)
                        {
                            return (f.std_name.ToUpper().StartsWith(v_search.ToUpper()));
                        }
                        else if (search_cmb.SelectedIndex == 1)
                        {
                            return (f.father_name.ToUpper().StartsWith(v_search.ToUpper()));
                        }
                        else if (search_cmb.SelectedIndex == 2)
                        {
                            return (f.adm_no.ToUpper().StartsWith(v_search.ToUpper()));
                        }
                        else if (search_cmb.SelectedIndex == 3)
                        {
                            return (f.std_cell_no.ToUpper().StartsWith(v_search.ToUpper()));
                        }
                        else 
                        {
                            return true;
                        }

                    };
                }           
            SearchTextBox.Focus();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(defaulter_fee_grid, itemsSourceChanged);
            }
        }

        private void itemsSourceChanged(object sender, EventArgs e)
        {
            strength_textblock.Text = defaulter_fee_grid.Items.Count.ToString();
        }
    }
}
