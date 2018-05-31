namespace GameAnax.Core.Plugins.Common {
	public interface IDeviceDetail {
		void GetCountryName();
		void GetLanguage();
		void GetMAC();
		void GetIPAddress();
	}
}