using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_fine
    {
        public string id { set; get; }
        public string std_id { set; get; }
        public string std_name { set; get; }
        public string father_name { set; get; }
        public string adm_no { set; get; }
        public string class_name { set; get; }
        public string section_name { set; get; }
        public string session_id { set; get; }
        public string fine_type_id { set; get; }
        public string fine_type { set; get; }
        public string fine_description { set; get; }
        public string amount { set; get; }
        public string created_by { set; get; }
        public string insertion { set; get; }
        public string updation { set; get; }
        public DateTime date_time { set; get; }
        public DateTime fine_date { set; get; }
        public string monthId { set; get; }
        public string month { set; get; }
       
    }
}
