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
    
    public partial class sms_emp_login
    {
        public int id { get; set; }
        public Nullable<int> emp_id { get; set; }
        public Nullable<int> emp_type_id { get; set; }
        public string emp_user_name { get; set; }
        public string emp_pwd { get; set; }
        public string created_by { get; set; }
        public string updation { get; set; }
        public string deletion { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
        public string insertion { get; set; }
        public string branded_url { get; set; }
        public string branded_url_encoded { get; set; }
        public string branded_user_name { get; set; }
        public string branded_pwd { get; set; }
        public string branded_name { get; set; }
        public string branded_check_remaining_url { get; set; }
        public int branch_id { get; set; }
    }
}
