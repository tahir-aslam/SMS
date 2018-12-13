using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.DAL
{
    public class ClassesDAL
    {
        public ClassesDAL()
        {
        }

        public List<classes> getAllClasses()
        {
            List<classes> classes_list = new List<classes>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                transport_charges = Convert.ToString(reader["transport_charges"].ToString()),
                                boarding_fee = Convert.ToString(reader["boarding_fee"].ToString()),
                                misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                stationary_charges = Convert.ToString(reader["stationary_charges"].ToString()),
                                books_charges = Convert.ToString(reader["books_charges"].ToString()),
                                other_exp = Convert.ToString(reader["other_exp"].ToString()),
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            };
                            classes_list.Add(classes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return classes_list;
        }

        public List<sms_fees_actual> getAllFeesClasses()
        {
            List<sms_fees_actual> classes_fees_list = new List<sms_fees_actual>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_classes ORDER BY class_id ASC";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees_actual classes = new sms_fees_actual()
                            {

                                id = Convert.ToInt32(reader["id"]),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"]),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                actual_amount = Convert.ToInt32(reader["amount"]),
                                discount= 0,
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"]),
                            };
                            classes_fees_list.Add(classes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return classes_fees_list;
        }

        public int insertFeesClasses(sms_fees_actual obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_fees_classes(class_id,class_name,fees_category_id,fees_category,fees_sub_category_id,fees_sub_category,amount, created_by, date_time, emp_id) Values(@class_id, @class_name, @fees_category_id,@fees_category,@fees_sub_category_id,@fees_sub_category,@amount, @created_by, @date_time, @emp_id)";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@class_id", MySqlDbType.Int32).Value = obj.class_id;
                        cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = obj.class_name;                        
                        cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = obj.fees_category_id;
                        cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = obj.fees_category;                        
                        cmd.Parameters.Add("@fees_sub_category_id", MySqlDbType.Int32).Value = 0;
                        cmd.Parameters.Add("@fees_sub_category", MySqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = obj.amount;

                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = obj.created_by;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = obj.date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

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

        public int updateFeesClasses(sms_fees_actual obj)
        {
            int i = 0;            
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_fees_classes SET class_id=@class_id, class_name=@class_name, fees_category_id=@fees_category_id,fees_category=@fees_category,fees_sub_category_id=@fees_sub_category_id,fees_sub_category=@fees_sub_category,amount=@amount, created_by=@created_by, date_time=@date_time, emp_id=@emp_id where id = @id";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.id;
                        cmd.Parameters.Add("@class_id", MySqlDbType.Int32).Value = obj.class_id;
                        cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = obj.class_name;                        
                        cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = obj.fees_category_id;
                        cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = obj.fees_category;                       
                        cmd.Parameters.Add("@fees_sub_category_id", MySqlDbType.Int32).Value = obj.fees_sub_category_id;
                        cmd.Parameters.Add("@fees_sub_category", MySqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = obj.amount;

                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = obj.created_by;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = obj.date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

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
        
        public List<classes> get_all_classes()
        {
            List<classes> classes_list = new List<classes>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                transport_charges = Convert.ToString(reader["transport_charges"].ToString()),
                                boarding_fee = Convert.ToString(reader["boarding_fee"].ToString()),
                                misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                stationary_charges = Convert.ToString(reader["stationary_charges"].ToString()),
                                books_charges = Convert.ToString(reader["books_charges"].ToString()),
                                other_exp = Convert.ToString(reader["other_exp"].ToString()),
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            
                            };
                            classes_list.Add(classes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return classes_list;
        }
        
        public List<sections> get_all_sections(string id)
        {
            List<sections> sections_list = new List<sections>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects where class_id = " + id;
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                            };
                            sections_list.Add(section);

                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            return sections_list;
        }
    }
}
