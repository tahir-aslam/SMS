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
using SMS.EmployeeManagement.ADDEMP;
using SMS.Models;
using MySql.Data.MySqlClient;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using SMS.DAL;
using System.IO;
using SMS.AdmissionManagement.Camera;

namespace SMS.EmployeeManagement.ADDEMP
{
    /// <summary>
    /// Interaction logic for AddEmpForm.xaml
    /// </summary>
    public partial class AddEmpForm : Window
    {
        ADDEMPSEARCH AES;
        List<employees> emp_list;
        List<employees_types> emp_types_list;
        List<sms_emp_title> emp_title_list;
        List<sms_emp_designation> emp_designation_list;
        EmployeesDAL empDAL;
        
        employees emp_obj;
        employees obj;
        string mode;
        public DateTime leaving_date;
        public bool isYes = false;

        public string FileName = "";
        bool check_filedialog;

        public AddEmpForm(string m, ADDEMPSEARCH aes, employees ob)
        {
            InitializeComponent();

            AES = aes;
            this.obj = ob;
            this.mode = m;
            empDAL = new EmployeesDAL();

            emp_types_cmb.Focus();

            emp_list = new List<employees>();
            emp_types_list = new List<employees_types>();
            emp_title_list = new List<sms_emp_title>();

            get_all_emp();
            get_all_emp_types();

            
            emp_types_cmb.SelectedIndex = 0;                       
            emp_types_list.Insert(0, new employees_types() { emp_types = "---Select Department---", id = "-1" });
            emp_types_cmb.ItemsSource = emp_types_list;

            emp_title_list = empDAL.get_all_employee_title();
            emp_title_cmb.SelectedIndex = 0;
            emp_title_list.Insert(0, new sms_emp_title() { title = "---Select Title---", id = -1 });
            emp_title_cmb.ItemsSource = emp_title_list;

            emp_designation_list = empDAL.get_all_employee_designation();
            emp_designation_cmb.SelectedIndex = 0;
            emp_designation_list.Insert(0, new sms_emp_designation() { designation = "---Select Designation---", id = -1 });
            emp_designation_cmb.ItemsSource = emp_designation_list;

            nationality_textbox.Text = "Pakistani";
            religion_textbox.Text = "Islam";
            joining_textbox.SelectedDate = DateTime.Now;
            
            if (mode == "edit")
            {
                
                fill_control();
            }
        }


        //--------------           Save          ----------------------

        private void click_save(object sender, RoutedEventArgs e)
        {
            save();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                save();
            }
        }

