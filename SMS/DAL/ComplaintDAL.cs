using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;
using SMS.Common;

namespace SMS.DAL
{
    public class ComplaintDAL
    {        
        public ComplaintDAL()
        {            
        }
        public List<sms_complaint_from> getAllComplaintFrom()
        {

            List<sms_complaint_from> list = new List<sms_complaint_from>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_complaint_from";
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sms_complaint_from obj = new sms_complaint_from()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                name = Convert.ToString(reader["name"])

                            };
                            list.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
        public List<sms_complaint_type> getAllComplaintTypes()
        {

            List<sms_complaint_type> list = new List<sms_complaint_type>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_complaint_type";
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sms_complaint_type obj = new sms_complaint_type()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                complaint_type = Convert.ToString(reader["complaint_type"]),
                                complaint_type_abb = Convert.ToString(reader["complaint_type_abb"]),
                                remarks = Convert.ToString(reader["remarks"]),
                                complaint_from_id = Convert.ToInt32(reader["complaint_from_id"]),

                            };
                            list.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
        public List<sms_complaint_status> getAllComplaintStatus()
        {
            List<sms_complaint_status> list = new List<sms_complaint_status>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_complaint_status";
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sms_complaint_status obj = new sms_complaint_status()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                complaint_status = Convert.ToString(reader["complaint_type"]),
                            };
                            list.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
        public List<sms_complaint_register> getAllComplaintsRegiter(DateTime sDate, DateTime eDate)
        {
            List<sms_complaint_register> list = new List<sms_complaint_register>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_complaint_register as register " +
                                            "Inner Join sms_admission as adm ON adm.id=register.std_id && adm.session_id=@session_id " +
                                            "Inner Join sms_complaint_from as c_from ON c_from.id=register.complaint_from_id " +
                                            "Inner Join sms_complaint_status as c_status ON c_status.id=register.complaint_status_id " +
                                            "Inner Join sms_complaint_type as c_type ON c_type.id=register.complaint_type_id " +
                                            "Where register.deletion='false' && (DATE(register.created_date_time) >= @sDate && DATE(register.created_date_time) <= @eDate)";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Parameters.Add("@session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sms_complaint_register obj = new sms_complaint_register()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                complaint_type_id = Convert.ToInt32(reader["complaint_type_id"]),
                                complaint_status_id = Convert.ToInt32(reader["complaint_status_id"]),
                                complaint_from_id = Convert.ToInt32(reader["complaint_from_id"]),
                                complaint_remarks = Convert.ToString(reader["complaint_remarks"]),
                                complaint_resolved_remarks = Convert.ToString(reader["complaint_resolved_remarks"]),
                                complaint_date = Convert.ToDateTime(reader["complaint_date"]),
                                complaint_resolved_date = Convert.ToDateTime(reader["complaint_resolved_date"]),
                                created_date_time = Convert.ToDateTime(reader["created_date_time"]),
                                updated_date_time = Convert.ToDateTime(reader["updated_date_time"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                updated_by = Convert.ToString(reader["updated_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),

                                std_name = Convert.ToString(reader["std_name"]),
                                father_name = Convert.ToString(reader["father_name"]),
                                adm_no = Convert.ToString(reader["adm_no"]),
                                roll_no = Convert.ToString(reader["roll_no"]),
                                cell_no = Convert.ToString(reader["cell_no"]),
                                class_id = Convert.ToString(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToString(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                session_id = Convert.ToInt32(reader["session_id"]),
                            };
                            list.Add(obj);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }
        public int insertComplaintRegister(sms_complaint_register obj)
{
    try
    {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_complaint_register(std_id,session_id,complaint_type_id,complaint_status_id,complaint_from_id, complaint_remarks,complaint_resolved_remarks,complaint_date, complaint_resolved_date, created_by, updated_by, created_date_time, updated_date_time, emp_id) Values(@std_id, @session_id, @complaint_type_id, @complaint_status_id, @complaint_from_id, @complaint_remarks, @complaint_resolved_remarks, @complaint_date, @complaint_resolved_date, @created_by, @updated_by, @created_date_time, @updated_date_time, @emp_id)";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = obj.std_id;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = obj.session_id;
                        cmd.Parameters.Add("@complaint_type_id", MySqlDbType.Int32).Value = obj.complaint_type_id;
                        cmd.Parameters.Add("@complaint_status_id", MySqlDbType.Int32).Value = obj.complaint_status_id;
                        cmd.Parameters.Add("@complaint_from_id", MySqlDbType.Int32).Value = obj.complaint_from_id;
                        cmd.Parameters.Add("@complaint_remarks", MySqlDbType.VarChar).Value = obj.complaint_remarks;
                        cmd.Parameters.Add("@complaint_resolved_remarks", MySqlDbType.VarChar).Value = obj.complaint_resolved_remarks;
                        cmd.Parameters.Add("@complaint_date", MySqlDbType.Date).Value = obj.complaint_date;
                        cmd.Parameters.Add("@complaint_resolved_date", MySqlDbType.Date).Value = obj.complaint_resolved_date;
                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = obj.created_by;
                        cmd.Parameters.Add("@updated_by", MySqlDbType.VarChar).Value = obj.updated_by;
                        cmd.Parameters.Add("@created_date_time", MySqlDbType.Date).Value = obj.created_date_time;
                        cmd.Parameters.Add("@updated_date_time", MySqlDbType.Date).Value = obj.updated_date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

                        return Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
}
public int updateComplaintRegister(sms_complaint_register obj)
{
    try
    {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_complaint_register Set std_id=@std_id, session_id=@session_id, complaint_type_id=@complaint_type_id,complaint_status_id=@complaint_status_id, complaint_from_id=@complaint_from_id, complaint_remarks=@complaint_remarks, complaint_resolved_remarks=@complaint_resolved_remarks, complaint_date=@complaint_date, complaint_resolved_date=@complaint_resolved_date, updated_date_time=@updated_date_time, updated_by=@updated_by, emp_id=@emp_id";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = obj.std_id;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = obj.session_id;
                        cmd.Parameters.Add("@complaint_type_id", MySqlDbType.Int32).Value = obj.complaint_type_id;
                        cmd.Parameters.Add("@complaint_status_id", MySqlDbType.Int32).Value = obj.complaint_status_id;
                        cmd.Parameters.Add("@complaint_from_id", MySqlDbType.Int32).Value = obj.complaint_from_id;
                        cmd.Parameters.Add("@complaint_remarks", MySqlDbType.VarChar).Value = obj.complaint_remarks;
                        cmd.Parameters.Add("@complaint_resolved_remarks", MySqlDbType.VarChar).Value = obj.complaint_resolved_remarks;
                        cmd.Parameters.Add("@complaint_date", MySqlDbType.Date).Value = obj.complaint_date;
                        cmd.Parameters.Add("@complaint_resolved_date", MySqlDbType.Date).Value = obj.complaint_resolved_date;
                        cmd.Parameters.Add("@updated_by", MySqlDbType.VarChar).Value = obj.updated_by;
                        cmd.Parameters.Add("@updated_date_time", MySqlDbType.DateTime).Value = obj.updated_date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

                        return Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int deleteComplaintRegister(sms_complaint_register obj)
{
    try
    {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_complaint_register Set deletion=@deletion, updated_date_time=@updated_date_time, updated_by=@updated_by, emp_id=@emp_id where id=@id)";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@deletion", MySqlDbType.VarChar).Value = "true";
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.id;
                        cmd.Parameters.Add("@updated_by", MySqlDbType.VarChar).Value = obj.updated_by;
                        cmd.Parameters.Add("@updated_date_time", MySqlDbType.DateTime).Value = obj.updated_date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

                        return Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
