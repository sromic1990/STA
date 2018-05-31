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

using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.iOS;

using Rand = UnityEngine.Random;

using GameAnax.Core.NotificationSystem;
using GameAnax.Core.Delegates;
using GameAnax.Core.Extension;
using GameAnax.Core.IO;
using GameAnax.Core.Net;
using GameAnax.Core.Security;
using GameAnax.Core.Singleton;
using GameAnax.Core.Sound;
using GameAnax.Core.UI.Buttons;
using GameAnax.Core.Utility.Popup;
using Network = GameAnax.Core.Net.Network;

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

namespace GameAnax.Core.Utility {
	[PersistentSignleton(true, true)]
	public class CoreUtility : SingletonAuto<CoreUtility> {
		#region Events and Delegats
		public event InAppEventHandler InAppFailEvent;
		public event InAppEventHandler InAppSucessEvent;
		public event FreeContinueSucessEventHandler FreeContinueSucess;
		public event FreeLevelSucessEventHandler FreeLevelSucess;
		public event FreeCashSucessEventHandler FreeCashSucess;

		public void OnInAppFail(string sku) {
			if(null != InAppFailEvent)
				InAppFailEvent(sku);
		}
		public void OnInAppSucess(string sku) {
			if(null != InAppSucessEvent)
				InAppSucessEvent(sku);
		}
		public void OnFreeContinueSucess() {
			if(null != FreeContinueSucess)
				FreeContinueSucess();
		}
		public void OnFreeLevelSucess(string lvlName) {
			if(null != FreeLevelSucess)
				FreeLevelSucess(lvlName);
		}
		public void OnFreeCashSucess(string currency, long amount) {
			if(null != FreeCashSucess)
				FreeCashSucess(currency, amount);
		}
		#endregion

		public const string LOW_NET_SPEED = "Your internet speed is low.\n\nYou might won't get\n100% game experince.";
		public const string CHECK_NET_SPEED = "Checking your internet speed.\n\nIt will take a few moments.";

		#region public non-editor inspector members
		[HideInInspector]
		public Settings settings;
		[HideInInspector]
		public float appVersion { get; set; }
		[HideInInspector]
		public string platform { get; set; }
		[HideInInspector]
		public CultureInfo locale;
		[HideInInspector]
		public ShareMode shareMode = ShareMode.None;
		[HideInInspector]
		public double netSpeedKBps;
		[HideInInspector]
		public bool isGameStarted;
		#endregion

		#region public members
		public string settingFile;
		public bool isEncryptDataFile = false;
		public string encryptionKey;

		[Space(5)]
		public GameObject fakeFadeInOut;

		[Space(5)]
		public Menus activeMenu;
		public List<Menus> lastMenuStack;
		[Space(5)]
		[Range(40, 100)]
		public double minNetSpeedKBpsRequire = 48;
		#endregion

		#region Mono Actions
		// Use this for initialization
		void Awake() {
#if UNITY_IOS
			platform = "iOS";
			appVersion = 1f;
#endif
#if UNITY_ANDROID
			platform = "Android";
			appVersion = 1;
#endif
			NotificationCenter.Me.AddObserver(this, "HoverInPopEffect");
			NotificationCenter.Me.AddObserver(this, "HoverOutPopEffect");

			NotificationCenter.Me.AddObserver(this, "RotateObjConti");
			NotificationCenter.Me.AddObserver(this, "StopRotatingObj");
			NotificationCenter.Me.AddObserver(this, "ResetRotatingObj");
			NotificationCenter.Me.AddObserver(this, "RotateToObjPartial");
			NotificationCenter.Me.AddObserver(this, "RotateByObjPartial");
			NotificationCenter.Me.AddObserver(this, "RotateByObjAsSyncEffect");

			NotificationCenter.Me.AddObserver(this, "AdsSuccess");
			NotificationCenter.Me.AddObserver(this, "AdsFail");
			NotificationCenter.Me.AddObserver(this, "AdsCancelled");

			Screen.sleepTimeout = 10;
			Application.targetFrameRate = 60;
			this.SetQuality();
			locale = CoreMethods.GetCultureBasedLocale("en-US");
		}
		IEnumerator Start() {
			MyDebug.Log("CoreUtility::Start Called");
			this.LoadSettings();
			CoreMethods.gameStatus = GamePlayState.Splash;
			this.SetAndroidAleartTheme();
			yield return new WaitForEndOfFrame();
			//TODO: Set Launch Effect to Analytic System
			//
			settings.gameLaunchCount++;
			this.SaveSettings();
			yield return Resources.UnloadUnusedAssets();
			this.isGameStarted = true;
			MyDebug.Log("CoreUtility::Start Complete");
			BGSound.Me.Play(0);
		}

