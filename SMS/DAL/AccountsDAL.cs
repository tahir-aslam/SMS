using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;
using MySql.Data.MySqlClient;

namespace SMS.DAL
{
    public class AccountsDAL
    {
        public AccountsDAL()
        {
        }

        #region Chart Of Accouts

        public List<chart_of_accounts> getAllChartOfAccounts()
        {
            List<chart_of_accounts> accList = new List<chart_of_accounts>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select acc.isCashAccount, acc.account_name, acc.id, acc.p_id, acc.account_code, acc.account_full_code, acc.account_type_id, acc.account_category_id, acc.date_time,acc.created_by,acc.emp_id,types.account_type from sms_chart_of_accounts AS acc INNER JOIN sms_account_types AS types ON acc.account_type_id = types.id";
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            chart_of_accounts acc = new chart_of_accounts()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                account_category_id = Convert.ToInt32(reader["account_category_id"]),
                                account_full_code = reader["account_full_code"].ToString(),
                                account_code = Convert.ToInt32(reader["account_code"]),
                                account_name = Convert.ToString(reader["account_name"]),
                                account_type = Convert.ToString(reader["account_type"]),
                                account_type_id = Convert.ToInt32(reader["account_type_id"]),
                                p_id = Convert.ToInt32(reader["p_id"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                isCashAccount = Convert.ToString(reader["isCashAccount"]),

                            };
                            accList.Add(acc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return accList;
        }

        public int insertAccount(chart_of_accounts obj)
        {
            int i = 0;

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_chart_of_accounts(account_name,p_id, account_code, account_type_id, account_category_id,created_by, date_time, emp_id, account_full_code) Values(@account_name, @p_id, @account_code, @account_type_id, @account_category_id, @created_by, @date_time, @emp_id, @account_full_code);";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@account_name", MySqlDbType.VarChar).Value = obj.account_name;
                        cmd.Parameters.Add("@p_id", MySqlDbType.Int32).Value = obj.p_id;
                        cmd.Parameters.Add("@account_type_id", MySqlDbType.Int32).Value = obj.account_type_id;
                        cmd.Parameters.Add("@account_code", MySqlDbType.Int32).Value = obj.account_code;
                        cmd.Parameters.Add("@account_full_code", MySqlDbType.VarChar).Value = obj.account_full_code;
                        cmd.Parameters.Add("@account_category_id", MySqlDbType.Int32).Value = obj.account_category_id;
                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = obj.created_by;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = obj.date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cmd.Parameters.Clear();
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

        public int updateAccount(chart_of_accounts obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_chart_of_accounts SET account_name=@account_name, p_id=@p_id, account_code=@account_code, account_type_id=@account_type_id, account_category_id=@account_category_id,created_by=@created_by, date_time=@date_time, emp_id=@emp_id, account_full_code=@account_full_code where id = @id";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = obj.id;
                        cmd.Parameters.Add("@account_name", MySqlDbType.VarChar).Value = obj.account_name;
                        cmd.Parameters.Add("@p_id", MySqlDbType.Int32).Value = obj.p_id;
                        cmd.Parameters.Add("@account_type_id", MySqlDbType.Int32).Value = obj.account_type_id;
                        cmd.Parameters.Add("@account_code", MySqlDbType.Int32).Value = obj.account_code;
                        cmd.Parameters.Add("@account_full_code", MySqlDbType.VarChar).Value = obj.account_full_code;
                        cmd.Parameters.Add("@account_category_id", MySqlDbType.Int32).Value = obj.account_category_id;
                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = obj.created_by;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = obj.date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = obj.emp_id;

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }

        public int deleteAccount(int id)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Delete from sms_chart_of_accounts where id=@id";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
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

        public int getLastAccountNo(int p_id)
        {
            int code = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select MAX(account_code) as code from sms_chart_of_accounts where p_id = @p_id";
                        cmd.Parameters.Add("@p_id", MySqlDbType.Int32).Value = p_id;
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            code = Convert.ToInt32(reader[0]);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return code;
        }

        public bool checkAccountName(string name, int p_id, int id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select id from sms_chart_of_accounts where p_id = @p_id && account_name=@name && id != @id";
                        cmd.Parameters.Add("@p_id", MySqlDbType.Int32).Value = p_id;
                        cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        #endregion

        public double getOpeningBalance(int account_detail_id, DateTime date)
        {
            double balance = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher_entries AS ve Inner Join sms_voucher AS v on ve.voucher_id = v.id where ve.account_detail_id=@account_detail_id && v.voucher_date < @date order by v.voucher_date ASC, ve.id ASC";
                        cmd.Parameters.Add("@account_detail_id", MySqlDbType.Int32).Value = account_detail_id;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = date;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_no = Convert.ToString(reader["voucher_no"]),
                                account_detail_id = Convert.ToInt32(reader["account_detail_id"]),
                                account_detail = Convert.ToString(reader["account_detail"]),
                                account_head_id = Convert.ToInt32(reader["account_head_id"]),
                                account_head = Convert.ToString(reader["account_head"]),
                                description = Convert.ToString(reader["description"]),
                                debit = Convert.ToDouble(reader["debit"]),
                                credit = Convert.ToDouble(reader["credit"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                voucher_date = Convert.ToDateTime(reader["voucher_date"]),
                            };

                            balance = balance + voucher.debit;
                            balance = balance - voucher.credit;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (balance < 0)
            {
                balance = balance * -1;
            }
            return balance;
        }

        //opening balance list
        public List<sms_voucher_entries> getAllVoucherEntriesByAccountDetailID(int account_detail_id)
        {
            List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();
            double balance = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher_entries AS ve Inner Join sms_voucher AS v on ve.voucher_id = v.id where account_detail_id=@account_detail_id order by v.voucher_date ASC, ve.id ASC";
                        cmd.Parameters.Add("@account_detail_id", MySqlDbType.Int32).Value = account_detail_id;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_no = Convert.ToString(reader["voucher_no"]),
                                account_detail_id = Convert.ToInt32(reader["account_detail_id"]),
                                account_detail = Convert.ToString(reader["account_detail"]),
                                account_head_id = Convert.ToInt32(reader["account_head_id"]),
                                account_head = Convert.ToString(reader["account_head"]),
                                description = Convert.ToString(reader["description"]),
                                debit = Convert.ToDouble(reader["debit"]),
                                credit = Convert.ToDouble(reader["credit"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                voucher_date = Convert.ToDateTime(reader["voucher_date"]),
                            };

                            balance = balance + voucher.debit;
                            balance = balance - voucher.credit;
                            voucher.balance = balance;
                            voucherEntriesList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherEntriesList;
        }

        public List<sms_voucher_entries> getAllIncomeStatement(DateTime sDate, DateTime eDate)
        {
            List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();
            double balance = 0;
            double e_debit = 0;
            double e_credit = 0;

            double r_debit = 0;
            double r_credit = 0;

            double t_expense = 0;
            double t_revenue = 0;
            double income = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select coa_type.id, coa_type.account_type, ve.account_head_id, ve.account_head, ve.account_detail_id, ve.account_detail,"
                                           + "ve.debit, ve.credit, v.voucher_date from sms_voucher_entries AS ve Inner Join sms_voucher AS v on ve.voucher_id = v.id "
                                           + "Inner join sms_chart_of_accounts AS coa on ve.account_detail_id= coa.id "
                                           + "Inner join sms_account_types as coa_type on coa.account_type_id = coa_type.id "
                                           + "where (coa_type.id=2 OR coa_type.id=3) && (DATE(v.voucher_date) >= @sDate && DATE(v.voucher_date) <= @eDate)";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                account_type_id = Convert.ToInt32(reader[0]),
                                account_type = Convert.ToString(reader[1]),
                                account_head_id = Convert.ToInt32(reader[2]),
                                account_head = Convert.ToString(reader[3]),
                                account_detail_id = Convert.ToInt32(reader[4]),
                                account_detail = Convert.ToString(reader[5]),
                                debit = Convert.ToDouble(reader[6]),
                                credit = Convert.ToDouble(reader[7]),
                                voucher_date = Convert.ToDateTime(reader[8]),
                            };
                            //balance = balance + voucher.debit;
                            //balance = balance - voucher.credit;
                            //voucher.balance = balance;
                            if (voucher.account_type_id == 2)
                            {
                                e_debit = e_debit + voucher.debit;
                                e_credit = e_credit + voucher.credit;
                            }
                            if (voucher.account_type_id == 3)
                            {
                                r_debit = r_debit + voucher.debit;
                                r_credit = r_credit + voucher.credit;
                            }
                            voucherEntriesList.Add(voucher);
                        }
                    }
                }
                t_expense = e_debit - e_credit;
                t_revenue = r_credit - r_debit;
                income = t_revenue - t_expense;

                foreach (var item in voucherEntriesList)
                {
                    item.total_expense = t_expense;
                    item.total_revenue = t_revenue;
                    item.income = income;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return voucherEntriesList;
        }

        public List<sms_voucher_entries> getTrialBalance(DateTime eDate)
        {
            List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select coa_type.id, coa_type.account_type, ve.account_head_id, ve.account_head, ve.account_detail_id, ve.account_detail,"
                                           + "ve.debit, ve.credit, v.voucher_date from sms_voucher_entries AS ve Inner Join sms_voucher AS v on ve.voucher_id = v.id "
                                           + "Inner join sms_chart_of_accounts AS coa on ve.account_detail_id= coa.id "
                                           + "Inner join sms_account_types as coa_type on coa.account_type_id = coa_type.id "
                                           + "where DATE(v.voucher_date) <= @eDate";

                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                account_type_id = Convert.ToInt32(reader[0]),
                                account_type = Convert.ToString(reader[1]),
                                account_head_id = Convert.ToInt32(reader[2]),
                                account_head = Convert.ToString(reader[3]),
                                account_detail_id = Convert.ToInt32(reader[4]),
                                account_detail = Convert.ToString(reader[5]),
                                debit = Convert.ToDouble(reader[6]),
                                credit = Convert.ToDouble(reader[7]),
                                voucher_date = Convert.ToDateTime(reader[8]),
                            };

                            if (voucher.account_type_id == 1)
                            {
                                voucherEntriesList.Add(voucher);
                            }
                            else if (voucher.account_type_id == 2)
                            {
                                voucherEntriesList.Add(voucher);
                            }
                            else if (voucher.account_type_id == 3)
                            {
                                voucherEntriesList.Add(voucher);
                            }
                            else if (voucher.account_type_id == 4)
                            {
                                voucherEntriesList.Add(voucher);
                            }
                            else if (voucher.account_type_id == 5)
                            {
                                voucherEntriesList.Add(voucher);
                            }
                            else
                            {
                            }

                            //if (voucher.account_type_id == 1 && voucher.debit > 0)
                            //{
                            //    voucherEntriesList.Add(voucher);
                            //}
                            //else if (voucher.account_type_id == 2 && voucher.debit > 0)
                            //{
                            //    voucherEntriesList.Add(voucher);
                            //}
                            //else if (voucher.account_type_id == 3 && voucher.credit > 0)
                            //{
                            //    voucherEntriesList.Add(voucher);
                            //}
                            //else if (voucher.account_type_id == 4 && voucher.credit > 0)
                            //{
                            //    voucherEntriesList.Add(voucher);
                            //}
                            //else if (voucher.account_type_id == 5 && voucher.credit > 0)
                            //{
                            //    voucherEntriesList.Add(voucher);
                            //}
                            //else
                            //{
                            //}

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return voucherEntriesList;
        }

        //public List<sms_voucher_entries> getTrialBalance(DateTime eDate)
        //{
        //    List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();

        //    try
        //    {
        //        using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
        //        {
        //            using (MySqlCommand cmd = new MySqlCommand())
        //            {
        //                cmd.CommandText = "select coa_type.id, coa_type.account_type, ve.account_head_id, ve.account_head, ve.account_detail_id, ve.account_detail,"
        //                                   + "ve.debit, ve.credit, v.voucher_date from sms_voucher_entries AS ve Inner Join sms_voucher AS v on ve.voucher_id = v.id "
        //                                   + "Inner join sms_chart_of_accounts AS coa on ve.account_detail_id= coa.id "
        //                                   + "Inner join sms_account_types as coa_type on coa.account_type_id = coa_type.id "
        //                                   + "where DATE(v.voucher_date) <= @eDate";

        //                cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
        //                cmd.Connection = con;
        //                con.Open();

        //                MySqlDataReader reader = cmd.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    sms_voucher_entries voucher = new sms_voucher_entries()
        //                    {
        //                        account_type_id = Convert.ToInt32(reader[0]),
        //                        account_type = Convert.ToString(reader[1]),
        //                        account_head_id = Convert.ToInt32(reader[2]),
        //                        account_head = Convert.ToString(reader[3]),
        //                        account_detail_id = Convert.ToInt32(reader[4]),
        //                        account_detail = Convert.ToString(reader[5]),
        //                        debit = Convert.ToDouble(reader[6]),
        //                        credit = Convert.ToDouble(reader[7]),
        //                        voucher_date = Convert.ToDateTime(reader[8]),
        //                    };

        //                    if (voucher.account_type_id == 1 && voucher.debit > 0)
        //                    {
        //                        voucherEntriesList.Add(voucher);
        //                    }
        //                    else if (voucher.account_type_id == 2 && voucher.debit > 0)
        //                    {
        //                        voucherEntriesList.Add(voucher);
        //                    }
        //                    else if (voucher.account_type_id == 3 && voucher.credit > 0)
        //                    {
        //                        voucherEntriesList.Add(voucher);
        //                    }
        //                    else if (voucher.account_type_id == 4 && voucher.credit > 0)
        //                    {
        //                        voucherEntriesList.Add(voucher);
        //                    }
        //                    else if (voucher.account_type_id == 5 && voucher.credit > 0)
        //                    {
        //                        voucherEntriesList.Add(voucher);
        //                    }
        //                    else
        //                    {
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return voucherEntriesList;
        //}

        public List<sms_voucher_entries> getAllCashFlowStatement(DateTime sDate, DateTime eDate)
        {
            List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();
            List<sms_voucher_entries> exp_list = new List<sms_voucher_entries>();
            List<chart_of_accounts> chartOfAccountList = getAllChartOfAccounts();

            //opening balances
            foreach (var coa in chartOfAccountList.Where(x => x.isCashAccount != ""))
            {
                sms_voucher_entries voucher = new sms_voucher_entries()
                {
                    account_type_id = 6,
                    account_type = "Opening Balances",
                    account_head_id = coa.p_id,
                    account_head = chartOfAccountList.Where(x => x.id == coa.p_id).First().account_name,
                    account_detail_id = coa.id,
                    account_detail = coa.account_name,
                    debit = getOpeningBalance(coa.id, sDate),
                    credit = 0,
                };
                voucherEntriesList.Add(voucher);
            }

            //closing balance
            foreach (var coa in chartOfAccountList.Where(x => x.isCashAccount != ""))
            {
                sms_voucher_entries voucher = new sms_voucher_entries()
                {
                    account_type_id = coa.account_type_id,
                    account_type = "Closing Balances",
                    account_head_id = coa.p_id,
                    account_head = chartOfAccountList.Where(x => x.id == coa.p_id).First().account_name,
                    account_detail_id = coa.id,
                    account_detail = coa.account_name,
                    debit = getOpeningBalance(coa.id, eDate),
                    credit = 0,
                };
                voucherEntriesList.Add(voucher);
            }


            double balance = 0;
            double e_debit = 0;
            double e_credit = 0;

            double r_debit = 0;
            double r_credit = 0;

            double t_expense = 0;
            double t_revenue = 0;
            double income = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select coa_type.id, coa_type.account_type, ve.account_head_id, ve.account_head, ve.account_detail_id, ve.account_detail,"
                                           + "ve.debit, ve.credit, v.voucher_date from sms_voucher_entries AS ve Inner Join sms_voucher AS v on ve.voucher_id = v.id "
                                           + "Inner join sms_chart_of_accounts AS coa on ve.account_detail_id= coa.id "
                                           + "Inner join sms_account_types as coa_type on coa.account_type_id = coa_type.id "
                                           + "where (coa_type.id=2 OR coa_type.id=3) && (DATE(v.voucher_date) >= @sDate && DATE(v.voucher_date) <= @eDate)";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                account_type_id = Convert.ToInt32(reader[0]),
                                account_type = Convert.ToString(reader[1]),
                                account_head_id = Convert.ToInt32(reader[2]),
                                account_head = Convert.ToString(reader[3]),
                                account_detail_id = Convert.ToInt32(reader[4]),
                                account_detail = Convert.ToString(reader[5]),
                                debit = Convert.ToDouble(reader[6]),
                                credit = Convert.ToDouble(reader[7]),
                                voucher_date = Convert.ToDateTime(reader[8]),
                            };
                            if (voucher.account_type_id == 2)
                            {
                                e_debit = e_debit + voucher.debit;
                                e_credit = e_credit + voucher.credit;
                                exp_list.Add(voucher);
                            }
                            if (voucher.account_type_id == 3)
                            {
                                r_debit = r_debit + voucher.debit;
                                r_credit = r_credit + voucher.credit;
                            }
                            voucherEntriesList.Add(voucher);
                        }
                    }
                }

                t_expense = e_debit - e_credit;
                t_revenue = r_credit - r_debit;
                income = t_revenue - t_expense;
                // finaliszing
                foreach (var item in voucherEntriesList)
                {
                    item.total_expense = t_expense;
                    item.total_revenue = t_revenue;
                    item.income = income;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return voucherEntriesList;
        }

        public List<sms_account_type> getAllAccountsTypes()
        {
            List<sms_account_type> accList = new List<sms_account_type>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_account_type";
                        cmd.Connection = con;
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            sms_account_type acc = new sms_account_type()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                account_type = Convert.ToString(reader["account_type"]),
                                is_active = Convert.ToString(reader["is_active"]),
                            };
                            accList.Add(acc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return accList;
        }

        public List<sms_voucher_types> getAllVoucherTypes()
        {
            List<sms_voucher_types> voucherTypesList = new List<sms_voucher_types>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher_types";
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_types voucher = new sms_voucher_types()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_type = Convert.ToString(reader["voucher_type"]),
                                description = Convert.ToString(reader["description"]),

                            };
                            voucherTypesList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherTypesList;
        }

        public int getLastVoucherNo(int voucher_id)
        {
            int last_voucher_no;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select last_voucher_no from sms_voucher_types where id = @voucher_id";
                        cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = voucher_id;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        last_voucher_no = Convert.ToInt32(reader["last_voucher_no"]);

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return last_voucher_no;
        }

        public int updateVoucherNo(int voucher_id, int new_voucher_no)
        {
            int status = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "update sms_voucher_types SET last_voucher_no=@voucher_no where id = @voucher_id";
                        cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = voucher_id;
                        cmd.Parameters.Add("@voucher_no", MySqlDbType.Int32).Value = new_voucher_no;

                        cmd.Connection = con;
                        con.Open();
                        status = Convert.ToInt32(cmd.ExecuteScalar());

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return status;
        }

        public List<sms_voucher> getAllVoucher()
        {
            List<sms_voucher> voucherList = new List<sms_voucher>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher ORDER BY voucher_date ASC";
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher voucher = new sms_voucher()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_no = Convert.ToString(reader["voucher_no"]),
                                voucher_no_int = Convert.ToInt32(reader["voucher_no_int"]),
                                voucher_type_id = Convert.ToInt32(reader["voucher_type_id"]),
                                voucher_type = Convert.ToString(reader["voucher_type"]),
                                voucher_description = Convert.ToString(reader["voucher_description"]),
                                voucher_date = Convert.ToDateTime(reader["voucher_date"]),
                                amount = Convert.ToDouble(reader["amount"]),
                                cheque_date = Convert.ToDateTime(reader["cheque_date"]),
                                cheque_no = Convert.ToInt32(reader["cheque_no"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                is_posted = Convert.ToString(reader["is_posted"]),
                            };
                            voucherList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherList;
        }

        public List<sms_voucher> getAllVoucherByDate(DateTime sDate, DateTime eDate)
        {
            List<sms_voucher> voucherList = new List<sms_voucher>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher where DATE(voucher_date) >= @sDate && DATE(voucher_date) <= @eDate ORDER BY date_time DESC";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher voucher = new sms_voucher()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_no = Convert.ToString(reader["voucher_no"]),
                                voucher_no_int = Convert.ToInt32(reader["voucher_no_int"]),
                                voucher_type_id = Convert.ToInt32(reader["voucher_type_id"]),
                                voucher_type = Convert.ToString(reader["voucher_type"]),
                                voucher_description = Convert.ToString(reader["voucher_description"]),
                                voucher_date = Convert.ToDateTime(reader["voucher_date"]),
                                amount = Convert.ToDouble(reader["amount"]),
                                cheque_date = Convert.ToDateTime(reader["cheque_date"]),
                                cheque_no = Convert.ToInt32(reader["cheque_no"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                is_posted = Convert.ToString(reader["is_posted"]),
                            };
                            voucherList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherList;
        }

        public List<sms_voucher_entries> getAllVoucherEntriesByVoucherID(int voucherId)
        {
            List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher_entries where voucher_id = @voucher_id ORDER By date_time DESC";
                        cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = voucherId;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_no = Convert.ToString(reader["voucher_no"]),
                                account_detail_id = Convert.ToInt32(reader["account_detail_id"]),
                                account_detail = Convert.ToString(reader["account_detail"]),
                                account_head_id = Convert.ToInt32(reader["account_head_id"]),
                                account_head = Convert.ToString(reader["account_head"]),
                                balance = Convert.ToDouble(reader["balance"]),
                                description = Convert.ToString(reader["description"]),
                                debit = Convert.ToDouble(reader["debit"]),
                                credit = Convert.ToDouble(reader["credit"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                            };
                            voucherEntriesList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherEntriesList;
        }

        public List<sms_voucher_entries> getAllVoucherEntriesByVoucherDateAndType(DateTime sDate, DateTime eDate)
        {
            List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher_entries where (DATE(voucher_date) >= @sDate && DATE(voucher_date) <= @eDate) ORDER By id DESC";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_no = Convert.ToString(reader["voucher_no"]),
                                account_detail_id = Convert.ToInt32(reader["account_detail_id"]),
                                account_detail = Convert.ToString(reader["account_detail"]),
                                account_head_id = Convert.ToInt32(reader["account_head_id"]),
                                account_head = Convert.ToString(reader["account_head"]),
                                balance = Convert.ToDouble(reader["balance"]),
                                description = Convert.ToString(reader["description"]),
                                debit = Convert.ToDouble(reader["debit"]),
                                credit = Convert.ToDouble(reader["credit"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                            };
                            voucherEntriesList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherEntriesList;
        }

        public List<sms_voucher_entries> getAllVoucherEntries()
        {
            List<sms_voucher_entries> voucherEntriesList = new List<sms_voucher_entries>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_voucher_entries ORDER By date_time DESC";
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher_entries voucher = new sms_voucher_entries()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                voucher_id = Convert.ToInt32(reader["voucher_id"]),
                                voucher_no_int = Convert.ToInt32(reader["voucher_no_int"]),
                                voucher_no = Convert.ToString(reader["voucher_no"]),
                                account_detail_id = Convert.ToInt32(reader["account_detail_id"]),
                                account_detail = Convert.ToString(reader["account_detail"]),
                                account_head_id = Convert.ToInt32(reader["account_head_id"]),
                                account_head = Convert.ToString(reader["account_head"]),
                                balance = Convert.ToDouble(reader["balance"]),
                                description = Convert.ToString(reader["description"]),
                                debit = Convert.ToDouble(reader["debit"]),
                                credit = Convert.ToDouble(reader["credit"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                emp_id = Convert.ToInt32(reader["emp_id"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                            };
                            voucherEntriesList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherEntriesList;
        }


        public int submitVoucher(sms_voucher voucher)
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

                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_voucher(voucher_no,voucher_no_int, voucher_type_id, voucher_type, voucher_date, voucher_description, amount, cheque_date,cheque_no, created_by, date_time, emp_id) Values(@voucher_no,@voucher_no_int, @voucher_type_id, @voucher_type, @voucher_date, @voucher_description, @amount, @cheque_date,@cheque_no, @created_by, @date_time, @emp_id); SELECT LAST_INSERT_ID()";
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

                                i = Convert.ToInt32(cmd.ExecuteScalar());
                                cmd.Parameters.Clear();
                            }
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "update sms_voucher_types SET last_voucher_no=@voucher_no_int, last_voucher=@voucher_no where id = @voucher_id";
                                cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = voucher.voucher_type_id;
                                cmd.Parameters.Add("@voucher_no_int", MySqlDbType.Int32).Value = voucher.voucher_no_int;
                                cmd.Parameters.Add("@voucher_no", MySqlDbType.VarChar).Value = voucher.voucher_no;

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

        public int updateVoucher(sms_voucher voucher)
        {
            int i = 0;

            i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_voucher SET voucher_description=@voucher_description, amount=@amount, created_by=@created_by, date_time=@date_time, emp_id=@emp_id where id = @id";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = voucher.id;
                        cmd.Parameters.Add("@voucher_description", MySqlDbType.VarChar).Value = voucher.voucher_description;
                        cmd.Parameters.Add("@amount", MySqlDbType.Double).Value = voucher.amount;

                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = voucher.created_by;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = voucher.date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = voucher.emp_id;

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }

        public int postVoucher(sms_voucher voucher)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_voucher SET is_posted=@is_posted, created_by=@created_by, date_time=@date_time, emp_id=@emp_id where id = @id";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = voucher.id;
                        cmd.Parameters.Add("@is_posted", MySqlDbType.VarChar).Value = "Y";

                        cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = voucher.created_by;
                        cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = voucher.date_time;
                        cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = voucher.emp_id;

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return i;
        }

        public int submitVoucherEntries(List<sms_voucher_entries> voucherEntriesList)
        {
            int i = 0;
            foreach (sms_voucher_entries voucherEntry in voucherEntriesList)
            {
                i = 0;
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "INSERT INTO sms_voucher_entries(voucher_id,voucher_no,voucher_no_int,voucher_type_id,voucher_type, account_head_id,account_head,account_detail, account_detail_id, description, debit, credit, balance, created_by, date_time, emp_id) Values(@voucher_id,@voucher_no,@voucher_no_int,@voucher_type_id,@voucher_type, @account_head_id, @account_head,@account_detail, @account_detail_id,  @description, @debit, @credit, @balance, @created_by, @date_time, @emp_id)";
                                cmd.Connection = con;

                                cmd.Parameters.Add("@voucher_id", MySqlDbType.Int32).Value = voucherEntry.voucher_id;
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

                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                cmd.Parameters.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return i;
        }

        public int updateVoucherEntries(List<sms_voucher_entries> voucherEntriesList)
        {
            int i = 0;
            foreach (sms_voucher_entries voucherEntry in voucherEntriesList)
            {
                i = 0;
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                    {
                        con.Open();
                        try
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.CommandText = "UPDATE sms_voucher_entries SET description=@description, debit=@debit, credit=@credit, balance=@balance, created_by=@created_by, date_time=@date_time, emp_id=@emp_id where id=@id";
                                cmd.Connection = con;

                                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = voucherEntry.id;

                                cmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = voucherEntry.description;
                                cmd.Parameters.Add("@debit", MySqlDbType.Double).Value = voucherEntry.debit;
                                cmd.Parameters.Add("@credit", MySqlDbType.Double).Value = voucherEntry.credit;
                                cmd.Parameters.Add("@balance", MySqlDbType.Double).Value = voucherEntry.balance;
                                cmd.Parameters.Add("@created_by", MySqlDbType.VarChar).Value = voucherEntry.created_by;
                                cmd.Parameters.Add("@date_time", MySqlDbType.DateTime).Value = voucherEntry.date_time;
                                cmd.Parameters.Add("@emp_id", MySqlDbType.Int32).Value = voucherEntry.emp_id;

                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                cmd.Parameters.Clear();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return i;
        }

        public int deleteVoucherEntry(int id)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Delete from sms_voucher_entries where id=@id";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
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

        public int deleteVoucher(int id)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Delete from sms_voucher where id=@id";
                        cmd.Connection = con;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
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

        #region Get all revenue and expense
        public List<sms_voucher> getAllVoucherIncomeExpenseGroupWiseByDate(DateTime sDate, DateTime eDate)
        {
            List<sms_voucher> voucherList = new List<sms_voucher>();

            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT ve.voucher_type_id, ve.voucher_date, Sum(amount), ve.voucher_type  FROM sms_voucher AS ve " +
                                            "where (Date(ve.voucher_date) >= @sDate && Date(ve.voucher_date) <= @eDate)" +
                                            "GROUP BY ve.voucher_date, ve.voucher_type_id " +
                                            "ORDER BY ve.voucher_date ASC";
                        cmd.Parameters.Add("@sDate", MySqlDbType.Date).Value = sDate;
                        cmd.Parameters.Add("@eDate", MySqlDbType.Date).Value = eDate;
                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sms_voucher voucher = new sms_voucher()
                            {
                                voucher_type_id = Convert.ToInt32(reader[0]),
                                voucher_date = Convert.ToDateTime(reader[1]),
                                amount = Convert.ToDouble(reader[2]),
                                voucher_type = reader[3].ToString(),
                                
                            };
                            voucherList.Add(voucher);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return voucherList;
        }
        #endregion

    }
}

















