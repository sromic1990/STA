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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.SceneManagement;

using GameAnax.Core;
using GameAnax.Core.NotificationSystem;
using GameAnax.Core.Security;
using GameAnax.Core.Singleton;
using GameAnax.Core.UI.Buttons;
using GameAnax.Core.Utility;
using GameAnax.Core.Sound;
using Network = GameAnax.Core.Net.Network;


using GameAnax.Game.CommonSystem;
using GameAnax.Game.Leaderboard;
using GameAnax.Game.Model;
using GameAnax.Game.Social;
using GameAnax.Game.Utility.Popup;
using GameAnax.Game.Utility.Ad;

#region prime[31] iOS 'n' Android Manager Settings
//using Prime31;

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
//

namespace GameAnax.Game.Utility {
	[PersistentSignleton(true, true)]
	public class GameUtility : Singleton<GameUtility> {
		#region Declaration
		#region FB Related
		public const string APP_FB_IMAGE_SHORT_URL = "";
		public const string FB_PAGE_URL = "";
		public const string FB_APP_LINK = "";
		#endregion

		#region App Related
		public const string APPNAME = "Ball Dance";
		public const string APPITUNESID = "1277590011";
#if UNITY_IOS
		public const string STORE_SHORT_URL = "https://goo.gl/KVGDZv";
		public const string STORE_LONG_URL = "itms-apps://itunes.apple.com/app/id1277590011";
#endif
#if UNITY_ANDROID
		public const string STORE_SHORT_URL = "https://goo.gl/";
		public const string STORE_LONG_URL = "https://play.google.com/store/apps/details?id=com.chitralekha.balldance";
#endif
		public readonly string[] BEST_SELLER = new string[] {
		};
		// Android Market URL: "market://details?id=<<BUNDLE_ID>>"
		// Android PlayStore URL: "https://play.google.com/store/apps/details?id=<<BUNDLE_ID>>"
		// iOS iTune URL: https://itunes.apple.com/us/app/id<<APPITUNEID>>
		// iOS Store URL: itms-apps://itunes.apple.com/app/id<<APPITUNEID>>
		#endregion

		#region Gameplay and Score
		public string REVIEW_TITLE { private set; get; }
		public string REVIEW_TEXT { private set; get; }
		public const string BEST_SCORE = "{0}";

		string[] _gameShareText;
		string[] _highScoreShareText;

		[HideInInspector]
		public int coinEarned;
		[HideInInspector]
		public FreeContentType freeContent = FreeContentType.None;

		#endregion

		#region public non-editor inspector members
		[HideInInspector]
		public int videoReward;
		[HideInInspector]
		public int videoRewardTime;
		[HideInInspector]
		public AlertType alert = AlertType.None;
		[HideInInspector]
		public StoreOrigan storeVia = StoreOrigan.None;
		[HideInInspector]
		public PlayerData playerData;
		[HideInInspector]
		public Mode eMode, pMode;
		[HideInInspector]
		public Level eLevel, pLevel;
		[HideInInspector]
		public GameModes gameMode;
		#endregion

		#region public members
		[Space(5)]
		public string dataFile;
		public bool isEncryptDataFile;
		public string encryptionKey;
		[Space(5)]
		public string nextScene;
		//
		[Space(5)]
		public int screenTimeOutForMenu;
		//
		[Space(5)]
		[Header("Share Reward")]
		public int fbPageCoin;
		public int fbShareCoin;
		public int twShareCoin;
		public int twFollowCoin;
		public int nativeShareCoin;

		[Header("Video Reward")]
		public int iOSVideoReward;
		public int iOSVideoRewardTime;
		[Space(5)]
		public int androidVideoReward;
		public int androidVideoRewardTime;
		//
		[Header("Review Related Values")]
		public int reviewCoin;
		public int reviewAfterGame;
		//
		[Header("Ads Related Values")]
		public bool isBannerAd;
		public bool isBannerAdDurinGamePlay;
		public bool isBannerAdDuringMenus;