		void OnApplicationPause(bool isPaused) {
			if(!isGameStarted) {
				return;
			}
			if(isPaused) this.SaveSettings();
		}
		void OnApplicationQuit() {
			this.SaveSettings();
		}
		#endregion

		#region Utility Mehtods

		#region Update & Analytic methods
		public void UpdateVersion() {
			settings.isAlreadyRated = false;
			settings.isNeverAskForReview = false;
#if UNITY_ANDROID
#if ETCETERA
		EtceteraB.resetAskForReview();
#endif
#endif
			SaveSettings();
		}
		#endregion

		#region Load and Save Settings
		public bool SaveSettings() {
			bool retValue;
			try {
				string settingData;
				settingData = CoreMethods.Serialize<Settings>(settings, SerializationType.XML);
				if(isEncryptDataFile) {
					settingData = Encryption.RFC2898Encrypt(settingData, new EncOption() { secureKey = encryptionKey });
				}
				retValue = CoreMethods.SaveData(settingFile, settingData);
			} catch(Exception ex) {
				Debug.LogError(ex.Message + '\n' + ex.Data);
				retValue = false;
				Application.Quit();
			}
			return retValue;
		}

		public bool LoadSettings() {
			bool retValue = false;
			bool isPlayerDataFound;
			string settingData = string.Empty;
			try {
				isPlayerDataFound = CoreMethods.LoadData(settingFile, ref settingData);
				bool isEncrypt;
				if(isPlayerDataFound) {
					isEncrypt = !settingData.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase);
				} else {
					isEncrypt = false;
				}
				if(!string.IsNullOrEmpty(settingData) && isEncrypt)
					settingData = Encryption.RFC2898Encrypt(settingData, new EncOption() { secureKey = encryptionKey });

				if(isPlayerDataFound && !string.IsNullOrEmpty(settingData)) {
					Settings tempPrefabs;
					tempPrefabs = CoreMethods.Deserialize<Settings>(settingData, SerializationType.XML) as Settings;
					if(null != tempPrefabs) {
						settings = tempPrefabs;
						settingData = string.Empty;
						retValue = true;
					} else {
						Debug.Log("CoreUtility::Setting Data => currepted data found");
						GeDefaultSettings();
					}
				} else {
					Debug.Log("CoreUtility::Setting Data => Data not found");
					GeDefaultSettings();
				}
			} catch(Exception ex) {
				Debug.LogWarning(ex.Message + '\n' + ex.Data);
				GeDefaultSettings();
				retValue = false;
			}
			return retValue;
		}
		void GeDefaultSettings() {
			settings = new Settings();
			MyDebug.Log("GeDefaultSettings => Save Progress");
			SaveSettings();
		}

		#endregion

		#region Basic Game Mehtods (Common for most game)
		#region Own Event Manager
		public event ScreenChangeEventHandler ScreenChangeEvent;
		public void OnScreenChange(string screenName, string status) {
			if(null != ScreenChangeEvent) {
				ScreenChangeEvent(screenName, status);
			}
		}
		#endregion

		public string GetDummyID() {
			return string.Format("{0:ssmmHHddMMyyyy}", DateTime.UtcNow);
		}
		public void CheckFBIDnName() {
			string id = GetDummyID();
			string gusetName = "Guest_" + id;

			settings.myFBInfo.name = string.IsNullOrEmpty(settings.myFBInfo.name) ? gusetName : settings.myFBInfo.name;
			settings.myFBInfo.id = string.IsNullOrEmpty(settings.myFBInfo.id) ? id : settings.myFBInfo.id;
		}
		public string GetRandomShortName(string name) {
			string nametoShow = name;
			string[] s = nametoShow.Split(' ');
			if(s.Length > 0) {
				int fullID = Rand.Range(0, s.Length);
				nametoShow = "";
				for(int i = 0; i < s.Length; i++) {
					if(i == fullID)
						nametoShow += " " + s[i];
					else if(s[i].Length > 0)
						nametoShow += " " + s[i].Left(1) + ".";
				}
			}
			nametoShow = nametoShow.Length > 11 ? nametoShow.Left(11).Trim() : nametoShow;
			return nametoShow;
		}

