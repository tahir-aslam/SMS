using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class exam_entry
    {
       public string id { set; get; }
       public string exam_id { set; get; }
       public string exam_name { set; get; }
       public string class_id { set; get; }
       public string class_name { set; get; }
       public string section_id { set; get; }
       public string section_name { set; get; }
       public string std_id { set; get; }
       public string std_name { set; get; }
       public string obtained_marks { set; get; }
       public string total_marks { set; get; }
       public string percentage { set; get; }
       public string remarks { set; get; }
       public string grade { set; get; }
       public string position { set; get; }
       public string position_tb { set; get; }
       public string std_roll_no { set; get; }
       public string std_cell_no { set; get; }
       public byte[] std_img { set; get; }
       public byte[] institute_logo { set; get; }
       public string institute_name { set; get; }
       public string father_name { set; get; }
       public string date_time { set; get; }


       public List<admission> adm_list{set;get;}
       public List<exam_marks> marks_list { set; get; }

       public string subj1_id { set; get; }
       public string subj1_name { set; get; }
       public string subj1_marks { set; get; }
       public string subj1_total { set; get; }

       public string subj2_id { set; get; }
       public string subj2_name { set; get; }
       public string subj2_marks { set; get; }
       public string subj2_total { set; get; }

       public string subj3_id { set; get; }
       public string subj3_name { set; get; }
       public string subj3_marks { set; get; }
       public string subj3_total { set; get; }

       public string subj4_id { set; get; }
       public string subj4_name { set; get; }
       public string subj4_marks { set; get; }
       public string subj4_total { set; get; }

       public string subj5_id { set; get; }
       public string subj5_name { set; get; }
       public string subj5_marks { set; get; }
       public string subj5_total { set; get; }

       public string subj6_id { set; get; }
       public string subj6_name { set; get; }
       public string subj6_marks { set; get; }
       public string subj6_total { set; get; }

       public string subj7_id { set; get; }
       public string subj7_name { set; get; }
       public string subj7_marks { set; get; }
       public string subj7_total { set; get; }

       public string subj8_id { set; get; }
       public string subj8_name { set; get; }
       public string subj8_marks { set; get; }
       public string subj8_total { set; get; }

       public string subj9_id { set; get; }
       public string subj9_name { set; get; }
       public string subj9_marks { set; get; }
       public string subj9_total { set; get; }

       public string subj10_id { set; get; }
       public string subj10_name { set; get; }
       public string subj10_marks { set; get; }
       public string subj10_total { set; get; }
       
       
    }
}
