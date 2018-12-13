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
using System.IO;
using SMS.FeeManagement.FeeVouchers;
using System.Collections.ObjectModel;
using SUT.PrintEngine.Utils;
using SUT.PrintEngine.Views;
using System.Windows.Markup;



namespace SMS.FeeManagement.FeeVouchers
{
    /// <summary>
    /// Interaction logic for FeeVoucherSearch.xaml
    /// </summary>
    public partial class FeeVoucherSearch : Page
    {
        int pending_amount = 0;
        string pending_desc;
        int months_id;
        string months;
        sections s;
        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_months> months_list;
        ObservableCollection<admission> adm_list;
        List<admission> checked_adm_list;
        string class_id = "0";
        VoucherOptionWindow vow;
        fee_voucher fee_v_obj;
        List<fee> fee_list;
        List<fee_voucher> fee_voucher_list;
        bool two_slips = false;
        bool one_slips = false;
        string last_receipt_no = "";
        int reciept_no = 0;
        List<fee_voucher> fee_voucher_lst_pending;

        string bank_name = "";
        string branch_name = "";
        string account_no = "";
        string account_title = "";
        string fee_note = "";

        List<fee> fine_fee_list;
        List<sms_months> fine_months;
        int fine_amount = 0;
        string fine_desc = "";
        List<fee_voucher> fine_voucher_lst;

        List<fee> other_fee_list;
        List<sms_months> other_months;
        int other_amount = 0;
        string other_desc = "";
        List<fee_voucher> other_voucher_lst;
        List<other_fee> other_desc_list;
        List<fee> paid_fee_list;