		#region Plugin related Method



		public string freeLelveNameOrID = "";
		public FreeContentType freeContent = FreeContentType.None;


		void AdsSuccess(string provider) {
			switch(freeContent) {
			case FreeContentType.FreeContinue:
				OnFreeContinueSucess();
				break;
			case FreeContentType.FreeLevel:
				OnFreeLevelSucess(freeLelveNameOrID);
				break;
			case FreeContentType.FreeCash:
				OnFreeCashSucess("", 0);
				break;
			case FreeContentType.None:
			default:
				break;
			}
			freeContent = FreeContentType.None;
		}
		void AdsFail(string provider) {
			freeContent = FreeContentType.None;
			if(!freeContent.Equals(FreeContentType.None))
				PopupMessages.Me.NoFreeContentMessage();
		}
		void AdsCancelled(string provider) {
			freeContent = FreeContentType.None;
		}
		#endregion

		#region Utility Methods
		public string ConvertURL(string oldURL) {
			string newURL = oldURL;
#if UNITY_IOS && !UNITY_EDITOR
		if(File.GetOSVersion() >= 9 && oldURL.StartsWith("http://")) {
			MyDebug.Log("AdsLayout::ReadInfoFromServer => iOS version is higher/equal than 9 so convert Normal URL to Sequre URL");
			newURL = oldURL.Replace("http://", "https://");
		} 
#endif
			return newURL;
		}
		public void ChangeCam(GameObject obj2ChngCam, Camera fromCam, Camera toCam) {
			Vector3 pos = obj2ChngCam.transform.position;
			Vector3 tpPos = fromCam.WorldToViewportPoint(obj2ChngCam.transform.position);
			tpPos = toCam.ViewportToWorldPoint(tpPos);
			tpPos.z = pos.z;
			obj2ChngCam.transform.position = tpPos;
			obj2ChngCam.layer = toCam.gameObject.layer;
			obj2ChngCam.transform.ChangeLayer(toCam.gameObject.layer, true);
		}
		public void Fade(GameObject menu, float toVal, float time = 1f) {
			iTween.ColorTo(menu, iTween.Hash("a", toVal, "includechildren", true, "time", time,
											 "ignoretimescale", true));
		}
		public void MoveY(GameObject obj, float yPos, float moveTime) {
			iTween.MoveTo(obj, iTween.Hash("y", yPos, "time", moveTime,
				"ignoretimescale", true, "easeType", iTween.EaseType.linear));
		}
		public void PunchObject(GameObject g, float x, float y, float time) {
			iTween.Stop(g);
			iTween.ScaleTo(g, iTween.Hash("x", x, "y", y, "time", time, "easeType", iTween.EaseType.easeOutElastic));
		}
		#endregion

		#region Fake Tint, Loading etc.
		public void FadeInTint(float time = 1f) {
			CancelInvoke("DeactiveFakeTint");
			if(null == fakeFadeInOut) return;
			fakeFadeInOut.SetActive(true);
			iTween.ColorTo(fakeFadeInOut, iTween.Hash("a", 1, "includechildren", true, "time", time));
		}
		public void FadeOutTint(float time = 1f) {
			if(null == fakeFadeInOut) return;
			iTween.ColorTo(fakeFadeInOut, iTween.Hash("a", 0, "includechildren", true, "time", time));
			Invoke("DeactiveFakeTint", time + 0.04f);
		}
		void DeactiveTint() {
			if(null == fakeFadeInOut) return;
			fakeFadeInOut.SetActive(false);
		}
		#endregion

