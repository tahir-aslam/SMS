using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_voucher_entries
    {
        public int id { set; get; }
        public int voucher_id { set; get; }
        public string voucher_no { set; get; }
        public int voucher_no_int { set; get; }
        public int voucher_type_id { set; get; }
        public string voucher_type { set; get; }
        public string voucher_type_description { set; get; }
        public int account_head_id { set; get; }
        public string account_head { set; get; }
        public int account_detail_id { set; get; }
        public string account_detail { set; get; }
        public string description { set; get; }
        public double debit { set; get; }
        public double credit { set; get; }
        public double balance { set; get; }
        public string created_by { set; get; }
        public DateTime date_time { set; get; }
        public int emp_id { set; get; }

        public DateTime voucher_date { set; get; }
        public string voucher_description { set; get; }
        public DateTime cheque_date { set; get; }
        public int cheque_no { set; get; }
        public string account_code { get; set; }
        public string amount_in_words { get; set; }

        public int account_type_id { get; set; }
        public string account_type { get; set; }

        //report 
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public double total_expense { get; set; }
        public double total_revenue { get; set; }
        public double income { get; set; }

    }
}
