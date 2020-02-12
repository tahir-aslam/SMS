using MySql.Data.MySqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.DAL
{
    public class SubjectsDAL
    {
        public SubjectsDAL()
        {
        }

        public List<sms_exams_subjects> GetAllSubjects()
        {
            List<sms_exams_subjects> lst = new List<sms_exams_subjects>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_exams_subjects where is_active='Y' order by subject_name ASC";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_exams_subjects obj = new sms_exams_subjects()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                subject_name = Convert.ToString(reader["subject_name"]),
                                subject_abb = Convert.ToString(reader["subject_abb"]),
                                subjects_group_id = Convert.ToInt32(reader["subjects_group_id"]),
                                subject_type_id = Convert.ToInt32(reader["subject_type_id"]),
                                subject_code = Convert.ToString(reader["subject_code"]),
                                remarks = Convert.ToString(reader["remarks"]),
                                created_emp_id = Convert.ToInt32(reader["created_emp_id"]),
                                updated_emp_id = Convert.ToInt32(reader["updated_emp_id"]),
                                created_date_time = Convert.ToDateTime(reader["created_date_time"]),
                                updated_date_time = Convert.ToDateTime(reader["updated_date_time"]),
                                is_active = Convert.ToString(reader["is_active"]),
                            };
                            lst.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
        public int DeleteSubjectAssignment(List<sms_exams_subjects> lst)
        {
            int result = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    foreach (var item in lst)                    
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Delete from sms_exams_subject_assignment where subject_id = @subject_id && section_id=@section_id";
                            cmd.Parameters.Add("@subject_id", MySqlDbType.Int32).Value = item.id;
                            cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = item.section_id;
                            cmd.Connection = con;
                            result = Convert.ToInt32(cmd.ExecuteNonQuery());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public int InsertSubjectAssignment(List<sms_exams_subjects> subjectsList)
        {
            int i = 0;

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                con.Open();
                try
                {
                    foreach (sms_exams_subjects obj in subjectsList)
                    {
                        i = 0;
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_exams_subject_assignment(subject_id, section_id, emp_id, std_id, created_date_time, updated_date_time, created_emp_id, updated_emp_id, sort_order) Values(@subject_id, @section_id, @emp_id, @std_id, @created_date_time, @updated_date_time, @created_emp_id, @updated_emp_id, @sort_order)";
                            cmd.Connection = con;

                            cmd.Parameters.Add("@subject_id", MySqlDbType.Int32).Value = obj.id;
                            cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = obj.section_id;
                            cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;
                            cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = 0;
                            cmd.Parameters.Add("@sort_order", MySqlDbType.Int32).Value = obj.sort_order;

                            cmd.Parameters.Add("@created_date_time", MySqlDbType.DateTime).Value = obj.created_date_time;
                            cmd.Parameters.Add("@updated_date_time", MySqlDbType.DateTime).Value = obj.updated_date_time;
                            cmd.Parameters.Add("@created_emp_id", MySqlDbType.Int32).Value = obj.created_emp_id;
                            cmd.Parameters.Add("@updated_emp_id", MySqlDbType.Int32).Value = obj.updated_emp_id;                            

                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            cmd.Parameters.Clear();
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;

        }
        public List<sms_exams_subjects> GetAllSubjectsAssignment()
        {
            List<sms_exams_subjects> lst = new List<sms_exams_subjects>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT emp.emp_name, cl.id, cl.class_name, sec.section_name, subj.subject_name, subj_g.group_name, subj_t.subject_type, "+
                                            "emp_c.emp_name, emp_u.emp_name, ass.subject_id, ass.section_id, ass.emp_id, ass.created_emp_id, ass.updated_emp_id, ass.created_date_time, ass.updated_date_time, ass.is_active, subj.subject_code, emp_desg.id, emp_desg.designation, ass.sort_order " +
                                            "from sms_exams_subject_assignment AS ass "+
                                            "INNER JOIN sms_exams_subjects AS subj ON subj.id = ass.subject_id "+
                                            "INNER JOIN sms_exams_subjects_group AS subj_g ON subj_g.id = subj.subjects_group_id "+
                                            "INNER JOIN sms_exams_subject_type AS subj_t ON subj_t.id = subj.subject_type_id "+
                                            "INNER JOIN sms_subjects AS sec ON sec.id = ass.section_id "+
                                            "INNER JOIN sms_classes AS cl ON cl.id = sec.class_id "+
                                            "INNER JOIN sms_emp AS emp ON emp.id = ass.emp_id "+
                                            "Inner JOIN sms_emp AS emp_c ON emp_c.id = ass.created_emp_id " +
                                            "Inner JOIN sms_emp AS emp_u ON emp_u.id = ass.updated_emp_id " +
                                            "INNER JOIN sms_emp_designation AS emp_desg ON emp_desg.id=emp.emp_designation_id " +
                                            "ORDER BY ass.section_id ASC, subj.subject_name ASC ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_exams_subjects obj = new sms_exams_subjects()
                            {
                                emp_name = Convert.ToString(reader[0]),
                                class_id = Convert.ToInt32(reader[1]),
                                class_name = Convert.ToString(reader[2]),
                                section_name = Convert.ToString(reader[3]),
                                subject_name = Convert.ToString(reader[4]),
                                subjects_group = Convert.ToString(reader[5]),
                                subject_type = Convert.ToString(reader[6]),
                                created_emp_name = Convert.ToString(reader[7]),
                                updated_emp_name = Convert.ToString(reader[8]),
                                id = Convert.ToInt32(reader[9]),
                                section_id = Convert.ToInt32(reader[10]),
                                emp_id = Convert.ToInt32(reader[11]),
                                created_emp_id = Convert.ToInt32(reader[12]),
                                updated_emp_id = Convert.ToInt32(reader[13]),
                                created_date_time = Convert.ToDateTime(reader[14]),
                                updated_date_time = Convert.ToDateTime(reader[15]),
                                is_active = Convert.ToString(reader[16]),
                                subject_code = Convert.ToString(reader[17]),
                                emp_designation_id = Convert.ToInt32(reader[18]),
                                emp_designation = Convert.ToString(reader[19]),
                                sort_order = Convert.ToInt32(reader[20]),
                            };
                            lst.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
        public List<sms_exams_subjects> GetAllSubjectsAssignmentOfSection(int sectionID)
        {
            List<sms_exams_subjects> lst = new List<sms_exams_subjects>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT emp.emp_name, cl.id, cl.class_name, sec.section_name, subj.subject_name, subj_g.group_name, subj_t.subject_type, " +
                                            "emp_c.emp_name, emp_u.emp_name, ass.subject_id, ass.section_id, ass.emp_id, ass.created_emp_id, ass.updated_emp_id, ass.created_date_time, ass.updated_date_time, ass.is_active, subj.subject_code, emp_desg.id, emp_desg.designation, ass.sort_order " +
                                            "from sms_exams_subject_assignment AS ass " +
                                            "INNER JOIN sms_exams_subjects AS subj ON subj.id = ass.subject_id " +
                                            "INNER JOIN sms_exams_subjects_group AS subj_g ON subj_g.id = subj.subjects_group_id " +
                                            "INNER JOIN sms_exams_subject_type AS subj_t ON subj_t.id = subj.subject_type_id " +
                                            "INNER JOIN sms_subjects AS sec ON sec.id = ass.section_id " +
                                            "INNER JOIN sms_classes AS cl ON cl.id = sec.class_id " +
                                            "INNER JOIN sms_emp AS emp ON emp.id = ass.emp_id " +
                                            "Inner JOIN sms_emp AS emp_c ON emp_c.id = ass.created_emp_id " +
                                            "Inner JOIN sms_emp AS emp_u ON emp_u.id = ass.updated_emp_id " +
                                            "INNER JOIN sms_emp_designation AS emp_desg ON emp_desg.id=emp.emp_designation_id " +
                                            "Where ass.section_id=@section_id "+
                                            "ORDER BY ass.sort_order ASC ";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = sectionID;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_exams_subjects obj = new sms_exams_subjects()
                            {
                                emp_name = Convert.ToString(reader[0]),
                                class_id = Convert.ToInt32(reader[1]),
                                class_name = Convert.ToString(reader[2]),
                                section_name = Convert.ToString(reader[3]),
                                subject_name = Convert.ToString(reader[4]),
                                subjects_group = Convert.ToString(reader[5]),
                                subject_type = Convert.ToString(reader[6]),
                                created_emp_name = Convert.ToString(reader[7]),
                                updated_emp_name = Convert.ToString(reader[8]),
                                id = Convert.ToInt32(reader[9]),
                                section_id = Convert.ToInt32(reader[10]),
                                emp_id = Convert.ToInt32(reader[11]),
                                created_emp_id = Convert.ToInt32(reader[12]),
                                updated_emp_id = Convert.ToInt32(reader[13]),
                                created_date_time = Convert.ToDateTime(reader[14]),
                                updated_date_time = Convert.ToDateTime(reader[15]),
                                is_active = Convert.ToString(reader[16]),
                                subject_code = Convert.ToString(reader[17]),
                                emp_designation_id = Convert.ToInt32(reader[18]),
                                emp_designation = Convert.ToString(reader[19]),
                                sort_order = Convert.ToInt32(reader[20]),
                            };
                            lst.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lst;
        }
        public int UpdateSubjectAssignment(sms_exams_subjects obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_exams_subject_assignment SET emp_id=@emp_id,std_id=@std_id,updated_date_time=@updated_date_time,updated_emp_id=@updated_emp_id, sort_order=@sort_order where subject_id = @subject_id && section_id=@section_id";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@subject_id", MySqlDbType.Int32).Value = obj.id;
                        cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = obj.section_id;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = 0;
                        cmd.Parameters.Add("@sort_order", MySqlDbType.Int32).Value = obj.sort_order;

                        cmd.Parameters.Add("@updated_date_time", MySqlDbType.DateTime).Value = obj.updated_date_time;                        
                        cmd.Parameters.Add("@updated_emp_id", MySqlDbType.Int32).Value = obj.updated_emp_id;

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cmd.Parameters.Clear();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }
    }
}
