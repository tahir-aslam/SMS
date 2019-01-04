using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class exam
    {
       public string id { set; get; }
       public string exam_name { set; get; }
       public DateTime exam_date { set; get; }
       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
       public bool Checked { set; get; }
       
    }
}
