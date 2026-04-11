using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using RestSharp;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using ZeroDoseMetrics.Model;
using ZeroDoseMetrics.OOSZamfara;

namespace ZeroDoseMetrics
{
    public partial class MainPage : ContentPage
    {
        private List<AppVersion> version;

        public MainPage()
        {
            InitializeComponent();
            version = new List<AppVersion>();
        }

        protected override void OnAppearing()
        {   
            base.OnAppearing();

        }

        void configureBtn_Clicked(System.Object sender, System.EventArgs e)
        {

            Navigation.PushAsync(new OOSLandingPage());
        }

        void loginButton_Clicked(System.Object sender, System.EventArgs e)
        {

            Navigation.PushAsync(new DefaulterLanding());
        }
  
    }

    
}

