using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class sms_exam_requirements
    {
       public double lower_percentage { set; get; }
       public double upper_percentage { set; get; }
       public string remarks { set; get; }
       public string overall_remarks { set; get; }
       public string grade { set; get; }
       public int fail_subjects { set; get; }
    }
}
