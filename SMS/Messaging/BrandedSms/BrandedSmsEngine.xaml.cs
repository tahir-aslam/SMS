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
using MySql.Data.MySqlClient;
using SMS.Models;
using SMS.Messaging;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;
using System.Net;

namespace SMS.Messaging.BrandedSms
{
    /// <summary>
    /// Interaction logic for BrandedSmsEngine.xaml
    /// </summary>
    public partial class BrandedSmsEngine : Window
    {
        private BackgroundWorker bw = new BackgroundWorker();
        public List<admission> std_nos;
        sms_history sh;
        string[] messages;
        bool isEncoded = false;
        string a;
        string b;
        int error_no = 0;

        admission adm_obj;
        int i = 0;

        public BrandedSmsEngine(List<admission> nos, bool isEncoded=false)
        {
            InitializeComponent();
            std_nos = new List<admission>();
            std_nos = nos;
            this.isEncoded = isEncoded;
        }

        private void loaded_smsEngine(object sender, RoutedEventArgs e)
        {
            uploader_btn.Visibility = Visibility.Visible;
            try
            {
                
                sms_grid.ItemsSource = null;
                //progress bar

                progressbar.Minimum = 0;
                progressbar.Maximum = std_nos.Count;
                uploader_content_total_textblock.Text = std_nos.Count.ToString();

                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new DoWorkEventHandler(bw_DoWork);
                bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (bw.IsBusy != true)
            {
                uploader_btn.Visibility = Visibility.Hidden;
                cancel_btn.Visibility = Visibility.Visible;
                this.status_textblock.Text = "";
                bw.RunWorkerAsync();
            }
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
            i = 0;
            foreach (admission adm in std_nos)
            {
                i++;
                try
                {
                    if ((worker.CancellationPending == true))
                    {
                        e.Cancel = true;
                        worker.ReportProgress(((i * 100) / std_nos.Count));
                        break;
                    }
                    else
                    {
                        adm_obj = new admission();
                        adm_obj.std_name = adm.std_name;
                        adm_obj.cell_no = adm.cell_no;
                        adm_obj.father_name = adm.father_name;
                        adm_obj.class_name = adm.class_name;
                        adm_obj.roll_no = adm.roll_no;


                        sh = new sms_history();
                        sh.sender_id = adm.id;
                        sh.sender_name = adm.std_name;
                        sh.class_id = adm.class_id;
                        sh.class_name = adm.class_name;
                        sh.section_id = adm.section_id;
                        sh.section_name = adm.section_name;
                        sh.cell = adm.cell_no;
                        sh.msg = adm.sms_message;
                        sh.sms_type = adm.sms_type;
                        sh.created_by = MainWindow.emp_login_obj.emp_user_name;
                        sh.date_time = DateTime.Now;
                        try
                        {
                            //Thread.Sleep(500);
                            var client = new WebClient();
                            string res;
                            string url="";
                            //string res = client.DownloadString("http://cloud.advancesmssoftware.com/api/sendsms/plain.aspx?user=tahir315&password=rzs5874k000&sender=AIGS&smstext=" + adm.sms_message + "&gsm=92" + adm.cell_no);
                            //string res = client.DownloadString(MainWindow.emp_login_obj.branded_url+"?user="+MainWindow.emp_login_obj.branded_user_name+"&password="+MainWindow.emp_login_obj.branded_pwd+"&sender="+MainWindow.emp_login_obj.branded_name+"&smstext="+adm.sms_message+"&gsm=92"+adm.cell_no);
                            if (isEncoded)
                            {
                                //res = client.DownloadString(MainWindow.emp_login_obj.branded_url + "?id=" + MainWindow.emp_login_obj.branded_user_name + "&pass=" + MainWindow.emp_login_obj.branded_pwd + "&mask=" + MainWindow.emp_login_obj.branded_name + "&msg=" + adm.sms_message + "&to=92" + adm.cell_no + "&lang=urdu&type=xml");
                                url = MainWindow.emp_login_obj.branded_url_encoded;                                
                                url = url.Replace("cell_no", "92" + adm.cell_no);
                                url = url.Replace("text_message", adm.sms_message);
                                res = client.DownloadString(url);
                            }
                            else                             
                            {
                                //res = client.DownloadString(MainWindow.emp_login_obj.branded_url + "?id=" + MainWindow.emp_login_obj.branded_user_name + "&pass=" + MainWindow.emp_login_obj.branded_pwd + "&mask=" + MainWindow.emp_login_obj.branded_name + "&msg=" + adm.sms_message + "&to=92" + adm.cell_no + "&lang=English&type=xml");
                                url = MainWindow.emp_login_obj.branded_url;                                
                                url = url.Replace("cell_no", "92" + adm.cell_no);
                                url = url.Replace("text_message", adm.sms_message);
                                res = client.DownloadString(url);
                            
                            }


                            
                            if (res.Contains("<code>300</code>"))
                            {
                                adm_obj.sms_status = "Sent";
                                submit();
                                worker.ReportProgress(((i * 100) / std_nos.Count));                                
                            }
                            else if (res.Contains("Message Sent Successfully"))
                            {
                                adm_obj.sms_status = "Sent";
                                submit();
                                worker.ReportProgress(((i * 100) / std_nos.Count));  
                            }    
                            else if(res.ToUpper().Contains("PROMOTIONAL MESSAGE IS BLOCKED"))
                            {
                                adm_obj.sms_status = "Sent";
                                submit();
                                worker.ReportProgress(((i * 100) / std_nos.Count));         
                            }
                            else
                            {                                
                                MessageBox.Show(res);
                                continue;
                                //break;                                
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(500);
                }

            }

        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                cancel_btn.Visibility = Visibility.Hidden;
                finsish_btn.Visibility = Visibility.Visible;
                sms_grid.Items.Clear();
                this.status_textblock.Text = "Canceled!";
            }

            else if (!(e.Error == null))
            {
                cancel_btn.Visibility = Visibility.Hidden;
                finsish_btn.Visibility = Visibility.Visible;
                this.status_textblock.Text = ("Error: " + e.Error.Message);
            }

            else
            {
                uploader_btn.Visibility = Visibility.Hidden;
                cancel_btn.Visibility = Visibility.Hidden;
                finsish_btn.Visibility = Visibility.Visible;
                this.status_textblock.Text = "  Successfully Sent!";
            }
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.sms_grid.Items.Insert(0, adm_obj);
            this.sms_grid.Items.Refresh();

            this.progressbar_textblock.Text = (e.ProgressPercentage.ToString() + "%");
            this.progressbar.Value = i;
            this.uploader_content_textblock.Text = i.ToString();
        }
        // ----------------------------------------SMS----------------------------------------------------------------------------

        //---------------           Submit Form    ----------------------------------

        public int submit()
        {
            int i = 0;
            try
            {
                using (MySqlConnection con = new MySqlConnection(Connection_String.con_string))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.CommandText = "INSERT INTO sms_history(sender_id,sender_name,class_id,class_name,section_id,section_name,cell,created_by,date_time,sms_type,msg) Values(@sender_id,@sender_name,@class_id,@class_name,@section_id,@section_name,@cell,@created_by,@date_time,@sms_type,@msg)";
                        cmd.Connection = con;
                        //cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add("@sender_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.sender_id;
                        cmd.Parameters.Add("@sender_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.sender_name;
                        cmd.Parameters.Add("@class_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.class_id;
                        cmd.Parameters.Add("@class_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.class_name;
                        cmd.Parameters.Add("@section_id", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.section_id;
                        cmd.Parameters.Add("@section_name", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.section_name;
                        cmd.Parameters.Add("@cell", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.cell;
                        cmd.Parameters.Add("@msg", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.msg;
                        cmd.Parameters.Add("@sms_type", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.sms_type;
                        cmd.Parameters.Add("@created_by", MySql.Data.MySqlClient.MySqlDbType.VarChar).Value = sh.created_by;
                        cmd.Parameters.Add("@date_time", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = sh.date_time;

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

        private void finsish_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
