using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_voucher
    {
        public int id { set; get; }
        public string voucher_no { set; get; }
        public int voucher_no_int { set; get; }
        public int voucher_type_id { set; get; }
        public string voucher_type { set; get; }
        public DateTime voucher_date { set; get; }
        public string voucher_description { set; get; }
        public double amount { set; get; }
        public string is_posted { set; get; }
        public DateTime cheque_date { set; get; }
        public int cheque_no { set; get; }
        public string created_by { set; get; }
        public DateTime date_time { set; get; }
        public int emp_id { set; get; }

    }
}
