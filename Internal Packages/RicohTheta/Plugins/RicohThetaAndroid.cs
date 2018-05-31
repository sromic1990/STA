using System;


using UnityEngine;

using GameAnax.Core.Utility;

using GameAnax.Core.Plugins.Common;

namespace GameAnax.Core.Plugins.Android {
#if UNITY_ANDROID
	public class RichoTheta : IRicohTheta {
		private string _platfrom;

		// store the main plugin class instance so we can make calls easily
		private AndroidJavaClass _javaClass = null;
		private AndroidJavaObject _javaObject = null;
		private AndroidJavaObject activityContext = null;

		public RichoTheta() {
			_platfrom = Application.platform.ToString();
			if(Application.platform != RuntimePlatform.Android) return;
#if MYDEBUG
			AndroidJNIHelper.debug = true;
#endif
			//Getting Current Contetxt from Launcher Activity
			MyDebug.Warning("Getting Current Activity form \"com.unity3d.player.UnityPlayer\"");
			if(activityContext == null) {
				using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
					activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
			}

			// find the plugin instance
			MyDebug.Log("Creating class object of Flurry Helper");
			_javaClass = new AndroidJavaClass("com.GameAnax.ricohTheta360Unity.RicohTheta360");
			Debug.Log("Getting Instance of the class");
			_javaObject = _javaClass.CallStatic<AndroidJavaObject>("Me");

			string s = (activityContext == null ? "null" : "not null");
			MyDebug.Log("Calling native method \"SetContext\" wiht activityContext == {0} ", s);
			_javaObject.Call("SetContext", activityContext);
			MyDebug.Log("native method \"SetContext\" executed");
		}

		#region user access methods
		public void ConnectCamera(string ipAddress) {
			if(string.IsNullOrEmpty(ipAddress))
				throw new MissingFieldException("Camera IP Address is Missing");
			if(Application.platform != RuntimePlatform.Android) return;

			MyDebug.Log("Trying to call Native Method for Connect 360 Camera on {0}", ipAddress);
			_javaObject.Call("ConnectCamera", ipAddress);
		}

		public void DisconnectCamera() {
			if(Application.platform != RuntimePlatform.Android) return;

			MyDebug.Log("Disconteting from Camera");
			_javaObject.Call("DisconnectCamera");
		}

		public bool IsConnectedCamera() {
			if(Application.platform != RuntimePlatform.Android) return false;

			MyDebug.Log("Checking for isConnected to Camera or not?");
			bool x = _javaObject.Call<bool>("IsConnectedCamera");
			return x;
		}

		public void CaptureImage(bool isUniqueFileName) {
			if(Application.platform != RuntimePlatform.Android) return;
			MyDebug.Log("Captureing Image, Will Overwrite old file? {0}", !isUniqueFileName);
			_javaObject.Call("CaptureImage", isUniqueFileName);
		}

		public void StartLiveStream() {
			if(Application.platform != RuntimePlatform.Android) return;

			MyDebug.Log("Starting Live Stream");
			_javaObject.Call("StartLiveStream");
		}

		public void StopLiveStream() {
			if(Application.platform != RuntimePlatform.Android) return;

			MyDebug.Log("Stoping Live Stream");
			_javaObject.Call("StopLiveStream");
		}

		#endregion
	}
#endif
}