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


namespace SMS.AdmissionManagement.Admission
{
    /// <summary>
    /// Interaction logic for AdmissionForm.xaml
    /// </summary>
    public partial class AdmissionForm : Window
    {
        int reg = 0;
        int adm = 0;
        int security = 0;
        int misc = 0;
        int exam = 0;
        int other = 0;
        char Active= 'Y';
        string month_name = "";
        public string remarks = "";
        public DateTime withdrawal_date;
        public bool withdraw_status = false;

        AdmissionSearch AS;

        List<admission> adm_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<fee> paid_fee_list;
        List<sms_months> months_list;
        List<session> session_list;

        roll_no roll_no;
        string roll_format = "";
        public string FileName = "";
        bool check_filedialog;
        int last_id=0;

        string cl_id;
        int last_roll = 0;
        int last_adm = 0;

        int total_fee = 0;
        int reg_fee = 0;
        int adm_fee = 0;
        int tutuion_fee = 0;
        int mis_fee = 0;
        int exam_fee = 0;
        int sec_fee = 0;
        int stat_fee = 0;
        int transport_fee = 0;
        int other_exp = 0;
        int scholarship = 0;

        admission adm_obj;
        admission obj;
        string mode;

        ClassesDAL classesDAL;        
       
        public AdmissionForm(string m, AdmissionSearch adm_s , admission ob )
        {
            InitializeComponent();
            classesDAL = new ClassesDAL();            

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
            session_cmb.SelectedIndex = Convert.ToInt32(MainWindow.session.id)-1;
            session_cmb.IsEnabled = false;
            get_all_months();
            std_card_btn.IsEnabled = false;
            sibling_btn.IsEnabled = false;                       

            if (mode == "edit")
            {
                sibling_btn.IsEnabled = true;
                std_card_btn.IsEnabled = true;
                

                last_id = Convert.ToInt32(obj.id);
                reg=0;
                adm=0;
                security=0;
                misc=0;
                exam = 0;
                other = 0;

                fill_control();
                //section_cmb.IsEnabled = true;
                //fees_grid.IsEnabled = false;
                class_cmb.IsEnabled = true;
                get_starting_month();
                check_fee_paid();
                foreach (fee f in paid_fee_list) 
                {
                    reg = reg + Convert.ToInt32(f.reg_fee);
                    adm = adm + Convert.ToInt32(f.adm_fee);
                    security = security + Convert.ToInt32(f.security_fee);
                    mis_fee = mis_fee + Convert.ToInt32(f.misc_charges);
                    exam = exam + Convert.ToInt32(f.exam_fee);

                    if (month_name == f.month)
                    {
                        other = other + Convert.ToInt32(f.other_expenses);
                    }
                }

                if(reg > 0)
                {
                    reg_textbox.IsEnabled = false;            
                }
                if(adm > 0)
                {
                    admmission_textbox.IsEnabled = false;
                }
                if(security > 0)
                {
                    security_textbox.IsEnabled = false;
                }
                if (exam > 0)
                {
                    exam_textbox.IsEnabled = false;
                }
                if(misc > 0)
                {
                    misc_textbox.IsEnabled = false;
                }

                if (other > 0)
                {
                    other_textbox.IsEnabled = false;
                }
                full_name_textbox.Focus();                
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

        public void check_fee_paid()
        {
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid where std_id =" + obj.id + "&& session_id ="+ MainWindow.session.id ;

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
            get_starting_month();
            if (validate())
            {                
                if (mode == "insert")
                {
                    if (submit() > 0)
                    {                        
                        delete_last_roll_no();
                        insert_last_roll_no();
                        update_adm_no();
                        insert_fee();
                        admission_date_challans();
                        AS.load_grid();
                        AS.adm_grid.SelectedValue = last_id;
                        AS.adm_grid.ScrollIntoView(adm_obj);
                        //insert actual fees
                        
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
                        admission_date_challans();
                        cl_index = AS.class_cmb.SelectedIndex;
                        sec_index = AS.section_cmb.SelectedIndex;
                        AS.load_grid();
                        AS.class_cmb.SelectedIndex = cl_index;
                        AS.section_cmb.SelectedIndex = sec_index;
                        AS.adm_grid.SelectedValue = obj.id;
                        AS.adm_grid.ScrollIntoView(18);
                                                
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

        public void get_starting_month() 
        {
            try
            {
                int month = adm_date_textbox.SelectedDate.Value.Month;
                int day = adm_date_textbox.SelectedDate.Value.Day;

                session sess = (session)session_cmb.SelectedItem;
                if (adm_date_textbox.SelectedDate.Value < sess.session_start)
                {
                    month = sess.session_start.Month;
                }
                else
                {
                    month = adm_date_textbox.SelectedDate.Value.Month;
                    day = adm_date_textbox.SelectedDate.Value.Day;
                    if (day > 26)
                    {
                        DateTime dt = adm_date_textbox.SelectedDate.Value.AddMonths(1);
                        month = dt.Month;
                    }
                }
                //for isActive='N'                
                foreach (sms_months sm in months_list)
                {
                    if (Convert.ToInt32(sm.month_id) == month)
                    {
                        month_name = sm.month_name;
                    }
                }
            }catch(Exception ex){}
            
        }
        // close challans according to admission date
        public void admission_date_challans() 
        {
            session sess = (session)session_cmb.SelectedItem;
            bool status= false;
            get_all_months();
            TimeSpan ts = DateTime.Now - adm_obj.adm_date;            
            if (adm_obj.adm_date < sess.session_start)
            {
                update_isActive_all();
            }
            else
            {
                int month = adm_obj.adm_date.Month;
                int day = adm_obj.adm_date.Day;
                if(day > 26)
                {
                   DateTime dt = adm_obj.adm_date.AddMonths(1);
                   month = dt.Month;
                }
                
                //for isActive='N'                
                foreach (sms_months sm in months_list) 
                {
                    if (Convert.ToInt32(sm.month_id) == month)
                    {
                        status = true;
                        Active = 'Y';
                        update_isActive(sm);
                        month_name = sm.month_name;
                    }
                    else if(status == true)
                    {
                        Active = 'Y';
                        update_isActive(sm);
                    }
                    else 
                    {
                        Active = 'N';
                        update_isActive(sm);
                    }                  
                   
                }               
            }
        }
        //Update isActive All
        public void update_isActive_all()
        {
            try
            {
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {

                        cmd_fee.CommandText = "Update sms_fee SET isActive=@isActive where std_id = @std_id && session_id="+MainWindow.session.id;

                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_id.ToString();                        
                        cmd_fee.Parameters.Add("@isActive", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = 'Y';

                        con_fee.Open();
                        cmd_fee.ExecuteScalar();
                        con_fee.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Update isActive
        public void update_isActive(sms_months sm) 
        {
            try 
            {
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {

                        cmd_fee.CommandText = "Update sms_fee SET isActive=@isActive where std_id = @std_id && month=@month && session_id="+MainWindow.session.id;

                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = last_id.ToString();
                        cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sm.month_name;
                        cmd_fee.Parameters.Add("@isActive", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = Active;

                        con_fee.Open();
                        cmd_fee.ExecuteScalar();
                        con_fee.Close();
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                        System.Drawing.Image thumbnail = image.GetThumbnailImage(300, 250, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                        MemoryStream imageStream = new MemoryStream();
                        thumbnail.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ImageData = new Byte[imageStream.Length];
                        imageStream.Position = 0;
                        imageStream.Read(ImageData, 0, (int)imageStream.Length);
                       
                        //fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                        //br = new BinaryReader(fs);
                        //ImageData = br.ReadBytes((int)fs.Length);
                        //br.Close();
                        //fs.Close();
                    }
                    else
                    {
                        
                        ImageData = MainWindow.ins.male_image;
                    }

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_admission(class_id,std_name,father_name,father_cnic,father_income,religion,dob,b_form,parmanent_adress,adm_date,cell_no,emergency_address,previous_school,boarding,transport,comm_adress,roll_no,adm_no,reg_fee,adm_fee,tution_fee,scholarship,misc_charges,exam_fee,security_fee,transport_fee,other_exp,stationary_fee,total,class_name,section_id,section_name,image,is_active,created_by,date_time,session_id,fees_package_id,fees_package) Values(@class_id,@std_name,@father_name,@father_cnic,@father_income,@religion,@dob,@b_form,@parmanent_adress,@adm_date,@cell_no,@emergency_address,@previous_school,@boarding,@transport,@comm_adress,@roll_no,@adm_no,@reg_fee,@adm_fee,@tution_fee,@scholarship,@misc_charges,@exam_fee,@security_fee,@transport_fee,@other_exp,@stationary_fee,@total,@class_name,@section_id,@section_name,@image,@is_active,@created_by,@date_time,@session_id, @fees_package_id,@fees_package); SELECT LAST_INSERT_ID()";
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
                   
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.date_time;

                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;

                        cmd.Parameters.Add("@fees_package_id", MySqlDbType.Int32).Value = adm_obj.fees_package_id;
                        cmd.Parameters.Add("@fees_package", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.fees_package;

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
                        try
                        {
                            string[] months = new string[12] { "January", "February", "March","April","May","June","July","August","September","October","November","December" };
                            for (int j = 0; j < 12; j++)
                            {
                                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                                {
                                    using (MySqlCommand cmd_fee = new MySqlCommand())
                                    {
                                        cmd_fee.CommandText = "INSERT INTO sms_fee(std_id,reg_fee,adm_fee,tution_fee,exam_fee,security_fee,transport_fee,other_exp,rem_reg_fee,rem_adm_fee,rem_tution_fee,rem_exam_fee,rem_security_fee,rem_transport_fee,rem_other_exp,month,created_by,date_time,session_id) Values(@std_id,@reg_fee,@adm_fee,@tution_fee,@exam_fee,@security_fee,@transport_fee,@other_exp,@rem_reg_fee,@rem_adm_fee,@rem_tution_fee,@rem_exam_fee,@rem_security_fee,@rem_transport_fee,@rem_other_exp,@month,@created_by,@date_time,@session_id)";

                                        cmd_fee.Connection = con_fee;
                                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                        cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = i;

                                        cmd_fee.Parameters.Add("@session_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.session.id;
                                        cmd_fee.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;
                                        cmd_fee.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;
                                        cmd_fee.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;
                                        cmd_fee.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;

                                        cmd_fee.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;
                                        cmd_fee.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;
                                        cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;
                                        cmd_fee.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;

                                        cmd_fee.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                                        cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                                        cmd_fee.Parameters.Add("@transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                        cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                                        if (months[j] == month_name)
                                        {
                                            cmd_fee.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                                            cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                                        }
                                        else 
                                        {
                                            cmd_fee.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                            cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                        }

                                        cmd_fee.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = months[j];
                                        cmd_fee.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.created_by;
                                        cmd_fee.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.date_time;
                                        
                                        con_fee.Open();
                                        cmd_fee.ExecuteScalar();
                                        con_fee.Close();
                                    }
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
            get_starting_month();
            int i = 0;
            try
            {
                FileStream fs;

                BinaryReader br;
                byte[] ImageData;
                if (FileName != "")
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(FileName);
                    System.Drawing.Image thumbnail = image.GetThumbnailImage(300, 250, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                    MemoryStream imageStream = new MemoryStream();
                    thumbnail.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);                    
                    ImageData = new Byte[imageStream.Length];
                    imageStream.Position = 0;
                    imageStream.Read(ImageData, 0, (int)imageStream.Length);

                    //fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    //br = new BinaryReader(fs);
                    //ImageData = br.ReadBytes((int)fs.Length);
                    //br.Close();
                    //fs.Close();
                }
                else 
                {
                    ImageData = obj.image;
                }
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_admission SET class_id=@class_id,std_name=@std_name,b_form=@b_form,section_name=@section_name,transport=@transport,stationary_fee=@stationary_fee,class_name=@class_name,section_id=@section_id,comm_adress=@comm_adress,roll_no=@roll_no,adm_no=@adm_no,tution_fee=@tution_fee,total=@total,scholarship=@scholarship,misc_charges=@misc_charges,exam_fee=@exam_fee,security_fee=@security_fee,other_exp=@other_exp,transport_fee=@transport_fee,parmanent_adress=@parmanent_adress,adm_date=@adm_date,cell_no=@cell_no,emergency_address=@emergency_address,previous_school=@previous_school,boarding=@boarding,father_name=@father_name,father_cnic=@father_cnic,father_income=@father_income,religion=@religion,dob=@dob,reg_fee=@reg_fee,adm_fee=@adm_fee,is_active=@is_active,created_by=@created_by,date_time=@date_time,image=@image,updation=@updation, fees_package_id=@fees_package_id, fees_package=@fees_package WHERE id = @id && session_id=@session_id";
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
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;

                        cmd.Parameters.Add("@fees_package_id", MySqlDbType.Int32).Value = adm_obj.fees_package_id;
                        cmd.Parameters.Add("@fees_package", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.fees_package;

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                bool check = false;
                string[] months = new string[12] { "January", "February", "March","April","May","June","July","August","September","October","November","December" };
                for (int j = 0; j < 12; j++)
                {
                    check = false;
                    foreach(fee f in paid_fee_list)
                    {
                        if(f.month == months[j])
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                        {
                            using (MySqlCommand cmd_fee = new MySqlCommand())
                            {
                                cmd_fee.CommandText = "Update sms_fee SET tution_fee=@tution_fee, other_exp=@other_exp, rem_tution_fee=@rem_tution_fee,rem_other_exp=@rem_other_exp where std_id = @std_id && month =@months && session_id="+MainWindow.session.id;

                                cmd_fee.Connection = con_fee;
                                //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                                cmd_fee.Parameters.Add("@months", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = months[j].ToString();


                                cmd_fee.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                                cmd_fee.Parameters.Add("@rem_tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;

                                if (months[j] == month_name)
                                {
                                    cmd_fee.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                                    cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                                }
                                else 
                                {
                                    cmd_fee.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                    cmd_fee.Parameters.Add("@rem_other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                                }
                                

                                con_fee.Open();
                                cmd_fee.ExecuteScalar();
                                con_fee.Close();
                            }
                        }
                    }
                }
                //Annual Fund

                if (reg > 0)
                {

                }
                else 
                {
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {

                            cmd_fee.CommandText = "Update sms_fee SET reg_fee=@reg_fee,rem_reg_fee=@rem_reg_fee where std_id = @std_id && session_id="+MainWindow.session.id;

                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;

                            cmd_fee.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;                            
                            cmd_fee.Parameters.Add("@rem_reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;                            

                            con_fee.Open();
                            cmd_fee.ExecuteScalar();
                            con_fee.Close();
                        }
                    }
                }

                // Admission Fee
                if (adm > 0)
                {
                }
                else 
                {
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {

                            cmd_fee.CommandText = "Update sms_fee SET adm_fee=@adm_fee, rem_adm_fee=@rem_adm_fee where std_id = @std_id && session_id="+MainWindow.session.id;

                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                            
                            cmd_fee.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;                           
                            cmd_fee.Parameters.Add("@rem_adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;

                            con_fee.Open();
                            cmd_fee.ExecuteScalar();
                            con_fee.Close();
                        }
                    }
                }
                // Exam Fee
                if (exam > 0)
                {
                }
                else
                {
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {

                            cmd_fee.CommandText = "Update sms_fee SET exam_fee=@exam_fee, rem_exam_fee=@rem_exam_fee where std_id = @std_id && session_id="+MainWindow.session.id;

                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;

                            cmd_fee.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;
                            cmd_fee.Parameters.Add("@rem_exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;

                            con_fee.Open();
                            cmd_fee.ExecuteScalar();
                            con_fee.Close();
                        }
                    }
                }
                // Security Fee
                if (security > 0)
                {
                }
                else
                {
                    using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd_fee = new MySqlCommand())
                        {

                            cmd_fee.CommandText = "Update sms_fee SET security_fee=@security_fee, rem_security_fee=@rem_security_fee where std_id = @std_id && session_id="+MainWindow.session.id;

                            cmd_fee.Connection = con_fee;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                            cmd_fee.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;
                            cmd_fee.Parameters.Add("@rem_security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;

                            con_fee.Open();
                            cmd_fee.ExecuteScalar();
                            con_fee.Close();
                        }
                    }
                }

                // Set transport fee=0 in all months
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {

                        cmd_fee.CommandText = "Update sms_fee SET transport_fee=@transport_fee, rem_transport_fee=@rem_transport_fee where std_id = @std_id && session_id="+MainWindow.session.id;

                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;

                        cmd_fee.Parameters.Add("@transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";
                        cmd_fee.Parameters.Add("@rem_transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "0";

                        con_fee.Open();
                        cmd_fee.ExecuteScalar();
                        con_fee.Close();
                    }
                }

                // Student Attendence
                sections sec = (sections)section_cmb.SelectedItem;
                using (MySqlConnection con_fee = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_fee = new MySqlCommand())
                    {

                        cmd_fee.CommandText = "Update sms_student_attendence SET section_id=@section_id where std_id = @std_id && session_id=" + MainWindow.session.id;

                        cmd_fee.Connection = con_fee;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd_fee.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_fee.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value =sec.id ;                        

                        con_fee.Open();
                        cmd_fee.ExecuteScalar();
                        con_fee.Close();
                    }
                }
                
                
            }
            catch (Exception ex)
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

            adm_obj.roll_no = roll_textbox.Text.Trim();
            adm_obj.adm_no = adm_textbox.Text.Trim();

            adm_obj.reg_fee = reg_textbox.Text.Trim();
            adm_obj.adm_fee = admmission_textbox.Text.Trim();
            adm_obj.tution_fee = tutuion_textbox.Text.Trim();
            adm_obj.scholarship = "0";
            adm_obj.misc_charges = "0";
            adm_obj.exam_fee = exam_textbox.Text.Trim();
            adm_obj.security_fee = security_textbox.Text.Trim();
            adm_obj.stationary_fee = "0";
            adm_obj.transport_fee = "0";
            adm_obj.other_exp = other_textbox.Text.Trim();
            adm_obj.total = total_textbox.Text.Trim();


            adm_obj.date_time = DateTime.Now;
            adm_obj.created_by = MainWindow.emp_login_obj.emp_user_name;

            if (transport_yes.IsChecked == true)
            {
                adm_obj.transport = "Y";
            }
            else
            {
                adm_obj.transport = "N";
            }

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

            if (is_active_chekbox.IsChecked == true)
            {
                adm_obj.is_active = "Y";
            }
            else
            {
                adm_obj.is_active = "N";
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
            roll_textbox.Text = obj.roll_no;
            adm_textbox.Text = obj.adm_no;
            pre_school_textbox.Text = obj.previous_school;
            reg_textbox.Text = obj.reg_fee;
            admmission_textbox.Text = obj.adm_fee;
            tutuion_textbox.Text = obj.tution_fee;
            scholarship_textbox.Text = obj.scholarship;
            misc_textbox.Text = obj.misc_charges;
            exam_textbox.Text = obj.exam_fee;
            security_textbox.Text = obj.security_fee;
            stationary_textbox.Text = obj.stationary_fee;
            other_textbox.Text = obj.other_exp;
            transport_textbox.Text = obj.transport_fee;
            total_textbox.Text = obj.total;

            

            if (obj.transport == "Y")
            {
                transport_yes.IsChecked = true;
            }
            else
            {
                transport_no.IsChecked = true;
            }

            if (obj.boarding == "Y")
            {
                boarding_yes.IsChecked = true;
            }
            else
            {
                boarding_no.IsChecked = true;
            }

            if (obj.is_active == "Y")
            {
                is_active_chekbox.IsChecked = true;
            }
            else
            {
                is_active_chekbox.IsChecked = false;
            }
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
            else if (reg_textbox.Text.Trim().Length == 0)
            {
                reg_textbox.Focus();
                string alertText = "Registration Fee Should Not Be Blank";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (admmission_textbox.Text.Trim().Length == 0)
            {
                admmission_textbox.Focus();
                string alertText = "Admission Fee Should Not Be Blank";
                MessageBox.Show(alertText, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (tutuion_textbox.Text.Trim().Length == 0)
            {
                tutuion_textbox.Focus();
                string alertText = "Tution Fee Should Not Be Blank";
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

            bool a = Regex.IsMatch(adm_textbox.Text.Trim(), @"^\d+$");
            bool b = false;
            foreach(admission adm in AS.adm_list)
            {
                b = Regex.IsMatch(adm.adm_no, @"^\d+$");
                if (a && b)
                {
                    if (!string.IsNullOrWhiteSpace(adm.adm_no) || !string.IsNullOrEmpty(adm.adm_no))
                    {
                        if (Convert.ToInt32(adm.adm_no) == Convert.ToInt32(adm_textbox.Text.Trim()) && adm.id != obj.id)
                        {
                            check = true;
                        }
                    }
                }
                else 
                {
                    if (!string.IsNullOrWhiteSpace(adm.adm_no) || !string.IsNullOrEmpty(adm.adm_no))
                    {
                        if (adm.adm_no == adm_textbox.Text.Trim() && adm.id != obj.id)
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
                        foreach (classes c in classes_list)
                        {
                            if (c.id == cl_id)
                            {
                                reg_textbox.Text = c.reg_fee;
                                admmission_textbox.Text = c.adm_fee;
                                tutuion_textbox.Text = c.tution_fee;
                                misc_textbox.Text = "0";
                                exam_textbox.Text = c.exam_fee;
                                security_textbox.Text = c.security_fee;
                                stationary_textbox.Text = "0";
                                other_textbox.Text = c.other_exp;
                                transport_textbox.Text = "0";

                                total();
                                break;
                            }
                        }             
                    }                    
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

        //==============           Total                ===========================
        public void total() 
        {

            total_fee = 0;

            adm_fee=0;
            tutuion_fee=0;
            mis_fee=0;
            exam_fee = 0;
            sec_fee = 0;
            stat_fee = 0;
            transport_fee = 0;
            other_exp = 0;
            scholarship = 0;

            try
            {
                if (reg_textbox.Text.Trim() == "" || reg_textbox == null)
                {
                    reg_textbox.Text = "0";
                }
                if (admmission_textbox.Text.Trim() == "" || admmission_textbox == null)
                {
                    admmission_textbox.Text = "0";
                }
                if (tutuion_textbox.Text.Trim() == "" || tutuion_textbox == null)
                {
                    tutuion_textbox.Text = "0";
                }
                if (exam_textbox.Text.Trim() == "" || exam_textbox == null)
                {
                    exam_textbox.Text = "0";
                }
                if (security_textbox.Text.Trim() == "" || security_textbox == null)
                {
                    security_textbox.Text = "0";
                }
                if (transport_textbox.Text.Trim() == "" || transport_textbox == null)
                {
                    transport_textbox.Text = "0";
                }
                if (other_textbox.Text.Trim() == "" || other_textbox == null)
                {
                    other_textbox.Text = "0";
                }
                if (stationary_textbox.Text.Trim() == "" || stationary_textbox == null)
                {
                    stationary_textbox.Text = "0";
                }
                if (scholarship_textbox.Text.Trim() == "" || scholarship_textbox == null)
                {
                    scholarship_textbox.Text = "0";
                }
                if (misc_textbox.Text.Trim() == "" || misc_textbox == null)
                {
                    misc_textbox.Text = "0";
                }

                reg_fee = Convert.ToInt32(reg_textbox.Text.Trim());
                adm_fee = Convert.ToInt32(admmission_textbox.Text.Trim());
                tutuion_fee = Convert.ToInt32(tutuion_textbox.Text.Trim());
                mis_fee = Convert.ToInt32(misc_textbox.Text.Trim());
                exam_fee = Convert.ToInt32(exam_textbox.Text.Trim());
                sec_fee = Convert.ToInt32(security_textbox.Text.Trim());
                stat_fee = Convert.ToInt32(stationary_textbox.Text.Trim());
                transport_fee = Convert.ToInt32(transport_textbox.Text.Trim());
                other_exp = Convert.ToInt32(other_textbox.Text.Trim());
                scholarship = Convert.ToInt32(scholarship_textbox.Text.Trim());
            }
            catch(Exception ex) 
            {
            }

            total_fee = reg_fee + adm_fee + tutuion_fee + mis_fee + exam_fee + sec_fee + stat_fee + transport_fee + other_exp - scholarship;
            if (total_fee != 0)
            {
                total_textbox.Text = total_fee.ToString();
            }
            else 
            {
                total_textbox.Text = "0";
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

        //==================         All text boxed selelction changed                =============================================================

        private void reg_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void admmission_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void tutuion_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void scholarship_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void misc_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void exam_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void security_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void stationary_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void transport_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
        }

        private void other_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            total();
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


        //=========================   insert in fees     =======================
        public void insert_fee()
        {           
        }

        private void camera_btn_Click(object sender, RoutedEventArgs e)
        {            
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
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Withdrawal This Student ?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                GetDateRemarksWindow GDRW = new GetDateRemarksWindow(this);
                GDRW.ShowDialog();
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
            if (obj.boarding == "Y")
            {
                obj.gender = "Male";
            }
            else 
            {
                obj.gender = "Female";
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
    }
}
