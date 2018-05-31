using System.Collections.Generic;

namespace GameAnax.Core.Plugins.Common {
	public interface IRicohTheta {
		void ConnectCamera(string ipAddress);
		void DisconnectCamera();
		bool IsConnectedCamera();
		void CaptureImage(bool isUniqueFileName);
		void StartLiveStream();
		void StopLiveStream();
	}
}