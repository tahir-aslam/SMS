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
using System.IO;
using SUT.PrintEngine.Utils;
using System.Data;
using SMS.PrintHeaderTemplates;
using System.Windows.Markup;

namespace SMS.FeeManagement.PaidFeeList
{
    /// <summary>
    /// Interaction logic for PaidFee.xaml
    /// </summary>
    public partial class PaidFee : Page
    {
        public List<fee> paid_fee_list;
        List<fee> total_paid_list;
        string months;
        List<fee_history> fee_history_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        fee_history fh;
        public static int total_fee_paid = 0;
        string class_id = "0";
        public static DateTime dt;
        List<sms_months> months_list;
        List<fee> fee_list;

        public PaidFee()
        {
            InitializeComponent();

            section_cmb.IsEnabled = false;
            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name = "---Select Month---", id = "-1" });
            month_cmb.ItemsSource = months_list;

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            get_all_admissions();
            date_picker_to.SelectedDate = DateTime.Now;
            date_picker.SelectedDate = DateTime.Now;
            fill_filter_cmb();
        }

        // =======      Fill month cmb        =============================
        public void fill_filter_cmb()
        {            
            filterCmb.Items.Add("--Select Filter--");
            filterCmb.Items.Add("Annual Fund");
            filterCmb.Items.Add("Tution Fee");
            filterCmb.Items.Add("Other Fee");
            filterCmb.Items.Add("Admission Fee");
            filterCmb.Items.Add("Exam Fee");
            filterCmb.Items.Add("Security Fee");
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

        //========      Get Fee History       =============================
        public void get_fee_history(string id)
        {
            int total_amount = 0;
            int total_paid = 0;
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where session_id="+MainWindow.session.id;
                        cmd.Parameters.Add("@months", MySqlDbType.VarChar).Value = months;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee paid_fee = new fee()
                            {
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),                                
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                            };
                                if(paid_fee.date_time.Month == Convert.ToInt32(id))
                                {
                                    total_paid = Convert.ToInt32(paid_fee.reg_fee) + Convert.ToInt32(paid_fee.adm_fee) + Convert.ToInt32(paid_fee.tution_fee) + Convert.ToInt32(paid_fee.other_expenses) + Convert.ToInt32(paid_fee.security_fee) + Convert.ToInt32(paid_fee.exam_fee) + Convert.ToInt32(paid_fee.fine_fee);
                                    if(total_paid > 0)
                                    {
                                        total_amount = get_fee_data(paid_fee.month, paid_fee.std_id);
                                        paid_fee.total_paid = total_paid.ToString();
                                        paid_fee.total_balance = total_amount.ToString();
                                        paid_fee_list.Add(paid_fee);
                                    }                                    
                                }
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
                cmd.CommandText = "SELECT* FROM sms_admission where session_id=" + MainWindow.session.id+" ORDER BY adm_no ASC";
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
                            tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
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
        
        private void click_edit(object sender, RoutedEventArgs e)
        {

        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            date_picker.SelectedDate = DateTime.Now;
            class_cmb.SelectedIndex = 0;
        }

        private void month_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sms_months sm = (sms_months)month_cmb.SelectedItem;
            if (month_cmb.SelectedIndex != 0)
            {
                date_picker.SelectedDate = null;
                class_cmb.IsEnabled = true;
                class_cmb.SelectedIndex = 0;
                total_fee_paid = 0;
                paid_fee_grid.ItemsSource = null;
                months = month_cmb.SelectedItem.ToString();
                get_fee_history(sm.month_id);
                set_paid_list();
                paid_fee_grid.ItemsSource = total_paid_list.OrderBy(y=>y.class_id).OrderBy(x => x.section_id);
                total_fee_paid_tb.Text = total_fee_paid.ToString();
            }
            else 
            {               
                section_cmb.IsEnabled = false;
                class_cmb.SelectedIndex = 0;
                

                total_fee_paid_tb.Text = "0";
                total_fee_paid = 0;
                paid_fee_grid.ItemsSource = null;
            }

        }

        public void set_paid_list() 
        {
            int total = 0;
            total_fee_paid = 0;
            total_paid_list = new List<fee>();
            List<string> months_list;
            List<string> receipt_list;
            fee total_paid_obj;
            

            int regFee = 0;
            int admFee = 0;
            int secFee = 0;
            int examFee = 0;
            int tutionFee = 0;
            int otherFee = 0;
            int fineFee = 0;

            if (splitCheckBox.IsChecked == false)
            {
               foreach(var id in paid_fee_list.Select(x=>x.std_id).Distinct())
               {
                   regFee = 0;
                   admFee = 0;
                   secFee = 0;
                   examFee = 0;
                   tutionFee = 0;
                   otherFee = 0;
                   fineFee = 0;
                   total_paid_obj = new fee();
                   months_list = new List<string>();
                   receipt_list = new List<string>();

                   foreach (fee fee in paid_fee_list.Where(x => x.std_id == id))
                   {
                       regFee = regFee + Convert.ToInt32(fee.reg_fee);
                       admFee = admFee + Convert.ToInt32(fee.adm_fee);
                       secFee = secFee + Convert.ToInt32(fee.security_fee);
                       examFee = examFee + Convert.ToInt32(fee.exam_fee);
                       tutionFee = tutionFee + Convert.ToInt32(fee.tution_fee);
                       otherFee = otherFee + Convert.ToInt32(fee.other_expenses);
                       fineFee = fineFee + Convert.ToInt32(fee.fine_fee);

                       months_list.Add(fee.month);
                       receipt_list.Add(fee.receipt_no);
                       total_paid_obj = fee;
                   }

                   total_paid_obj.total_paid = (regFee + admFee + secFee + examFee + tutionFee + otherFee + fineFee).ToString();
                   total_paid_obj.reg_fee = regFee.ToString();
                   total_paid_obj.adm_fee = admFee.ToString();
                   total_paid_obj.security_fee = secFee.ToString();
                   total_paid_obj.exam_fee = examFee.ToString();
                   total_paid_obj.tution_fee = tutionFee.ToString();
                   total_paid_obj.other_expenses = otherFee.ToString();
                   total_paid_obj.fine_fee = fineFee.ToString();

                   //months
                   total_paid_obj.month = "";
                   foreach(string month in months_list.Distinct())
                   {
                       total_paid_obj.month = total_paid_obj.month + " " +month;
                   }

                   //receipt
                   total_paid_obj.receipt_no = "";
                   foreach(string receipt in receipt_list.Distinct())
                   {
                       total_paid_obj.receipt_no = total_paid_obj.receipt_no + " " + receipt;
                   }

                   total_paid_list.Add(total_paid_obj);
               }

               foreach (fee f in total_paid_list)
               {
                   foreach (admission adm in adm_list)
                   {
                       if (f.std_id == adm.id)
                       {
                           total_paid_obj = new fee();

                           f.std_name = adm.std_name;
                           f.image = adm.image;
                           f.class_name = adm.class_name;
                           f.class_id = adm.class_id;
                           f.section_id = adm.section_id;
                           f.section_name = adm.section_name;
                           f.adm_no = adm.adm_no;
                           f.father_name = adm.father_name;
                           f.rem_tution_fee = adm.tution_fee;
                       }
                   }

               }
            }
            else 
            {
                foreach (fee f in paid_fee_list)
                {
                    foreach (admission adm in adm_list)
                    {
                        if (f.std_id == adm.id)
                        {
                            total_paid_obj = new fee();

                            f.std_name = adm.std_name;
                            f.image = adm.image;
                            f.class_name = adm.class_name;
                            f.class_id = adm.class_id;
                            f.section_id = adm.section_id;
                            f.section_name = adm.section_name;
                            f.adm_no = adm.adm_no;
                            f.father_name = adm.father_name;
                            f.rem_tution_fee = adm.tution_fee;

                            total_paid_obj = f;
                            total_paid_list.Add(total_paid_obj);
                        }
                    }

                }
            }

            

            foreach(fee f in total_paid_list)
            {
                total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee) + Convert.ToInt32(f.fine_fee);
                total_fee_paid = total_fee_paid + total;
            }
        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int total = 0;
            int total_fee_paid_class = 0;
            classes c = (classes)class_cmb.SelectedItem;
            class_id = c.id;
            if (class_cmb.SelectedIndex != 0)
            {
                
                section_cmb.IsEnabled = true;

                get_all_sections(c.id);
                sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });               
                section_cmb.ItemsSource = sections_list;                
                section_cmb.SelectedIndex = 0;
                paid_fee_grid.ItemsSource = paid_fee_list.Where(x=>x.class_id == c.id);

                for (int i = 0; i < paid_fee_grid.Items.Count;i++ ) 
                {
                    fee f = (fee)paid_fee_grid.Items[i];
                    if (f.class_id == c.id)
                    {
                        total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee);
                        total_fee_paid_class = total_fee_paid_class + total;
                    }
                }
                total_fee_paid_tb.Text = total_fee_paid_class.ToString();
                total_fee_paid = total_fee_paid_class;
            }
            else 
            {
                section_cmb.IsEnabled = false;
                section_cmb.SelectedIndex = 0;
                paid_fee_grid.ItemsSource = paid_fee_list;
            }
        }

        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int total = 0;
            int total_fee_paid_section = 0;
            sections s = (sections)section_cmb.SelectedItem;
            if (s != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    paid_fee_grid.ItemsSource = paid_fee_list.Where(x => x.section_id == s.id);
                    for (int i = 0; i < paid_fee_grid.Items.Count; i++)
                    {
                        fee f = (fee)paid_fee_grid.Items[i];
                        if (f.class_id == s.id)
                        {
                            total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee);
                            total_fee_paid_section = total_fee_paid_section + total;
                        }
                    }
                    total_fee_paid_tb.Text = total_fee_paid_section.ToString();
                    total_fee_paid = total_fee_paid_section;

                }
                else
                {
                    paid_fee_grid.ItemsSource = paid_fee_list.Where(x => x.class_id == class_id);
                }
            }

            
        }

       

        public void get_fee_history_by_date(DateTime startDate, DateTime endDate) 
        {
            int total_amount = 0;
            int total_paid = 0;
            string sDate;
            string eDate;
            string dateTime;
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where session_id=" + MainWindow.session.id;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;

                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee paid_fee = new fee()
                            {
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"]),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };

                            dateTime = paid_fee.date_time.ToString("dd-MMM-yyyy");
                            sDate = startDate.ToString("dd-MMM-yyyy");
                            eDate = endDate.ToString("dd-MMM-yyyy");

                            if(Convert.ToDateTime(dateTime) >= Convert.ToDateTime(sDate) && Convert.ToDateTime(dateTime) <= Convert.ToDateTime(eDate))
                            {
                                total_paid = Convert.ToInt32(paid_fee.reg_fee) + Convert.ToInt32(paid_fee.adm_fee) + Convert.ToInt32(paid_fee.tution_fee) + Convert.ToInt32(paid_fee.other_expenses) + Convert.ToInt32(paid_fee.security_fee) + Convert.ToInt32(paid_fee.exam_fee) + Convert.ToInt32(paid_fee.fine_fee);
                                if(total_paid > 0)
                                {
                                    //total_amount = get_fee_data(paid_fee.month, paid_fee.std_id);

                                    //paid_fee.total_balance = total_amount.ToString();
                                    paid_fee.total_paid = total_paid.ToString();
                                    paid_fee_list.Add(paid_fee);
                                }                                
                           }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void print_button_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 30, 110, 110, 50, 60, 50, 60, 60, 100, 50, 50, 50, 50, 50, 50 };
            // var columnWidths = new List<double>() { 30, 100, 100, 50, 60, 50, 50, 80, 50, 50, 50, 50, 50, 50, 50, 40 };
            var ht = new PaidFeeLedger();
            var headerTemplates = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplates);
            printControl.ShowPrintPreview();
        }

        private DataTable CreateSampleDataTable()
        {
            var dataTable = new DataTable();

            AddColumn(dataTable, "Sr#", typeof(string));
            AddColumn(dataTable, "Name", typeof(string));
            AddColumn(dataTable, "Father Name", typeof(string));
            AddColumn(dataTable, "Adm#", typeof(string));
            AddColumn(dataTable, "Class", typeof(string));
            AddColumn(dataTable, "Section", typeof(string));
            AddColumn(dataTable, "Receipt#", typeof(string));
            AddColumn(dataTable, "A.T Fee", typeof(string));
            AddColumn(dataTable, "Month", typeof(string));
            AddColumn(dataTable, "Adm Fee", typeof(string));
            AddColumn(dataTable, "A.Fund", typeof(string));
            AddColumn(dataTable, "Tution Fee", typeof(string));
            AddColumn(dataTable, "Other", typeof(string));            
            AddColumn(dataTable, "Total Paid", typeof(string));            
            AddColumn(dataTable, "Received", typeof(string));

            # region hide
            //AddColumn(dataTable, "Sr#", typeof(string));
            //AddColumn(dataTable, "Name", typeof(string));
            //AddColumn(dataTable, "Father Name", typeof(string));
            //AddColumn(dataTable, "Adm#", typeof(string));
            //AddColumn(dataTable, "Class", typeof(string));
            //AddColumn(dataTable, "Section", typeof(string));
            //AddColumn(dataTable, "Receipt#", typeof(string));
            //AddColumn(dataTable, "Month", typeof(string));
            //AddColumn(dataTable, "Adm Fee", typeof(string));
            //AddColumn(dataTable, "A.Fund", typeof(string));
            //AddColumn(dataTable, "Tution Fee", typeof(string));
            //AddColumn(dataTable, "Other", typeof(string));
            //AddColumn(dataTable, "Exam", typeof(string));
            //AddColumn(dataTable, "Security", typeof(string));
            //AddColumn(dataTable, "Total Paid", typeof(string));
            ////AddColumn(dataTable, "Balance", typeof(string));
            //AddColumn(dataTable, "Received", typeof(string));
            #endregion
            int j = 0;
            for (int i = 0; i < paid_fee_grid.Items.Count;i++ )
            {
                j++;
                fee f = (fee)paid_fee_grid.Items[i];

                var dataRow = dataTable.NewRow();
                dataRow[0] = j.ToString();
                dataRow[1] = f.std_name.ToString();
                dataRow[2] = f.father_name.ToString();
                dataRow[3] = f.adm_no.ToString();
                dataRow[4] = f.class_name.ToString();
                dataRow[5] = f.section_name.ToString();
                dataRow[6] = f.receipt_no.ToString();
                dataRow[7] = f.rem_tution_fee.ToString();
                dataRow[8] = f.month.ToString();
                dataRow[9] = f.adm_fee.ToString();
                dataRow[10] = f.reg_fee.ToString();
                dataRow[11] = f.tution_fee.ToString();
                dataRow[12] = f.other_expenses.ToString();                
                dataRow[13] = f.total_paid.ToString();                
                dataRow[14] = f.created_by.ToString();

                #region hide
                //var dataRow = dataTable.NewRow();
                //dataRow[0] = j.ToString();
                //dataRow[1] = f.std_name.ToString();
                //dataRow[2] = f.father_name.ToString();
                //dataRow[3] = f.adm_no.ToString();
                //dataRow[4] = f.class_name.ToString();
                //dataRow[5] = f.section_name.ToString();
                //dataRow[6] = f.receipt_no.ToString();
                //dataRow[7] = f.month.ToString();
                //dataRow[8] = f.adm_fee.ToString();
                //dataRow[9] = f.reg_fee.ToString();
                //dataRow[10] = f.tution_fee.ToString();
                //dataRow[11] = f.other_expenses.ToString();
                //dataRow[12] = f.exam_fee.ToString();
                //dataRow[13] = f.security_fee.ToString();
                //dataRow[14] = f.total_paid.ToString();
                ////dataRow[15] = f.total_balance.ToString();
                //dataRow[15] = f.created_by.ToString();
                #endregion
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            search_box();
        }

        public void search_box()
        {
            int total = 0;
            total_fee_paid = 0;
            int adm_no= 0;
            int search= 0;

            if (search_cmb.SelectedIndex == 0)
            {
                string v_search = SearchTextBox.Text;
                paid_fee_grid.ItemsSource = total_paid_list.Where(x => x.adm_no.ToUpper().StartsWith(v_search.ToUpper()) || x.adm_no.ToUpper().Contains(v_search.ToUpper()));               
                paid_fee_grid.Items.Refresh();
            }
            else if (search_cmb.SelectedIndex == 1)
            {
                string v_search = SearchTextBox.Text;
                paid_fee_grid.ItemsSource = total_paid_list.Where(x => x.receipt_no.StartsWith(v_search));
                paid_fee_grid.Items.Refresh();
            }
            if (search_cmb.SelectedIndex == 2)
            {
                string v_search = SearchTextBox.Text;
                paid_fee_grid.ItemsSource = total_paid_list.Where(x => x.std_name.ToUpper().StartsWith(v_search.ToUpper()));
                paid_fee_grid.Items.Refresh();
            }           
            else
            {
            }
            SearchTextBox.Focus();

            for (int i = 0; i < paid_fee_grid.Items.Count;i++ )
            {
                fee f = (fee)paid_fee_grid.Items[i];

                total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee) + Convert.ToInt32(f.fine_fee);
                total_fee_paid = total_fee_paid + total;
            }
            total_fee_paid_tb.Text = total_fee_paid.ToString();
        }

        private void search_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //SearchTextBox.Focus();           
        }

        public int get_orginal_tution_fee(string std_id)
        {
            int tution_fee = 0;
            fee_list = new List<fee>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT tution_fee FROM sms_admission where isActive='Y' && session_id=" + MainWindow.session.id + " && std_id=" + std_id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            tution_fee = Convert.ToInt32(reader["tution_fee"]);                           
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            return tution_fee;
        }

        //========      Get fee data          ====================================
        public int get_fee_data(string month,string std_id)
        {
            int total = 0;
            fee_list = new List<fee>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where month = @month && isActive='Y' && session_id=" + MainWindow.session.id+" && std_id="+std_id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {                       

                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = month;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee f = new fee();

                            f.transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString());
                            f.reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString());                            
                            f.tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString());
                            f.rem_tution_fee = Convert.ToString(reader["tution_fee"].ToString()); // for original tution fee
                            f.exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString());
                            f.security_fee = Convert.ToString(reader["rem_security_fee"].ToString());
                            f.other_expenses = Convert.ToString(reader["rem_other_exp"].ToString());
                            f.fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString());
                            f.adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString());
                            f.month = Convert.ToString(reader["month"].ToString());
                            f.std_id = Convert.ToString(reader["std_id"].ToString());

                            total = Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee) + Convert.ToInt32(f.fine_fee);
                        }
                                                
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            return total;
        }

        private void filterCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Annual Fund
            if(filterCmb.SelectedIndex == 1)
            {

            }
        }

        private void date_picker_to_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker.SelectedDate;

                if (date_picker_to.SelectedDate <= date_picker.SelectedDate)
                {
                    month_cmb.SelectedIndex = 0;
                    total_fee_paid = 0;
                    dt = date_picker.SelectedDate.Value;
                    get_fee_history_by_date(date_picker_to.SelectedDate.Value, date_picker.SelectedDate.Value);
                    set_paid_list();
                    paid_fee_grid.ItemsSource = total_paid_list.OrderBy(y => y.class_id).OrderBy(x => x.section_id);
                    total_fee_paid_tb.Text = total_fee_paid.ToString();
                }
            }   
        }

        private void date_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (date_picker.SelectedDate != null && date_picker_to.SelectedDate != null)
            {
                date_picker.DisplayDateStart = date_picker_to.SelectedDate;
                date_picker_to.DisplayDateEnd = date_picker.SelectedDate;
                if (date_picker_to.SelectedDate <= date_picker.SelectedDate)
                {
                    month_cmb.SelectedIndex = 0;
                    total_fee_paid = 0;
                    dt = date_picker.SelectedDate.Value;
                    get_fee_history_by_date(date_picker_to.SelectedDate.Value, date_picker.SelectedDate.Value);
                    set_paid_list();
                    paid_fee_grid.ItemsSource = total_paid_list.OrderBy(y => y.class_id).OrderBy(x => x.section_id);
                    total_fee_paid_tb.Text = total_fee_paid.ToString();
                }
            }
        }

        private void splitCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
