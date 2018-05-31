using System;
using System.Collections;

using UnityEngine;

using GameAnax.Core.Singleton;

using GameAnax.Core.Plugins.Common;
using GameAnax.Core.Utility;


namespace GameAnax.Core.Plugins {
	[PersistentSignleton(true, true)]
	public class RicohThetaUnity : SingletonAuto<RicohThetaUnity>, IRicohTheta {
		private IRicohTheta _ricohThetaClient;

		#region Events
		/// <summary>
		/// First string will be servier ID with port
		/// second string will session ID if available
		/// </summary>
		public Action<string, string> CameraConncted;
		public Action CameraConnctionFail;
		public Action CameraDisconncted;

		/// <summary>
		/// First string will be path of thumbnail file (from local device storage)
		/// second string will URL of 360 image (need download from Camera Storage)
		/// </summary>
		public Action<string, string> ImageCaptureSuccess;
		/// <summary>
		/// Error Message if any recevied
		/// </summary>
		public Action<string> ImageCaptureFail;
		public Action LiveCaptureStarted;
		public Action<Texture2D> LiveCaptureData;
		public Action LiveCaptureStoped;


		#endregion
		private string camearAddress = string.Empty;

		Texture2D liveTexture;
		void Awake() {
			Me = this;

			_ricohThetaClient = RicohThetaClientFactory.BuildRicohThetaClient();
			liveTexture = new Texture2D(0, 0);
		}

		#region user access methods
		public void ConnectCamera(string ipAddress) {
			_ricohThetaClient.ConnectCamera(ipAddress);
		}
		public void DisconnectCamera() {
			_ricohThetaClient.DisconnectCamera();
		}
		public bool IsConnectedCamera() {
			return _ricohThetaClient.IsConnectedCamera();
		}
		public void CaptureImage(bool isUniqueFileName) {
			_ricohThetaClient.CaptureImage(isUniqueFileName);
		}
		public void StartLiveStream() {
			_ricohThetaClient.StartLiveStream();
		}
		public void StopLiveStream() {
			_ricohThetaClient.StopLiveStream();
		}
		#endregion

		#region data recevier from native code
		private void RicohThetaConnectionSuccessful(string camAddress) {
			Debug.Log("Connection Suceesful: " + camAddress);
			camearAddress = camAddress;
			OnCameraConncted(camAddress, "");
		}
		private void RicohThetaConnectionFailed() {
			Debug.Log("Connection Fail");
			camearAddress = string.Empty;
			OnCameraConnctionFail();
		}
		private void RicohThetaCameraDisconnected() {
			MyDebug.Log("Camera Disconnected");
			camearAddress = string.Empty;
			OnCameraDisconncted();
		}

		private void RicohThetaImageCaptured(string filepaths) {
			//{'thumbName':'','imageUrl':'100RICOH/R0010070.JPG'}

			Debug.Log("Here Path: " + filepaths);
			string imageUrl = string.Empty, thumbPath = string.Empty;
			IDictionary ht;
			string rawPath = Application.persistentDataPath + "/";

			ht = (IDictionary)MiniJSON.Json.Deserialize(filepaths);

			if(ht.Contains("imageUrl") && !string.IsNullOrEmpty(ht["imageUrl"].ToString())) {
				imageUrl = rawPath + ht["imageUrl"];
			}
			if(ht.Contains("thumbName") && !string.IsNullOrEmpty(ht["thumbName"].ToString())) {
				thumbPath = rawPath + ht["thumbName"];
			}
			OnImageCaptureSuccess(thumbPath, imageUrl);
		}
		private void RicohThetaImageCaptureFailed(string error) {
			Debug.Log("iamge capture process failed");
			OnImageCaptureFail(error);
		}

		private void RicohThetaStartedLiveStreaming() {
			Debug.Log("Started Live Streaming");
			OnLiveCaptureStart();
		}
		private void RicohThetaLiveFeedFailed(string error) { }
		private void RicohThetaLiveStreamingData(string base64) {
			byte[] imagebytes = System.Convert.FromBase64String(base64);
			if(imagebytes.Length > 0)
				liveTexture.LoadImage(imagebytes);

			OnLiveCaptureData(liveTexture);
		}
		private void RicohThetaStopLiveStreaming() {
			byte[] imagebytes = { };
			OnLiveCaptureStop();
			Debug.Log("Live Streaming Stoped");
		}
		#endregion

		#region Event Caller

		// First string will be servier ID with port
		// second string will session ID if available
		private void OnCameraConncted(string serverId, string sessionID) {
			if(CameraConncted != null) {
				CameraConncted.Invoke(serverId, sessionID);
			}
		}
		private void OnCameraConnctionFail() {
			if(CameraConnctionFail != null) {
				CameraConnctionFail.Invoke();
			}
		}
		private void OnCameraDisconncted() {
			if(CameraDisconncted != null) {
				CameraDisconncted.Invoke();
			}
		}
		// First string will be path of thumbnail file (from local device storage)
		// second string will URL of 360 image (need download from Camera Storage)
		private void OnImageCaptureSuccess(string thumbPath, string imageURL) {
			if(ImageCaptureSuccess != null) {
				ImageCaptureSuccess.Invoke(thumbPath, imageURL);
			}
		}
		private void OnImageCaptureFail(string error) {
			if(ImageCaptureFail != null) {
				ImageCaptureFail.Invoke(error);
			}
		}
		private void OnLiveCaptureStart() {
			if(LiveCaptureStarted != null) {
				LiveCaptureStarted.Invoke();
			}
		}
		private void OnLiveCaptureData(Texture2D liveT2D) {
			if(LiveCaptureData != null) {
				LiveCaptureData.Invoke(liveT2D);
			}
		}
		private void OnLiveCaptureStop() {
			if(LiveCaptureStoped != null) {
				LiveCaptureStoped.Invoke();
			}
		}
		#endregion

	}
}