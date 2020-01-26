using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Models
{
    public class BaseModel
    {
        public int created_emp_id { get; set; }
        public int updated_emp_id { get; set; }
        public string created_emp_name { get; set; }
        public string updated_emp_name { get; set; }
        public DateTime? created_date_time { get; set; }
        public DateTime? updated_date_time { get; set; }
        public int is_active { set; get; }
        public int is_deleted { set; get; }
        public int is_synchronized { set; get; }
        public int sort_order { set; get; }
    }
}
