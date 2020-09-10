using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SMS.Helpers
{
    class SingleInstance : IDisposable
    {
        private readonly Guid _appGuid;
        private readonly string _assemblyName;
        private readonly App _appContext;

        private Mutex _mutex;
        private bool _owned;
        private Window _window;

        private class Bridge
        {
            public event Action<Guid> BringToFront;        

            public void OnBringToFront(Guid appGuid)
            {
                if (BringToFront != null)
                    BringToFront(appGuid);
            }
            
            private static readonly Bridge _instance = new Bridge();

            static Bridge()
            {
            }

            public static Bridge Instance
            {
                get { return _instance; }
            }
        }

        public SingleInstance(Guid appGuid, App appContext)
        {
            _appGuid = appGuid;
            _assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            _appContext = appContext;

            Bridge.Instance.BringToFront += BringToFront;       

            _mutex = new Mutex(true, _assemblyName + _appGuid, out _owned);
        }

        public void Dispose()
        {
            if (_owned) // always release a mutex if you own it
            {
                _owned = false;
                _mutex.ReleaseMutex();
            }
        }

        public void ShutDown()
        {
            _appContext.Shutdown();

            if (_owned) // always release a mutex if you own it
            {
                _owned = false;
                _mutex.ReleaseMutex();
            }
        }

        public void Run(Func<Window> showWindow)
        {
            if (_owned)
            {
                // show the main app window
                _window = showWindow();
                                
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        
        private void BringToFront(Guid appGuid)
        {
            if (appGuid == _appGuid)
            {
                _window.Dispatcher.BeginInvoke((ThreadStart)delegate ()
                {
                    if (_window.WindowState == WindowState.Minimized)
                        _window.WindowState = WindowState.Normal;
                    _window.Activate();
                });
            }
        }        
    }
}
