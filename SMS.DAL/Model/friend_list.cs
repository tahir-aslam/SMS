using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class friend_list
    {
       public string id { set; get; }
       public string friend_name { set; get; }
       public string friend_cell { set; get; }
       public string friend_occupation { set; get; }
       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
       public bool Checked { get; set; }
       public int freind_type_id { get; set; }
    }
}
