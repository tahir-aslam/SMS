using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_complaint_type
    {
        public int id { get; set; }
        public string complaint_type { get; set; }
        public string complaint_type_abb { get; set; }
        public string remarks { get; set; }
        public int complaint_from_id { get; set; }
    }
}
