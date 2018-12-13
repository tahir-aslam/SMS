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
using System.ComponentModel;
using SMS.Models;
using MySql.Data.MySqlClient;
using System.IO;
using System.Net;

namespace SMS.Controls
{
    /// <summary>
    /// Interaction logic for DataUploader.xaml
    /// </summary>
    public partial class DataUploader : UserControl
    {
        private BackgroundWorker bw = new BackgroundWorker();
        

        public List<int> list_deleted;

        public List<admission> adm_list;        
        public List<web_slider_images> slider_images_list;
        public List<web_events> events_list;
        public List<web_event_image> events_images_list;
        public List<web_messages> messages_list;
        public List<classes> classes_list;
        public List<sections> sections_list;
        public List<subjects> subjects_list;
        public List<employees> emp_list;
        institute ins;
        public List<fee> paid_fee_list;
        

        string notification_text;
        string notification_updation;
        DateTime notification_datetime;
        string notification_created_by;

        int j = 0;
        string insertion;
        string updation;
        string table_name = "";
        string records_changes = "";
        int effective_rows = 0;
        int minimum = 0;
        int maximum = 0;
         


        public DataUploader()
        {
            InitializeComponent();
            
            
            get_all_admissions();            
            get_notification();
            get_all_slider_images();
            get_all_events();
            get_all_event_images();
            get_all_messages();
            get_all_classes();
            get_all_sections();
            get_all_subjects();
            get_all_employees();
            get_institute();
            get_all_paid_fee();
            

            maximum = adm_list.Count  + 1 + slider_images_list.Count + events_list.Count + events_images_list.Count + messages_list.Count + classes_list.Count + sections_list.Count + subjects_list.Count + emp_list.Count +1 + paid_fee_list.Count   ;

            uploader_btn.Visibility = Visibility.Visible;
            progressbar.Minimum = 0;
            progressbar.Maximum = maximum ;
            uploader_content_total_textblock.Text =Convert.ToString(maximum);

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        private void uploader_btn_Click(object sender, RoutedEventArgs e)
        {
            
            if (bw.IsBusy != true)
            {
                uploader_btn.Visibility = Visibility.Hidden;
                cancel_btn.Visibility = Visibility.Visible;
                this.status_textblock.Text = "";
                bw.RunWorkerAsync();
               
            }
        }
        private void finsish_btn_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
                
            }
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //============       Admission      ==========================================
            #region Admission
            for (int i = 0; i < adm_list.Count ; i++)
                {
                    j++;
                    admission adm = adm_list[i];

                    if ((worker.CancellationPending == true))
                    {
                        e.Cancel = true;
                        j = 0;
                        effective_rows = 0;
                        worker.ReportProgress(((j * 100) / maximum));
                        break;
                    }
                    else
                    {

                        // Perform a time consuming operation and report progress.
                        //System.Threading.Thread.Sleep(500);

                        // ----- insertion = true , updation =false ----  OR  ----- insertion = true , updation = true ---- 

                        if( (adm.insertion == "true" && adm.updation == "false") || (adm.insertion == "true" && adm.updation == "true") )
                        {
                            insertion = "false";
                            updation = "false";

                            if (insert_into_online_db(adm) > 0) 
                            {
                                insertion = "false";
                                updation = "false";

                                if (update_local_db(adm) > 0) 
                                {
                                    records_changes = "Performing Insertion On Admissions";
                                    effective_rows++;                                    
                                }
                            }
                        }

                        // ----- insertion = false , updation = true ---- 

                        else if (adm.insertion == "false" && adm.updation == "true")
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_into_online_db(adm) > 0)
                            {
                                insertion = "false";
                                updation = "false";

                                if (update_local_db(adm) > 0)
                                {
                                    records_changes = "Performing Updation On Admissions";
                                    effective_rows++;
                                }
                            }
                        }
                        else 
                        {
                            
                        }
                        