		[Space(5)]
		[Range(1, 100)]
		public int adAfterGameStart;
		[Space(5)]
		[Range(1, 100)]
		public int bgAdsSecAfter;
		[Space(5)]
		[Range(0, 100)]
		public int noAdUntilGameLunch;
		[Range(0, 100)]
		public int noAdUntilGamePlay;
		[Range(0, 100)]
		public int noAdUntilGameOver;

		[Space(10)]
		public bool isMiniMapInGame;

		[Space(10)]
		public bool isLaterBoxing;
		public List<GameObject> letterboxGroups;

		[Space(10)]
		public List<Scoreboard> leaderboards;
		public List<Achievement> achievements;

		[Space(20)]
		public bool isModeBasedGame;
		public List<Mode> modes;
		#endregion
		#endregion

		#region Mono Actions

		// Use this for initialization
		void Awake() {
			Me = this;
			Application.targetFrameRate = 60;
			NotificationCenter.Me.AddObserver(this, "ShareGame");
			NotificationCenter.Me.AddObserver(this, "ShareHighscore");

			NotificationCenter.Me.AddObserver(this, "ShowLeaderboard");
			NotificationCenter.Me.AddObserver(this, "ShowAchievemetns");

			NotificationCenter.Me.AddObserver(this, "RemoveAds");

			NotificationCenter.Me.AddObserver(this, "BestGames");
			NotificationCenter.Me.AddObserver(this, "FacebookPage");

			NotificationCenter.Me.AddObserver(this, "GoToHome");

			NotificationCenter.Me.AddObserver(this, "CallAdsForFreeContinue");
			NotificationCenter.Me.AddObserver(this, "CallAdsForFreeLevel");
			NotificationCenter.Me.AddObserver(this, "CallAdsForFreeCash");


			#region iOS Plugin Start
#if UNITY_IOS
			videoReward = iOSVideoReward;
			videoRewardTime = iOSVideoRewardTime;
#endif
			#endregion

			#region Android Plugin Start
#if UNITY_ANDROID
			if(androidVideoReward >= 0) {
				videoReward = androidVideoReward;
			} else {
				videoReward = iOSVideoReward;
			}
			if(androidVideoRewardTime >= 0) {
				videoRewardTime = androidVideoRewardTime;
			} else {
				videoRewardTime = iOSVideoRewardTime;
			}
#endif
			#endregion

			SendLaunchInfo();

			_gameShareText = new[] {
				"Download #" + APPNAME.Replace(" ", "") + ", it's really #amazing #mobile #game",
				"you played ever #" + APPNAME.Replace(" ", "") + "? you will #love this #mobile #game",
				"I loves #" + APPNAME.Replace(" ", "") + " #mobile #game, Try it now"
			};
			_highScoreShareText = new[] {
				"I just scored \"{0}\" in #" + APPNAME.Replace(" ", "") + " #game, can you beat my score?",
				"I just collected \"{0}\" points in #game #" + APPNAME.Replace(" ", "") + ", can you?"

			};
			if(reviewCoin > 0) {
				REVIEW_TITLE = "Like this game?";
				//REVIEW_TEXT = "We'll love to hear your feedback! Please review the game as you like and get " + reviewCoin + " coins free";
				REVIEW_TEXT = "We'll love to hear your feedback! Please review the game as you like and get reward";
			} else {
				REVIEW_TITLE = "Like this game?";
				REVIEW_TEXT = "We'll love to hear your feedback!";
			}
			letterboxGroups.ForEach(o => o.SetActive(isLaterBoxing));
		}
		IEnumerator Start() {
			ShowProgressDialog();
			this.LoadPlayeData();
			UpdateModeInfo();
			if(CoreUtility.Me.settings.appVersion < CoreUtility.Me.appVersion) {
				UpdateVersion();
			}
			CoreUtility.Me.CheckFBIDnName();
			yield return new WaitForEndOfFrame();

			this.StartLeaderboard(true);
			this.SavePlayerData();

			yield return Resources.UnloadUnusedAssets();
			MyDebug.Log("GameUtility::Start Complete");
			if(string.IsNullOrEmpty(nextScene)) {
				NotificationCenter.Me.PostNotification(this, "StartMenuSystem");
			} else {
				SceneManager.LoadScene(nextScene);
			}
		}
		//
		void OnEnable() {
#if ETCETERA
			EtceteraM.alertButtonClickedEvent += AlertButtonClicked;
#if UNITY_ANDROID
			EtceteraM.askForReviewWillOpenMarketEvent += ReviewWillOpenMarketEvent;
			EtceteraM.askForReviewRemindMeLaterEvent += ReviewRemindMeLaterEvent;
			EtceteraM.askForReviewDontAskAgainEvent += ReviewDontAskAgainEvent;
#endif
#endif
		}

