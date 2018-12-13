using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SMS.Models
{
  public  class roles
    {
      public string id { set; get; }
      public string module_name { set; get; }
      public string module_pid { set; get; }
      public string is_active { get; set; }
      public ObservableCollection<roles> children { get; set; }
      public bool Checked { get; set; }
      public string created_by { set; get; }
      public DateTime date_time { set; get; }
      public string insertion { set; get; }
      public string updation { set; get; }
      public string module_id { set; get; }
      public string emp_id { set; get; }
      
    }
}
