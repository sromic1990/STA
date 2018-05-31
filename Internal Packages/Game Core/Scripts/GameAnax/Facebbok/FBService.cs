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
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using GameAnax.Core;
using GameAnax.Core.UI.Buttons;
using GameAnax.Core.Extension;
using GameAnax.Core.IO;
using GameAnax.Core.JSonTools;
using GameAnax.Core.NotificationSystem;
using GameAnax.Core.Singleton;
using GameAnax.Core.Social;
using GameAnax.Core.Utility;

using GameAnax.Core.Delegates;
using GameAnax.Game.Enums;
using GameAnax.Game.Utility;
using GameAnax.Game.Utility.Popup;

using MiniJSON;

#if FBUNITYSDK
using Facebook.Unity;
#endif

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

#if SOCIALNETWORKING
using TwitterM = Prime31.TwitterManager;
#if UNITY_IOS
using TwitterB = Prime31.TwitterBinding;
#endif
#if UNITY_ANDROID
using TwitterB = Prime31.TwitterAndroid;
#endif
#endif
#endregion


namespace GameAnax.Game.Social {
	[PersistentSignleton(true, true)]
	public class FBService : Singleton<FBService> {
		public event FBActionSucess FacebookLoginEvent;
		public event FBActionFail FacebookLogoutEvent;

		public event FBActionSucess FBMyScoreReceivedEvent;
		public event FBActionFail FBMyScoreFailEvent;

		public event FBActionSucess FBFriendReceivedEvent;
		public event FBActionFail FBFriendFailEvent;

		public event FBActionFail FBFriendScoreComleteEvent;


		string[] readPermission = { "email", "user_friends" };
		string[] publishPermission = { "publish_actions" };

		string shareMessageStatus;
		string shareMessageTitle;
		string shareText, shareURL;
		FBAction myFBAction = FBAction.None;
		FBAction fbActualAction = FBAction.None;

		[HideInInspector]
		public List<FBUser> MyFBFriends;
		public string FBAppID;
		public string TwitterAPIKey;
		public string TwitterAPISec;

		#region Mono Action
		// Use this for initialization
		void Awake() {
			Me = this;
		}

		void Start() {
#if FBUNITYSDK
		FB.Init(OnFBInit, OnHideUnity);
		status = "FB.Init() called with " + FB.AppId;
#endif

#if UNITY_IOS
#if SOCIALNETWORKING
			if(!string.IsNullOrEmpty(TwitterAPIKey) && !string.IsNullOrEmpty(TwitterAPISec)) {
				TwitterB.init(TwitterAPIKey, TwitterAPISec);
			}
#endif
#if ETCETERA
			EtceteraB.setBadgeCount(0);
#endif
			UnityEngine.iOS.NotificationServices.ClearRemoteNotifications();
#endif

#if UNITY_ANDROID
#if SOCIALNETWORKING
		if(!string.IsNullOrEmpty(TwitterAPIKey) && !string.IsNullOrEmpty(TwitterAPISec)) {
			TwitterB.init(TwitterAPIKey, TwitterAPISec);
		}
#endif
#endif
		}

		void OnEnable() {
#if SOCIALNETWORKING
#if UNITY_IOS
			Prime31.SharingManager.sharingFinishedWithActivityTypeEvent += OnSharingSuccessed;
			Prime31.SharingManager.sharingCancelledEvent += OnSharingFailed;
#endif

			//Twiiter Events
			TwitterM.loginSucceededEvent += TWloginSucceededEvent;
			TwitterM.loginFailedEvent += TWLoginFailedEvent;
			TwitterM.requestDidFinishEvent += TWRequestDidFinishEvent;
			TwitterM.requestDidFailEvent += TWRequestDidFailEvent;

#if UNITY_IOS
			TwitterM.tweetSheetCompletedEvent += TWTweetSheetCompletedEvent;
#endif

#if UNITY_ANDROID
		TwitterM.twitterInitializedEvent += TWTwitterInitializedEvent;
#endif

#endif
		}

		void OnDisable() {
#if SOCIALNETWORKING
#if UNITY_IOS
			Prime31.SharingManager.sharingFinishedWithActivityTypeEvent -= OnSharingSuccessed;
			Prime31.SharingManager.sharingCancelledEvent -= OnSharingFailed;
#endif

			//Twiiter Events
			TwitterM.loginSucceededEvent -= TWloginSucceededEvent;
			TwitterM.loginFailedEvent -= TWLoginFailedEvent;
			TwitterM.requestDidFinishEvent -= TWRequestDidFinishEvent;
			TwitterM.requestDidFailEvent -= TWRequestDidFailEvent;
#if UNITY_IOS
			TwitterM.tweetSheetCompletedEvent -= TWTweetSheetCompletedEvent;
#endif

#if UNITY_ANDROID
		TwitterM.twitterInitializedEvent -= TWTwitterInitializedEvent;
#endif

#endif
		}

		void OnApplicationPause(bool isPause) {
		}

		#endregion


		public IEnumerator CheckFB() {
			if(IsValidFB()) {
				myFBAction = FBAction.None;
				fbActualAction = FBAction.None;
#if FBUNITYSDK
			MyFBInfo();
#endif
				yield break;
			}
		}
		public void GetFBMP() {
			fbActualAction = FBAction.CheckFBMP;

#if FBUNITYSDK
		FBLogin();
#endif
		}
		IEnumerator DownloandMyImage(string url = "", string fbid = "") {
			string location = File.DataPath() + "ProfilePic/";
			string fileName, imageID, fbImgUrl;

			imageID = fbid;
			fbImgUrl = url;

			fileName = imageID + ".jpg";

			if(!string.IsNullOrEmpty(fbImgUrl)) {
				if(fbImgUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
					fbImgUrl = fbImgUrl.Replace("https://", "http://");
				}
				WWW www = new WWW(fbImgUrl);
				yield return www;
				if(string.IsNullOrEmpty(www.error)) {
					byte[] fileData = www.bytes;
					File.WriteFile(location, fileName, fileData);
					yield return StartCoroutine(CoreMethods.Wait(1f));
				} else {
					MyDebug.Log("error in downlaod image ERROR: " + www.error);
				}
			} else {
				MyDebug.Log("File no exists and no photo url");
			}
		}
		void SetLastLayer() {
			if(CoreMethods.layer.Contain(Menus.Social)) {
				CoreMethods.layer = CoreMethods.lastLayer;
			}
			if(myFBAction.Equals(FBAction.CheckFBMP) || fbActualAction.Equals(FBAction.CheckFBMP)) {
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "ShowMultiplayer",
					new ButtonEventArgs()));
			}

