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
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SMS.AdminPanel
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window, INotifyPropertyChanged
    {
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        sections s;
        int roll_no = 0;
        string last_adm_no = "0";
        List<sms_fees> feesListToBePaid;

        public AdminWindow()
        {
            InitializeComponent();
            section_cmb.IsEnabled = false;

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            this.DataContext = this;
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
                        MessageBox.Show("DB not connected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }

        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;            
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
            sections s = (sections)section_cmb.SelectedItem;
            if (s != null)
            {
                if (section_cmb.SelectedIndex != 0)
                {
                    
                }
                else
                {
                    
                }
            }


        }

        // ===============     Get All Admissions          ================
        public void get_all_admissions(string id)
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && section_id=" + id + "&& session_id=" + MainWindow.session.id+" ORDER BY adm_no_int ASC";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();                    
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
                            adm_no_int = Convert.ToInt32(reader["adm_no_int"]),
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool check = false;
            sections sec = (sections)section_cmb.SelectedItem;
            classes cl = (classes)class_cmb.SelectedItem;
            if(class_cmb.SelectedIndex == 0)
            {

            }
            else if (section_cmb.SelectedIndex == 0)
            {

            }
            else 
            {
                check = false;
                get_all_admissions(sec.id);
                roll_no = 1;
                foreach (admission adm in adm_list) 
                {
                    if (update_admission(adm, roll_no) > 0) 
                    {
                        roll_no++;                        
                        check = true;
                    }
                }
                if (check)
                {
                    get_adm_no(sec.id);
                    delete_last_roll_no(sec.id);
                    insert_last_roll_no(--roll_no,last_adm_no,cl.id,sec.id);
                    MessageBox.Show("Successfully Arranged Roll Nos", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else 
                {
                    MessageBox.Show("No Record Updated", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        public int update_admission(admission adm,int roll) 
        {
            int i = 0;
            try
            {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_admission SET roll_no=@roll_no, roll_no_int=@roll_no_int,updation=@updation WHERE id = @id && session_id=@session_id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm.id;
                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;                        
                        cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = roll.ToString();
                        cmd.Parameters.Add("@roll_no_int", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = Convert.ToInt32(roll);
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //========        Delete Last Roll No            =======================================
        public void delete_last_roll_no(string id)
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {

                    sections s = (sections)section_cmb.SelectedItem;

                    cmd.CommandText = "Delete from sms_roll_no where section_id=" + id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //==========       Insert last roll no        ================================
        public void insert_last_roll_no(int roll,string adm_no, string cl_id, string sec_id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        sections s = (sections)section_cmb.SelectedItem;
                        cmd.CommandText = "INSERT INTO sms_roll_no(class_id,section_id,last_roll_no,last_adm_no)Values(@class_id,@section_id,@roll_no,@last_adm_no)";

                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@last_adm_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_adm_no;
                        cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = roll;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sec_id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = cl_id;


                        con.Open();

                        cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //==============           Get sms_roll_no                ===========================
        public void get_adm_no(string id)
        {
            last_adm_no = "";
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {                        
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_roll_no";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (id == Convert.ToString(reader["section_id"].ToString()))
                                {                                    
                                    last_adm_no = Convert.ToString(reader["last_adm_no"]);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            last_adm_no = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            try
            {
                bool check = false;
                ClassesDAL dal = new ClassesDAL();
                FeesDAL feesDAL = new FeesDAL();
                int count = 0;
                count_TB.Text = count.ToString();
                count_TB.Refresh();
                //this.Dispatcher.BeginInvoke(new Action(() => this.count_TB.Text = count.ToString()), null);

                List<sms_fees_actual> submitClassesList = new List<sms_fees_actual>();
                List<classes> classes_list = dal.get_all_classes();

                List<sms_fees_actual> actual_list = new List<sms_fees_actual>();
                sms_fees_actual obj;

                obj = new sms_fees_actual();
                obj.fees_category_id = 111;
                obj.fees_category = "Admission Fee";
                actual_list.Add(obj);

                obj = new sms_fees_actual();
                obj.fees_category_id = 112;
                obj.fees_category = "Annual Fund";
                actual_list.Add(obj);

                obj = new sms_fees_actual();
                obj.fees_category_id = 113;
                obj.fees_category = "Tution Fee";
                actual_list.Add(obj);

                obj = new sms_fees_actual();
                obj.fees_category_id = 115;
                obj.fees_category = "Other";
                actual_list.Add(obj);

                obj = new sms_fees_actual();
                obj.fees_category_id = 117;
                obj.fees_category = "Security Fee";
                actual_list.Add(obj);

                obj = new sms_fees_actual();
                obj.fees_category_id = 118;
                obj.fees_category = "Exam Fee";
                actual_list.Add(obj);

                foreach (var actual in actual_list)
                {
                    check = false;
                    foreach (var classes in classes_list)
                    {

                        sms_fees_actual classes_obj = new sms_fees_actual();

                        classes_obj.class_id = Convert.ToInt32(classes.id);
                        classes_obj.class_name = classes.class_name;
                        classes_obj.fees_category_id = actual.fees_category_id;
                        classes_obj.fees_category = actual.fees_category;


                        try
                        {
                            if (actual.fees_category_id == 111)
                            {
                                classes_obj.amount = Convert.ToInt32(classes.adm_fee);
                            }
                            else if (actual.fees_category_id == 112)
                            {
                                classes_obj.amount = Convert.ToInt32(classes.reg_fee);
                            }
                            else if (actual.fees_category_id == 113)
                            {
                                classes_obj.amount = Convert.ToInt32(classes.tution_fee);
                            }
                            else if (actual.fees_category_id == 115)
                            {
                                classes_obj.amount = Convert.ToInt32(classes.other_exp);
                            }
                            else if (actual.fees_category_id == 117)
                            {
                                classes_obj.amount = Convert.ToInt32(classes.security_fee);
                            }
                            else if (actual.fees_category_id == 118)
                            {
                                classes_obj.amount = Convert.ToInt32(classes.exam_fee);
                            }
                            else
                            {
                            }
                        }
                        catch
                        {
                            classes_obj.amount = 0;
                        }


                        classes_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                        classes_obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                        classes_obj.date_time = DateTime.Now;

                        if (dal.insertFeesClasses(classes_obj) > 0)
                        {
                            check = true;
                            count++;
                            count_TB.Text = count.ToString();
                            count_TB.Refresh();
                            //this.Dispatcher.BeginInvoke(new Action(() => this.count_TB.Text = count.ToString()), null);
                        }
                        else
                        {
                            MessageBox.Show("Not Inserted in classes");
                        }
                    }



                }
                if (check)
                {
                    MessageBox.Show("Successfully inserted Classes");
                }


                // Admission----------------------------------------------------------------------------------------------------------------------------------

                AdmissionDAL admDAL = new AdmissionDAL();
                adm_list = admDAL.get_all_admissions_YN();
                List<sms_fees_actual> fees_list;
                List<sms_fees_actual> submitfeeslist = new List<sms_fees_actual>();
                sms_fees_actual actual_obj;

                foreach (var adm in adm_list)
                {
                    fees_list = new List<sms_fees_actual>();
                    fees_list = dal.getAllFeesClasses().Where(x => x.class_id == Convert.ToInt32(adm.class_id)).ToList();

                    foreach (var fee in fees_list)
                    {
                        actual_obj = new sms_fees_actual();
                        actual_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                        actual_obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                        actual_obj.date_time = DateTime.Now;
                        actual_obj.fees_category_id = fee.fees_category_id;
                        actual_obj.fees_category = fee.fees_category;
                        actual_obj.std_id = Convert.ToInt32(adm.id);
                        actual_obj.actual_amount = fee.amount;
                        try
                        {
                            if (fee.fees_category_id == 111)
                            {
                                if (adm.adm_fee != "")
                                {
                                    actual_obj.amount = Convert.ToInt32(adm.adm_fee);
                                    if (actual_obj.amount <= fee.actual_amount)
                                    {
                                        actual_obj.discount = fee.actual_amount - actual_obj.amount;
                                    }
                                    else
                                    {
                                        actual_obj.discount = 0;
                                    }
                                }
                                else
                                {
                                    actual_obj.amount = fee.amount;
                                    actual_obj.discount = 0;
                                }
                            }
                            else if (fee.fees_category_id == 112)
                            {
                                if (adm.reg_fee != "")
                                {
                                    actual_obj.amount = Convert.ToInt32(adm.reg_fee);
                                    if (actual_obj.amount <= fee.actual_amount)
                                    {
                                        actual_obj.discount = fee.actual_amount - actual_obj.amount;
                                    }
                                    else
                                    {
                                        actual_obj.discount = 0;
                                    }
                                }
                                else
                                {
                                    actual_obj.amount = fee.amount;
                                    actual_obj.discount = 0;
                                }
                            }
                            else if (fee.fees_category_id == 113)
                            {
                                if (adm.tution_fee != "")
                                {
                                    actual_obj.amount = Convert.ToInt32(adm.tution_fee);
                                    if (actual_obj.amount <= fee.actual_amount)
                                    {
                                        actual_obj.discount = fee.actual_amount - actual_obj.amount;
                                    }
                                    else
                                    {
                                        actual_obj.discount = 0;
                                    }
                                }
                                else
                                {
                                    actual_obj.amount = fee.amount;
                                    actual_obj.discount = 0;
                                }
                            }
                            else if (fee.fees_category_id == 115)
                            {
                                if (adm.other_exp != "")
                                {
                                    actual_obj.amount = Convert.ToInt32(adm.other_exp);
                                    if (actual_obj.amount <= fee.actual_amount)
                                    {
                                        actual_obj.discount = fee.actual_amount - actual_obj.amount;
                                    }
                                    else
                                    {
                                        actual_obj.discount = 0;
                                    }
                                }
                                else
                                {
                                    actual_obj.amount = fee.amount;
                                    actual_obj.discount = 0;
                                }
                            }
                            else if (fee.fees_category_id == 117)
                            {
                                if (adm.security_fee != "")
                                {
                                    actual_obj.amount = Convert.ToInt32(adm.security_fee);
                                    if (actual_obj.amount <= fee.actual_amount)
                                    {
                                        actual_obj.discount = fee.actual_amount - actual_obj.amount;
                                    }
                                    else
                                    {
                                        actual_obj.discount = 0;
                                    }
                                }
                                else
                                {
                                    actual_obj.amount = fee.amount;
                                    actual_obj.discount = 0;
                                }

                            }
                            else if (fee.fees_category_id == 118)
                            {
                                if (adm.exam_fee != "")
                                {
                                    actual_obj.amount = Convert.ToInt32(adm.exam_fee);
                                    if (actual_obj.amount <= fee.actual_amount)
                                    {
                                        actual_obj.discount = fee.actual_amount - actual_obj.amount;
                                    }
                                    else
                                    {
                                        actual_obj.discount = 0;
                                    }
                                }
                                else
                                {
                                    actual_obj.amount = fee.amount;
                                    actual_obj.discount = 0;
                                }
                            }
                            else
                            {
                            }
                        }
                        catch
                        {
                            actual_obj.amount = fee.amount;
                            actual_obj.discount = 0;
                        }
                        submitfeeslist.Add(actual_obj);
                        count++;
                        count_TB.Text = count.ToString();
                        count_TB.Refresh();
                    }
                }
                if (feesDAL.insertActualFees(submitfeeslist) > 0)
                {
                    count++;
                    count_TB.Text = count.ToString();
                    count_TB.Refresh();
                    MessageBox.Show("successfully inserted admission fees");
                }
                else
                {
                    MessageBox.Show("Error in submitted admission fees");
                }

                #region fees generated

                // Fees Generated ------------------------------------------------------------------------------------------------------------------------

                //List<fee> fee_list = get_all_fee();
                //List<sms_fees> generated_fees_list = new List<sms_fees>();
                //sms_fees generatedFee;
                //sms_fees generatedFeeObj;
                //admission adm_obj = new admission();

                //foreach (var item in fee_list)
                //{
                //    if (adm_list.Where(x => x.id == item.std_id).Count() > 0)
                //    {
                //        adm_obj = adm_list.Where(x => x.id == item.std_id).First();

                //        if (Convert.ToInt32(item.adm_fee) > 0 && generated_fees_list.Where(x => x.std_id.ToString() == item.std_id).Where(x => x.fees_category_id == 111).Count() == 0)
                //        {
                //            generatedFee = new sms_fees();
                //            if (item.month == "April")
                //            {
                //                generatedFee.month = 4;
                //                generatedFee.month_name = "April";
                //            }
                //            else if (item.month == "May")
                //            {
                //                generatedFee.month = 5;
                //                generatedFee.month_name = "May";
                //            }
                //            else if (item.month == "June")
                //            {
                //                generatedFee.month = 6;
                //                generatedFee.month_name = "June";
                //            }
                //            else if (item.month == "July")
                //            {
                //                generatedFee.month = 7;
                //                generatedFee.month_name = "July";
                //            }
                //            else if (item.month == "August")
                //            {
                //                generatedFee.month = 8;
                //                generatedFee.month_name = "August";
                //            }
                //            else if (item.month == "September")
                //            {
                //                generatedFee.month = 9;
                //                generatedFee.month_name = "September";
                //            }
                //            else { }

                //            generatedFee.std_id = Convert.ToInt32(item.std_id);
                //            generatedFee.class_id = Convert.ToInt32(adm_obj.class_id);
                //            generatedFee.class_name = adm_obj.class_name;
                //            generatedFee.section_id = Convert.ToInt32(adm_obj.section_id);
                //            generatedFee.section_name = adm_obj.section_name;

                //            generatedFee.year = 2016;
                //            generatedFee.date = item.date_time;
                //            generatedFee.due_date = item.date_time.AddDays(20);
                //            generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                //            generatedFee.created_by = item.created_by;
                //            generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                //            generatedFee.date_time = DateTime.Now;
                //            generatedFee.fees_category_id = 111;
                //            generatedFee.fees_category = "Admission Fee";
                //            generatedFee.amount = Convert.ToInt32(item.adm_fee);
                //            generatedFee.rem_amount = Convert.ToInt32(item.adm_fee);
                //            generatedFee.discount = 0;

                //            count++;
                //            count_TB.Text = count.ToString();
                //            count_TB.Refresh();
                //            generated_fees_list.Add(generatedFee);

                //        }
                //        if (Convert.ToInt32(item.reg_fee) > 0 && generated_fees_list.Where(x => x.std_id.ToString() == item.std_id).Where(x => x.fees_category_id == 112).Count() == 0)
                //        {
                //            generatedFee = new sms_fees();
                //            if (item.month == "April")
                //            {
                //                generatedFee.month = 4;
                //                generatedFee.month_name = "April";
                //            }
                //            else if (item.month == "May")
                //            {
                //                generatedFee.month = 5;
                //                generatedFee.month_name = "May";
                //            }
                //            else if (item.month == "June")
                //            {
                //                generatedFee.month = 6;
                //                generatedFee.month_name = "June";
                //            }
                //            else if (item.month == "July")
                //            {
                //                generatedFee.month = 7;
                //                generatedFee.month_name = "July";
                //            }
                //            else if (item.month == "August")
                //            {
                //                generatedFee.month = 8;
                //                generatedFee.month_name = "August";
                //            }
                //            else if (item.month == "September")
                //            {
                //                generatedFee.month = 9;
                //                generatedFee.month_name = "September";
                //            }
                //            else { }

                //            generatedFee.std_id = Convert.ToInt32(item.std_id);
                //            generatedFee.class_id = Convert.ToInt32(adm_obj.class_id);
                //            generatedFee.class_name = adm_obj.class_name;
                //            generatedFee.section_id = Convert.ToInt32(adm_obj.section_id);
                //            generatedFee.section_name = adm_obj.section_name;

                //            generatedFee.year = 2016;
                //            generatedFee.date = item.date_time;
                //            generatedFee.due_date = item.date_time.AddDays(20);
                //            generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                //            generatedFee.created_by = item.created_by;
                //            generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                //            generatedFee.date_time = DateTime.Now;
                //            generatedFee.fees_category_id = 112;
                //            generatedFee.fees_category = "Annual Fund";
                //            generatedFee.amount = Convert.ToInt32(item.reg_fee);
                //            generatedFee.rem_amount = Convert.ToInt32(item.reg_fee);
                //            generatedFee.discount = 0;

                //            count++;
                //            count_TB.Text = count.ToString();
                //            count_TB.Refresh();
                //            generated_fees_list.Add(generatedFee);
                //        }
                //        if (Convert.ToInt32(item.tution_fee) > 0)
                //        {
                //            generatedFee = new sms_fees();
                //            if (item.month == "April")
                //            {
                //                generatedFee.month = 4;
                //                generatedFee.month_name = "April";
                //            }
                //            else if (item.month == "May")
                //            {
                //                generatedFee.month = 5;
                //                generatedFee.month_name = "May";
                //            }
                //            else if (item.month == "June")
                //            {
                //                generatedFee.month = 6;
                //                generatedFee.month_name = "June";
                //            }
                //            else if (item.month == "July")
                //            {
                //                generatedFee.month = 7;
                //                generatedFee.month_name = "July";
                //            }
                //            else if (item.month == "August")
                //            {
                //                generatedFee.month = 8;
                //                generatedFee.month_name = "August";
                //            }
                //            else if (item.month == "September")
                //            {
                //                generatedFee.month = 9;
                //                generatedFee.month_name = "September";
                //            }
                //            else { }

                //            generatedFee.std_id = Convert.ToInt32(item.std_id);
                //            generatedFee.class_id = Convert.ToInt32(adm_obj.class_id);
                //            generatedFee.class_name = adm_obj.class_name;
                //            generatedFee.section_id = Convert.ToInt32(adm_obj.section_id);
                //            generatedFee.section_name = adm_obj.section_name;

                //            generatedFee.year = 2016;
                //            generatedFee.date = item.date_time;
                //            generatedFee.due_date = item.date_time.AddDays(20);
                //            generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                //            generatedFee.created_by = item.created_by;
                //            generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                //            generatedFee.date_time = DateTime.Now;
                //            generatedFee.fees_category_id = 113;
                //            generatedFee.fees_category = "Tution Fee";
                //            generatedFee.amount = Convert.ToInt32(item.tution_fee);
                //            generatedFee.rem_amount = Convert.ToInt32(item.tution_fee);
                //            generatedFee.discount = 0;

                //            count++;
                //            count_TB.Text = count.ToString();
                //            count_TB.Refresh();
                //            generated_fees_list.Add(generatedFee);

                //        }
                //        if (Convert.ToInt32(item.other_expenses) > 0)
                //        {
                //            generatedFee = new sms_fees();
                //            if (item.month == "April")
                //            {
                //                generatedFee.month = 4;
                //                generatedFee.month_name = "April";
                //            }
                //            else if (item.month == "May")
                //            {
                //                generatedFee.month = 5;
                //                generatedFee.month_name = "May";
                //            }
                //            else if (item.month == "June")
                //            {
                //                generatedFee.month = 6;
                //                generatedFee.month_name = "June";
                //            }
                //            else if (item.month == "July")
                //            {
                //                generatedFee.month = 7;
                //                generatedFee.month_name = "July";
                //            }
                //            else if (item.month == "August")
                //            {
                //                generatedFee.month = 8;
                //                generatedFee.month_name = "August";
                //            }
                //            else if (item.month == "September")
                //            {
                //                generatedFee.month = 9;
                //                generatedFee.month_name = "September";
                //            }
                //            else { }

                //            generatedFee.std_id = Convert.ToInt32(item.std_id);
                //            generatedFee.class_id = Convert.ToInt32(adm_obj.class_id);
                //            generatedFee.class_name = adm_obj.class_name;
                //            generatedFee.section_id = Convert.ToInt32(adm_obj.section_id);
                //            generatedFee.section_name = adm_obj.section_name;

                //            generatedFee.year = 2016;
                //            generatedFee.date = item.date_time;
                //            generatedFee.due_date = item.date_time.AddDays(20);
                //            generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                //            generatedFee.created_by = item.created_by;
                //            generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                //            generatedFee.date_time = DateTime.Now;
                //            generatedFee.fees_category_id = 115;
                //            generatedFee.fees_category = "Other";
                //            generatedFee.amount = Convert.ToInt32(item.other_expenses);
                //            generatedFee.rem_amount = Convert.ToInt32(item.other_expenses);
                //            generatedFee.discount = 0;

                //            count++;
                //            count_TB.Text = count.ToString();
                //            count_TB.Refresh();
                //            generated_fees_list.Add(generatedFee);
                //        }
                //        if (Convert.ToInt32(item.security_fee) > 0 && generated_fees_list.Where(x => x.std_id.ToString() == item.std_id).Where(x => x.fees_category_id == 117).Count() == 0)
                //        {
                //            generatedFee = new sms_fees();
                //            if (item.month == "April")
                //            {
                //                generatedFee.month = 4;
                //                generatedFee.month_name = "April";
                //            }
                //            else if (item.month == "May")
                //            {
                //                generatedFee.month = 5;
                //                generatedFee.month_name = "May";
                //            }
                //            else if (item.month == "June")
                //            {
                //                generatedFee.month = 6;
                //                generatedFee.month_name = "June";
                //            }
                //            else if (item.month == "July")
                //            {
                //                generatedFee.month = 7;
                //                generatedFee.month_name = "July";
                //            }
                //            else if (item.month == "August")
                //            {
                //                generatedFee.month = 8;
                //                generatedFee.month_name = "August";
                //            }
                //            else if (item.month == "September")
                //            {
                //                generatedFee.month = 9;
                //                generatedFee.month_name = "September";
                //            }
                //            else { }

                //            generatedFee.std_id = Convert.ToInt32(item.std_id);
                //            generatedFee.class_id = Convert.ToInt32(adm_obj.class_id);
                //            generatedFee.class_name = adm_obj.class_name;
                //            generatedFee.section_id = Convert.ToInt32(adm_obj.section_id);
                //            generatedFee.section_name = adm_obj.section_name;

                //            generatedFee.year = 2016;
                //            generatedFee.date = item.date_time;
                //            generatedFee.due_date = item.date_time.AddDays(20);
                //            generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                //            generatedFee.created_by = item.created_by;
                //            generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                //            generatedFee.date_time = DateTime.Now;
                //            generatedFee.fees_category_id = 117;
                //            generatedFee.fees_category = "Security Fee";
                //            generatedFee.amount = Convert.ToInt32(item.security_fee);
                //            generatedFee.rem_amount = Convert.ToInt32(item.security_fee);
                //            generatedFee.discount = 0;

                //            count++;
                //            count_TB.Text = count.ToString();
                //            count_TB.Refresh();
                //            generated_fees_list.Add(generatedFee);
                //        }
                //        if (Convert.ToInt32(item.exam_fee) > 0)
                //        {
                //            generatedFee = new sms_fees();
                //            if (item.month == "April")
                //            {
                //                generatedFee.month = 4;
                //                generatedFee.month_name = "April";
                //            }
                //            else if (item.month == "May")
                //            {
                //                generatedFee.month = 5;
                //                generatedFee.month_name = "May";
                //            }
                //            else if (item.month == "June")
                //            {
                //                generatedFee.month = 6;
                //                generatedFee.month_name = "June";
                //            }
                //            else if (item.month == "July")
                //            {
                //                generatedFee.month = 7;
                //                generatedFee.month_name = "July";
                //            }
                //            else if (item.month == "August")
                //            {
                //                generatedFee.month = 8;
                //                generatedFee.month_name = "August";
                //            }
                //            else if (item.month == "September")
                //            {
                //                generatedFee.month = 9;
                //                generatedFee.month_name = "September";
                //            }
                //            else { }

                //            generatedFee.std_id = Convert.ToInt32(item.std_id);
                //            generatedFee.class_id = Convert.ToInt32(adm_obj.class_id);
                //            generatedFee.class_name = adm_obj.class_name;
                //            generatedFee.section_id = Convert.ToInt32(adm_obj.section_id);
                //            generatedFee.section_name = adm_obj.section_name;

                //            generatedFee.year = 2016;
                //            generatedFee.date = item.date_time;
                //            generatedFee.due_date = item.date_time.AddDays(20);
                //            generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                //            generatedFee.created_by = item.created_by;
                //            generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                //            generatedFee.date_time = DateTime.Now;
                //            generatedFee.fees_category_id = 118;
                //            generatedFee.fees_category = "Exam Fee";
                //            generatedFee.amount = Convert.ToInt32(item.exam_fee);
                //            generatedFee.rem_amount = Convert.ToInt32(item.exam_fee);
                //            generatedFee.discount = 0;

                //            count++;
                //            count_TB.Text = count.ToString();
                //            count_TB.Refresh();
                //            generated_fees_list.Add(generatedFee);
                //        }
                //        if (Convert.ToInt32(item.fine_fee) > 0)
                //        {
                //            generatedFee = new sms_fees();
                //            if (item.month == "April")
                //            {
                //                generatedFee.month = 4;
                //                generatedFee.month_name = "April";
                //            }
                //            else if (item.month == "May")
                //            {
                //                generatedFee.month = 5;
                //                generatedFee.month_name = "May";
                //            }
                //            else if (item.month == "June")
                //            {
                //                generatedFee.month = 6;
                //                generatedFee.month_name = "June";
                //            }
                //            else if (item.month == "July")
                //            {
                //                generatedFee.month = 7;
                //                generatedFee.month_name = "July";
                //            }
                //            else if (item.month == "August")
                //            {
                //                generatedFee.month = 8;
                //                generatedFee.month_name = "August";
                //            }
                //            else if (item.month == "September")
                //            {
                //                generatedFee.month = 9;
                //                generatedFee.month_name = "September";
                //            }
                //            else { }

                //            generatedFee.std_id = Convert.ToInt32(item.std_id);
                //            generatedFee.class_id = Convert.ToInt32(adm_obj.class_id);
                //            generatedFee.class_name = adm_obj.class_name;
                //            generatedFee.section_id = Convert.ToInt32(adm_obj.section_id);
                //            generatedFee.section_name = adm_obj.section_name;

                //            generatedFee.year = 2016;
                //            generatedFee.date = item.date_time;
                //            generatedFee.due_date = item.date_time.AddDays(20);
                //            generatedFee.session_id = Convert.ToInt32(MainWindow.session.id);

                //            generatedFee.created_by = item.created_by;
                //            generatedFee.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                //            generatedFee.date_time = DateTime.Now;
                //            generatedFee.fees_category_id = 119;
                //            generatedFee.fees_category = "Fine";
                //            generatedFee.amount = Convert.ToInt32(item.fine_fee);
                //            generatedFee.rem_amount = Convert.ToInt32(item.fine_fee);
                //            generatedFee.discount = 0;

                //            count++;
                //            count_TB.Text = count.ToString();
                //            count_TB.Refresh();
                //            generated_fees_list.Add(generatedFee);
                //        }
                //    }
                //    else 
                //    {
                //        MessageBox.Show("No entry found in sms_admission  std_id="+item.std_id);
                //    }
                //}

                
                
                
                
                
                
                
                
                
                
                //if (feesDAL.insertFeesGenerated(generated_fees_list) > 0)
                //{
                //    count_TB.Refresh();
                //    MessageBox.Show("successfully Generated Fees");

                //}
                //else
                //{
                //    MessageBox.Show("Error in generation fees");
                //}


                //// Paid-----------------------------------------------------------------------------------------------------------------------------------------------


                //List<sms_fees> fees_generated_list;
                //List<fee> fee_list_paid = get_All_paid_fee();
                //sms_fees feeObj;
                //sms_fees fees_generated_obj = new sms_fees();
                //int total = 0;

                //foreach (var item in fee_list_paid.Select(x => x.receipt_no).Distinct())
                //{
                //    total = 0;
                //    if (item == "4174")
                //    {
                //    }
                //    foreach (var f in fee_list_paid.Where(x => x.receipt_no == item))
                //    {
                //        total = total + Convert.ToInt32(f.adm_fee) + Convert.ToInt32(f.reg_fee) + Convert.ToInt32(f.tution_fee) + Convert.ToInt32(f.security_fee) + Convert.ToInt32(f.exam_fee) + Convert.ToInt32(f.other_expenses) + Convert.ToInt32(f.fine_fee);
                //    }

                //    feesListToBePaid = new List<sms_fees>();
                //    foreach (var paid_fee in fee_list_paid.Where(x => x.receipt_no == item))
                //    {                        
                //        try
                //        {
                //            admission admObj = adm_list.Where(x => x.id == paid_fee.std_id).First();
                //            fees_generated_list = feesDAL.get_all_fees_generated_by_stdID(Convert.ToInt32(admObj.id));
                //            if (fees_generated_list.Count > 0)
                //            {
                //                #region fees
                                

                //                if (Convert.ToInt32(paid_fee.adm_fee) > 0)
                //                {
                //                    fees_generated_obj = fees_generated_list.Where(x => x.std_id.ToString() == admObj.id).Where(x => x.fees_category_id == 111).First();
                //                    feeObj = new sms_fees();
                //                    feeObj.std_id = Convert.ToInt32(paid_fee.std_id);
                //                    feeObj.class_id = Convert.ToInt32(admObj.class_id);
                //                    feeObj.class_name = admObj.class_name;
                //                    feeObj.section_id = Convert.ToInt32(admObj.section_id);
                //                    feeObj.section_name = admObj.section_name;

                //                    feeObj.date = paid_fee.date_time;
                //                    feeObj.date_time = DateTime.Now;
                //                    feeObj.receipt_no = Convert.ToInt32(paid_fee.receipt_no);
                //                    feeObj.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + feeObj.receipt_no.ToString("D6");
                //                    feeObj.fees_collection_place_id = 22;
                //                    feeObj.fees_collection_place = "Cash In Hand";
                //                    feeObj.total_amount = total;
                //                    feeObj.total_paid = total;
                //                    feeObj.amount_in_words = feesDAL.NumberToWords(total);
                //                    feeObj.total_remaining = 0;
                //                    feeObj.year = 2016;
                //                    feeObj.emp_id = 0;
                //                    feeObj.session_id = 3;
                //                    feeObj.created_by = paid_fee.created_by;
                //                    feeObj.id = fees_generated_obj.id;
                //                    feeObj.amount_paid = Convert.ToInt32(paid_fee.adm_fee);
                //                    feeObj.amount = fees_generated_obj.rem_amount;
                //                    feeObj.rem_amount = fees_generated_obj.rem_amount;
                //                    feeObj.fees_category_id = 111;
                //                    feeObj.fees_category = "Admission Fee";
                //                    feeObj.month_name = fees_generated_obj.month_name;
                //                    feeObj.month = fees_generated_obj.month;

                //                    count++;
                //                    count_TB.Text = count.ToString();
                //                    count_TB.Refresh();
                //                    feesListToBePaid.Add(feeObj);
                //                }
                //                if (Convert.ToInt32(paid_fee.reg_fee) > 0)
                //                {
                //                    fees_generated_obj = fees_generated_list.Where(x => x.std_id.ToString() == admObj.id).Where(x => x.fees_category_id == 112).First();
                //                    feeObj = new sms_fees();
                //                    feeObj.std_id = Convert.ToInt32(paid_fee.std_id);
                //                    feeObj.class_id = Convert.ToInt32(admObj.class_id);
                //                    feeObj.class_name = admObj.class_name;
                //                    feeObj.section_id = Convert.ToInt32(admObj.section_id);
                //                    feeObj.section_name = admObj.section_name;

                //                    feeObj.date = paid_fee.date_time;
                //                    feeObj.date_time = DateTime.Now;
                //                    feeObj.receipt_no = Convert.ToInt32(paid_fee.receipt_no);
                //                    feeObj.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + feeObj.receipt_no.ToString("D6");
                //                    feeObj.fees_collection_place_id = 22;
                //                    feeObj.fees_collection_place = "Cash In Hand";
                //                    feeObj.total_amount = total;
                //                    feeObj.total_paid = total;
                //                    feeObj.amount_in_words = feesDAL.NumberToWords(total);
                //                    feeObj.total_remaining = 0;
                //                    feeObj.year = 2016;
                //                    feeObj.emp_id = 0;
                //                    feeObj.session_id = 3;
                //                    feeObj.created_by = paid_fee.created_by;
                //                    feeObj.id = fees_generated_obj.id;
                //                    feeObj.amount_paid = Convert.ToInt32(paid_fee.reg_fee);
                //                    feeObj.amount = fees_generated_obj.rem_amount;
                //                    feeObj.rem_amount = fees_generated_obj.rem_amount;
                //                    feeObj.fees_category_id = 112;
                //                    feeObj.fees_category = "Annual Fund";
                //                    feeObj.month_name = fees_generated_obj.month_name;
                //                    feeObj.month = fees_generated_obj.month;

                //                    count++;
                //                    count_TB.Text = count.ToString();
                //                    count_TB.Refresh();
                //                    feesListToBePaid.Add(feeObj);
                //                }
                //                if (Convert.ToInt32(paid_fee.tution_fee) > 0)
                //                {
                //                    fees_generated_obj = fees_generated_list.Where(x => x.std_id.ToString() == admObj.id).Where(x => x.fees_category_id == 113).Where(x => x.month_name == paid_fee.month).First();
                //                    feeObj = new sms_fees();
                //                    feeObj.std_id = Convert.ToInt32(paid_fee.std_id);
                //                    feeObj.class_id = Convert.ToInt32(admObj.class_id);
                //                    feeObj.class_name = admObj.class_name;
                //                    feeObj.section_id = Convert.ToInt32(admObj.section_id);
                //                    feeObj.section_name = admObj.section_name;

                //                    feeObj.date = paid_fee.date_time;
                //                    feeObj.date_time = DateTime.Now;
                //                    feeObj.receipt_no = Convert.ToInt32(paid_fee.receipt_no);
                //                    feeObj.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + feeObj.receipt_no.ToString("D6");
                //                    feeObj.fees_collection_place_id = 22;
                //                    feeObj.fees_collection_place = "Cash In Hand";
                //                    feeObj.total_amount = total;
                //                    feeObj.total_paid = total;
                //                    feeObj.amount_in_words = feesDAL.NumberToWords(total);
                //                    feeObj.total_remaining = 0;
                //                    feeObj.year = 2016;
                //                    feeObj.emp_id = 0;
                //                    feeObj.session_id = 3;
                //                    feeObj.created_by = paid_fee.created_by;
                //                    feeObj.id = fees_generated_obj.id;
                //                    feeObj.amount_paid = Convert.ToInt32(paid_fee.tution_fee);
                //                    feeObj.amount = fees_generated_obj.rem_amount;
                //                    feeObj.rem_amount = fees_generated_obj.rem_amount;
                //                    feeObj.fees_category_id = 113;
                //                    feeObj.fees_category = "Tution Fee";
                //                    feeObj.month_name = fees_generated_obj.month_name;
                //                    feeObj.month = fees_generated_obj.month;

                //                    count++;
                //                    count_TB.Text = count.ToString();
                //                    count_TB.Refresh();
                //                    feesListToBePaid.Add(feeObj);
                //                }
                //                if (Convert.ToInt32(paid_fee.other_expenses) > 0)
                //                {
                //                    fees_generated_obj = fees_generated_list.Where(x => x.std_id.ToString() == admObj.id).Where(x => x.fees_category_id == 115).Where(x => x.month_name == paid_fee.month).First();
                //                    feeObj = new sms_fees();
                //                    feeObj.std_id = Convert.ToInt32(paid_fee.std_id);
                //                    feeObj.class_id = Convert.ToInt32(admObj.class_id);
                //                    feeObj.class_name = admObj.class_name;
                //                    feeObj.section_id = Convert.ToInt32(admObj.section_id);
                //                    feeObj.section_name = admObj.section_name;

                //                    feeObj.date = paid_fee.date_time;
                //                    feeObj.date_time = DateTime.Now;
                //                    feeObj.receipt_no = Convert.ToInt32(paid_fee.receipt_no);
                //                    feeObj.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + feeObj.receipt_no.ToString("D6");
                //                    feeObj.fees_collection_place_id = 22;
                //                    feeObj.fees_collection_place = "Cash In Hand";
                //                    feeObj.total_amount = total;
                //                    feeObj.total_paid = total;
                //                    feeObj.amount_in_words = feesDAL.NumberToWords(total);
                //                    feeObj.total_remaining = 0;
                //                    feeObj.year = 2016;
                //                    feeObj.emp_id = 0;
                //                    feeObj.session_id = 3;
                //                    feeObj.created_by = paid_fee.created_by;
                //                    feeObj.id = fees_generated_obj.id;
                //                    feeObj.amount_paid = Convert.ToInt32(paid_fee.other_expenses);
                //                    feeObj.amount = fees_generated_obj.rem_amount;
                //                    feeObj.rem_amount = fees_generated_obj.rem_amount;
                //                    feeObj.fees_category_id = 115;
                //                    feeObj.fees_category = "Other";
                //                    feeObj.month_name = fees_generated_obj.month_name;
                //                    feeObj.month = fees_generated_obj.month;

                //                    count++;
                //                    count_TB.Text = count.ToString();
                //                    count_TB.Refresh();
                //                    feesListToBePaid.Add(feeObj);
                //                }
                //                if (Convert.ToInt32(paid_fee.security_fee) > 0)
                //                {
                //                    fees_generated_obj = fees_generated_list.Where(x => x.std_id.ToString() == admObj.id).Where(x => x.fees_category_id == 117).First();
                //                    feeObj = new sms_fees();
                //                    feeObj.std_id = Convert.ToInt32(paid_fee.std_id);
                //                    feeObj.class_id = Convert.ToInt32(admObj.class_id);
                //                    feeObj.class_name = admObj.class_name;
                //                    feeObj.section_id = Convert.ToInt32(admObj.section_id);
                //                    feeObj.section_name = admObj.section_name;

                //                    feeObj.date = paid_fee.date_time;
                //                    feeObj.date_time = DateTime.Now;
                //                    feeObj.receipt_no = Convert.ToInt32(paid_fee.receipt_no);
                //                    feeObj.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + feeObj.receipt_no.ToString("D6");
                //                    feeObj.fees_collection_place_id = 22;
                //                    feeObj.fees_collection_place = "Cash In Hand";
                //                    feeObj.total_amount = total;
                //                    feeObj.total_paid = total;
                //                    feeObj.amount_in_words = feesDAL.NumberToWords(total);
                //                    feeObj.total_remaining = 0;
                //                    feeObj.year = 2016;
                //                    feeObj.emp_id = 0;
                //                    feeObj.session_id = 3;
                //                    feeObj.created_by = paid_fee.created_by;
                //                    feeObj.id = fees_generated_obj.id;
                //                    feeObj.amount_paid = Convert.ToInt32(paid_fee.security_fee);
                //                    feeObj.amount = fees_generated_obj.rem_amount;
                //                    feeObj.rem_amount = fees_generated_obj.rem_amount;
                //                    feeObj.fees_category_id = 117;
                //                    feeObj.fees_category = "Security Fee";
                //                    feeObj.month_name = fees_generated_obj.month_name;
                //                    feeObj.month = fees_generated_obj.month;

                //                    count++;
                //                    count_TB.Text = count.ToString();
                //                    count_TB.Refresh();
                //                    feesListToBePaid.Add(feeObj);
                //                }
                //                if (Convert.ToInt32(paid_fee.exam_fee) > 0)
                //                {
                //                    fees_generated_obj = fees_generated_list.Where(x => x.std_id.ToString() == admObj.id).Where(x => x.fees_category_id == 118).First();
                //                    feeObj = new sms_fees();
                //                    feeObj.std_id = Convert.ToInt32(paid_fee.std_id);
                //                    feeObj.class_id = Convert.ToInt32(admObj.class_id);
                //                    feeObj.class_name = admObj.class_name;
                //                    feeObj.section_id = Convert.ToInt32(admObj.section_id);
                //                    feeObj.section_name = admObj.section_name;

                //                    feeObj.date = paid_fee.date_time;
                //                    feeObj.date_time = DateTime.Now;
                //                    feeObj.receipt_no = Convert.ToInt32(paid_fee.receipt_no);
                //                    feeObj.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + feeObj.receipt_no.ToString("D6");
                //                    feeObj.fees_collection_place_id = 22;
                //                    feeObj.fees_collection_place = "Cash In Hand";
                //                    feeObj.total_amount = total;
                //                    feeObj.total_paid = total;
                //                    feeObj.amount_in_words = feesDAL.NumberToWords(total);
                //                    feeObj.total_remaining = 0;
                //                    feeObj.year = 2016;
                //                    feeObj.emp_id = 0;
                //                    feeObj.session_id = 3;
                //                    feeObj.created_by = paid_fee.created_by;
                //                    feeObj.id = fees_generated_obj.id;
                //                    feeObj.amount_paid = Convert.ToInt32(paid_fee.exam_fee);
                //                    feeObj.amount = fees_generated_obj.rem_amount;
                //                    feeObj.rem_amount = fees_generated_obj.rem_amount;
                //                    feeObj.fees_category_id = 118;
                //                    feeObj.fees_category = "Exam Fee";
                //                    feeObj.month_name = fees_generated_obj.month_name;
                //                    feeObj.month = fees_generated_obj.month;

                //                    count++;
                //                    count_TB.Text = count.ToString();
                //                    count_TB.Refresh();
                //                    feesListToBePaid.Add(feeObj);
                //                }
                //                if (Convert.ToInt32(paid_fee.fine_fee) > 0)
                //                {
                //                    fees_generated_obj = fees_generated_list.Where(x => x.std_id.ToString() == admObj.id).Where(x => x.fees_category_id == 119).Where(x => x.month_name == paid_fee.month).First();
                //                    feeObj = new sms_fees();
                //                    feeObj.std_id = Convert.ToInt32(paid_fee.std_id);
                //                    feeObj.class_id = Convert.ToInt32(admObj.class_id);
                //                    feeObj.class_name = admObj.class_name;
                //                    feeObj.section_id = Convert.ToInt32(admObj.section_id);
                //                    feeObj.section_name = admObj.section_name;

                //                    feeObj.date = paid_fee.date_time;
                //                    feeObj.date_time = DateTime.Now;
                //                    feeObj.receipt_no = Convert.ToInt32(paid_fee.receipt_no);
                //                    feeObj.receipt_no_full = "CRV-" + DateTime.Now.ToString("yy") + "-" + feeObj.receipt_no.ToString("D6");
                //                    feeObj.fees_collection_place_id = 22;
                //                    feeObj.fees_collection_place = "Cash In Hand";
                //                    feeObj.total_amount = total;
                //                    feeObj.total_paid = total;
                //                    feeObj.amount_in_words = feesDAL.NumberToWords(total);
                //                    feeObj.total_remaining = 0;
                //                    feeObj.year = 2016;
                //                    feeObj.emp_id = 0;
                //                    feeObj.session_id = 3;
                //                    feeObj.created_by = paid_fee.created_by;
                //                    feeObj.id = fees_generated_obj.id;
                //                    feeObj.amount_paid = Convert.ToInt32(paid_fee.fine_fee);
                //                    feeObj.amount = fees_generated_obj.rem_amount;
                //                    feeObj.rem_amount = fees_generated_obj.rem_amount;
                //                    feeObj.fees_category_id = 119;
                //                    feeObj.fees_category = "Fine Fee";
                //                    feeObj.month_name = fees_generated_obj.month_name;
                //                    feeObj.month = fees_generated_obj.month;

                //                    count++;
                //                    count_TB.Text = count.ToString();
                //                    count_TB.Refresh();
                //                    feesListToBePaid.Add(feeObj);
                //                }

                //                #endregion
                //            }
                //            else
                //            {
                //                MessageBox.Show("No found in sms_fees_generated   std_id=" + paid_fee.std_id);
                //            }                            
                //        }
                //        catch (Exception ex)
                //        {
                //            MessageBox.Show(ex.Message + "receipt_no=   " + item);
                //        }
                //    }
                //    if (feesListToBePaid.Count > 0)
                //    {
                //        string std_id = feesListToBePaid.First().std_id.ToString();
                //        admission adm = new admission();
                //        if (adm_list.Where(x => x.id == std_id).Count() > 0)
                //        {
                //            adm = adm_list.Where(x => x.id == std_id).First();
                //            List<sms_fees> feesVoucherHistoryList = getFeeVoucherHistoryList(feesListToBePaid.First(), adm);
                //            check = false;

                //            if (feesDAL.submitFees(feesListToBePaid, feesListToBePaid.First().receipt_no, feesVoucherHistoryList, fillVoucherObject(adm), fillVoucherEntryList(adm)) > 0)
                //            {
                //                check = true;
                //                count++;
                //                count_TB.Text = count.ToString();
                //                count_TB.Refresh();
                //            }
                //        }
                //    }

                //}
                //if (check)
                //{
                //    MessageBox.Show("successfully Paid Fees");
                //}
                //else
                //{
                //    MessageBox.Show("Error in PAID fees");
                //}

                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }        
        }



        public List<fee> get_All_paid_fee()
        {           
            List<fee> fees_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where (month=@month1 || month=@month2 || month=@month3 || month=@month4 || month=@month5 || month=@month6) && session_id=3";
                        cmd.Parameters.Add("@month1", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "April";
                        cmd.Parameters.Add("@month2", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "May";
                        cmd.Parameters.Add("@month3", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "June";
                        cmd.Parameters.Add("@month4", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "July";
                        cmd.Parameters.Add("@month5", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "August";
                        cmd.Parameters.Add("@month6", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "September";

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
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                fine_fee = Convert.ToString(reader["fine_fee_paid"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                            };
                            fees_list.Add(paid_fee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return fees_list;
        }
        public List<fee> get_all_fee() 
        {
            List<fee> fee_list = new List<fee>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_fee where (month=@month1 || month=@month2 || month=@month3 || month=@month4 || month=@month5 || month=@month6) && isActive='Y' && session_id=" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        cmd.Parameters.Add("@month1", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "April";
                        cmd.Parameters.Add("@month2", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "May";
                        cmd.Parameters.Add("@month3", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "June";
                        cmd.Parameters.Add("@month4", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "July";
                        cmd.Parameters.Add("@month5", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "August";
                        cmd.Parameters.Add("@month6", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "September";
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee f = new fee()
                            {                                
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                fine_fee = Convert.ToString(reader["fine_fee"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
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
            return fee_list;
        }

        public sms_voucher fillVoucherObject(admission admObj)
        {
            sms_voucher sms_voucher_obj = new sms_voucher();
            try
            {               
                sms_voucher_obj.voucher_type = "CRV";
                sms_voucher_obj.voucher_type_id = 4;
                sms_voucher_obj.is_posted = "Y";
                sms_voucher_obj.voucher_date = feesListToBePaid.First().date;
                sms_voucher_obj.voucher_description = admObj.std_name + "[" + admObj.adm_no + "]";
                sms_voucher_obj.voucher_no_int = feesListToBePaid.First().receipt_no;
                sms_voucher_obj.voucher_no = feesListToBePaid.First().receipt_no_full;
                sms_voucher_obj.amount = feesListToBePaid.First().total_paid;

                sms_voucher_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                sms_voucher_obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
                sms_voucher_obj.date_time = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return sms_voucher_obj;
        }

        public List<sms_voucher_entries> fillVoucherEntryList(admission admObj)
        {
            List<sms_voucher_entries> list = new List<sms_voucher_entries>();
            sms_voucher_entries sms_voucher_entries_obj;

            foreach (var item in feesListToBePaid)
            {
                //debit
                sms_voucher_entries_obj = new sms_voucher_entries();
                sms_voucher_entries_obj.account_head_id = 49;
                sms_voucher_entries_obj.account_head = "Fee";

                sms_voucher_entries_obj.account_detail_id = item.fees_category_id;
                sms_voucher_entries_obj.account_detail = item.fees_category;

                sms_voucher_entries_obj.voucher_no = item.receipt_no_full;
                sms_voucher_entries_obj.voucher_no_int = item.receipt_no;

                sms_voucher_entries_obj.voucher_type = "CRV";
                sms_voucher_entries_obj.voucher_type_id = 4;

                sms_voucher_entries_obj.description = admObj.std_name + "[" + admObj.adm_no + "]- " + item.month_name + "-" + item.fees_category;
                sms_voucher_entries_obj.credit = item.amount_paid;
                sms_voucher_entries_obj.debit = 0;

                sms_voucher_entries_obj.created_by = item.created_by;
                sms_voucher_entries_obj.emp_id = item.emp_id;
                sms_voucher_entries_obj.date_time = item.date_time;

                list.Add(sms_voucher_entries_obj);

                //credit
                sms_voucher_entries_obj = new sms_voucher_entries();
                sms_voucher_entries_obj.account_head_id = 15;
                sms_voucher_entries_obj.account_head = "Current Assets";

                sms_voucher_entries_obj.account_detail_id = item.fees_collection_place_id;
                sms_voucher_entries_obj.account_detail = item.fees_collection_place;

                sms_voucher_entries_obj.voucher_no = item.receipt_no_full;
                sms_voucher_entries_obj.voucher_no_int = item.receipt_no;

                sms_voucher_entries_obj.voucher_type = "CRV";
                sms_voucher_entries_obj.voucher_type_id = 4;

                sms_voucher_entries_obj.description = admObj.std_name + "[" + admObj.adm_no + "]- " + item.month_name + "-" + item.fees_category;
                sms_voucher_entries_obj.credit = 0;
                sms_voucher_entries_obj.debit = item.amount_paid;

                sms_voucher_entries_obj.created_by = item.created_by;
                sms_voucher_entries_obj.emp_id = item.emp_id;
                sms_voucher_entries_obj.date_time = item.date_time;

                list.Add(sms_voucher_entries_obj);
            }

            return list;
        }
        public List<sms_fees> getFeeVoucherHistoryList(sms_fees obj, admission adm)
        {
            List<sms_fees> historyList = new List<sms_fees>();

            foreach (var item in feesListToBePaid)
            {
                item.std_name = adm.std_name;
                item.father_name = adm.father_name;
                item.adm_no = adm.adm_no;
                item.class_name = adm.class_name;
                item.section_name = adm.section_name;

                item.receipt_no = obj.receipt_no;
                item.receipt_no_full = obj.receipt_no_full;
                item.date = obj.date;
                item.total_amount = obj.total_amount;
                item.total_paid = obj.total_paid;
                item.total_remaining = obj.total_remaining;
                item.amount_in_words = obj.amount_in_words;
                item.discount = 0;
                item.wave_off = 0;
                item.fees_collection_place = obj.fees_collection_place;
                item.emp_id = obj.emp_id;

                historyList.Add(item);
            }

            return historyList;
        }

        //==========================================   Debug Accounts  ===================================================================================

        private void synch_btn_Click(object sender, RoutedEventArgs e)
        {
            AccountsDAL accountsDAL = new AccountsDAL();
            bool check = false;
            double debit = 0;
            double credit = 0;

            List<sms_voucher> vouchers_list = accountsDAL.getAllVoucher();
            List<sms_voucher_entries> vouchers_entries_list = accountsDAL.getAllVoucherEntries();

            foreach (var voucher in vouchers_list)
            {
                check = false;
                debit = 0;
                credit = 0;

                foreach (var voucher_entry in vouchers_entries_list.Where(x=>x.voucher_id == voucher.id))
                {
                    check = true;
                    debit = debit + voucher_entry.debit;
                    credit = credit = credit + voucher_entry.credit;
                }
                if (check == false)
                {
                    MessageBox.Show("voucher_id=" + voucher.id + " Not exist in sms_voucher_entries");
                }
                else 
                {
                    if (voucher.amount == debit && voucher.amount == credit)
                    {
                    }
                    else 
                    {
                        MessageBox.Show("voucher_id=" + voucher.id + " with amount="+voucher.amount+" not equal in sms_voucher_entries with debit="+debit+" credit="+credit);
                    }
                }
            }
            MessageBox.Show("Successfully Ending Debuging");
        }




        // =============================================================================================================================
        private string myValue;
        public string MyValue
        {
            get { return myValue; }
            set
            {
                myValue = value;
                RaisePropertyChanged("MyValue");
            }
        }

        private void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
 
    }
    public static class ExtensionMethods
    {

        private static Action EmptyDelegate = delegate() { };


        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
