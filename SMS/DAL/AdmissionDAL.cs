﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using SMS.Models;
using System.IO;

namespace SMS.DAL
{
    public class AdmissionDAL
    {
        public AdmissionDAL()
        {

        }

        public List<admission> get_all_admissions()
        {
            List<admission> adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT " +
                    "adm.image," +
                    "adm.id," +
                    "adm.std_name," +
                    "adm.father_name ," +
                    "adm.father_cnic ," +
                    "adm.father_income ," +
                    "adm.religion ," +
                    "adm.dob ," +
                    "adm.b_form ," +
                    "adm.parmanent_adress ," +
                    "adm.adm_date ," +
                    "adm.cell_no ," +
                    "adm.emergency_address ," +
                    "adm.previous_school ," +
                    "adm.boarding ," +
                    "adm.transport ," +
                    "adm.comm_adress ," +
                    "adm.class_id ," +
                    "adm.class_name ," +
                    "adm.section_id ," +
                    "subj.section_name ," +
                    "adm.roll_no ," +
                    "adm.adm_no ," +
                    "adm.transport_fee ," +
                    "adm.reg_fee ," +
                    "adm.tution_fee ," +
                    "adm.exam_fee ," +
                    "adm.security_fee ," +
                    "adm.stationary_fee ," +
                    "adm.scholarship  ," +
                    "adm.misc_charges  ," +
                    "adm.other_exp  ," +
                    "adm.adm_fee  ," +
                    "adm.total  ," +
                    "adm.date_time  ," +
                    "adm.created_by  ," +
                    "adm.is_active  ," +
                    "adm.fees_package_id   ," +
                    "adm.fees_package   ," +
                    "adm.class_in_id   ," +
                    "adm.adm_no_prefix_id   ," +
                    "adm.roll_no_prefix_id   ," +
                    "adm.city_area_id   ," +
                    "adm.family_group_id   ," +
                    "adm.roll_no_int   ," +
                    "adm.adm_no_int " +
                    "FROM sms_admission as adm Inner join sms_subjects as subj ON adm.section_id=subj.id where adm.is_active='Y' && adm.session_id=" + MainWindow.session.id + " ORDER BY adm.adm_no_int ASC";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader[0] == "")
                        {
                            string path = "/SMS;component/images/Delete-icon.png";
                            img = File.ReadAllBytes(path);
                        }
                        else
                        {
                            img = (byte[])(reader[0]);
                        }

                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader[1].ToString()),
                            std_name = Convert.ToString(reader[2].ToString()),
                            father_name = Convert.ToString(reader[3].ToString()),
                            father_cnic = Convert.ToString(reader[4].ToString()),
                            father_income = Convert.ToString(reader[5].ToString()),
                            religion = Convert.ToString(reader[6].ToString()),
                            dob = Convert.ToDateTime(reader[7]),
                            b_form = Convert.ToString(reader[8].ToString()),
                            parmanent_adress = Convert.ToString(reader[9].ToString()),
                            adm_date = Convert.ToDateTime(reader[10]),
                            cell_no = Convert.ToString(reader[11].ToString()),
                            emergency_address = Convert.ToString(reader[12].ToString()),
                            previous_school = Convert.ToString(reader[13].ToString()),
                            boarding = Convert.ToString(reader[14].ToString()),
                            transport = Convert.ToString(reader[15].ToString()),
                            comm_adress = Convert.ToString(reader[16].ToString()),
                            class_id = Convert.ToString(reader[17].ToString()),
                            class_name = Convert.ToString(reader[18].ToString()),
                            section_id = Convert.ToString(reader[19].ToString()),
                            section_name = Convert.ToString(reader[20].ToString()),
                            roll_no = Convert.ToString(reader[21].ToString()),
                            adm_no = Convert.ToString(reader[22].ToString()),
                            transport_fee = Convert.ToString(reader[23].ToString()),
                            reg_fee = Convert.ToString(reader[24].ToString()),
                            tution_fee = Convert.ToString(reader[25].ToString()),
                            exam_fee = Convert.ToString(reader[26].ToString()),
                            security_fee = Convert.ToString(reader[27].ToString()),
                            stationary_fee = Convert.ToString(reader[28].ToString()),
                            scholarship = Convert.ToString(reader[29].ToString()),
                            misc_charges = Convert.ToString(reader[30].ToString()),
                            other_exp = Convert.ToString(reader[31].ToString()),
                            adm_fee = Convert.ToString(reader[32].ToString()),
                            total = Convert.ToString(reader[33].ToString()),
                            date_time = Convert.ToDateTime(reader[34]),
                            created_by = Convert.ToString(reader[35].ToString()),
                            is_active = Convert.ToString(reader[36].ToString()),