			myFBAction = FBAction.None;
			fbActualAction = FBAction.None;
			myTWAction = TWAction.None;
		}

		public Dictionary<string, object> GetFinalDict(JsonObject rawData) {
			Dictionary<string, object> tDict;
			string jcode;

			jcode = Json.Serialize(rawData);
			tDict = new Dictionary<string, object>();
			IDictionary dictionary = Json.Deserialize(jcode) as IDictionary;
			CoreMethods.ReGenHashJsonTable(dictionary, ref tDict, "");
			return tDict;
		}
		public FBUser GetUserByID(string fbID) {
			FBUser fbuser = null;
			if(CoreUtility.Me.settings.myFBInfo.id.Equals(fbID)) {
				fbuser = CoreUtility.Me.settings.myFBInfo;
			} else {
				MyFBFriends.ForEach(fbFri => {
					if(fbFri.id.Equals(fbID)) {
						fbuser = fbFri;
					}
				});
			}
			return fbuser;
		}

		#region Facbook

		//

		#region FB Unity SDK

		string status;
		string lastResponse;
		int fbFriendIndex;
#if FBUNITYSDK
	int tryScore;
	bool isGetAllFriendScore;
	void OnFBInit() {
		FB.ActivateApp();
	}

	void OnHideUnity(bool isGameShown) {
		status = "Success - Check logk for details";
		lastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
		MyDebug.Log("Is game shown: " + isGameShown);
	}

	void PreProcessHandler(string method, IResult result, ref bool isSucess, ref string rawResult) {

		if(result == null) {
			lastResponse = "Response is null";
			rawResult = "null";
			SetLastLayer();
		} else if(!string.IsNullOrEmpty(result.Error)) {
			status = "Error - Check log for details";
			lastResponse = "Response with ERROR: " + result.Error;
			rawResult = "ERROR: " + result.Error;
		} else if(result.Cancelled) {
			status = "Cancelled - Check log for details";
			lastResponse = "Response: Cancelled by user";
		} else if(!string.IsNullOrEmpty(result.RawResult)) {
			isSucess = true;
			status = "Success";
			lastResponse = "Success Response:\n" + result.RawResult;
			rawResult = result.RawResult;
		} else {
			lastResponse = "Empty Response\n";
			rawResult = "Unknown or Empty Response";
		}
		//
		if(result != null && !isSucess) {
			MyDebug.Log(Json.encode(result));
		}
		//
		MyDebug.Log("Method: {0}, Status: {1}, Response: {2}\nError: {3}\nRAW Data: {4}",
			method, status, lastResponse, result == null ? "null" : result.Error,
			result == null ? "null" : result.RawResult
		);
	}

	protected void OnLoginHandler(IResult result) {
		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnLoginHandler", result, ref isSucess, ref rawResult);

		//		if(result == null) {
		//			lastResponse = "Null Response\n";
		//			//MyDebug.Log(lastResponse);
		//			SetLastLayer();
		//		} else if(!string.IsNullOrEmpty(result.Error)) {
		//			status = "Error - Check log for details";
		//			lastResponse = "Error Response:\n" + result.Error;
		//			//MyDebug.Log(result.Error);
		//		} else if(result.Cancelled) {
		//			status = "Cancelled - Check log for details";
		//			lastResponse = "Cancelled Response:\n" + result.RawResult;
		//			//MyDebug.Log("RawResult :: " + result.RawResult);
		//		} else if(!string.IsNullOrEmpty(result.RawResult)) {
		//			isSucess = true;
		//			status = "Success - Check log for details";
		//			lastResponse = "Success Response:\n" + result.RawResult;
		//			//MyDebug.Log(result.RawResult);
		//		} else {
		//			lastResponse = "Empty Response\n";
		//			//MyDebug.Log(lastResponse);
		//		}
		//		//
		//		if(result != null && !isSucess) {
		//			MyDebug.Log(Json.encode(result));
		//		}

		if(isSucess) {
			//JsonObject rs = (JsonObject)Json.decode(result.RawResult);
			JsonObject rs = (JsonObject)Json.decode(rawResult);
			GUtility.Me.PProgress.MyFBInfo.Token = rs["access_token"].ToString();
			MyDebug.Log("FB Access token is: " + GUtility.Me.PProgress.MyFBInfo.Token);
			GUtility.Me.SavePProgres();

			ProcessLoginPermission(rs);
			StartCoroutine(ActionOnPermission());
		} else {
			PopupsText.Me.FacebookLoginFailMessage();
			CancelLogin();
		}
	}

	void ProcessLoginPermission(JsonObject rs) {
		List<string> permisson = new List<string>();

		if(rs != null) {
			string jsonKey;// = "granted_permissions";
#if UNITY_IOS
			jsonKey = "granted_permissions";
#endif
#if UNITY_ANDROID
			jsonKey = "permissions";
#endif
			if(rs.ContainsKey(jsonKey)) {
#if UNITY_IOS
				JsonArray datas = (JsonArray)rs[jsonKey];
#endif
#if UNITY_ANDROID
				string[] datas = rs[jsonKey].ToString().Split(',');
#endif
				foreach(object o in datas) {
					permisson.Add(o.ToString());
				}
				if(permisson.Count > 0) {
					MyDebug.Log("{0} granted permission found", permisson.Count);
					GUtility.Me.PProgress.MyFBInfo.Permissions.Clear();
					GUtility.Me.PProgress.MyFBInfo.Permissions.AddRange(permisson.ToArray());
					GUtility.Me.SavePProgres();
				}
			} else
				MyDebug.Log("Data array not contians in result");
		} else
			MyDebug.Log("result is null");
	}

	protected void OnPostHandler(IResult result) {
		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnPostHandler", result, ref isSucess, ref rawResult);

		if(result == null) {
			lastResponse = "Null Response\n";
			//MyDebug.Log(lastResponse);
			shareMessageStatus = "Facebook posting failed.\nPlease try after some time.";
		} else if(!string.IsNullOrEmpty(result.Error)) {
			status = "Error - Check log for details";
			lastResponse = "Error Response:\n" + result.Error;
			//MyDebug.Log(result.Error);
			shareMessageStatus = "Facebook posting failed.\nPlease try after some time.";
		} else if(result.Cancelled) {
			status = "Cancelled - Check log for details";
			lastResponse = "Cancelled Response:\n" + result.RawResult;
			//MyDebug.Log(result.RawResult);
			shareMessageStatus = "You Cancelled facebook posting.";
		} else if(!string.IsNullOrEmpty(result.RawResult)) {
			status = "Success - Check log for details";
			lastResponse = "Success Response:\n" + result.RawResult;
			//MyDebug.Log(result.RawResult);
#if UNITY_IOS
			isSucess = result.RawResult.Contains("postId");
#endif
#if UNITY_ANDROID
			isSucess = result.RawResult.Contains("id");
			if(!isSucess) {
				shareMessageStatus = "You Cancelled facebook posting.";
			}
#endif
		} else {
			lastResponse = "Empty Response\n";
			//MyDebug.Log(lastResponse);
		}
		//
		if(result != null && !isSucess) {
			MyDebug.Log(Json.encode(result));
		}
		//
		if(isSucess) {
			shareMessageStatus = "Successfully posted on your facebook wall.";
			if(GUtility.Me.FBShareCoin > 0) {
				TimeSpan ts = DateTime.UtcNow - GUtility.Me.PProgress.LastFBShare;
				if(ts.TotalHours >= 24 && GUtility.Me.CShareMode.Equals(ShareMode.Game)) {
					GUtility.Me.PProgress.LastFBShare = DateTime.UtcNow;
					GUtility.Me.AddInCoin(GUtility.Me.FBShareCoin);
					GUtility.Me.SavePProgres();
					shareMessageStatus += "You have got #" + GUtility.Me.FBShareCoin + "\nfrom Facebook Share";
				} else if(ts.TotalHours < 24) {
					shareMessageStatus +=  "Please try after\nfew hour to get free coins";
				}
			}
		} else {
			MyDebug.Log("FBNWebService::FBShareDialogFailedEvent => Facebook Posting Fail,");
			shareMessageStatus = "Facebook posting failed. Please try after some time.";
		}
		PopupMessages.Me.ShareSuccessMessage(shareMessageStatus);
		SetLastLayer();
	}

	protected void OnGetMyInfoHandler(IResult result) {
		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnGetMyInfoHandler", result, ref isSucess, ref rawResult);

		if(result == null) {
			lastResponse = "Null Response\n";
			//MyDebug.Log(lastResponse);
		} else if(!string.IsNullOrEmpty(result.Error)) {
			status = "Error - Check log for details";
			lastResponse = "Error Response:\n" + result.Error;
			//MyDebug.Log(result.Error);
		} else if(result.Cancelled) {
			status = "Cancelled - Check log for details";
			lastResponse = "Cancelled Response:\n" + result.RawResult;
			//MyDebug.Log(result.RawResult);
		} else if(!string.IsNullOrEmpty(result.RawResult)) {
			status = "Success - Check log for details";
			lastResponse = "Success Response:\n" + result.RawResult;
			isSucess = true;
			//MyDebug.Log(result.RawResult);
		} else {
			lastResponse = "Empty Response\n";
			//MyDebug.Log(lastResponse);
		}
		//
		if(result != null && !isSucess) {
			MyDebug.Log(Json.encode(result));
		}
		//
		if(isSucess) {
			MyDebug.Log("My info Success");
			StartCoroutine(ProcessMyInfo(result.RawResult));
		} else {
			MyDebug.Log("St: {0}\nLastM:{1}\nError:{2}\nRAW Data:{3}",
				status, lastResponse, result == null ? "" : result.Error, result == null ? "" : result.RawResult
			);
			CancelLogin();
			PopupsText.Me.GetMyInfoFailMessage();
		}

	}

	protected IEnumerator ProcessMyInfo(string result) {
		MyDebug.Log("My info Success and parsing");
		Dictionary<string, object> finalDict = new Dictionary<string, object>();
		IDictionary dictionary = Json.decode(result) as IDictionary;
		GameEngine.ReGenHashJsonTable(dictionary, ref finalDict, "");


		GUtility.Me.PProgress.MyFBInfo.ID = finalDict["id"].ToString();
		if(finalDict.ContainsKey("name")) {
			GUtility.Me.PProgress.MyFBInfo.Name = finalDict["name"].ToString();
		} else if(finalDict.ContainsKey("first_name")) {
			GUtility.Me.PProgress.MyFBInfo.Name = finalDict["first_name"].ToString();
		}

		if(finalDict.ContainsKey("gender")) {
			GUtility.Me.PProgress.MyFBInfo.Gender = finalDict["gender"].ToString();
		}
		if(finalDict.ContainsKey("email")) {
			GUtility.Me.PProgress.MyFBInfo.EMail = finalDict["email"].ToString();
		}

		if(finalDict.ContainsKey("verified")) {
			bool.TryParse(finalDict["verified"].ToString(), out GUtility.Me.PProgress.MyFBInfo.IsVerified);
		}
		if(finalDict.ContainsKey("installed")) {
			bool.TryParse(finalDict["installed"].ToString(), out GUtility.Me.PProgress.MyFBInfo.Installed);
		}
		if(finalDict.ContainsKey("install_type")) {
			GUtility.Me.PProgress.MyFBInfo.InstallType = finalDict["install_type"].ToString();
		}
		if(finalDict.ContainsKey("picture:data:url")) {
			GUtility.Me.PProgress.MyFBInfo.PhotoURL = finalDict["picture:data:url"].ToString();
		}
		if(finalDict.ContainsKey("link")) {
			GUtility.Me.PProgress.MyFBInfo.ProfileURL = finalDict["link"].ToString();
		}
		if(finalDict.ContainsKey("age_range:min")) {
			int.TryParse(finalDict["age_range:min"].ToString(), out GUtility.Me.PProgress.MyFBInfo.AgeRangeMin);
		}

		GUtility.Me.SavePProgres();
		OnFacebookLogin();
		StartCoroutine(DownloandMyImage());
		myFBAction = fbActualAction;
		StartCoroutine(ActionOnPermission());
		yield break;
	}

	protected void OnPermissionHandler(IResult result) {
		MyDebug.Log("Start Permission Handler");
		List<string> permisson = new List<string>();

		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnPermissionHandler", result, ref isSucess, ref rawResult);

		if(result == null) {
			lastResponse = "Null Response\n";
			//MyDebug.Log(lastResponse);
		} else if(!string.IsNullOrEmpty(result.Error)) {
			status = "Error - Check log for details";
			lastResponse = "Error Response:\n" + result.Error;
			//MyDebug.Log(result.Error);
		} else if(result.Cancelled) {
			status = "Cancelled - Check log for details";
			lastResponse = "Cancelled Response:\n" + result.RawResult;
			//MyDebug.Log(result.RawResult);
		} else if(!string.IsNullOrEmpty(result.RawResult)) {
			status = "Success - Check log for details";
			lastResponse = "Success Response:\n" + result.RawResult;
			isSucess = true;
			//MyDebug.Log(result.RawResult);
		} else {
			lastResponse = "Empty Response\n";
			//MyDebug.Log(lastResponse);
		}
		//
		if(result != null && !isSucess) {
			MyDebug.Log(Json.encode(result));
		}
		//
		//MyDebug.Log("Result: Status: {0}, LResponse: {1}", status, lastResponse);
		if(isSucess) {
			JsonObject rs = (JsonObject)Json.decode(result.RawResult);
			if(rs != null) {
				if(rs.ContainsKey("data")) {
					JsonArray datas = (JsonArray)rs["data"];
					foreach(object o in datas) {
						JsonObject tmpP = (JsonObject)o;
						if(tmpP.ContainsKey("status") && tmpP["status"].ToString().Equals("granted")) {
							permisson.Add(tmpP["permission"].ToString());
						}
					}
					if(permisson.Count > 0) {
						MyDebug.Log("{0} granted permission found", permisson.Count);
						GUtility.Me.PProgress.MyFBInfo.Permissions.Clear();
						GUtility.Me.PProgress.MyFBInfo.Permissions.AddRange(permisson.ToArray());
						GUtility.Me.SavePProgres();
					}
				} else
					MyDebug.Log("Data array not contians in result");
			} else
				MyDebug.Log("result is null");
		} else {
			//MyDebug.Log("St: {0}\nLastM:{1}\nError:{2}\nRAW Data:{3}",
			//	status, lastResponse,
			//	result == null ? "" : result.Error,
			//	result == null ? "" : result.RawResult
			//);
			SetLastLayer();
		}
	}

	protected void OnGetAppLinkHandler(IAppLinkResult result) {
		Utils.logObject(result);
		if(!String.IsNullOrEmpty(result.Url)) {
			var index = (new Uri(result.Url)).Query.IndexOf("request_ids");
			if(index != -1) {
			}
		}
	}

	protected void OnAppRequestHandler(IAppRequestResult result) {
		Utils.logObject(result);
	}

	protected void OnGetMyScoreHandler(IResult result) {
		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnPostScoreHandler", result, ref isSucess, ref rawResult);
		if(isSucess) {
			JsonObject rawData = (JsonObject)Json.decode(result.RawResult);
			if(rawData != null) {
				ValidateFBScore(rawData);
				OnScoreReceived();
			}
		} else {
			OnScoreFail(rawResult);
		}
	}

	protected void OnPostScoreHandler(IResult result) {
		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnPostScoreHandler", result, ref isSucess, ref rawResult);

		if(isSucess) {
			MyDebug.Log("My Score Success and parsing");
			GUtility.Me.PProgress.MyFBInfo.Score = tryScore;
			tryScore = 0;
			PopupsText.Me.FBScorePostSuccessMessage();
		} else {
			PopupsText.Me.FBScorePostFailMessage();
		}
	}

	protected void OnGetMyFriendListHandler(IResult result) {
		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnGetMyFriendListHandler", result, ref isSucess, ref rawResult);

		if(isSucess) {
			MyDebug.Log("My info Success Friend list: " + rawResult);
			Dictionary<string, object> finalDict;
			JsonArray jrData;
			JsonObject jrtemp;
			FBUser fi;

			JsonObject rs = (JsonObject)Json.decode(rawResult);

			MyFBFriends = new List<FBUser>();
			jrData = (JsonArray)rs["data"];

			if(jrData == null) {
				MyDebug.Log("FBNWebService::FriendlistHandler => FB friend not found");
				return;
			}

			for(int i = 0; i < jrData.Count; i++) {
				jrtemp = (JsonObject)jrData[i];
				finalDict = GetFinalDict(jrtemp);
				fi = new FBUser();

				fi.ID = finalDict["id"].ToString();
				if(finalDict.ContainsKey("name")) {
					fi.Name = finalDict["name"].ToString();
				} else if(finalDict.ContainsKey("first_name")) {
					fi.Name = finalDict["first_name"].ToString();
				}

				if(finalDict.ContainsKey("gender")) {
					fi.Gender = finalDict["gender"].ToString();
				}
				if(finalDict.ContainsKey("email")) {
					fi.EMail = finalDict["email"].ToString();
				}

				if(finalDict.ContainsKey("verified")) {
					bool.TryParse(finalDict["verified"].ToString(), out fi.IsVerified);
				}
				if(finalDict.ContainsKey("installed")) {
					bool.TryParse(finalDict["installed"].ToString(), out fi.Installed);
				}
				if(finalDict.ContainsKey("install_type")) {
					fi.InstallType = finalDict["install_type"].ToString();
				}
				if(finalDict.ContainsKey("picture:data:url")) {
					fi.PhotoURL = finalDict["picture:data:url"].ToString();
				}
				if(finalDict.ContainsKey("link")) {
					fi.ProfileURL = finalDict["link"].ToString();
				}
				if(finalDict.ContainsKey("age_range:min")) {
					int.TryParse(finalDict["age_range:min"].ToString(), out fi.AgeRangeMin);
				}
				MyDebug.Log("FBNService:: Name: " + fi.Name);
				MyFBFriends.Add(fi);
			}
			MyDebug.Log("FBNService::Friends Count: " + MyFBFriends.Count);
			OnFriendListReceived();
		} else {
			OnFriendListFail(rawResult);
			PopupsText.Me.FBFriendRetriveFailMessage();
		}
	}

	protected void OnGetMyFriendScoreHandler(IResult result) {
		bool isSucess = false;
		string rawResult = string.Empty;
		PreProcessHandler("OnGetMyFriendScoreHandler", result, ref isSucess, ref rawResult);

		if(isSucess) {
			MyDebug.Log("FBNWebService:: RawResult Friend Score: " + result.RawResult);
			JsonObject rs = (JsonObject)Json.decode(result.RawResult);
			if(rs != null) {
				ValidateFBScore(rs);
			}
			if(isGetAllFriendScore) {
				fbFriendIndex++;
				if(fbFriendIndex >= MyFBFriends.Count) {
					fbFriendIndex = -1;
					OnFriendScoreReceived("Completed for all");
					return;
				}
				GetFBFriendScore(fbFriendIndex);
			}
		} else {
		}

	}

	void ValidateFBScore(JsonObject rawData) {
		Dictionary<string, object> finalDict;
		JsonArray mainDataAr;
		JsonObject dataRecord;
		mainDataAr = (JsonArray)rawData["data"];
		MyDebug.Log("Raw Score Result:");
		Utils.logObject(mainDataAr);
		if(mainDataAr != null) {
			if(mainDataAr.Count > 0) {
				for(int i = 0; i < mainDataAr.Count; i++) {
					dataRecord = (JsonObject)mainDataAr[i];
					finalDict = GetFinalDict(dataRecord);
					MyDebug.Log("Managed score result:");
					Utils.logObject(finalDict);
					if(finalDict.ContainsKey("user:id") && finalDict.ContainsKey("application:id")) {
						if(finalDict["application:id"].ToString() == FBAppID) {
							string userFBId = finalDict["user:id"].ToString();
							FBUser user = GetUserByID(userFBId);
							if(user != null) {
								int.TryParse(finalDict["score"].ToString(), out user.Score);
							}
							break;
						}
					}
				}
			}
		}
	}

	void GetPermssion() {
		MyDebug.Log("Reading permisison List from FB Unity SDK");
		FB.API("/me/permissions", HttpMethod.GET, OnPermissionHandler);
	}

	public void FBLogout() {
		status = "Logout called";
		OnFacebookLogOut("Logout Called");
		FB.LogOut();

		// the line code we have added to cealr all permsssion list to verify. due to current logout issue
		// if we don't add this line then it will still count as loged, I don't how and why but it will not clear 
		// info and don't logout/
		// after adding this line we make fake logout from facebook.
		// when user click again on button, it will just redirect to permission ask
		// page by code line
		//	FB.LogInWithReadPermissions(readPermission, this.OnLoginHandler);
		//
		GUtility.Me.PProgress.MyFBInfo.ID = string.Empty;
		GUtility.Me.PProgress.MyFBInfo.Name = string.Empty;
		GUtility.Me.PProgress.MyFBInfo.PhotoURL = string.Empty;
		GUtility.Me.PProgress.MyFBInfo.ProfileURL = string.Empty;

		GUtility.Me.PProgress.MyFBInfo.AgeRangeMin = 0;
		GUtility.Me.PProgress.MyFBInfo.Score = 0;
		GUtility.Me.PProgress.MyFBInfo.Token = string.Empty;

		GUtility.Me.PProgress.MyFBInfo.Gender = string.Empty;
		GUtility.Me.PProgress.MyFBInfo.Installed = false;
		GUtility.Me.PProgress.MyFBInfo.InstallType = string.Empty;
		GUtility.Me.PProgress.MyFBInfo.IsVerified = false;
		GUtility.Me.PProgress.MyFBInfo.Permissions = new List<string>();
		GUtility.Me.SavePProgres();
		// force code end
	}

	public void FBLogin() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}
		MyDebug.Log("FBNWebService::LoginFacebook => Facebook Login called");
		MyDebug.Log("FBNWebService::LoginFacebook => Loging with Fresh permissions: ");
		myFBAction = FBAction.ReadPermission;
		FB.LogInWithReadPermissions(readPermission, OnLoginHandler);
		status = "Login called";
	}
	void GetPubishPurmission() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}

		// It is generally good behavior to split asking for read and publish
		// permissions rather than ask for them all at once.
		//
		// In your own game, consider postponing this call until the moment
		// you actually need it.

		MyDebug.Log("FBNWebService::GetPubishPurmission => Getting Facebook Publish permission");
		myFBAction = FBAction.PublishPermission;
		status = "Login (for publish_actions) called";
		FB.LogInWithPublishPermissions(publishPermission, OnLoginHandler);
	}
	void PostOnFaceBook() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}

		MyDebug.Log("FBNWebService::PostOnFaceBook => Try to posting on facebook");
		FB.Mobile.ShareDialogMode = ShareDialogMode.AUTOMATIC;
		FB.ShareLink(new Uri(shareURL), GUtility.APPNAME,
			shareText, null, OnPostHandler);
	}

	void FBInvites() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}
		myFBAction = FBAction.Invite;
		FB.Mobile.AppInvite(new Uri(GUtility.FB_APP_LINK), null);
	}

	void PostMyScore() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}
		MyDebug.Log("Posting My Score on the facebook");
		var parameters = new Dictionary<string, string>() {
			{ "score", tryScore.ToString() }
		};
		FB.API("/me/scores", HttpMethod.POST, OnPostScoreHandler, parameters);
	}

	void MyFBInfo() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}
		MyDebug.Log("Getting My Info");
		FB.API("/me/?fields=id,email,name,gender,first_name,last_name,verified,install_type,installed," +
		"link,age_range,picture.width(256).height(256).type(square)",
			HttpMethod.GET, OnGetMyInfoHandler);
	}

	void MyFBScore() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}
		MyDebug.Log("Getting My Score for the Game");
		FB.API("/me/scores?fields=score,application", HttpMethod.GET, OnGetMyScoreHandler);
	}

	void FBFrinedList() {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}
		MyDebug.Log("Getting My FB Friends list who plays the game.");
		FB.API("/me/friends/?fields=id,email,name,gender,first_name,last_name,verified,install_type,installed," +
		"link,age_range,picture.width(256).height(256).type(square)", HttpMethod.GET, OnGetMyFriendListHandler);
	}

	void FBFriendsScore(string friendFBId) {
		if(!GUtility.Me.ShowInternetConnection()) {
			return;
		}
		MyDebug.Log("Getting My FB Friend score for the game");
		FB.API("/" + friendFBId + "/scores?fields=score,application", HttpMethod.GET, OnGetMyFriendScoreHandler);
	}


	void FBAppRequest() {
		FBAppRequest("", GUtility.APPNAME, "", 50);
	}

	void FBAppRequest(string message) {
		FBAppRequest(message, GUtility.APPNAME, "", 50);
	}

	void FBAppRequest(string message, int maxFriend) {
		FBAppRequest(message, GUtility.APPNAME, "", maxFriend);
	}

	void FBAppRequest(string message, string title) {
		FBAppRequest(message, title, "", 50);
	}

	void FBAppRequest(string message, string title, int maxFriend) {
		FBAppRequest(message, title, "", maxFriend);
	}

	void FBAppRequest(string message, string title, string otherData) {
		FBAppRequest(message, title, otherData, 50);
	}

	void FBAppRequest(string message, string title, string otherData, int maxFriend) {
		if(string.IsNullOrEmpty(message)) {
			message = "Come play this great game!";
		}
		FB.AppRequest(message, null, null, null, maxFriend, otherData, title,
			OnAppRequestHandler);
	}

	public void GetAppLink() {
		FB.GetAppLink(OnGetAppLinkHandler);
	}

