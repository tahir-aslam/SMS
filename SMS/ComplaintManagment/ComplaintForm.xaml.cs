using SMS.Common;
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

namespace SMS.ComplaintManagment
{
    /// <summary>
    /// Interaction logic for ComplaintForm.xaml
    /// </summary>
    public partial class ComplaintForm : Window
    {
        List<admission> adm_list;
        List<sms_complaint_from> complaintFromList;
        List<sms_complaint_type> complaintTypeList;
        List<sms_complaint_status> complaintStatusList;
        AdmissionDAL admissionDAL;
        ComplaintDAL complaintDAL;
        sms_complaint_register complaintRegisterObj;
        ComplaintRegister complaintRegisterSearch;
        string mode = "";

        public ComplaintForm(string mode, ComplaintRegister search, sms_complaint_register obj)
        {
            InitializeComponent();

            admissionDAL = new AdmissionDAL();
            complaintDAL = new ComplaintDAL();
            this.mode = mode;
            this.complaintRegisterObj = obj;
            this.complaintRegisterSearch = search;
            LoadGrid();
        }

        void LoadGrid()
        {
            try {
                v_complaint_from_cmb.ItemsSource = complaintDAL.getAllComplaintFrom();
                v_complaint_type_cmb.ItemsSource = complaintDAL.getAllComplaintTypes();
                v_complaint_status_cmb.ItemsSource = complaintDAL.getAllComplaintStatus();
                v_complaint_date.SelectedDate = DateTime.Now;
                v_complaint_resolved_date.SelectedDate = DateTime.Now;

                v_complaint_from_cmb.SelectedIndex = 0;
                v_complaint_type_cmb.SelectedIndex = 0;
                v_complaint_status_cmb.SelectedIndex = 0;

                if (mode == "insert")
                {
                    std_name_TB.Visibility = Visibility.Collapsed;
                    v_complaint_no_sp.Visibility = Visibility.Collapsed;
                }
                else
                {
                    std_count_SP.Visibility = Visibility.Collapsed;
                    v_complaint_no_sp.Visibility = Visibility.Visible;
                    std_SP.Visibility = Visibility.Collapsed;
                    std_name_TB.Visibility = Visibility.Visible;
                    std_name_TB.Text = complaintRegisterObj.std_name;
                    FillControl(complaintRegisterObj);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void allOption_Checked(object sender, RoutedEventArgs e)
        {
            adm_list = admissionDAL.get_all_admissions();
            selectedStudentCountTB.Text = adm_list.Count.ToString();
        }
        private void selectedOption_Checked(object sender, RoutedEventArgs e)
        {
            StudentSelectionWindowNew ssw = new StudentSelectionWindowNew();
            ssw.ShowDialog();
            adm_list = ssw.adm_list.Where(x => x.Checked == true).ToList();
            selectedStudentCountTB.Text = adm_list.Count.ToString();
        }

        private void v_complaint_status_cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (v_complaint_status_cmb.SelectedItem != null)
            {
                sms_complaint_status obj = v_complaint_status_cmb.SelectedItem as sms_complaint_status;
                if (obj.id > 1)
                {
                    v_complaint_resolved_date_sp.Visibility = Visibility.Visible;
                    v_complaint_resolved_remarks_sp.Visibility = Visibility.Visible;
                }
                else
                {
                    v_complaint_resolved_date_sp.Visibility = Visibility.Collapsed;
                    v_complaint_resolved_remarks_sp.Visibility = Visibility.Collapsed;
                }
            }
        }

        List<sms_complaint_register> FillList()
        {
            List<sms_complaint_register> list = new List<sms_complaint_register>();
            sms_complaint_register obj;
            sms_complaint_from c_from = (sms_complaint_from)v_complaint_from_cmb.SelectedItem;
            sms_complaint_type c_type = (sms_complaint_type)v_complaint_type_cmb.SelectedItem;
            sms_complaint_status c_status = (sms_complaint_status)v_complaint_status_cmb.SelectedItem;

            if (adm_list != null && adm_list.Where(x => x.Checked == true).Count() > 0)
            {
                foreach (var item in adm_list.Where(x => x.Checked == true))
                {
                    obj = new sms_complaint_register();
                    obj.std_id = Convert.ToInt32(item.id);
                    obj.session_id = Convert.ToInt32(MainWindow.session.id);
                    obj.complaint_from_id = c_from.id;
                    obj.complaint_status_id = c_status.id;
                    obj.complaint_type_id = c_type.id;
                    obj.complaint_date = v_complaint_date.SelectedDate.Value;
                    obj.complaint_remarks = v_complaint_remarks.Text;
                    obj.complaint_resolved_date = v_complaint_resolved_date.SelectedDate.Value;
                    obj.complaint_resolved_remarks = v_complaint_resolved_remarks.Text;
                    obj.created_by = MainWindow.emp_login_obj.emp_user_name;
                    obj.created_date_time = DateTime.Now;
                    obj.updated_by = MainWindow.emp_login_obj.emp_user_name;
                    obj.updated_date_time = DateTime.Now;
                    obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.id);

                    list.Add(obj);
                }
            }
            else
            {
                MessageBox.Show("Select Minimum one student");
            }

            return list;
        }
        sms_complaint_register FillObject()
        {
            sms_complaint_register obj;
            sms_complaint_from c_from = (sms_complaint_from)v_complaint_from_cmb.SelectedItem;
            sms_complaint_type c_type = (sms_complaint_type)v_complaint_type_cmb.SelectedItem;
            sms_complaint_status c_status = (sms_complaint_status)v_complaint_status_cmb.SelectedItem;


            obj = new sms_complaint_register();
            obj.id = complaintRegisterObj.id;
            obj.std_id = complaintRegisterObj.std_id;
            obj.session_id = Convert.ToInt32(MainWindow.session.id);
            obj.complaint_from_id = c_from.id;
            obj.complaint_status_id = c_status.id;
            obj.complaint_type_id = c_type.id;
            obj.complaint_date = v_complaint_date.SelectedDate.Value;
            obj.complaint_remarks = v_complaint_remarks.Text;
            obj.complaint_resolved_date = v_complaint_resolved_date.SelectedDate.Value;
            obj.complaint_resolved_remarks = v_complaint_resolved_remarks.Text;
            obj.created_by = MainWindow.emp_login_obj.emp_user_name;
            obj.created_date_time = DateTime.Now;
            obj.updated_by = MainWindow.emp_login_obj.emp_user_name;
            obj.updated_date_time = DateTime.Now;
            obj.emp_id = Convert.ToInt32(MainWindow.emp_login_obj.id);

            return obj;

        }

        void FillControl(sms_complaint_register obj)
        {
            v_complaint_no.Text = obj.id.ToString();
            v_complaint_from_cmb.SelectedValue = obj.complaint_from_id;
            v_complaint_type_cmb.SelectedValue = obj.complaint_type_id;
            v_complaint_status_cmb.SelectedValue = obj.complaint_status_id;
            v_complaint_remarks.Text = obj.complaint_remarks;
            v_complaint_resolved_remarks.Text = obj.complaint_resolved_remarks;
            v_complaint_date.SelectedDate = obj.complaint_date;
            v_complaint_resolved_date.SelectedDate = obj.complaint_resolved_date;
        }

        private void v_save_Click(object sender, RoutedEventArgs e)
        {
            if (mode == "insert")
            {
                List<sms_complaint_register> lst = FillList();
                if (lst.Count > 0)
                {
                    MessageBoxResult mbr = MessageBox.Show("Do You Want To Register This Complain of " + lst.Count + " Student?", "Confirmation", MessageBoxButton.YesNo);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        if (complaintDAL.insertComplaintRegister(lst) > 0)
                        {
                            MessageBox.Show("Registered Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                            complaintRegisterSearch.load_grid();
                        }
                        else
                        {
                            MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Select Minimum One Student!!!!!!!!");
                }
            }
            else if (mode == "edit")
            {
                sms_complaint_register obj = FillObject();
                MessageBoxResult mbr = MessageBox.Show("Do You Want To Update This Complain?", "Confirmation", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    if (complaintDAL.updateComplaintRegister(obj) > 0)
                    {
                        MessageBox.Show("Updated Successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        complaintRegisterSearch.load_grid();
                    }
                    else
                    {
                        MessageBox.Show("OOPs! There's some thing wrong, Please try again");
                    }
                }
            }
        }

        private void v_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
