using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SMS.Models;
using MySql.Data.MySqlClient;
using SUT.PrintEngine.Utils;
using System.Windows.Markup;
using System.Data;

namespace SMS.StudentManagement.AttendanceReport
{
    /// <summary>
    /// Interaction logic for DailyAttendanceReportPage.xaml
    /// </summary>
    public partial class DailyAttendanceReportPage : Page
    {
        public static DateTime dt;
        public static int t_strenght = 0;
        public static int t_absents = 0;
        public static int t_presents = 0;
        List<classes> classes_list;
        List<sections> sections_list;
        List<admission> adm_list;
        List<AttendanceReportClass> report_list;
        List<student_attendence> all_attendence_list;

        public DailyAttendanceReportPage()
        {
            InitializeComponent();            
            get_all_sections();
            get_all_admissions();
            dt_picker.SelectedDate = DateTime.Now;            
        }

        private void dt_picker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dt = dt_picker.SelectedDate.Value;
            get_all_attendence();
            get_report();
        }

        public void get_report()
        {
            report_list = new List<AttendanceReportClass>();
            AttendanceReportClass arc_obj;
            int count = 0;
            int absents = 0;
            int presents = 0;

            t_strenght = 0;
            t_absents = 0;
            t_presents = 0;

            t_strenght = adm_list.Count;

            foreach (sections sec in sections_list) 
            {                
                count = 0;
                absents = 0;
                presents = 0;
                
                foreach (student_attendence sa in all_attendence_list.Where(x=>x.section_id == sec.id)) 
                {
                    if(sa.attendence == 'A')
                    {
                        absents++;
                        t_absents++;
                    }
                    if(sa.attendence == 'P')
                    {
                        presents++;
                        t_presents++;
                    }
                }
                
                foreach(admission adm in adm_list.Where(x=>x.section_id == sec.id))
                {
                    count++;
                }

                arc_obj = new AttendanceReportClass();
                arc_obj.class_name = sec.class_name;
                arc_obj.section_name = sec.section_name;
                arc_obj.strength = count.ToString();
                arc_obj.presents = presents.ToString();
                arc_obj.absents = absents.ToString();

                report_list.Add(arc_obj);
            }
            report_datagrid.ItemsSource = report_list;
            t_strength_tb.Text = t_strenght.ToString();
            t_presents_tb.Text = t_presents.ToString();
            t_absents_tb.Text = t_absents.ToString();
        }
              

