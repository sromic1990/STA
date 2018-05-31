//
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	Some Where from GitHUg
// Last Modified: 	Ranpariya Ankur
// Contributed By:	Andorid Team of Indianic Infotech (Nomesh Gour, Mihir, Bhaumik Shah)
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

using UnityEngine;

using GameAnax.Core.Utility;


namespace GameAnax.Core.Notification {
	public class GCM {
#if UNITY_ANDROID
		const string CLASS_NAME = "com.kskkbys.unitygcmplugin.UnityGCMRegister";
		const string NOTIFICATION_CLASS_NAME = "com.kskkbys.unitygcmplugin.UnityGCMNotificationManager";
		static GameObject _receiver = null;

		/// <summary>
		/// Initialize this plugin (Create receiver game object)
		/// </summary>
		public static void Initialize() {
			MyDebug.Warning("GCM::Initialize for " + Application.platform.ToString());
			if(_receiver == null) {
				_receiver = new GameObject("GCMReceiver");
				_receiver.AddComponent<GCMReceiver>();
			}
		}

		/// <summary>
		/// Register the specified senderIds.
		/// </summary>
		/// <param name='senderIds'>
		/// Sender identifiers.
		/// </param>
		public static void Register(params string[] senderIds) {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					string senderIdsStr = string.Join(",", senderIds);
					cls.CallStatic("register", senderIdsStr);
				}
			}
		}

		/// <summary>
		/// Unregister Android GCM
		/// </summary>
		public static void Unregister() {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					cls.CallStatic("unregister");
				}
			}
		}

		/// <summary>
		/// Determines whether this instance is registered.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is registered; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsRegistered() {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					return cls.CallStatic<bool>("isRegistered");
				}
			} else {
				return false;
			}
		}

		/// <summary>
		/// Gets the registration identifier.
		/// </summary>
		/// <returns>
		/// The registration identifier.
		/// </returns>
		public static string GetRegistrationId() {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					return cls.CallStatic<string>("getRegistrationId");
				}
			} else {
				return null;
			}
		}

		/// <summary>
		/// Sets the registered on server.
		/// </summary>
		/// <param name='isRegistered'>
		/// Is registered.
		/// </param>
		public static void SetRegisteredOnServer(bool isRegistered) {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					cls.CallStatic("setRegisteredOnServer", isRegistered);
				}
			}
		}

		/// <summary>
		/// Determines whether this instance is registered on server.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is registered on server; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsRegisteredOnServer() {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					return cls.CallStatic<bool>("isRegisteredOnServer");
				}
			} else {
				return false;
			}
		}

		/// <summary>
		/// Sets the register on server lifespan.
		/// </summary>
		/// <param name='lifespan'>
		/// Lifespan.
		/// </param>
		public static void SetRegisterOnServerLifespan(long lifespan) {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					cls.CallStatic("setRegisterOnServerLifespan", lifespan);
				}
			}
		}

		/// <summary>
		/// Gets the register on server lifespan.
		/// </summary>
		/// <returns>
		/// The register on server lifespan.
		/// </returns>
		public static long GetRegisterOnServerLifespan() {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(CLASS_NAME)) {
					return cls.CallStatic<long>("getRegisterOnServerLifespan");
				}
			} else {
				return 0L;
			}
		}


		// Setings for local notification

		/// <summary>
		/// Schedules the notification.
		/// </summary>
		/// <param name="sec">Sec.</param>
		/// <param name="contentTitle">Content title.</param>
		/// <param name="contentText">Content text.</param>
		public static void ScheduleLocalNotification(long sec, string contentTitle, string contentText) {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(NOTIFICATION_CLASS_NAME)) {
					cls.CallStatic("showNotification", sec, contentTitle, contentText);
				}
			}
		}

		/// <summary>
		/// Clears all notifications.
		/// </summary>
		public static void ClearAllNotifications() {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass(NOTIFICATION_CLASS_NAME)) {
					cls.CallStatic("clearAllNotifications");
				}
			}
		}
		/// <summary>
		/// Shows the toast.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void ShowToast(string message) {
			if(Application.platform == RuntimePlatform.Android) {
				using(AndroidJavaClass cls = new AndroidJavaClass("com.kskkbys.unitygcmplugin.Util")) {
					cls.CallStatic("showToast", message);
				}
			}
		}

#endif
	}
}