using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.DAL
{
    public class FeesDAL
    {
        AccountsDAL accountsDAL;
        public FeesDAL()
        {
            accountsDAL = new AccountsDAL();
        }
      
        public List<sms_fees_category> get_all_fees_category()
        {
            List<sms_fees_category> fees_category_list = new List<sms_fees_category>();
            sms_fees_category fees_category;
            try
            {
                List<chart_of_accounts> accounts_list = accountsDAL.getAllChartOfAccounts().Where(x => x.p_id == 49).ToList();
                foreach (var item in accounts_list)
                {
                    fees_category = new sms_fees_category()
                    {
                        id = item.id,
                        fees_category = item.account_name,
                        fees_type_id = 0,
                        is_active = "Y",
                    };
                    fees_category_list.Add(fees_category);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fees_category_list;
        }

        public List<sms_fees_sub_category> get_all_fees_sub_category()
        {
            List<sms_fees_sub_category> fees_sub_category_list = new List<sms_fees_sub_category>();
            sms_fees_sub_category fees_sub_category;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_fees_sub_category";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fees_sub_category = new sms_fees_sub_category()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"]),
                                is_active = Convert.ToString(reader["is_active"]),
                            };
                            fees_sub_category_list.Add(fees_sub_category);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fees_sub_category_list;
        }

        //public List<sms_fees_collection_place> getAllFeesCollectionPlace()
        //{
        //    List<sms_fees_collection_place> fees_place_list = new List<sms_fees_collection_place>();
        //    try
        //    {
        //        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand())
        //            {
        //                //cmd.CommandText = "GetAllRoles";
        //                cmd.CommandText = "SELECT* FROM sms_fees_collection_place where is_active='Y'";
        //                cmd.Connection = con;
        //                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
        //                con.Open();
        //                MySqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    sms_fees_collection_place obj = new sms_fees_collection_place()
        //                    {
        //                        id = Convert.ToInt32(reader["id"]),
        //                        place = Convert.ToString(reader["place"]),
        //                        is_active = Convert.ToString(reader["is_active"]),
        //                    };
        //                    fees_place_list.Add(obj);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return fees_place_list;
        //}
        public List<sms_fees_collection_place> getAllFeesCollectionPlace()
        {
            List<sms_fees_collection_place> fees_place_list = new List<sms_fees_collection_place>();
            try
            {
                List<chart_of_accounts> accounts_list = new List<chart_of_accounts>();
                accounts_list = accountsDAL.getAllChartOfAccounts().Where(x=>x.p_id == 15).ToList();
                foreach (var item in accounts_list)
                {
                    sms_fees_collection_place obj = new sms_fees_collection_place()
                    {
                        id = item.id,
                        place = item.account_name,
                        is_active = "y",
                    };
                    fees_place_list.Add(obj);                
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fees_place_list;
        }

        #region Unpaid Fee(sms_fees_generated)

        public List<sms_fees> getAllUnPaidFeesByStdId(int std_id)
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_generated where std_id=@std_id && rem_amount > 0";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                rem_amount = Convert.ToInt32(reader["rem_amount"]),
                                discount = Convert.ToInt32(reader["discount"]),
                                actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                wave_off = Convert.ToInt32(reader["wave_off"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                year = Convert.ToInt32(reader["year"]),
                                date = Convert.ToDateTime(reader["date"]),
                                due_date = Convert.ToDateTime(reader["due_date"]),
                                session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),
                            };
                            fees_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fees_list;
        }
        public List<sms_fees> getAllUnPaidFeesByStdId(List<admission> _lst)
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    MySqlDataReader reader;
                    foreach (var item in _lst)
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            //cmd.CommandText = "GetAllRoles";
                            cmd.CommandText = "SELECT* FROM sms_fees_generated where std_id=@std_id && rem_amount > 0";
                            cmd.Connection = con;
                            cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = item.id;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                                                
                             reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                sms_fees fees = new sms_fees()
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    std_id = Convert.ToInt32(reader["std_id"]),
                                    fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                    fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                    fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                    fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                    amount = Convert.ToInt32(reader["amount"]),
                                    rem_amount = Convert.ToInt32(reader["rem_amount"]),
                                    discount = Convert.ToInt32(reader["discount"]),
                                    actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                    wave_off = Convert.ToInt32(reader["wave_off"]),
                                    month = Convert.ToInt32(reader["month"]),
                                    month_name = Convert.ToString(reader["month_name"].ToString()),
                                    class_id = Convert.ToInt32(reader["class_id"]),
                                    class_name = Convert.ToString(reader["class_name"]),
                                    section_id = Convert.ToInt32(reader["section_id"]),
                                    section_name = Convert.ToString(reader["section_name"]),
                                    year = Convert.ToInt32(reader["year"]),
                                    date = Convert.ToDateTime(reader["date"]),
                                    due_date = Convert.ToDateTime(reader["due_date"]),
                                    session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                    is_active = Convert.ToString(reader["is_active"].ToString()),
                                    created_by = Convert.ToString(reader["created_by"].ToString()),
                                    emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                    date_time = Convert.ToDateTime(reader["date_time"].ToString()),
                                };
                                fees_list.Add(fees);
                            }
                            reader.Close();
                        }

                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return fees_list;
        }
        public List<sms_fees> getAllUnPaidFeesByMonth(int month)
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_generated where month=@month && rem_amount > 0";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@month", MySqlDbType.Int32).Value = month;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                rem_amount = Convert.ToInt32(reader["rem_amount"]),
                                discount = Convert.ToInt32(reader["discount"]),
                                actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                wave_off = Convert.ToInt32(reader["wave_off"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                year = Convert.ToInt32(reader["year"]),
                                date = Convert.ToDateTime(reader["date"]),
                                due_date = Convert.ToDateTime(reader["due_date"]),
                                session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),
                            };
                            fees_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fees_list;
        }
        public List<sms_fees> getAllUnPaidFees()
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT fee.id, fee.std_id, fee.fees_category_id, fee.fees_category, fee.fees_sub_category, fee.fees_sub_category_id, "+
                                        "fee.amount, fee.rem_amount, fee.discount, fee.actual_amount, fee.wave_off, fee.month, fee.month_name, "+
                                        "adm.class_id, adm.class_name, adm.section_id, adm.section_name, "+
                                        "fee.year, fee.date, fee.due_date, "+
                                        "adm.session_id, fee.is_active, adm.is_active, fee.created_by, fee.emp_id, fee.date_time, "+
                                        "adm.std_name, adm.father_name, adm.cell_no, adm.adm_no, adm.roll_no "+
                                        "FROM sms_fees_generated As fee "+
                                        "INNER JOIN sms_admission As adm ON fee.std_id = adm.id "+
                                        "where adm.session_id = (select adm_inner.session_id from sms_admission as adm_inner where adm_inner.id = fee.std_id order by adm_inner.session_id DESC Limit 1) && fee.rem_amount > 0";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader[0]),
                                std_id = Convert.ToInt32(reader[1]),
                                fees_category_id = Convert.ToInt32(reader[2]),
                                fees_category = Convert.ToString(reader[3].ToString()),
                                fees_sub_category = Convert.ToString(reader[4].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader[5]),
                                amount = Convert.ToInt32(reader[6]),
                                rem_amount = Convert.ToInt32(reader[7]),
                                discount = Convert.ToInt32(reader[8]),
                                actual_amount = Convert.ToInt32(reader[9]),
                                wave_off = Convert.ToInt32(reader[10]),
                                month = Convert.ToInt32(reader[11]),
                                month_name = Convert.ToString(reader[12].ToString()),
                                class_id = Convert.ToInt32(reader[13]),
                                class_name = Convert.ToString(reader[14]),
                                section_id = Convert.ToInt32(reader[15]),
                                section_name = Convert.ToString(reader[16]),
                                year = Convert.ToInt32(reader[17]),
                                date = Convert.ToDateTime(reader[18]),
                                due_date = Convert.ToDateTime(reader[19]),
                                session_id = Convert.ToInt32(reader[20].ToString()),
                                is_active = Convert.ToString(reader[21].ToString()),
                                adm_is_active = Convert.ToString(reader[22].ToString()),
                                created_by = Convert.ToString(reader[23].ToString()),
                                emp_id = Convert.ToInt32(reader[24].ToString()),
                                date_time = Convert.ToDateTime(reader[25].ToString()),

                                //std_image = (byte[])(reader["image"]),
                                std_name = Convert.ToString(reader[26]),
                                father_name = Convert.ToString(reader[27]),
                                cell_no = Convert.ToString(reader[28]),
                                adm_no = Convert.ToString(reader[29]),
                                roll_no = Convert.ToString(reader[30]),

                            };
                            fees_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if(fees_list.Count > 0)
            {
                //fees_list = fees_list.OrderByDescending(x => x.month).OrderByDescending(x => x.year).OrderByDescending(x => x.std_id).ToList();                
                //foreach (var item in fees_list.Select(x=>x.std_id).Distinct())
                //{
                //    foreach (var item1 in fees_list.Where(x=>x.std_id == item))
                //    {
                //        item1.class_id = fees_list.Where(x => x.std_id == item).First().class_id;
                //        item1.class_name = fees_list.Where(x => x.std_id == item).First().class_name;
                //        item1.section_id = fees_list.Where(x => x.std_id == item).First().section_id;
                //        item1.section_name = fees_list.Where(x => x.std_id == item).First().section_name;
                //    }
                //}
            }
            return fees_list;
        }

        #endregion

        //public int get_last_reciept_no()
        //{
        //    int receipt_no = 0;
        //    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
        //    {
        //        using (MySqlCommand cmd = new MySqlCommand())
        //        {
        //            cmd.CommandText = "SELECT* FROM sms_last_receipt_no ";
        //            cmd.Connection = con;
        //            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
        //            try
        //            {
        //                con.Open();
        //                MySqlDataReader reader = cmd.ExecuteReader();
        //                reader.Read();

        //                receipt_no = Convert.ToInt32(reader["last_receipt_no"]);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //    }
        //    return receipt_no;
        //}

        //public int update_last_receipt_no(int receipt_no) 
        //{
        //    int i = 0;
        //    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
        //    {
        //        using (MySqlCommand cmd = new MySqlCommand())
        //        {
        //            cmd.CommandText = "Update sms_last_receipt_no SET last_receipt_no=@last_receipt_no";
        //            cmd.Connection = con;

        //            con.Open();
        //            cmd.Parameters.Add("@last_receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = receipt_no.ToString();
        //            i = Convert.ToInt32(cmd.ExecuteNonQuery());                    
        //            con.Close();
        //        }
        //    }
        //    return i;
        //}

        #region submit fees (sms_fees_paid)

        public int submitFees(List<sms_fees> feesList, int receipt_no, List<sms_fees> historyList, sms_voucher voucher, List<sms_voucher_entries> voucherEntriesList )
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            foreach (sms_fees fee in feesList)
                            {
                                i = 0;
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_fees_paid(std_id,fees_generated_id, fees_category_id, fees_category, fees_sub_category_id, fees_sub_category, amount,amount_paid, month, month_name, class_id, class_name, section_id, section_name, year, date, session_id, created_by, emp_id, date_time, receipt_no,fees_collection_place_id,fees_collection_place, receipt_no_full, total_amount,total_paid, total_remaining, amount_in_words, discount, wave_off) Values(@std_id, @fees_generated_id, @fees_category_id,@fees_category,@fees_sub_category_id,@fees_sub_category,@amount,@amount_paid,@month,@month_name,@class_id,@class_name,@section_id,@section_name,@year,@date,@session_id,@created_by,@emp_id,@date_time, @receipt_no, @fees_collection_place_id, @fees_collection_place, @receipt_no_full, @total_amount,@total_paid, @total_remaining, @amount_in_words, @discount, @wave_off)";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                    cmd.Parameters.Add("@fees_generated_id", MySqlDbType.Int32).Value = fee.id;
                                    cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = fee.fees_category_id;
                                    cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = fee.fees_category;
                                    cmd.Parameters.Add("@fees_sub_category_id", MySqlDbType.Int32).Value = fee.fees_sub_category_id;
                                    cmd.Parameters.Add("@fees_sub_category", MySqlDbType.VarChar).Value = fee.fees_sub_category;
                                    cmd.Parameters.Add("@amount_paid", MySqlDbType.Int32).Value = fee.amount_paid;
                                    cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = fee.amount;
                                    cmd.Parameters.Add("@month", MySqlDbType.Int32).Value = fee.month;
                                    cmd.Parameters.Add("@month_name", MySqlDbType.VarChar).Value = fee.month_name;
                                    cmd.Parameters.Add("@year", MySqlDbType.Int32).Value = fee.year;
                                    cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = fee.date;
                                    cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = fee.receipt_no;
                                    cmd.Parameters.Add("@receipt_no_full", MySqlDbType.VarChar).Value = fee.receipt_no_full;
                                    cmd.Parameters.Add("@class_id", MySqlDbType.Int32).Value = fee.class_id;
                                    cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = fee.class_name;
                                    cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = fee.section_id;
                                    cmd.Parameters.Add("@section_name", MySqlDbType.VarChar).Value = fee.section_name;
                                    cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = fee.session_id;
                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = fee.created_by;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = fee.date_time;

                                    cmd.Parameters.Add("@fees_collection_place_id", MySqlDbType.Int32).Value = fee.fees_collection_place_id;
                                    cmd.Parameters.Add("@fees_collection_place", MySqlDbType.VarChar).Value = fee.fees_collection_place;

                                    cmd.Parameters.Add("@total_amount", MySqlDbType.Int32).Value = fee.total_amount;
                                    cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = fee.total_paid;
                                    cmd.Parameters.Add("@total_remaining", MySqlDbType.Int32).Value = fee.total_remaining;
                                    cmd.Parameters.Add("@amount_in_words", MySqlDbType.VarChar).Value = fee.amount_in_words;

                                    cmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = fee.discount;
                                    cmd.Parameters.Add("@wave_off", MySqlDbType.Int32).Value = fee.wave_off;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }

                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_fees_generated SET rem_amount=@rem_amount where id=@id";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = fee.id;
                                    cmd.Parameters.Add("@rem_amount", MySqlDbType.Int32).Value = fee.rem_amount - fee.amount_paid;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }
                            }

                            foreach (var fee in historyList)
                            {
                                i = 0;
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_fees_voucher_history(std_id, std_name, father_name, adm_no, fees_category, amount, month_name, class_name, section_name, date, due_date, receipt_no, receipt_no_full, fees_collection_place, total_amount,total_paid, total_remaining, amount_in_words, discount, wave_off, emp_id) Values(@std_id, @std_name, @father_name, @adm_no, @fees_category, @amount, @month_name, @class_name, @section_name, @date, @due_date, @receipt_no, @receipt_no_full, @fees_collection_place, @total_amount, @total_paid, @total_remaining, @amount_in_words, @discount, @wave_off, @emp_id)";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                    cmd.Parameters.Add("@std_name", MySqlDbType.VarChar).Value = fee.std_name;
                                    cmd.Parameters.Add("@father_name", MySqlDbType.VarChar).Value = fee.father_name;
                                    cmd.Parameters.Add("@adm_no", MySqlDbType.VarChar).Value = fee.adm_no;
                                    cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = fee.fees_category;
                                    cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = fee.rem_amount;
                                    cmd.Parameters.Add("@month_name", MySqlDbType.VarChar).Value = fee.month_name;
                                    cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = fee.class_name;
                                    cmd.Parameters.Add("@section_name", MySqlDbType.VarChar).Value = fee.section_name;
                                    cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = fee.date;
                                    cmd.Parameters.Add("@due_date", MySqlDbType.DateTime).Value = fee.due_date;
                                    cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = fee.receipt_no;
                                    cmd.Parameters.Add("@receipt_no_full", MySqlDbType.VarChar).Value = fee.receipt_no_full;
                                    cmd.Parameters.Add("@fees_collection_place", MySqlDbType.VarChar).Value = fee.fees_collection_place;
                                    cmd.Parameters.Add("@total_amount", MySqlDbType.Int32).Value = fee.total_amount;
                                    cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = fee.total_paid;
                                    cmd.Parameters.Add("@total_remaining", MySqlDbType.Int32).Value = fee.total_remaining;
                                    cmd.Parameters.Add("@amount_in_words", MySqlDbType.VarChar).Value = fee.amount_in_words;
                                    cmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = fee.discount;
                                    cmd.Parameters.Add("@wave_off", MySqlDbType.Int32).Value = fee.wave_off;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }
                            }

                            //update CRV#
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "update sms_voucher_types SET last_voucher_no=@voucher_no_int, last_voucher=@voucher_no where id = @voucher_id";
                                cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = 4;
                                cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = receipt_no;
                                cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = "CRV-" + DateTime.Now.ToString("yy") + "-" + receipt_no.ToString("D6");

                                cmd.Connection = con;
                                cmd.Transaction = trans;
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            }

                            //Voucher
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_voucher(voucher_no,voucher_no_int, voucher_type_id, voucher_type, voucher_date, voucher_description, amount, cheque_date,cheque_no, created_by, date_time, emp_id, is_posted) Values(@voucher_no,@voucher_no_int, @voucher_type_id, @voucher_type, @voucher_date, @voucher_description, @amount, @cheque_date,@cheque_no, @created_by, @date_time, @emp_id, @is_posted); SELECT LAST_INSERT_ID()";
                                cmd.Connection = con;
                                cmd.Transaction = trans;

                                cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = voucher.voucher_no;
                                cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = voucher.voucher_no_int;
                                cmd.Parameters.Add("@voucher_type_id", MySqlDbType.Int32).Value = voucher.voucher_type_id;
                                cmd.Parameters.Add("@voucher_type", MySqlDbType.VarChar).Value = voucher.voucher_type;
                                cmd.Parameters.Add("@voucher_date", MySqlDbType.Date).Value = voucher.voucher_date;
                                cmd.Parameters.Add("@voucher_description", MySqlDbType.VarChar).Value = voucher.voucher_description;
                                cmd.Parameters.Add("@amount", MySqlDbType.Double).Value = voucher.amount;
                                cmd.Parameters.Add("@cheque_date", MySqlDbType.Date).Value = voucher.cheque_date;
                                cmd.Parameters.Add("@cheque_no", MySqlDbType.Int32).Value = voucher.cheque_no;
                                cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = voucher.created_by;
                                cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = voucher.date_time;
                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = voucher.emp_id;
                                cmd.Parameters.Add("@is_posted", MySqlDbType.VarChar).Value = voucher.is_posted;

                                i = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();
                            }

                            //Voucher Entries
                            foreach (sms_voucher_entries voucherEntry in voucherEntriesList)
                            {
                                
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_voucher_entries(voucher_id,voucher_no,voucher_no_int,voucher_type_id,voucher_type, account_head_id,account_head,account_detail, account_detail_id, description, debit, credit, balance, created_by, date_time, emp_id) Values(@voucher_id,@voucher_no,@voucher_no_int,@voucher_type_id,@voucher_type, @account_head_id, @account_head,@account_detail, @account_detail_id,  @description, @debit, @credit, @balance, @created_by, @date_time, @emp_id)";
                                    cmd.Connection = con;

                                    cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = i;
                                    cmd.Parameters.Add("@account_head_id", MySqlDbType.Int32).Value = voucherEntry.account_head_id;
                                    cmd.Parameters.Add("@account_head", MySqlDbType.VarChar).Value = voucherEntry.account_head;
                                    cmd.Parameters.Add("@account_detail_id", MySqlDbType.Int32).Value = voucherEntry.account_detail_id;
                                    cmd.Parameters.Add("@account_detail", MySqlDbType.VarChar).Value = voucherEntry.account_detail;
                                    cmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = voucherEntry.description;
                                    cmd.Parameters.Add("@debit", MySqlDbType.Double).Value = voucherEntry.debit;
                                    cmd.Parameters.Add("@credit", MySqlDbType.Double).Value = voucherEntry.credit;
                                    cmd.Parameters.Add("@balance", MySqlDbType.Double).Value = voucherEntry.balance;

                                    cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = voucherEntry.voucher_no;
                                    cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = voucherEntry.voucher_no_int;
                                    cmd.Parameters.Add("@voucher_type_id", MySqlDbType.Int32).Value = voucherEntry.voucher_type_id;
                                    cmd.Parameters.Add("@voucher_type", MySqlDbType.VarChar).Value = voucherEntry.voucher_type;

                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = voucherEntry.created_by;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = voucherEntry.date_time;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = voucherEntry.emp_id;

                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                }                               
                            }

                            trans.Commit();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        public int submitFeesVoucher(List<sms_fees> feesList, List<sms_fees> historyList, sms_fees obj, sms_voucher voucher, List<sms_voucher_entries> voucherEntriesList)
        {
            int i = 0;

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            foreach (sms_fees fee in feesList)
                            {
                                i = 0;
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_fees_paid(std_id,fees_generated_id, fees_category_id, fees_category, fees_sub_category_id, fees_sub_category, amount,amount_paid, month, month_name, class_id, class_name, section_id, section_name, year, date, session_id, created_by, emp_id, date_time, receipt_no,fees_collection_place_id,fees_collection_place, receipt_no_full, total_amount,total_paid, total_remaining, amount_in_words, discount, wave_off) Values(@std_id, @fees_generated_id, @fees_category_id,@fees_category,@fees_sub_category_id,@fees_sub_category,@amount,@amount_paid,@month,@month_name,@class_id,@class_name,@section_id,@section_name,@year,@date,@session_id,@created_by,@emp_id,@date_time, @receipt_no, @fees_collection_place_id, @fees_collection_place, @receipt_no_full, @total_amount,@total_paid, @total_remaining, @amount_in_words, @discount, @wave_off)";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                    cmd.Parameters.Add("@fees_generated_id", MySqlDbType.Int32).Value = fee.id;
                                    cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = fee.fees_category_id;
                                    cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = fee.fees_category;
                                    cmd.Parameters.Add("@fees_sub_category_id", MySqlDbType.Int32).Value = fee.fees_sub_category_id;
                                    cmd.Parameters.Add("@fees_sub_category", MySqlDbType.VarChar).Value = fee.fees_sub_category;
                                    cmd.Parameters.Add("@amount_paid", MySqlDbType.Int32).Value = fee.amount_paid;
                                    cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = fee.amount;
                                    cmd.Parameters.Add("@month", MySqlDbType.Int32).Value = fee.month;
                                    cmd.Parameters.Add("@month_name", MySqlDbType.VarChar).Value = fee.month_name;
                                    cmd.Parameters.Add("@year", MySqlDbType.Int32).Value = fee.year;
                                    cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = fee.date;
                                    cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = fee.receipt_no;
                                    cmd.Parameters.Add("@receipt_no_full", MySqlDbType.VarChar).Value = fee.receipt_no_full;
                                    cmd.Parameters.Add("@class_id", MySqlDbType.Int32).Value = fee.class_id;
                                    cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = fee.class_name;
                                    cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = fee.section_id;
                                    cmd.Parameters.Add("@section_name", MySqlDbType.VarChar).Value = fee.section_name;
                                    cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = fee.session_id;
                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = fee.created_by;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = fee.date_time;

                                    cmd.Parameters.Add("@fees_collection_place_id", MySqlDbType.Int32).Value = fee.fees_collection_place_id;
                                    cmd.Parameters.Add("@fees_collection_place", MySqlDbType.VarChar).Value = fee.fees_collection_place;

                                    cmd.Parameters.Add("@total_amount", MySqlDbType.Int32).Value = fee.total_amount;
                                    cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = fee.total_paid;
                                    cmd.Parameters.Add("@total_remaining", MySqlDbType.Int32).Value = fee.total_remaining;
                                    cmd.Parameters.Add("@amount_in_words", MySqlDbType.VarChar).Value = fee.amount_in_words;

                                    cmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = fee.discount;
                                    cmd.Parameters.Add("@wave_off", MySqlDbType.Int32).Value = fee.wave_off;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }

                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_fees_generated SET rem_amount=@rem_amount where id=@id";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = fee.id;
                                    cmd.Parameters.Add("@rem_amount", MySqlDbType.Int32).Value = fee.rem_amount - fee.amount_paid;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }
                            }

                            foreach (var fee in historyList)
                            {
                                i = 0;
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_fees_voucher_history(std_id, std_name, father_name, adm_no, fees_category, amount, month_name, class_name, section_name, date, due_date, receipt_no, receipt_no_full, fees_collection_place, total_amount,total_paid, total_remaining, amount_in_words, discount, wave_off, emp_id) Values(@std_id, @std_name, @father_name, @adm_no, @fees_category, @amount, @month_name, @class_name, @section_name, @date, @due_date, @receipt_no, @receipt_no_full, @fees_collection_place, @total_amount, @total_paid, @total_remaining, @amount_in_words, @discount, @wave_off, @emp_id)";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                    cmd.Parameters.Add("@std_name", MySqlDbType.VarChar).Value = fee.std_name;
                                    cmd.Parameters.Add("@father_name", MySqlDbType.VarChar).Value = fee.father_name;
                                    cmd.Parameters.Add("@adm_no", MySqlDbType.VarChar).Value = fee.adm_no;
                                    cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = fee.fees_category;
                                    cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = fee.rem_amount;
                                    cmd.Parameters.Add("@month_name", MySqlDbType.VarChar).Value = fee.month_name;
                                    cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = fee.class_name;
                                    cmd.Parameters.Add("@section_name", MySqlDbType.VarChar).Value = fee.section_name;
                                    cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = fee.date;
                                    cmd.Parameters.Add("@due_date", MySqlDbType.DateTime).Value = fee.due_date;
                                    cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = fee.receipt_no;
                                    cmd.Parameters.Add("@receipt_no_full", MySqlDbType.VarChar).Value = fee.receipt_no_full;
                                    cmd.Parameters.Add("@fees_collection_place", MySqlDbType.VarChar).Value = fee.fees_collection_place;
                                    cmd.Parameters.Add("@total_amount", MySqlDbType.Int32).Value = fee.total_amount;
                                    cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = fee.total_paid;
                                    cmd.Parameters.Add("@total_remaining", MySqlDbType.Int32).Value = fee.total_remaining;
                                    cmd.Parameters.Add("@amount_in_words", MySqlDbType.VarChar).Value = fee.amount_in_words;
                                    cmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = fee.discount;
                                    cmd.Parameters.Add("@wave_off", MySqlDbType.Int32).Value = fee.wave_off;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }
                            }

                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Update sms_fees_voucher SET is_active='N', created_by=@created_by, date_time=@date_time, emp_id=@emp_id, total_paid=@total_paid where receipt_no = @receipt_no";
                                cmd.Connection = con;
                                cmd.Transaction = trans;

                                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.id;
                                cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = obj.receipt_no;
                                cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = obj.total_paid;

                                cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = obj.created_by;
                                cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = obj.date_time;
                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                cmd.Parameters.Clear();
                            }

                            //Voucher
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_voucher(voucher_no,voucher_no_int, voucher_type_id, voucher_type, voucher_date, voucher_description, amount, cheque_date,cheque_no, created_by, date_time, emp_id, is_posted) Values(@voucher_no,@voucher_no_int, @voucher_type_id, @voucher_type, @voucher_date, @voucher_description, @amount, @cheque_date,@cheque_no, @created_by, @date_time, @emp_id, @is_posted); SELECT LAST_INSERT_ID()";
                                cmd.Connection = con;
                                cmd.Transaction = trans;

                                cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = voucher.voucher_no;
                                cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = voucher.voucher_no_int;
                                cmd.Parameters.Add("@voucher_type_id", MySqlDbType.Int32).Value = voucher.voucher_type_id;
                                cmd.Parameters.Add("@voucher_type", MySqlDbType.VarChar).Value = voucher.voucher_type;
                                cmd.Parameters.Add("@voucher_date", MySqlDbType.Date).Value = voucher.voucher_date;
                                cmd.Parameters.Add("@voucher_description", MySqlDbType.VarChar).Value = voucher.voucher_description;
                                cmd.Parameters.Add("@amount", MySqlDbType.Double).Value = voucher.amount;
                                cmd.Parameters.Add("@cheque_date", MySqlDbType.Date).Value = voucher.cheque_date;
                                cmd.Parameters.Add("@cheque_no", MySqlDbType.Int32).Value = voucher.cheque_no;
                                cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = voucher.created_by;
                                cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = voucher.date_time;
                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = voucher.emp_id;
                                cmd.Parameters.Add("@is_posted", MySqlDbType.VarChar).Value = voucher.is_posted;

                                i = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();
                            }

                            //Voucher Entries
                            foreach (sms_voucher_entries voucherEntry in voucherEntriesList)
                            {

                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_voucher_entries(voucher_id,voucher_no,voucher_no_int,voucher_type_id,voucher_type, account_head_id,account_head,account_detail, account_detail_id, description, debit, credit, balance, created_by, date_time, emp_id) Values(@voucher_id,@voucher_no,@voucher_no_int,@voucher_type_id,@voucher_type, @account_head_id, @account_head,@account_detail, @account_detail_id,  @description, @debit, @credit, @balance, @created_by, @date_time, @emp_id)";
                                    cmd.Connection = con;

                                    cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = i;
                                    cmd.Parameters.Add("@account_head_id", MySqlDbType.Int32).Value = voucherEntry.account_head_id;
                                    cmd.Parameters.Add("@account_head", MySqlDbType.VarChar).Value = voucherEntry.account_head;
                                    cmd.Parameters.Add("@account_detail_id", MySqlDbType.Int32).Value = voucherEntry.account_detail_id;
                                    cmd.Parameters.Add("@account_detail", MySqlDbType.VarChar).Value = voucherEntry.account_detail;
                                    cmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = voucherEntry.description;
                                    cmd.Parameters.Add("@debit", MySqlDbType.Double).Value = voucherEntry.debit;
                                    cmd.Parameters.Add("@credit", MySqlDbType.Double).Value = voucherEntry.credit;
                                    cmd.Parameters.Add("@balance", MySqlDbType.Double).Value = voucherEntry.balance;

                                    cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = voucherEntry.voucher_no;
                                    cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = voucherEntry.voucher_no_int;
                                    cmd.Parameters.Add("@voucher_type_id", MySqlDbType.Int32).Value = voucherEntry.voucher_type_id;
                                    cmd.Parameters.Add("@voucher_type", MySqlDbType.VarChar).Value = voucherEntry.voucher_type;

                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = voucherEntry.created_by;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = voucherEntry.date_time;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = voucherEntry.emp_id;

                                    cmd.ExecuteNonQuery();
                                    cmd.Parameters.Clear();
                                }
                            }


                            trans.Commit();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        #endregion

        #region Fees History (sms_fees_paid)

        public List<sms_fees> getFeesPaidByStdId(int std_id)
        {
            List<sms_fees> feesHistoryList = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_paid where std_id= @std_id ORDER BY date_time DESC";
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                amount_paid = Convert.ToInt32(reader["amount_paid"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                year = Convert.ToInt32(reader["year"]),
                                date = Convert.ToDateTime(reader["date"]),
                                session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),

                                fees_collection_place_id = Convert.ToInt32(reader["fees_collection_place_id"]),
                                fees_collection_place = Convert.ToString(reader["fees_collection_place"]),

                                receipt_no_full = Convert.ToString(reader["receipt_no_full"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                                total_paid = Convert.ToInt32(reader["total_paid"]),
                                total_remaining = Convert.ToInt32(reader["total_remaining"]),
                                amount_in_words = Convert.ToString(reader["amount_in_words"].ToString()),

                                discount = Convert.ToInt32(reader["discount"]),
                                wave_off = Convert.ToInt32(reader["wave_off"]),
                            };
                            feesHistoryList.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return feesHistoryList;
        }

        public List<sms_fees> getFeesPaidByDate(DateTime sDate, DateTime eDate)
        {
            List<sms_fees> feesHistoryList = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "SELECT * FROM sms_fees_paid AS fee_p INNER JOIN sms_admission AS adm ON fee_p.std_id = adm.id INNER JOIN sms_fees_generated AS fee_g ON fee_p.fees_generated_id = fee_g.id where DATE(fee_p.Date) >= @sDate && DATE(fee_p.Date) <= @eDate && adm.session_id=@session_id ORDER BY fee_p.date_time DESC";
                        cmd.CommandText = "SELECT fee_p.id, fee_p.std_id,fee_p.fees_generated_id, fee_p.fees_category_id,fee_p.fees_category, fee_g.amount, fee_p.amount_paid, fee_p.amount, fee_p.discount, fee_p.wave_off, fee_p.month, fee_p.month_name, fee_p.receipt_no, fee_p.receipt_no_full, adm.class_id, adm.class_name, adm.section_id, adm.section_name, fee_p.year,  fee_p.date, fee_g.due_date, fee_p.session_id, fee_p.created_by, fee_p.emp_id, fee_p.date_time, fee_p.fees_collection_place_id, fee_p.fees_collection_place, fee_p.total_amount,fee_p.total_paid, fee_p.total_remaining, fee_p.amount_in_words, adm.image, adm.std_name, adm.father_name, adm.cell_no, adm.adm_no, fee_g.discount FROM sms_fees_paid AS fee_p INNER JOIN sms_admission AS adm ON fee_p.std_id = adm.id INNER JOIN sms_fees_generated AS fee_g ON fee_p.fees_generated_id = fee_g.id where DATE(fee_p.Date) >= @sDate && DATE(fee_p.Date) <= @eDate && adm.session_id=(select adm_inner.session_id from sms_admission as adm_inner where adm_inner.id=fee_p.std_id order by adm_inner.session_id DESC Limit 1) ORDER BY fee_p.date_time DESC,fee_p.fees_category_id=113 DESC";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;

                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader[0]),
                                std_id = Convert.ToInt32(reader[1]),
                                fees_generated_id = Convert.ToInt32(reader[2]),
                                fees_category_id = Convert.ToInt32(reader[3]),
                                fees_category = Convert.ToString(reader[4]),
                                actual_amount = Convert.ToInt32(reader[5]),
                                amount_paid = Convert.ToInt32(reader[6]),
                                amount = Convert.ToInt32(reader[7]),
                                rem_amount = Convert.ToInt32(reader[7]) - Convert.ToInt32(reader[6]),
                                discount = Convert.ToInt32(reader[8]),
                                wave_off = Convert.ToInt32(reader[9]),
                                month = Convert.ToInt32(reader[10]),
                                month_name = Convert.ToString(reader[11]),
                                receipt_no = Convert.ToInt32(reader[12]),
                                receipt_no_full = Convert.ToString(reader[13]),
                                class_id = Convert.ToInt32(reader[14]),
                                class_name = Convert.ToString(reader[15]),
                                section_id = Convert.ToInt32(reader[16]),
                                section_name = Convert.ToString(reader[17]),
                                year = Convert.ToInt32(reader[18]),
                                date = Convert.ToDateTime(reader[19]),
                                due_date = Convert.ToDateTime(reader[20]),
                                session_id = Convert.ToInt32(reader[21]),
                                created_by = Convert.ToString(reader[22]),
                                emp_id = Convert.ToInt32(reader[23]),
                                date_time = Convert.ToDateTime(reader[24]),

                                fees_collection_place_id = Convert.ToInt32(reader[25]),
                                fees_collection_place = Convert.ToString(reader[26]),

                                total_amount = Convert.ToInt32(reader[27]),
                                total_paid = Convert.ToInt32(reader[28]),
                                total_remaining = Convert.ToInt32(reader[29]),
                                amount_in_words = Convert.ToString(reader[30]),

                                std_image = (byte[])(reader[31]),
                                std_name = Convert.ToString(reader[32]),
                                father_name = Convert.ToString(reader[33]),
                                cell_no = Convert.ToString(reader[34]),
                                adm_no = Convert.ToString(reader[35]),
                                Std_discount_tution_fee = Convert.ToInt32(reader[36]),

                                //id = Convert.ToInt32(reader["id"]),
                                //std_id = Convert.ToInt32(reader["std_id"]),
                                //fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                //fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                //fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                //actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                //amount_paid = Convert.ToInt32(reader["amount_paid"]),
                                //amount = Convert.ToInt32(reader["amount"]),
                                //rem_amount = Convert.ToInt32(reader["amount"]) - Convert.ToInt32(reader["amount_paid"]),
                                //discount = Convert.ToInt32(reader["discount"]),
                                //wave_off = Convert.ToInt32(reader["wave_off"]),
                                //month = Convert.ToInt32(reader["month"]),
                                //month_name = Convert.ToString(reader["month_name"].ToString()),
                                //receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                //receipt_no_full = Convert.ToString(reader["receipt_no_full"]),
                                //class_id = Convert.ToInt32(reader["class_id"]),
                                //class_name = Convert.ToString(reader["class_name"]),
                                //section_id = Convert.ToInt32(reader["section_id"]),
                                //section_name = Convert.ToString(reader["section_name"]),
                                //year = Convert.ToInt32(reader["year"]),
                                //date = Convert.ToDateTime(reader["date"]),
                                //due_date = Convert.ToDateTime(reader["due_date"]),
                                //session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                //created_by = Convert.ToString(reader["created_by"].ToString()),
                                //emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                //date_time = Convert.ToDateTime(reader["date_time"].ToString()),

                                //fees_collection_place_id = Convert.ToInt32(reader["fees_collection_place_id"]),
                                //fees_collection_place = Convert.ToString(reader["fees_collection_place"]),
                                
                                //total_amount = Convert.ToInt32(reader["total_amount"]),
                                //total_paid = Convert.ToInt32(reader["total_paid"]),
                                //total_remaining = Convert.ToInt32(reader["total_remaining"]),
                                //amount_in_words = Convert.ToString(reader["amount_in_words"].ToString()),

                                //std_image = (byte[])(reader["image"]),
                                //std_name = Convert.ToString(reader["std_name"]),
                                //father_name = Convert.ToString(reader["father_name"]),
                                //cell_no = Convert.ToString(reader["cell_no"]),
                                //adm_no = Convert.ToString(reader["adm_no"]),

                                

                            };
                            feesHistoryList.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return feesHistoryList;
        }

        public List<sms_fees> getFeesPaidByDateGroupByReceiptNo(DateTime sDate, DateTime eDate)
        {
            List<sms_fees> feesHistoryList = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT * FROM sms_fees_paid AS fee_p INNER JOIN sms_admission AS adm ON fee_p.std_id = adm.id INNER JOIN sms_fees_generated AS fee_g ON fee_p.fees_generated_id = fee_g.id where DATE(fee_p.Date) >= @sDate && DATE(fee_p.Date) <= @eDate && adm.session_id=@session_id ORDER BY fee_p.date_time DESC";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;

                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                amount_paid = Convert.ToInt32(reader["amount_paid"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                rem_amount = Convert.ToInt32(reader["amount"]) - Convert.ToInt32(reader["amount_paid"]),
                                discount = Convert.ToInt32(reader["discount"]),
                                wave_off = Convert.ToInt32(reader["wave_off"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                year = Convert.ToInt32(reader["year"]),
                                date = Convert.ToDateTime(reader["date"]),
                                due_date = Convert.ToDateTime(reader["due_date"]),
                                session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),

                                fees_collection_place_id = Convert.ToInt32(reader["fees_collection_place_id"]),
                                fees_collection_place = Convert.ToString(reader["fees_collection_place"]),

                                receipt_no_full = Convert.ToString(reader["receipt_no_full"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                                total_paid = Convert.ToInt32(reader["total_paid"]),
                                total_remaining = Convert.ToInt32(reader["total_remaining"]),
                                amount_in_words = Convert.ToString(reader["amount_in_words"].ToString()),

                                std_image = (byte[])(reader["image"]),
                                std_name = Convert.ToString(reader["std_name"]),
                                father_name = Convert.ToString(reader["father_name"]),
                                cell_no = Convert.ToString(reader["cell_no"]),
                                adm_no = Convert.ToString(reader["adm_no"]),

                                actual_amount = Convert.ToInt32(reader["actual_amount"]),

                            };
                            feesHistoryList.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return feesHistoryList;
        }

        #endregion

        public List<sms_fees_package> getAllFeesPackage()
        {
            List<sms_fees_package> package_list = new List<sms_fees_package>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_package";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees_package fees = new sms_fees_package()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                package_name = Convert.ToString(reader["package_name"].ToString()),
                                is_free = Convert.ToString(reader["is_free"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                            };
                            package_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return package_list;
        }

        public List<sms_fees_actual> getActualFeesByStdID(int std_id)
        {
            List<sms_fees_actual> fees_list = new List<sms_fees_actual>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_actual where std_id = @std_id";
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees_actual fees = new sms_fees_actual()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                amount = Convert.ToInt32(reader["amount"]),
                                discount = Convert.ToInt32(reader["discount"]),
                                actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),
                            };
                            fees_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fees_list;
        }

        public List<sms_fees_actual> get_all_actual_fees()
        {
            List<sms_fees_actual> feesList = new List<sms_fees_actual>();
            sms_fees_actual actual_fees;

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_fees_actual";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            actual_fees = new sms_fees_actual()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                discount = Convert.ToInt32(reader["discount"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"])
                            };
                            feesList.Add(actual_fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return feesList;
        }


        public int insertActualFees(List<sms_fees_actual> feesList)
        {
            int i = 0;

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {

                            foreach (sms_fees_actual fee in feesList)
                            {

                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Delete from sms_fees_actual where std_id=@std_id && fees_category_id=@fees_category_id";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                    cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = fee.fees_category_id;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                }

                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_fees_actual(std_id,fees_category_id, fees_category, fees_sub_category_id, fees_sub_category, amount, discount, actual_amount, created_by, emp_id, date_time) Values(@std_id,@fees_category_id, @fees_category, @fees_sub_category_id, @fees_sub_category, @amount, @discount, @actual_amount, @created_by, @emp_id, @date_time)";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                    cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = fee.fees_category_id;
                                    cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = fee.fees_category;
                                    cmd.Parameters.Add("@fees_sub_category_id", MySqlDbType.Int32).Value = 0;
                                    cmd.Parameters.Add("@fees_sub_category", MySqlDbType.VarChar).Value = "";
                                    cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = fee.amount;
                                    cmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = fee.discount;
                                    cmd.Parameters.Add("@actual_amount", MySqlDbType.Int32).Value = fee.actual_amount;

                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = fee.created_by;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = fee.date_time;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }
                            }

                            trans.Commit();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;

        }

        public List<sms_fees> get_all_fees_generated()
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT * FROM sms_fees_generated AS fee INNER JOIN sms_admission AS adm ON fee.std_id = adm.id where adm.session_id= @session_id ORDER BY fee.date_time DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                rem_amount = Convert.ToInt32(reader["rem_amount"]),
                                discount = Convert.ToInt32(reader["discount"]),
                                actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                wave_off = Convert.ToInt32(reader["wave_off"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                year = Convert.ToInt32(reader["year"]),
                                date = Convert.ToDateTime(reader["date"]),
                                due_date = Convert.ToDateTime(reader["due_date"]),
                                session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),

                                std_name = Convert.ToString(reader["std_name"]),
                                father_name = Convert.ToString(reader["father_name"]),
                                adm_no = Convert.ToString(reader["adm_no"]),
                                roll_no = Convert.ToString(reader["roll_no"]),
                                cell_no = Convert.ToString(reader["cell_no"]),
                            };
                            fees_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fees_list;
        }
        public List<sms_fees> get_all_fees_generated_by_stdID(int std_id)
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT * FROM sms_fees_generated AS fee INNER JOIN sms_admission AS adm ON fee.std_id = adm.id where adm.session_id= @session_id && fee.std_id= @std_id ORDER BY fee.date_time DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                rem_amount = Convert.ToInt32(reader["rem_amount"]),
                                discount = Convert.ToInt32(reader["discount"]),
                                actual_amount = Convert.ToInt32(reader["actual_amount"]),
                                wave_off = Convert.ToInt32(reader["wave_off"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                year = Convert.ToInt32(reader["year"]),
                                date = Convert.ToDateTime(reader["date"]),
                                due_date = Convert.ToDateTime(reader["due_date"]),
                                session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),

                                std_name = Convert.ToString(reader["std_name"]),
                                father_name = Convert.ToString(reader["father_name"]),
                                adm_no = Convert.ToString(reader["adm_no"]),
                                roll_no = Convert.ToString(reader["roll_no"]),
                                cell_no = Convert.ToString(reader["cell_no"]),
                            };
                            fees_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fees_list;
        }

        public List<sms_fees> get_all_fees_generated_by_date(DateTime sDate, DateTime eDate)
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT fee.id,fee.std_id,fee.fees_category_id,fee.fees_category,fee.amount,fee.rem_amount,fee.discount, fee.actual_amount, fee.wave_off, fee.month, fee.month_name, fee.year, fee.date, fee.due_date, fee.date_time, fee.created_by, fee.emp_id, "+
                            "adm.std_name, adm.father_name, adm.adm_no, adm.class_id, adm.class_name, adm.section_id, adm.section_name, adm.roll_no, adm.cell_no, adm.is_active, adm.session_id FROM sms_fees_generated AS fee INNER JOIN sms_admission AS adm ON fee.std_id = adm.id where DATE(fee.Date) >= @sDate && DATE(fee.Date) <= @eDate ORDER BY fee.date_time DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader[0]),
                                std_id = Convert.ToInt32(reader[1]),
                                fees_category_id = Convert.ToInt32(reader[2]),
                                fees_category = Convert.ToString(reader[3].ToString()),                                
                                amount = Convert.ToInt32(reader[4]),
                                rem_amount = Convert.ToInt32(reader[5]),
                                discount = Convert.ToInt32(reader[6]),
                                actual_amount = Convert.ToInt32(reader[7]),
                                wave_off = Convert.ToInt32(reader[8]),
                                month = Convert.ToInt32(reader[9]),
                                month_name = Convert.ToString(reader[10].ToString()),
                                year = Convert.ToInt32(reader[11]),
                                date = Convert.ToDateTime(reader[12]),
                                due_date = Convert.ToDateTime(reader[13]),                                
                                date_time = Convert.ToDateTime(reader[14].ToString()),
                                created_by = Convert.ToString(reader[15].ToString()),
                                emp_id = Convert.ToInt32(reader[16].ToString()),                                                              
                                
                                std_name = Convert.ToString(reader[17]),
                                father_name = Convert.ToString(reader[18]),
                                adm_no = Convert.ToString(reader[19]),
                                class_id = Convert.ToInt32(reader[20]),
                                class_name = Convert.ToString(reader[21]),
                                section_id = Convert.ToInt32(reader[22]),
                                section_name = Convert.ToString(reader[23]),
                                roll_no = Convert.ToString(reader[24]),
                                cell_no = Convert.ToString(reader[25]),
                                adm_is_active = Convert.ToString(reader[26]),
                                session_id = Convert.ToInt32(reader[27].ToString()),                                
                            };
                            fees_list.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return fees_list;
        }

        public bool isFeesGenerated(int std_id, int fees_category_id, int fees_sub_category_id, int month, int year)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select 1 from sms_fees_generated where std_id=@std_id && fees_category_id=@fees_category_id && fees_sub_category_id=@fees_sub_category_id && month=@month && year=@year";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = fees_category_id;
                        cmd.Parameters.Add("@fees_sub_category_id", MySqlDbType.Int32).Value = fees_sub_category_id;
                        cmd.Parameters.Add("@month", MySqlDbType.Int32).Value = month;
                        cmd.Parameters.Add("@year", MySqlDbType.Int32).Value = year;

                        con.Open();
                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        if (result == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int insertFeesGenerated(List<sms_fees> feesGeneratedList)
        {
            int i = 0;
            int count = 0;
            
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        foreach (sms_fees fee in feesGeneratedList)
                        {
                            i = 0;
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_fees_generated(std_id, fees_category_id, fees_category, fees_sub_category_id, fees_sub_category, amount,actual_amount, rem_amount, discount, month, month_name, class_id, class_name, section_id, section_name, year, date, session_id, created_by, emp_id, date_time, due_date) Values(@std_id,@fees_category_id,@fees_category,@fees_sub_category_id,@fees_sub_category,@amount,@actual_amount,@rem_amount,@discount,@month,@month_name,@class_id,@class_name,@section_id,@section_name,@year,@date,@session_id,@created_by,@emp_id,@date_time, @due_date)";
                                cmd.Connection = con;

                                cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                cmd.Parameters.Add("@fees_category_id", MySqlDbType.Int32).Value = fee.fees_category_id;
                                cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = fee.fees_category;
                                cmd.Parameters.Add("@fees_sub_category_id", MySqlDbType.Int32).Value = fee.fees_sub_category_id;
                                cmd.Parameters.Add("@fees_sub_category", MySqlDbType.VarChar).Value = fee.fees_sub_category;
                                cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = fee.amount;
                                cmd.Parameters.Add("@actual_amount", MySqlDbType.Int32).Value = fee.actual_amount;
                                cmd.Parameters.Add("@rem_amount", MySqlDbType.Int32).Value = fee.rem_amount;
                                cmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = fee.discount;
                                cmd.Parameters.Add("@month", MySqlDbType.Int32).Value = fee.month;
                                cmd.Parameters.Add("@month_name", MySqlDbType.VarChar).Value = fee.month_name;
                                cmd.Parameters.Add("@class_id", MySqlDbType.Int32).Value = fee.class_id;
                                cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = fee.class_name;
                                cmd.Parameters.Add("@section_id", MySqlDbType.Int32).Value = fee.section_id;
                                cmd.Parameters.Add("@section_name", MySqlDbType.VarChar).Value = fee.section_name;
                                cmd.Parameters.Add("@year", MySqlDbType.Int32).Value = fee.year;
                                cmd.Parameters.Add("@date", MySqlDbType.Date).Value = fee.date;
                                cmd.Parameters.Add("@due_date", MySqlDbType.Date).Value = fee.due_date;
                                cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = fee.session_id;
                                cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = fee.created_by;
                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;
                                cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = fee.date_time;

                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteScalar());
                                con.Close();
                                if (i == 0)
                                {
                                    count++;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }           

            return count;
        }

        public int updateFeesGenerated(sms_fees obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_fees_generated SET wave_off=@wave_off, amount=@amount, rem_amount=@rem_amount, due_date=@due_date, created_by=@created_by, date_time=@date_time, emp_id=@emp_id where id = @id";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.id;
                        cmd.Parameters.Add("@wave_off", MySqlDbType.Int32).Value = obj.wave_off;
                        cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = obj.amount;
                        cmd.Parameters.Add("@rem_amount", MySqlDbType.Int32).Value = obj.rem_amount;
                        cmd.Parameters.Add("@due_date", MySqlDbType.Date).Value = obj.due_date;


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

        public int deleteGeneratedFees(List<sms_fees> list)
        {
            int i = 0;
            int count = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    foreach (var item in list)
                    {
                        i = 0;
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Delete from sms_fees_generated where id = @id";
                            cmd.Connection = con;
                            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = item.id;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                            try
                            {                                
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());                                
                                if(i > 0)
                                {
                                    count++;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count;
        }

        public sms_fees getLastFeeReceived(int std_id)
        {
            sms_fees fees = new sms_fees();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT * FROM sms_fees_paid WHERE  std_id = @std_id ORDER  BY date DESC LIMIT 1";
                    cmd.Connection = con;
                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;

                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {

                            fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                fees_category_id = Convert.ToInt32(reader["fees_category_id"]),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                fees_sub_category = Convert.ToString(reader["fees_sub_category"].ToString()),
                                fees_sub_category_id = Convert.ToInt32(reader["fees_sub_category_id"]),
                                amount = Convert.ToInt32(reader["amount"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                year = Convert.ToInt32(reader["year"]),
                                date = Convert.ToDateTime(reader["date"]),
                                session_id = Convert.ToInt32(reader["session_id"].ToString()),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                emp_id = Convert.ToInt32(reader["emp_id"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),

                                fees_collection_place_id = Convert.ToInt32(reader["fees_collection_place_id"]),
                                fees_collection_place = Convert.ToString(reader["fees_collection_place"]),

                                receipt_no_full = Convert.ToString(reader["receipt_no_full"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                                total_paid = Convert.ToInt32(reader["total_paid"]),
                                total_remaining = Convert.ToInt32(reader["total_remaining"]),
                            };
                            break;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            } return fees;
        }

        public string getFeesNote()
        {
            string note = "";

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT * FROM sms_fees_note";
                    cmd.Connection = con;
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            note = Convert.ToString(reader["fees_note"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return note;
        }

        public string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "And ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        #region fees vouchers

        public List<sms_fees> getAllFeesVoucherByStdID(int std_id)
        {
            List<sms_fees> list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT * FROM sms_fees_voucher where std_id = @std_id && is_active= 'Y' Group By receipt_no";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees voucher = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                receipt_no_full = Convert.ToString(reader["receipt_no_full"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                            };
                            list.Add(voucher);
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

        public List<sms_fees> getAllFeesVoucherByReceiptNo(int receipt_no)
        {
            List<sms_fees> list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT * FROM sms_fees_voucher AS fv INNER JOIN sms_admission AS adm ON adm.id = fv.std_id where fv.receipt_no = @receipt_no && fv.is_active= 'Y' && adm.session_id=@session_id ";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = receipt_no;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees voucher = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                father_name = Convert.ToString(reader["father_name"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                adm_no = Convert.ToString(reader["adm_no"].ToString()),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                receipt_no_full = Convert.ToString(reader["receipt_no_full"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                            };
                            list.Add(voucher);
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
        public List<sms_fees> getAllFeesVoucherByDate(DateTime dt)
        {
            List<sms_fees> list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT * FROM sms_fees_voucher AS fv INNER JOIN sms_admission AS adm ON adm.id = fv.std_id where DATE(fv.date_time)= @date && fv.is_active= 'N' && adm.session_id=@session_id GROUP BY fv.receipt_no ORDER BY fv.date_time DESC";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = MainWindow.session.id;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees voucher = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                father_name = Convert.ToString(reader["father_name"].ToString()),
                                adm_no = Convert.ToString(reader["adm_no"].ToString()),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                receipt_no_full = Convert.ToString(reader["receipt_no_full"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                                total_paid = Convert.ToInt32(reader["total_paid"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),
                            };
                            list.Add(voucher);
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
        public List<sms_fees> getAllFeesVoucherByReceiptNoFull(string receipt_no_full)
        {
            List<sms_fees> list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT * FROM sms_fees_voucher where receipt_no_full = @receipt_no_full && is_active= 'Y' ";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@receipt_no_full", MySqlDbType.VarChar).Value = receipt_no_full;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees voucher = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                receipt_no_full = Convert.ToString(reader["receipt_no_full"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                            };
                            list.Add(voucher);
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
        public List<sms_fees> getAllUnPaidFeesVoucher()
        {
            List<sms_fees> list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT * FROM sms_fees_voucher AS fv INNER JOIN sms_admission AS adm ON adm.id = fv.std_id && adm.session_id = @session_id where fv.is_active= 'Y'  ORDER BY fv.date_time DESC";
                        cmd.Parameters.Add("@session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd.Connection = con;                                               

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees voucher = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                father_name = Convert.ToString(reader["father_name"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                roll_no = Convert.ToString(reader["roll_no"].ToString()),
                                class_id = Convert.ToInt32(reader["class_id"]),
                                section_id = Convert.ToInt32(reader["section_id"]),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                cell_no = Convert.ToString(reader["cell_no"].ToString()),
                                adm_no = Convert.ToString(reader["adm_no"].ToString()),
                                fees_generated_id = Convert.ToInt32(reader["fees_generated_id"]),
                                receipt_no_full = Convert.ToString(reader["receipt_no_full"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                                total_paid = Convert.ToInt32(reader["total_paid"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"].ToString()),
                            };
                            list.Add(voucher);
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

        public List<sms_fees> getAllUnPaidFeesVoucherWithAmount()
        {
            List<sms_fees> list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT adm.std_name,adm.father_name,adm.class_id,adm.class_name, adm.section_id, adm.section_name, adm.cell_no, adm.adm_no, adm.roll_no," +
                                            "fee.rem_amount, fee.fees_category,fee.month_name,fee.due_date," +
                                            "fv.fees_generated_id, fv.receipt_no_full, fv.receipt_no, fv.total_amount, fv.total_paid, fv.date_time, fv.std_id FROM sms_fees_voucher AS fv " +
                                            "INNER JOIN sms_admission AS adm ON adm.id = fv.std_id && adm.session_id = (select adm_inner.session_id from sms_admission as adm_inner where adm_inner.id=fv.std_id order by adm_inner.session_id DESC Limit 1) " +
                                            "Inner join sms_fees_generated as fee on fee.id = fv.fees_generated_id "+
                                            "where fv.is_active= 'Y' "+
                                            "ORDER BY fv.date_time DESC";
                        cmd.Parameters.Add("@session_id", MySqlDbType.VarChar).Value = MainWindow.session.id;
                        cmd.Connection = con;

                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees voucher = new sms_fees()
                            {
                                std_name = Convert.ToString(reader[0].ToString()),
                                father_name = Convert.ToString(reader[1].ToString()),
                                class_id = Convert.ToInt32(reader[2]),
                                class_name = Convert.ToString(reader[3].ToString()),
                                section_id = Convert.ToInt32(reader[4]),
                                section_name = Convert.ToString(reader[5].ToString()),
                                cell_no = Convert.ToString(reader[6].ToString()),
                                adm_no = Convert.ToString(reader[7].ToString()),
                                roll_no = Convert.ToString(reader[8].ToString()),
                                amount = Convert.ToInt32(reader[9]),
                                rem_amount = Convert.ToInt32(reader[9]),  
                                fees_category = Convert.ToString(reader[10]),
                                month_name = Convert.ToString(reader[11]),
                                due_date = Convert.ToDateTime(reader[12]),
                                fees_generated_id = Convert.ToInt32(reader[13]),
                                receipt_no_full = Convert.ToString(reader[14].ToString()),
                                receipt_no = Convert.ToInt32(reader[15]),
                                total_amount = Convert.ToInt32(reader[16]),
                                total_paid = Convert.ToInt32(reader[17]),    
                                date = Convert.ToDateTime(reader[18]),
                                std_id = Convert.ToInt32(reader[19]),                                
                            };
                            list.Add(voucher);
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

        public int insertFeesVoucher(List<sms_fees> voucherList, int receipt_no)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            foreach (sms_fees fee in voucherList)
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_fees_voucher(std_id,fees_generated_id,receipt_no, receipt_no_full, total_amount, created_by, emp_id, date_time, total_paid) Values(@std_id, @fees_generated_id, @receipt_no, @receipt_no_full, @total_amount, @created_by, @emp_id, @date_time, @total_paid)";
                                    cmd.Connection = con;
                                    cmd.Transaction = trans;

                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                                    cmd.Parameters.Add("@fees_generated_id", MySqlDbType.Int32).Value = fee.fees_generated_id;
                                    cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = fee.receipt_no;
                                    cmd.Parameters.Add("@receipt_no_full", MySqlDbType.VarChar).Value = fee.receipt_no_full;
                                    cmd.Parameters.Add("@total_amount", MySqlDbType.Int32).Value = fee.total_amount;
                                    cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = 0;

                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = fee.created_by;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = fee.date_time;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }
                            }

                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "update sms_voucher_types SET last_voucher_no=@voucher_no_int, last_voucher=@voucher_no where id = @voucher_id";
                                cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = 4;
                                cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = receipt_no;
                                cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = "CRV-" + DateTime.Now.ToString("yy") + "-" + receipt_no.ToString("D6");

                                cmd.Connection = con;
                                cmd.Transaction = trans;
                                cmd.ExecuteScalar();
                            }
                            trans.Commit();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;

        }

        public int updateFeesVoucher(sms_fees obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_fees_voucher SET is_active='N', created_by=@created_by, date_time=@date_time, emp_id=@emp_id, total_paid=@total_paid where receipt_no = @receipt_no";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.id;
                        cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = obj.receipt_no;
                        cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = obj.total_paid;

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

        public int deleteFeesVoucher(int receipt_no)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Delete from sms_fees_voucher where receipt_no=@receipt_no";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = receipt_no;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        try
                        {
                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return i;
        }

        #endregion

        #region fees Voucher Histroy

        public int insertFeesVoucherHistory(List<sms_fees> feesList)
        {
            int i = 0;

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    foreach (sms_fees fee in feesList)
                    {
                        i = 0;
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "INSERT INTO sms_fees_voucher_history(std_id, std_name, father_name, adm_no, fees_category, amount, month_name, class_name, section_name, date, due_date, receipt_no, receipt_no_full, fees_collection_place, total_amount,total_paid, total_remaining, amount_in_words, discount, wave_off, emp_id) Values(std_id, std_name, father_name, adm_no, fees_category, amount, month_name, class_name, section_name, date, due_date, receipt_no, receipt_no_full, fees_collection_place, total_amount, @total_paid, @total_remaining, @amount_in_words, @discount, @wave_off, @emp_id)";
                            cmd.Connection = con;

                            cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = fee.std_id;
                            cmd.Parameters.Add("@std_name", MySqlDbType.VarChar).Value = fee.std_name;
                            cmd.Parameters.Add("@father_name", MySqlDbType.VarChar).Value = fee.father_name;
                            cmd.Parameters.Add("@adm_no", MySqlDbType.VarChar).Value = fee.adm_no;
                            cmd.Parameters.Add("@fees_category", MySqlDbType.VarChar).Value = fee.fees_category;
                            cmd.Parameters.Add("@amount", MySqlDbType.Int32).Value = fee.rem_amount;
                            cmd.Parameters.Add("@month_name", MySqlDbType.VarChar).Value = fee.month_name;
                            cmd.Parameters.Add("@class_name", MySqlDbType.VarChar).Value = fee.class_name;
                            cmd.Parameters.Add("@section_name", MySqlDbType.VarChar).Value = fee.section_name;
                            cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = fee.date;
                            cmd.Parameters.Add("@due_date", MySqlDbType.DateTime).Value = fee.due_date;
                            cmd.Parameters.Add("@receipt_no", MySqlDbType.Int32).Value = fee.receipt_no;
                            cmd.Parameters.Add("@receipt_no_full", MySqlDbType.VarChar).Value = fee.receipt_no_full;
                            cmd.Parameters.Add("@fees_collection_place", MySqlDbType.VarChar).Value = fee.fees_collection_place;
                            cmd.Parameters.Add("@total_amount", MySqlDbType.Int32).Value = fee.total_amount;
                            cmd.Parameters.Add("@total_paid", MySqlDbType.Int32).Value = fee.total_paid;
                            cmd.Parameters.Add("@total_remaining", MySqlDbType.Int32).Value = fee.total_remaining;
                            cmd.Parameters.Add("@amount_in_words", MySqlDbType.VarChar).Value = fee.amount_in_words;
                            cmd.Parameters.Add("@discount", MySqlDbType.Int32).Value = fee.discount;
                            cmd.Parameters.Add("@wave_off", MySqlDbType.Int32).Value = fee.wave_off;
                            cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = fee.emp_id;

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

        public List<sms_fees> getFeesVoucherHistoryByStdId(int std_id)
        {
            List<sms_fees> feesHistoryList = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_fees_voucher_history where std_id= @std_id ORDER BY date DESC";
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_fees fees = new sms_fees()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                std_id = Convert.ToInt32(reader["std_id"]),
                                std_name = reader["std_name"].ToString(),
                                father_name = reader["father_name"].ToString(),
                                adm_no = reader["adm_no"].ToString(),
                                fees_category = Convert.ToString(reader["fees_category"].ToString()),
                                amount = Convert.ToInt32(reader["amount"]),
                                month_name = Convert.ToString(reader["month_name"].ToString()),
                                receipt_no = Convert.ToInt32(reader["receipt_no"]),
                                receipt_no_full = Convert.ToString(reader["receipt_no_full"]),
                                class_name = Convert.ToString(reader["class_name"]),
                                section_name = Convert.ToString(reader["section_name"]),
                                date = Convert.ToDateTime(reader["date"]),
                                due_date = Convert.ToDateTime(reader["due_date"]),
                                fees_collection_place = Convert.ToString(reader["fees_collection_place"]),
                                total_amount = Convert.ToInt32(reader["total_amount"]),
                                total_paid = Convert.ToInt32(reader["total_paid"]),
                                total_remaining = Convert.ToInt32(reader["total_remaining"]),
                                amount_in_words = Convert.ToString(reader["amount_in_words"].ToString()),
                                discount = Convert.ToInt32(reader["discount"]),
                                wave_off = Convert.ToInt32(reader["wave_off"]),
                            };
                            feesHistoryList.Add(fees);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return feesHistoryList;
        }

        #endregion

        public sms_fees get_bank_details()
        {
            sms_fees fees = new sms_fees();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT* FROM sms_bank_details ";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fees = new sms_fees()
                            {
                                bank_name = Convert.ToString(reader["bank_name"].ToString()),
                                branch_address = Convert.ToString(reader["branch_name"].ToString()),
                                account_no = Convert.ToString(reader["account_no"].ToString()),
                                account_title = Convert.ToString(reader["account_title"].ToString()),
                                bank_logo = (byte[])reader["bank_logo"],
                            };

                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return fees;
        }

        #region cancel_challan

        public int cancelFees(List<sms_fees> fees_list)
        {
            string receipt_no_full = fees_list.First().receipt_no_full;
            List<sms_fees> generated_fees_list = get_all_fees_generated_by_stdID(fees_list.First().std_id);            
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            foreach (var obj in fees_list)
                            {
                                sms_fees generated_fees = generated_fees_list.Where(x => x.id == obj.fees_generated_id).First();

                                // update sms_fees_generated
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "Update sms_fees_generated SET  rem_amount=@rem_amount, created_by=@created_by, date_time=@date_time, emp_id=@emp_id where id = @id";
                                    cmd.Connection = con;

                                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.fees_generated_id;
                                    cmd.Parameters.Add("@rem_amount", MySqlDbType.Int32).Value = generated_fees.rem_amount + obj.amount_paid;

                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = DateTime.Now;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = MainWindow.emp_login_obj.id;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }                                

                                //insert into sms_fees_cancel
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.CommandText = "INSERT INTO sms_voucher_cancel(voucher_no,voucher_no_int,voucher_type_id, account_head_id, account_detail_id, description, amount, std_id, created_by, date_time, emp_id) Values(@voucher_no,@voucher_no_int,@voucher_type_id, @account_head_id, @account_detail_id, @description, @amount, @std_id, @created_by, @date_time, @emp_id)";
                                    cmd.Connection = con;
                                    
                                    cmd.Parameters.Add("@account_head_id", MySqlDbType.Int32).Value = 49;                                    
                                    cmd.Parameters.Add("@account_detail_id", MySqlDbType.Int32).Value = obj.fees_category_id;
                                    cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = obj.receipt_no_full;
                                    cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = obj.receipt_no;
                                    cmd.Parameters.Add("@voucher_type_id", MySqlDbType.Int32).Value = 4;
                                    cmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = generated_fees.std_name + "[" + generated_fees.adm_no + "]- " + obj.month_name + "-" + obj.fees_category;
                                    cmd.Parameters.Add("@amount", MySqlDbType.Double).Value = obj.amount_paid;
                                    cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = obj.std_id;                                    

                                    cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = MainWindow.emp_login_obj.emp_user_name;
                                    cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = DateTime.Now;
                                    cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = MainWindow.emp_login_obj.id;

                                    i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                    cmd.Parameters.Clear();
                                }                               
                            }

                            // delete from sms_fees_paid
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Delete from sms_fees_paid where receipt_no_full = @id";
                                cmd.Connection = con;
                                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = receipt_no_full;
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                cmd.Parameters.Clear();
                            }

                            // delete from sms_voucher_entries
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Delete from sms_voucher_entries where voucher_no = @id";
                                cmd.Connection = con;
                                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = receipt_no_full;
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                cmd.Parameters.Clear();
                            }

                            // delete from sms_vouchers
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Delete from sms_voucher where voucher_no = @id";
                                cmd.Connection = con;
                                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = receipt_no_full;
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                cmd.Parameters.Clear();
                            }                            

                            // delete from sms_fees_history
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "Delete from sms_fees_voucher_history where receipt_no_full = @id";
                                cmd.Connection = con;
                                cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = receipt_no_full;
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                cmd.Parameters.Clear();
                            }   
                 
                            
                            
                            trans.Commit();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }

        #endregion

        public List<sms_fees> get_all_fees_by_StdID(int std_id, string session_id)
        {
            List<sms_fees> fees_list = new List<sms_fees>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT STR_TO_DATE(CONCAT(year.id,'-',gen.month,'-',01), '%Y-%m-%d') as MonthYear, gen.id,gen.year, gen.month_name,gen.month, gen.fees_category_id,gen.fees_category, gen.amount as AAmount, IFNull(paid.amount,gen.amount) as Amount, IFNull(paid.amount_paid,0) as Paid  ,IFNull(IFNull(paid.amount,gen.amount)- IFNull(paid.amount_paid,0),0) as Rem, paid.date, paid.receipt_no_full " +
                                            "FROM sms_fees_generated as gen Left Outer Join sms_fees_paid as paid on gen.id=paid.fees_generated_id " +
                                            "Inner Join sms_admission as adm on adm.id=gen.std_id && adm.session_id=@session_id " +
                                            "Inner join sms_months as month on month.month=gen.month " +
                                            "Inner join sms_years as year on year.year = gen.year " +
                                            "where gen.std_id=@std_id Order By MonthYear ASC , paid.receipt_no ASC ";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@std_id", MySqlDbType.Int32).Value = std_id;
                        cmd.Parameters.Add("@session_id", MySqlDbType.Int32).Value = session_id;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    

                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sms_fees fee = new sms_fees()
                            {
                                year = Convert.ToInt32(reader["year"]),
                                month = Convert.ToInt32(reader["month"]),
                                month_name = Convert.ToString(reader["month_name"]),
                                fees_category = Convert.ToString(reader["fees_category"]),
                                actual_amount = Convert.ToInt32(reader["AAmount"]),
                                amount = Convert.ToInt32(reader["Amount"]),
                                amount_paid = Convert.ToInt32(reader["Paid"]),
                                rem_amount = Convert.ToInt32(reader["Rem"]),
                               // date_received = Convert.ToDateTime(reader["date"]),
                                receipt_no_full = Convert.ToString(reader["receipt_no_full"]),                            
                            };
                            if (Convert.IsDBNull(reader["date"]))
                            {
                                //fee.date_received = new DateTime(2001, 01, 01);
                                //fee.date_received = rea;
                            }
                            else
                            {
                                fee.date_received = Convert.ToDateTime(reader["date"]);
                            }
                            fees_list.Add(fee);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            return fees_list;
        }
    }
}