                            fees_package_id = Convert.ToInt32(reader[37]),
                            fees_package = Convert.ToString(reader[38].ToString()),

                            class_in_id = Convert.ToInt32(reader[39]),
                            adm_no_prefix_id = Convert.ToInt32(reader[40]),
                            roll_no_prefix_id = Convert.ToInt32(reader[41]),
                            area_id = Convert.ToInt32(reader[42]),
                            family_group_id = Convert.ToInt32(reader[43]),
                            roll_no_int = Convert.ToInt32(reader[44]),
                            adm_no_int = Convert.ToInt32(reader[45]),

                            image = img,
                        };
                        adm_list.Add(adm);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return adm_list;
            }
        }
        public List<admission> get_all_admissions_YN()
        {
            List<admission> adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT adm.id, adm.std_name, adm.father_name, adm.father_cnic, adm.father_income, adm.religion, adm.dob, adm.b_form, adm.parmanent_adress, adm.adm_date, adm.withdrawal_date, " +
                   "adm.cell_no, adm.emergency_address, adm.previous_school, adm.boarding, adm.transport, adm.comm_adress, " +
                   "adm.class_id, adm.class_name, adm.section_id, adm.section_name, adm.roll_no, adm.adm_no, " +
                   "adm.adm_no_int, adm.date_time, adm.created_by, adm.is_active, adm.fees_package_id, adm.fees_package, " +
                   "adm.image, cl.class_name " +
                    "FROM sms_admission as adm Left join sms_classes as cl ON cl.id=adm.class_in_id where adm.session_id= @session_id Order By adm.adm_date Asc";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;    
                cmd.Parameters.Add("session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;
                try
                {
                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader[0].ToString()),
                            std_name = Convert.ToString(reader[1].ToString()),
                            father_name = Convert.ToString(reader[2].ToString()),
                            father_cnic = Convert.ToString(reader[3].ToString()),
                            father_income = Convert.ToString(reader[4].ToString()),
                            religion = Convert.ToString(reader[5].ToString()),
                            dob = Convert.ToDateTime(reader[6]),
                            b_form = Convert.ToString(reader[7].ToString()),
                            parmanent_adress = Convert.ToString(reader[8].ToString()),
                            adm_date = Convert.ToDateTime(reader[9]),
                            //withdrawal_date = Convert.ToDateTime(reader[10]),
                            cell_no = Convert.ToString(reader[11].ToString()),
                            emergency_address = Convert.ToString(reader[12].ToString()),
                            previous_school = Convert.ToString(reader[13].ToString()),
                            boarding = Convert.ToString(reader[14].ToString()),
                            transport = Convert.ToString(reader[15].ToString()),
                            comm_adress = Convert.ToString(reader[16].ToString()),
                            class_id = Convert.ToString(reader[17].ToString()),
                            class_name = Convert.ToString(reader[18].ToString()),

                            section_id = Convert.ToString(reader[19].ToString()),
                            section_name = Convert.ToString(reader[20].ToString()),
                            roll_no = Convert.ToString(reader[21].ToString()),
                            adm_no = Convert.ToString(reader[22].ToString()),
                            adm_no_int = Convert.ToInt32(reader[23]),
                            //transport_fee = Convert.ToString(reader["transport_fee"].ToString()),
                            //reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                            //tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                            //exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                            //security_fee = Convert.ToString(reader["security_fee"].ToString()),
                            //stationary_fee = Convert.ToString(reader["stationary_fee"].ToString()),
                            //scholarship = Convert.ToString(reader["scholarship"].ToString()),
                            //misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                            //other_exp = Convert.ToString(reader["other_exp"].ToString()),
                            //adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                            //total = Convert.ToString(reader["total"].ToString()),
                            date_time = Convert.ToDateTime(reader[24]),
                            created_by = Convert.ToString(reader[25].ToString()),
                            is_active = Convert.ToString(reader[26].ToString()),

                            fees_package_id = Convert.ToInt32(reader[27]),
                            fees_package = Convert.ToString(reader[28].ToString()),
                            std_image = (byte[])(reader[29]),
                            class_in_name = Convert.ToString(reader[30].ToString()),
                        };
                        if (!reader.IsDBNull(10))
                        {
                            adm.withdrawal_date = Convert.ToDateTime(reader[10]);
                        }
                        adm_list.Add(adm);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return adm_list;
        }
        public List<admission> getAdmissionsBySectionID(int section_id)
        {
            List<admission> adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && section_id=@section_id && session_id=" + MainWindow.session.id + " ORDER BY adm_no_int ASC";
                cmd.Connection = con;
                cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = section_id;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["image"] == "")
                        {
                            string path = "/SMS;component/images/Delete-icon.png";
                            img = File.ReadAllBytes(path);
                        }
                        else
                        {
                            img = (byte[])(reader["image"]);
                        }

                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            father_cnic = Convert.ToString(reader["father_cnic"].ToString()),
                            father_income = Convert.ToString(reader["father_income"].ToString()),
                            religion = Convert.ToString(reader["religion"].ToString()),
                            dob = Convert.ToDateTime(reader["dob"]),
                            b_form = Convert.ToString(reader["b_form"].ToString()),
                            parmanent_adress = Convert.ToString(reader["parmanent_adress"].ToString()),
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            emergency_address = Convert.ToString(reader["emergency_address"].ToString()),
                            previous_school = Convert.ToString(reader["previous_school"].ToString()),
                            boarding = Convert.ToString(reader["boarding"].ToString()),
                            transport = Convert.ToString(reader["transport"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            transport_fee = Convert.ToString(reader["transport_fee"].ToString()),
                            reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                            tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                            exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                            security_fee = Convert.ToString(reader["security_fee"].ToString()),
                            stationary_fee = Convert.ToString(reader["stationary_fee"].ToString()),
                            scholarship = Convert.ToString(reader["scholarship"].ToString()),
                            misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                            other_exp = Convert.ToString(reader["other_exp"].ToString()),
                            adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                            total = Convert.ToString(reader["total"].ToString()),
                            date_time = Convert.ToDateTime(reader["date_time"]),
                            created_by = Convert.ToString(reader["created_by"].ToString()),
                            is_active = Convert.ToString(reader["is_active"].ToString()),

                            fees_package_id = Convert.ToInt32(reader["fees_package_id"]),
                            fees_package = Convert.ToString(reader["fees_package"].ToString()),

                            class_in_id = Convert.ToInt32(reader["class_in_id"]),
                            adm_no_prefix_id = Convert.ToInt32(reader["adm_no_prefix_id"]),
                            roll_no_prefix_id = Convert.ToInt32(reader["roll_no_prefix_id"]),
                            area_id = Convert.ToInt32(reader["city_area_id"]),
                            family_group_id = Convert.ToInt32(reader["family_group_id"]),
                            roll_no_int = Convert.ToInt32(reader["roll_no_int"]),
                            adm_no_int = Convert.ToInt32(reader["adm_no_int"]),

                            image = img,
                        };
                        adm_list.Add(adm);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return adm_list;
            }
        }
        public List<admission> get_all_siblings(admission adm)
        {
            List<admission> adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                //cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && id !="+adm_obj.id+ "&& cell_no="+adm_obj.cell_no+"&& session_id=" + MainWindow.session.id;
                if (string.IsNullOrEmpty(adm.father_cnic) || string.IsNullOrWhiteSpace(adm.father_cnic) || adm.father_cnic.Contains("_"))
                {
                    cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' &&  cell_no=@cell && session_id=" + MainWindow.session.id;
                }
                else
                {
                    cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' &&  father_cnic=@cnic && session_id=" + MainWindow.session.id;
                }
                cmd.Connection = con;
                cmd.Parameters.Add("@cnic", MySqlDbType.VarChar).Value = adm.father_cnic;
                cmd.Parameters.Add("@cell", MySqlDbType.VarChar).Value = adm.cell_no;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["image"] == "")
                        {
                            string path = "/SMS;component/images/Delete-icon.png";
                            img = File.ReadAllBytes(path);
                        }
                        else
                        {
                            img = (byte[])(reader["image"]);
                        }

                        admission adm1 = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            father_cnic = Convert.ToString(reader["father_cnic"].ToString()),
                            father_income = Convert.ToString(reader["father_income"].ToString()),
                            religion = Convert.ToString(reader["religion"].ToString()),
                            dob = Convert.ToDateTime(reader["dob"]),
                            b_form = Convert.ToString(reader["b_form"].ToString()),
                            parmanent_adress = Convert.ToString(reader["parmanent_adress"].ToString()),
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            emergency_address = Convert.ToString(reader["emergency_address"].ToString()),
                            previous_school = Convert.ToString(reader["previous_school"].ToString()),
                            boarding = Convert.ToString(reader["boarding"].ToString()),
                            transport = Convert.ToString(reader["transport"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            transport_fee = Convert.ToString(reader["transport_fee"].ToString()),
                            reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                            tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                            exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                            security_fee = Convert.ToString(reader["security_fee"].ToString()),
                            stationary_fee = Convert.ToString(reader["stationary_fee"].ToString()),
                            scholarship = Convert.ToString(reader["scholarship"].ToString()),
                            misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                            other_exp = Convert.ToString(reader["other_exp"].ToString()),
                            adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                            total = Convert.ToString(reader["total"].ToString()),
                            date_time = Convert.ToDateTime(reader["date_time"]),
                            created_by = Convert.ToString(reader["created_by"].ToString()),
                            is_active = Convert.ToString(reader["is_active"].ToString()),
                            image = img,
                        };
                        adm_list.Add(adm1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return adm_list;
        }
        public List<admission> get_all_admissions_sessions()
        {
            List<admission> adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission ORDER BY adm_no_int ASC";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["image"] == "")
                        {
                            string path = "/SMS;component/images/Delete-icon.png";
                            img = File.ReadAllBytes(path);
                        }
                        else
                        {
                            img = (byte[])(reader["image"]);
                        }

                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            father_cnic = Convert.ToString(reader["father_cnic"].ToString()),
                            father_income = Convert.ToString(reader["father_income"].ToString()),
                            religion = Convert.ToString(reader["religion"].ToString()),
                            dob = Convert.ToDateTime(reader["dob"]),
                            b_form = Convert.ToString(reader["b_form"].ToString()),
                            parmanent_adress = Convert.ToString(reader["parmanent_adress"].ToString()),
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            emergency_address = Convert.ToString(reader["emergency_address"].ToString()),
                            previous_school = Convert.ToString(reader["previous_school"].ToString()),
                            boarding = Convert.ToString(reader["boarding"].ToString()),
                            transport = Convert.ToString(reader["transport"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            transport_fee = Convert.ToString(reader["transport_fee"].ToString()),
                            reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                            tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                            exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                            security_fee = Convert.ToString(reader["security_fee"].ToString()),
                            stationary_fee = Convert.ToString(reader["stationary_fee"].ToString()),
                            scholarship = Convert.ToString(reader["scholarship"].ToString()),
                            misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                            other_exp = Convert.ToString(reader["other_exp"].ToString()),
                            adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                            total = Convert.ToString(reader["total"].ToString()),
                            date_time = Convert.ToDateTime(reader["date_time"]),
                            created_by = Convert.ToString(reader["created_by"].ToString()),
                            is_active = Convert.ToString(reader["is_active"].ToString()),

                            fees_package_id = Convert.ToInt32(reader["fees_package_id"]),
                            fees_package = Convert.ToString(reader["fees_package"].ToString()),

                            class_in_id = Convert.ToInt32(reader["class_in_id"]),
                            adm_no_prefix_id = Convert.ToInt32(reader["adm_no_prefix_id"]),
                            roll_no_prefix_id = Convert.ToInt32(reader["roll_no_prefix_id"]),
                            area_id = Convert.ToInt32(reader["city_area_id"]),
                            family_group_id = Convert.ToInt32(reader["family_group_id"]),
                            roll_no_int = Convert.ToInt32(reader["roll_no_int"]),
                            adm_no_int = Convert.ToInt32(reader["adm_no_int"]),

                            image = img,
                        };
                        adm_list.Add(adm);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return adm_list;
            }
        }
    }
}
