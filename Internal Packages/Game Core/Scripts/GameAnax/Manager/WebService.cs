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

using System.Threading;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using GameAnax.Core;
using GameAnax.Core.Data;
using GameAnax.Core.Net;
using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;

using GameAnax.Game.Social;


namespace GameAnax.Game.Web {
	[PersistentSignleton(true, true)]
	public class WebService : Singleton<WebService> {
		#region URL declaration
		readonly string localhost = "http://127.0.0.1/";
		readonly string localStaging = "";
		readonly string staging = "";
		readonly string live = "";

		readonly string localhostPage = "http://127.0.0.1/";
		readonly string localStagingPage = "";
		readonly string stagingPage = "";
		readonly string livePage = "";

		string _apiURL = "";
		string _serverURL = "";
		#endregion
		[Space(10)]
		public Server server = Server.LocalHost;
		public string key = "";
		public WebMethod method = WebMethod.POST;
		public int timeout = 5000;
		// Use this for initialization
		void Awake() {
			Me = this;
		}
		void Start() {
			switch(server) {
			case Server.LocalHost:
				_serverURL = localhost;
				_apiURL = localhost + "/" + localhost;
				break;

			case Server.LocalStatging:
				_serverURL = localStaging;
				_apiURL = localStaging + "/" + localStagingPage;
				break;

			case Server.Staging:
				_serverURL = staging;
				_apiURL = staging + "/" + stagingPage;
				break;
			case Server.Live:
				_serverURL = live;
				_apiURL = live + "/" + livePage;
				break;

			default:
				Debug.LogError("Invalid Server");
				Debug.Break();
				break;
			}
			_apiURL = CoreUtility.Me.ConvertURL(_apiURL);
		}

#if WEBSERVICES
		void OnEnable() {
			FBService.Me.FacebookLoginEvent += FacebookLoginEListner;
			FBService.Me.FacebookLogoutEvent += FacebookLogoutEListner;
		}
		void OnDisable() {
			FBService.Me.FacebookLoginEvent -= FacebookLoginEListner;
			FBService.Me.FacebookLogoutEvent -= FacebookLogoutEListner;
		}

		void OnApplicationPause(bool isPause) {
#if FBUNITYSDK
			if(FBService.Me.IsValidFB()) {
#else
			{
#endif
				if(!isPause) {
					if(!SceneManager.GetActiveScene().name.Equals("MainMenu", System.StringComparison.OrdinalIgnoreCase)) {
						WBSetUserStatusThread(UserWBStatus.Busy);
					} else {
						WBSetUserStatusThread(UserWBStatus.Free);
					}
				} else {
					WBSetUserStatusThread(UserWBStatus.Offline);
				}
			}

		}
		void OnApplicationQuit() {
			if(FBService.Me.IsValidFB()) {
				WBSetUserStatusThread(UserWBStatus.Offline);
			}
		}
		//
		public IEnumerator StartSevices() {
			yield return StartCoroutine(CoreMethods.Wait(0f));

			WBLoginThread();
			WBSetUserStatusThread(UserWBStatus.Free);
		}
		//
		void FacebookLoginEListner() {
			WBLoginThread();
			WBSetUserStatusThread(UserWBStatus.Free);
		}
		void FacebookLogoutEListner(string message) {
			WBSetUserStatusThread(UserWBStatus.Offline);
		}
		//
		#region Web Services
		public void WBSetTokenThread() {
			Thread newThread = new Thread(WBSetToken);
			newThread.Start();
		}
		void WBSetToken() {
			MyDebug.Log("Add Token Web Service Start");
			string serverData;
			string userName = string.Empty;
#if FBUNITYSDK
			userName = Utility.Me.Settings.myFBInfo.name;
#endif
			userName = string.IsNullOrEmpty(userName) || userName.StartsWith("G_", System.StringComparison.OrdinalIgnoreCase) ? "Guest" : userName;
			WebData wd = new WebData();
			Dictionary<string, string> urlData;
			urlData = new Dictionary<string, string> {
			{ "key", key },
			{ "method", "AddDeviceToken" },
			{ "name",userName },
			{ "devicetoken", CoreUtility .Me.settings.pushToken },
			{ "platform", CoreUtility.Me.platform.ToLower() }
		};
			serverData = wd.GetDataAsString(_apiURL, timeout, method, urlData);
			MyDebug.Log("WBAddToken :: >>" + serverData);
		}

		public void WBSetUserStatusThread(UserWBStatus status) {
			Thread newThread = new Thread(() => WBSetUserStatus(status));
			newThread.Start();
		}
		void WBSetUserStatus(UserWBStatus status) {
			string serverData;
			WebData wd = new WebData();
			Dictionary<string, string> urlData;
			urlData = new Dictionary<string, string> {
			{ "key", key },
			{ "method", "UpdateUserStatus" },
			{ "fbid",CoreUtility.Me.settings.myFBInfo.id   },
			{ "userstatus", status.ToString().ToLower() }
		};
			serverData = wd.GetDataAsString(_apiURL, timeout, method, urlData);
			MyDebug.Log("SetUserStatus:: >> " + serverData);
		}

		public void WBLoginThread() {
			Thread newThread = new Thread(WBLogin);
			newThread.Start();
		}
		void WBLogin() {
			string serverData;
			WebData wd = new WebData();
			Dictionary<string, string> urlData;
			urlData = new Dictionary<string, string> {
			{ "key", key },
			{ "method", "FacebookLogin" },
			{ "fbid",CoreUtility.Me.settings.myFBInfo.id   },
			{ "fbaccesstoken", CoreUtility.Me.settings.myFBInfo.token },
			{ "platform", CoreUtility.Me.platform.ToLower() },
			{ "devicetoken",CoreUtility.Me.settings.pushToken }
		};
			serverData = wd.GetDataAsString(_apiURL, timeout, method, urlData);
			MyDebug.Log("SetFBLogin :: >>" + serverData);
		}
		#endregion

#endif
		public bool CheckGameVersion() {
			bool retVal = true;
			string serverData;
			string filePath;
			float liveVersion;
			WebData wd = new WebData();
			filePath = string.Format("{0}/{1}.txt", _serverURL, CoreUtility.Me.platform);
			//serverData = wd.GetDataAsString(filePath, timeout, method);
			//if(!string.IsNullOrEmpty(serverData)) {
			//	serverData = TextDataReader.GetDataFromFile(serverData, '\n', ' ')[0][0];
			//	float.TryParse(serverData, out liveVersion);
			//	retVal = CoreUtility.Me.appVersion >= liveVersion;
			//}
			return retVal;
		}

	}
	public enum Server {
		LocalHost = 0,
		LocalStatging = 1,
		Staging = 2,
		Live = 3
	}

	public enum UserWBStatus {
		Offline,
		Free,
		Busy
	}
}