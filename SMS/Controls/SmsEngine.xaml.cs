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
using SMS.Messaging;
using System.IO.Ports;
using System.Threading;
using SMS.Upload;
using MySql.Data.MySqlClient;
using GsmComm.PduConverter;
using GsmComm.GsmCommunication;
using GsmComm.PduConverter.SmartMessaging;
using System.Collections.Specialized;

namespace SMS.Controls
{
    /// <summary>
    /// Interaction logic for SmsEngine.xaml
    /// </summary>
    public partial class SmsEngine : UserControl
    {
        public static SerialPort port = new SerialPort();
        private BackgroundWorker bw = new BackgroundWorker();
        public List<admission> std_nos;
        sms_history sh;
        string[] messages;
        string a;
        string b;
        int error_no = 0;
        public GsmCommMain comm;
        private static ushort refNumber;        
        int totalSmsSent = 0;
        bool isWholeSent = false;

        admission adm_obj;
        int i = 0;

        public SmsEngine()
        {
            InitializeComponent();     
        }
        public void openPort() 
        {
            try
            {                
                comm = new GsmCommMain("COM22", 115200, 300);
                //comm = new GsmCommMain("COM22", 19200, 300);
                comm.Open();
            }
            catch (Exception ex)                                                                                       
            {
                MessageBox.Show("Opening Port Exception: " + ex.Message);
            }
        }
        private void loaded_smsEngine(object sender, RoutedEventArgs e)
        {                        
            start_btn.Visibility = Visibility.Visible;           
            try
            {
                foreach (var portName in SerialPort.GetPortNames())
                {
                    if (portName == "COM22")
                    {
                        SerialPort port = new SerialPort(portName);
                        if (port.IsOpen)
                        {

                        }
                        else
                        {
                            //port.Close();
                            port.Dispose();
                        }
                    }
                }                

                openPort();
                
                std_nos = new List<admission>();
                std_nos = UploadWindow.std_nos_sms;
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
                start_btn.Visibility = Visibility.Collapsed ;           
            }
        }
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (bw.IsBusy != true)
            {
                start_btn.Visibility = Visibility.Hidden;
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

           
              
            if (sendMsg(std_nos , worker, e))
            {
                //MessageBox.Show("Message has sent successfully");
                // sms_prog_bar.Visibility = Visibility.Hidden;
                //MessageBox.Show("Message has sent successfully","Successfully Sent",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else
            {
                //MessageBox.Show("Failed to send message");
                // sms_prog_bar.Visibility = Visibility.Hidden;
                MessageBox.Show("Failed to send message", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
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
                start_btn.Visibility = Visibility.Hidden;
                cancel_btn.Visibility = Visibility.Hidden;
                finsish_btn.Visibility = Visibility.Visible;
                this.status_textblock.Text = "  Successfully Sent!";
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(isWholeSent)
            {
                this.sms_grid.Items.Insert(0, adm_obj);
                this.sms_grid.Items.Refresh();
            }            

            this.progressbar_textblock.Text = (e.ProgressPercentage.ToString() + "%");
            this.progressbar.Value = i;
            this.uploader_content_textblock.Text = i.ToString();
            this.totalSmsSentTB.Text = totalSmsSent.ToString();
        }
        
        // ----------------------------------------SMS----------------------------------------------------------------------------             

        #region Send SMS

        static AutoResetEvent readNow = new AutoResetEvent(false);

        public void closePort() 
        {
            try
            {
                if (comm.IsOpen())
                {
                    comm.Close();
                }              
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Closing Port Exception: "+ex.Message);
            }
        }

        public bool sendMsg(List<admission> std_nos , BackgroundWorker worker, DoWorkEventArgs e)
        {        
            bool isSend = false;
            i = 0;                        

            foreach (admission adm in std_nos)
            {                
                i++;
                try
                {
                    if ((worker.CancellationPending == true))
                    {
                        i = 0;
                        e.Cancel = true;
                        worker.ReportProgress(((i * 100) / std_nos.Count));
                        break;
                    }
                    else
                    {
                        isSend = false;
                        Thread.Sleep(500);
                        adm_obj = new admission();

                        adm_obj.std_name = adm.std_name;
                        adm_obj.cell_no = adm.cell_no;
                        adm_obj.father_name = adm.father_name;
                        adm_obj.class_name = adm.class_name;
                        adm_obj.roll_no = adm.roll_no;
                        adm_obj.sms_status = "Sent";
                        isWholeSent = false;

                        SmsSubmitPdu[] pdu;
                        if (UploadWindow.isEncoded)
                        {
                            pdu = CreateConcatTextMessage(adm.sms_message, true, Convert.ToString("+92" + adm.cell_no));
                        }
                        else
                        {
                            pdu = CreateConcatTextMessage(adm.sms_message, false, Convert.ToString("+92" + adm.cell_no));
                        }

                        for (int j = 0; j < pdu.Length; j++)
                        {
                            try
                            {
                                if ((worker.CancellationPending == true))
                                {
                                    i = 0;
                                    e.Cancel = true;
                                    worker.ReportProgress(((i * 100) / std_nos.Count));
                                    break;
                                }

                                if (comm.IsConnected())
                                {
                                    comm.SendMessage(pdu[j],false);
                                    isSend = true;
                                    totalSmsSent++;
                                    if (j + 1 == pdu.Length)
                                    {
                                        isWholeSent = true;
                                    }
                                    worker.ReportProgress(((i * 100) / std_nos.Count));
                                }
                                else 
                                {
                                    j--;
                                    MessageBox.Show("IsConnected()=false", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                                    isSend = false;
                                    adm_obj.sms_status = "Not Sent";
                                    

                                    j--;
                                    MessageBoxResult mbr = MessageBox.Show("Do You Want To Retry ?", "Send Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                                    if (mbr == MessageBoxResult.Yes)
                                    {

                                        closePort();
                                        openPort();
                                    }
                                    else if (mbr == MessageBoxResult.No)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        return false;
                                        
                                    }
                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK, MessageBoxImage.Stop);
                                isSend = false;
                                adm_obj.sms_status = "Not Sent";
                                if (ex.Message.ToUpper().Equals("PORT NOT OPEN") || ex.Message.ToUpper().Equals("PHONE NOT CONNECTED"))
                                {
                                    
                                    j--;
                                    MessageBoxResult mbr = MessageBox.Show("Do You Want To Retry ?", "Send Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                                    if (mbr == MessageBoxResult.Yes)
                                    {
                                        
                                        closePort();
                                        openPort();
                                    }
                                    else if (mbr == MessageBoxResult.No)
                                    {
                                        break;
                                    }
                                    else 
                                    {
                                        return false;
                                    }
                                }
                                
                            }

                        }
                        // saved to sms history table
                        if (isSend)
                        {
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

                            submit();
                        }

                        // Send multipart sms  -----------------------
                        #region Old SMS
                        //if (isMultiPartAccess == "Y")
                        //{
                        //    isWholeSent = false;
                        //    SmsSubmitPdu[] pdu = CreateConcatTextMessage(adm.sms_message, false, Convert.ToString("+92" + adm.cell_no));
                        //    for (int j = 0; j < pdu.Length; j++)
                        //    {
                        //        try
                        //        {
                        //            comm.SendMessage(pdu[j]);
                        //            isSend = true;
                        //            totalSmsSent++;
                        //            if (j + 1 == pdu.Length)
                        //            {
                        //                isWholeSent = true;
                        //            }
                        //            worker.ReportProgress(((i * 100) / std_nos.Count));
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            MessageBox.Show(ex.Message);
                        //            isSend = false;
                        //            adm_obj.sms_status = "Not Sent";
                        //        }

                        //    }
                        //    // saved to sms history table
                        //    if (isSend)
                        //    {
                        //        sh = new sms_history();
                        //        sh.sender_id = adm.id;
                        //        sh.sender_name = adm.std_name;
                        //        sh.class_id = adm.class_id;
                        //        sh.class_name = adm.class_name;
                        //        sh.section_id = adm.section_id;
                        //        sh.section_name = adm.section_name;
                        //        sh.cell = adm.cell_no;
                        //        sh.msg = adm.sms_message;
                        //        sh.sms_type = adm.sms_type;
                        //        sh.created_by = MainWindow.emp_login_obj.emp_user_name;
                        //        sh.date_time = DateTime.Now;

                        //        submit();
                        //    }
                        //}
                        //else
                        //{
                        //    isWholeSent = true;
                        //    // send single sms -------------------------------------
                        //    if (adm.sms_message.Length > 160)
                        //    {
                        //        messages = new string[2];
                        //        messages[0] = adm.sms_message.Substring(0, 160);
                        //        messages[1] = adm.sms_message.Substring(160);
                        //        //messages[2] = adm.sms_message.Substring(321);
                        //    }
                        //    else
                        //    {
                        //        messages = new string[1];
                        //        messages[0] = adm.sms_message.ToString();
                        //    }
                        //    for (int j = 0; j < messages.Length; j++)
                        //    {
                        //        sh = new sms_history();
                        //        sh.sender_id = adm.id;
                        //        sh.sender_name = adm.std_name;
                        //        sh.class_id = adm.class_id;
                        //        sh.class_name = adm.class_name;
                        //        sh.section_id = adm.section_id;
                        //        sh.section_name = adm.section_name;
                        //        sh.cell = adm.cell_no;
                        //        sh.msg = messages[j];
                        //        sh.sms_type = adm.sms_type;
                        //        sh.created_by = MainWindow.emp_login_obj.emp_user_name;
                        //        sh.date_time = DateTime.Now;
                        //        try
                        //        {
                        //            try
                        //            {
                        //                SmsSubmitPdu pdu;
                        //                byte dcs = (byte)DataCodingScheme.GeneralCoding.Alpha7BitDefault;
                        //                pdu = new SmsSubmitPdu(messages[j], Convert.ToString("+92" + adm.cell_no), dcs);
                        //                comm.SendMessage(pdu);
                        //                isSend = true;
                        //                totalSmsSent++;
                        //                worker.ReportProgress(((i * 100) / std_nos.Count));
                        //                submit();
                        //                Thread.Sleep(500);
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                MessageBox.Show("Modem is not available");
                        //                isSend = false;
                        //                adm_obj.sms_status = "Not Sent";
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            MessageBox.Show("SMS not send");
                        //            isSend = false;
                        //            adm_obj.sms_status = "Not Sent";
                        //        }
                        //    }
                        //}
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    isSend = false;
                }
            }            
            return isSend;
        }
        public void setConnected() 
        {
            string cmbCOM = "COM22";
            if (cmbCOM == "")
            {
                MessageBox.Show("Invalid Port Name");
                return;
            }
            comm = new GsmCommMain(cmbCOM, 115200, 150);
            //Cursor.Current = Cursors.Default;

            bool retry;
            do
            {
                retry = false;
                try
                {
                    //Cursor.Current = Cursors.WaitCursor;
                    comm.Open();
                    //Cursor.Current = Cursors.Default;
                    MessageBox.Show("Modem Connected Sucessfully");
                }
                catch (Exception)
                {
                    //Cursor.Current = Cursors.Default;                    
                }
            }
            while (retry);
            
        }

        static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                    readNow.Set();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

       

        private void finsish_btn_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
           // comm.Close();            
            parentWindow.Close();
        }


        //public static SmsSubmitPdu[] CreateConcatTextMessage(string userDataText, string destinationAddress)
        //{
        //    return SmartMessageFactory.CreateConcatTextMessage(userDataText, false, destinationAddress);
        //}

        /// <summary>
        /// Creates a concatenated text message.
        /// </summary>
        /// <param name="userDataText">The message text.</param>
        /// <param name="unicode">Specifies if the userDataText is to be encoded as Unicode. If not, the GSM 7-bit default alphabet is used.</param>
        /// <param name="destinationAddress">The message's destination address.</param>
        /// <returns>A set of <see cref="T:GsmComm.PduConverter.SmsSubmitPdu" /> objects that represent the message.</returns>
        /// <remarks>
        /// <para>A concatenated message makes it possible to exceed the maximum length of a normal message,
        /// created by splitting the message data into multiple parts.</para>
        /// <para>Concatenated messages are also known as long or multi-part messages.</para>
        /// <para>If no concatenation is necessary, a single, non-concatenated <see cref="T:GsmComm.PduConverter.SmsSubmitPdu" /> object is created.</para>
        /// </remarks>
        /// <exception cref="T:System.ArgumentException"><para>userDataText is so long that it would create more than 255 message parts.</para></exception>
        public static SmsSubmitPdu[] CreateConcatTextMessage(string userDataText, bool unicode, string destinationAddress)
        {
            string str;
            int length = 0;
            int num;
            byte[] bytes;
            SmsSubmitPdu smsSubmitPdu;
            int num1;
            byte num2;
            if (unicode)
            {
                num1 = 70;
            }
            else
            {
                num1 = 160;
            }
            int num3 = num1;
            if (unicode)
            {
                str = userDataText;
            }
            else
            {
                str = TextDataConverter.StringTo7Bit(userDataText);
            }
            if (str.Length <= num3)
            {
                if (unicode)
                {
                    smsSubmitPdu = new SmsSubmitPdu(userDataText, destinationAddress, 8);
                }
                else
                {
                    smsSubmitPdu = new SmsSubmitPdu(userDataText, destinationAddress);
                }
                SmsSubmitPdu[] smsSubmitPduArray = new SmsSubmitPdu[1];
                smsSubmitPduArray[0] = smsSubmitPdu;
                return smsSubmitPduArray;
            }
            else
            {
                ConcatMessageElement16 concatMessageElement16 = new ConcatMessageElement16(0, 0, 0);
                byte length1 = (byte)((int)SmartMessageFactory.CreateUserDataHeader(concatMessageElement16).Length);
                byte num4 = (byte)((double)length1 / 7 * 8);
                if (unicode)
                {
                    num2 = length1;
                }
                else
                {
                    num2 = num4;
                }
                byte num5 = num2;
                StringCollection stringCollections = new StringCollection();
                for (int i = 0; i < str.Length; i = i + length)
                {
                    if (!unicode)
                    {
                        if (str.Length - i < num3 - num5)
                        {
                            length = str.Length - i;
                        }
                        else
                        {
                            length = num3 - num5;
                        }
                    }
                    else
                    {
                        if (str.Length - i < (num3 * 2 - num5) / 2)
                        {
                            length = str.Length - i;
                        }
                        else
                        {
                            length = (num3 * 2 - num5) / 2;
                        }
                    }
                    string str1 = str.Substring(i, length);
                    stringCollections.Add(str1);
                }
                if (stringCollections.Count <= 255)
                {
                    SmsSubmitPdu[] smsSubmitPduArray1 = new SmsSubmitPdu[stringCollections.Count];
                    ushort num6 = CalcNextRefNumber();
                    byte num7 = 0;
                    for (int j = 0; j < stringCollections.Count; j++)
                    {
                        num7 = (byte)(num7 + 1);
                        ConcatMessageElement16 concatMessageElement161 = new ConcatMessageElement16(num6, (byte)stringCollections.Count, num7);
                        byte[] numArray = SmartMessageFactory.CreateUserDataHeader(concatMessageElement161);
                        if (unicode)
                        {
                            Encoding bigEndianUnicode = Encoding.BigEndianUnicode;
                            bytes = bigEndianUnicode.GetBytes(stringCollections[j]);
                            num = (int)bytes.Length;
                        }
                        else
                        {
                            bytes = TextDataConverter.SeptetsToOctetsInt(stringCollections[j]);
                            num = stringCollections[j].Length;
                        }
                        SmsSubmitPdu smsSubmitPdu1 = new SmsSubmitPdu();
                        smsSubmitPdu1.DestinationAddress = destinationAddress;
                        if (unicode)
                        {
                            smsSubmitPdu1.DataCodingScheme = 8;
                        }
                        smsSubmitPdu1.SetUserData(bytes, (byte)num);
                        smsSubmitPdu1.AddUserDataHeader(numArray);
                        smsSubmitPduArray1[j] = smsSubmitPdu1;
                    }
                    return smsSubmitPduArray1;
                }
                else
                {
                    throw new ArgumentException("A concatenated message must not have more than 255 parts.", "userDataText");
                }
            }
        }

        protected static ushort CalcNextRefNumber()
        {
            ushort num;
            lock (typeof(SmartMessageFactory))
            {                
                num = refNumber;
                if (refNumber != 65535)
                {
                    refNumber = (ushort)(refNumber + 1);
                }
                else
                {
                    refNumber = 1;
                }
            }
            return num;
        }

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
    }
}
