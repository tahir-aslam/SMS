using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class exam_marks
    {
       public string subj_id{set;get;}
       public string subj_name { set; get; }
       public string subj_total { set; get; }
       public string subj_obtained { set; get; }
       //public string total { set; get; }
       public string total_obtained { set; get; }
       public string percentage { set; get; }
       public string grade { set; get; }
       public string remarks { set; get; }

    }
}