		void OnDisable() {
#if ETCETERA
			EtceteraM.alertButtonClickedEvent -= AlertButtonClicked;
#if UNITY_ANDROID
			EtceteraM.askForReviewWillOpenMarketEvent -= ReviewWillOpenMarketEvent;
			EtceteraM.askForReviewRemindMeLaterEvent -= ReviewRemindMeLaterEvent;
			EtceteraM.askForReviewDontAskAgainEvent -= ReviewDontAskAgainEvent;
#endif
#endif
		}


		#endregion

		#region Utility Mehtods
		#region Update & Analytic methods
		void UpdateVersion() {
			CoreUtility.Me.UpdateVersion();
		}
		public void SendLaunchInfo() { }
		#endregion

		#region Load and Save Settings
		public bool SavePlayerData() {
			bool retValue;
			try {
				string settingData;
				settingData = CoreMethods.Serialize<PlayerData>(playerData, SerializationType.XML);
				if(isEncryptDataFile) {
					settingData = Encryption.RFC2898Encrypt(settingData, encryptionKey, PaddingMode.None);
				}
				retValue = CoreMethods.SaveData(dataFile, settingData);
			} catch(Exception ex) {
				Debug.LogError(ex.Message + '\n' + ex.Data);
				retValue = false;
				Application.Quit();
			}
			return retValue;
		}
		public bool LoadPlayeData() {
			bool retValue = false;
			bool isPlayerDataFound;
			string settingData = string.Empty;
			try {
				isPlayerDataFound = CoreMethods.LoadData(dataFile, ref settingData);
				bool isEncrypt;
				if(isPlayerDataFound) {
					isEncrypt = !settingData.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase);
				} else {
					isEncrypt = false;
				}
				if(!string.IsNullOrEmpty(settingData) && isEncrypt)
					settingData = Encryption.RFC2898Encrypt(settingData, encryptionKey, PaddingMode.None);

				if(isPlayerDataFound && !string.IsNullOrEmpty(settingData)) {
					PlayerData tempPrefabs;
					tempPrefabs = CoreMethods.Deserialize<PlayerData>(settingData, SerializationType.XML) as PlayerData;
					if(null != tempPrefabs) {
						playerData = tempPrefabs;
						settingData = string.Empty;
						retValue = true;
					} else {
						Debug.Log("GameUtility::Player Data => currepted data found");
						GeDefaultPrefrence();
					}
				} else {
					Debug.Log("GameUtility::Player Data => Data not found");
					GeDefaultPrefrence();
				}
			} catch(Exception ex) {
				Debug.LogWarning(ex.Message + '\n' + ex.Data);
				GeDefaultPrefrence();
				retValue = false;
			}
			return retValue;
		}
		void UpdateModeInfo() {
			if(playerData.modes == null) {
				playerData.modes = new List<Mode>();
			}
			foreach(Mode tmpEMode in modes) {
				MyDebug.Log("Editor Mode: " + tmpEMode.ModeName);
				if(!tmpEMode.isModeActive) {
					continue;
				}
				bool isModeFind = false;
				foreach(Mode tmpPMode in playerData.modes) {
					if(tmpPMode.ModeName != tmpEMode.ModeName) {
						continue;
					}
					isModeFind = true;
					UpdateModeAchievements(tmpEMode, tmpPMode);
					UpdateLevel(tmpEMode.levels, tmpPMode.levels);
				}

				if(!isModeFind) {
					Mode tmpMode = tmpEMode.Copy();
					UpdateModeAchievements(tmpEMode, tmpMode);
					UpdateLevel(tmpEMode.levels, tmpMode.levels);
					playerData.modes.Add(tmpMode);
				}
			}
		}
		private void UpdateLevel(List<Level> eLevels, List<Level> pLevels) {
			foreach(Level tmpELevel in eLevels) {

				bool isLevelFound = false;
				foreach(Level tmpPLevels in pLevels) {
					if(tmpPLevels.levelNumber != tmpELevel.levelNumber) {
						continue;
					}
					isLevelFound = true;
				}

				if(!isLevelFound) {
					Level tempLevel = tmpELevel.Copy();
					pLevels.Add(tempLevel);
				}
			}
		}

