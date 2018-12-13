using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class sms
    {
       public string id { set; get; }
       public string msg_name { set; get; }
       public string msg { set; get; }
       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
    }
}
