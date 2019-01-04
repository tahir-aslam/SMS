using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SMS.DAL
{
   public class AttendanceBioDAL
    {
       public List<sms_emp_attendance_bio> get_all_attendance_by_date(DateTime sDate, DateTime eDate)
       {
           List<sms_emp_attendance_bio> att_list = new List<sms_emp_attendance_bio>();
           try
           {
               using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
               {
                   using (MySqlCommand cmd = new MySqlCommand())
                   {
                       cmd.CommandText = "SELECT emp.emp_pay,emp.id, emp.emp_name, emp.emp_father, att.mode, att.date_time , emp.emp_designation_id, designation.designation " +
                                            "FROM sms_emp as emp  Left outer join sms_emp_attendance as att  on att.emp_id=emp.id && (att.date_time is null || ((Date(att.date_time) >=@sDate && Date(att.date_time) <=@eDate))) " +  
                                            "Inner Join sms_emp_title as title on emp.emp_title_id=title.id "+
                                            "Inner join sms_emp_designation as designation on emp.emp_designation_id=designation.id " +
                                            "where emp.is_active='Y' ";
                       cmd.Connection = con;
                       cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                       cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                       //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    

                       con.Open();

                       MySqlDataReader reader = cmd.ExecuteReader();

                       while (reader.Read())
                       {
                           sms_emp_attendance_bio att = new sms_emp_attendance_bio()
                           {                             
                               emp_id = Convert.ToInt32(reader["id"]),
                               salary = Convert.ToDouble(reader["emp_pay"]),
                               mode = reader["mode"].ToString(),                              
                               emp_name = reader["emp_name"].ToString(),
                               designation = reader["designation"].ToString(),
                               designation_id = Convert.ToInt32(reader["emp_designation_id"]),
                               father_name = reader["emp_father"].ToString(),                              
                           };
                           if (Convert.IsDBNull(reader["date_time"]))
                            {
                                att.date_time = new DateTime(2001,01,01);
                            }
                            else 
                            {
                                att.date_time = Convert.ToDateTime( reader["date_time"]);
                            }
                           att_list.Add(att);                           
                       }
                   }
               }
           }
           catch (Exception ex)
           { throw ex; }
           return att_list;
       }

       public List<sms_emp_attendance_bio> get_all__checkin_attendance_by_month(DateTime sDate, DateTime eDate)
       {
           List<sms_emp_attendance_bio> att_list = new List<sms_emp_attendance_bio>();
           try
           {
               using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
               {
                   using (MySqlCommand cmd = new MySqlCommand())
                   {
                       cmd.CommandText = "SELECT emp.id, emp.emp_name, emp.emp_father, att.mode, att.date_time , emp.emp_designation_id, designation.designation " +
                                            "FROM sms_emp as emp  Left outer join sms_emp_attendance as att on att.emp_id=emp.id && att.mode='checkin' && (att.date_time is null || ((Date(att.date_time) >=@sDate && Date(att.date_time) <=@eDate))) " +
                                            "Inner Join sms_emp_title as title on emp.emp_title_id=title.id " +
                                            "Inner join sms_emp_designation as designation on emp.emp_designation_id=designation.id " +
                                            "where emp.is_active='Y' group by emp.id, Date(att.date_time)";
                       cmd.Connection = con;
                       cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                       cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                       //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    

                       con.Open();

                       MySqlDataReader reader = cmd.ExecuteReader();

                       while (reader.Read())
                       {
                           sms_emp_attendance_bio att = new sms_emp_attendance_bio()
                           {
                               emp_id = Convert.ToInt32(reader["id"]),
                               mode = reader["mode"].ToString(),                               
                               emp_name = reader["emp_name"].ToString(),
                               designation = reader["designation"].ToString(),
                               designation_id = Convert.ToInt32(reader["emp_designation_id"]),
                               father_name = reader["emp_father"].ToString(),
                           };
                           if (Convert.IsDBNull(reader["date_time"]))
                           {
                               att.date_time = new DateTime(2001, 01, 01);
                           }
                           else
                           {
                               att.date_time = Convert.ToDateTime(reader["date_time"]);                               
                           }
                           att_list.Add(att);
                       }
                   }
               }
           }
           catch (Exception ex)
           { throw ex; }
           return att_list;
       }
    }
}
