using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
  public  class fee_history
    {
      public string month { set; get; }
      public string particulars { set; get; }
      public string amount { set; get; }
      public DateTime date_time { set; get; }
      public string receipt_no { set; get; }
      public string insertion { set; get; }
      public string updation { set; get; }
    }
}
