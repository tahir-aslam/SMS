using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class sms_exams_date_sheet : BaseModel
    {
        public int id { get; set; }
        public int exam_id { get; set; }
        public string exam_name { get; set; }
        public int subject_id { get; set; }
        public string subject_name { get; set; }
        public int section_id { get; set; }
        public string section_name { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public DateTime exam_date { get; set; }
        public string exam_time { get; set; }
        public string remarks { get; set; }      
        public bool IsChecked { get; set; } 

    }
}
