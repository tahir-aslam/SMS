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
using SMS.EmployeeManagement.EmpLogin;
using MySql.Data.MySqlClient;
using System.Windows.Markup;
using SUT.PrintEngine.Utils;
using System.Data;
using System.Collections.ObjectModel;
using SMS.DAL;


namespace SMS.EmployeeManagement.EmpRoles
{
    /// <summary>
    /// Interaction logic for EmployeeRolesForm.xaml
    /// </summary>
    public partial class EmployeeRolesForm : Window
    {
        roles role_obj;
        roles child_obj;
        emp_login obj;
        List<roles> roles_list;
        List<roles> roles_assignment_list;
        ObservableCollection<roles> oc;
        List<roles> save_list;
        RolesDAL rolesDAL;

        public EmployeeRolesForm(string mode, EmployeeRolesSearch ERS, emp_login obj)
        {
            InitializeComponent();
            this.obj = obj;            
            get_all_roles_assignment();
            rolesDAL = new RolesDAL();
            roles_list = rolesDAL.get_all_roles();
            populate_treeview();            
        }

        public void populate_treeview() 
        {
            oc = new ObservableCollection<roles>();
            foreach(roles rol in roles_list.Where(x=>x.is_active == "Y"))
            {
                if(rol.module_pid=="0")
                {
                    role_obj = new roles();
                    role_obj = rol;
                    if(check_role_assignment(role_obj))
                    {
                        role_obj.Checked = true;
                    }
                    role_obj.children = new ObservableCollection<roles>();
                    foreach(roles r in roles_list)
                    {
                        if(rol.id == r.module_pid)
                        {
                            if (check_role_assignment(r))
                            {
                                r.Checked = true;
                            }
                            role_obj.children.Add(r);
                            
                        }
                    }
                    oc.Add(role_obj);
                }
                
            }
            this.DataContext = oc;
        }

        public bool check_role_assignment(roles r) 
        {
            foreach(roles rol in roles_assignment_list.Where(x=>x.module_id == r.id))
            {
                return true;
                
            }
            return false;
        }

        //-------Get All Roles Assignment ---------
        public void get_all_roles_assignment()
        {
            roles_assignment_list = new List<roles>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from sms_roles_assignment where emp_id ="+obj.emp_id;
                        cmd.Parameters.Add("id",MySqlDbType.VarChar).Value= obj.emp_id;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            roles rol = new roles()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                module_name = Convert.ToString(reader["module_name"]),
                                module_pid = Convert.ToString(reader["module_pid"]),
                                module_id = Convert.ToString(reader["module_id"]),
                            };
                            roles_assignment_list.Add(rol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        //--------- Checked------------
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            int count = 0;
            if (null == checkBox) return;

            //foreach (roles rol in oc)
            //{
            //    count = 0;
            //    foreach (roles r in rol.children)
            //    {
            //        if (r.Checked == true)
            //        {
            //            count++;
            //        }
            //    }
            //    if (rol.children.Count == count)
            //    {
            //        rol.Checked = true;
            //    }
            //    else
            //    {
            //        rol.Checked = false;
            //    }

            //}

            //foreach (roles rol in oc)
            //{
            //    if (rol.Checked == true)
            //    {
            //        foreach (roles r in rol.children)
            //        {
            //            if (r.Checked == true) 
            //            {
            //                r.Checked = checkBox.IsChecked.Value;
            //            }
                        
            //        }
            //    }
            //}
            //rolestreeView.Items.Refresh();
        }        

        private void save_btn_Click(object sender, RoutedEventArgs e)
        {
            fill_object();
            save();
        }

        public void fill_object()
        {
            save_list = new List<roles>();
            bool check = false;

            foreach(roles rol in oc)
            {
                check = false;
                    foreach(roles r in rol.children)
                    {                       
                        if(r.Checked == true)
                        {
                            role_obj = new roles();
                            role_obj = r;
                            role_obj.module_id = r.id;
                            role_obj.emp_id = obj.emp_id;
                            role_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                            role_obj.date_time = DateTime.Now;
                            save_list.Add(role_obj);
                            check = true;
                        }
                    }
                    if (check == true)
                    {
                        role_obj = new roles();
                        role_obj = rol;
                        role_obj.module_id = rol.id;
                        role_obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                        role_obj.emp_id = obj.emp_id;
                        role_obj.date_time = DateTime.Now;
                        save_list.Add(role_obj);
                    }
                
            }
        }

        public void save()
        {
            delete();   
                if (submit() > 0)
                {
                    MessageBox.Show("Record Added Successfully", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("All Modules Should not be empty.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            
        }

        //-------------     Delete          ---------------------------
        public int delete()
        {
            int i = 0;
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "Delete from sms_roles_assignment where emp_id=" + obj.emp_id;
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

        //---------------           Submit Form    ----------------------------------
        public int submit()
        {
            int i = 0;
            try
            {
                foreach (roles r in save_list)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {

                            cmd.CommandText = "INSERT INTO sms_roles_assignment(emp_id,module_id,module_name,module_pid,created_by,date_time) Values(@emp_id,@module_id,@module_name,@module_pid,@created_by,@date_time)";
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = r.emp_id;
                            cmd.Parameters.Add("@module_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = r.module_id;
                            cmd.Parameters.Add("@module_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = r.module_name;
                            cmd.Parameters.Add("@module_pid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = r.module_pid;
                            cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                            cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = r.date_time;

                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            con.Close();

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

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

  
    }
}
