using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.DAL
{
    public class EmployeesDAL
    {
        public List<emp_login> get_all_emp_login()
        {
            List<emp_login> emp_login_list = new List<emp_login>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_emp_login";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            emp_login el = new emp_login()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_user_name = Convert.ToString(reader["emp_user_name"].ToString()),
                                emp_pwd = Convert.ToString(reader["emp_pwd"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };                            
                            emp_login_list.Add(el);
                            
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }

            return emp_login_list;
        }

        public List<employee_attendence> get_emp_attendance_by_month(int emp_id, int month, int year) 
        {
            List<employee_attendence> attendance_list = new List<employee_attendence>();                                
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_emp_attendence where emp_id=@emp_id && Month(attendence_date)=@month && Year(attendence_date)=@year ORDER BY attendence_date DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@month",MySqlDbType.Int32).Value=month;
                        cmd.Parameters.Add("@year", MySqlDbType.Int32).Value = year;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = emp_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {               
                           
                            employee_attendence att = new employee_attendence()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),                                
                                att_percentage = Convert.ToString(reader["att_percentage"].ToString()),
                                total_days = Convert.ToString(reader["total_days"].ToString()),
                                total_abs = Convert.ToString(reader["total_abs"].ToString()),
                                total_presents = Convert.ToString(reader["total_presents"].ToString()),
                                attendence = Convert.ToChar(reader["attendence"]),
                                attendence_date = Convert.ToDateTime(reader["attendence_date"]),

                            };
                            attendance_list.Add(att);                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
            return attendance_list;
        }

        public List<employees> get_all_active_employees()
        {
            List<employees> emp_list = new List<employees>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp as emp Inner Join sms_emp_title as title on emp.emp_title_id=title.id Inner join sms_emp_designation as designation on emp.emp_designation_id= designation.id  where emp.is_active='Y' Order by emp.emp_type_id ASC";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_father = Convert.ToString(reader["emp_father"].ToString()),
                                emp_nationality = Convert.ToString(reader["emp_nationality"].ToString()),
                                emp_religion = Convert.ToString(reader["emp_religion"].ToString()),
                                emp_exp = Convert.ToString(reader["emp_exp"].ToString()),
                                emp_cnic = Convert.ToString(reader["emp_cnic"].ToString()),
                                emp_qual = Convert.ToString(reader["emp_qual"].ToString()),
                                emp_sex = Convert.ToString(reader["emp_sex"].ToString()),
                                emp_marital = Convert.ToString(reader["emp_marital"].ToString()),
                                emp_dob = Convert.ToDateTime(reader["emp_dob"]),
                                joining_date = Convert.ToDateTime(reader["joining_date"]),
                                emp_email = Convert.ToString(reader["emp_email"].ToString()),
                                emp_address = Convert.ToString(reader["emp_address"].ToString()),
                                emp_remarks = Convert.ToString(reader["emp_remarks"].ToString()),
                                emp_pay = Convert.ToString(reader["emp_pay"].ToString()),
                                emp_cell = Convert.ToString(reader["emp_cell"].ToString()),
                                emp_phone = Convert.ToString(reader["emp_phone"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                                title_id = Convert.ToInt32(reader["emp_title_id"].ToString()),
                                title = Convert.ToString(reader["title"].ToString()),
                                designation_id = Convert.ToInt32(reader["emp_designation_id"]),
                                designation = Convert.ToString(reader["designation"]),
                                image = (byte[])reader["image"],                                
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                            };
                            
                            emp_list.Add(emp);
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return emp_list;
        }

        public List<employees> get_salary_sheet(int month, int year)
        {
            double one_day_salary = 0;
            double deduction = 0;            
            List<employees> emp_list = get_all_active_employees();

            foreach (var emp in emp_list)
            {
                try
                {
                    one_day_salary = 0;
                    deduction = 0;
                    List<employee_attendence> attendance_list = get_emp_attendance_by_month(Convert.ToInt32(emp.id), month, year);
                    emp.total_absents = 0;
                    emp.total_absents = attendance_list.Where(x => x.attendence == 'A').Count();
                    emp.total_days = attendance_list.Count();
                    one_day_salary = Convert.ToDouble(emp.emp_pay) / 30;
                    deduction = one_day_salary * emp.total_absents;
                    emp.deduction_amount = deduction;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }          
            return emp_list;
        }

        public List<sms_emp_title> get_all_employee_title()
        {
            List<sms_emp_title> emp_list = new List<sms_emp_title>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp_title where is_active='Y' Order by sort_order ASC";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_emp_title emp = new sms_emp_title()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                title = Convert.ToString(reader["title"]),
                                sort_order = Convert.ToInt32(reader["sort_order"]),
                                is_active = Convert.ToString(reader["is_active"]),
                            };
                            emp_list.Add(emp);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return emp_list;
        }

        public List<sms_emp_designation> get_all_employee_designation()
        {
            List<sms_emp_designation> emp_list = new List<sms_emp_designation>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_emp_designation where is_active='Y' Order by sort_order ASC";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_emp_designation emp = new sms_emp_designation()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                designation = Convert.ToString(reader["designation"]),
                                sort_order = Convert.ToInt32(reader["sort_order"]),
                                is_active = Convert.ToString(reader["is_active"]),
                            };
                            emp_list.Add(emp);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return emp_list;
        }

        
    }
}
