using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
   public class fee_voucher
    {
       public byte[] institute_logo { set; get; }
       public string institute_name { set;get; }
       public string std_name{set;get;}
       public string std_id { set; get; }
       public string class_name{set;get;}
       public string adm_no{set;get;}
       public string month{set;get;}
       public string father_name { set; get; }
       public string date_time { set; get; }
       public string reciept_no { set; get; }
       public string section_name { set; get; }

       public string other_exp_type { set; get; }
       public int other_exp_type_id { set; get; }
       public int other_exp_id { set; get; }      

       public string rem_reg_fee { set; get; }
       public string rem_adm_fee { set; get; }
       public string rem_tution_fee { set; get; }
       public string rem_transport_fee { set; get; }
       public string rem_other_fee { set; get; }       
       public string rem_exam_fee { set; get; }
       public string rem_security_fee { set; get; }
       public string rem_fine_fee { set; get; }

       public string total { set; get; }
       public string paid { set; get; }
       public string remaining { set; get; }
       public string total_in_words { set; get; }
       public string created_by { set; get; }       

       public string pending_amount { set; get; }
       public string pending_desc { set; get; }

       public string fine_amount { set; get; }
       public string fine_desc { set; get; }

       public string other_amount { set; get; }
       public string other_desc { set; get; }

       public string bank_name { set; get; }
       public string branch_name { set; get; }
       public string account_no { set; get; }
       public string account_title { set; get; }

       public string fee_note { set; get; }

       public List<fee_voucher> pending_list { set; get; }
       public List<fee_voucher> fine_list { set; get; }
       public List<fee_voucher> other_list { set; get; }

    }
}


