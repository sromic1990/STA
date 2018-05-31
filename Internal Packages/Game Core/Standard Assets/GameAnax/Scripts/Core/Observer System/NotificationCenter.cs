//  
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	http://wiki.unity3d.com/index.php/NotificationCenter
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

using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;


namespace GameAnax.Core.NotificationSystem {
	[PersistentSignleton(true, true)]
	public class NotificationCenter : SingletonAuto<NotificationCenter> {
		Hashtable _listnersData = new Hashtable();
		IEnumerator DelayedFire(NotificationInfo info, float wait, bool isIgnoreTimeScale) {
			if(isIgnoreTimeScale) {
				yield return new WaitForSecondsRealtime(wait);
			} else {
				yield return new WaitForSeconds(wait);
			}
			this.PostNotification(info);
		}

		//
		public void AddObserver(Component observer, String name) {
			//MyDebug.Log("Function: " + name + ", added for " + observer.name);
			if(string.IsNullOrEmpty(name)) {
				MyDebug.Info("NotificationCenter::AddObserver => empty name specificed for method in AddListener.");
				return;
			}
			if(_listnersData.Contains(name) == false) {
				_listnersData[name] = new ArrayList();
			}

			ArrayList listnerList = (ArrayList)_listnersData[name];
			if(!listnerList.Contains(observer)) {
				listnerList.Add(observer);
			}
		}
		public void RemoveLister(Component observer, String name) {
			ArrayList listnerList = (ArrayList)_listnersData[name];

			if(null != listnerList) {
				if(listnerList.Contains(observer)) {
					listnerList.Remove(observer);
				}
				if(listnerList.Count == 0) {
					_listnersData.Remove(name);
				}
			}
		}
		//
		public void DelayedPostNotification(Component aSender, String aName, float wait) {
			StartCoroutine(DelayedFire(new NotificationInfo(aSender, aName, null), wait, true));
		}
		public void DelayedPostNotification(Component aSender, String aName, float wait, bool isIgnoreTimeScale) {
			StartCoroutine(DelayedFire(new NotificationInfo(aSender, aName, null), wait, isIgnoreTimeScale));
		}

		public void DelayedPostNotification(Component aSender, String aName, object aData, float wait) {
			StartCoroutine(DelayedFire(new NotificationInfo(aSender, aName, aData), wait, true));
		}
		public void DelayedPostNotification(Component aSender, String aName, object aData, float wait, bool isIgnoreTimeScale) {
			StartCoroutine(DelayedFire(new NotificationInfo(aSender, aName, aData), wait, isIgnoreTimeScale));
		}

		public void DelayedPostNotification(NotificationInfo info, float wait, bool isIgnoreTimeScale) {
			StartCoroutine(DelayedFire(info, wait, isIgnoreTimeScale));
		}
		public void DelayedPostNotification(NotificationInfo info, float wait) {
			StartCoroutine(DelayedFire(info, wait, true));
		}

		//
		public void PostNotification(Component aSender, String aName) {
			PostNotification(new NotificationInfo(aSender, aName, null));
		}
		public void PostNotification(Component aSender, String aName, Hashtable aData) {
			PostNotification(new NotificationInfo(aSender, aName, aData));
		}
		public void PostNotification(NotificationInfo info) {
			if(string.IsNullOrEmpty(info.method)) {
				MyDebug.Error("NotificationCenter::PostNotification => empty name sent to NotificationCenter.");
				return;
			}

			ArrayList listnerList = (ArrayList)_listnersData[info.method];

			if(null == listnerList && info.sender) {
				MyDebug.Warning("NotificationCenter::PostNotification => Method: " + info.method +
				" Sent by: " + info.sender.name + " not found in listners data.");
				return;
			} else if(null == listnerList) {
				MyDebug.Warning("NotificationCenter::PostNotification => Method: " + info.method +
				" not found in listners data.");
				return;
			}

			ArrayList observersToRemove = new ArrayList();
			foreach(Component listner in listnerList) {
				if(!listner) {
					observersToRemove.Add(listner);
				} else {
					listner.SendMessage(info.method, info.data, SendMessageOptions.DontRequireReceiver);
				}
			}

			foreach(object observer in observersToRemove) {
				listnerList.Remove(observer);
			}
		}
	}
}