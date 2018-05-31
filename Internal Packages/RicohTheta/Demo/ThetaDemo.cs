using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

using GameAnax.Core.Extension;
using GameAnax.Core.Net;
using GameAnax.Core.Plugins;
using GameAnax.Core.Utility;



public class ThetaDemo : MonoBehaviour {

	[SerializeField]
	private Material thumbMate;
	[SerializeField]
	private Material preview360Mate;

	private Texture2D thumb;
	private Texture2D image360;

	// Use this for initialization
	//void Awake() { }
	//void Start() { }

	// Update is called once per frame
	//void Update() { }

	void OnEnable() {
		RicohThetaUnity.Me.CameraConncted += CameraConnctedListner;
		RicohThetaUnity.Me.CameraConnctionFail += CameraConnctionFailListner;
		RicohThetaUnity.Me.CameraDisconncted += CameraDisconnctedListner;

		RicohThetaUnity.Me.ImageCaptureSuccess += ImageCaptureSuccessListner;
		RicohThetaUnity.Me.ImageCaptureFail += ImageCaptureFailListner;
		RicohThetaUnity.Me.LiveCaptureStarted += LiveCaptureStartedListner;
		RicohThetaUnity.Me.LiveCaptureData += LiveCaptureDataListner;
		RicohThetaUnity.Me.LiveCaptureStoped += LiveCaptureStopedListner;
	}
	void OnDisable() {
		RicohThetaUnity.Me.CameraConncted -= CameraConnctedListner;
		RicohThetaUnity.Me.CameraConnctionFail -= CameraConnctionFailListner;
		RicohThetaUnity.Me.CameraDisconncted -= CameraDisconnctedListner;

		RicohThetaUnity.Me.ImageCaptureSuccess -= ImageCaptureSuccessListner;
		RicohThetaUnity.Me.ImageCaptureFail -= ImageCaptureFailListner;
		RicohThetaUnity.Me.LiveCaptureStarted -= LiveCaptureStartedListner;
		RicohThetaUnity.Me.LiveCaptureData -= LiveCaptureDataListner;
		RicohThetaUnity.Me.LiveCaptureStoped -= LiveCaptureStopedListner;
	}

	public void ConnectCamera() {
		RicohThetaUnity.Me.ConnectCamera("192.168.1.1");
	}
	public void DisconnectCamera() {
		RicohThetaUnity.Me.DisconnectCamera();
	}
	public void IsConnectedCamera() {
		bool isConnect = RicohThetaUnity.Me.IsConnectedCamera();
		MyDebug.Log("Ricoh Thtea Connection: {0}", isConnect);
	}
	public void OpenWiFiSettings() {
		NativeCodeUnity.Me.OpenWiFiSettings();
	}

	public void CaptureUniqueNameImage() {
		RicohThetaUnity.Me.CaptureImage(true);
	}
	public void CaptureNOverwriteImage() {
		RicohThetaUnity.Me.CaptureImage(false);
	}
	public void StartLiveStream() {
		RicohThetaUnity.Me.StartLiveStream();
	}
	public void StopLiveStream() {
		RicohThetaUnity.Me.StopLiveStream();
	}

	/// <summary>
	/// First string will be servier ID with port
	/// second string will session ID if available
	/// </summary>
	private void CameraConnctedListner(string ipaddress, string sesionID) {
		MyDebug.Log("Camera Connected to {0}, Seesion ID: {1}", ipaddress, sesionID);
	}
	private void CameraConnctionFailListner() {
		MyDebug.Log("Camera Connected Fail");
	}
	private void CameraDisconnctedListner() {
		MyDebug.Log("Camera Disconnected");
	}

	/// <summary>
	/// First string will be path of thumbnail file (from local device storage)
	/// second string will URL of 360 image (need download from Camera Storage)
	/// </summary>
	private void ImageCaptureSuccessListner(string thumbPath, string imageUrl) {
		MyDebug.Log("Image Captures Sucesfully, ThumbName: {0}, mainFile: {1}", thumbPath, imageUrl);

		thumb = new Texture2D(0, 0);
		image360 = new Texture2D(0, 0);

		Resources.UnloadUnusedAssets();

		if(!string.IsNullOrEmpty(thumbPath)) {
			if(File.Exists(thumbPath)) {
				byte[] timg = File.ReadAllBytes(thumbPath);
				thumb.LoadImage(timg);
				thumb = new Texture2D(thumb.width, thumb.height, TextureFormat.RGBA32, false);
				thumb.LoadImage(timg);
			} else {
				Debug.Log("No thumb File");
			}
		} else {
			Debug.Log("Empty thumb Path");
		}
		thumbMate.mainTexture = thumb;

		if(!string.IsNullOrEmpty(imageUrl)) {
			if(File.Exists(imageUrl)) {
				byte[] img360 = File.ReadAllBytes(imageUrl);
				image360.LoadImage(img360);
				//TextureScale.Bilinear(image360, 2048, 1024);
				//createNewImage(image360);
			} else {
				Debug.Log("No 360Image File");
			}
		} else {
			Debug.Log("Empty Image Path");
		}
		preview360Mate.mainTexture = image360;

	}
	private void ImageCaptureFailListner(string error) {
		MyDebug.Log("Image Capture Fail: ERROR: {0}", error);
	}

	private void LiveCaptureStartedListner() {
		MyDebug.Log("Live Capture Started");
	}
	private void LiveCaptureDataListner(Texture2D texture) {
		if(texture == null) return;
		preview360Mate.mainTexture = texture;
	}
	private void LiveCaptureStopedListner() {
		MyDebug.Log("Live Capture Stoped");
	}
}