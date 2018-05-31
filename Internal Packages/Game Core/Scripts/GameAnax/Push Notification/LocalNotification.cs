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

using GameAnax.Game.Enums;
using GameAnax.Game.Utility;

#region prime[31] iOS 'n' Android Manager Settings
//using Prime31;
#if UNITY_IOS
#if ETCETERA
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
	public class LocalNotification : Singleton<LocalNotification> {
#if UNITY_IOS
		UnityEngine.iOS.LocalNotification notif;
#endif
		//
		public PushNotificationInfo[] notifications;
		// Use this for initialization
		void Awake() {
			Me = this;
		}
		void Start() {
#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.RegisterForNotifications(
				NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);
#endif
			ClearLocalNotifications();
		}

		void OnApplicationPause(bool pauseStatus) {
			if(pauseStatus) {
				SecheduleLocalNotification();
			} else {
				ClearLocalNotifications();
			}
		}
		void OnApplicationQuit() {
			SecheduleLocalNotification();
		}
		//
		void ClearLocalNotifications() {
#if UNITY_IOS
			UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
			UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
#if ETCETERA
			EtceteraBinding.setBadgeCount(0);
#endif
#endif
#if UNITY_ANDROID && ETCETERA
			EtceteraAndroid.cancelAllNotifications();
#endif
		}
		void SecheduleLocalNotification() {
			int dealyInSeconds = 0;
			for(int i = 0; i < notifications.Length; i++) {
				switch(notifications[i].dealyType) {
				case FierDealy.Seconds:
					dealyInSeconds = notifications[i].dealy;
					break;
				case FierDealy.Mintes:
					dealyInSeconds = notifications[i].dealy * 60;
					break;
				case FierDealy.Hours:
					dealyInSeconds = notifications[i].dealy * 3600;
					break;
				case FierDealy.Days:
					dealyInSeconds = notifications[i].dealy * 86400;
					break;
				}
#if UNITY_IOS
				notif = new UnityEngine.iOS.LocalNotification();
				notif.fireDate = DateTime.Now.AddSeconds(dealyInSeconds);
				notif.alertBody = notifications[i].message;
				notif.applicationIconBadgeNumber = (i + 1);
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);
#endif
#if UNITY_ANDROID && ETCETERA
				string title = GameUtility.APPNAME;
				if(!string.IsNullOrEmpty(notifications[i].androidTitle)) {
					title = notifications[i].androidTitle;
				}
				AndroidNotificationConfiguration anc = new AndroidNotificationConfiguration(dealyInSeconds, title, notifications[i].message, "");
				anc.secondsFromNow = dealyInSeconds;
				anc.title = title;
				anc.subtitle = notifications[i].message;
				anc.tickerText = title;
				anc.smallIcon = "small_icon";
				anc.largeIcon = "small_icon";
				anc.extraData = "";
				anc.requestCode = 0;
				//int notifID = EtceteraAndroid.scheduleNotification(anc);
				EtceteraAndroid.scheduleNotification(anc);
#endif
			}
		}
	}

	[System.Serializable]
	public class PushNotificationInfo {
		public FierDealy dealyType = FierDealy.Hours;
		public int dealy = 2;
		public string message;
		[Space(5)]
		public string androidTitle;
	}
}
namespace GameAnax.Game.Enums {
	public enum FierDealy {
		Seconds,
		Mintes,
		Hours,
		Days
	}
}