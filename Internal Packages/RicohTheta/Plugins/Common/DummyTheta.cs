using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAnax.Core.Plugins.Common {
	internal class DummyTheta : IRicohTheta {
		private string _platfrom;
		public DummyTheta() {
			_platfrom = Application.platform.ToString();
		}

		public void ConnectCamera(string ipAddress) {
			Debug.Log("ConnectCamera not support on current Platform");
		}

		public void DisconnectCamera() {
			Debug.Log("DisconnectCamera not support on current Platform");

		}

		public bool IsConnectedCamera() {
			Debug.Log("IsConnectedCamera not support on current Platform");
			return false;
		}

		public void CaptureImage(bool isUniqueFileName) {
			Debug.Log("CaptureImage not support on current Platform");
		}

		public void StartLiveStream() {
			Debug.Log("StartLiveStream not support on current Platform");
		}

		public void StopLiveStream() {
			Debug.Log("StartLiveStream not support on current Platform");
		}
	}
}