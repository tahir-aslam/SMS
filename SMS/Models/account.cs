using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class account
    {
       public string id { set; get; }
       public string account_name { set; get; }
       public string account_desc { set; get; }
       public string account_holder_name { set; get; }
       public string account_holder_cell { set; get; }
       public string account_holder_phn { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
       public DateTime date_time { set; get; }
       public string created_by { set; get; }
    }
}
