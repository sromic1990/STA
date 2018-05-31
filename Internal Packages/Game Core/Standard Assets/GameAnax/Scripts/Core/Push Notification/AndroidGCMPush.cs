//
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	N/A
// Last Modified: 	Ranpariya Ankur
// Contributed By:	N/A
// Curtosey By:		N/A
// 
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
// following conditions are met:
// 
//  *	Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//  	disclaimer.
//  *	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
//  	disclaimer in the documentation and/or other materials provided with the distribution.
//  *	Neither the name of the [ORGANIZATION] nor the names of its contributors may be used to endorse or promote products
//  	derived from this software without specific prior written permission.
// 
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
//

using System;

using UnityEngine;

using GameAnax.Core.Singleton;
using GameAnax.Core.JSonTools;
using GameAnax.Core.Utility;


namespace GameAnax.Core.Notification {
	public class AndroidGCMPush : SingletonAuto<AndroidGCMPush> {
#if PUSHNOTIFICATION && UNITY_ANDROID
		public event Action<string> ReceviedDeviceToken;
		public event Action<JsonObject> RemoteNotificationRecevied;

		void Awake() {
			MyDebug.Warning("AndroidGCMPush:Awake => GCM.Initialize ");
			GCM.Initialize();
		}
		void OnEnable() {
			GCMReceiver.onRegistered += OnRegistered;
			GCMReceiver.onMessage += OnMessageReceived;
			GCMReceiver.onUnregistered += OnUnregistered;
			GCMReceiver.onError += OnError;
			GCMReceiver.onDeleteMessages += OnDeleteMessages;
		}
		void OnDisable() {
			GCMReceiver.onRegistered -= OnRegistered;
			GCMReceiver.onMessage -= OnMessageReceived;
			GCMReceiver.onUnregistered -= OnUnregistered;
			GCMReceiver.onError -= OnError;
			GCMReceiver.onDeleteMessages -= OnDeleteMessages;

		}
		#region Method

		public static void GetGCMToken(string GCMSenderID) {
			MyDebug.Warning("AndroidGCMPush::GetGCMtoken for " + GCMSenderID);
			GCM.Register(new string[] { GCMSenderID });
		}

		public static void Unregister() {
			GCM.Unregister();
		}
		public static bool IsRegistered() {
			bool Isreg = GCM.IsRegistered();
			return Isreg;
		}
		public static string GetRegistrationId() {
			string _text = "GetRegistrationId = " + GCM.GetRegistrationId();
			return _text;
		}
		public static bool IsRegisteredOnServer() {
			bool isreg = GCM.IsRegisteredOnServer();
			return isreg;
		}
		public static void SetRegisteredOnServer(bool isRegistered) {
			GCM.SetRegisteredOnServer(isRegistered);
		}
		public static long GetRegisterOnServerLifespan() {
			long lifespan = GCM.GetRegisterOnServerLifespan();
			return lifespan;
		}
		public static void SetRegisterOnServerLifespan(long lifespan) {
			GCM.SetRegisterOnServerLifespan(lifespan);
		}

		#endregion

		#region Events / callbacks

		void OnRegistered(string finalToken) {
			MyDebug.Log("GSM Recevied from Native: " + finalToken);
			if(ReceviedDeviceToken != null) {
				ReceviedDeviceToken.Invoke(finalToken);
			}
		}
		void OnMessageReceived(string notidata) {
			MyDebug.Log("Noti Data: " + notidata);
			JsonObject not = Json.Deserialize(notidata) as JsonObject;
			if(not.ContainsKey("aps")) {
				string tmpAPS = not["aps"].ToString();
				tmpAPS = tmpAPS.TrimStart('"');
				tmpAPS = tmpAPS.TrimEnd('"');
				JsonObject finalAPS = (JsonObject)Json.Deserialize(tmpAPS);
				if(RemoteNotificationRecevied != null) {
					RemoteNotificationRecevied.Invoke(finalAPS);
				}
			}
		}
		void OnUnregistered(string registrationId) {
			Debug.Log("Unregistered: " + registrationId);
		}
		void OnError(string errorId) {
			Debug.Log("Error: " + errorId);
		}
		void OnDeleteMessages(int total) {
			Debug.Log("DeleteMessages: " + total);
		}

		#endregion


		//	public static event Action<string> ImageCropComplete;
		//
		//
		//	static AndroidJavaClass unityPlayer;
		//	static AndroidJavaObject adc;
		//	static AndroidJavaObject currentActivity;
		//
		//	void Start() {
		//		#if!UNITY_EDITOR
		//		//create unityPlayer class instance
		//		unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		//		adc = new AndroidJavaObject("com.example.unitysample.Bridge");
		//		//create the unity activity instance, to be passed in the plugin
		//		currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		//		#endif
		//	}
		//
		//	public static void ShowToast(string message) {
		//		#if !UNITY_EDITOR
		//		adc.CallStatic<string> ("showMessage", currentActivity, message);
		//		#endif
		//	}
#endif
	}
}