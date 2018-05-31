using UnityEngine;

using System.Runtime.InteropServices;

using GameAnax.Core.Plugins.Common;
using GameAnax.Core.Singleton;

namespace GameAnax.Core.Plugins.iOS {
#if UNITY_IOS || UNITY_TVOS
	[PersistentSignleton(true, true)]
	public class NativeCodeiOS : SingletonAuto<NativeCodeiOS>, INativeCode {
		void Awake() {
			Me = this;
		}

		[DllImport("__Internal")]
		private static extern void _showActivityView();
		public void ShowActivityView() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_showActivityView();

		}

		[DllImport("__Internal")]
		private static extern void CheckNetworkStatus();
		public void GetNetworkMode() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			CheckNetworkStatus();

		}

		[DllImport("__Internal")]
		private static extern void _hideActivityView();
		public void HideActivityView() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_hideActivityView();

		}

		[DllImport("__Internal")]
		private static extern void _showSingleImage(string imgFile, float speed);
		public void ShowActivityWithImageRotation(string imgFile, float speed) {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_showSingleImage(imgFile, speed);

		}

		[DllImport("__Internal")]
		private static extern void _showImageSequence(string imgFile, float speed);
		public void ShowActivityImageSequence(string imgFile, float speed) {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_showImageSequence(imgFile, speed);

		}

		[DllImport("__Internal")]
		private static extern void _changeImageSequence(string imgFile);
		public void ChangeImageSequence(string imgFile) {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_changeImageSequence(imgFile);

		}


		[DllImport("__Internal")]
		private static extern void _showTwoButtonAlert(string title, string message, string button1, string button2);
		public void ShowAlert(string title, string message, string positiveButton, string negativeButton) {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_showTwoButtonAlert(title, message, positiveButton, negativeButton);

		}

		[DllImport("__Internal")]
		private static extern void _showSingleButtonAlert(string title, string message, string button);
		public void ShowAlert(string title, string message, string button) {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_showSingleButtonAlert(title, message, button);

		}

		[DllImport("__Internal")]
		private static extern void _openReview(string appId);
		public void OpenReview(string appId) {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_openReview(appId);
		}

		[DllImport("__Internal")]
		private static extern void _openURL(string url);
		public void OpenURL(string url) {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			_openURL(url);
		}


		[DllImport("__Internal")]
		private static extern int _canSendMail();
		public bool CanSendMail() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return false;
			return _canSendMail() == 1;
		}

		[DllImport("__Internal")]
		private static extern void openWifiSettings();
		public void OpenWiFiSettings() {
			if(Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.tvOS) return;
			openWifiSettings();
		}

	}
#endif
}