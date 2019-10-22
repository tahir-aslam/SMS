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
using SMS.AdmissionManagement.Admission;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using SMS.AdmissionManagement.Camera;
using WPF_Webcam;
using SMS.DAL;
using System.Collections.ObjectModel;
using SMS.AdmissionManagement.Admission.BirthDayCertificate;
using SMS.Common;

namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for AdmissionFormNew.xaml
    /// </summary>
    public partial class AdmissionFormNew : Window
    {

        
        char Active = 'Y';
        string month_name = "";
        public string remarks = "";
        public DateTime withdrawal_date;
        public bool withdraw_status = false;

        AdmissionSearchNew AS;

        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;        
        List<sms_months> months_list;
        List<session> session_list;

        roll_no roll_no;
        string roll_format = "";
        public string FileName = "";
        bool check_filedialog;
        int last_id = 0;

        string cl_id;
        int last_roll = 0;
        int last_adm = 0;        

        admission adm_obj;
        admission obj;
        string mode;

        ClassesDAL classesDAL;
        FeesDAL feesDAL;
        AdmissionDAL admDAL;
        List<sms_fees_actual> fees_list;

        public AdmissionFormNew(string m, AdmissionSearchNew adm_s, admission ob)
        {
            InitializeComponent();
            classesDAL = new ClassesDAL();
            feesDAL = new FeesDAL();
            admDAL = new AdmissionDAL();

            AS = adm_s;
            this.obj = ob;
            this.mode = m;

            session_cmb.SelectedIndex = 0;
            classes_list = new List<classes>();
            sections_list = new List<sections>();
            get_all_classes();
            class_cmb.SelectedIndex = 0;
            classes_list.Insert(0, new classes() { class_name = "-Select Class-", id = "-1" });
            class_cmb.ItemsSource = classes_list;
            religion_textbox.Text = "Islam";
            roll_textbox.IsEnabled = true;
            get_all_sessions();
            session_cmb.ItemsSource = session_list;
            session_cmb.SelectedIndex = Convert.ToInt32(MainWindow.session.id) - 1;
            session_cmb.IsEnabled = false;
            get_all_months();
            std_card_btn.IsEnabled = false;
            sibling_btn.IsEnabled = false;
            schoolConfirmation_btn.IsEnabled = true;

            roll_no_prefix_cmb.ItemsSource = MainWindow.roll_no_prefix_list;
            adm_no_prefix_cmb.ItemsSource = MainWindow.adm_no_prefix_list;
            area_cmb.ItemsSource = MainWindow.area_list;
            class_in_cmb.ItemsSource = classes_list;

            roll_no_prefix_cmb.SelectedIndex = 0;
            adm_no_prefix_cmb.SelectedIndex = 0;
            area_cmb.SelectedIndex = 0;
            class_in_cmb.SelectedIndex = 0;

            List<sms_fees_package> fees_package_list = MainWindow.fees_package_list;
            package_cmb.ItemsSource = fees_package_list;
            package_cmb.SelectedIndex = 0;

            List<sms_months> months_list = MainWindow.months_list;
            month_cmb.ItemsSource = months_list;
            month_cmb.SelectedIndex = 0;            

            if (mode == "edit")
            {
                birthday_btn.IsEnabled = true;
                sibling_btn.IsEnabled = true;
                std_card_btn.IsEnabled = true;

                last_id = Convert.ToInt32(obj.id);                
                fill_control();                
                class_cmb.IsEnabled = true;               
                
                full_name_textbox.Focus();

                generate_CB.Visibility = Visibility.Collapsed;
            }
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //--------------           Save          ----------------------
        public void save()
        {
            fill_object();            
            if (validate())
            {                
                if (mode == "insert")
                {
                    if (submit() > 0)
                    {                        
                        delete_last_roll_no();
                        insert_last_roll_no();
                        update_adm_no();                        
                        AS.load_grid();
                        AS.adm_grid.SelectedValue = last_id;
                        AS.adm_grid.ScrollIntoView(adm_obj);
                        //insert actual fees
                        FeesDAL feesDAL = new FeesDAL();
                        if (feesDAL.insertActualFees(getFeesList(last_id)) > 0)
                        {

                        }
                        else 
                        {
                            MessageBox.Show("Fees Data Not Updated");
                        }
                        MessageBox.Show("Record Added Successfully","Information",MessageBoxButton.OK,MessageBoxImage.Information);
                        this.Close();
                        
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else if (mode == "edit")
                {
                    if (update() > 0)
                    {
                        int cl_index;
                        int sec_index;
                        delete_last_roll_no();
                        insert_last_roll_no();
                        update_adm_no();                        
                        cl_index = AS.class_cmb.SelectedIndex;
                        sec_index = AS.section_cmb.SelectedIndex;
                        AS.load_grid();
                        AS.class_cmb.SelectedIndex = cl_index;
                        AS.section_cmb.SelectedIndex = sec_index;
                        AS.adm_grid.SelectedValue = obj.id;
                        AS.adm_grid.ScrollIntoView(18);
                        FeesDAL feesDAL = new FeesDAL();
                        if (feesDAL.insertActualFees(getFeesList(Convert.ToInt32(obj.id))) > 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("Fees Data Not Updated");
                        }
                        MessageBox.Show("Record Updated Successfully","Information",MessageBoxButton.OK,MessageBoxImage.Information);                        
                        this.Close();
                        AS.adm_grid.Focus();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
                else
                {
                    MessageBox.Show("mode not set");
                }
            }
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
        //---------------           Submit Form    ----------------------------------
        public int submit()
        {
            int i = 1;
            FileStream fs;

            BinaryReader br;
            byte[] ImageData;
            
            try
            {        
                    if ( FileName != "" ) 
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromFile(FileName);
                        if (image.Width >= 200)
                        {
                            System.Drawing.Image thumbnail = image.GetThumbnailImage(200, 250, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                            MemoryStream imageStream = new MemoryStream();
                            thumbnail.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            ImageData = new Byte[imageStream.Length];
                            imageStream.Position = 0;
                            imageStream.Read(ImageData, 0, (int)imageStream.Length);
                        }
                        else
                        {
                            fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                            br = new BinaryReader(fs);
                            ImageData = br.ReadBytes((int)fs.Length);
                            br.Close();
                            fs.Close();
                        }
                       
                        
                    }
                    else
                    {
                        if (boarding_yes.IsChecked == true)
                        {
                            ImageData = MainWindow.ins.male_image;
                        }
                        else                         
                        {
                            ImageData = MainWindow.ins.female_image;
                        }
                        
                    }

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_admission(class_id,std_name,father_name,father_cnic,father_income,religion,dob,b_form,parmanent_adress,adm_date,cell_no,emergency_address,previous_school,boarding,transport,comm_adress,roll_no,adm_no,reg_fee,adm_fee,tution_fee,scholarship,misc_charges,exam_fee,security_fee,transport_fee,other_exp,stationary_fee,total,class_name,section_id,section_name,image,is_active,created_by,date_time,session_id,fees_package_id,fees_package, class_in_id, roll_no_prefix_id, adm_no_prefix_id, city_area_id, family_group_id, adm_no_int, roll_no_int) Values(@class_id,@std_name,@father_name,@father_cnic,@father_income,@religion,@dob,@b_form,@parmanent_adress,@adm_date,@cell_no,@emergency_address,@previous_school,@boarding,@transport,@comm_adress,@roll_no,@adm_no,@reg_fee,@adm_fee,@tution_fee,@scholarship,@misc_charges,@exam_fee,@security_fee,@transport_fee,@other_exp,@stationary_fee,@total,@class_name,@section_id,@section_name,@image,@is_active,@created_by,@date_time,@session_id, @fees_package_id,@fees_package, @class_in_id, @roll_no_prefix_id, @adm_no_prefix_id, @area_id, @family_group_id, @adm_no_int, @roll_no_int); SELECT LAST_INSERT_ID()";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_name;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_id;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_name; 

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
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                        cmd.Parameters.Add("@scholarship", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.scholarship;
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.misc_charges;
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;
                        cmd.Parameters.Add("@transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.transport_fee;
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                        cmd.Parameters.Add("@stationary_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.stationary_fee;                        
                   
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "Y";
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.date_time;

                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;

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
            }                           
                                           
            return i;
        }

        //---------------           Update Form        ---------------------------------
        public int update()
        {            
            int i = 0;
            try
            {
                FileStream fs;

                BinaryReader br;
                byte[] ImageData;
                if (FileName != "")
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(FileName);
                    if (image.Width >= 200)
                    {
                        System.Drawing.Image thumbnail = image.GetThumbnailImage(200, 250, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                        MemoryStream imageStream = new MemoryStream();
                        thumbnail.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ImageData = new Byte[imageStream.Length];
                        imageStream.Position = 0;
                        imageStream.Read(ImageData, 0, (int)imageStream.Length);
                    }
                    else
                    {
                        fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                        br = new BinaryReader(fs);
                        ImageData = br.ReadBytes((int)fs.Length);
                        br.Close();
                        fs.Close();
                    }


                }
                else
                {
                    
                        ImageData = obj.image;
                    
                }
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_admission SET class_id=@class_id,std_name=@std_name,b_form=@b_form,section_name=@section_name,transport=@transport,stationary_fee=@stationary_fee,class_name=@class_name,section_id=@section_id,comm_adress=@comm_adress,roll_no=@roll_no,adm_no=@adm_no,tution_fee=@tution_fee,total=@total,scholarship=@scholarship,misc_charges=@misc_charges,exam_fee=@exam_fee,security_fee=@security_fee,other_exp=@other_exp,transport_fee=@transport_fee,parmanent_adress=@parmanent_adress,adm_date=@adm_date,cell_no=@cell_no,emergency_address=@emergency_address,previous_school=@previous_school,boarding=@boarding,father_name=@father_name,father_cnic=@father_cnic,father_income=@father_income,religion=@religion,dob=@dob,reg_fee=@reg_fee,adm_fee=@adm_fee,is_active=@is_active,created_by=@created_by,date_time=@date_time,image=@image,updation=@updation, fees_package_id=@fees_package_id, fees_package=@fees_package, class_in_id=@class_in_id, roll_no_prefix_id=@roll_no_prefix_id, adm_no_prefix_id=@adm_no_prefix_id, city_area_id=@area_id, family_group_id=@family_group_id, adm_no_int=@adm_no_int, roll_no_int=@roll_no_int WHERE id = @id && session_id=@session_id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_id;
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
                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                        cmd.Parameters.Add("@scholarship", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.scholarship;
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.misc_charges;
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;
                        cmd.Parameters.Add("@transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.transport_fee;
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                        cmd.Parameters.Add("@stationary_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.stationary_fee;
                        cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.total;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_name;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_id;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_name;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "Y";
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;

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

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }         
            return i;
        }

        //------------------    Fill Object      ------------------------
        public void fill_object()
        {
            adm_obj = new admission();

            classes cl = (classes)class_cmb.SelectedItem;
            string cl_id = cl.id.ToString();
            string cl_name = cl.class_name.ToString();

            sections sec = (sections)section_cmb.SelectedItem;
            if (sec != null)
            {
                string sec_id = sec.id.ToString();
                string sec_name = sec.section_name.ToString();
                adm_obj.section_id = sec_id;
                adm_obj.section_name = sec_name;
            }

            adm_obj.class_id = cl_id;
            adm_obj.class_name = cl_name;
            
            adm_obj.std_name = full_name_textbox.Text.Trim();
            adm_obj.father_name = fname_textbox.Text.Trim();
            adm_obj.father_cnic = fCNIC_textbox.Text.Trim();
            adm_obj.father_income = fIncome_textbox.Text.Trim();
            adm_obj.religion = religion_textbox.Text.Trim();
            if(dob_textbox.SelectedDate != null)
            {
                adm_obj.dob = dob_textbox.SelectedDate.Value;
            }
            
            adm_obj.b_form = bForm_textbox.Text.Trim();
            adm_obj.parmanent_adress = address_textbox.Text.Trim();
            adm_obj.cell_no = cell_textbox.Text.Trim();
            if(adm_date_textbox.SelectedDate != null)
            {
                adm_obj.adm_date = adm_date_textbox.SelectedDate.Value;
            }
            
            adm_obj.emergency_address = Ephone_textbox.Text.Trim();
            adm_obj.previous_school = pre_school_textbox.Text.Trim();
            adm_obj.comm_adress = commm_textbox.Text.Trim();

            try
            {
                adm_obj.roll_no_int = Convert.ToInt32(roll_textbox.Text.Trim());
                adm_obj.adm_no_int = Convert.ToInt32(adm_textbox.Text.Trim());
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

            adm_obj.date_time = DateTime.Now;
            adm_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            
            if (boarding_yes.IsChecked == true)
            {
                adm_obj.boarding = "Y";
                adm_obj.gender = "Male";
            }
            else
            {
                adm_obj.boarding = "N";
                adm_obj.gender = "Female";
            }
            
            if (package_cmb.SelectedItem != null)
            {
                sms_fees_package package = (sms_fees_package)package_cmb.SelectedItem;
                adm_obj.fees_package_id = package.id;
                adm_obj.fees_package = package.package_name;
            }

            if (roll_no_prefix_cmb.SelectedItem != null)
            {
                prefixNo obj = (prefixNo)roll_no_prefix_cmb.SelectedItem;
                adm_obj.roll_no_prefix_id = obj.id;
                adm_obj.roll_no = obj.prefix_abbreviation + adm_obj.roll_no_int.ToString();
            }

            if (adm_no_prefix_cmb.SelectedItem != null)
            {
                prefixNo obj = (prefixNo)adm_no_prefix_cmb.SelectedItem;
                adm_obj.adm_no_prefix_id = obj.id;
                adm_obj.adm_no = obj.prefix_abbreviation + adm_obj.adm_no_int.ToString();
            }

            if (area_cmb.SelectedItem != null)
            {
                CityArea obj = (CityArea)area_cmb.SelectedItem;
                adm_obj.area_id = obj.id;
            }

            if (class_in_cmb.SelectedItem != null)
            {
                classes obj = (classes)class_in_cmb.SelectedItem;
                adm_obj.class_in_id = Convert.ToInt32(obj.id);
            }     
        }

        //------------------    Fill Control     -------------------------
        public void fill_control()
        {
            MemoryStream stream = new MemoryStream(obj.image);
            student_image.Source = ByteToImage(obj.image);

            class_cmb.SelectedValue = obj.class_id;
            section_cmb.SelectedValue = obj.section_id;
            full_name_textbox.Text = obj.std_name;
            fname_textbox.Text = obj.father_name;
            fCNIC_textbox.Text = obj.father_cnic;
            fIncome_textbox.Text = obj.father_income;
            religion_textbox.Text = obj.religion;
            dob_textbox.SelectedDate = obj.dob;
            bForm_textbox.Text = obj.b_form;
            address_textbox.Text = obj.parmanent_adress;
            cell_textbox.Text = obj.cell_no;
            adm_date_textbox.SelectedDate = obj.adm_date;
            Ephone_textbox.Text = obj.emergency_address;
            commm_textbox.Text = obj.comm_adress;
            try 
            {
                if (obj.roll_no_int == 0)                
                {
                   string result = Regex.Match(obj.roll_no, @"\d+").Value;
                   roll_textbox.Text = result;
                }
                else 
                {
                    roll_textbox.Text = obj.roll_no_int.ToString();
                }

                if (obj.adm_no_int == 0)
                {
                    string result = Regex.Match(obj.adm_no, @"\d+").Value;
                    adm_textbox.Text = result;
                }
                else 
                {
                    adm_textbox.Text = obj.adm_no_int.ToString();
                }                
                
            }
            catch(Exception ex)
            {
                roll_textbox.Text = obj.roll_no_int.ToString();
                adm_textbox.Text = obj.adm_no_int.ToString();
            }
            
            pre_school_textbox.Text = obj.previous_school;            

            package_cmb.SelectedValue = obj.fees_package_id;
            

            if (obj.boarding == "Y")
            {
                boarding_yes.IsChecked = true;
            }
            else
            {
                boarding_no.IsChecked = true;
            }

            roll_no_prefix_cmb.SelectedValue = obj.roll_no_prefix_id;
            adm_no_prefix_cmb.SelectedValue = obj.adm_no_prefix_id;
            area_cmb.SelectedValue = obj.area_id;
            class_in_cmb.SelectedValue = obj.class_in_id;
        }

        //------------------    Validation       -------------------------
        public bool validate()
        {


            if (full_name_textbox.Text.Trim().Length == 0)
            {
                full_name_textbox.Focus();
                string alertText = "Student Name Should Not Be Blank";
                MessageBox.Show(alertText,"Validation Error",MessageBoxButton.OK,MessageBoxImage.Warning);
                return false;
            }
            else if (fname_textbox.Text.Trim().Length == 0)
            {
                fname_textbox.Focus();
                string alertText = "Father Name Should Not Be Blank";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (dob_textbox.SelectedDate==null)
            {
                dob_textbox.Focus();
                string alertText = "Date Of Birth Should Not Be Blank.";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (cell_textbox.Text.Length == 0)
            {
                
                cell_textbox.Focus();
                string alertText = "Cell # Should Not Be Blank";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (adm_date_textbox.SelectedDate == null)
            {
                adm_date_textbox.Focus();
                string alertText = "Admission Date Should Not Be Blank.";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
           else if (class_cmb.SelectedIndex == 0)
            {
                class_cmb.Focus();
                string alertText = "Class Name Should Not Be Blank.";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (section_cmb.SelectedIndex == 0 || section_cmb.SelectedIndex < 0 )
            {

                string alertText = "Section Name Should Not Be Blank";
                
                section_cmb.Focus();
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            
            else if (adm_textbox.Text.Trim().Length == 0)
            {
                adm_textbox.Focus();
                string alertText = "Admission No Should Not Be Blank";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (roll_textbox.Text.Trim().Length == 0)
            {
                roll_textbox.Focus();
                string alertText = "Roll No Should Not Be Blank";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (check_adm_no())
            {
                adm_textbox.Focus();
                string alertText = "Admission No Already Exist";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (feesDataGrid.Items.Count < 1)
            {
                feesDataGrid.Focus();
                string alertText = "Fees Grid Should Not Be Blank, Please Inser Fees According To Classes";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (package_cmb.SelectedItem == null || package_cmb.SelectedIndex == 0)
            {
                package_cmb.Focus();
                string alertText = "Please Select Fees Package";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (generate_CB.IsChecked == true && month_cmb.SelectedIndex == 0)
            {
                month_cmb.Focus();
                string alertText = "Please Select Month To Generate Fees";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            else
            {
                return true;
            }

        }
        public bool check_adm_no() 
        {
            bool check= false;
            if(mode == "edit")
            {

            }
            else
            {
                obj = new admission();
                obj.id="0";
            }

            List<admission> adm_list = admDAL.get_all_admissions_sessions();
            bool a = Regex.IsMatch(adm_textbox.Text.Trim(), @"^\d+$");
            bool b = false;
            string full_adm_no = "";
            prefixNo objP = (prefixNo)adm_no_prefix_cmb.SelectedItem;

            try
            {
                full_adm_no = objP.prefix_abbreviation + Convert.ToInt32(adm_textbox.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return true;
            }

           

            foreach (admission adm in adm_list)
            {                
                b = Regex.IsMatch(adm.adm_no, @"^\d+$");
                if (a && b)
                {
                    if (!string.IsNullOrWhiteSpace(adm.adm_no) || !string.IsNullOrEmpty(adm.adm_no))
                    {
                        if (adm.adm_no == full_adm_no && adm.id != obj.id)
                        {
                            check = true;
                        }
                    }
                }
                else 
                {
                    if (!string.IsNullOrWhiteSpace(adm.adm_no) || !string.IsNullOrEmpty(adm.adm_no))
                    {
                        if (adm.adm_no == full_adm_no && adm.id != obj.id)
                        {
                            check = true;
                        }
                    }
                }
            }

            if (check)
            {
                return true;
            }
            else 
            {
                return false;
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
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                transport_charges = Convert.ToString(reader["transport_charges"].ToString()),
                                boarding_fee = Convert.ToString(reader["boarding_fee"].ToString()),
                                misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                stationary_charges = Convert.ToString(reader["stationary_charges"].ToString()),
                                books_charges = Convert.ToString(reader["books_charges"].ToString()),
                                other_exp = Convert.ToString(reader["other_exp"].ToString()),
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            };
                            classes_list.Add(classes);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Classes DB not connected");
            }
        }

        //------------         Get All Sections   ------------------------
        public void get_all_sections(string id)
        {


            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {

                    sections_list = new List<sections>();
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects where class_id =" + id;
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
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                save();
            }
        }

        //-----------     Classes Selection Changed        --------------------------------------
        private void class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (class_cmb.SelectedIndex != 0)
                {
                    classes cl = (classes)class_cmb.SelectedItem;
                    cl_id = cl.id;

                    get_all_sections(cl_id);

                    section_cmb.IsEnabled = true;
                    sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                    section_cmb.ItemsSource = sections_list;
                    section_cmb.SelectedIndex = 0;

                    if (mode == "insert")
                    {                        
                        fees_list = new List<sms_fees_actual>();
                        fees_list = classesDAL.getAllFeesClasses().Where(x => x.class_id == Convert.ToInt32(cl.id)).ToList();
                        feesDataGrid.ItemsSource = fees_list;
                    }
                    else
                    {
                        bool check = false;
                        List<sms_fees_actual> fees_actual_list = feesDAL.getActualFeesByStdID(Convert.ToInt32(obj.id));
                        List<sms_fees_actual> fees_classes_list = classesDAL.getAllFeesClasses().Where(x => x.class_id == Convert.ToInt32(cl.id)).ToList();
                        List<sms_fees_actual> new_list = new List<sms_fees_actual>();

                        foreach (var classes in fees_classes_list)
                        {
                            check = false;
                            foreach (var actual in fees_actual_list.Where(x => x.fees_category_id == classes.fees_category_id))
                            {
                                check = true;
                            }
                            if(check == false)
                            {
                                new_list.Add(classes);
                            }
                        }

                        // add new items in actual list
                        foreach (var item in new_list)
                        {
                            fees_actual_list.Add(item);
                        }
                        feesDataGrid.ItemsSource = fees_actual_list;                        
                    }
                    // fees grid                   
                }
                else
                {
                    sections_list.Insert(0, new sections() { section_name = "---Select Section---", id = "-1" });
                    section_cmb.SelectedIndex = 0;
                    section_cmb.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //==============           Generate Roll # and Admission #                ===========================
        public void generate_roll_no(string id)
        {
            last_roll = 0;
            last_adm = 0;

            get_roll_no(id);
            

            last_roll = Convert.ToInt32(roll_no.last_roll_no);
            
            last_roll++;
            
            roll_format = roll_format + last_roll.ToString("D3");
            roll_textbox.Text = roll_format;

            last_adm = Convert.ToInt32(roll_no.last_adm_no);
            if(mode=="insert")
            {
                
                last_adm = Convert.ToInt32(roll_no.last_adm_no);
                last_adm++;
                adm_textbox.Text = last_adm.ToString("D6");
            }
        }

        //==============           Get sms_roll_no                ===========================
        public void get_roll_no(string id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        string section_id;
                        roll_no = new roll_no();
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
                                    roll_no.last_roll_no = Convert.ToString(reader["last_roll_no"].ToString());
                                    roll_no.last_adm_no = Convert.ToString(reader["last_adm_no"].ToString());
                                    break;
                                }
                                else 
                                {
                                    roll_no.last_adm_no = Convert.ToString(reader["last_adm_no"].ToString());
                                }
                            }
                        }
                        else
                        {
                            roll_no.last_roll_no = "00";
                            roll_no.last_adm_no = "000000";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        //========        Delete Last Roll No            =======================================
        public void delete_last_roll_no()
        {
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {

                    sections s = (sections)section_cmb.SelectedItem;

                    cmd.CommandText = "Delete from sms_roll_no where section_id=" + s.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        //==========       Insert last roll no        ================================
        public void insert_last_roll_no() 
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        sections s = (sections)section_cmb.SelectedItem;
                        cmd.CommandText = "INSERT INTO sms_roll_no(class_id,section_id,last_roll_no)Values(@class_id,@section_id,@roll_no)";
                       
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value =last_roll;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = s.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = cl_id;
                       

                        con.Open();

                        cmd.ExecuteScalar();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void update_adm_no() 
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {

                        cmd.CommandText = "update sms_roll_no SET last_adm_no=@adm_no";

                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                               
                        cmd.Parameters.Add("@adm_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_adm;
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
        
        //=======            Image upload           ==============================
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.Filter = "Image files | *.jpg; *.jpeg; *.png;";

                if (openFileDialog1.ShowDialog() == true)
                {
                    FileName = openFileDialog1.FileName;
                    student_image.Source = new BitmapImage(new Uri(openFileDialog1.FileName));

                    
                    check_filedialog = true;
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        public bool ThumbnailCallback()
        {
            return true;
        }

        public ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            try
            {

                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }
        
        private void camera_btn_Click_1(object sender, RoutedEventArgs e)
        {
        //    CameraEngineWindow cew = new CameraEngineWindow();
        //    cew.ShowDialog();
        //    Form1 f = new Form1();
        //    f.ShowDialog();
          //  MainForm mf = new MainForm();
           // mf.Show();
            try
            {
                //WPF_Webcam.MainWindow wm = new WPF_Webcam.MainWindow();
                //wm.ShowDialog();
                CameraEngineWindow cew = new CameraEngineWindow(this);
                cew.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void section_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

            sections s = (sections)section_cmb.SelectedItem;
            if (s != null)
            {
                if (Convert.ToInt32(s.id) > 0)
                {
                    roll_format = s.roll_no_format;
                    generate_roll_no(s.id);
                }
                else 
                {
                    roll_textbox.Text = "";
                    //adm_textbox.Text = "";
                }
            }
        }

        private void withdraw_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Do You Want To Withdrawal This Student ?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                GetDateRemarksWindow window = new GetDateRemarksWindow(this);
                window.ShowDialog();
                if (withdraw_status)
                {
                    update_withdraw();
                    AS.load_grid();
                    MessageBox.Show("Admission Withdrawal Successfully","Successfully",MessageBoxButton.OK,MessageBoxImage.Information);
                    this.Close();
                }
            }
        }

        public void update_withdraw()
        {
            try 
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_admission SET is_active=@is_active, withdrawal_date=@withdrawal_date, remarks=@remarks, created_by=@created_by, date_time=@date_time, updation=@updation  WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;                        
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = 'N';
                        cmd.Parameters.Add("@withdrawal_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = withdrawal_date;
                        cmd.Parameters.Add("@remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = remarks;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {

            if(obj != null)
            {
                if (obj.boarding == "Y")
                {
                    obj.gender = "Male";
                }
                else
                {
                    obj.gender = "Female";
                }
            }
            else
            {

                obj = new admission();
                obj.adm_date = DateTime.Now;
                obj.dob.ToString("");
            }
            
            AdmissionFormPrint afp = new AdmissionFormPrint(obj);
            afp.ShowDialog();
            
        }

        private void std_card_btn_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            StudentCardWindow STW = new StudentCardWindow(obj);
            STW.ShowDialog();
        }

        private void sibling_btn_Click(object sender, RoutedEventArgs e)
        {
            SiblingWindow sw = new SiblingWindow(obj,AS.adm_list);
            sw.ShowDialog();
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AS.SearchTextBox.Clear();
        }        

        private void generate_CB_Checked(object sender, RoutedEventArgs e)
        {
            month_cmb.Visibility = Visibility.Visible;
            month_Lbl.Visibility = Visibility.Visible;
        }

        private void generate_CB_Unchecked(object sender, RoutedEventArgs e)
        {
            month_cmb.Visibility = Visibility.Collapsed;
            month_Lbl.Visibility = Visibility.Collapsed;
        }

        public void setFeesGrid() 
        {
            List<sms_fees_actual> fees_list = new List<sms_fees_actual>();
            sms_fees_actual fees_obj;
            for (int i = 0; i < feesDataGrid.Items.Count; i++)
            {
                fees_obj = new sms_fees_actual();
                fees_obj = feesDataGrid.Items[i] as sms_fees_actual;
                fees_obj.amount = fees_obj.actual_amount - fees_obj.discount;                
                fees_list.Add(fees_obj);
            }
            feesDataGrid.ItemsSource = fees_list;
            feesDataGrid.Items.Refresh();
        }

        public List<sms_fees_actual> getFeesList(int std_id) 
        {
            List<sms_fees_actual> fees_list = new List<sms_fees_actual>();
            sms_fees_actual fees_obj;

            for (int i = 0; i < feesDataGrid.Items.Count; i++ ) 
            {
                fees_obj = new sms_fees_actual();
                fees_obj = (sms_fees_actual)feesDataGrid.Items[i];
                fees_obj.amount = fees_obj.actual_amount - fees_obj.discount;
                fees_obj.std_id = std_id;
                fees_list.Add(fees_obj);
            }           
            return fees_list;
        }

        private void birthday_btn_Click(object sender, RoutedEventArgs e)
        {
            List<admission> adm_list = new List<admission>();
            admission adm = new admission();
            adm = obj;
            adm.dob_in_words = DateToWords.DateToWritten(obj.dob.Date);
            DateToAge age = new DateToAge(obj.dob.Date, DateTime.Now.Date);
            adm.age_in_words = age.Years + " Years, " + age.Months + " Months, " + age.Days + " Days";
            adm_list.Add(adm);
            BirthDayCertificateWindow window = new BirthDayCertificateWindow(adm_list);
            window.ShowDialog();
        }

        private void schoolConfirmation_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<admission> adm_list = new List<admission>();
                admission adm = new admission();
                adm = obj;
                adm.dob_in_words = DateToWords.DateToWritten(obj.dob.Date);
                DateToAge age = new DateToAge(obj.dob.Date, DateTime.Now.Date);
                adm.age_in_words = age.Years + " Years, " + age.Months + " Months, " + age.Days + " Days";
                adm_list.Add(adm);
                SchoolConfirmationCertificate.SchoolConfirmationWindow window = new SchoolConfirmationCertificate.SchoolConfirmationWindow(adm_list);
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void rfid_btn_Click(object sender, RoutedEventArgs e)
        {
            RFIDWindow window = new RFIDWindow(obj);
            window.ShowDialog();
        }

        private void adm_slip_btn_Click(object sender, RoutedEventArgs e)
        {
             List<admission> adm_list = new List<admission>();
            admission adm = new admission();
            adm = obj;
            adm.dob_in_words = DateToWords.DateToWritten(obj.dob.Date);
            adm_list.Add(adm);
            AdmissionSlip window = new AdmissionSlip(adm_list);
            window.ShowDialog();
        }            
        
    }
}
