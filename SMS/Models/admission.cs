using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
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
        public DateTime adm_date { set; get; }
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
        public string gender { set; get; }
        public string stationary_fee { set; get; }
        public string total { set; get; }
        public string is_active { set; get; }
        public DateTime date_time { set; get; }
        public string created_by { set; get; }
        public string class_id { set; get; }
        public string section_id { set; get; }
        public byte[] image { set; get; }
        public byte[] std_image { set; get; }
        public string insertion { set; get; }
        public string updation { set; get; }
        public string sms_status { set; get; }
        public string sms_message { set; get; }
        public string sms_type { set; get; }
        public bool Checked { get; set; }
        public string remarks { set; get; }
        public DateTime? withdrawal_date { set; get; }
        public string institute_name { set; get; }
        public byte[] institute_logo { set; get; }
        public int session_id { get; set; }
        public int fees_package_id { set; get; }
        public string fees_package { set; get; }

        public string dob_in_words { get; set; }
        public string age_in_words { get; set; }

        public int roll_no_prefix_id { set; get; }
        public int adm_no_prefix_id { set; get; }
        public int area_id { set; get; }
        public int class_in_id { set; get; }
        public string class_in_name { set; get; }
        public int roll_no_int { get; set; }
        public int adm_no_int { get; set; }
        public int family_group_id { get; set; }

        //roll no slip
        public string exam_name { get; set; }
        public DateTime exam_date { get; set; }
        public string exam_time { get; set; }
        public string exam_remarks { get; set; }
        public string subject_name { get; set; }
    }
}

