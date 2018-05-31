using System;

using UnityEngine;

using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;
using GameAnax.Core.Plugins.Common;


namespace GameAnax.Core.Plugins {
	[PersistentSingleton(true, true)]
	public class NativeCodeUnity : SingletonAuto<NativeCodeUnity>, INativeCode {
		public event Action<string> AlertClicked;
		public event Action<string> LocalNotificationClicked;
		public event Action<string> RemoteNotificationClicked;
		public event Action<string> ReceiveNetworkMode;

		private INativeCode _nativeCodeClinet;
		protected override void Awake() {
			base.Awake();
			_nativeCodeClinet = NativeCodeClientFactory.BuildNativeCodeClient();
		}


		public void ShowActivityView() {
			_nativeCodeClinet.ShowActivityView();
		}
		public void GetNetworkMode() {
			_nativeCodeClinet.GetNetworkMode();
		}
		public void HideActivityView() {
			_nativeCodeClinet.HideActivityView();
		}
		public void ShowActivityWithImageRotation(string imgFile, float speed) {
			_nativeCodeClinet.ShowActivityWithImageRotation(imgFile, speed);
		}
		public void ShowActivityImageSequence(string imgFile, float speed) {
			_nativeCodeClinet.ShowActivityImageSequence(imgFile, speed);
		}
		public void ChangeImageSequence(string imgFile) {
			_nativeCodeClinet.ChangeImageSequence(imgFile);
		}
		public void ShowAlert(string title, string message, string positiveButton, string negativeButton) {
			_nativeCodeClinet.ShowAlert(title, message, positiveButton, negativeButton);
		}
		public void ShowAlert(string title, string message, string button) {
			_nativeCodeClinet.ShowAlert(title, message, button);
		}
		public void OpenReview(string appId) {
			_nativeCodeClinet.OpenReview(appId);
		}
		public void OpenURL(string url) {
			_nativeCodeClinet.OpenURL(url);
		}
		public bool CanSendMail() {
			return _nativeCodeClinet.CanSendMail();
		}
		public void OpenWiFiSettings() {
			_nativeCodeClinet.OpenWiFiSettings();
		}

		#region "Data Recevie Methods"
		void alertClicked(string args) {
			Log.l("alertClicked: {0}", args);
			if(AlertClicked != null) {
				AlertClicked.Invoke(args);
			}
		}
		#endregion

		#region "Local Notification Click"
		void localNotificationClicked(string args) {
			Log.l("localNotificationClicked: {0}", args);
			if(LocalNotificationClicked != null) {
				LocalNotificationClicked.Invoke(args);
			}
		}
		#endregion

		#region "Local Notification Click"
		void remoteNotificationClicked(string args) {
			Log.l("remoteNotificationClicked: {0}", args);
			if(RemoteNotificationClicked != null) {
				RemoteNotificationClicked.Invoke(args);
			}
		}
		#endregion

		#region "NetWork Mode Received
		void didReceiveNetworkMode(string args) {
			Log.l("didReceiveNetworkMode: {0}", args);
			if(ReceiveNetworkMode != null) {
				ReceiveNetworkMode.Invoke(args);
			}
		}
		#endregion
	}
}