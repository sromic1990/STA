using GameAnax.Core.Plugins.Common;


namespace GameAnax.Core.Plugins {
	public class NativeCodeClientFactory {
		internal static INativeCode BuildNativeCodeClient() {
#if UNITY_EDITOR
			return new GameAnax.Core.Plugins.Common.DummyNativeCode();
#elif UNITY_ANDROID
			//return new GameAnax.Core.Plugins.Android.NativeCodeAndroid();
			return new GameAnax.Core.Plugins.Common.DummyNativeCode();
#elif UNITY_IPHONE
			return new GameAnax.Core.Plugins.iOS.NativeCodeiOS(); ;
#else
			return new GameAnax.Core.Plugins.Common.DummyNativeCode();
#endif
		}
	}
}
