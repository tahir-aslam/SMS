﻿using MySql.Data.MySqlClient;
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

        public int DeleteSubject(List<int> lst)
        {
            int result =0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();                    
                    for (int i = 0; i < lst.Count; i++)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Delete from sms_exams_subjects where id=@id";                            
                            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = lst[i];
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
    }
}