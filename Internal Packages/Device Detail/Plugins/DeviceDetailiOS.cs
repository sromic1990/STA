using UnityEngine;

using System.Runtime.InteropServices;

using GameAnax.Core.Plugins.Common;
using GameAnax.Core.Singleton;


namespace GameAnax.Core.Plugins.iOS {
#if UNITY_IOS || UNITY_TVOS
	[PersistentSignleton(true, true)]
	public class DeviceDetailiOS : SingletonAuto<DeviceDetailiOS>, IDeviceDetail {
		void Awake() {
			Me = this;
		}

		[DllImport("__Internal")]
		private static extern void _getCountry();
		public void GetCountryName() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_getCountry();
		}

		[DllImport("__Internal")]
		private static extern void _getLanguageInfo();
		public void GetLanguage() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_getLanguageInfo();
		}

		[DllImport("__Internal")]
		private static extern void _getMACAddress();
		public void GetMAC() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_getMACAddress();
		}

		[DllImport("__Internal")]
		private static extern void _getIPAddress();
		public void GetIPAddress() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_getIPAddress();
		}
	}
#endif
}