                        worker.ReportProgress(((j * 100) /maximum),effective_rows);
                        
                    }
                } // end for

                //System.Threading.Thread.Sleep(500);
                // Delete functionality

                table_name = "sms_admission_deleted";
                get_all_deleted(table_name);
                maximum = maximum + list_deleted.Count;
                
                if (list_deleted.Count > 0) 
                {

                    table_name = "sms_admission";
                    if (delete_from_online_db(list_deleted,table_name) > 0) 
                    {
                        table_name = "sms_admission_deleted";
                        if(delete_from_local_db(table_name) > 0)
                        {
                            records_changes = "Performing Deletion On " + table_name;
                            j = j + list_deleted.Count;
                            effective_rows = effective_rows + list_deleted.Count;
                            worker.ReportProgress(((j * 100) / maximum), effective_rows);
                        }
                    }

                }
                worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion

                // =============         Web            ===================================
               
                //System.Threading.Thread.Sleep(500);
                #region Notification
                if (notification_updation == "true")
                {
                    insertion = "false";
                    updation = "false";

                    if (update_online_db_notification() > 0)
                    {
                        insertion = "false";
                        updation = "false";

                        if (update_local_db_notification() > 0)
                        {
                            records_changes = "Performing Updation On Notification";
                            effective_rows++;
                        }
                    }
                }
            
            records_changes = "Notification Completed";            
            j++;
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
                #endregion
                
            #region Slider Image
            //-------------------    Slider Images        -------------
            for (int i = 0; i < slider_images_list.Count; i++ )
            {
                j++;
                web_slider_images wsi = slider_images_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                   // System.Threading.Thread.Sleep(500);

                    if(wsi.insertion == "true")
                    {
                        insertion = "false";
                        updation = "false";
                        if(insert_online_db_slider(wsi) > 0)
                        {
                            insertion = "false";
                            if(update_local_db_slider(wsi) > 0)
                            {
                                records_changes = "Performing Insertion on Slider Images";
                                effective_rows++;
                            }
                        }
                    }

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

            //System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "web_slider_images_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "web_slider_images";
                if (delete_from_online_db(list_deleted, table_name) > 0)
                {
                    table_name = "web_slider_images_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion 

            #region Events
            //-------------------    Events        -------------
            for (int i = 0; i < events_list.Count; i++)
            {
                j++;
                web_events w_e = events_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    // ----- insertion = true , updation =false ----  OR  ----- insertion = true , updation = true ---- 

                    if ((w_e.insertion == "true" && w_e.updation == "false") || (w_e.insertion == "true" && w_e.updation == "true"))
                    {
                        insertion = "false";
                        updation = "false";

                        if (insert_online_db_events(w_e) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_events(w_e) > 0)
                            {
                                records_changes = "Performing Insertion On Events";
                                effective_rows++;
                            }
                        }
                    }

                    // ----- insertion = false , updation = true ---- 

                    else if (w_e.insertion == "false" && w_e.updation == "true")
                    {
                        insertion = "false";
                        updation = "false";

                        if (update_online_db_events(w_e) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_events(w_e) > 0)
                            {
                                records_changes = "Performing Updation On Events";
                                effective_rows++;
                            }
                        }
                    }
                    else
                    {

                    }

                    worker.ReportProgress(((j * 100) / maximum), effective_rows);

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

            //System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "web_events_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "web_events";
                if (delete_from_online_db(list_deleted, table_name) > 0)
                {
                    table_name = "web_events_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion 

            #region Events Images
            //-------------------    Events Images        -------------
            for (int i = 0; i < events_images_list.Count; i++)
            {
                j++;
                web_event_image w_e_i = events_images_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    if (w_e_i.insertion == "true")
                    {
                        insertion = "false";
                        updation = "false";
                        if (insert_online_db_events_images(w_e_i) > 0)
                        {
                            insertion = "false";
                            if (update_local_db_events_images(w_e_i) > 0)
                            {
                                records_changes = "Performing Insertion on Events Images";
                                effective_rows++;
                            }
                        }
                    }

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

            //System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "web_events_images_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "web_events_images";
                if (delete_from_online_db_events_images(list_deleted, table_name) > 0)
                {
                    table_name = "web_events_images_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion 

            #region Messages
            //-------------------    Messages        -------------
            for (int i = 0; i < messages_list.Count; i++)
            {
                j++;
                web_messages wm = messages_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    // ----- insertion = true , updation =false ----  OR  ----- insertion = true , updation = true ---- 

                    if ((wm.insertion == "true" && wm.updation == "false") || (wm.insertion == "true" && wm.updation == "true"))
                    {
                        insertion = "false";
                        updation = "false";

                        if (insert_online_db_messages(wm) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_messages(wm) > 0)
                            {
                                records_changes = "Performing Insertion On NEws And Messages";
                                effective_rows++;
                            }
                        }
                    }

                    // ----- insertion = false , updation = true ---- 

                    else if (wm.insertion == "false" && wm.updation == "true")
                    {
                        insertion = "false";
                        updation = "false";

                        if (update_online_db_messages(wm) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_messages(wm) > 0)
                            {
                                records_changes = "Performing Updation On News And Messages";
                                effective_rows++;
                            }
                        }
                    }
                    else
                    {

                    }

                    worker.ReportProgress(((j * 100) / maximum), effective_rows);

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

            //System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "web_messages_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "web_messages";
                if (delete_from_online_db(list_deleted, table_name) > 0)
                {
                    table_name = "web_messages_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion             

            //==============    Class Management        =======================================================
            #region Classes
            //-------------------    Classes        -------------
            for (int i = 0; i < classes_list.Count; i++)
            {
                j++;
                classes c = classes_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    // ----- insertion = true , updation =false ----  OR  ----- insertion = true , updation = true ---- 

                    if ((c.insertion == "true" && c.updation == "false") || (c.insertion == "true" && c.updation == "true"))
                    {
                        insertion = "false";
                        updation = "false";

                        if (insert_online_db_classes(c) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_classes(c) > 0)
                            {
                                records_changes = "Performing Insertion On Classes";
                                effective_rows++;
                            }
                        }
                    }

                    // ----- insertion = false , updation = true ---- 

                    else if (c.insertion == "false" && c.updation == "true")
                    {
                        insertion = "false";
                        updation = "false";

                        if (update_online_db_classes(c) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_classes(c) > 0)
                            {
                                records_changes = "Performing Updation On Classes";
                                effective_rows++;
                            }
                        }
                    }
                    else
                    {

                    }

                    worker.ReportProgress(((j * 100) / maximum), effective_rows);

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

            //System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "sms_classes_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "sms_classes";
                if (delete_from_online_db(list_deleted, table_name) > 0)
                {
                    table_name = "sms_classes_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion 

            #region Sections
            //-------------------    Sections        -------------
            for (int i = 0; i < sections_list.Count; i++)
            {
                j++;
                sections s = sections_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    // ----- insertion = true , updation =false ----  OR  ----- insertion = true , updation = true ---- 

                    if ((s.insertion == "true" && s.updation == "false") || (s.insertion == "true" && s.updation == "true"))
                    {
                        insertion = "false";
                        updation = "false";

                        if (insert_online_db_sections(s) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_sections(s) > 0)
                            {
                                records_changes = "Performing Insertion On Sections";
                                effective_rows++;
                            }
                        }
                    }

                    // ----- insertion = false , updation = true ---- 

                    else if (s.insertion == "false" && s.updation == "true")
                    {
                        insertion = "false";
                        updation = "false";

                        if (update_online_db_sections(s) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_sections(s) > 0)
                            {
                                records_changes = "Performing Updation On Sections";
                                effective_rows++;
                            }
                        }
                    }
                    else
                    {

                    }

                    worker.ReportProgress(((j * 100) / maximum), effective_rows);

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

           // System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "sms_subjects_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "sms_subjects";
                if (delete_from_online_db(list_deleted, table_name) > 0)
                {
                    table_name = "sms_subjects_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion 

            #region Subjects
            //-------------------    Subjects        -------------
            for (int i = 0; i < subjects_list.Count; i++)
            {
                j++;
                subjects s = subjects_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    // ----- insertion = true , updation =false ----  OR  ----- insertion = true , updation = true ---- 

                    if ((s.insertion == "true" && s.updation == "false") || (s.insertion == "true" && s.updation == "true"))
                    {
                        insertion = "false";
                        updation = "false";

                        if (insert_online_db_subjects(s) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_subjects(s) > 0)
                            {
                                records_changes = "Performing Insertion On subjects";
                                effective_rows++;
                            }
                        }
                    }

                    // ----- insertion = false , updation = true ---- 

                    else if (s.insertion == "false" && s.updation == "true")
                    {
                        insertion = "false";
                        updation = "false";

                        if (update_online_db_subjects(s) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_subjects(s) > 0)
                            {
                                records_changes = "Performing Updation On subjects";
                                effective_rows++;
                            }
                        }
                    }
                    else
                    {

                    }

                    worker.ReportProgress(((j * 100) / maximum), effective_rows);

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

           // System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "sms_subject_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "sms_subject";
                if (delete_from_online_db(list_deleted, table_name) > 0)
                {
                    table_name = "sms_subject_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion 

            //==============   Employee Management      =======================================================

            #region Employees
            //-------------------    Employees        -------------
            for (int i = 0; i < emp_list.Count; i++)
            {
                j++;
                employees s = emp_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    // ----- insertion = true , updation =false ----  OR  ----- insertion = true , updation = true ---- 

                    if ((s.insertion == "true" && s.updation == "false") || (s.insertion == "true" && s.updation == "true"))
                    {
                        insertion = "false";
                        updation = "false";

                        if (insert_online_db_emp(s) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_emp(s) > 0)
                            {
                                records_changes = "Performing Insertion On Employees";
                                effective_rows++;
                            }
                        }
                    }

                    // ----- insertion = false , updation = true ---- 

                    else if (s.insertion == "false" && s.updation == "true")
                    {
                        insertion = "false";
                        updation = "false";

                        if (update_online_db_emp(s) > 0)
                        {
                            insertion = "false";
                            updation = "false";

                            if (update_local_db_emp(s) > 0)
                            {
                                records_changes = "Performing Updation On Employees";
                                effective_rows++;
                            }
                        }
                    }
                    else
                    {

                    }

                    worker.ReportProgress(((j * 100) / maximum), effective_rows);

                }
                worker.ReportProgress(((j * 100) / maximum));
            }

            // System.Threading.Thread.Sleep(500);
            // Delete functionality

            table_name = "sms_emp_deleted";
            get_all_deleted(table_name);
            maximum = maximum + list_deleted.Count;

            if (list_deleted.Count > 0)
            {

                table_name = "sms_emp";
                if (delete_from_online_db(list_deleted, table_name) > 0)
                {
                    table_name = "sms_emp_deleted";
                    if (delete_from_local_db(table_name) > 0)
                    {
                        records_changes = "Performing Deletion On " + table_name;
                        j = j + list_deleted.Count;
                        effective_rows = effective_rows + list_deleted.Count;
                        worker.ReportProgress(((j * 100) / maximum), effective_rows);
                    }
                }

            }
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion 


            //===========      Sms Intitute             =======================================================

            #region institue
            if (ins.insertion == "true")
            {
                insertion = "false";
                updation = "false";

                if (update_online_db_institute() > 0)
                {
                    insertion = "false";
                    updation = "false";

                    if (update_local_db_institute() > 0)
                    {
                        records_changes = "Performing Updation On Institute";
                        effective_rows++;
                    }
                }
            }

            records_changes = "Completed";
            j++;
            worker.ReportProgress(((j * 100) / maximum), effective_rows);
            #endregion

            // ==========      Paid Fee                =======================================================

            #region Paid Fee
            //-------------------    Events Images        -------------
            for (int i = 0; i < paid_fee_list.Count; i++)
            {
                j++;
                fee w_e_i = paid_fee_list[i];
                if ((worker.CancellationPending == true))
                {
                    e.Cancel = true;
                    j = 0;
                    effective_rows = 0;
                    records_changes = "Cancelled";
                    worker.ReportProgress(((j * 100) / maximum));
                    break;
                }
                else
                {
                    //System.Threading.Thread.Sleep(500);

                    if (w_e_i.insertion == "true")
                    {
                        insertion = "false";
                        updation = "false";
                        if (insert_online_db_paid_fee(w_e_i) > 0)
                        {
                            insertion = "false";
                            updation = "false";
                            if (update_local_db_paid_fee(w_e_i) > 0)
                            {
                                records_changes = "Performing Insertion on Paid Fee";
                                effective_rows++;
                            }
                        }
                    }

                }
                worker.ReportProgress(((j * 100) / maximum));
            }
            #endregion
        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                cancel_btn.Visibility = Visibility.Hidden;
                uploader_btn.Visibility = Visibility.Visible;               

                this.status_textblock.Text = "Canceled!";
               
            }

            else if (!(e.Error == null))
            {
                cancel_btn.Visibility = Visibility.Hidden;
                uploader_btn.Visibility = Visibility.Visible;
                this.status_textblock.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                uploader_btn.Visibility = Visibility.Hidden;
                cancel_btn.Visibility = Visibility.Hidden;
                finsish_btn.Visibility = Visibility.Visible;
                this.status_textblock.Text = "  Successfully Updated!";
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressbar.Maximum = maximum;
            uploader_content_total_textblock.Text = Convert.ToString(maximum);
            this.progressbar_textblock.Text = (e.ProgressPercentage.ToString() + "%");
            this.progressbar.Value = j;
            this.uploader_content_textblock.Text = j.ToString();
            this.effective_rows_tb.Text = effective_rows.ToString()+"  Effective Row(s)";
            this.records_chnges_tb.Text = records_changes;

        }

        //===============     Get All  Deleted    ================

        public void get_all_deleted(string table)
        {
            list_deleted = new List<int>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM "+table;
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        list_deleted.Add(Convert.ToInt32(reader["id"]));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        //===============     Delete from Local  Db             =================

        public int delete_from_local_db(string table)
        {
            int i = 0;
            try
            {

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Delete from "+table;
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        try
                        {
                            con.Open();
                            i = Convert.ToInt32(cmd.ExecuteNonQuery());
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //===============     Delete from Online  Db             =================

        public int delete_from_online_db(List<int> list_del,string table)
        {           
            int i = 0;
            try
            {
                foreach(int id in list_del)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Delete from "+table+" where id =" + id;
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                            try
                            {
                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

       
        //===============     Delete from Online  Db  Events Images           =================

        public int delete_from_online_db_events_images(List<int> list_del, string table)
        {
            int i = 0;
            try
            {
                foreach (int id in list_del)
                {
                    using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.CommandText = "Delete from " + table + " where image_id =" + id;
                            cmd.Connection = con;
                            //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                            try
                            {
                                con.Open();
                                i = Convert.ToInt32(cmd.ExecuteNonQuery());
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //===============     Update Online  Db             =================

        public int update_into_online_db(admission adm_obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_admission SET class_id=@class_id,std_name=@std_name,b_form=@b_form,section_name=@section_name,transport=@transport,stationary_fee=@stationary_fee,class_name=@class_name,section_id=@section_id,comm_adress=@comm_adress,roll_no=@roll_no,adm_no=@adm_no,tution_fee=@tution_fee,total=@total,scholarship=@scholarship,misc_charges=@misc_charges,exam_fee=@exam_fee,security_fee=@security_fee,other_exp=@other_exp,transport_fee=@transport_fee,parmanent_adress=@parmanent_adress,adm_date=@adm_date,cell_no=@cell_no,emergency_address=@emergency_address,previous_school=@previous_school,boarding=@boarding,father_name=@father_name,father_cnic=@father_cnic,father_income=@father_income,religion=@religion,dob=@dob,reg_fee=@reg_fee,adm_fee=@adm_fee,is_active=@is_active,created_by=@created_by,date_time=@date_time,image=@image,insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_id;
                        cmd.Parameters.Add("@std_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.std_name;
                        cmd.Parameters.Add("@father_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_name;
                        cmd.Parameters.Add("@father_cnic", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_cnic;
                        cmd.Parameters.Add("@father_income", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_income;
                        cmd.Parameters.Add("@religion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.religion;
                        cmd.Parameters.Add("@dob", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.dob;
                        cmd.Parameters.Add("@b_form", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.b_form;
                        cmd.Parameters.Add("@parmanent_adress", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.parmanent_adress;
                        cmd.Parameters.Add("@adm_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = adm_obj.adm_date;
                        cmd.Parameters.Add("@cell_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.cell_no;
                        cmd.Parameters.Add("@emergency_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.emergency_address;
                        cmd.Parameters.Add("@previous_school", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.previous_school;
                        cmd.Parameters.Add("@boarding", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.boarding;
                        cmd.Parameters.Add("@transport", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.transport;
                        cmd.Parameters.Add("@comm_adress", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.comm_adress;
                        cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.roll_no;
                        cmd.Parameters.Add("@adm_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_no;
                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                        cmd.Parameters.Add("@scholarship", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.scholarship;
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.misc_charges;
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;
                        cmd.Parameters.Add("@transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.transport_fee;
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                        cmd.Parameters.Add("@stationary_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.stationary_fee;
                        cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.total;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_name;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_id;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_name;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.date_time;                        
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = adm_obj.image;

                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "false";
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "false";

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        //===============     Update Local Db             =================

        public int update_local_db(admission adm)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_admission SET insertion=@insertion,updation=@updation where id =" + adm.id;
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        // ===============     insert_into_online_db(adm); ================

        public int insert_into_online_db(admission adm_obj)
        {
            int i = 0; 
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_admission(id,class_id,std_name,father_name,father_cnic,father_income,religion,dob,b_form,parmanent_adress,adm_date,cell_no,emergency_address,previous_school,boarding,transport,comm_adress,roll_no,adm_no,reg_fee,adm_fee,tution_fee,scholarship,misc_charges,exam_fee,security_fee,transport_fee,other_exp,stationary_fee,total,class_name,section_id,section_name,image,is_active,created_by,date_time,insertion,updation) Values(@id,@class_id,@std_name,@father_name,@father_cnic,@father_income,@religion,@dob,@b_form,@parmanent_adress,@adm_date,@cell_no,@emergency_address,@previous_school,@boarding,@transport,@comm_adress,@roll_no,@adm_no,@reg_fee,@adm_fee,@tution_fee,@scholarship,@misc_charges,@exam_fee,@security_fee,@transport_fee,@other_exp,@stationary_fee,@total,@class_name,@section_id,@section_name,@image,@is_active,@created_by,@date_time,@insertion,@updation)";
                       // cmd.CommandTimeout = 0; 
                        cmd.Connection = con;
                        
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.class_name;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_id;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.section_name;

                        cmd.Parameters.Add("@std_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.std_name;
                        cmd.Parameters.Add("@father_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_name;
                        cmd.Parameters.Add("@father_cnic", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_cnic;
                        cmd.Parameters.Add("@father_income", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.father_income;
                        cmd.Parameters.Add("@religion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.religion;
                        cmd.Parameters.Add("@dob", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.dob;
                        cmd.Parameters.Add("@b_form", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.b_form;
                        cmd.Parameters.Add("@parmanent_adress", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.parmanent_adress;

                        cmd.Parameters.Add("@adm_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = adm_obj.adm_date;
                        cmd.Parameters.Add("@cell_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.cell_no;
                        cmd.Parameters.Add("@emergency_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.emergency_address;
                        cmd.Parameters.Add("@previous_school", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.previous_school;
                        cmd.Parameters.Add("@boarding", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.boarding;
                        cmd.Parameters.Add("@transport", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.transport;
                        cmd.Parameters.Add("@comm_adress", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.comm_adress;

                        cmd.Parameters.Add("@roll_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.roll_no;
                        cmd.Parameters.Add("@adm_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_no;
                        cmd.Parameters.Add("@total", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.total;

                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.adm_fee;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.tution_fee;
                        cmd.Parameters.Add("@scholarship", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.scholarship;
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.misc_charges;
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.exam_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.security_fee;
                        cmd.Parameters.Add("@transport_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.transport_fee;
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.other_exp;
                        cmd.Parameters.Add("@stationary_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.stationary_fee;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = adm_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = adm_obj.date_time;
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = adm_obj.image;

                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        // ===============     Get All Admissions          ================

        public void get_all_admissions()
        {
            adm_list = new List<admission>();

            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "SELECT* FROM sms_admission where is_active='Y'";
                cmd.Connection = con;
                //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                try
                {
                    con.Open();
                    Byte[] img;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader["image"] == "")
                        {
                            string path = "/SMS;component/images/Delete-icon.png";
                            img = File.ReadAllBytes(path);
                        }
                        else
                        {
                            img = (byte[])(reader["image"]);
                        }

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
                            insertion = Convert.ToString(reader["insertion"].ToString()),
                            updation = Convert.ToString(reader["updation"].ToString()),
                            image = img,
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

       
        // ====================================================  Notifications ========================================

        // ---------      Get  notification   ---------------
        public void get_notification()
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_notification";
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();

                        notification_text = Convert.ToString(reader["notification"].ToString());
                        notification_updation = Convert.ToString(reader["updation"].ToString());
                        notification_datetime = Convert.ToDateTime(reader["date_time"]);
                        notification_created_by = Convert.ToString(reader["created_by"].ToString());
                        
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ---------      Update Online Db    ---------------
        public int update_online_db_notification()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_notification SET notification=@notification,created_by=@created_by,date_time=@date_time,updation=@updation ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@notification", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = notification_text;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = notification_created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = notification_datetime;

                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------       Update Local Db     ---------------
        public int update_local_db_notification()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_notification SET updation=@updation ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //======================================================  Slider          =====================================

        //  ----------------------     Get All Images           -----------------------------------
        public void get_all_slider_images()
        {
            try
            {
                slider_images_list = new List<web_slider_images>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_slider_images";
                        con.Open();

                        Byte[] img;

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["image"] == "")
                            {
                                string path = "/SMS;component/images/Delete-icon.png";
                                img = File.ReadAllBytes(path);
                            }
                            else
                            {
                                img = (byte[])(reader["image"]);
                            }

                            web_slider_images web = new web_slider_images()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                created_by= Convert.ToString(reader["created_by"].ToString()),
                                date_time =Convert.ToDateTime(reader["date_time"]),
                                image = img,
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                            };
                            slider_images_list.Add(web);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ----------------------     Insert into online db Slider Images ------------------------
        public int insert_online_db_slider(web_slider_images wsi)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into web_slider_images (id,image , created_by , date_time, insertion, updation) values (@id,@image , @created_by , @date_time , @insertion , @updation)";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = wsi.id;
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = wsi.image;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = wsi.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = wsi.date_time;
                        cmd.Parameters.Add("@insertion",MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //-----------------------      Update Local Db Slider Images      ------------------------
        public int update_local_db_slider(web_slider_images wsi)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_slider_images SET insertion=@insertion where id ="+wsi.id;
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        //====================================================   Events ====================================

        //---------------           Insert Online Db Events   ----------------------------------

        public int insert_online_db_events(web_events event_obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO web_events(id,event_date,event_name,event_description,created_by,date_time,insertion,updation) Values(@id,@event_date,@event_name,@event_description,@created_by,@date_time,@insertion,@updation)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.id;
                        cmd.Parameters.Add("@event_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = event_obj.event_date;
                        cmd.Parameters.Add("@event_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_name;
                        cmd.Parameters.Add("@event_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_description;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = event_obj.date_time;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;



                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Online Db Events      ---------------------------------

        public int update_online_db_events(web_events event_obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_events SET event_date=@event_date,event_name=@event_name,event_description=@event_description,created_by=@created_by,date_time=@date_time,insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.id;
                        cmd.Parameters.Add("@event_date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = event_obj.event_date;
                        cmd.Parameters.Add("@event_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_name;
                        cmd.Parameters.Add("@event_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.event_description;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = event_obj.date_time;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Sections DB no Connected");
            }
            return i;
        }

        //---------------           Update Local Db Events      ---------------------------------

        public int update_local_db_events(web_events event_obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_events SET insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = event_obj.id;                        
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }
        //  ----------------------     Get All Events            -----------------------------------
        public void get_all_events()
        {
            try
            {
                events_list = new List<web_events>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_events";
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            web_events w_e = new web_events()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                event_date = Convert.ToDateTime(reader["event_date"]),
                                event_name = Convert.ToString(reader["event_name"].ToString()),
                                event_description = Convert.ToString(reader["event_description"].ToString()),
                                date_time = Convert.ToDateTime(reader["event_date"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),

                            };
                            events_list.Add(w_e);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //====================================================   Events images ====================================

        //  ----------------------     Get All Event Images           -----------------------------------
        public void get_all_event_images()
        {
            try
            {
                events_images_list = new List<web_event_image>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_events_images";
                        con.Open();

                        Byte[] img;

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["image"] == "")
                            {
                                string path = "/SMS;component/images/Delete-icon.png";
                                img = File.ReadAllBytes(path);
                            }
                            else
                            {
                                img = (byte[])(reader["image"]);
                            }

                            web_event_image web = new web_event_image()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                image = img,
                                image_id = Convert.ToString(reader["image_id"].ToString()),
                                insertion= Convert.ToString(reader["insertion"].ToString()),
                            };
                            events_images_list.Add(web);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ----------------------     Insert into online db events Images ------------------------

        public int insert_online_db_events_images(web_event_image w_e_i)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "insert into web_events_images (id,image,image_id,insertion) values (@id , @image , @image_id ,@insertion )";
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = w_e_i.id;
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = w_e_i.image;
                        cmd.Parameters.Add("@image_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = w_e_i.image_id;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;


                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //-----------------------      Update Local Db Events Images      ------------------------
        public int update_local_db_events_images(web_event_image w_e_i)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_events_images SET insertion=@insertion WHERE image_id = @image_id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@image_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = w_e_i.image_id;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //====================================================   NEws And Messages ====================================

        //  ----------------------     Get All NEws and messages           -----------------------------------
        public void get_all_messages()
        {
            try
            {
                messages_list = new List<web_messages>();

                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from web_messages ORDER BY date_time DESC";
                        con.Open();

                        Byte[] img;

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (reader["image"] == "")
                            {
                                string path = "/SMS;component/images/Delete-icon.png";
                                img = File.ReadAllBytes(path);
                            }
                            else
                            {
                                img = (byte[])(reader["image"]);
                            }

                            web_messages wm = new web_messages()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                message_heading = Convert.ToString(reader["message_heading"]),
                                message_description = Convert.ToString(reader["message_description"]),
                                created_by = Convert.ToString(reader["created_by"]),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                insertion = Convert.ToString(reader["insertion"]),
                                updation = Convert.ToString(reader["updation"]),
                                 
                                 
                                image = img,
                            };
                            messages_list.Add(wm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ----------------------     Insert online db Messages ------------------------

        public int insert_online_db_messages(web_messages message_obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO web_messages(id,message_heading,message_description,image,created_by,date_time,insertion,updation) Values(@id,@message_heading,@message_description,@image,@created_by,@date_time,@insertion,@updation)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.id;
                        cmd.Parameters.Add("@message_heading", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_heading;
                        cmd.Parameters.Add("@message_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_description;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = message_obj.date_time;
                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = message_obj.image;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.updation;
                        

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                        // Get the object used to communicate with the server.
                        //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.scenariomanagementsolution.com/1.jpeg");
                        //request.Method = WebRequestMethods.Ftp.UploadFile;

                        //// This example assumes the FTP site uses anonymous logon.
                        //request.Credentials = new NetworkCredential("providence@scenariomanagementsolution.com", "7507674");

                        //// Copy the contents of the file to the request stream.
                        
                        //StreamReader sourceStream = new StreamReader(@"C:/WebcamSnapshots/1.Jpeg");
                        //byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                        //sourceStream.Close();
                        //request.ContentLength = fileContents.Length;

                        //Stream requestStream = request.GetRequestStream();
                        //requestStream.Write(fileContents, 0, fileContents.Length);
                        //requestStream.Close();

                        //FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                        ////Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
                        //MessageBox.Show("Upload File Complete");
                        //response.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Online Db Messages      ---------------------------------
        public int update_online_db_messages(web_messages message_obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_messages SET message_heading=@message_heading,message_description=@message_description,created_by=@created_by,date_time=@date_time,image=@image,insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.id;
                        cmd.Parameters.Add("@message_heading", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_heading;
                        cmd.Parameters.Add("@message_description", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.message_description;

                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = message_obj.date_time;

                        cmd.Parameters.Add("@image", MySql.Data.MySqlClient.MySqlDbType.Blob).Value = message_obj.image;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }
        //---------------           Update Local Db Messages      ---------------------------------
        public int update_local_db_messages(web_messages message_obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update web_messages SET insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = message_obj.id;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        // ================================================    Classes =======================================

        //  ----------------------     Get All classes           -----------------------------------
        public void get_all_classes()
        {
            classes_list = new List<classes>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {


                        //cmd.CommandText = "GetAllRoles";
                        cmd.CommandText = "SELECT* FROM sms_classes";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            classes classes = new classes()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                reg_fee = Convert.ToString(reader["reg_fee"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee"].ToString()),
                                transport_charges = Convert.ToString(reader["transport_charges"].ToString()),
                                boarding_fee = Convert.ToString(reader["boarding_fee"].ToString()),
                                misc_charges = Convert.ToString(reader["misc_charges"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee"].ToString()),
                                stationary_charges = Convert.ToString(reader["stationary_charges"].ToString()),
                                books_charges = Convert.ToString(reader["books_charges"].ToString()),
                                other_exp = Convert.ToString(reader["other_exp"].ToString()),
                                roll_no_format = Convert.ToString(reader["roll_no_format"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),


                            };
                            classes_list.Add(classes);

                        }


                    }
                }
            }
            catch
            {
                MessageBox.Show("Classes DB not connected");
            }
        }


        // ----------------------     Insert online db Classes ------------------------
        public int insert_online_db_classes(classes class_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_classes(id,class_name,reg_fee,adm_fee,security_fee,tution_fee,transport_charges,boarding_fee,misc_charges,exam_fee,stationary_charges,books_charges,other_exp,roll_no_format,is_active,date_time,created_by,insertion,updation) Values(@id,@class_name,@reg_fee,@adm_fee,@security_fee,@tution_fee,@transport_charges,@boarding_fee,@misc_charges,@exam_fee,@stationary_charges,@books_charges,@other_exp,@roll_no_format,@is_active,@date_time,@created_by,@insertion,@updation)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.class_name;
                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.adm_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.security_fee;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.tution_fee;
                        cmd.Parameters.Add("@transport_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.transport_charges;
                        cmd.Parameters.Add("@boarding_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.boarding_fee;
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.misc_charges;
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.exam_fee;
                        cmd.Parameters.Add("@stationary_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.stationary_charges;
                        cmd.Parameters.Add("@books_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.books_charges;
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.other_exp;
                        cmd.Parameters.Add("@roll_no_format", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.roll_no_format;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.is_active;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = class_obj.date_time;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.created_by;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Online Db Classes      ---------------------------------
        public int update_online_db_classes(classes class_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_classes SET reg_fee=@reg_fee,adm_fee=@adm_fee,security_fee=@security_fee,tution_fee=@tution_fee,transport_charges=@transport_charges,boarding_fee=@boarding_fee,misc_charges=@misc_charges,exam_fee=@exam_fee,stationary_charges=@stationary_charges,books_charges=@books_charges,other_exp=@other_exp,roll_no_format=@roll_no_format,is_active=@is_active,date_time=@date_time,created_by=@created_by,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.id;
                        cmd.Parameters.Add("@reg_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.reg_fee;
                        cmd.Parameters.Add("@adm_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.adm_fee;
                        cmd.Parameters.Add("@security_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.security_fee;
                        cmd.Parameters.Add("@tution_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.tution_fee;
                        cmd.Parameters.Add("@transport_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.transport_charges;
                        cmd.Parameters.Add("@boarding_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.boarding_fee;
                        cmd.Parameters.Add("@misc_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.misc_charges;
                        cmd.Parameters.Add("@exam_fee", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.exam_fee;
                        cmd.Parameters.Add("@stationary_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.stationary_charges;
                        cmd.Parameters.Add("@books_charges", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.books_charges;
                        cmd.Parameters.Add("@other_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.other_exp;
                        cmd.Parameters.Add("@roll_no_format", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.roll_no_format;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.is_active;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = class_obj.date_time;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = class_obj.created_by;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "false";

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Local Db Classes      ---------------------------------
        public int update_local_db_classes(classes c) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_classes SET updation=@updation,insertion=@insertion WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = c.id;                        
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        // ================================================    Sections =======================================

        //  ----------------------     Get All Sections           -----------------------------------
        public void get_all_sections()
        {

            sections_list = new List<sections>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {


                    //cmd.CommandText = "GetAllRoles";
                    cmd.CommandText = "SELECT* FROM sms_subjects";
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
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),


                            };
                            sections_list.Add(section);

                        }
                        con.Close();
                    }
                    catch
                    {
                        MessageBox.Show("DB not connected");
                    }

                }
            }
        }

        // ----------------------     Insert online db Sections ------------------------
        public int insert_online_db_sections(sections sections_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_subjects(id,class_id,class_name,emp_id,emp_name,section_name,is_active,created_by,date_time,insertion,updation) Values(@id,@class_id,@class_name,@emp_id,@emp_name,@section_name,@is_active,@created_by,@date_time,@insertion,@updation)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_name;
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_id;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_name;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.section_name;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sections_obj.date_time;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;


                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Online Db sections      ---------------------------------
        public int update_online_db_sections(sections sections_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_subjects SET class_id=@class_id,class_name=@class_name,emp_id=@emp_id,emp_name=@emp_name,section_name=@section_name,is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.class_name;
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_id;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.emp_name;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.section_name;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sections_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Local Db sections      ---------------------------------
        public int update_local_db_sections(sections sections_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_subjects SET insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sections_obj.id;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;                       
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        // ================================================    Subjects =======================================

        //-----------       Get All Subjects Data    ----------------------
        public void get_all_subjects()
        {

            subjects_list = new List<subjects>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {



                    cmd.CommandText = "SELECT* FROM sms_subject";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            subjects subject = new subjects()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                subject_name = Convert.ToString(reader["subject_name"].ToString()),
                                class_id = Convert.ToString(reader["class_id"].ToString()),
                                class_name = Convert.ToString(reader["class_name"].ToString()),
                                emp_id = Convert.ToString(reader["emp_id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                is_Active = Convert.ToString(reader["is_Active"].ToString()),
                                total_marks = Convert.ToString(reader["total_marks"].ToString()),
                                remarks = Convert.ToString(reader["remarks"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),


                            };
                            subjects_list.Add(subject);

                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        // ----------------------     Insert online db Subjects ------------------------
        public int insert_online_db_subjects(subjects subjects_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_subject(id,class_id,class_name,emp_id,emp_name,subject_name,remarks,total_marks,is_active,created_by,date_time,insertion,updation) Values(@id,@class_id,@class_name,@emp_id,@emp_name,@subject_name,@remarks,@total_marks,@is_active,@created_by,@date_time,@insertion,@updation)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_name;
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_id;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_name;
                        cmd.Parameters.Add("@subject_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_name;
                        cmd.Parameters.Add("@remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.remarks;
                        cmd.Parameters.Add("@total_marks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.total_marks;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.is_Active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = subjects_obj.date_time;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;


                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Online Db subjects      ---------------------------------
        public int update_online_db_subjects(subjects subjects_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_subject SET class_id=@class_id,class_name=@class_name,emp_id=@emp_id,emp_name=@emp_name,subject_name=@subject_name,remarks=@remarks,total_marks=@total_marks,is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.id;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.class_name;
                        cmd.Parameters.Add("@emp_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_id;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.emp_name;
                        cmd.Parameters.Add("@subject_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.subject_name;
                        cmd.Parameters.Add("@remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.remarks;
                        cmd.Parameters.Add("@total_marks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.total_marks;
                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.is_Active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = subjects_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Local Db subjects      ---------------------------------
        public int update_local_db_subjects(subjects subjects_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_subject SET insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = subjects_obj.id;                        
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        //==============================================   Employees =======================================

        // -------------      Get All Employees       --------------------

        public void get_all_employees()
        {
            emp_list = new List<employees>();
            using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {



                    cmd.CommandText = "SELECT* FROM sms_emp";
                    cmd.Connection = con;
                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;                    
                    try
                    {
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            employees emp = new employees()
                            {

                                id = Convert.ToString(reader["id"].ToString()),
                                emp_name = Convert.ToString(reader["emp_name"].ToString()),
                                emp_father = Convert.ToString(reader["emp_father"].ToString()),
                                emp_nationality = Convert.ToString(reader["emp_nationality"].ToString()),
                                emp_religion = Convert.ToString(reader["emp_religion"].ToString()),
                                emp_exp = Convert.ToString(reader["emp_exp"].ToString()),
                                emp_cnic = Convert.ToString(reader["emp_cnic"].ToString()),
                                emp_qual = Convert.ToString(reader["emp_qual"].ToString()),
                                emp_sex = Convert.ToString(reader["emp_sex"].ToString()),
                                emp_marital = Convert.ToString(reader["emp_marital"].ToString()),
                                emp_dob = Convert.ToDateTime(reader["emp_dob"]),
                                emp_email = Convert.ToString(reader["emp_email"].ToString()),
                                emp_address = Convert.ToString(reader["emp_address"].ToString()),
                                emp_remarks = Convert.ToString(reader["emp_remarks"].ToString()),
                                emp_pay = Convert.ToString(reader["emp_pay"].ToString()),
                                emp_cell = Convert.ToString(reader["emp_cell"].ToString()),
                                emp_phone = Convert.ToString(reader["emp_phone"].ToString()),
                                emp_type_id = Convert.ToString(reader["emp_type_id"].ToString()),
                                emp_type = Convert.ToString(reader["emp_type"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                created_by = Convert.ToString(reader["created_by"].ToString()),
                                is_active = Convert.ToString(reader["is_active"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),
                            };
                            emp_list.Add(emp);

                        }
                    }
                    catch
                    {
                        MessageBox.Show("oops! Employees DB not connected");
                    }

                }
            }
        }

        // ----------------------     Insert online db employees ------------------------
        public int insert_online_db_emp(employees emp_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_emp(id,emp_type_id,emp_type,emp_name,emp_father,emp_nationality,emp_religion,emp_exp,emp_cnic,emp_qual,emp_sex,emp_marital,emp_dob,emp_email,emp_address,emp_remarks,emp_phone,emp_cell,emp_pay,is_active,created_by,date_time,insertion,updation) Values(@id,@emp_type_id,@emp_type,@emp_name,@emp_father,@emp_nationality,@emp_religion,@emp_exp,@emp_cnic,@emp_qual,@emp_sex,@emp_marital,@emp_dob,@emp_email,@emp_address,@emp_remarks,@emp_phone,@emp_cell,@emp_pay,@is_active,@created_by,@date_time,@insertion,@updation)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.id;
                        cmd.Parameters.Add("@emp_type_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type_id;
                        cmd.Parameters.Add("@emp_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_name;
                        cmd.Parameters.Add("@emp_father", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_father;
                        cmd.Parameters.Add("@emp_nationality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_nationality;
                        cmd.Parameters.Add("@emp_religion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_religion;
                        cmd.Parameters.Add("@emp_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_exp;
                        cmd.Parameters.Add("@emp_cnic", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cnic;
                        cmd.Parameters.Add("@emp_qual", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_qual;
                        cmd.Parameters.Add("@emp_sex", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_sex;
                        cmd.Parameters.Add("@emp_marital", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_marital;
                        cmd.Parameters.Add("@emp_dob", MySql.Data.MySqlClient.MySqlDbType.Date).Value = emp_obj.emp_dob;
                        cmd.Parameters.Add("@emp_email", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_email;
                        cmd.Parameters.Add("@emp_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_address;
                        cmd.Parameters.Add("@emp_remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_remarks;
                        cmd.Parameters.Add("@emp_phone", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_phone;
                        cmd.Parameters.Add("@emp_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cell;
                        cmd.Parameters.Add("@emp_pay", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_pay;

                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = emp_obj.date_time;

                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;


                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());

                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Online Db emp      ---------------------------------
        public int update_online_db_emp(employees emp_obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_emp SET emp_type_id=@emp_type_id,emp_type=@emp_type,emp_name=@emp_name,emp_father=@emp_father,emp_nationality=@emp_nationality,emp_religion=@emp_religion,emp_exp=@emp_exp,emp_cnic=@emp_cnic,emp_qual=@emp_qual,emp_sex=@emp_sex,emp_marital=@emp_marital,emp_dob=@emp_dob,emp_email=@emp_email,emp_address=@emp_address,emp_remarks=@emp_remarks,emp_phone=@emp_phone,emp_cell=@emp_cell,emp_pay=@emp_pay,is_active=@is_active,created_by=@created_by,date_time=@date_time,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.id;
                        cmd.Parameters.Add("@emp_type_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type_id;
                        cmd.Parameters.Add("@emp_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_type;
                        cmd.Parameters.Add("@emp_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_name;
                        cmd.Parameters.Add("@emp_father", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_father;
                        cmd.Parameters.Add("@emp_nationality", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_nationality;
                        cmd.Parameters.Add("@emp_religion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_religion;
                        cmd.Parameters.Add("@emp_exp", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_exp;
                        cmd.Parameters.Add("@emp_cnic", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cnic;
                        cmd.Parameters.Add("@emp_qual", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_qual;
                        cmd.Parameters.Add("@emp_sex", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_sex;
                        cmd.Parameters.Add("@emp_marital", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_marital;
                        cmd.Parameters.Add("@emp_dob", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = emp_obj.emp_dob;
                        cmd.Parameters.Add("@emp_email", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_email;
                        cmd.Parameters.Add("@emp_address", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_address;
                        cmd.Parameters.Add("@emp_remarks", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_remarks;
                        cmd.Parameters.Add("@emp_phone", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_phone;
                        cmd.Parameters.Add("@emp_cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_cell;
                        cmd.Parameters.Add("@emp_pay", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.emp_pay;

                        cmd.Parameters.Add("@is_active", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.is_active;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp_obj.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = emp_obj.date_time;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con.Open();

                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //---------------           Update Local Db emp      ---------------------------------
        public int update_local_db_emp(employees emp) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_emp SET insertion=@insertion,updation=@updation WHERE id = @id";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = emp.id;
                        cmd.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;

                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        //============================================ Sms Institute ========================================

        // ---------      Get  institute   ---------------
        public void get_institute()
        {
            ins = new institute();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "select * from sms_institute";
                        con.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                       
                        ins.institute_name = Convert.ToString(reader["institute_name"].ToString());
                        ins.male_image = (byte[])(reader["male_image"]);
                        ins.female_image = (byte[])(reader["female_image"]);
                        ins.institute_logo = (byte[])(reader["institute_logo"]);
                        ins.mac = Convert.ToString(reader["mac"].ToString());
                        ins.insertion = Convert.ToString(reader["insertion"].ToString());

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ---------      Update Online Db    ---------------
        public int update_online_db_institute()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_institute SET institute_name=@institute_name,mac=@mac,insertion=@insertion,institute_logo=@institute_logo ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        cmd.Parameters.Add("@institute_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = ins.institute_name;
                        cmd.Parameters.Add("@institute_logo", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = ins.institute_logo;
                        cmd.Parameters.Add("@male_image", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = ins.male_image;
                        cmd.Parameters.Add("@female_image", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = ins.female_image;
                        cmd.Parameters.Add("@mac", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = ins.mac;
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "false";
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }
        // ---------      Update Local Db    ---------------
        public int update_local_db_institute()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "Update sms_institute SET insertion=@insertion ";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                                
                        cmd.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = "false";
                        con.Open();
                        i = Convert.ToInt32(cmd.ExecuteNonQuery());
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }


        //======================================= Paid Feeses ====================================================

        // ------------ Get All Paid Feeses ----------------

        public void get_all_paid_fee()
        {
            paid_fee_list = new List<fee>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "select * from sms_fee_paid ";

                        cmd.Connection = con;
                        con.Open();

                        MySqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            fee paid_fee = new fee()
                            {
                                id = Convert.ToString(reader["id"].ToString()),
                                std_id = Convert.ToString(reader["std_id"].ToString()),
                                reg_fee = Convert.ToString(reader["reg_fee_paid"].ToString()),
                                adm_fee = Convert.ToString(reader["adm_fee_paid"].ToString()),
                                security_fee = Convert.ToString(reader["security_fee_paid"].ToString()),
                                exam_fee = Convert.ToString(reader["exam_fee_paid"].ToString()),
                                transport_fee = Convert.ToString(reader["transport_fee_paid"].ToString()),
                                tution_fee = Convert.ToString(reader["tution_fee_paid"].ToString()),
                                other_expenses = Convert.ToString(reader["other_exp_paid"].ToString()),
                                month = Convert.ToString(reader["month"].ToString()),
                                date_time = Convert.ToDateTime(reader["date_time"]),
                                receipt_no = Convert.ToString(reader["receipt_no"].ToString()),
                                insertion = Convert.ToString(reader["insertion"].ToString()),
                                updation = Convert.ToString(reader["updation"].ToString()),



                            };
                            paid_fee_list.Add(paid_fee);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ----------------------     Insert online db Paid Fee ------------------------
        public int insert_online_db_paid_fee(fee obj) 
        {
            int i = 0;
            try
            {
                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string_web))
                {
                    using (MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "insert into sms_fee_paid (id,std_id,reg_fee_paid,adm_fee_paid,security_fee_paid,exam_fee_paid,transport_fee_paid,tution_fee_paid,other_exp_paid,month,date_time,created_by,receipt_no,insertion,updation)Values(@id,@std_id,@reg_fee_paid,@adm_fee_paid,@security_fee_paid,@exam_fee_paid,@transport_fee_paid,@tution_fee_paid,@other_exp_paid,@month,@date_time,@created_by,@receipt_no,@insertion,@updation)";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;
                        cmd_paid.Parameters.Add("@std_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.std_id;
                        cmd_paid.Parameters.Add("@reg_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.reg_fee;
                        cmd_paid.Parameters.Add("@adm_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.adm_fee;
                        cmd_paid.Parameters.Add("@security_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.security_fee;
                        cmd_paid.Parameters.Add("@exam_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.exam_fee;
                        cmd_paid.Parameters.Add("@transport_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.transport_fee;
                        cmd_paid.Parameters.Add("@tution_fee_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.tution_fee;
                        cmd_paid.Parameters.Add("@other_exp_paid", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.other_expenses;
                        cmd_paid.Parameters.Add("@month", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.month;
                        cmd_paid.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = obj.date_time;
                        cmd_paid.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.created_by;
                        cmd_paid.Parameters.Add("@receipt_no", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.receipt_no;

                        cmd_paid.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd_paid.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con_paid.Open();
                        i=cmd_paid.ExecuteNonQuery();
                        con_paid.Close();
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }

        //-----------------------    Update Local Db paid Fee  ------------------------

        public int update_local_db_paid_fee(fee obj)
        {
            int i = 0;
            try
            {
                using (MySqlConnection con_paid = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd_paid = new MySqlCommand())
                    {
                        cmd_paid.CommandText = "Update sms_fee_paid SET insertion=@insertion where id = @id";
                        cmd_paid.Connection = con_paid;

                        cmd_paid.Parameters.Add("@id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = obj.id;                        

                        cmd_paid.Parameters.Add("@insertion", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = insertion;
                        cmd_paid.Parameters.Add("@updation", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = updation;

                        con_paid.Open();
                        i = cmd_paid.ExecuteNonQuery();
                        con_paid.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return i;
        }
    }
}
