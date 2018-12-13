using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SMS.Models
{
    public class employee_attendence : INotifyPropertyChanged
    {       
            public string id { set; get; }
            public int session_id { get; set; }
            public string emp_id { set; get; }
            public string emp_name { set; get; }
            public string emp_type_id { set; get; }
            public string emp_type { set; get; }
            public DateTime date_time { set; get; }
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
            public DateTime attendence_date { set; get; }
            public string created_by { set; get; }
            public string total_days { set; get; }
            public string total_abs { set; get; }
            public string total_presents { set; get; }
            public string total_leaves { set; get; }

            private void ReportChange(string propertyName)
            { if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            public bool isPresent { get; set; }
            public bool isAbsent { get; set; }
            public bool isLeave { get; set; }

        }
    
}
