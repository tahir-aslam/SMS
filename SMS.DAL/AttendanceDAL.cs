using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.DAL
{
    public class AttendanceDAL
    {
        public List<student_attendence> GetStudentAttendanceByDate(DateTime sDate, DateTime eDate, List<admission> lst) 
        {
            string ids = string.Join(",", lst.Select(x=>x.id.ToString()));
            List<student_attendence> attendance_list = new List<student_attendence>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT att.std_id, att.attendence_date, att.attendence, att.total_days, att.total_abs, att.total_presents, att.total_leaves, att.att_percentage, att.date_time," +
                            "adm.class_id, adm.class_name, adm.section_id, adm.section_name, adm.std_name, adm.father_name, adm.adm_no, adm.roll_no, adm.adm_no_int, adm.roll_no_int  FROM sms_student_attendence AS att " +
                            "INNER JOIN sms_admission AS adm ON att.std_id = adm.id && adm.session_id=@session_id " +
                            "where (DATE(att.attendence_date) >= @sDate && DATE(att.attendence_date) <= @eDate) && att.std_id IN("+ids+")";
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
                            student_attendence att = new student_attendence()
                            {                                
                                std_id = Convert.ToString(reader[0]),
                                attendence_date = Convert.ToDateTime(reader[1]),
                                attendence = Convert.ToChar(reader[2]),                                
                                total_days = Convert.ToString(reader[3]),
                                total_abs = Convert.ToString(reader[4]),
                                total_presents = Convert.ToString(reader[5]),
                                total_leaves = Convert.ToString(reader[6]),
                                att_percentage = Convert.ToString(reader[7]),
                                date_time = Convert.ToDateTime(reader[8]),

                                class_id = Convert.ToString(reader[9]),
                                class_name = Convert.ToString(reader[10]),
                                section_id = Convert.ToString(reader[11]),
                                section_name = Convert.ToString(reader[12]),                      
                                std_name = Convert.ToString(reader[13]),
                                father_name = Convert.ToString(reader[14]),
                                adm_no = Convert.ToString(reader[15]),
                                roll_no = Convert.ToString(reader[16]),
                                adm_no_int = Convert.ToInt32(reader[17]),
                                roll_no_int = Convert.ToInt32(reader[18]),
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

        public List<student_attendence> getStudentAttendanceByDate(DateTime sDate, DateTime eDate)
        {
            List<student_attendence> attendance_list = new List<student_attendence>();
            student_attendence std;           
            try
            {                
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        
                        cmd.CommandText = "select count(*) as total, att.attendence,att.std_id,adm.std_name,adm.adm_no,adm.father_name,adm.class_id,adm.class_name,adm.section_id,adm.section_name, "+
                                            "att.attendence_date,att.total_days,att.total_abs,att.total_presents,att.total_leaves,att.att_percentage,adm.roll_no " +
                                           "from sms_student_attendence as att Inner join sms_admission as adm on adm.id=att.std_id && adm.session_id=att.session_id "+
                                           "where att.session_id=@session_id && DATE(att.attendence_date) >= @sDate && DATE(att.attendence_date) <= @eDate " +
                                           "group by attendence,std_id ";
                        cmd.Connection = con;                        
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            std = new student_attendence()
                            {
                                total_attendance = Convert.ToInt32(reader[0]),
                                attendence = Convert.ToChar(reader[1]),
                                std_id = Convert.ToString(reader[2]),
                                std_name = Convert.ToString(reader[3]),
                                adm_no = Convert.ToString(reader[4]),
                                father_name = Convert.ToString(reader[5]),
                                class_id = Convert.ToString(reader[6]),
                                class_name = Convert.ToString(reader[7]),
                                section_id = Convert.ToString(reader[8]),
                                section_name = Convert.ToString(reader[9]),
                                attendence_date = Convert.ToDateTime(reader[10]),
                                total_days = Convert.ToString(reader[11]),
                                total_abs = Convert.ToString(reader[12]),
                                total_presents = Convert.ToString(reader[13]),
                                total_leaves = Convert.ToString(reader[14]),
                                att_percentage = Convert.ToString(reader[15]),
                                roll_no = Convert.ToString(reader[16]),      
                            };
                            attendance_list.Add(std);
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

        public List<student_attendence> getStudentAttendanceGroupByClassAndAttendance(DateTime sDate)
        {
            List<student_attendence> attendance_list = new List<student_attendence>();
            student_attendence std;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {

                        cmd.CommandText = "select att.attendence_date, cl.class_name, att.class_id, att.attendence, Count(att.attendence) from sms_student_attendence as att Inner join sms_classes as cl on cl.id=att.class_id where att.attendence_date = Date(@sDate) group by att.class_id, att.attendence";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;                       
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            std = new student_attendence()
                            {
                                attendence_date = Convert.ToDateTime(reader[0]),
                                class_name = Convert.ToString(reader[1]),
                                class_id = Convert.ToString(reader[2]),
                                attendence = Convert.ToChar(reader[3]),                              
                                total_attendance = Convert.ToInt32(reader[4]),                                
                            };
                            attendance_list.Add(std);
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
