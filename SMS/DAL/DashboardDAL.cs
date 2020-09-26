using MySql.Data.MySqlClient;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.DAL
{
    public class DashboardDAL
    {
        public DashboardChartModel loadPieChart(int sessionID)
        {
            dynamic res = new ExpandoObject();
            DashboardChartModel _charts = new DashboardChartModel();

            try
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "SELECT COUNT(*) FROM sms_admission AS adm WHERE adm.session_id=@sessionID && adm.is_active='Y';" +
                            "SELECT COUNT(*) as count FROM sms_admission AS adm WHERE adm.session_id = @sessionID && adm.boarding = 'Y' && adm.is_active = 'Y'; " +
                            "SELECT COUNT(*) as count FROM sms_admission AS adm WHERE adm.session_id = @sessionID && adm.boarding = 'N' && adm.is_active = 'Y'; " +
                            "SELECT COUNT(*) as count FROM sms_student_attendence AS att WHERE att.attendence_date = @date && att.attendence = 'P'; " +
                            "SELECT COUNT(*) as count FROM sms_student_attendence AS att WHERE att.attendence_date = @date && att.attendence = 'A'; " +
                            "SELECT COUNT(*) as count FROM sms_student_attendence AS att WHERE att.attendence_date = @date && att.attendence = 'L'; " +
                            "SELECT COUNT(*) as count FROM sms_emp AS emp WHERE emp.is_active = 'Y'; " +
                            "SELECT COUNT(*) as count FROM sms_emp AS emp WHERE emp.is_active = 'Y' && emp.emp_sex = 'Male'; " +
                            "SELECT COUNT(*) as count FROM sms_emp AS emp WHERE emp.is_active = 'Y' && emp.emp_sex = 'Female'; " +
                            "SELECT COUNT(*) as count FROM sms_emp_attendence AS att WHERE att.attendence_date = @date && att.attendence = 'P'; " +
                            "SELECT COUNT(*) as count FROM sms_emp_attendence AS att WHERE att.attendence_date = @date && att.attendence = 'A'; " +
                            "SELECT COUNT(*) FROM sms_emp_attendence AS att WHERE att.attendence_date = @date && att.attendence = 'L';";

                            cmd.Parameters.Add("@sessionID", MySqlDbType.Int32).Value = sessionID;
                            cmd.Parameters.Add("@date", MySqlDbType.Date).Value = DateTime.Now;
                            cmd.Connection = con;
                            con.Open();

                            MySqlDataReader reader = cmd.ExecuteReader();

                            #region Students
                            while (reader.Read())
                            {
                                _charts.TotalStudents = reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalBoys = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalGirls = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalStudentPresent = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalStudentAbsent = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalStudentLeave = (int)reader.GetInt32(0);
                            }
                            #endregion

                            #region Employees
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalEmp = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalEmpMale = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalEmpFemale = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalEmpPresent = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalEmpAbsent = (int)reader.GetInt32(0);
                            }
                            reader.NextResult();
                            while (reader.Read())
                            {
                                _charts.TotalEmpLeave = (int)reader.GetInt32(0);
                            }
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                res.status = true;
                res.data = _charts;
                return _charts;
            }
            catch (Exception ex)
            {
                //show error
                res.status = false;
                res.message = ex.Message;
                return _charts;
            }
        }
    }
}
