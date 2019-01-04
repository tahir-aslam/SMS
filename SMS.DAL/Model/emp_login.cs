using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class emp_login
    {
       public string id { set; get; }
       public string emp_id { set; get; }
       public string emp_type_id { set; get; }
       public string emp_user_name { set; get; }
       public string emp_name { set; get; }
       public string emp_pwd { set; get; }
       public string created_by { set; get; }
       public string branded_url { set; get; }
       public string branded_url_encoded { set; get; }
       public string branded_user_name { set; get; }
       public string branded_pwd { set; get; }
       public string branded_name { set; get; }
       public string branded_check_remaining_url { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
    }
}
