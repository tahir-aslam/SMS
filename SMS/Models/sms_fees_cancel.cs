using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_fees_cancel
    {
        public int id { get; set; }
        public string voucher_no { get; set; }
        public int voucher_no_int { get; set; }
        public int account_head_id { get; set; }
        public int account_detail_id { get; set; }
        public int voucher_type_id { get; set; }
        public string description { get; set; }
        public double amount { get; set; }
        public int std_id { get; set; }
        public string created_by { get; set; }
        public DateTime date_time { get; set; }
        public int emp_id { get; set; }       
    }
}
