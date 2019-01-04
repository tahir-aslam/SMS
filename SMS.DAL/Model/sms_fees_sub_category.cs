using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_fees_sub_category
    {
        public int id { get; set; }
        public int fees_category_id { get; set; }
        public string fees_sub_category { get; set; }
        public string is_active { get; set; }
    }
}
