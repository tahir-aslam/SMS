using SMS.Helpers;
using SMS.Views.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace SMS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {

        private static SingleInstance si = null;
        private readonly Guid _appGuid = new Guid("{CAAD7A96-0E2E-4FD2-891B-7FD6CFED33CE}");
        protected override void OnStartup(StartupEventArgs e)
        {


            base.OnStartup(e);

            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            si = new SingleInstance(_appGuid, this);
            si.Run(() =>
            {
                try
                {
                    Splasher.Splash = new SplashWindow();
                    Splasher.ShowSplash();
                    Console.WriteLine("{0} Splash Window", DateTime.Now);

                    Console.WriteLine("{0} Loading DataBegins", DateTime.Now);
                    ApplicationManager.Instance.ExecuteAsync(() =>
                    {
                        ApplicationManager.Instance.LoadData();                        
                    }, () =>
                    {
                        Console.WriteLine("{0} Loading Data Finishes", DateTime.Now);

                        new MainWindow().Show();
                        Splasher.CloseSplash();
                    },true
                );
                    return this.MainWindow;
                }
                catch (Exception ex)
                {
                    //Logger.Error(ex.Message, ex);
                    // MessageBox.Show(ex.Message + ex.StackTrace);
                    Application.Current.Shutdown();
                    return null;
                }
            });
        }
    }
}
