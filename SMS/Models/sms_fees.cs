using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMS.Models
{
    public class sms_fees
    {
        public int id { get; set; }
        public int std_id { get; set; }
        public int fees_category_id { get; set; }
        public string fees_category { get; set; }
        public int fees_sub_category_id { get; set; }
        public string fees_sub_category { get; set; }
        public int actual_amount { get; set; }
        public int amount { get; set; }
        public int rem_amount { get; set; }
        public int rem_amount_group { get; set; }
        public int discount { get; set; }
        public int wave_off { get; set; }
        public int month { get; set; }
        public string month_name { get; set; }
        public string month_name_group { get; set; }
        public string fees_category_group { get; set; }
        public string receipt_no_group { get; set; }
        public int class_id { get; set; }
        public string class_name { get; set; }
        public int section_id { get; set; }
        public string section_name { get; set; }
        public int year { get; set; }
        public DateTime date { get; set; }
        public DateTime due_date { get; set; }
        public int session_id { get; set; }
        public string is_active { get; set; }
        public string adm_is_active { get; set; }
        public string created_by { get; set; }
        public int emp_id { get; set; }
        public DateTime date_time { get; set; }
        public DateTime? date_received { get; set; }
        public int actual_tution_fee { get; set; }
        public int Std_discount_tution_fee { get; set; }
        //public bool Checked { get; set; }

        //Last
        public DateTime last_fees_received { get; set; }
        public string last_receipt_no { get; set; }
        public int last_amount { get; set; }        

        //For sms_fees_paid
        public int fees_generated_id { set; get; }
        public int receipt_no { set; get; }
        public string receipt_no_full { set; get; }
        public int amount_paid { set; get; }

        public int total_amount { set; get; }
        public int total_paid { set; get; }
        public int total_remaining { set; get; }
        public string amount_in_words { set; get; }
        public string fees_note { set; get; }
        public string fees_place { set; get; }

        //For Display
        public byte[] std_image { set; get; }
        public string std_name { set; get; }
        public string father_name { set; get; }
        public string father_cnic { set; get; }
        public string adm_no { set; get; }
        public string roll_no { set; get; }
        public string cell_no { set; get; }
        public bool isChecked { set; get; }
        

        //Fees Collection Place
        public int fees_collection_place_id { get; set; }
        public string fees_collection_place { get; set; }

        //For Institute
        public string institute_name { set; get; }
        public byte[] institute_logo { set; get; }
        public string institute_cell { get; set; }
        public string institute_phone { get; set; }        
        public string school_cell_no { set; get; }

        //bank details------------------------
        public string bank_name { set; get; }
        public string branch_address { set; get; }
        public string account_no { set; get; }
        public string account_title { set; get; }
        public byte[] bank_logo { get; set; }

        //For Report
        public List<admission> adm_list;
        public DateTime toDate { get; set; }
        public DateTime fromDate { get; set; }
        public string r_classes { get; set; }
        public string r_sections { get; set; }
        public string r_fees_Category { get; set; }
        public string r_total_receipts { get; set; }
        public string r_toReceipt { get; set; }
        public string r_fromReceipts { get; set; }
        public string r_users { get; set; }
        public string r_collection_place { get; set; }
        public string r_months { get; set; }
        public string r_years { get; set; }
        public string r_total_students { get; set; }
        public string r_standard_discount { get; set; }
        public bool Checked { get; set; }

    }
}
