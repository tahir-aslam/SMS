using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class sms_investments
    {
       public string id { set; get; }
       public string investor_id { set; get; }
       public string investor_name { set; get; }
       public string amount { set; get; }
       public string description { set; get; }
       public DateTime investment_date { set; get; }
       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
       public string cheque_no { set; get; }
    }
}