        public FeeVoucherSearch()
        {
            InitializeComponent();

            section_cmb.IsEnabled = false;
            class_cmb.IsEnabled = false;

            get_all_months();
            month_cmb.SelectedIndex = 0;
            months_list.Insert(0, new sms_months() { month_name="---Select Month---",id="-1"});
            month_cmb.ItemsSource = months_list;

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            get_bank_details();
            get_fee_note();
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
            adm_list = new ObservableCollection<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && section_id=" + s.id + "&& session_id=" + MainWindow.session.id;
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
            sms_months sm = new sms_months();
            if (month_cmb.SelectedIndex != 0)
            {                
                class_cmb.IsEnabled = true;
                class_cmb.SelectedIndex = 0;
                sm = (sms_months)month_cmb.SelectedItem;
                months = sm.month_name;
                months_id =Convert.ToInt16(sm.id);
            }
            else
            {
                class_cmb.IsEnabled = false;
                section_cmb.IsEnabled = false;
                class_cmb.SelectedIndex = 0;                
            }

        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            class_id = c.id;            
            if (class_cmb.SelectedIndex != 0)
            {

                section_cmb.IsEnabled = true;

                get_all_sections(c.id);
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

        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            s = (sections)section_cmb.SelectedItem;

            if (s != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    voucher_lstbox.Visibility = Visibility.Hidden;
                    voucher3_lstbox.Visibility = Visibility.Hidden;
                    img_grid.Visibility = Visibility.Hidden;
                    adm_grid.Visibility = Visibility.Visible;
                    print_btn.Visibility = Visibility.Hidden;
                    get_all_admissions();
                    adm_datagrid.ItemsSource = adm_list;
                }
                else
                {
                    img_grid.Visibility = Visibility.Visible;
                    adm_grid.Visibility = Visibility.Hidden;
                    voucher_lstbox.Visibility = Visibility.Hidden;
                    voucher3_lstbox.Visibility = Visibility.Hidden;
                    print_btn.Visibility = Visibility.Hidden;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (null == checkBox) return;

            foreach (var item in adm_list)
            {
                item.Checked = checkBox.IsChecked.Value;
            }
            adm_datagrid.Items.Refresh();

        }
        private void CheckBox_Checked_sub(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            adm_datagrid.SelectedItem = e.Source;
            admission adm = new admission();
            adm = (admission)adm_datagrid.SelectedItem;
            foreach (admission s in adm_list)
            {
                if (adm.id == s.id)
                {
                    s.Checked = checkBox.IsChecked.Value;
                }
            }

        }

        private void proceed_btn_Click(object sender, RoutedEventArgs e)
        {
            get_all_checked_admission();
            if (checked_adm_list.Count > 0)
            {
                vow = new VoucherOptionWindow(this);
                vow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                vow.ShowDialog();                
            }
            else 
            {
                MessageBox.Show("Select Minimum One Student","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
            }
        }

        //get All checked admission
        public void get_all_checked_admission() 
        {
            checked_adm_list = new List<admission>();
            foreach(admission adm in adm_list)
            {
                if(adm.Checked == true)
                {
                    checked_adm_list.Add(adm);
                }
            }            
        }

        public void get_voucher_options(bool pending,bool one, bool two, bool three) 
        {
            img_grid.Visibility = Visibility.Hidden;
            adm_grid.Visibility = Visibility.Hidden;            
            print_btn.Visibility = Visibility.Visible;
            get_fee_data();
            set_fee_data(pending);

            if (one == true)
            {
                voucher1_lstbox.Visibility = Visibility.Visible;
                voucher1_grid.Visibility = Visibility.Visible;
                voucher1_lstbox.IsEnabled = true;
                voucher3_lstbox.Visibility = Visibility.Hidden;
                voucher3_grid.Visibility = Visibility.Hidden;
                voucher_lstbox.Visibility = Visibility.Hidden;
                voucher2_grid.Visibility = Visibility.Hidden;
                voucher1_lstbox.ItemsSource = null;
                voucher1_lstbox.Items.Refresh();
                voucher1_lstbox.ItemsSource = fee_voucher_list;
                one_slips = true;
                two_slips = false;
            }
           else if (two == true)
            {
                voucher_lstbox.Visibility = Visibility.Visible;
                voucher2_grid.Visibility = Visibility.Visible;
                voucher_lstbox.IsEnabled = true;
                voucher3_lstbox.Visibility = Visibility.Hidden;
                voucher3_grid.Visibility = Visibility.Hidden;
                voucher1_lstbox.Visibility = Visibility.Hidden;
                voucher1_grid.Visibility = Visibility.Hidden;
                voucher_lstbox.ItemsSource = null;
                voucher_lstbox.Items.Refresh();
                voucher_lstbox.ItemsSource = fee_voucher_list;
                two_slips = true;
                one_slips = false;
            }
            else 
            {
                voucher3_lstbox.Visibility = Visibility.Visible;
                voucher3_grid.Visibility = Visibility.Visible;
                voucher_lstbox.Visibility = Visibility.Hidden;
                voucher2_grid.Visibility = Visibility.Hidden;
                voucher1_lstbox.Visibility = Visibility.Hidden;
                voucher1_grid.Visibility = Visibility.Hidden;
                voucher3_lstbox.ItemsSource = null;
                voucher3_lstbox.Items.Refresh();
                voucher3_lstbox.ItemsSource = fee_voucher_list;                
                two_slips = false;
                one_slips = false;
            }

        }

        public void get_fee_data() 
        {
            fee_list = new List<fee>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where isActive='Y' && session_id="+MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {                        
                        cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = months;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            fee f = new fee()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),                                
                                rem_reg_fee = Convert.ToString(reader["rem_reg_fee"].ToString()),
                                rem_adm_fee = Convert.ToString(reader["rem_adm_fee"].ToString()),
                                rem_tution_fee = Convert.ToString(reader["rem_tution_fee"].ToString()),
                                rem_other_fee = Convert.ToString(reader["rem_other_exp"].ToString()),
                                rem_exam_fee = Convert.ToString(reader["rem_exam_fee"].ToString()),
                                rem_security_fee = Convert.ToString(reader["rem_security_fee"].ToString()),
                                rem_transport_fee = Convert.ToString(reader["rem_transport_fee"].ToString()),
                                rem_fine_fee = Convert.ToString(reader["rem_fine_fee"].ToString()),
                                
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

        public void set_fee_data( bool p) 
        {
            int total = 0;
            fee_voucher_list = new List<fee_voucher>();
            get_last_reciept_no();
            reciept_no = Convert.ToInt32(last_receipt_no);
            foreach(admission adm in checked_adm_list)
            {
                foreach(fee f in fee_list.Where(x=>x.month == months))
                {
                    if(adm.id == f.std_id)
                    {
                        fee_v_obj = new fee_voucher();

                        fee_v_obj.std_id = adm.id;
                        fee_v_obj.institute_logo = MainWindow.ins.institute_logo;
                        fee_v_obj.institute_name = MainWindow.ins.institute_name;
                        fee_v_obj.std_name = adm.std_name;
                        fee_v_obj.class_name = adm.class_name;
                        fee_v_obj.section_name = adm.section_name;
                        fee_v_obj.month = months;
                        fee_v_obj.adm_no = adm.adm_no;
                        fee_v_obj.date_time = DateTime.Now.ToString("dd/MM/yyyy");
                        fee_v_obj.father_name = adm.father_name;
                        fee_v_obj.reciept_no = (++reciept_no).ToString();

                        fee_v_obj.rem_reg_fee = f.rem_reg_fee;
                        fee_v_obj.rem_adm_fee = f.rem_adm_fee;
                        fee_v_obj.rem_tution_fee = f.rem_tution_fee;
                        fee_v_obj.rem_other_fee = f.rem_other_fee;
                        fee_v_obj.rem_exam_fee = f.rem_exam_fee;
                        fee_v_obj.rem_transport_fee = f.rem_transport_fee;
                        fee_v_obj.rem_security_fee = f.rem_security_fee;

                        //bank details
                        fee_v_obj.bank_name = bank_name;
                        fee_v_obj.branch_name = branch_name;
                        fee_v_obj.account_no = account_no;
                        fee_v_obj.account_title = account_title;

                        fee_v_obj.fee_note = fee_note;

                        //get all fee history
                        paid_fee_list = new List<fee>();
                        paid_fee_list = get_fee_history(adm.id);

                        //get all other
                        get_all_other(adm.id,fee_v_obj.reciept_no);
                        fee_v_obj.other_amount = other_amount.ToString();
                        fee_v_obj.other_desc = other_desc;
                        fee_v_obj.other_list = other_voucher_lst;

                        //get all fine
                        get_all_fine(adm.id, fee_v_obj.reciept_no);
                        fee_v_obj.fine_amount = fine_amount.ToString();
                        fee_v_obj.fine_desc = fine_desc;
                        fee_v_obj.fine_list = fine_voucher_lst;

                        //get pending fee
                        get_pending_fee(adm.id,fee_v_obj.reciept_no);
                        if (p == true)
                        {
                            fee_v_obj.pending_amount = pending_amount.ToString();
                            fee_v_obj.pending_desc = pending_desc;
                            fee_v_obj.pending_list = fee_voucher_lst_pending;
                        }
                        else 
                        {
                            fee_v_obj.pending_amount = "0";
                            fee_v_obj.pending_desc = "Pending Fee: ";
                            fee_v_obj.pending_list = new List<fee_voucher>();
                        }
                        
                        try
                        {
                            total = Convert.ToInt32(fee_v_obj.rem_reg_fee) + Convert.ToInt32(fee_v_obj.rem_adm_fee) + Convert.ToInt32(fee_v_obj.rem_tution_fee) + Convert.ToInt32(fee_v_obj.other_amount) + Convert.ToInt32(fee_v_obj.rem_exam_fee) + Convert.ToInt32(fee_v_obj.fine_amount) + Convert.ToInt32(fee_v_obj.rem_security_fee) + Convert.ToInt32(fee_v_obj.pending_amount);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        
                        fee_v_obj.total = total.ToString();
                        fee_v_obj.total_in_words = NumberToWords(total);                        
                        fee_voucher_list.Add(fee_v_obj);
                        //break;
                    }                    
                }
            }
        }


        //Get Other Expense
        public List<other_fee> get_other_expense(string stdId)
        {
            List<other_fee> other_fee_list = new List<other_fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_other_fee where std_id =" + stdId + "&& session_id = " + MainWindow.session.id;

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            other_fee other = new other_fee()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                fee_type_id = Convert.ToInt32(reader["fee_type_id"]),
                                fee_type = Convert.ToString(reader["fee_type"].ToString()),                                
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                amount = Convert.ToInt32(reader["amount"]),
                            };
                            other_fee_list.Add(other);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return other_fee_list;
        }

        public void get_all_other(string id, string receipt_no)
        {
            other_desc_list = new List<other_fee>();
            other_desc_list = get_other_expense(id);
            other_voucher_lst = new List<fee_voucher>();
            fee_voucher fv;
            string m_name = "";
            other_amount = 0;
            other_desc = "Other: ";
            int monthly_pending = 0;
            pending_amount = 0;
            bool isPaid = false;

            for (int i = 1; i <= months_id; i++)
            {
                //get Month name
                m_name = "";
                foreach (sms_months sm in months_list)
                {
                    if (i.ToString() == sm.id)
                    {
                        m_name = sm.month_name;
                        break;
                    }
                }

                foreach (fee f in fee_list)
                {
                    if (f.std_id == id && f.month == m_name)
                    {
                       
                        monthly_pending = 0;
                        monthly_pending = Convert.ToInt32(f.rem_other_fee);
                        other_amount = other_amount + monthly_pending;
                        if (monthly_pending > 0)
                        {
                            other_desc = other_desc + m_name + "(";
                            foreach (other_fee other in other_desc_list.Where(x => x.month_name == m_name))
                            {                                
                                // There is no functionality for partial other.
                                fv = new fee_voucher();
                                other_desc = other_desc + other.fee_type + ",";
                                fv.other_exp_id = other.id;
                                fv.other_exp_type = other.fee_type;
                                fv.other_exp_type_id = other.fee_type_id;
                                fv.rem_other_fee = other.amount.ToString();
                                fv.std_id = id;
                                fv.reciept_no = receipt_no;
                                fv.month = other.month_name;
                                fv.total = f.rem_other_fee;
                                other_voucher_lst.Add(fv);
                            }
                            other_desc = other_desc + ")";
                           // other_desc = other_desc + m_name + ",";
                        }                        
                        break;
                    }
                }
            }
        }       

        public void get_all_fine(string id, string receipt_no)
        {
            fine_voucher_lst = new List<fee_voucher>();
            fee_voucher fv;            
            string m_name="";
            fine_amount = 0;
            fine_desc = "Fine: ";
            int monthly_pending=0;
            pending_amount = 0;

            for (int i = 1; i <= months_id; i++)
            {
                //get Month name
                m_name = "";
                foreach (sms_months sm in months_list)
                {
                    if (i.ToString() == sm.id)
                    {
                        m_name = sm.month_name;
                        break;
                    }
                }

                foreach (fee f in fee_list)
                {
                    if (f.std_id == id && f.month == m_name)
                    {
                        fv = new fee_voucher();
                        monthly_pending = 0;
                        monthly_pending = Convert.ToInt32(f.rem_fine_fee);
                        fine_amount = fine_amount + monthly_pending;
                        if (monthly_pending > 0)
                        {
                            fine_desc = fine_desc + m_name + ",";
                        }
                        // pending fees
                        fv.rem_fine_fee = f.rem_fine_fee;
                        fv.total = monthly_pending.ToString();
                        fv.std_id = id;
                        fv.reciept_no = receipt_no;
                        fv.month = f.month;
                        
                        if (monthly_pending > 0)
                        {
                            fine_voucher_lst.Add(fv);
                        }
                        break;
                    }
                }
            }
        }       

        public void get_pending_fee(string id, string receipt_no)         
        {
            fee_voucher_lst_pending = new List<fee_voucher>();
            fee_voucher fv;
            int monthly_pending = 0;
            string m_name="";
            pending_amount = 0;
            pending_desc = "Pending Fee: ";
            
            for (int i = 1; i < months_id;i++) 
            {
                //get Month name
                m_name = "";
                foreach(sms_months sm in months_list)
                {
                    if(i.ToString() == sm.id)
                    {
                        m_name = sm.month_name;
                        break;
                    }
                }
                
                foreach (fee f in fee_list)
                {
                    if(f.std_id == id && f.month == m_name )
                    {
                        fv = new fee_voucher();
                        monthly_pending = 0;
                        monthly_pending = Convert.ToInt32(f.rem_tution_fee)  + Convert.ToInt32(f.rem_transport_fee) ;
                        pending_amount = pending_amount + monthly_pending;
                        if(monthly_pending > 0)
                        {
                            pending_desc = pending_desc + m_name + ",";
                        }
                        // pending fees
                        fv.rem_tution_fee = f.rem_tution_fee;
                        fv.rem_transport_fee = f.rem_transport_fee;
                        //fv.rem_other_fee = f.rem_other_fee;                       
                        fv.total = monthly_pending.ToString();
                        fv.std_id = id;
                        fv.reciept_no = receipt_no;
                        fv.month = f.month;

                        if (monthly_pending > 0)
                        {
                            fee_voucher_lst_pending.Add(fv);
                        }
                        break;
                    }
                }                
            }
        }
        // Printing
        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            submit();
            update_last_reciet_no();
            if (one_slips == true)
            {
                var visualSize = new Size(voucher1_lstbox.ActualWidth, voucher1_lstbox.ActualHeight);
                var printControl = PrintControlFactory.Create(visualSize, voucher1_lstbox);
                printControl.ShowPrintPreview();
            }
           else if (two_slips == true)
            {
                var visualSize = new Size(voucher_lstbox.ActualWidth, voucher_lstbox.ActualHeight);
                var printControl = PrintControlFactory.Create(visualSize, voucher_lstbox);
                printControl.ShowPrintPreview();
               //VisualPrintDialog printDlg = new VisualPrintDialog(this.voucher_lstbox);
                //printDlg.ShowDialog();

                //PrintDialog pd = new PrintDialog();
                //if (pd.ShowDialog() != true) return;

                //VoucherDocument.PageHeight = pd.PrintableAreaHeight;
                //VoucherDocument.PageWidth = pd.PrintableAreaWidth;

                //IDocumentPaginatorSource idocument = VoucherDocument as IDocumentPaginatorSource;

                //pd.PrintDocument(idocument.DocumentPaginator, "Printing Flow Document...");

                //PrintDialog pd = new PrintDialog();
                //var pageSize = new Size(8.26 * 96, 11.69 * 96); // A4 page, at 96 dpi
                //var document = new FixedDocument();
                //document.DocumentPaginator.PageSize = pageSize;

                //// Create FixedPage
                //var fixedPage = new FixedPage();
                //fixedPage.Width = pageSize.Width;
                //fixedPage.Height = pageSize.Height;
                //// Add visual, measure/arrange page.
                //fixedPage.Children.Add((UIElement)voucher_lstbox);
                //fixedPage.Measure(pageSize);
                //fixedPage.Arrange(new Rect(new Point(), pageSize));
                //fixedPage.UpdateLayout();

                //// Add page to document
                //var pageContent = new PageContent();
                //((IAddChild)pageContent).AddChild(fixedPage);
                //document.Pages.Add(pageContent);

                //// Send to the printer.
                //var pd = new PrintDialog();
                //pd.PrintDocument(document.DocumentPaginator, "My Document");
               
 
                //voucher2_grid.Visibility = Visibility.Hidden;
                //CreateMyWPFControlReport(fee_voucher_list);

                //PrintDialog dialog = new PrintDialog();

                //if (dialog.ShowDialog() != true) return;

                //voucher2_grid.Measure(new Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight));
                //voucher2_grid.Arrange(new Rect(new Point(50, 50), voucher2_grid.DesiredSize));

                //dialog.PrintVisual(voucher2_grid, "A WPF printing");

            }
            else 
            {                
                var visualSize = new Size(voucher3_lstbox.ActualWidth, voucher3_lstbox.ActualHeight);
                var printControl = PrintControlFactory.Create(visualSize, voucher3_lstbox);
                printControl.ShowPrintPreview();
            }      
            //PrintDialog printDlg = new PrintDialog();
            //printDlg.PrintVisual(Result_grid_lstbox, "Listbox Printing.");
            
        }

        //========      Get Fee History       =============================
        public List<fee> get_fee_history(string std_id)
        {
            List<fee> paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where std_id =" + std_id + "&& session_id = " + MainWindow.session.id;

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
                                other_exp_type = Convert.ToString(reader["other_exp_type"].ToString()),
                                other_exp_type_id = Convert.ToString(reader["other_exp_type_id"].ToString()),
                                other_exp_id = Convert.ToInt32(reader["other_exp_id"]),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                fine_fee_wave_off = Convert.ToString(reader["fine_fee_wave_off"].ToString()),
                                other_fee_wave_off = Convert.ToString(reader["other_fee_wave_off"].ToString()),
                                tution_fee_wave_off = Convert.ToString(reader["tution_fee_wave_off"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                            };
                            paid_fee_list.Add(paid_fee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return paid_fee_list;
        }


        public void CreateMyWPFControlReport(List<fee_voucher> fee_voucher_list)
        {
            //Set up the WPF Control to be printed
            //voucher_lstbox.DataContext = fee_voucher_list;

            voucher_lstbox = new ListBox();
            voucher_lstbox.DataContext = fee_voucher_list;

            FixedDocument fixedDoc = new FixedDocument();
            PageContent pageContent = new PageContent();
            FixedPage fixedPage = new FixedPage();

            //Create first page of document
            fixedPage.Children.Add(voucher_lstbox);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);
            //Create any other required pages here

            //View the document
            //documentViewer1.Document = fixedDoc;
        }

        //---------------           Submit Vouchers    ----------------------------------
        public void submit()
        {
            int i = 0;
            try
            {
                foreach (fee_voucher fv in fee_voucher_list)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_fee_vouchers(std_id,month,reciept_no,rem_reg_fee,rem_adm_fee,rem_tution_fee,rem_transport_fee,rem_exam_fee,total,created_by,date_time,rem_security_fee)Values(@std_id,@month,@reciept_no,@rem_reg_fee,@rem_adm_fee,@rem_tution_fee,@rem_transport_fee,@rem_exam_fee,@total,@created_by,@date_time,@rem_security_fee)";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.std_id;
                            cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.month;
                            cmd.Parameters.Add("@reciept_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.reciept_no;

                            cmd.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_reg_fee;
                            cmd.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_adm_fee;
                            cmd.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_tution_fee;
                            cmd.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_transport_fee;
                            //cmd.Parameters.Add("@rem_other_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_other_fee;
                            cmd.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_exam_fee;
                            cmd.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.rem_security_fee;
                            
                            cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv.total;
                            cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                            cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;                                                    

                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            con.Close();
                        }
                    }
                    foreach (fee_voucher fv_pending in fv.pending_list)
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_fee_vouchers(std_id,month,reciept_no,rem_tution_fee,rem_transport_fee,total,created_by,date_time)Values(@std_id,@month,@reciept_no,@rem_tution_fee,@rem_transport_fee,@total,@created_by,@date_time)";
                                cmd.Connection = con;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_pending.std_id;
                                cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_pending.month;
                                cmd.Parameters.Add("@reciept_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_pending.reciept_no;
                                
                                cmd.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_pending.rem_tution_fee;
                                cmd.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_pending.rem_transport_fee;
                                //cmd.Parameters.Add("@rem_other_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_pending.rem_other_fee;
                                

                                cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fv_pending.total;
                                cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                con.Close();
                            }
                        }
                    }
                    foreach (fee_voucher other_pending in fv.other_list)
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_fee_vouchers(std_id,month,reciept_no,rem_other_fee,total,created_by,date_time,other_exp_type,other_exp_type_id,other_exp_id)Values(@std_id,@month,@reciept_no,@rem_other_fee,@total,@created_by,@date_time,@other_exp_type,@other_exp_type_id,@other_exp_id)";
                                cmd.Connection = con;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other_pending.std_id;
                                cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other_pending.month;
                                cmd.Parameters.Add("@reciept_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other_pending.reciept_no;

                                cmd.Parameters.Add("@other_exp_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other_pending.other_exp_type;
                                cmd.Parameters.Add("@other_exp_type_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = other_pending.other_exp_type_id;
                                cmd.Parameters.Add("@other_exp_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = other_pending.other_exp_id;

                                cmd.Parameters.Add("@rem_other_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other_pending.rem_other_fee;
                                cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = other_pending.total;
                                cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                con.Close();
                            }
                        }
                    }

                    foreach (fee_voucher fine_pending in fv.fine_list)
                    {
                        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_fee_vouchers(std_id,month,reciept_no,rem_fine_fee,total,created_by,date_time)Values(@std_id,@month,@reciept_no,@rem_fine_fee,@total,@created_by,@date_time)";
                                cmd.Connection = con;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine_pending.std_id;
                                cmd.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine_pending.month;
                                cmd.Parameters.Add("@reciept_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine_pending.reciept_no;

                                cmd.Parameters.Add("@rem_fine_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine_pending.rem_fine_fee;
                                cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = fine_pending.total;
                                cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;

                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                con.Close();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        public void update_last_reciet_no() 
        {
            try 
            {
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {
                        cmd_fee.CommandText = "Update sms_last_receipt_no SET last_receipt_no=@last_receipt_no";
                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@last_receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = reciept_no.ToString();
                        

                        con_fee.Open();

                        Convert.ToInt32(cmd_fee.ExecuteScalar());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //get last receipt no
        public void get_last_reciept_no() 
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_last_receipt_no ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        try
                        {
                            con.Open();
                            MySqlDataReader reader = cmd.ExecuteReader();
                            reader.Read();

                            last_receipt_no = Convert.ToString(reader["last_receipt_no"].ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //get Fee note
        public void get_fee_note()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_fee_note ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        try
                        {
                            con.Open();
                            MySqlDataReader reader = cmd.ExecuteReader();
                            reader.Read();

                            fee_note = Convert.ToString(reader["fee_note"].ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //===========      Digits to words              ================================
        public string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "And ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public void get_bank_details() 
        {        
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_bank_details ";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {   
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while(reader.Read())
                        {
                            bank_name = Convert.ToString(reader["bank_name"].ToString());
                            branch_name = Convert.ToString(reader["branch_name"].ToString());
                            account_no = Convert.ToString(reader["account_no"].ToString());
                            account_title = Convert.ToString(reader["account_title"].ToString());                                                            
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
