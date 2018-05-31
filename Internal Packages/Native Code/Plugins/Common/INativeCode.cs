using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnax.Core.Plugins.Common {
	public interface INativeCode {
		void ShowActivityView();
		void GetNetworkMode();
		void HideActivityView();
		void ShowActivityWithImageRotation(string imgFile, float speed);
		void ShowActivityImageSequence(string imgFile, float speed);
		void ChangeImageSequence(string imgFile);
		void ShowAlert(string title, string message, string positiveButton, string negativeButton);
		void ShowAlert(string title, string message, string button);
		void OpenReview(string appId);
		void OpenURL(string url);
		bool CanSendMail();
		void OpenWiFiSettings();
	}
}