		private void UpdateModeAchievements(Mode em, Mode pm) {
			// Achiement Update for particular - START
			bool isAchieFind;
			foreach(Achievement eAch in em.achievements) {
				isAchieFind = false;
				foreach(Achievement pAch in pm.achievements) {
					if(pAch.keyGameCenteriOS != eAch.keyGameCenteriOS) {
						continue;
					}
					isAchieFind = true;

					pAch.type = eAch.type;
					pAch.value = eAch.value;


					pAch.keyGooglePlayService = eAch.keyGooglePlayService;
					pAch.keyGameCenterTVOS = eAch.keyGameCenterTVOS;

					pAch.message = eAch.message;
					pAch.analyticValue = eAch.analyticValue;
					pAch.prefKey = eAch.prefKey;
				}

				if(!isAchieFind) {
					Achievement a = eAch.Copy();
					pm.achievements.Add(a);
				}
			}
			// Achiement Update for particular - END
		}

		void GeDefaultPrefrence() {
			playerData = new PlayerData();
			MyDebug.Log("GeDefaultPrefrence");
			SavePlayerData();
		}
		#endregion

		#region Basic Game Mehtods (Common for most game)
		#region Plugin related Method

		public void AddInCoin(int value) {
			playerData.coins += value;
		}
		public void BestGames(ButtonEventArgs args) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
			int id = 0;
			int.TryParse(args.data, out id);
			MyDebug.Log("Best Seller ID: " + id);
			if(id < BEST_SELLER.Length) {
				MyDebug.Log("Best Seller URL: " + BEST_SELLER[id]);
				Application.OpenURL(BEST_SELLER[id]);
			}
		}

		List<string> _highScoreShareMessage = new List<string>();
		List<string> _gameShareMess = new List<string>();
		string GetRandomShareMessage() {
			if(_gameShareMess.Count <= 0) {
				_gameShareMess.Clear();
				_gameShareMess.AddRange(_gameShareText.ToArray());
			}
			int x = UnityEngine.Random.Range(0, _gameShareMess.Count);
			string f = _gameShareMess[x];
			_gameShareMess.RemoveAt(x);
			return f;
		}
		string GetRandomHighScoreShareMessage() {
			if(_highScoreShareMessage.Count <= 0) {
				_highScoreShareMessage.Clear();
				_highScoreShareMessage.AddRange(_highScoreShareText.ToArray());
			}
			int x = UnityEngine.Random.Range(0, _highScoreShareMessage.Count);
			string f = _highScoreShareMessage[x];
			_highScoreShareMessage.RemoveAt(x);
			return f;
		}
		public int GetBestScore() {
			if(isModeBasedGame) {
				if(eMode.levels.Count > 0)
					return pLevel.bestScore;
				else
					return pMode.bestScore;
			} else
				return playerData.bestScore;
		}
		public void SetBestScore(int bestScore) {
			if(isModeBasedGame) {
				if(eMode.levels.Count > 0)
					pLevel.bestScore = bestScore;
				else
					pMode.bestScore = bestScore;
			} else
				playerData.bestScore = bestScore;
		}

		public int GetScore() {
			if(isModeBasedGame) {
				if(eMode.levels.Count > 0)
					return pLevel.score;
				else
					return pMode.score;
			} else
				return playerData.score;
		}
		public void SetScore(int score) {
			if(isModeBasedGame) {
				if(eMode.levels.Count > 0)
					pLevel.score = score;
				else
					pMode.score = score;
			} else
				playerData.score = score;
		}

		// Twitter: 		com.apple.UIKit.activity.PostToTwitter
		// Mail: 			com.apple.UIKit.activity.Mail
		// Facebook:		com.apple.UIKit.activity.PostToFacebook
		// Message:			com.apple.UIKit.activity.Message
		// Linked:			com.linkedin.LinkedIn.ShareExtension
		// Reading List:	com.apple.UIKit.activity.AddToReadingList
		// Copy :			com.apple.UIKit.activity.CopyToPasteboard

