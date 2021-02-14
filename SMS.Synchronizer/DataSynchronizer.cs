using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SMS.Synchronizer
{
    public class DataSynchronizer
    {
        #region Fields
        private static DataSynchronizer instance;
        private static object syncRoot = new object();
        //private readonly smsEntities1 context;
        private BackgroundWorker bw = new BackgroundWorker();
        #endregion

        #region Ctors       
        private DataSynchronizer()
        {
            //context = new smsEntities1();

            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            bw.RunWorkerAsync();
        }
        #endregion

        #region Properties
        public static DataSynchronizer Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        instance = new DataSynchronizer();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Methods
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            //List<sms_admission> admList = context.sms_admission.Where(x => x.insertion == "true").ToList();

        }
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                //cancel_btn.Visibility = Visibility.Hidden;
                //uploader_btn.Visibility = Visibility.Visible;
                //this.status_textblock.Text = "Canceled!";
            }
            else if (!(e.Error == null))
            {
                //cancel_btn.Visibility = Visibility.Hidden;
                //uploader_btn.Visibility = Visibility.Visible;
                //this.status_textblock.Text = ("Error: " + e.Error.Message);
            }
            else
            {
                //uploader_btn.Visibility = Visibility.Hidden;
                //cancel_btn.Visibility = Visibility.Hidden;
                //finsish_btn.Visibility = Visibility.Visible;
                //this.status_textblock.Text = "  Successfully Updated!";
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressbar.Maximum = maximum;
            //uploader_content_total_textblock.Text = Convert.ToString(maximum);
            //this.progressbar_textblock.Text = (e.ProgressPercentage.ToString() + "%");
            //this.progressbar.Value = j;
            //this.uploader_content_textblock.Text = j.ToString();
            //this.effective_rows_tb.Text = effective_rows.ToString() + "  Effective Row(s)";
            //this.records_chnges_tb.Text = records_changes;

        }
        #endregion

        #region Commands
        #endregion
    }
}
