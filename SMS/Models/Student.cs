using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
  public  class Student
    {
        public String StudentName { get; set; }
        public int StudentId { get; set; }
        public List<decimal> ProjectScores { set; get; }
        public List<string> TitleList{set;get;}
        
    }
}
