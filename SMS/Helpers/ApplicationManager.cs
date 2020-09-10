using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SMS.Helpers
{
    class ApplicationManager
    {
        #region Fields

        private static ApplicationManager _instance;
        private bool _isFullScreenMode;
        private bool _isBusy;
        private static object syncRoot = new Object();
        //private AppSetting _appSettings;        
        #endregion

        #region Ctor

        static ApplicationManager()
        {

            if (_instance == null)
            {
                lock (syncRoot)
                {
                    _instance = new ApplicationManager();
                }
            }
        }

        private ApplicationManager()
        {
        }


        #endregion

        #region ExecuteAsync

        public void ExecuteOnMainThread(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public void ExecuteAsync(Action execute, Action completionAction, bool showBusyIndicator = false,
            string busyIndicatorMsg = "")
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) => execute();
            bw.RunWorkerCompleted += (sender, args) =>
            {

                if (showBusyIndicator)
                    //HideBusyInidicator();

                    if (completionAction != null)
                    {
                        completionAction();
                    }
            };

            if (showBusyIndicator)
            {
                //ShowBusyInidicator(busyIndicatorMsg);
            }

                bw.RunWorkerAsync();
        }

        #endregion

        #region Busy Indicator

        //public void ShowBusyInidicator(string message = null)
        //{
        //    SendBusyIndicatorMessage(true, busyNoticeDetails: message);
        //}
        //public void ShowNotification(string notificationMessage, bool displayBusyLoader = true,
        //    bool isErrorMessage = false)
        //{
        //    SendBusyIndicatorMessage(displayBusyLoader, true, isErrorMessage, notificationMessage);
        //}

        //public void HideNotification(bool closeBusyIndicatorIfDisplayed = false)
        //{
        //    SendBusyIndicatorMessage(closeBusyIndicatorIfDisplayed);

        //}

        //public void ShowBusyInidicator(string message = null)
        //{
        //    SendBusyIndicatorMessage(true, busyNoticeDetails: message);
        //}

        //public void HideBusyInidicator()
        //{
        //    Application.Current.Dispatcher.Invoke(new Action(() =>
        //    {
        //        IsBusy = false;
        //        SendBusyIndicatorMessage(false, false, false, "");
        //    }));


        //}

        //private void SendBusyIndicatorMessage(bool isBusy = false, bool isBusyNotice = false, bool isErrorNotice = false,
        //    string busyNoticeDetails = null)
        //{
        //    var busyMessage = new BusyIndicatorMessage();


        //    busyMessage.IsBusy = isBusy;



        //    busyMessage.IsBusyNotice = isBusyNotice;



        //    busyMessage.IsErrorNotice = isErrorNotice;


        //    if (!String.IsNullOrEmpty(busyNoticeDetails))
        //    {
        //        busyMessage.BusyNoticeDetails = busyNoticeDetails;
        //    }

        //    IsBusy = true;
        //    ExecuteOnMainThread(() => Messenger.Default.Send(busyMessage));
        //}

        #endregion

        public void LoadData()
        {
            Task.Delay(2000);
        }
        public static ApplicationManager Instance
        {
            get { return _instance; }
        }
    }
}
