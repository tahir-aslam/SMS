using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_emp_designation
    {
        public int id { get; set; }
        public string designation { get; set; }
        public string is_active { get; set;     }
        public int sort_order { get; set; }
    }
}
