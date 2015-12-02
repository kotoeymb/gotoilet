using System;
using System.Net;
using SystemConfiguration;
using CoreFoundation;

namespace OHouse.Connectivity
{
	public enum NetworkStatus {
		NotReachable,
		ReachableViaCarrierDataNetwork,
		ReachableViaWifiNetwork
	}

	public class ConnectionManager
	{
		public static string HostName = "www.google.com";
		public NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;

		public ConnectionManager ()
		{
		}

		public static bool isReachableWithoutRequiringConnection (NetworkReachabilityFlags flags)
		{
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0
			                            || (flags & NetworkReachabilityFlags.IsWWAN) != 0;

			return isReachable && noConnectionRequired;
		}

		public static bool IsHostReachable(string host) {
			if (string.IsNullOrEmpty (host)) {
				return false;
			}

			using (var r = new NetworkReachability (host)) {
				NetworkReachabilityFlags flags;

				if (r.TryGetFlags (out flags)) {
					return isReachableWithoutRequiringConnection (flags);
				}
					
			}

			return false;
		}

		public static event EventHandler ReachabilityChanged;

		static void OnChange(NetworkReachabilityFlags flags) {
			var h = ReachabilityChanged;
			if (h != null) {
				h (null, EventArgs.Empty);
			}
		}

		static NetworkReachability adHocWiFiNetworkReachability;
		public static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags) {
			if (adHocWiFiNetworkReachability == null) {
				adHocWiFiNetworkReachability = new NetworkReachability (new IPAddress (new byte[] { 169, 254, 0, 0 }));
				adHocWiFiNetworkReachability.SetNotification (OnChange);
				adHocWiFiNetworkReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			}

			return adHocWiFiNetworkReachability.TryGetFlags (out flags) && isReachableWithoutRequiringConnection (flags);
		}

		static NetworkReachability defaultRouteReachability;

		static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null) {
				defaultRouteReachability = new NetworkReachability(new IPAddress(0));
				defaultRouteReachability.SetNotification(OnChange);
				defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			return defaultRouteReachability.TryGetFlags(out flags) && isReachableWithoutRequiringConnection(flags);
		}
	
		static NetworkReachability remoteHostReachability;

		public static NetworkStatus RemoteHostStatus()
		{
			NetworkReachabilityFlags flags;
			bool reachable;
			if (remoteHostReachability == null) {
				remoteHostReachability = new NetworkReachability (HostName);

				reachable = remoteHostReachability.TryGetFlags (out flags);

				remoteHostReachability.SetNotification (OnChange);
				remoteHostReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
			} else {
				reachable = remoteHostReachability.TryGetFlags (out flags);
			}

			if (!reachable) {
				return NetworkStatus.NotReachable;
			}

			if (!isReachableWithoutRequiringConnection (flags)) {
				return NetworkStatus.NotReachable;
			}

			return (flags & NetworkReachabilityFlags.IsWWAN) != 0 ?
				NetworkStatus.ReachableViaCarrierDataNetwork : NetworkStatus.ReachableViaWifiNetwork;
		}

		public static NetworkStatus InternetConnectionStatus() {
			NetworkReachabilityFlags flags;

			bool defaultNetworkAvailable = IsNetworkAvailable (out flags);
			if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
				return NetworkStatus.NotReachable;
			else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				return NetworkStatus.ReachableViaCarrierDataNetwork;
			else if (flags == 0)
				return NetworkStatus.NotReachable;

			return NetworkStatus.ReachableViaWifiNetwork;
		}

		public static NetworkStatus LocalWifiConnectionStatus()
		{
			NetworkReachabilityFlags flags;
			if (IsAdHocWiFiNetworkAvailable(out flags))
			if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
				return NetworkStatus.ReachableViaWifiNetwork;

			return NetworkStatus.NotReachable;
		}

		/// <summary>
		/// Updates connection status.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
		}
	}
}

