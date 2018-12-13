using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SMS
{
  public  class WindowManager
    {
        public static void LaunchWindowNewThread<T>() where T : Window, new()
        {
            Thread newWindowThread = new Thread(ThreadStartingPoint);
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;

            Func<Window> f = delegate
            {
                T win = new T();
                return win;
            };

            newWindowThread.Start(f);
        }

        private static void ThreadStartingPoint(object t)
        {
            Func<Window> f = (Func<Window>)t;
            Window win = f();

            win.Show();
            Dispatcher.Run();
        }
    }
}
