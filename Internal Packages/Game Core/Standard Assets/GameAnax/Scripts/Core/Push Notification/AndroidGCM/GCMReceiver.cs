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

using UnityEngine;


namespace GameAnax.Core.Notification {
	public class GCMReceiver : MonoBehaviour {
		public static System.Action<string> onError;
		public static System.Action<string> onMessage;
		public static System.Action<string> onRegistered;
		public static System.Action<string> onUnregistered;
		public static System.Action<int> onDeleteMessages;

		void Awake() {
			// This receiver must not be destroyed on loading level
			DontDestroyOnLoad(transform.gameObject);
		}

		void OnError(string errorId) {
			Debug.Log("Error: " + errorId);
			if(onError != null) {
				onError(errorId);
			}
		}
		void OnMessage(string message) {
			Debug.Log("Message: " + message);
			if(onMessage != null) {
				onMessage(message);
			}
		}
		void OnRegistered(string registrationId) {
			Debug.Log("Registered: " + registrationId);
			if(onRegistered != null) {
				onRegistered(registrationId);
			}
		}
		void OnUnregistered(string registrationId) {
			Debug.Log("Unregistered: " + registrationId);
			if(onUnregistered != null) {
				onUnregistered(registrationId);
			}
		}
		void OnDeleteMessages(string total) {
			Debug.Log("DeleteMessages: " + total);
			if(onDeleteMessages != null) {
				int totalCnt = System.Convert.ToInt32(total);
				onDeleteMessages(totalCnt);
			}
		}
	}
}