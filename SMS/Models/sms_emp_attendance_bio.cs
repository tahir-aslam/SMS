using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_emp_attendance_bio
    {
        public int id { get; set; }
        public int emp_id { get; set; }
        public string emp_name { get; set; } 
        public string father_name { get; set; }
        public int total_absents { get; set; }
        public int total_presents { get; set; }
        public int total_days { get; set; }
        public double salary { get; set; }
        public double deduction { get; set; }
        public double total_hours { get; set; }
        public DateTime check_in { get; set; }
        public DateTime check_out { get; set; }
        public string attendance { get; set; }        
        public string mode { get; set; }
        public DateTime date_time { get; set; }
        public string created_by { get; set; }
        public string designation { get; set; }
        public int designation_id { get; set; }
        public int emp_login_id { get; set; }
        public byte[] image { get; set; }
        
    }
}
