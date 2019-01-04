using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_investor
    {
        public string id { set; get; }
        public string investor_name { set; get; }
        public string investor_cell { set; get; }
        public string investor_address { set; get; }
        public string investor_description { set; get; }
        public string created_by { set; get; }
        public DateTime date_time { set; get; }
    }
}
