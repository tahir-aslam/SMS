using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class institute
    {
       public int institute_id { get; set; }
       public string institute_name { set; get; }
       public byte[] institute_logo { set; get; }
       public string institute_cell { get; set; }
       public string institute_phone { get; set; }
       

       public DateTime installation_date { set; get; }
       public string institute_address { set; get; }
       public string institute_quote { set; get; }
       public string institute_owner_name { set; get; }
       public string institute_owner_cell { set; get; }
       public string expiry_instant { get; set; }

       public DateTime expiry_date { set; get; }
       public string expiry_message { set; get; }
       public string expiry_warning_message { set; get; }
       public int expiry_warning_day { set; get; }

       public byte[] male_image { set; get; }
       public byte[] female_image { set; get; }
       public byte[] bank_logo { set; get; }
       public string mac { set; get; }
       public string insertion { set; get; }
       public string isMultiPartSMSAccess { set; get; }

       public DateTime date { get; set; }
       public int page_no { get; set; }
       public string month_name { get; set; }
       public string year { get; set; }
       public DateTime sDate { get; set; }
       public DateTime eDate { get; set; }

       public bool check { get; set; }


       
    }
}