		#region Code base animation Methods
		public IEnumerator MenuInFrom(GameObject menu, Vector3 from) {
			yield return StartCoroutine(MenuInFrom(menu, from, 1f, iTween.EaseType.easeOutBack));
		}
		public IEnumerator MenuInFrom(GameObject menu, Vector3 from, float time) {
			yield return StartCoroutine(MenuInFrom(menu, from, time, iTween.EaseType.easeOutBack));
		}
		public IEnumerator MenuInFrom(GameObject menu, Vector3 from, iTween.EaseType easeType) {
			yield return StartCoroutine(MenuInFrom(menu, from, 1f, easeType));
		}
		public IEnumerator MenuInFrom(GameObject menu, Vector3 from, float time, iTween.EaseType easeType) {
			iTween.Stop(menu, "move");
			Vector3 pos = menu.transform.position;
			pos.x = from.x; pos.y = from.y;
			menu.transform.position = pos;
			iTween.MoveTo(menu, iTween.Hash("x", 0f, "y", 0f, "time", time,
								"ignoretimescale", true, "easetype", easeType));
			yield return StartCoroutine(CoreMethods.Wait(time));
			//iTween.Stop(menu, "move");

		}

		public IEnumerator MenuOutTo(GameObject menu, Vector3 to) {
			yield return StartCoroutine(MenuOutTo(menu, to, 1f, iTween.EaseType.easeInBack));
		}
		public IEnumerator MenuOutTo(GameObject menu, Vector3 to, float time) {
			yield return StartCoroutine(MenuOutTo(menu, to, time, iTween.EaseType.easeInBack));
		}
		public IEnumerator MenuOutTo(GameObject menu, Vector3 to, iTween.EaseType easeType) {
			yield return StartCoroutine(MenuOutTo(menu, to, 1f, easeType));
		}
		public IEnumerator MenuOutTo(GameObject menu, Vector3 to, float time, iTween.EaseType easeType) {
			iTween.Stop(menu, "move");
			iTween.MoveTo(menu, iTween.Hash("x", to.x, "y", to.y, "time", time,
								"ignoretimescale", true, "easetype", easeType));
			yield return StartCoroutine(CoreMethods.Wait(time));
			//iTween.Stop(menu, "move");

		}


		public void ScaleObjTo_1_1_1_forLang(List<GameObject> pumpObjs) {
			foreach(GameObject go in pumpObjs) {
				if(null == go) {
					continue;
				}
				iTween.Stop(go);
				go.transform.localScale = new Vector3(1, 1, 1);
			}
		}
		public void ScaleObjTo_0_0_1_forLang(List<GameObject> pumpObjs) {
			foreach(GameObject go in pumpObjs) {
				if(null == go) {
					continue;
				}
				iTween.Stop(go);
				go.transform.localScale = new Vector3(0, 0, 1);
			}
		}

		public IEnumerator PunchInObjects(List<GameObject> pumpObjs, bool isRandomWait) {
			yield return StartCoroutine(PunchInObjects(pumpObjs, 1f, 0.5f, 0f, isRandomWait));
		}
		public IEnumerator PunchInObjects(List<GameObject> pumpObjs, bool isRandomWait, float rndMin, float rndMax) {
			yield return StartCoroutine(PunchInObjects(pumpObjs, 1f, 0.5f, 0f, isRandomWait, rndMin, rndMax));
		}
		public IEnumerator PunchInObjects(List<GameObject> pumpObjs, float pumpTo = 1f, float time = 0.5f,
			float wait = 0f, bool isRandomWait = false, float rndMin = 0.05f, float rndMax = 0.1f) {
			if(isRandomWait) {
				pumpObjs.Shuffle();
			}

			foreach(GameObject go in pumpObjs) {
				if(null == go) {
					continue;
				}
				iTween.Stop(go, "scaleto");
				iTween.ScaleTo(go, iTween.Hash("x", pumpTo, "y", pumpTo, "time", time,
					"islocal", true, "ignoretimescale", true, "easetype", iTween.EaseType.easeOutBounce));

				if(isRandomWait) {
					yield return StartCoroutine(CoreMethods.Wait(UnityEngine.Random.Range(rndMin, rndMax)));
				} else if(wait > 0) {
					yield return StartCoroutine(CoreMethods.Wait(wait));
				}
			}
		}

