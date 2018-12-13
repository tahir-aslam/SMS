using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.messageEngine.Models
{
   public class admission
    {
        public string id { set; get; }
        public string std_name { set; get; }
        public string father_name { set; get; }
        public string father_cnic { set; get; }
        public string father_income { set; get; }
        public string religion { set; get; }
        public DateTime dob { set; get; }
        public string b_form { set; get; }
        public string parmanent_adress { set; get; }
        public string phone_no { set; get; }
        public string cell_no { set; get; }
        public string emergency_address { set; get; }
        public string previous_school { set; get; }
        public string boarding { set; get; }
        public string transport { set; get; }
        public string comm_adress { set; get; }
        public string class_name { set; get; }
        public string section_name { set; get; }
        public string roll_no { set; get; }
        public string adm_no { set; get; }
        public string reg_fee { set; get; }
        public string adm_fee { set; get; }
        public string tution_fee { set; get; }
        public string scholarship { set; get; }
        public string misc_charges { set; get; }
        public string exam_fee { set; get; }
        public string security_fee { set; get; }
        public string transport_fee { set; get; }
        public string other_exp { set; get; }
        public string stationary_fee { set; get; }
        public string total { set; get; }
        public string is_active { set; get; }
        public DateTime date_time { set; get; }
        public string created_by { set; get; }
        public string class_id { set; get; }
        public string section_id { set; get; }
        public byte[] image { set; get; }
    }
}
