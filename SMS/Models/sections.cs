using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class sections : INotifyPropertyChanged
    {
        public string id { set; get; }
        public string class_id { set; get; }
        public string class_name { set; get; }
        public string section_name { set; get; }
        public string emp_id { set; get; }
        public string emp_name { set; get; }
        public string is_active { set; get; }
        public DateTime date_time { set; get; }
        public string created_by { set; get; }
        public string insertion { set; get; }
        public string updation { set; get; }
        public string roll_no_format { set; get; }
        public bool isChecked { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)

                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
