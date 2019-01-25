using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_exam_admin_panel
    {
        public string position_visibility { set; get; }
        public string position_text_visibility { set; get; }
        public string position_percentage { set; get; }
        public string position_limit { set; get; }
        public string attendance_visibility { set; get; }
        public string attendance_text_visibility { set; get; }
        public string image_visibility { set; get; }
        public string remarks_visibility { set; get; }
        public string remarks_text_visibility { set; get; }
        public string teacher_visibility { set; get; }
        public string principal_visibility { set; get; }
        public string parents_visibility { set; get; }
        public string teacher_sig_text { set; get; }
        public string principal_sig_text { set; get; }
        public string parents_sig_text { set; get; }    
        public byte[] teacher_sig_image { get; set; }
        public byte[] principal_sig_image { get; set; }
        public byte[] parents_sig_image { get; set; }
    }
}
