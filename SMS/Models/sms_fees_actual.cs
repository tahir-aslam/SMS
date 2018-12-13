using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_fees_actual
    {
        public int id { set; get; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int std_id { set; get; }
        public int fees_category_id { set; get; }
        public string fees_category { set; get; }
        public int fees_sub_category_id { get; set; }
        public string fees_sub_category { get; set; }
        public int actual_amount { set; get; }
        public int amount { set; get; }
        public int discount { set; get; }       
        public int emp_id { get; set; }
        public DateTime date_time { set; get; }
        public string created_by { set; get; }
    }
}
