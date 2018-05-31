using GameAnax.Core.Utility;

namespace GameAnax.Core.Plugins.Common {
	public class DummyDeviceDetail : IDeviceDetail {
		public void GetCountryName() {
			Log.l("\"GetCountryName\" is supported for current platform");
		}
		public void GetLanguage() {
			Log.l("\"GetLanguage\" is supported for current platform");
		}
		public void GetMAC() {
			Log.l("\"GetMAC\" is supported for current platform");
		}
		public void GetIPAddress() {
			Log.l("\"GetIPAddress\" is supported for current platform");
		}
	}
}