using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using SMS.Models;

namespace SMS.DAL
{
    class LicenseDAL
    {
        public int inser_login_log_OnlineDB()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.tahir123_sms_security))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO login_log(institute_id,institute_name,institute_cell,institute_phone,institute_address,institute_owner_cell,institute_owner_name,institute_quote,emp_user_name, ip_address, local_server,installation_date, expiry_date,expiry_warning_day,expiry_message,expiry_warning_message, date_time) Values(@institute_id,@institute_name,@institute_cell,@institute_phone,@institute_address,@institute_owner_cell,@institute_owner_name,@institute_quote,@emp_user_name, @ip_address, @local_server,@installation_date, @expiry_date,@expiry_warning_day,@expiry_message,@expiry_warning_message, @date_time)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@institute_id", MySqlDbType.Int32).Value = MainWindow.ins.institute_id;
                        cmd.Parameters.Add("@institute_name", MySqlDbType.VarChar).Value = MainWindow.ins.institute_name;
                        cmd.Parameters.Add("@institute_cell", MySqlDbType.VarChar).Value = MainWindow.ins.institute_cell;
                        cmd.Parameters.Add("@institute_phone", MySqlDbType.VarChar).Value = MainWindow.ins.institute_phone;
                        cmd.Parameters.Add("@institute_address", MySqlDbType.VarChar).Value = MainWindow.ins.institute_address;
                        cmd.Parameters.Add("@institute_owner_cell", MySqlDbType.VarChar).Value = MainWindow.ins.institute_owner_cell;
                        cmd.Parameters.Add("@institute_owner_name", MySqlDbType.VarChar).Value = MainWindow.ins.institute_owner_name;
                        cmd.Parameters.Add("@institute_quote", MySqlDbType.VarChar).Value = MainWindow.ins.institute_quote;
                        cmd.Parameters.Add("@emp_user_name", MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                        cmd.Parameters.Add("@ip_address", MySqlDbType.VarChar).Value = MainWindow.getIpAddress();
                        cmd.Parameters.Add("@local_server", MySqlDbType.VarChar).Value = Connection_String.con_string;
                        cmd.Parameters.Add("@installation_date", MySqlDbType.DateTime).Value = MainWindow.ins.installation_date;
                        cmd.Parameters.Add("@expiry_date", MySqlDbType.DateTime).Value = MainWindow.ins.expiry_date;
                        cmd.Parameters.Add("@expiry_warning_day", MySqlDbType.Int32).Value = MainWindow.ins.expiry_warning_day;
                        cmd.Parameters.Add("@expiry_message", MySqlDbType.VarChar).Value = MainWindow.ins.expiry_message;
                        cmd.Parameters.Add("@expiry_warning_message", MySqlDbType.VarChar).Value = MainWindow.ins.expiry_warning_message;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = DateTime.Now;

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
        public institute get_expiry_OnlineDB() 
        {
            institute ins = new institute();
           try
           {
               using (MySqlConnection con = new MySqlConnection(Connection_String.tahir123_sms_security))
               {
                   using (MySqlCommand cmd = new MySqlCommand())                   
                   {
                       cmd.Connection = con;
                       con.Open();

                       cmd.CommandText = "SELECT* from  institute_expiry where institute_id = @institute_id";
                       cmd.Parameters.Add("@institute_id", MySqlDbType.Int32).Value = MainWindow.ins.institute_id;

                       MySqlDataReader reader = cmd.ExecuteReader();

                       if(reader.Read())
                       {
                           ins = new institute()
                           {
                                institute_id = Convert.ToInt32(reader["institute_id"]),
                                institute_name= Convert.ToString(reader["institute_name"]),
                                expiry_message= Convert.ToString(reader["expiry_message"]),
                                expiry_warning_message= Convert.ToString(reader["expiry_warning_message"]),
                                expiry_instant = Convert.ToString(reader["expiry_instant"]),
                                expiry_date= Convert.ToDateTime(reader["expiry_date"]),
                                expiry_warning_day = Convert.ToInt32(reader["expiry_warning_day"]),
                                check = true,
                           };
                       }
                       else
                       {
                           ins.check =false;
                       }
                        con.Close();
                    }
               }
           }
           catch(Exception ex)
           {
               throw ex;
           }
            return ins;
        }
        public int update_sms_institute_local(institute ins) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "update sms_institute set expiry_instant=@expiry_instant, expiry_date=@expiry_date,expiry_warning_day=@expiry_warning_day,expiry_message=@expiry_message,expiry_warning_message=@expiry_warning_message";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add("@expiry_date", MySqlDbType.DateTime).Value = ins.expiry_date;
                        cmd.Parameters.Add("@expiry_warning_day", MySqlDbType.Int32).Value = ins.expiry_warning_day;
                        cmd.Parameters.Add("@expiry_message", MySqlDbType.VarChar).Value = ins.expiry_message;
                        cmd.Parameters.Add("@expiry_warning_message", MySqlDbType.VarChar).Value = ins.expiry_warning_message;
                        cmd.Parameters.Add("@expiry_instant", MySqlDbType.VarChar).Value = ins.expiry_instant;                        

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
