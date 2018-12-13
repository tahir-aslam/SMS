using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using SMS.Models;

namespace SMS.DAL
{
    public class RfidDAL
    {
        public int InserRfidCard(rfid_assignment obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_rfid_assignment(session_id,card_holder_id,card_no,is_std,created_by,date_time,emp_id) Values(@session_id,@card_holder_id,@card_no,@is_std,@created_by,@date_time,@emp_id)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = obj.session_id;
                        cmd.Parameters.Add("@card_holder_id", MySqlDbType.Int32).Value = obj.card_holder_id;
                        cmd.Parameters.Add("@card_no", MySqlDbType.VarChar).Value = obj.card_no;
                        cmd.Parameters.Add("@is_std", MySqlDbType.VarChar).Value = obj.is_std;
                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = obj.created_by;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = obj.date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
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
