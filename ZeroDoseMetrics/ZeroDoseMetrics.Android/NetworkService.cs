using System;
using Android.Net;
using Android.Content;


namespace ZeroDoseMetrics.Droid
{
	public class NetworkService:INetworkService
	{
        public bool IsNetworkAvailable()
        {
            var connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            return activeNetworkInfo != null && activeNetworkInfo.IsConnected;
        }

        public NetworkService()
		{

		}
	}
}




