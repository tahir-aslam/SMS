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
using System.Collections.ObjectModel;
using SMS.ViewModels;
using SUT.PrintEngine.Utils;
using System.Windows.Markup;
using System.Data;
using System.IO;

namespace SMS.AdmissionManagement.PromoteStudents
{
    /// <summary>
    /// Interaction logic for PromoteStudentForm.xaml
    /// </summary>
    public partial class PromoteStudentForm : Window
    {
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        admission adm_obj;
        List<session> session_list;

        public PromoteStudentForm(List<admission> adm_list)
        {
            InitializeComponent();
            this.adm_list = adm_list;
            adm_obj = adm_list[0];

            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
            class_cmb.ItemsSource = classes_list;

            std_count_lbl.Content=adm_list.Count;
            class_selected_lbl.Content = adm_obj.class_name;
            section_selected_lbl.Content = adm_obj.section_name;

            get_all_sessions();
            session_cmb.ItemsSource = session_list;
            session_cmb.SelectedIndex = session_list.Count - 1;
            session_cmb.IsEnabled = true;
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

        //------------------------  Class Selection Changed -------------------------
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classes c = (classes)class_cmb.SelectedItem;
            string id = c.id;

            if (class_cmb.SelectedIndex != 0)
            {
                section_cmb.IsEnabled = true;
                get_all_sections(id);
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

        // ----------------------  Sections Selection changed---------------------------
        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (section_cmb.SelectedIndex > 0)
            {
                sections s = (sections)section_cmb.SelectedItem;
                classes c = (classes)class_cmb.SelectedItem;
                if (Convert.ToInt32(s.id) > 0)
                {
                    
                }
            }
            else
            {
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

        //------------              Get All Sections   ------------------------
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
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        public bool validate() 
        {
            if (class_cmb.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Promoted Class", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            else if (section_cmb.SelectedIndex == 0)
            {
                MessageBox.Show("Please Select Promoted Section", "Stop", MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            else 
            {
            }

            return true;
        }

        private void click_save(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Do You Want To Promote this section ?", "Confirmation", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                if (validate())
                {
                    promote_students();
                }
            }
        }

        public void promote_students() 
        {
            classes cl = (classes)class_cmb.SelectedItem;
            sections sec = (sections)section_cmb.SelectedItem;
            int roll_no=0;

            foreach(admission adm in adm_list)
            {
                if (reArrangeRollNoCheckBox.IsChecked == true)
                {
                    roll_no++;                    
                }
                else
                {
                    roll_no = adm.roll_no_int;
                }

                adm.roll_no_int = roll_no;
                adm.roll_no = MainWindow.roll_no_prefix_list.Where(x => x.id == adm.roll_no_prefix_id).FirstOrDefault().prefix_abbreviation+ roll_no;
                //adm.adm_no = MainWindow.adm_no_prefix_list.Where(x => x.id == adm.adm_no_prefix_id).FirstOrDefault().prefix_abbreviation + adm.adm_no_int;

                if (submit(adm, cl, sec) > 0) 
                {
                    
                }
            }
            MessageBox.Show("Successfull Promoted","Stop",MessageBoxButton.OK,MessageBoxImage.Information);
            this.Close();
            class_cmb.SelectedIndex = 0;
        }

        public int submit(admission adm_obj,classes cl_obj, sections sec_obj) 
        {
            int i = 0;
            int last_id=0;
            session sess = (session)session_cmb.SelectedItem;            
            try
            {

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_admission(id,class_id,std_name,father_name,father_cnic,father_income,religion,dob,b_form,parmanent_adress,adm_date,cell_no,emergency_address,previous_school,boarding,transport,comm_adress,roll_no,adm_no,reg_fee,adm_fee,tution_fee,scholarship,misc_charges,exam_fee,security_fee,transport_fee,other_exp,stationary_fee,total,class_name,section_id,section_name,image,is_active,created_by,date_time,session_id,fees_package_id,fees_package, class_in_id, roll_no_prefix_id, adm_no_prefix_id, city_area_id, family_group_id, adm_no_int, roll_no_int) Values(@id,@class_id,@std_name,@father_name,@father_cnic,@father_income,@religion,@dob,@b_form,@parmanent_adress,@adm_date,@cell_no,@emergency_address,@previous_school,@boarding,@transport,@comm_adress,@roll_no,@adm_no,@reg_fee,@adm_fee,@tution_fee,@scholarship,@misc_charges,@exam_fee,@security_fee,@transport_fee,@other_exp,@stationary_fee,@total,@class_name,@section_id,@section_name,@image,@is_active,@created_by,@date_time,@session_id, @fees_package_id,@fees_package, @class_in_id, @roll_no_prefix_id, @adm_no_prefix_id, @area_id, @family_group_id, @adm_no_int, @roll_no_int); SELECT LAST_INSERT_ID()";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value =sess.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = cl_obj.id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = cl_obj.class_name;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sec_obj.id;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sec_obj.section_name; 

                        cmd.Parameters.Add("@std_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.std_name;
                        cmd.Parameters.Add("@father_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_name;
                        cmd.Parameters.Add("@father_cnic", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_cnic;
                        cmd.Parameters.Add("@father_income", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_income;
                        cmd.Parameters.Add("@religion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.religion;
                        cmd.Parameters.Add("@dob", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.dob;
                        cmd.Parameters.Add("@b_form", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.b_form;
                        cmd.Parameters.Add("@parmanent_adress", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.parmanent_adress;

                        cmd.Parameters.Add("@adm_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = adm_obj.adm_date;
                        cmd.Parameters.Add("@cell_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.cell_no;
                        cmd.Parameters.Add("@emergency_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.emergency_address;
                        cmd.Parameters.Add("@previous_school", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.previous_school;
                        cmd.Parameters.Add("@boarding", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.boarding;
                        cmd.Parameters.Add("@transport", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.transport;
                        cmd.Parameters.Add("@comm_adress", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.comm_adress;

                        cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.roll_no;
                        cmd.Parameters.Add("@adm_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_no;
                        cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.total;

                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                        cmd.Parameters.Add("@scholarship", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;
                        cmd.Parameters.Add("@transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                        cmd.Parameters.Add("@stationary_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";                        
                   
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "Y";
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.date_time;

                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = adm_obj.image;

                        cmd.Parameters.Add("@fees_package_id", MySqlDbType.Int32).Value = adm_obj.fees_package_id;
                        cmd.Parameters.Add("@fees_package", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.fees_package;

                        cmd.Parameters.Add("@class_in_id", MySqlDbType.Int32).Value = adm_obj.class_in_id;
                        cmd.Parameters.Add("@roll_no_prefix_id", MySqlDbType.Int32).Value = adm_obj.roll_no_prefix_id;
                        cmd.Parameters.Add("@adm_no_prefix_id", MySqlDbType.Int32).Value = adm_obj.adm_no_prefix_id;
                        cmd.Parameters.Add("@area_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = adm_obj.area_id;
                        cmd.Parameters.Add("@family_group_id", MySql.Data.MySqlClient.MySqlDbType.Int32).Value = adm_obj.family_group_id;
                        cmd.Parameters.Add("@adm_no_int", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_no_int;
                        cmd.Parameters.Add("@roll_no_int", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.roll_no_int;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                        last_id = i;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }    
                        
                        return i;  
        }

        private void reArrangeRollNoCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