#endif
		#endregion

		#region Own Event Manager

		void OnFacebookLogin() {
			if(FacebookLoginEvent != null) {
				FacebookLoginEvent();
			}
		}

		void OnFacebookLogOut(string error) {
			if(FacebookLogoutEvent != null) {
				FacebookLogoutEvent(error);
			}
		}

		void OnScoreReceived() {
			if(FBMyScoreReceivedEvent != null) {
				FBMyScoreReceivedEvent();
			}
		}

		void OnScoreFail(string errorData) {
			if(FBMyScoreFailEvent != null) {
				FBMyScoreFailEvent(errorData);
			}
		}

		void OnFriendListReceived() {
			if(FBFriendReceivedEvent != null) {
				FBFriendReceivedEvent();
			}
		}

		void OnFriendListFail(string errorData) {
			if(FBFriendFailEvent != null) {
				FBFriendFailEvent(errorData);
			}
		}

		void OnFriendScoreReceived(string result) {
			if(FBFriendScoreComleteEvent != null) {
				FBFriendScoreComleteEvent(result);
			}
		}

		#endregion


		public bool IsValidFB() {
			bool hasRP = false, hasPP = false, fbInit = false, fbLogin = false;

#if FBUNITYSDK
		hasRP = CheckPermisson(readPermission);
		hasPP = publishPermission.Length <= 0 || CheckPermisson(publishPermission);
		fbInit = FB.IsInitialized;
		fbLogin = FB.IsLoggedIn;
		string logMessage = string.Format(
								"RP='{0}', PP='{1}', IsInitialized='{2}', IsLoggedIn='{3}'",
								hasRP, hasPP, fbInit, fbLogin);
		MyDebug.Log(logMessage);
#endif
			return (hasPP && hasRP && fbInit && fbLogin);
		}

		bool CheckPermisson(string[] perToCheck) {
			bool retValue = true;
			List<string> tmpPer;// = new List<string>();
			tmpPer = CoreUtility.Me.settings.myFBInfo.permissions;
			foreach(string s in perToCheck) {
				//if(!tmpPer.Contains(s)) {
				//	MyDebug.Log("Permission: {0} is missing", s);
				//	retValue = false;
				//}
				retValue &= tmpPer.Contains(s);
			}
			return retValue;
		}

		void CancelLogin() {
#if FBUNITYSDK
		FBLogout();
#endif
			SetLastLayer();
		}

		void TakeActionAfterPP() {
#if FBUNITYSDK
		MyFBInfo();
		MyFBScore();
#endif
		}

		IEnumerator ActionOnPermission() {
			switch(myFBAction) {
			case FBAction.ReadPermission:
				if(CheckPermisson(readPermission)) {
					if(publishPermission.Length > 0) {
						if(!CheckPermisson(publishPermission)) {
							yield return new WaitForSeconds(1f);
#if FBUNITYSDK
						GetPubishPurmission();
#endif
							yield break;
						}
					}
					MyDebug.Log("Has Publish Permission");
					yield return new WaitForSeconds(1f);
					TakeActionAfterPP();
				} else {
					MyDebug.Log("some of read permission are missing in access list");
					PopupMessages.Me.FacebookLoginFailMessage();
					CancelLogin();
				}
				break;

			case FBAction.PublishPermission:
				if(publishPermission.Length > 0) {
					if(!CheckPermisson(publishPermission)) {
						MyDebug.Log("some of publish permission are missing in access list");
						PopupMessages.Me.FacebookLoginFailMessage();
						CancelLogin();
						yield break;
					}
				}
				yield return new WaitForSeconds(1f);
				TakeActionAfterPP();
				break;

			case FBAction.Post:
				fbActualAction = FBAction.None;
#if FBUNITYSDK
			PostOnFaceBook();
#endif
				break;


			case FBAction.Invite:
				fbActualAction = FBAction.None;
#if FBUNITYSDK
			FBInvites();
#endif
				break;

			case FBAction.PostScore:
				fbActualAction = FBAction.None;
#if FBUNITYSDK
			PostMyScore();
#endif
				break;

			case FBAction.MyInfo:
				fbActualAction = FBAction.None;
#if FBUNITYSDK
			MyFBInfo();
#endif
				break;

			case FBAction.GetMyScore:
				fbActualAction = FBAction.None;
#if FBUNITYSDK
			MyFBScore();
#endif
				break;

			case FBAction.GetFriendList:
				fbActualAction = FBAction.None;
#if FBUNITYSDK
			FBFrinedList();
#endif
				break;

			case FBAction.AppRequest:
				fbActualAction = FBAction.None;
				SetLastLayer();
				break;


			case FBAction.Like:
				fbActualAction = FBAction.None;
				SetLastLayer();
				break;


			case FBAction.CheckFBMP:
				yield return new WaitForSeconds(0.5f);
				fbActualAction = FBAction.None;
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "ShowFBFrends", new ButtonEventArgs()));
				break;

			default:
				fbActualAction = FBAction.None;
				SetLastLayer();
				break;
			}
		}


		public void GetMyFBInfo() {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

			if(IsValidFB()) {
#if FBUNITYSDK
			MyFBInfo();
#endif
			} else {
				MyDebug.Log("FBNWebService::PostScoreOnFB => Not Loged in ever");
				fbActualAction = FBAction.MyInfo;
#if FBUNITYSDK
			FBLogin();
#endif
			}
		}

		public void ShareTextOnFB(string text, string url) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

			CoreMethods.lastLayer = CoreMethods.layer;
			CoreMethods.layer = Menus.Social;
			shareText = text;
			shareURL = url;
			if(IsValidFB()) {
#if FBUNITYSDK
			PostOnFaceBook();
#endif
			} else {
				MyDebug.Log("FBNWebService::ShereInFaceBook => Not Loged in ever");
				fbActualAction = FBAction.Post;
#if FBUNITYSDK
			FBLogin();
#endif
			}
		}

		public void InviteFBFriends() {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
			if(IsValidFB()) {
#if FBUNITYSDK
			FBInvites();
#endif
			} else {
				MyDebug.Log("FBNWebService::ShereInFaceBook => Not Loged in ever");
				fbActualAction = FBAction.Invite;
#if FBUNITYSDK
			FBLogin();
#endif
			}
		}

		public void PostScoreOnFB(int score) {
			if(CoreUtility.Me.settings.myFBInfo.score > score) {
				return;
			}

			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

#if FBUNITYSDK
		tryScore = score;
#endif
			if(IsValidFB()) {
#if FBUNITYSDK
			PostMyScore();
#endif
			} else {
				MyDebug.Log("FBNWebService::PostScoreOnFB => Not Loged in ever");
				fbActualAction = FBAction.PostScore;
#if FBUNITYSDK
			FBLogin();
#endif
			}
		}

		public void GetFBScore() {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

			if(IsValidFB()) {
#if FBUNITYSDK
			MyFBScore();
#endif
			} else {
				MyDebug.Log("FBNWebService::GetFBScore => Not Loged in ever");
				fbActualAction = FBAction.GetMyScore;
#if FBUNITYSDK
			FBLogin();
#endif
			}
		}

		public void GetFBFriends() {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

			if(IsValidFB()) {
#if FBUNITYSDK
			FBFrinedList();
#endif
			} else {
				MyDebug.Log("FBNWebService::GetFBScore => Not Loged in ever");
				fbActualAction = FBAction.GetFriendList;
#if FBUNITYSDK
			FBLogin();
#endif
			}
		}

		public void GetFBFriendScore() {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
#if FBUNITYSDK
		isGetAllFriendScore = true;
#endif
			fbFriendIndex = 0;
			GetFBFriendScore(fbFriendIndex);
		}
		public void GetFBFriendScore(string fbID) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
			if(string.IsNullOrEmpty(fbID)) {
				PopupMessages.Me.InvalidRequest();
				return;
			}
