using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SMS.Models
{
    public class student_attendence : INotifyPropertyChanged
    {
        public string id { set; get; }
        public string std_id { set; get; }
        public string std_name { set; get; }
        public string father_name { set; get; }
        public string cell_no { set; get; }
        public string roll_no { set; get; }
        public string adm_no { set; get; }
        public Int32 adm_no_int { set; get; }
        public Int32 roll_no_int { set; get; }
        public string class_id { set; get; }
        public string class_name { set; get; }
        public string section_name { set; get; }
        public string section_id { set; get; }
        public string att_percentage { set; get; }
        public char attendence { set; get; }
        public List<char> att_lst { get; set; }
        public List<DateTime> att_date_lst { get; set; }
        private bool active;
        public bool Active
        {
            get { return active; }
            set { active = value; ReportChange("Active"); }
        }
        public DateTime date_time { set; get; }
        public DateTime attendence_date { set; get; }
        public string created_by { set; get; }
        public string total_days { set; get; }
        public string total_abs { set; get; }
        public string total_presents { set; get; }
        public int total_attendance { set; get; }

        public int AbsentCount { get; set; }
        public int LeaveCount { get; set; }

        private void ReportChange(string propertyName)
        { if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public bool isPresent { get; set; }
        public bool isAbsent { get; set; }
        public bool isLeave { get; set; }
        public string total_leaves { set; get; }


    }
}
