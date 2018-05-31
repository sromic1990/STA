using System;

using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;
using GameAnax.Core.Plugins.Common;


namespace GameAnax.Core.Plugins {
	[PersistentSingleton(true, true)]
	public class DeviceDetailUnity : SingletonAuto<DeviceDetailUnity>, IDeviceDetail {
		public event Action<string> ReceviedCountry;
		public event Action<string, string, string> ReceviedLanguage;
		public event Action<string> ReceviedMAC;
		public event Action<string> ReceviedMACError;
		public event Action<string> ReceviedIP;

		private IDeviceDetail _deviceDetailClient;

		protected override void Awake() {
			base.Awake();
			_deviceDetailClient = DeviceDetailClientFactory.BuildDeviceDetailClient();
		}


		public void GetCountryName() {
			_deviceDetailClient.GetCountryName();
		}
		public void GetLanguage() {
			_deviceDetailClient.GetLanguage();
		}
		public void GetMAC() {
			_deviceDetailClient.GetMAC();
		}
		public void GetIPAddress() {
			_deviceDetailClient.GetIPAddress();
		}

		#region "Data Recevie Methods"

		void didRecevieCountry(string args) {
			args = GetDataFromXML.GetCountryInfo(args);
			if(ReceviedCountry != null) {
				ReceviedCountry.Invoke(args);
			}
		}

		void didReceiveLanguage(string args) {
			Log.l("DeviceManager::didReceiveLanguage => " + args);
			string locale = args.Split(',')[0];
			string languageCode = args.Split(',')[1];
			if(languageCode.Contains("-")) {
				languageCode = languageCode.Split('-')[0];
			}
			string language = GetDataFromXML.GetLanguageInfo(languageCode, LanguageISO.ISO_639_1, LanguageISO.English_Name);
			if(ReceviedLanguage != null) {
				ReceviedLanguage.Invoke(languageCode, language, locale);
			}
		}

		void didReceiveMAC(string args) {
			if(ReceviedMAC != null) {
				ReceviedMAC.Invoke(args);
			}
		}

		void didReceiveMACError(string args) {
			if(ReceviedMACError != null) {
				ReceviedMACError.Invoke(args);
			}
		}

		void didReceiveIP(string args) {
			if(ReceviedIP != null) {
				ReceviedIP.Invoke(args);
			}
		}
		#endregion
	}
}