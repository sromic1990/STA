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

using UnityEngine.SceneManagement;

using GameAnax.Core.Singleton;
using GameAnax.Core.JSonTools;
using GameAnax.Core.Notification;
using GameAnax.Core.Utility;

using GameAnax.Game.Social;
using GameAnax.Game.Web;

using MiniJSON;

#region prime[31] iOS 'n' Android Manager Settings
#if ETCETERA
#if UNITY_IOS
using EtceteraB = Prime31.EtceteraBinding;
using EtceteraM = Prime31.EtceteraManager;
#endif
#if UNITY_ANDROID
using EtceteraB = Prime31.EtceteraAndroid;
using EtceteraM = Prime31.EtceteraAndroidManager;
#endif
#endif
#endregion


namespace GameAnax.Game.Notification {
	[PersistentSignleton(true, true)]
	public class RemoteNotiManager : Singleton<RemoteNotiManager> {
		public string AndroidGCMID;
		// Use this for initialization
		void Awake() {
			Me = this;
#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
#endif
		}
#if PUSHNOTIFICATION
		void Start() {
#if UNITY_ANDROID
		Invoke("GetGCMToken", 5f);
#endif

#if UNITY_IOS
#if ETCETERA
		EtceteraB.setBadgeCount(0);
#endif
			this.CheckPushON();
#endif
		}
		void OnEnable() {

#if UNITY_IOS
			iOSPush.Me.ReceviedDeviceToken += RemotePushTokenReceived;
			iOSPush.Me.RemoteNotificationiReceived += RemotePushReceived;
#endif
#if UNITY_ANDROID && !AMAZONSTORE
			AndroidGCMPush.Me.`ReceviedDeviceToken += RemotePushTokenReceived;
			AndroidGCMPush.Me.RemoteNotificationRecevied += RemotePushReceived;
#endif
		}
		void OnDisable() {

#if UNITY_IOS
			iOSPush.Me.ReceviedDeviceToken -= RemotePushTokenReceived;
			iOSPush.Me.RemoteNotificationiReceived -= RemotePushReceived;
#endif
#if UNITY_ANDROID && !AMAZONSTORE
			AndroidGCMPush.Me.ReceviedDeviceToken -= RemotePushTokenReceived;
			AndroidGCMPush.Me.RemoteNotificationRecevied -= RemotePushReceived;
		
#endif
		}

		#region Remote Push Notificaiton Related Methdo
		public void GetGCMToken() {
#if UNITY_ANDROID && !AMAZONSTORE
		AndroidGCMPush.GetGCMToken(AndroidGCMID);
#endif
		}
		public void RemotePushTokenReceived(string token) {
#if(UNITY_IOS || UNITY_ANDROID)
			MyDebug.Log("Token recevied to Game: " + token);
			if(!string.IsNullOrEmpty(token)) {
				CoreUtility.Me.settings.pushToken = token;
				CoreUtility.Me.SaveSettings();

#if WEBSERVICES
				WebService.Me.WBSetTokenThread();
				if(FBService.Me.IsValidFB()) {
					WebService.Me.WBLoginThread();
				}
#endif
			}
#endif
		}
		#endregion

		public void CheckPushON() {
#if UNITY_IOS && !UNITY_EDITOR
		if(!NativeBinding.IsPushOn()) {
			PopupsText.Me.PushOnMessage();
		}
#endif
		}

		void RemotePushReceived(JsonObject obj) {
#if UNITY_IOS
#if ETCETERA
		EtceteraB.setBadgeCount(0);
#endif
			UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
#endif
			//if (!Application.loadedLevelName.Equals ("MainMenu")) {
			if(SceneManager.GetActiveScene().name.StartsWith("MainMenu", System.StringComparison.OrdinalIgnoreCase)) {
				return;
			}

			MyDebug.Log("Push notification recevied, checking data");
			if(obj != null) {
				//MyDebug.Log(Json.Serialize(obj));
				if(obj.ContainsKey("action")) {
					JsonObject action = (JsonObject)obj["action"];
					if(action.ContainsKey("type") && action.ContainsKey("method")) {
						string notiMethod = action["method"].ToString().ToLower();
						switch(notiMethod) {
						case "challenge":
						case "response":
							break;
						}
					} else {
						MyDebug.Log("Push Notification has wrong data");
					}
				} else {
					MyDebug.Log("Push Notification has not action information");
				}
			} else {
				MyDebug.Log("Push Notification doesn't have any data");
			}
		}
#endif
	}
}