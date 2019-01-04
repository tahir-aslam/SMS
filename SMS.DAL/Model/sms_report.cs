using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_report
    {
        public int total_strength { get; set; }
        public int male_strength { get; set; }
        public int female_strength { get; set; }
        public string session { get; set; }
    }
}
