using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class sms_history
    {
       public string id { set; get; }
       public string sender_id { set; get; }
       public string sender_name { set; get; }
       public string created_by { set; get; }
       public string insertion { set; get; }
       public string class_id { set; get; }
       public string class_name { set; get; }
       public string section_id { set; get; }
       public string section_name { set; get; }
       public string cell { set; get; }
       public string msg { set; get; }
       public string sms_type { set; get; }
       public DateTime date_time { set; get; }
       public DateTime toDate { set; get; }
       public DateTime fromDate { set; get; }
    }
}
