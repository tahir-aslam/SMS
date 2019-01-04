using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class web_messages
    {
       public string id { set; get; }
       public string message_heading { set; get; }
       public string message_description { set; get; }
       public byte[] image { set; get; }
       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
    }
}
