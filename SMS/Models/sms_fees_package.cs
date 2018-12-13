using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_fees_package
    {
        public int id { get; set; }
        public string package_name { get; set; }
        public string is_free { get; set; }
        public string is_active { get; set; }
    }
}
