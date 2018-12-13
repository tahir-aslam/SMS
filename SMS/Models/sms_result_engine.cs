using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_result_engine
    {
        public int success_count { get; set; }
        public int failure_count { get; set; }

        public int serial_no { get; set; }
        public string id { get; set; }
        public string action { get; set; }
        public string reason { get; set; }

        public List<sms_result_engine> success_list { get; set; }
        public List<sms_result_engine> failure_list { get; set; }
    }
}
