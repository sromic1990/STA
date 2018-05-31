using GameAnax.Core.Plugins.Common;

namespace GameAnax.Core.Plugins {
	public class DeviceDetailClientFactory {
		internal static IDeviceDetail BuildDeviceDetailClient() {
#if UNITY_EDITOR
			return new GameAnax.Core.Plugins.Common.DummyDeviceDetail();
#elif UNITY_ANDROID
			//return new GameAnax.Core.Plugins.Android.DeviceDetailAndroid();
			return new GameAnax.Core.Plugins.Common.DummyDeviceDetail();
#elif UNITY_IPHONE
			return new GameAnax.Core.Plugins.iOS.DeviceDetailiOS(); ;
#else
			return new GameAnax.Core.Plugins.Common.DummyDeviceDetails();
#endif
		}
	}
}