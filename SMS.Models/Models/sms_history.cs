//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMS.Models.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sms_history
    {
        public int id { get; set; }
        public Nullable<int> sender_id { get; set; }
        public string sender_name { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
        public string created_by { get; set; }
        public string insertion { get; set; }
        public Nullable<int> class_id { get; set; }
        public string class_name { get; set; }
        public Nullable<int> section_id { get; set; }
        public string section_name { get; set; }
        public string cell { get; set; }
        public string msg { get; set; }
        public string sms_type { get; set; }
        public string is_branded { get; set; }
        public string brand_name { get; set; }
        public string updation { get; set; }
        public string deletion { get; set; }
        public int branch_id { get; set; }
    }
}
