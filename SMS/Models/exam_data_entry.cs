using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SMS.Models
{
    public class exam_data_entry : INotifyPropertyChanged
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
       public string father_name { set; get; }
       public string adm_no { set; get; }
       public string roll_no { set; get; }
       public string comm_adress { set; get; }
       public string cell_no { set; get; }
       public byte[] std_img { set; get; }
       public byte[] institute_logo { set; get; }
       public string institute_name { set; get; }


       public string total_marks { set; get; }
       public string obtained_marks { set; get; }
       public string percentage { set; get; }
       public double percentageDouble { set; get; }
       public string grade { set; get; }
       public string remarks { set; get; }
       public string position { set; get; }
       public string class_position { set; get; }
       public string total_remarks { set; get; }

       public bool Checked { get; set; }

       public string subject_id { set; get; }
       public string subject_name { set; get; }
       public string subject_total { set; get; }
       public string subject_obtained { set; get; }
       public string subject_percentage { set; get; }
       public string subject_grade { set; get; }
       public string subject_remarks { set; get; }

       public string subject_obtained_grand { set; get; }
       public string subject_total_grand { set; get; }

       public string oral_obtained { set; get; }
       public string max_marks { set; get; }
       public string max_obtained { set; get; }
       public string max_grade { set; get; }
       public string max_remarks { set; get; }
       public string max_percentage { set; get; }

       public string created_by { set; get; }
       public DateTime date_time { set; get; }
       public string insertion { set; get; }
       public string updation { set; get; }
       public List<exam_data_entry> subj_list { set; get; }

       public string total_days { set; get; }
       public string total_presents { set; get; }
       public string total_absents { set; get; }
        public string total_leaves { set; get; }
        public string att_percentage { set; get; }

       public string sectionStrength { set; get; }
       public string classStrength { set; get; }

       public string teacher_sig_text { set; get; }
       public string principal_sig_text { set; get; }
       public string parents_sig_text { set; get; }

        public Byte[] teacher_sig_image { get; set; }
        public Byte[] principal_sig_image { get; set; }
        public Byte[] parents_sig_image { get; set; }

        public string date { set; get; }

       private void ReportChange(string propertyName)
       {
           if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
       }

       #region INotifyPropertyChanged Members

       public event PropertyChangedEventHandler PropertyChanged;

       #endregion
           
    }
}
