using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class AttendanceReport
    {
       public string class_name { set; get; }
       public string section_name { set; get; }
       public string strength { set; get; }
       public string absents { set; get; }
       public string presents { set; get; }       
    }
}
