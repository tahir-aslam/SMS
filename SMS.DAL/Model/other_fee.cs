using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class other_fee
    {
        public int id { set; get; }
        public int std_id { set; get; }
        public int fee_type_id { set; get; }
        public string fee_type { set; get; }
        public string description { set; get; }
        public int amount { set; get; }
        public DateTime date { set; get; }
        public int month_id { set; get; }
        public string month_name { set; get; }
        public DateTime date_time { set; get; }
        public string created_by { set; get; }

        public string std_name { set; get; }
        public string father_name { set; get; }
        public string adm_no { set; get; }
        public string class_name { set; get; }
        public string section_name { set; get; }        
        //public string std_name { set; get; }

    }
}
