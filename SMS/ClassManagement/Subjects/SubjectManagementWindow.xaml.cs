using SMS.DAL;
using SMS.Models;
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
using System.Windows.Shapes;

namespace SMS.ClassManagement.Subjects
{
    /// <summary>
    /// Interaction logic for SubjectManagementWindow.xaml
    /// </summary>
    public partial class SubjectManagementWindow : Window
    {
        SubjectManagementPage ShowGridPage;

        List<classes> classes_list;
        List<sections> sections_list;
        List<employees> emp_list;
        List<sms_emp_designation> emp_types_list;
        List<sms_exams_subjects> subjects_list;

        sms_exams_subjects subjects_obj;
        sms_exams_subjects obj;
        string mode;

        SubjectsDAL subjectsDAL;
        EmployeesDAL empDAL;
        ClassesDAL classesDAL;

        public SubjectManagementWindow(string m, SubjectManagementPage sp, sms_exams_subjects ob)
        {
            InitializeComponent();

            ShowGridPage = sp;
            this.obj = ob;
            this.mode = m;

            subjectsDAL = new SubjectsDAL();
            empDAL = new EmployeesDAL();
            classesDAL = new ClassesDAL();

            v_class_cmb.Focus();
            LoadData();

        }

        void LoadData()
        {
            try
            {
                classes_list = classesDAL.getAllClasses();
                subjects_list = subjectsDAL.GetAllSubjects();
                emp_list = empDAL.get_all_active_employees();
                emp_types_list = empDAL.get_all_employee_designation();

                classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
                v_class_cmb.SelectedIndex = 0;
                v_class_cmb.ItemsSource = classes_list;

                subjects_list.Insert(0, new sms_exams_subjects() { subject_name = "---Select Subjects---", id = -1 });
                v_subject_cmb.SelectedIndex = 0;
                v_subject_cmb.ItemsSource = subjects_list;

                emp_types_list.Insert(0, new sms_emp_designation() { designation = "---Select Designation---", id = -1 });
                v_emp_types_cmb.SelectedIndex = 0;
                v_emp_types_cmb.ItemsSource = emp_types_list;

                if (mode == "edit")
                {
                    v_class_cmb.SelectedValue =  obj.class_id.ToString();
                    v_class_cmb.IsEnabled = false;

                    v_section_cmb.SelectedValue = obj.section_id;
                    sections_list.Where(x => x.id == obj.section_id.ToString()).FirstOrDefault().isChecked = true;
                    v_section_cmb.IsEnabled = false;

                    v_subject_cmb.SelectedValue = obj.id;
                    subjects_list.Where(x => x.id == obj.id).FirstOrDefault().isChecked = true;
                    v_subject_cmb.IsEnabled = false;

                    v_emp_types_cmb.SelectedValue = obj.emp_designation_id;
                    v_emp_cmb.SelectedValue = obj.emp_id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void click_save(object sender, RoutedEventArgs e)
        {
            try
            {
                if (validate())
                {
                    if (mode == "edit")
                    {
                        sms_exams_subjects obj = FillObjectForUpdate(this.obj);
                        if (subjectsDAL.UpdateSubjectAssignment(obj) > 0)
                        {
                            MessageBox.Show("Record Updated Successfully");
                            ShowGridPage.load_grid();
                            this.Close();
                            
                        }
                    }
                    else
                    {

                        List<sms_exams_subjects> lst = FillObject();
                        if (subjectsDAL.InsertSubjectAssignment(lst) > 0)
                        {
                            MessageBox.Show("Record Added Successfully");
                            ShowGridPage.load_grid();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Not Any Record Inserted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private sms_exams_subjects FillObjectForUpdate(sms_exams_subjects obj)
        {
            employees emp = v_emp_cmb.SelectedItem as employees;
            
            obj.emp_id = Convert.ToInt32(emp.id);
            obj.updated_date_time = DateTime.Now;
            obj.updated_emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
            return obj;
        }
        
        private List<sms_exams_subjects> FillObject()
        {
            List<sms_exams_subjects> lst = new List<sms_exams_subjects>();
            sms_exams_subjects obj;
            employees emp = v_emp_cmb.SelectedItem as employees;

            foreach (var sec in sections_list.Where(x => x.isChecked == true).Where(x => x.id != "-1"))
            {
                foreach (var subj in subjects_list.Where(x => x.isChecked == true).Where(x=>x.id != -1))
                {
                    obj = new sms_exams_subjects()
                    {
                        id = subj.id,
                        section_id = Convert.ToInt32(sec.id),
                        emp_id = Convert.ToInt32(emp.id),
                        created_date_time = DateTime.Now,
                        updated_date_time = DateTime.Now,
                        created_emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id),
                        updated_emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id),

                    };

                    lst.Add(obj);
                }
            }

            return lst;
        }
        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void v_emp_types_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sms_emp_designation et = (sms_emp_designation)v_emp_types_cmb.SelectedItem;
            if (v_emp_types_cmb.SelectedIndex != 0)
            {
                v_emp_cmb.Items.Clear();
                v_emp_cmb.IsEnabled = true;
                v_emp_cmb.SelectedIndex = 0;
                v_emp_cmb.Items.Insert(0, new employees() { emp_name = "---Select Teacher---", id = "-1" });
                foreach (employees emp in emp_list)
                {
                    if (emp.designation_id == et.id)
                    {
                        v_emp_cmb.Items.Add(emp);
                    }
                }

                //emp_cmb.ItemsSource = emp_list.Where(x => x.emp_type_id == et.id);
            }
            else
            {
                v_emp_cmb.SelectedIndex = 0;
                v_emp_cmb.IsEnabled = false;
            }
        }
        private void v_class_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                classes c = (classes)v_class_cmb.SelectedItem;
                string id = c.id;

                if (v_class_cmb.SelectedIndex != 0)
                {
                    sections_list = classesDAL.get_all_sections(id);


                    v_section_cmb.IsEnabled = true;
                    sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                    v_section_cmb.ItemsSource = sections_list;
                    v_section_cmb.SelectedIndex = 0;
                }
                else
                {

                    v_section_cmb.IsEnabled = false;
                    v_section_cmb.SelectedIndex = 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CheckBox_Checked_Sections(object sender, RoutedEventArgs e)
        {
        }
        private void CheckBox_Checked_subjects(object sender, RoutedEventArgs e)
        {

        }
        private bool validate()
        {
            if (v_class_cmb.SelectedIndex == 0)
            {
                v_class_cmb.Focus();
                string alertText = "Class Name Should Not Be Blank.";
                MessageBox.Show(alertText);
                return false;
            }

            else if (sections_list.Where(x => x.isChecked == true).Count() == 0)
            {
                v_section_cmb.Focus();
                string alertText = "Please Select Minimum One Section";
                MessageBox.Show(alertText);
                return false;
            }
            else if (subjects_list.Where(x => x.isChecked == true).Count() == 0)
            {
                v_subject_cmb.Focus();
                string alertText = "Please Select Minimum One Subject";
                MessageBox.Show(alertText);
                return false;
            }
            else if (v_emp_types_cmb.SelectedIndex == 0)
            {
                v_emp_types_cmb.Focus();
                string alertText = "Please Select Teacher Designation";
                MessageBox.Show(alertText);
                return false;
            }
            else if (v_emp_cmb.SelectedIndex == 0)
            {
                v_emp_types_cmb.Focus();
                string alertText = "Please Select Teacher";
                MessageBox.Show(alertText);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