#if FBUNITYSDK
		isGetAllFriendScore = false;
#endif
			GFBFriendScore(fbID);
		}
		public void GetFBFriendScore(FBUser friend) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
			if(friend == null) {
				PopupMessages.Me.InvalidRequest();
				return;
			}
			if(string.IsNullOrEmpty(friend.id)) {
				PopupMessages.Me.InvalidRequest();
				return;
			}
#if FBUNITYSDK
		isGetAllFriendScore = false;
#endif
			GFBFriendScore(friend.id);
		}
		void GetFBFriendScore(int index) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
			if(MyFBFriends.Count >= index || index < 0) {
				PopupMessages.Me.NotInFriendList();
				return;
			}
			if(MyFBFriends[index] == null) {
				PopupMessages.Me.NotInFriendList();
				return;
			}
			GFBFriendScore(MyFBFriends[index].id);
		}
		void GFBFriendScore(string fbID) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
			if(string.IsNullOrEmpty(fbID)) {
				PopupMessages.Me.InvalidRequest();
				return;
			}
#if FBUNITYSDK
		FBFriendsScore(fbID);
#endif
		}

		#endregion

		#region Prime31 Social Networking
		#region TWITTER
		TWAction myTWAction = TWAction.None;
#if SOCIALNETWORKING
		public void ShareOnTwitter(string text, string url) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

			CoreMethods.lastLayer = CoreMethods.layer;
			shareText = text;
			shareURL = url;
			CoreMethods.layer = Menus.Social;
			if(TwitterB.isLoggedIn()) {
				Tweet();
			} else {
				myTWAction = TWAction.Tweet;
				LoginTwitter();
			}
		}

		public void LoginTwitter() {
#if UNITY_ANDROID
		TwitterB.showLoginDialog(false);
#endif
#if UNITY_IOS
			TwitterB.showLoginDialog();
#endif
		}
		public void LogoutTwitter() {
			TwitterB.logout();
		}
		void Tweet() {
			TwitterB.postStatusUpdate(shareText + "\n" + shareURL);
		}

		void TWTwitterInitializedEvent() {
			MyDebug.Log("FBNWebService::TWTwitterInitializedEvent =>");
		}

		void TWloginSucceededEvent(string message) {
			MyDebug.Log("FBNWebService::TWloginSucceededEvent => twitter Login Success Message" + message);
			if(myTWAction == TWAction.Tweet) {
				Tweet();
			} else {
				SetLastLayer();
			}
		}
		void TWLoginFailedEvent(string error) {
			MyDebug.Log("FBNWebService::TWLoginFailedEvent => Twitter login failed, ERROR: " + error);
			PopupMessages.Me.TwitterFailMessage();
			SetLastLayer();
		}
		void TWRequestDidFinishEvent(object result) {
			MyDebug.Log("FBNWebService::TWRequestDidFinishEvent =>");
			bool isSuccess;
			JsonObject twitResult = (JsonObject)result;
			Prime31.Utils.logObject(result);
			shareMessageTitle = "";
			isSuccess = twitResult.ContainsKey("text");

			if(isSuccess) {
				shareMessageStatus = "Successfully posted on your\ntwitter timeline.";
				if(GameUtility.Me.twShareCoin > 0) {
					TimeSpan ts = DateTime.UtcNow - CoreUtility.Me.settings.lastTwitterShare;
					if(ts.TotalHours >= 24 && CoreUtility.Me.shareMode.Equals(ShareMode.Game)) {
						CoreUtility.Me.settings.lastTwitterShare = DateTime.UtcNow;
						GameUtility.Me.AddInCoin(GameUtility.Me.twShareCoin);
						CoreUtility.Me.SaveSettings();
						GameUtility.Me.SavePlayerData();

						shareMessageStatus += "You have got\n# " + GameUtility.Me.twShareCoin + " from twitter Share";
					} else if(ts.TotalHours < 24) {
						shareMessageStatus += "Please try after\nfew hour to get free coins";
					}
				}
			} else {
				shareMessageStatus = "Twitter posting failed.\nPlease try after some time.";
				if(twitResult.ContainsKey("errors")) {
					JsonArray err = (JsonArray)twitResult["errors"];
					if(err.Capacity > 0) {
						JsonObject twitError;
						twitError = (JsonObject)err[0];
						switch(twitError["code"].ToString()) {
						case "187":
						case "403":
							shareMessageTitle = "No new tweets available";
							break;
						}
					}
				}
			}

			PopupMessages.Me.TwitterSuccessMessage(shareMessageTitle, shareMessageStatus);
			SetLastLayer();
		}
		void TWRequestDidFailEvent(string error) {
			MyDebug.Log("FBNWebService::TWRequestDidFailEvent => ERROR: " + error);
			PopupMessages.Me.TwitterFailMessage();
			SetLastLayer();
		}
		void TWTweetSheetCompletedEvent(bool isComplete) {
			MyDebug.Log("FBNWebService::TWTweetSheetCompletedEvent => Is Complete: " + isComplete);
			SetLastLayer();
		}
