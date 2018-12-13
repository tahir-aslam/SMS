using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.DAL
{
    public class RolesDAL
    {
        //-------Get All Roles ---------
        public List<roles> get_all_roles()
        {
            List<roles> roles_list = new List<roles>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from sms_roles";
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            roles rol = new roles()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                module_name = Convert.ToString(reader["module_name"]),
                                module_pid = Convert.ToString(reader["module_pid"]),
                                is_active = Convert.ToString(reader["is_active"]),
                                insertion = Convert.ToString(reader["insertion"]),
                                updation = Convert.ToString(reader["updation"]),
                            };
                            roles_list.Add(rol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return roles_list;
        }
    }
}
