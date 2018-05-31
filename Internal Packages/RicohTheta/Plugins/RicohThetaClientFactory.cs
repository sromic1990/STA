using GameAnax.Core.Plugins.Common;

namespace GameAnax.Core.Plugins {
	internal class RicohThetaClientFactory {
		internal static IRicohTheta BuildRicohThetaClient() {
#if UNITY_EDITOR
			return new GameAnax.Core.Plugins.Common.DummyTheta();
#elif UNITY_ANDROID
			return new GameAnax.Core.Plugins.Android.RichoTheta();
#elif UNITY_IPHONE
			return new GameAnax.Core.Plugins.iOS.RichoTheta(); ;
#else
			return new GameAnax.Core.Plugins.Common.DummyTheta();
#endif
		}
	}
}