#if UNITY_IOS && SOCIALNETWORKING
		string[] _excludeActivity =
			new[] {
			"com.apple.UIKit.activity.AddToReadingList", "com.apple.UIKit.activity.CopyToPasteboard"
			};
#endif
		public void ShareGame(ButtonEventArgs args) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
			MyDebug.Log("GameUtility::ShareGame => Share to: " + args.data);
			CoreUtility.Me.shareMode = ShareMode.Game;
			string shareText = GetRandomShareMessage();
			MyDebug.Log(args.data + ": Share is => " + shareText);
#if UNITY_EDITOR
			GameAnax.Core.Utility.Popup.PopupMessages.Me.NativeFuncionNonAvailableMessage();
			return;
#endif
			if(args.data.Equals("Facebook", StringComparison.OrdinalIgnoreCase)) {
#if SOCIALNETWORKING
				FBService.Me.ShareTextOnFB(shareText, STORE_LONG_URL);
#endif
			} else if(args.data.Equals("Native", StringComparison.OrdinalIgnoreCase)) {
#if UNITY_IOS && SOCIALNETWORKING
				SharingBinding.shareItems(new[] { shareText, STORE_SHORT_URL }, _excludeActivity);
#endif
#if UNITY_ANDROID && ETCETERA
				EtceteraB.shareWithNativeShareIntent(shareText + "\n" + STORE_SHORT_URL, "", "Share On", null);
#endif
			} else if(args.data.Equals("Twitter", StringComparison.OrdinalIgnoreCase)) {
#if SOCIALNETWORKING
				FBService.Me.ShareOnTwitter(shareText, STORE_SHORT_URL);
#endif
			}
		}
		public void ShareHighscore(ButtonEventArgs args) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

			MyDebug.Log("GameUtility::ShareHighscore => Share to: " + args.data);
			CoreUtility.Me.shareMode = ShareMode.Highscore;
			string shareText = GetRandomHighScoreShareMessage();
			int score = GetBestScore();
			string hScoreShareText = string.Format(shareText, score);
			MyDebug.Log(args.data + ": Share is => " + hScoreShareText);
#if UNITY_EDITOR
			GameAnax.Core.Utility.Popup.PopupMessages.Me.NativeFuncionNonAvailableMessage();
			return;
