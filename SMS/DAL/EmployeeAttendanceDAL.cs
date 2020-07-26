using MySql.Data.MySqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
    class EmployeeAttendanceDAL
    {
        public List<employee_attendence> GetStudentAttendanceByDate(DateTime sDate, DateTime eDate, List<employees> lst)
        {
            string ids = string.Join(",", lst.Select(x => x.id.ToString()));
            List<employee_attendence> attendance_list = new List<employee_attendence>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT att.emp_id, att.attendence_date, att.attendence, att.total_days, att.total_abs, att.total_presents, att.total_leaves, att.att_percentage, att.date_time, " +
                                            "emp.emp_name, emp.emp_father, emp.emp_type_id, emp.emp_type FROM sms_emp_attendence AS att " +
                                            "INNER JOIN sms_emp AS emp ON att.emp_id = emp.id "+
                                            "where(DATE(att.attendence_date) >= @sDate && DATE(att.attendence_date) <= @eDate) && att.emp_id IN(" + ids + ")";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@ids", MySqlDbType.VarChar).Value = ids;
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Parameters.Add("@session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employee_attendence att = new employee_attendence()
                            {
                                emp_id = Convert.ToString(reader[0]),
                                attendence_date = Convert.ToDateTime(reader[1]),
                                attendence = Convert.ToChar(reader[2]),
                                total_days = Convert.ToString(reader[3]),
                                total_abs = Convert.ToString(reader[4]),
                                total_presents = Convert.ToString(reader[5]),
                                total_leaves = Convert.ToString(reader[6]),
                                att_percentage = Convert.ToString(reader[7]),
                                date_time = Convert.ToDateTime(reader[8]),
                                emp_name = Convert.ToString(reader[9]),
                                emp_father = Convert.ToString(reader[10]), 
                                emp_type_id = Convert.ToString(reader[11]),
                                emp_type = Convert.ToString(reader[12]),
                                
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
    }
}
