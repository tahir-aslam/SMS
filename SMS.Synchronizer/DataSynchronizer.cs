using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using SMS.Models.Models;

namespace SMS.Synchronizer
{
    public class DataSynchronizer
    {
        #region Fields
        private static DataSynchronizer instance;
        public static object syncRoot = new object();
        #endregion

        #region Ctors       
        private DataSynchronizer()
        {
            context
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
        public void ExecuteOnMainThread(Action action)
        {
            //Application.Current.Dispatcher.BeginInvokeOnMainThread(action);
        }
        public void ExecuteAsync(Action execute, Action completionAction, bool showBusyIndicator = false,
            string busyIndicatorMsg = "")
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) => execute();
            bw.RunWorkerCompleted += (sender, args) =>
            {

                if (showBusyIndicator)
                    HideBusyInidicator();

                if (completionAction != null)
                {
                    completionAction();
                }
            };

            if (showBusyIndicator)
                ShowBusyInidicator(busyIndicatorMsg);

            bw.RunWorkerAsync();
        }

        #region Busy Indicator

        public void ShowBusyInidicator(string message = null)
        {
            //SendBusyIndicatorMessage(true, busyNoticeDetails: message);
        }

        public void HideBusyInidicator()
        {            
        }


        #endregion
        #endregion

        #region Commands
        #endregion


    }
}
