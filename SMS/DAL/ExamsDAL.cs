using MySql.Data.MySqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
    public class ExamsDAL
    {
        //-----------              Get All Exam     ----------------------
        public List<exam> get_all_exams()
        {
            List<exam> exam_list = new List<exam>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_exam where session_id=" + MainWindow.session.id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            exam ex = new exam()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                exam_name = Convert.ToString(reader["exam_name"].ToString()),
                                exam_date = Convert.ToDateTime(reader["exam_date"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                            };
                            exam_list.Add(ex);

                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            return exam_list;
        }
        public List<exam_data_entry> GetAllExamDataEntryBySection(int session_id, int exam_id, int section_id)
        {
            List<exam_data_entry> list = new List<exam_data_entry>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT ses.id, ses.session_name, exam.id, exam.exam_name, cl.id, cl.class_name, sec.id, sec.section_name, emp.emp_name, "+
                                        "adm.id, adm.std_name, adm.father_name, adm.cell_no, adm.roll_no, adm.adm_no, adm.parmanent_adress, adm.image, "+
                                        "subj.id, subj.subject_name, "+
                                        "ede.subject_obtained, ede.subject_total, ede.subject_percentage, ede.subject_grade, ede.subject_remarks, ede.obtained_marks, ede.total_marks, ede.percentage, ede.remarks, ede.grade, "+
                                        "ins.institute_name, ins.institute_logo, "+
                                        "ede.subject_obtained_int, ede.subject_total_int, ede.position, ass.sort_order, adm.adm_no_int, adm.roll_no_int " +
                                        "FROM sms_institute as ins, sms_exams_data_entry AS ede " +
                                        "INNER JOIN sms_admission AS adm ON adm.id = ede.std_id AND adm.session_id = ede.session_id " +
                                        "INNER JOIN sms_classes AS cl ON cl.id = ede.class_id "+
                                        "INNER JOIN sms_subjects AS sec ON sec.id = ede.section_id "+
                                        "INNER JOIN sms_exams_subjects AS subj ON subj.id = ede.subject_id "+
                                        "INNER JOIN sms_exam AS exam ON exam.id = ede.exam_id "+
                                        "INNER JOIN sms_exams_subject_assignment AS ass ON ass.subject_id = ede.subject_id AND ass.section_id = ede.section_id "+
                                        "INNER JOIN sms_emp AS emp ON emp.id = ass.emp_id "+
                                        "INNER JOIN sms_sessions AS ses ON ses.id = ede.session_id "+
                                        "WHERE ede.section_id = @section_id  AND ede.exam_id = @exam_id AND ede.session_id = @session_id " +
                                        "ORDER BY adm.adm_no_int ASC";
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = session_id;
                        cmd.Parameters.Add("@exam_id", MySqlDbType.Int32).Value = exam_id;
                        cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = section_id;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            exam_data_entry obj = new exam_data_entry()
                            {
                                session_id = Convert.ToInt32(reader[0]),
                                session_name = Convert.ToString(reader[1]),
                                exam_id = Convert.ToString(reader[2]),
                                exam_name = Convert.ToString(reader[3]),
                                class_id = Convert.ToString(reader[4]),
                                class_name = Convert.ToString(reader[5]),
                                section_id = Convert.ToString(reader[6]),
                                section_name = Convert.ToString(reader[7]),
                                emp_name = Convert.ToString(reader[8]),
                                std_id = Convert.ToString(reader[9]),
                                std_name = Convert.ToString(reader[10]),
                                father_name = Convert.ToString(reader[11]),
                                cell_no = Convert.ToString(reader[12]),
                                roll_no = Convert.ToString(reader[13]),
                                adm_no = Convert.ToString(reader[14]),
                                permanent_address = Convert.ToString(reader[15]),
                                std_img = (Byte[])(reader[16]),
                                subject_id = Convert.ToString(reader[17]),
                                subject_name = Convert.ToString(reader[18]),
                                subject_obtained = Convert.ToString(reader[19]),
                                subject_total = Convert.ToString(reader[20]),
                                subject_percentage = Convert.ToString(reader[21]),
                                subject_grade = Convert.ToString(reader[22]),
                                subject_remarks = Convert.ToString(reader[23]),
                                obtained_marks = Convert.ToString(reader[24]),
                                total_remarks = Convert.ToString(reader[25]),
                                percentage = Convert.ToString(reader[26]),
                                remarks = Convert.ToString(reader[27]),
                                grade = Convert.ToString(reader[28]),   
                                institute_name = Convert.ToString(reader[29]),
                                institute_logo = (Byte[])(reader[30]),
                                subject_obtained_int = Convert.ToInt32(reader[31]),
                                subject_total_int = Convert.ToInt32(reader[32]),
                                position = Convert.ToString(reader[33]),
                                sort_order = Convert.ToInt32(reader[34]),
                                adm_no_int = Convert.ToInt32(reader[35]),
                                roll_no_int = Convert.ToInt32(reader[36]),
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
        public List<exam_data_entry> GetAllExamDataEntryBySession(int session_id)
        {
            List<exam_data_entry> list = new List<exam_data_entry>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT ses.id, ses.session_name, exam.id, exam.exam_name, cl.id, cl.class_name, sec.id, sec.section_name, emp.emp_name, " +
                                        "adm.id, adm.std_name, adm.father_name, adm.cell_no, adm.roll_no, adm.adm_no, adm.parmanent_adress, adm.image, " +
                                        "subj.id, subj.subject_name, " +
                                        "ede.subject_obtained, ede.subject_total, ede.subject_percentage, ede.subject_grade, ede.subject_remarks, ede.obtained_marks, ede.total_marks, ede.percentage, ede.remarks, ede.grade, " +
                                        "ins.institute_name, ins.institute_logo, " +
                                        "ede.subject_obtained_int, ede.subject_total_int, ede.position " +
                                        "FROM sms_institute as ins, sms_exams_data_entry AS ede " +
                                        "INNER JOIN sms_admission AS adm ON adm.id = ede.std_id AND adm.session_id = ede.session_id " +
                                        "INNER JOIN sms_classes AS cl ON cl.id = ede.class_id " +
                                        "INNER JOIN sms_subjects AS sec ON sec.id = ede.section_id " +
                                        "INNER JOIN sms_exams_subjects AS subj ON subj.id = ede.subject_id " +
                                        "INNER JOIN sms_exam AS exam ON exam.id = ede.exam_id " +
                                        "INNER JOIN sms_exams_subject_assignment AS ass ON ass.subject_id = ede.subject_id AND ass.section_id = ede.section_id " +
                                        "INNER JOIN sms_emp AS emp ON emp.id = ass.emp_id " +
                                        "INNER JOIN sms_sessions AS ses ON ses.id = ede.session_id " +
                                        "WHERE ede.session_id = @session_id ";
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = session_id;
                        
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            exam_data_entry obj = new exam_data_entry()
                            {
                                session_id = Convert.ToInt32(reader[0]),
                                session_name = Convert.ToString(reader[1]),
                                exam_id = Convert.ToString(reader[2]),
                                exam_name = Convert.ToString(reader[3]),
                                class_id = Convert.ToString(reader[4]),
                                class_name = Convert.ToString(reader[5]),
                                section_id = Convert.ToString(reader[6]),
                                section_name = Convert.ToString(reader[7]),
                                emp_name = Convert.ToString(reader[8]),
                                std_id = Convert.ToString(reader[9]),
                                std_name = Convert.ToString(reader[10]),
                                father_name = Convert.ToString(reader[11]),
                                cell_no = Convert.ToString(reader[12]),
                                roll_no = Convert.ToString(reader[13]),
                                adm_no = Convert.ToString(reader[14]),
                                permanent_address = Convert.ToString(reader[15]),
                                std_img = (Byte[])(reader[16]),
                                subject_id = Convert.ToString(reader[17]),
                                subject_name = Convert.ToString(reader[18]),
                                subject_obtained = Convert.ToString(reader[19]),
                                subject_total = Convert.ToString(reader[20]),
                                subject_percentage = Convert.ToString(reader[21]),
                                subject_grade = Convert.ToString(reader[22]),
                                subject_remarks = Convert.ToString(reader[23]),
                                obtained_marks = Convert.ToString(reader[24]),
                                total_remarks = Convert.ToString(reader[25]),
                                percentage = Convert.ToString(reader[26]),
                                remarks = Convert.ToString(reader[27]),
                                grade = Convert.ToString(reader[28]),
                                institute_name = Convert.ToString(reader[29]),
                                institute_logo = (Byte[])(reader[30]),
                                subject_obtained_int = Convert.ToInt32(reader[31]),
                                subject_total_int = Convert.ToInt32(reader[32]),
                                position = Convert.ToString(reader[33]),
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

        //----------             Date Sheet         ------------------------
        public List<sms_exams_date_sheet> get_all_date_sheet()
        {
            List<sms_exams_date_sheet> exam_list = new List<sms_exams_date_sheet>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT eds.id, eds.exam_date, eds.exam_time, eds.remarks, ex.id, ex.exam_name,"+
                                        "cl.id, cl.class_name, sec.id, sec.section_name, "+
                                        "subj.id, subj.subject_name, "+
                                        "emp_c.emp_name, emp_u.emp_name, eds.created_date_time, eds.updated_date_time " +
                                        "FROM sms_exams_date_sheet AS eds " +                                        
                                        "INNER JOIN sms_exam AS ex ON ex.id = eds.exam_id "+
                                        "INNER JOIN sms_classes AS cl ON cl.id = eds.class_id "+
                                        "INNER JOIN sms_subjects AS sec ON sec.id = eds.section_id "+
                                        "INNER JOIN sms_exams_subjects AS subj ON subj.id = eds.subject_id "+
                                        "LEFT JOIN sms_emp AS emp_c ON emp_c.id = eds.created_emp_id " +
                                        "LEFT JOIN sms_emp AS emp_u ON emp_u.id = eds.updated_emp_id "+
                                        "where eds.is_deleted=0";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_exams_date_sheet obj = new sms_exams_date_sheet()
                            {
                                id = Convert.ToInt32(reader[0]),
                                exam_date = Convert.ToDateTime(reader[1]),
                                exam_time = Convert.ToString(reader[2]),
                                remarks = Convert.ToString(reader[3]),
                                exam_id = Convert.ToInt32(reader[4]),
                                exam_name = Convert.ToString(reader[5]),
                                class_id = Convert.ToInt32(reader[6]),
                                class_name = Convert.ToString(reader[7]),
                                section_id = Convert.ToInt32(reader[8]),
                                section_name = Convert.ToString(reader[9]),                                
                                subject_id = Convert.ToInt32(reader[10]),
                                subject_name = Convert.ToString(reader[11]),
                                created_emp_name = Convert.ToString(reader[12]),
                                updated_emp_name = Convert.ToString(reader[13]),
                                created_date_time = Convert.ToDateTime(reader[14]),                                
                            };
                            if (!reader.IsDBNull(15))
                            {
                                obj.updated_date_time = Convert.ToDateTime(reader[15]);
                            }
                            exam_list.Add(obj);
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            return exam_list;
        }
        public int InsertDateSheet(List<sms_exams_date_sheet> lst)
        {
            int i = 0;

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                con.Open();
                try
                {
                    foreach (sms_exams_date_sheet obj in lst)
                    {
                        i = 0;
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_exams_date_sheet(exam_id, subject_id, class_id, section_id, exam_date, exam_time, remarks, created_date_time, created_emp_id) Values(@exam_id, @subject_id, @class_id, @section_id, @exam_date, @exam_time, @remarks, @created_date_time, @created_emp_id)";
                            cmd.Connection = con;

                            cmd.Parameters.Add("@exam_id", MySqlDbType.Int32).Value = obj.exam_id;
                            cmd.Parameters.Add("@subject_id", MySqlDbType.Int32).Value = obj.subject_id;
                            cmd.Parameters.Add("@class_id", MySqlDbType.Int32).Value = obj.class_id;
                            cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = obj.section_id;
                            cmd.Parameters.Add("@exam_date", MySqlDbType.Date).Value = obj.exam_date;
                            cmd.Parameters.Add("@exam_time", MySqlDbType.VarChar).Value = obj.exam_time;
                            cmd.Parameters.Add("@remarks", MySqlDbType.VarChar).Value = obj.remarks;                            

                            cmd.Parameters.Add("@created_date_time", MySqlDbType.DateTime).Value = obj.created_date_time;                            
                            cmd.Parameters.Add("@created_emp_id", MySqlDbType.Int32).Value = obj.created_emp_id;                           

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
        public int UpdateDateSheet(sms_exams_date_sheet obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_exams_date_sheet SET subject_id=@subject_id, exam_date=@exam_date, exam_time=@exam_time, remarks=@remarks, updated_date_time=@updated_date_time,updated_emp_id=@updated_emp_id where id=@id";
                        cmd.Connection = con;
                                               
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.id;
                        cmd.Parameters.Add("@subject_id", MySqlDbType.Int32).Value = obj.subject_id;
                        cmd.Parameters.Add("@exam_date", MySqlDbType.Date).Value = obj.exam_date;
                        cmd.Parameters.Add("@exam_time", MySqlDbType.VarChar).Value = obj.exam_time;
                        cmd.Parameters.Add("@remarks", MySqlDbType.VarChar).Value = obj.remarks;

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
        public int DeleteDateSheet(List<int> ids, int emp_id)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    foreach (var item in ids)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Update sms_exams_date_sheet SET is_deleted=@is_deleted, is_synchronized=@is_synchronized where id=@id";
                            cmd.Connection = con;

                            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = item;                            
                            cmd.Parameters.Add("@is_deleted", MySqlDbType.Int16).Value =1;
                            cmd.Parameters.Add("@is_synchronized", MySqlDbType.Int32).Value = 0;

                            cmd.Parameters.Add("@updated_date_time", MySqlDbType.DateTime).Value = DateTime.Now;
                            cmd.Parameters.Add("@updated_emp_id", MySqlDbType.Int32).Value = emp_id;

                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            cmd.Parameters.Clear();                            
                        }
                    }
                    con.Close();
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
