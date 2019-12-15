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
                                        "ede.subject_obtained, ede.subject_total, ede.subject_percentage, ede.subject_grade, ede.subject_remarks, ede.obtained_marks, ede.total_marks, ede.percentage, ede.remarks, ede.grade "+
                                        "FROM sms_exams_data_entry AS ede "+
                                        "INNER JOIN sms_admission AS adm ON adm.id = ede.std_id "+
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
    }
}