        //------------         Get All Sections   ------------------------
        public void get_all_sections()
        {
            sections_list = new List<sections>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects ORDER BY class_id ASC";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            sections section = new sections()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                section_name = Convert.ToString(reader["section_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),


                            };
                            sections_list.Add(section);

                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        // ===============     Get All Admissions          ================
        public void get_all_admissions()
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y' && session_id=" + MainWindow.session.id + " ORDER BY adm_no ASC";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();                   

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        admission adm = new admission()
                        {
                            id = Convert.ToString(reader["id"].ToString()),
                            std_name = Convert.ToString(reader["std_name"].ToString()),
                            father_name = Convert.ToString(reader["father_name"].ToString()),
                            father_cnic = Convert.ToString(reader["father_cnic"].ToString()),
                            father_income = Convert.ToString(reader["father_income"].ToString()),
                            religion = Convert.ToString(reader["religion"].ToString()),
                            dob = Convert.ToDateTime(reader["dob"]),
                            b_form = Convert.ToString(reader["b_form"].ToString()),
                            parmanent_adress = Convert.ToString(reader["parmanent_adress"].ToString()),
                            adm_date = Convert.ToDateTime(reader["adm_date"]),
                            cell_no = Convert.ToString(reader["cell_no"].ToString()),
                            emergency_address = Convert.ToString(reader["emergency_address"].ToString()),
                            previous_school = Convert.ToString(reader["previous_school"].ToString()),
                            boarding = Convert.ToString(reader["boarding"].ToString()),
                            transport = Convert.ToString(reader["transport"].ToString()),
                            comm_adress = Convert.ToString(reader["comm_adress"].ToString()),
                            class_id = Convert.ToString(reader["class_id"].ToString()),
                            class_name = Convert.ToString(reader["class_name"].ToString()),
                            section_id = Convert.ToString(reader["section_id"].ToString()),
                            section_name = Convert.ToString(reader["section_name"].ToString()),
                            roll_no = Convert.ToString(reader["roll_no"].ToString()),
                            adm_no = Convert.ToString(reader["adm_no"].ToString()),
                            transport_fee = Convert.ToString(reader["transport_fee"].ToString()),
                            reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                            tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                            exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                            security_fee = Convert.ToString(reader["security_fee"].ToString()),
                            stationary_fee = Convert.ToString(reader["stationary_fee"].ToString()),
                            scholarship = Convert.ToString(reader["scholarship"].ToString()),
                            misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                            other_exp = Convert.ToString(reader["other_exp"].ToString()),
                            adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                            total = Convert.ToString(reader["total"].ToString()),
                            date_time = Convert.ToDateTime(reader["date_time"]),
                            created_by = Convert.ToString(reader["created_by"].ToString()),
                            is_active = Convert.ToString(reader["is_active"].ToString()),                            
                        };
                        adm_list.Add(adm);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //---------------           Get All Attendences    ----------------------------------
        public void get_all_attendence()
        {
            all_attendence_list = new List<student_attendence>();            
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "SELECT* FROM sms_student_attendence where attendence_date= @date && session_id=" + MainWindow.session.id;
                        cmd.Connection = con;
                        cmd.Parameters.Add("@date", MySqlDbType.Date).Value = dt;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            student_attendence att = new student_attendence()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                std_name = Convert.ToString(reader["std_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                section_id = Convert.ToString(reader["section_id"].ToString()),
                                roll_no = Convert.ToString(reader["roll_no"].ToString()),
                                att_percentage = Convert.ToString(reader["att_percentage"].ToString()),
                                total_days = Convert.ToString(reader["total_days"].ToString()),
                                total_abs = Convert.ToString(reader["total_abs"].ToString()),
                                total_presents = Convert.ToString(reader["total_presents"].ToString()),
                                attendence = Convert.ToChar(reader["attendence"]),
                                attendence_date = Convert.ToDateTime(reader["attendence_date"]),
                            };
                            all_attendence_list.Add(att);                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void click_refresh(object sender, RoutedEventArgs e)
        {
            dt_picker.SelectedDate = DateTime.Now;
        }

        private void print_btn_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = CreateSampleDataTable();
            var columnWidths = new List<double>() { 180, 180, 150, 100, 100 };
            var ht = new SMS.PrintHeaderTemplates.StudentAttendanceHeader();
            var headerTemplate = XamlWriter.Save(ht);
            var printControl = PrintControlFactory.Create(dataTable, columnWidths, headerTemplate);
            printControl.ShowPrintPreview();

        }

        private DataTable CreateSampleDataTable()
        {            
            var dataTable = new DataTable();
            
            AddColumn(dataTable, "Class Name", typeof(string));
            AddColumn(dataTable, "Section Name", typeof(string));
            AddColumn(dataTable, "Strength", typeof(string));
            AddColumn(dataTable, "Absents", typeof(string));
            AddColumn(dataTable, "Presents", typeof(string));            

            foreach (AttendanceReportClass arc  in report_list)
            {
                
                var dataRow = dataTable.NewRow();
                dataRow[0] = arc.class_name;
                dataRow[1] = arc.section_name;
                dataRow[2] = arc.strength;
                dataRow[3] = arc.absents;
                dataRow[4] = arc.presents;               

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void AddColumn(DataTable dataTable, string columnName, Type type)
        {
            var dataColumn = new DataColumn(columnName, type);
            dataTable.Columns.Add(dataColumn);
        }

    }
}
