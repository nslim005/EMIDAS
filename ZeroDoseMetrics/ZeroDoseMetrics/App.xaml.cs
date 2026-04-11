using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroDoseMetrics.Model;
using ZeroDoseMetrics.OOSZamfara;

namespace ZeroDoseMetrics
{
    public partial class App : Application
    {
        private NetworkMonitor _networkMonitor;
        public static string DatabaseLocation = string.Empty;
        public static string ExcelExportLocation = string.Empty;

        public App ()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }


       
        public App(string dataBaseLocation, string excelPath)
        {
            InitializeComponent();
            _networkMonitor = new NetworkMonitor(() =>
            {
                // This is the block of code that will be executed when internet becomes available
                VaccinLogPage vaccinLog = new VaccinLogPage();
                //vaccinLog.TriggerSynchronization();
                //Console.WriteLine("Internet is now available!");
                // SyncData();
                // RefreshUserInterface();
                // etc.
            });
            MainPage = new NavigationPage(new MainPage());
            DatabaseLocation = dataBaseLocation;
            ExcelExportLocation = excelPath;
        }

        protected override void OnStart ()
        {
            _networkMonitor?.Start();
        }

        protected override void OnSleep ()
        {
            _networkMonitor?.Stop();
        }

        protected override void OnResume ()
        {
            _networkMonitor?.Start();
        }
    }
}