        public void save()
        {
            
            if (validate())
            {
                fill_object();
                if (mode == "insert")
                {
                    //if (check_emp_name() == false)
                    //{
                    //    if (check_principal() == false)
                    //    {
                    //        if (check_vprincipal() == false)
                    //        {
                                if (submit() > 0)
                                {
                                    AES.load_grid();
                                    MessageBox.Show("Record Added Successfully");
                                    this.Close();

                                }
                                else
                                {
                                    MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                                }
                    //        }
                    //        else 
                    //        {
                    //            MessageBox.Show("Vice Principal Already Exist, Please Choose Another Designation");
                    //        }
                    //    }
                    //    else 
                    //    {
                    //        MessageBox.Show("Principal Already Exist, Please Choose Another Designation");
                    //    }
                    //}
                    //else 
                    //{
                    //    MessageBox.Show("Employee Name Already Exists, Please Choose A Different Name");
                    //    full_name_textbox.Focus();
                    //}
                    
                }
                else if (mode == "edit")
                {
                    //if (check_emp_name_editing() == false)
                    //{
                    //    if (check_principal() == false)
                    //    {
                    //        if (check_vprincipal() == false)
                    //        {
                                if (update() == 0)
                                {
                                    AES.load_grid();
                                    MessageBox.Show("Record Updated Successfully");
                                    this.Close();

                                }

                                else
                                {
                                    MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                                }
                    //        }
                    //        else 
                    //        {
                    //            MessageBox.Show("Vice Principal Already Exist, Please Choose Another Designation");
                    //        }
                    //    }
                    //    else 
                    //    {
                    //        MessageBox.Show("Principal Already Exist, Please Choose Another Designation");
                    //    }
                    //}
                    //else 
                    //{
                    //    MessageBox.Show("Employee Name Already Exists, Please Choose A Different Name");
                    //    full_name_textbox.Focus();
                    //}

                }
                else
                {
                    MessageBox.Show("mode not set");
                }

            }
        }        

        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //---------------           Submit Form    ----------------------------------
        public int submit()
        {
            int i = 0;
            byte[] ImageData;
            FileStream fs;
            BinaryReader br;

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
                    if (male.IsChecked == true)
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
                        cmd.CommandText = "INSERT INTO sms_emp(emp_title_id, emp_designation_id, image, emp_type_id,emp_type,emp_name,emp_father,emp_nationality,emp_religion,emp_exp,emp_cnic,emp_qual,emp_sex,emp_marital,emp_dob,emp_email,emp_address,emp_remarks,emp_phone,emp_cell,emp_pay,is_active,created_by,date_time,joining_date) Values(@emp_title_id, @emp_designation_id, @image, @emp_type_id,@emp_type,@emp_name,@emp_father,@emp_nationality,@emp_religion,@emp_exp,@emp_cnic,@emp_qual,@emp_sex,@emp_marital,@emp_dob,@emp_email,@emp_address,@emp_remarks,@emp_phone,@emp_cell,@emp_pay,@is_active,@created_by,@date_time,@joining_date)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@emp_title_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.title_id;
                        cmd.Parameters.Add("@emp_designation_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.designation_id;
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;
                        cmd.Parameters.Add("@emp_type_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type_id;
                        cmd.Parameters.Add("@emp_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_name;
                        cmd.Parameters.Add("@emp_father", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_father;
                        cmd.Parameters.Add("@emp_nationality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_nationality;
                        cmd.Parameters.Add("@emp_religion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_religion;
                        cmd.Parameters.Add("@emp_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_exp;
                        cmd.Parameters.Add("@emp_cnic", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cnic;
                        cmd.Parameters.Add("@emp_qual", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_qual;
                        cmd.Parameters.Add("@emp_sex", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_sex;
                        cmd.Parameters.Add("@emp_marital", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_marital;
                        cmd.Parameters.Add("@emp_dob", MySql.Data.MySqlClient.MySqlDbType.Date).Value = emp_obj.emp_dob;
                        cmd.Parameters.Add("@joining_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = emp_obj.joining_date;
                        cmd.Parameters.Add("@emp_email", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_email;
                        cmd.Parameters.Add("@emp_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_address;
                        cmd.Parameters.Add("@emp_remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_remarks;
                        cmd.Parameters.Add("@emp_phone", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_phone;
                        cmd.Parameters.Add("@emp_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cell;
                        cmd.Parameters.Add("@emp_pay", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_pay;

                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = emp_obj.date_time;
                        


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

        //---------------           Update Form        ---------------------------------
        public int update()
        {
            int i = 1;
            byte[] ImageData;
            FileStream fs;
            BinaryReader br;

            try
            {
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
                    if (obj.image == null)
                    {
                        if (obj.emp_sex == "Male")
                        {
                            ImageData = MainWindow.ins.male_image;
                        }
                        else
                        {
                            ImageData = MainWindow.ins.female_image;
                        }
                    }
                    else
                    {
                        ImageData = obj.image;
                    }                    
                }

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_emp SET emp_title_id=@emp_title_id, emp_designation_id=@emp_designation_id, image=@image, emp_type_id=@emp_type_id,emp_type=@emp_type,emp_name=@emp_name,emp_father=@emp_father,emp_nationality=@emp_nationality,emp_religion=@emp_religion,emp_exp=@emp_exp,emp_cnic=@emp_cnic,emp_qual=@emp_qual,emp_sex=@emp_sex,emp_marital=@emp_marital,emp_dob=@emp_dob,emp_email=@emp_email,emp_address=@emp_address,emp_remarks=@emp_remarks,emp_phone=@emp_phone,emp_cell=@emp_cell,emp_pay=@emp_pay,is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation,joining_date=@joining_date WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@emp_title_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.title_id;
                        cmd.Parameters.Add("@emp_designation_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.designation_id;
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = ImageData;
                        cmd.Parameters.Add("@emp_type_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type_id;
                        cmd.Parameters.Add("@emp_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_name;
                        cmd.Parameters.Add("@emp_father", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_father;
                        cmd.Parameters.Add("@emp_nationality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_nationality;
                        cmd.Parameters.Add("@emp_religion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_religion;
                        cmd.Parameters.Add("@emp_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_exp;
                        cmd.Parameters.Add("@emp_cnic", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cnic;
                        cmd.Parameters.Add("@emp_qual", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_qual;
                        cmd.Parameters.Add("@emp_sex", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_sex;
                        cmd.Parameters.Add("@emp_marital", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_marital;
                        cmd.Parameters.Add("@emp_dob", MySql.Data.MySqlClient.MySqlDbType.Date).Value = emp_obj.emp_dob;
                        cmd.Parameters.Add("@joining_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = emp_obj.joining_date;
                        cmd.Parameters.Add("@emp_email", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_email;
                        cmd.Parameters.Add("@emp_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_address;
                        cmd.Parameters.Add("@emp_remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_remarks;
                        cmd.Parameters.Add("@emp_phone", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_phone;
                        cmd.Parameters.Add("@emp_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cell;
                        cmd.Parameters.Add("@emp_pay", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_pay;

                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = emp_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteScalar());
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
            emp_obj = new employees();

            employees_types type = (employees_types)emp_types_cmb.SelectedItem;
            string type_id = type.id.ToString();
            string type_name = type.emp_types.ToString();

            emp_obj.title_id =  (emp_title_cmb.SelectedItem as sms_emp_title).id;
            emp_obj.designation_id = (emp_designation_cmb.SelectedItem as sms_emp_designation).id;
            emp_obj.emp_type_id = type_id;
            emp_obj.emp_type = type_name;
            emp_obj.emp_name = full_name_textbox.Text.Trim();
            emp_obj.emp_father = fname_textbox.Text.Trim();
            emp_obj.emp_nationality = nationality_textbox.Text.Trim();
            emp_obj.emp_religion = religion_textbox.Text.Trim();
            emp_obj.emp_exp = exp_textbox.Text.Trim();
            emp_obj.emp_cnic = cnic_textbox.Text.Trim();
            emp_obj.emp_qual = qual_textbox.Text.Trim();
            if(dob_textbox.SelectedDate != null)
            {
                emp_obj.emp_dob = dob_textbox.SelectedDate.Value;
            }
            emp_obj.joining_date = joining_textbox.SelectedDate.Value;
            emp_obj.emp_email = email_textbox.Text.Trim();
            emp_obj.emp_address = address_textbox.Text.Trim();
            emp_obj.emp_remarks = remarks_textbox.Text.Trim();
            emp_obj.emp_pay = pay_textbox.Text.Trim();
            emp_obj.emp_cell = cell_textbox.Text.Trim();
            emp_obj.emp_phone = phone_textbox.Text.Trim();

            emp_obj.date_time = DateTime.Now;
            emp_obj.created_by = MainWindow.emp_login_obj.emp_user_name;



            if (single.IsChecked == true)
            {
                emp_obj.emp_marital = "Single";
            }
            else 
            {
                emp_obj.emp_marital = "Married";
            }

            if(male.IsChecked == true)
            {
                emp_obj.emp_sex="Male";
            }
            else
            {
                emp_obj.emp_sex="Female";
            }


            if (is_active_chekbox.IsChecked == true)
            {
                emp_obj.is_active = "Y";
            }
            else
            {
                emp_obj.is_active = "N";
            }
        }

        //------------------    Fill Control     -------------------------

        public void fill_control()
        {
            if(obj.image.Length == 0)
            {
                if (obj.emp_sex == "Male")
                {
                    obj.image = MainWindow.ins.male_image;
                }
                else 
                {
                    obj.image = MainWindow.ins.female_image;
                }
            }

            MemoryStream stream = new MemoryStream(obj.image);
            emp_image.Source = ByteToImage(obj.image);

            emp_designation_cmb.SelectedValue = obj.designation_id;
            emp_title_cmb.SelectedValue = obj.title_id;
            emp_types_cmb.SelectedValue = obj.emp_type_id;
            full_name_textbox.Text = obj.emp_name;
            fname_textbox.Text = obj.emp_father;
            cnic_textbox.Text = obj.emp_cnic;
            nationality_textbox.Text = obj.emp_nationality;
            religion_textbox.Text = obj.emp_religion;
            exp_textbox.Text = obj.emp_exp;
            qual_textbox.Text = obj.emp_qual;
            dob_textbox.SelectedDate = obj.emp_dob;
            joining_textbox.SelectedDate = obj.joining_date;
            email_textbox.Text = obj.emp_email;
            address_textbox.Text = obj.emp_address;
            remarks_textbox.Text = obj.emp_remarks;
            phone_textbox.Text = obj.emp_phone;
            cell_textbox.Text = obj.emp_cell;
            pay_textbox.Text = obj.emp_pay;

            if (obj.emp_sex == "Male")
            {
                male.IsChecked = true;
            }
            else 
            {
                female.IsChecked = true;
            }

            if (obj.emp_marital == "Single")
            {
                single.IsChecked = true;
            }
            else 
            {
                married.IsChecked = true;
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
            if (emp_title_cmb.SelectedIndex == 0)
            {
                emp_title_cmb.Focus();
                string alertText = "Employee Title Should Not Be Blank.";
                MessageBox.Show(alertText);
                return false;
            }      
            
            else if (full_name_textbox.Text.Trim().Length == 0)
            {
                full_name_textbox.Focus();
                string alertText = "Full Name Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (fname_textbox.Text.Trim().Length == 0)
            {
                fname_textbox.Focus();
                string alertText = "Father Name Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }

            else if (emp_types_cmb.SelectedIndex == 0)
            {
                emp_types_cmb.Focus();
                string alertText = "Employee Department Should Not Be Blank.";
                MessageBox.Show(alertText);
                return false;
            }

            else if (emp_designation_cmb.SelectedIndex == 0 )
            {
                emp_designation_cmb.Focus();
                string alertText = "Employee Designation Should Not Be Blank.";
                MessageBox.Show(alertText);
                return false;
            }
            else if (dob_textbox.SelectedDate==null)
            {
                dob_textbox.Focus();
                string alertText = "Date Of Birth Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (cell_textbox.Text.Trim().Length == 0)
            {
                cell_textbox.Focus();
                string alertText = "Cell # Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            else if (pay_textbox.Text.Trim().Length == 0)
            {
                pay_textbox.Focus();
                string alertText = "Pay Should Not Be Blank";
                MessageBox.Show(alertText);
                return false;
            }
            

            else
            {
                return true;
            }

        }

        //---------------           Get All Employees    ----------------------------------

        public void get_all_emp()
        {
            try
            {

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_emp ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                            };
                            emp_list.Add(emp);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Teacher DB not connected");
            }
        }

        //---------------           Get All Employees types    ----------------------------------

        public void get_all_emp_types()
        {
            try
            {
                emp_types_list = new List<employees_types>();
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                                               
                        cmd.CommandText = "SELECT* FROM sms_emp_types";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees_types emp_types = new employees_types()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_types = Convert.ToString(reader["emp_types"].ToString()),
                            };
                            emp_types_list.Add(emp_types);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Employees Types DB not connected");
            }
        }

        //private void upload_image_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog op = new OpenFileDialog();
        //    op.Title = "Select a picture";
        //    op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        //        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        //        "Portable Network Graphic (*.png)|*.png";
        //    if (op.ShowDialog() == true)
        //    {
        //        string a = op.FileName;
        //        MessageBox.Show(a);
        //    }
        //}

        //------------           Check Section Name   -------------------

        bool check_emp_name()
        {
            foreach (employees emp in emp_list)
            {
                if (emp.emp_name.ToString().ToUpper().Equals(emp_obj.emp_name.ToString().ToUpper()))
                {
                    return true;

                }
            }
            return false;
        }

        bool check_emp_name_editing()
        {
            foreach (employees emp in emp_list)
            {
                if (obj.id != emp.id)
                {
                    if (emp.emp_name.ToString().ToUpper().Equals(emp_obj.emp_name.ToString().ToUpper()))
                    {
                        return true;

                    }
                }
            }
            return false;
        }

        bool check_principal()
        {
            foreach (employees emp in emp_list)
            {
                if (emp.emp_type_id == "1")
                {
                    if (emp_obj.emp_type_id == emp.emp_type_id)
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        bool check_vprincipal()
        {
            foreach (employees emp in emp_list)
            {
                if (emp.emp_type_id == "2")
                {
                    if (emp_obj.emp_type_id == emp.emp_type_id)
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        // -------------------      check principal and vice principal editing       -----------------------------
        bool check_principal_editing()
        {
            foreach (employees emp in emp_list)
            {
                if (emp.emp_type_id == "1" && emp_obj.id!= emp.id)
                {
                    if (emp_obj.emp_type_id == emp.emp_type_id)
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        bool check_vprincipal_editing()
        {
            foreach (employees emp in emp_list )
            {
                if (emp.emp_type_id == "2" && emp_obj.id != emp.id)
                {
                    if (emp_obj.emp_type_id == emp.emp_type_id)
                    {

                        return true;
                    }
                }
            }
            return false;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void withdraw_btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are You Want To Withdraw This Employee ?", "Withdraw Confirmation", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                GetDateWindow gdw = new GetDateWindow(this);
                gdw.ShowDialog();
                if (isYes)
                {                    
                    update_withdraw();
                    AES.load_grid();
                    MessageBox.Show("Employee Withdraw Successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else 
                {
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
                        cmd.CommandText = "Update sms_emp SET is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation,leaving_date=@leaving_date WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd.Parameters.Add("@leaving_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = leaving_date;

                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "N";
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "true";

                        con.Open();
                        cmd.ExecuteScalar();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.Filter = "Image files | *.jpg; *.jpeg; *.png;";

                if (openFileDialog1.ShowDialog() == true)
                {
                    FileName = openFileDialog1.FileName;
                    emp_image.Source = new BitmapImage(new Uri(openFileDialog1.FileName));


                    check_filedialog = true;
                }

            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void camera_btn_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {               
                CameraEngineWindow cew = new CameraEngineWindow(this);
                cew.ShowDialog();
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
    }
}
