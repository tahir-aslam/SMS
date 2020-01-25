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
using SMS.DAL;
using SMS.Models;

namespace SMS.ExamsManagement.DateSheet
{
    /// <summary>
    /// Interaction logic for DateSheetWindow.xaml
    /// </summary>
    public partial class DateSheetWindow : Window
    {
        DateSheetPage ShowGridPage;

        List<exam> exam_list;
        List<classes> classes_list;
        List<sections> sections_list;
        List<sms_exams_subjects> subjects_list;
        ExamsDAL examsDAL;

        sms_exams_date_sheet subjects_obj;
        sms_exams_date_sheet obj;
        string mode;

        SubjectsDAL subjectsDAL;
        EmployeesDAL empDAL;
        ClassesDAL classesDAL;

        public DateSheetWindow(string m, DateSheetPage sp, sms_exams_date_sheet ob)
        {
            InitializeComponent();

            ShowGridPage = sp;
            this.obj = ob;
            this.mode = m;

            subjectsDAL = new SubjectsDAL();
            empDAL = new EmployeesDAL();
            classesDAL = new ClassesDAL();
            examsDAL = new ExamsDAL();

            exam_cmb.Focus();
            LoadData();
        }

        void LoadData()
        {
            try
            {
                exam_list = examsDAL.get_all_exams();
                classes_list = classesDAL.getAllClasses();
                subjects_list = subjectsDAL.GetAllSubjects();
                //emp_list = empDAL.get_all_active_employees();
                //emp_types_list = empDAL.get_all_employee_designation();

                exam_cmb.ItemsSource = exam_list;
                exam_list.Insert(0, new exam() { exam_name = "---Select Exam---", id = "-1" });
                exam_cmb.SelectedIndex = 0;

                classes_list.Insert(0, new classes() { class_name = "---Select Class---", id = "-1" });
                v_class_cmb.SelectedIndex = 0;
                v_class_cmb.ItemsSource = classes_list;

                subjects_list.Insert(0, new sms_exams_subjects() { subject_name = "---Select Subjects---", id = -1 });
                v_subject_cmb.SelectedIndex = 0;
                v_subject_cmb.ItemsSource = subjects_list;

                //emp_types_list.Insert(0, new sms_emp_designation() { designation = "---Select Designation---", id = -1 });
                //v_emp_types_cmb.SelectedIndex = 0;
                //v_emp_types_cmb.ItemsSource = emp_types_list;

                if (mode == "edit")
                {
                    exam_cmb.SelectedValue = obj.exam_id;
                    exam_cmb.IsEnabled = false;

                    v_class_cmb.SelectedValue = obj.class_id.ToString();
                    classes_list.Where(x => x.id == obj.class_id.ToString()).FirstOrDefault().IsChecked = true;
                    v_class_cmb.IsEnabled = false;

                    sections_list = classesDAL.get_all_sections(obj.class_id.ToString());                                        
                    v_section_cmb.ItemsSource = sections_list;
                    v_section_cmb.SelectedValue = obj.section_id;
                    sections_list.Where(x => x.id == obj.section_id.ToString()).FirstOrDefault().IsChecked = true;
                    v_section_cmb.IsEnabled = false;

                    v_subject_cmb.SelectedValue = obj.subject_id;                    
                    v_subject_cmb.IsEnabled = true;

                    v_exam_date.SelectedDate = obj.exam_date;
                    v_exam_timing.Text = obj.exam_time;
                    v_exam_remarks.Text = obj.remarks;
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
                        sms_exams_date_sheet obj = FillObjectForUpdate(this.obj);
                        if (examsDAL.UpdateDateSheet(obj) > 0)
                        {
                            MessageBox.Show("Record Updated Successfully");
                            ShowGridPage.load_grid();
                            this.Close();
                        }
                    }
                    else
                    {
                        List<sms_exams_date_sheet> lst = FillObject();
                        if (examsDAL.InsertDateSheet(lst) > 0)
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

        private sms_exams_date_sheet FillObjectForUpdate(sms_exams_date_sheet obj)
        {
            sms_exams_subjects subject = (sms_exams_subjects)v_subject_cmb.SelectedItem;
            obj.subject_id = subject.id;
            obj.exam_date = v_exam_date.SelectedDate.Value;
            obj.exam_time = v_exam_timing.Text;
            obj.remarks = v_exam_remarks.Text;
            obj.updated_date_time = DateTime.Now;
            obj.updated_emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id);
            return obj;
        }
        private List<sms_exams_date_sheet> FillObject()
        {
            List<sms_exams_date_sheet> lst = new List<sms_exams_date_sheet>();
            exam exam = exam_cmb.SelectedItem as exam;
            sms_exams_subjects subject = v_subject_cmb.SelectedItem as sms_exams_subjects;
            sms_exams_date_sheet date_sheet_obj;

            foreach (var cl in classes_list.Where(x => x.id != "-1").Where(x => x.IsChecked == true))
            {
                if (v_section_cmb.IsEnabled)
                {
                    foreach (var sec in sections_list.Where(x => x.id != "-1").Where(x => x.isChecked == true))
                    {
                        date_sheet_obj = new sms_exams_date_sheet()
                        {
                            class_id = Convert.ToInt32(cl.id),
                            section_id = Convert.ToInt32(sec.id),
                            exam_id = Convert.ToInt32(exam.id),
                            subject_id = subject.id,
                            exam_date = v_exam_date.SelectedDate.Value,
                            exam_time = v_exam_timing.Text,
                            remarks = v_exam_remarks.Text,
                        };
                        lst.Add(date_sheet_obj);
                    }
                }
                else
                {
                    sections_list = classesDAL.get_all_sections(cl.id);
                    foreach (var sec in sections_list)
                    {
                        date_sheet_obj = new sms_exams_date_sheet()
                        {
                            class_id = Convert.ToInt32(cl.id),
                            section_id = Convert.ToInt32(sec.id),
                            exam_id = Convert.ToInt32(exam.id),
                            subject_id = subject.id,
                            exam_date = v_exam_date.SelectedDate.Value,
                            exam_time = v_exam_timing.Text,
                            remarks = v_exam_remarks.Text,
                        };
                        lst.Add(date_sheet_obj);
                    }
                }
            }

            lst.ForEach(x => x.created_date_time = DateTime.Now);
            lst.ForEach(x => x.created_emp_id = Convert.ToInt32(MainWindow.emp_login_obj.emp_id));
            
            return lst;
        }
        private void click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }        
        private void CheckBox_Checked_classes(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            classes classes = checkBox.DataContext as classes;
            if (classes.id == "-1")
            {
                if (checkBox.IsChecked == true)
                {
                    classes_list.ForEach(x => x.IsChecked = true);
                }
                else
                {
                    classes_list.ForEach(x => x.IsChecked = false);
                }
            }

            //for section cmb
            if (classes_list.Where(x => x.id != "-1").Where(x => x.IsChecked == true).Count() > 1)
            {
                v_section_cmb.IsEnabled = false;
            }
            else if (classes.id != "-1")
            {
                sections_list = classesDAL.get_all_sections(classes.id);
                v_section_cmb.IsEnabled = true;
                sections_list.Insert(0, new sections() { section_name = "---All Sections---", id = "-1" });
                v_section_cmb.ItemsSource = sections_list;
                v_section_cmb.SelectedIndex = 0;
            }
        }
        private void CheckBox_Checked_Sections(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            sections obj = checkBox.DataContext as sections;
            if (obj.id == "-1")
            {
                if (checkBox.IsChecked == true)
                {
                    sections_list.ForEach(x => x.IsChecked = true);
                }
                else
                {
                    sections_list.ForEach(x => x.IsChecked = false);
                }
            }
        }
        private void CheckBox_Checked_subjects(object sender, RoutedEventArgs e)
        {

        }
        private bool validate()
        {
            if (classes_list.Where(x => x.id != "-1").Where(x => x.IsChecked == true).Count() == 0)
            {
                v_class_cmb.Focus();
                string alertText = "Class Name Should Not Be Blank.";
                MessageBox.Show(alertText);
                return false;
            }
            else if (v_section_cmb.IsEnabled && sections_list.Where(x => x.id != "-1").Where(x => x.isChecked == true).Count() == 0)
            {
                v_section_cmb.Focus();
                string alertText = "Please Select Minimum One Section";
                MessageBox.Show(alertText);
                return false;
            }
            else if (v_subject_cmb.SelectedIndex == -1)
            {
                v_subject_cmb.Focus();
                string alertText = "Please Select Minimum One Subject";
                MessageBox.Show(alertText);
                return false;
            }
            else if (v_exam_date.SelectedDate == null)
            {
                v_exam_date.Focus();
                string alertText = "Please Select Exam Date";
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
