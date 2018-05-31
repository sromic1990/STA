using GameAnax.Core.Utility;


namespace GameAnax.Core.Plugins.Common {
	public class DummyNativeCode : INativeCode {
		public void ShowActivityView() {
			Log.l("\"ShowActivityView\" is supported for current Platform");
		}
		public void GetNetworkMode() {
			Log.l("\"GetNetworkMode\" is supported for current Platform");
		}
		public void HideActivityView() {
			Log.l("\"HideActivityView\" is supported for current Platform");
		}
		public void ShowActivityWithImageRotation(string imgFile, float speed) {
			Log.l("\"ShowActivityWithImageRotation\" is supported for current Platform");
		}
		public void ShowActivityImageSequence(string imgFile, float speed) {
			Log.l("\"ShowActivityImageSequence\" is supported for current Platform");
		}
		public void ChangeImageSequence(string imgFile) {
			Log.l("\"ChangeImageSequence\" is supported for current Platform");
		}
		public void ShowAlert(string title, string message, string positiveButton, string negativeButton) {
			Log.l("\"ShowAlert\" is supported for current Platform");
		}
		public void ShowAlert(string title, string message, string button) {
			Log.l("\"ShowAlert\" is supported for current Platform");
		}
		public void OpenReview(string appId) {
			Log.l("\"OpenReview\" is supported for current Platform");
		}
		public void OpenURL(string url) {
			Log.l("\"OpenURL\" is supported for current Platform");
		}
		public bool CanSendMail() {
			Log.l("\"CanSendMail\" is supported for current Platform");
			return false;
		}
		public void OpenWiFiSettings() {
			Log.l("\"OpenWiFiSettings\" is supported for current Platform");
		}
	}
}