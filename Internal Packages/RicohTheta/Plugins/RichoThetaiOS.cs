using UnityEngine;

using System.Runtime.InteropServices;

using GameAnax.Core.Utility;

using GameAnax.Core.Plugins.Common;


namespace GameAnax.Core.Plugins.iOS {
#if UNITY_IOS
	public class RichoTheta : IRicohTheta {
		private string _platfrom;
		public RichoTheta() {
			_platfrom = Application.platform.ToString();
		}

		[DllImport("__Internal")]
		extern static void _connectCamera(string ipAddress);

		[DllImport("__Internal")]
		extern static void _disconnectCamera();

		[DllImport("__Internal")]
		extern static int _isConnectedCamera();

		[DllImport("__Internal")]
		extern static void _captureImage(bool isUniqueFileName);

		[DllImport("__Internal")]
		extern static void _startLiveStream();

		[DllImport("__Internal")]
		extern static void _stopLiveStream();

	#region user access methods
		public void ConnectCamera(string ipAddress) {
			MyDebug.Log("Connecting Camera from C# - iOS to {0}", ipAddress);
			_connectCamera(ipAddress);
		}

		public void DisconnectCamera() {
			MyDebug.Log("Disconnecting Camera from C# - iOS");
			_disconnectCamera();
		}

		public bool IsConnectedCamera() {
			MyDebug.Log("Checking Camera Connection Status from C# - iOS");
			int i = _isConnectedCamera();
			return i == 1;
		}

		public void CaptureImage(bool isUniqueFileName) {
			Debug.Log("Captureing Image from C# - iOS");
			_captureImage(isUniqueFileName);
		}

		public void StartLiveStream() {
			Debug.Log("Start Live Stream from C# - iOS");
			_startLiveStream();
		}

		public void StopLiveStream() {
			Debug.Log("Stop Live Stream from C# - iOS");
			_stopLiveStream();
		}
	#endregion
	}
#endif
}