#endif

			if(args.data.Equals("Facebook", StringComparison.OrdinalIgnoreCase)) {
#if SOCIALNETWORKING
				FBService.Me.ShareTextOnFB(hScoreShareText, STORE_LONG_URL);
#endif
			} else if(args.data.Equals("Native", StringComparison.OrdinalIgnoreCase)) {
#if UNITY_IOS && SOCIALNETWORKING
				Prime31.SharingBinding.shareItems(new[] { hScoreShareText, STORE_SHORT_URL }, _excludeActivity);
#endif
#if UNITY_ANDROID && ETCETERA
				EtceteraB.shareWithNativeShareIntent(hScoreShareText + "\n" + STORE_SHORT_URL, "", "Share On", null);
#endif
			} else if(args.data.Equals("Twitter", StringComparison.OrdinalIgnoreCase)) {
#if SOCIALNETWORKING
				FBService.Me.ShareOnTwitter(hScoreShareText, STORE_SHORT_URL);
#endif
			}
		}


		public void CallAdsForFreeContinue(ButtonEventArgs args) {
#if UNITY_EDITOR
			CoreUtility.Me.OnFreeContinueSucess();
			return;
#endif

			CoreUtility.Me.freeContent = (FreeContentType.FreeContinue);
			if(AdsMCG.Me.IsAdAvailable(CoreUtility.Me.freeContent.ToString())) {
				AdsMCG.Me.ShowAd(CoreUtility.Me.freeContent.ToString());
			} else {
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsFail", ""));
			}
		}
		public void CallAdsForFreeLevel(ButtonEventArgs args) {
			CoreUtility.Me.freeLelveNameOrID = args.data;
#if UNITY_EDITOR
			CoreUtility.Me.OnFreeLevelSucess(CoreUtility.Me.freeLelveNameOrID);
			return;
#endif

			CoreUtility.Me.freeContent = (FreeContentType.FreeLevel);
			if(AdsMCG.Me.IsAdAvailable(CoreUtility.Me.freeContent.ToString())) {
				AdsMCG.Me.ShowAd(freeContent.ToString());
			} else {
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsFail", ""));
			}
		}
		public void CallAdsForFreeCash(ButtonEventArgs args) {
			CoreUtility.Me.freeLelveNameOrID = args.data;
#if UNITY_EDITOR
			CoreUtility.Me.OnFreeCashSucess("", 0);
			return;
#endif

			CoreUtility.Me.freeContent = (FreeContentType.FreeCash);
			if(AdsMCG.Me.IsAdAvailable(CoreUtility.Me.freeContent.ToString())) {
				AdsMCG.Me.ShowAd(CoreUtility.Me.freeContent.ToString());
			} else {
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsFail", ""));
			}
		}

		public void StartLeaderboard(bool autoStart = false) {
			if(!Network.IsInternetConnection() && !autoStart) {
				Core.Utility.Popup.PopupMessages.Me.InternetConnectionMessgae();
				return;
			}
#if UNITY_IOS && GAMECENTER
			if(!GameCenterBinding.isPlayerAuthenticated()) {
				GameCenterBinding.authenticateLocalPlayer();
			}
#endif
#if UNITY_ANDROID
#if GPGSERVIES && !AMAZONSTORE
			GPGManager.authenticationFailedEvent += (string error) => {
				MyDebug.Log("Prime[31] Google Play service authenticate fail with error:\n" + error);
			};
			GPGManager.authenticationSucceededEvent += (string data) => {
				MyDebug.Log("Prime[31] Google Play service authenticate compelete with data:\n" + data);
			};
			if(!PlayGameServices.isSignedIn()) {
#if MYDEBUG
				PlayGameServices.enableDebugLog(true);
#endif
				PlayGameServices.authenticate();
			}
#endif
#endif
		}

		public void ReportScore(int score, int modeId, int leaderboard) {
#if UNITY_IOS && GAMECENTER
			if(isModeBasedGame) {
				GameCenterBinding.reportScore(score, modes[modeId].leaderboards[leaderboard].keyGameCenteriOS);
			} else {
				GameCenterBinding.reportScore(score, leaderboards[leaderboard].keyGameCenteriOS);
			}
#endif
#if UNITY_ANDROID && GPGSERVIES
			if(isModeBasedGame) {
				PlayGameServices.submitScore(modes[modeId].leaderboards[leaderboard].keyGoogleGameServices, score);
			} else {
				PlayGameServices.submitScore(leaderboards[leaderboard].keyGoogleGameServices, score);
			}
#endif
		}
		public void ShowLeaderboard(ButtonEventArgs args) {
#if UNITY_IOS
#if GAMECENTER
			if(GameCenterBinding.isPlayerAuthenticated())
				GameCenterBinding.showLeaderboardWithTimeScope(GameCenterLeaderboardTimeScope.AllTime);
			else
				StartLeaderboard();
#else
			Core.Utility.Popup.PopupMessages.Me.NativeFuncionNonAvailableMessage();
#endif
#endif

#if UNITY_ANDROID
#if GPGSERVIES
			if(!PlayGameServices.isSignedIn()) {
				StartLeaderboard(false);
			} else {
				PlayGameServices.showLeaderboards();
			}
#else
			Core.Utility.Popup.PopupMessages.Me.NativeFuncionNonAvailableMessage();
#endif
#endif
		}
		public void ShowAchievemetns(ButtonEventArgs args) {
#if UNITY_IOS
#if GAMECENTER
#else
			Core.Utility.Popup.PopupMessages.Me.NativeFuncionNonAvailableMessage();
#endif
#endif

#if UNITY_ANDROID
#if GPGSERVIES
			if(!PlayGameServices.isSignedIn()) {
				StartLeaderboard();
			} else {
				PlayGameServices.showAchievements();
			}
#else
			Core.Utility.Popup.PopupMessages.Me.NativeFuncionNonAvailableMessage();
#endif
#endif
		}


		#endregion
		#endregion
		#region Only for this game Menthod (non common)
		public bool IsTutorialForMode() {
			bool retVal = false;
			if(null != eMode && eMode.isTutorial) {
				retVal = true;
			}

			return retVal;
		}
		#endregion
		#endregion

		#region Etcetera
		public void AskForReview(ButtonEventArgs args) {
			if(CoreUtility.Me.settings.isAlreadyRated || CoreUtility.Me.settings.isNeverAskForReview) {
				return;
			}
#if UNITY_IOS && ETCETERA
			alert = AlertType.Review;
			EtceteraB.askForReview(REVIEW_TITLE, REVIEW_TEXT, APPITUNESID);
#elif UNITY_ANDROID && ETCETERA
			EtceteraB.askForReviewSetButtonTitles("Next time", "Don't ask me again", "Sure!");
#if !AMAZONSTORE
			EtceteraB.askForReviewNow(REVIEW_TITLE, REVIEW_TEXT, false);
#endif
#if AMAZONSTORE
		EtceteraB.askForReviewNow(REVIEW_TITLE, REVIEW_TEXT, true);
#endif
#endif
		}
		public void ShowProgressDialog(string title = "", string message = "") {
#if ETCETERA
#if UNITY_IOS
			EtceteraB.showActivityView();
#endif
#if UNITY_ANDROID
			//EtceteraB.showProgressDialog(title, message);
#endif
#endif
		}
		public void HideProgressDialog() {
#if ETCETERA
#if UNITY_IOS
			EtceteraB.hideActivityView();
#endif
#if UNITY_ANDROID
			//EtceteraB.hideProgressDialog();
#endif
#endif
		}
		public void AlertButtonClicked(string text) {
			switch(alert) {

			case AlertType.Review:
				if(text.ToLower().Contains("ok")) {
					CoreUtility.Me.settings.isAlreadyRated = true;
					//if(reviewCoin > 0) {
					//	PopupMessages.Me.ReviewWithCoinMessage(reviewCoin + " coins");
					//} else {
					//	PopupMessages.Me.ReviewWithOutCoinMessgae();
					//}
				}
				break;

			case AlertType.RemoveAds:
				break;

#if UNITY_ANDROID && !AMAZONSTORE && GPGSERVIES
			case AlertType.GPGClicked:
				break;
#endif

#if UNITY_ANDROID && AMAZONSTORE && GAMECIRCLE
		case AlertType.AGSClicked:
		break;
#endif
			default:
				return;

			}
			alert = AlertType.None;
			MyDebug.Log("Alert Change " + alert + ", Aleart Click end");
		}
#if UNITY_ANDROID
		void ReviewWillOpenMarketEvent() {
			MyDebug.Log("GameUtility::ReviewWillOpenMarketEvent => Cache not Interstitialed");
			CoreUtility.Me.settings.isAlreadyRated = true;
		}
		void ReviewRemindMeLaterEvent() {
			MyDebug.Log("GameUtility::ReviewRemindMeLaterEvent => Cache not Interstitialed");
		}
		void ReviewDontAskAgainEvent() {
			MyDebug.Log("GameUtility::ReviewDontAskAgainEvent => Cache not Interstitialed");
			CoreUtility.Me.settings.isNeverAskForReview = true;
		}
#endif
		#endregion
	}

	public enum AlertType {
		#region Common Alert Type
		None = -1,
		RemoveAds = 0,
		Review = 1,
		GPGClicked = 2,
		Continue4Power = 3,
		InAppWindow = 11,
		NotEnoughCoin = 12
		#endregion

	}

	public enum StoreOrigan {
		None,
		UndoPower,
		RemoveCellPower,
		SwapCellPower,
		HammerPower,
		//
		BetScreen
	}
}