#endif
		#endregion

		#region iOS Native Social Sharing
#if SOCIALNETWORKING
		void OnSharingSuccessed(string activity) {
			// Twitter: 		com.apple.UIKit.activity.PostToTwitter
			// Mail: 			com.apple.UIKit.activity.Mail
			// Facebook:		com.apple.UIKit.activity.PostToFacebook
			// Message:			com.apple.UIKit.activity.Message
			// Linked:			com.linkedin.LinkedIn.ShareExtension
			// Reading List:	com.apple.UIKit.activity.AddToReadingList
			// Copy :			com.apple.UIKit.activity.CopyToPasteboard
			shareMessageStatus = "Successfully shared with your friends";
			if(GameUtility.Me.nativeShareCoin > 0) {
				TimeSpan ts = DateTime.UtcNow - CoreUtility.Me.settings.lastOtherShare;
				if(ts.TotalHours >= 24 && CoreUtility.Me.shareMode.Equals(ShareMode.Game)) {
					CoreUtility.Me.settings.lastOtherShare = DateTime.UtcNow;
					GameUtility.Me.AddInCoin(GameUtility.Me.nativeShareCoin);
					CoreUtility.Me.SaveSettings();
					GameUtility.Me.SavePlayerData();
					shareMessageStatus += "You have got # " + GameUtility.Me.nativeShareCoin;
				} else if(ts.TotalHours < 24) {
					shareMessageStatus += "Please try after\nfew hour to get free coins";
				}
			}
			PopupMessages.Me.ShareSuccessMessage(shareMessageStatus);
		}
		void OnSharingFailed() {
			PopupMessages.Me.SharingFailMessage();
		}
#endif
		#endregion

		#endregion
	}
}
namespace GameAnax.Game.Enums {
	public enum FBAction {
		None = -1,
		ReadPermission = 1,
		PublishPermission = 2,
		Post = 3,
		Invite = 4,
		PostScore,
		MyInfo,
		GetMyScore,
		GetFriendList,
		GetFriendScore,
		Like,
		AppRequest,
		CreateGameGroup,
		JoinGameGroup,
		CheckFBMP
	}

	public enum TWAction {
		None,
		Tweet,
		Follow,
		UnFollow
	}
}