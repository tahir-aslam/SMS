//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMS.Models.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sms_admission
    {
        public int id { get; set; }
        public int branch_id { get; set; }
        public string std_name { get; set; }
        public int session_id { get; set; }
        public string father_name { get; set; }
        public string father_cnic { get; set; }
        public string father_income { get; set; }
        public string religion { get; set; }
        public Nullable<System.DateTime> dob { get; set; }
        public string b_form { get; set; }
        public string parmanent_adress { get; set; }
        public string phone_no { get; set; }
        public string cell_no { get; set; }
        public string cell_no2 { get; set; }
        public string emergency_address { get; set; }
        public string previous_school { get; set; }
        public string boarding { get; set; }
        public string transport { get; set; }
        public string comm_adress { get; set; }
        public string class_name { get; set; }
        public string section_name { get; set; }
        public string roll_no { get; set; }
        public int roll_no_int { get; set; }
        public string adm_no { get; set; }
        public int adm_no_int { get; set; }
        public string reg_fee { get; set; }
        public string tution_fee { get; set; }
        public string scholarship { get; set; }
        public string misc_charges { get; set; }
        public string exam_fee { get; set; }
        public string security_fee { get; set; }
        public string stationary_fee { get; set; }
        public string transport_fee { get; set; }
        public string total { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
        public string created_by { get; set; }
        public Nullable<int> class_id { get; set; }
        public string is_active { get; set; }
        public Nullable<int> section_id { get; set; }
        public string other_exp { get; set; }
        public string adm_fee { get; set; }
        public byte[] image { get; set; }
        public string password { get; set; }
        public string insertion { get; set; }
        public string updation { get; set; }
        public string deletion { get; set; }
        public Nullable<System.DateTime> adm_date { get; set; }
        public Nullable<System.DateTime> withdrawal_date { get; set; }
        public string remarks { get; set; }
        public Nullable<int> fees_package_id { get; set; }
        public string fees_package { get; set; }
        public Nullable<int> class_in_id { get; set; }
        public Nullable<int> section_in_id { get; set; }
        public string family_group_id { get; set; }
        public int adm_no_prefix_id { get; set; }
        public int roll_no_prefix_id { get; set; }
        public int city_area_id { get; set; }
    }
}
