using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class chart_of_accounts
    {
        public int id { set; get; }
        public string account_name { set; get; }
        public int p_id { set; get; }
        public int account_code { set; get; }
        public string account_full_code { set; get; }
        public int account_type_id { set; get; }
        public string account_type { set; get; }
        public int account_category_id { set; get; }
        public DateTime date_time { set; get; }
        public string created_by { set; get; }
        public int emp_id { set; get; }
        public string isCashAccount { get; set; }
    }
}
