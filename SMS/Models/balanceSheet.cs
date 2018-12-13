using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMS.Models;

namespace SMS.Models
{
    public class balanceSheet
    {
        public byte[] image { set; get; }
        public string institute_name { set; get; }
        public string report_name { set; get; }
        public List<account_entry> account_entry_list { set; get; }
        public List<sms_investments> investment_list { set; get; }
        public string total_investments { set; get; }
        public string total_fee { set; get; }
        public string total_expenses { set; get; }
        public string total_amount { set; get; }
        public string date { set; get; }

            
    }
}
