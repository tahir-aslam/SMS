using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
  public  class fee
    {
        public string id { set; get; }
        public string reg_fee { set; get; }
        public string adm_fee { set; get; }
        public string tution_fee { set; get; }
        public string transport_fee { set; get; }
        public string month { set; get; }
        public string scholar_fee { set; get; }
        public string misc_charges { set; get; }
        public string exam_fee { set; get; }
        public string fine_fee { set; get; }
        public string security_fee { set; get; }
        public string stationary_fee { set; get; }

        public string other_expenses { set; get; }
        public string other_exp_type { set; get; }
        public string other_exp_type_id { set; get; }
        public int other_exp_id { set; get; }      

        public string total_fee { set; get; }
        public DateTime date_time { set; get; }
        public string created_by { set; get; }
        public string is_active { set; get; }
        public string class_id { set; get; }
        public string class_name { set; get; }
        public string section_id { set; get; }
        public string section_name { set; get; }
        public string roll_no { set; get; }
        public string adm_no { set; get; }
        public string receipt_no { set; get; }
        public string insertion { set; get; }
        public string updation { set; get; }
        public string std_id { set; get; }
        public string std_name { set; get; }
        public string father_name { set; get; }
        public string std_cell_no { set; get; }
        public byte[] image { set; get; }
        public string sms_type { set; get; }
        public byte[] institue_logo { set; get; }
        public string intitue_name { set; get; }

        public string rem_security_fee { set; get; }
        public string rem_reg_fee { set; get; }
        public string rem_adm_fee { set; get; }
        public string rem_tution_fee { set; get; }
        public string rem_transport_fee { set; get; }
        public string rem_other_fee { set; get; }
        public string rem_exam_fee { set; get; }
        public string rem_fine_fee { set; get; }

        public string total_amount { set; get; }
        public string total_paid { set; get; }
        public string total_balance { set; get; }

        public string fine_fee_wave_off { set; get; }
        public string tution_fee_wave_off { set; get; }
        public string other_fee_wave_off { set; get; }

        public char isActive { set; get; }
        public bool Checked { set; get; }

        

    }
}
