using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ZeroDoseMetrics.Model
{
	public class NetworkMonitor
	{

		private Action _onNetworkAvailable;


        public NetworkMonitor(Action onNetworkAvailable)
		{
            _onNetworkAvailable = onNetworkAvailable;
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _onNetworkAvailable?.Invoke();
                });
            }
        }


        public void Start()
        {
            // Check current network status
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _onNetworkAvailable?.Invoke();
                });
            }
        }

        public void Stop()
        {
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }
    }
}

