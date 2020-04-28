using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.DAL
{
    public class MiscDAL
    {
        public MiscDAL() 
        {

        }

        public List<sms_months> get_all_months()
        {
            List<sms_months> months_list = new List<sms_months>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_months";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_months sm = new sms_months()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                month_id = Convert.ToString(reader["month"].ToString()),
                            };
                            months_list.Add(sm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return months_list;
        }

        public List<sms_years> get_all_years()
        {
            List<sms_years> yearList = new List<sms_years>();
            sms_years year;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_years";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            year = new sms_years()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                year = Convert.ToString(reader["year"]),
                                is_active = Convert.ToString(reader["is_active"])
                            };
                            yearList.Add(year);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return yearList;
        }

        public List<sms_freind_type> get_all_freind_type()
        {
            List<sms_freind_type> freind_type_list = new List<sms_freind_type>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_freind_type";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_freind_type sm = new sms_freind_type()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                freind_type = reader["freind_type"].ToString(),
                                is_active = Convert.ToString(reader["is_active"])
                            };
                            freind_type_list.Add(sm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return freind_type_list;
        }

        public List<prefixNo> get_all_adm_no_prefix()
        {
            List<prefixNo> list = new List<prefixNo>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_prefix_admission_no";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            prefixNo sm = new prefixNo()
                            {
                                id = Convert.ToInt32(reader["id"].ToString()),
                                prefix = Convert.ToString(reader["prefix"].ToString()),
                                prefix_abbreviation = Convert.ToString(reader["prefix_abbreviation"].ToString()),
                                sort_order = Convert.ToInt32(reader["sort_order"].ToString()),
                            };
                            list.Add(sm);
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

        public List<prefixNo> get_all_roll_no_prefix()
        {
            List<prefixNo> list = new List<prefixNo>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_prefix_roll_no";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            prefixNo sm = new prefixNo()
                            {
                                id = Convert.ToInt32(reader["id"].ToString()),
                                prefix = Convert.ToString(reader["prefix"].ToString()),
                                prefix_abbreviation = Convert.ToString(reader["prefix_abbreviation"].ToString()),
                                sort_order = Convert.ToInt32(reader["sort_order"].ToString()),
                            };
                            list.Add(sm);
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

        public List<CityArea> get_all_area()
        {
            List<CityArea> list = new List<CityArea>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_city_area";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            CityArea sm = new CityArea()
                            {
                                id = Convert.ToInt32(reader["id"].ToString()),
                                area = Convert.ToString(reader["area"].ToString()),                                
                                sort_order = Convert.ToInt32(reader["sort_order"].ToString()),
                            };
                            list.Add(sm);
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

        public List<Databases> get_all_databases()
        {
            List<Databases> databases_list = new List<Databases>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SHOW DATABASES;";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Databases obj = new Databases()
                            {
                                DatabaseName = Convert.ToString(reader[0]),                                
                            };
                            databases_list.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return databases_list;
        }

        public MySqlConnection OpenOnlineDatabaseConnection()
        {
            try
            {
                MySqlConnection con = new MySqlConnection(Connection_String.tahir123_sms_security);
                con.Open();
                return con;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}
