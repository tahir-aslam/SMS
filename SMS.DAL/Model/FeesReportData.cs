using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class FeesReportData
    {
        public int std_id { get; set; }        
        public string std_name { set; get; }
        public string father_name { set; get; }        
        public string fees_category { get; set; }        
        public string fees_sub_category { get; set; }
        public int amount { get; set; }
    }
}
