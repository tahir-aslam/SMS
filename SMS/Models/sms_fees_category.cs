using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_fees_category
    {
        public int id { get; set; }
        public string fees_category { get; set; }
        public int fees_type_id { get; set; }
        public string is_active { get; set; }
    }
}
