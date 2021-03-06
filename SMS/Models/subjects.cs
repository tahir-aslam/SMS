﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class subjects
    {
       public string id { set; get; }
       public string subject_name { set; get; }
        public string subject_abb { set; get; }
        public int subjects_group_id { set; get; }
        public string subjects_group { set; get; }
        public int subject_type_id { set; get; }
        public string subject_type { set; get; }
        public string subject_code { set; get; }       
        public string class_id { set; get; }
       public string class_name { set; get; }
       public string emp_id { set; get; }
       public string emp_name { set; get; }
       public string remarks { set; get; }
       public string total_marks { set; get; }
       public string is_Active { set; get; }
       public DateTime date_time { set; get; }
       public string created_by { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
       public bool Checked { get; set; }
       //public List<subjects> subjects_list { get; set; }
        public int created_emp_id { get; set; }
        public int updated_emp_id { get; set; }
        public DateTime created_date_time { get; set; }
        public DateTime updated_date_time { get; set; }
    }
}
