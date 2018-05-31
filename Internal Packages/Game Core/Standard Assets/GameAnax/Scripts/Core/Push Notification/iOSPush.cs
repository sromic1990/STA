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
using System.Collections;

using UnityEngine;
using UnityEngine.iOS;

using GameAnax.Core.Singleton;
using GameAnax.Core.JSonTools;


namespace GameAnax.Core.Notification {
	public class iOSPush : SingletonAuto<iOSPush> {
#if PUSHNOTIFICATION && UNITY_IOS
		bool tokenSent = false;
		public event Action<string> ReceviedDeviceToken;
		public event Action<JsonObject> RemoteNotificationiReceived;

		void Start() {
			//MyDebug.Log("Start: iOSPush: ");
			UnityEngine.iOS.NotificationServices.RegisterForNotifications(
				NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
			InvokeRepeating("CheckTokenSent", 5, 15f);
		}

		void CheckTokenSent() {
			if(tokenSent) {
				CancelInvoke("CheckTokenSent");
				return;
			}

			//MyDebug.Log("Fetching iOS Token: ");
			byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
			//MyDebug.Log("iOS Token process complete: ");

			if(token != null) {
				//MyDebug.Log("iOS Token in native Style " + token);
				//send token to a provider
				string finalToken = System.BitConverter.ToString(token).Replace("-", "");
				if(ReceviedDeviceToken != null) {
					ReceviedDeviceToken.Invoke(finalToken);
				}
				tokenSent = true;
			} else {
				//MyDebug.Log("iOS token is null: " );
			}
		}

		void DeviceTokenRecevied(string finalToken) {
			//MyDebug.Log("iOS token Recevied from Native: " + finalToken);
			if(ReceviedDeviceToken != null) {
				ReceviedDeviceToken.Invoke(finalToken);
			}
			tokenSent = true;
		}

		void InGameNotificationReceived(string notidata) {
			JsonObject not = Json.Deserialize(notidata) as JsonObject;
			if(not.ContainsKey("aps")) {
				JsonObject notData = not["aps"] as JsonObject;
				if(RemoteNotificationiReceived != null) {
					RemoteNotificationiReceived.Invoke(notData);
				}
			}
		}
#endif
	}
}