using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;

namespace SMS.Models
{
   public class web_events
    {
       public string id { set; get; }
       public DateTime event_date { set; get; }
       public string event_name { set; get; }
       public string event_description { set; get; }
       public byte[] image { set; get; }       
       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }


       public List<web_event_image> event_images_lst { set; get; }
    }
}
