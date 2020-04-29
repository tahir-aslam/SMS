using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_exams_subjects : INotifyPropertyChanged
    {
        public int id { set; get; }
        public string subject_name { set; get; }
        public string subject_abb { set; get; }
        public int subjects_group_id { set; get; }
        public string subjects_group { set; get; }
        public int subject_type_id { set; get; }
        public string subject_type { set; get; }
        public string subject_code { set; get; }
        public int class_id { set; get; }
        public string class_name { set; get; }
        public int section_id { set; get; }
        public string section_name { set; get; }
        public int std_id { set; get; }
        public int emp_id { set; get; }
        public string emp_name { set; get; }
        public string remarks { set; get; }
        public string total_marks { set; get; }
        public string is_active { set; get; }
        public string is_deleted { set; get; }
        public string is_synchronized { set; get; }
        public string insertion { set; get; }
        public string updation { set; get; }
        public bool isChecked { get; set; }
        public bool Checked { get; set; }
        private bool _isChecked;
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
        public int sort_order { get; set; }
        
        //public List<subjects> subjects_list { get; set; }
        public int created_emp_id { get; set; }
        public int updated_emp_id { get; set; }
        public string created_emp_name { get; set; }
        public string updated_emp_name { get; set; }
        public DateTime created_date_time { get; set; }
        public DateTime updated_date_time { get; set; }
        public Int32 emp_designation_id { get; set; }
        public string emp_designation { get; set; }
        public DateTime exam_date { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
