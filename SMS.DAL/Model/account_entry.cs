using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class account_entry
    {
       public string id { set; get; }
       public string account_id { set; get; }
       public string account_name { set; get; }
       public string amount { set; get; }
       public string expenditure { set; get; }
       public DateTime date { set; get; }
       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
       public string cheque_no { set; get; }
    }
}

