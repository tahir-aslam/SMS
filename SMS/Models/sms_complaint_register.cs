using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SMS.Models
{
    public class sms_complaint_register : admission
    {
        public int id { get;set;}
        public int std_id { get; set; }
        public int complaint_type_id { get; set; }
        public string complaint_type { get; set; }
        public int complaint_status_id { get; set; }
        public string complaint_status { get; set; }
        public int complaint_from_id { get; set; }
        public string complaint_from { get; set; }
        public string complaint_remarks { get; set; }
        public string complaint_resolved_remarks { get; set; }        
        public DateTime complaint_date { get; set; }
        public DateTime complaint_resolved_date { get; set; }
        public string created_by { get; set; }
        public string updated_by { get; set; }
        public int emp_id { get; set; }
        public DateTime created_date_time { get; set; }
        public DateTime updated_date_time { get; set; }       
    }
}