		public IEnumerator PunchOutObjects(List<GameObject> pumpObjs, float pumpTo = 0f, float time = 0.1f, float wait = 0f) {
			foreach(GameObject go in pumpObjs) {
				if(null == go) { continue; }

				iTween.Stop(go, "scaleto");
				iTween.ScaleTo(go, iTween.Hash("x", pumpTo, "y", pumpTo, "time", time,
					"islocal", true, "ignoretimescale", true, "easetype", iTween.EaseType.linear));
				if(wait > 0) {
					yield return StartCoroutine(CoreMethods.Wait(wait));
				}
			}
		}

		public void StartPumpObject(GameObject go) {
			iTween.Stop(go, "scaleto");
			iTween.ScaleTo(go, iTween.Hash("x", 0.8f, "y", 0.8f,
				"time", 0.5f, "easeType", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
		}
		public void StopPumpObject(GameObject go) {
			iTween.Stop(go, "scaleto");
			iTween.ScaleTo(go, iTween.Hash("x", 1f, "y", 1, "time", 0.5f, "easeType", iTween.EaseType.linear));
		}

		public void FlyObjectEffect(GameObject aniGO, Vector3 pos) {
			iTween.MoveTo(aniGO, iTween.Hash("x", pos.x, "y", pos.y,
				"time", 0.5f, "easeType", iTween.EaseType.linear));
			Destroy(aniGO, 0.5f);
		}

		public void RotateObjConti(ButtonEventArgs args) {
			float angleToRoate = 0;
			float.TryParse(args.data, out angleToRoate);
			iTween.Stop(args.container);
			iTween.RotateBy(args.container, iTween.Hash("z", angleToRoate, "time", 1f, "ignoretimescale", true,
				"islocal", true, "easetype", iTween.EaseType.linear, "loopType", iTween.LoopType.loop));
		}
		public void StopRotatingObj(ButtonEventArgs args) {
			iTween.Stop(args.container);
		}

		public void ResetRotatingObj(ButtonEventArgs args) {
			iTween.Stop(args.container);
			args.container.transform.localEulerAngles = Vector3.zero;
		}
		public void RotateToObjPartial(ButtonEventArgs args) {
			float angleToRoate = 0;
			float.TryParse(args.data, out angleToRoate);
			iTween.Stop(args.container);
			iTween.RotateTo(args.container, iTween.Hash("z", angleToRoate, "time", 0.25f, "ignoretimescale", true,
				"islocal", true, "easetype", iTween.EaseType.linear));
		}
		public void RotateByObjPartial(ButtonEventArgs args) {
			float angleToRoate = 0;
			float.TryParse(args.data, out angleToRoate);
			iTween.Stop(args.container);
			if(angleToRoate > 0) {
				iTween.RotateBy(args.container, iTween.Hash("z", angleToRoate, "time", 0.25f, "ignoretimescale", true,
					"islocal", true, "easetype", iTween.EaseType.linear));
			}
		}
		public void RotateByObjAsSyncEffect(ButtonEventArgs args) {
			float angleToRoate = 0;
			float.TryParse(args.data, out angleToRoate);
			iTween.Stop(args.container);
			iTween.RotateTo(args.container, iTween.Hash("z", angleToRoate, "time", 0.5f, "ignoretimescale", true,
				"islocal", true, "easetype", iTween.EaseType.linear, "loopType", iTween.LoopType.pingPong));
		}

		public void HoverInPopEffect(ButtonEventArgs args) {
			if(null == args.extraGameObject) {
				return;
			}
			float scale = 0.95f;
			if(!string.IsNullOrEmpty(args.data)) {
				float.TryParse(args.data, out scale);
				if(scale <= 0) {
					scale = 0.95f;
				}
			}
			args.extraGameObject.ForEach((ego) => {
				iTween.Stop(ego, "Hover");
				iTween.ScaleTo(ego, iTween.Hash("name", "Hover", "x", scale, "y", scale,
					"time", 0.02f, "isLocal", true, "ignoretimescale", true, "easeType", iTween.EaseType.linear));
			});
		}
		public void HoverOutPopEffect(ButtonEventArgs args) {
			if(null == args.extraGameObject) {
				return;
			}
			float scale = 1.00f;
			if(!string.IsNullOrEmpty(args.data)) {
				float.TryParse(args.data, out scale);
				if(scale <= 0) {
					scale = 1.00f;
				}
			}
			args.extraGameObject.ForEach((ego) => {

				iTween.Stop(ego, "Hover");
				iTween.ScaleTo(ego, iTween.Hash("name", "Hover", "x", scale, "y", scale,
					"time", 0.02f, "isLocal", true, "ignoretimescale", true, "easeType", iTween.EaseType.linear));
			});
		}
		#endregion

		#region Performance related Method
		static SpeedCheckResult _speedResult;
		double _bps;
		double _kBps;
		double _mBps;

		public IEnumerator CheckInternetSpeed() {
			yield return StartCoroutine(CoreMethods.Wait(1f));
			_speedResult = Network.CheckSpeed();
			_bps = _speedResult.Speed;
			_kBps = (_bps / 1024d);
			_mBps = (_kBps / 1024d);
			netSpeedKBps = _kBps;

			if(_speedResult.Speed > 0) {
				string speedLog = string.Format("Speed in B/s: {0}, KB/s: {1}, MB/s: {2}, Class: {3}\n", _bps, _kBps, _mBps, _speedResult.Class);
				MyDebug.Log(speedLog);
				//if(NetSpeedKBps > MinNetSpeedKBps) {
				//} else { }
			}
			//else { }
		}

		public void SetQuality() {
			//	6	Level 6
			//	5	Fantastic
			//	4	Beautiful
			//	3	Good
			//	2	Simple
			//	1	Fast
			//	0	Fastest

			MyDebug.Log("QualitySettingIndex: " + QualitySettings.GetQualityLevel());
			int qualityIndex = 3;
#if UNITY_IOS
			switch(Device.generation) {
			case DeviceGeneration.iPad1Gen:
			case DeviceGeneration.iPad2Gen:

			case DeviceGeneration.iPadMini1Gen:

			case DeviceGeneration.iPhone:
			case DeviceGeneration.iPhone3G:
			case DeviceGeneration.iPhone3GS:

			case DeviceGeneration.iPodTouch1Gen:
			case DeviceGeneration.iPodTouch2Gen:
			case DeviceGeneration.iPodTouch3Gen:
				qualityIndex = 1;
				break;

			case DeviceGeneration.iPhone4:
			case DeviceGeneration.iPodTouch4Gen:
				qualityIndex = 2;
				break;

			case DeviceGeneration.iPadMini2Gen:

			case DeviceGeneration.iPodTouch5Gen:

			case DeviceGeneration.iPhone4S:
			case DeviceGeneration.iPhone5C:
				qualityIndex = 4;
				break;

			case DeviceGeneration.iPadMini3Gen:
			case DeviceGeneration.iPadMini4Gen:

			case DeviceGeneration.iPad3Gen:
			case DeviceGeneration.iPad4Gen:

			case DeviceGeneration.iPadAir1:
			case DeviceGeneration.iPadAir2:

			case DeviceGeneration.iPadPro1Gen:

			case DeviceGeneration.iPhone5:
			case DeviceGeneration.iPhone5S:
			case DeviceGeneration.iPhone6:
			case DeviceGeneration.iPhone6Plus:
			case DeviceGeneration.iPhone6S:
			case DeviceGeneration.iPhone6SPlus:
			case DeviceGeneration.iPhone7:
			case DeviceGeneration.iPhone7Plus:
			case DeviceGeneration.iPhone8:
			case DeviceGeneration.iPhone8Plus:
			case DeviceGeneration.iPhoneX:
				qualityIndex = 5;
				break;

			default:
				qualityIndex = 3;
				break;
			}
#endif
#if UNITY_ANDROID || UNITY_TIZEN
			if(SystemInfo.systemMemorySize <= 512) {
				qualityIndex = 3;
			} else if(SystemInfo.systemMemorySize.Between(512, 1024)) {
				qualityIndex = 4;
			} else if(SystemInfo.systemMemorySize >= 1024) {
				qualityIndex = 5;
			}
#endif
#if UNITY_WP8 || UNITY_WP_8_1
		if (SystemInfo.systemMemorySize > 512) {
			qualityIndex = 3;
		} else {
			qualityIndex = 1;
		}
#endif
#if UNITY_BLACKBERRY
		qualityIndex = 3;
#endif
			qualityIndex = Mathf.Clamp(qualityIndex, 0, QualitySettings.names.Length - 1);
			MyDebug.Log("QualitySettingIndex: " + qualityIndex);
			QualitySettings.SetQualityLevel(qualityIndex);
		}

		#endregion

		#region Lighting effect
		Gradient _rendAbGrad;
		GradientColorKey[] _rendAbGradCK;
		GradientAlphaKey[] _rendAbGradAK;
		public Color GetAmbientEquatorColor(Color skyColor, Color groundColor) {
			_rendAbGrad = new Gradient();
			_rendAbGradCK = new GradientColorKey[2];
			_rendAbGradCK[0].color = skyColor;
			_rendAbGradCK[0].time = 0.0f;
			_rendAbGradCK[1].color = groundColor;
			_rendAbGradCK[1].time = 1.0f;

			_rendAbGradAK = new GradientAlphaKey[2];
			_rendAbGradAK[0].alpha = 1.0f;
			_rendAbGradAK[0].time = 0.0f;
			_rendAbGradAK[1].alpha = 1.0f;
			_rendAbGradAK[1].time = 1.0f;
			_rendAbGrad.SetKeys(_rendAbGradCK, _rendAbGradAK);
			return _rendAbGrad.Evaluate(0.5f);

		}
		public void UpdateAmbiant(Color ambientSkyColor, Color ambientGroundColor, Color ambientEquatorColor) {
			RenderSettings.ambientSkyColor = ambientSkyColor;
			RenderSettings.ambientGroundColor = ambientGroundColor;
			RenderSettings.ambientEquatorColor = ambientEquatorColor;
		}
		public void UpdateAmbiant(Color ambientSkyColor, Color ambientGroundColor) {
			Color ambientEquatorColor = GetAmbientEquatorColor(ambientSkyColor, ambientGroundColor);
			UpdateAmbiant(ambientSkyColor, ambientGroundColor, ambientEquatorColor);
		}
		#endregion
		#endregion
		#endregion

		#region Etcetera
		public void ShowMessage(string title, string message) {
			ShowMessage(title, message, new[] { "Close" });
		}
		public void ShowMessage(string title, string message, string[] buttons) {
			if(string.IsNullOrEmpty(title)) {
				//title = APPNAME;
			}
			if(null == buttons || buttons.Length <= 0) {
				buttons = new[] { "Close" };
			}
#if UNITY_ANDROID && ETCETERA && !UNITY_EDITOR
		if(buttons.Length == 1) {
			EtceteraB.showAlert(title, message, buttons[0]);
		} else if(buttons.Length >= 2) {
			EtceteraB.showAlert(title, message, buttons[0], buttons[1]);
		}
#elif UNITY_IOS && ETCETERA && !UNITY_EDITOR
		EtceteraB.showAlertWithTitleMessageAndButtons(title, message, buttons);
#elif UNITY_EDITOR
#endif
			MyDebug.Log(title + "\n\n" + message);
		}

		public void SetAndroidAleartTheme() {
#if UNITY_ANDROID && ETCETERA
		//THEME_DEVICE_DEFAULT_LIGHT =5
		//THEME_DEVICE_DEFAULT_DARK = 4
		//THEME_HOLO_LIGHT= 3
		//THEME_HOLO_DARK = 2
		//THEME_TRADITIONAL = 1
		EtceteraB.setAlertDialogTheme(5);
#endif
		}
		#endregion

		public bool ShowInternetConnection() {
			bool retVal = Network.IsInternetConnection();
			if(!retVal) {
				PopupMessages.Me.InternetConnectionMessgae();
			}
			return retVal;
		}
		public void SetLastStack(Menus m) {
			if(!lastMenuStack.Contains(m))
				lastMenuStack.Insert(0, activeMenu);
		}
	}
	public enum FreeContentType {
		None = 0,
		FreeContinue = 1,
		FreeLevel = 2,
		FreeCash = 3
	}

	[Serializable]
	public enum ShareMode {
		None = -1,
		Game = 0,
		Highscore = 1,
		Challenge = 2
	}

}