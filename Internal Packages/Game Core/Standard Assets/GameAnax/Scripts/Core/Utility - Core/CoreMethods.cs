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

using UnityEngine;

using System.Collections;
using System.Globalization;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using GameAnax.Core.Extension;
using GameAnax.Core.Utility;

using Prime31;

namespace GameAnax.Core {
	public static class CoreMethods {
		public static Menus layer = Menus.Wait;
		public static Menus lastLayer = Menus.Wait;
		public static GamePlayState gameStatus = GamePlayState.Intermediate;


		public static bool isPaused { get; set; }
		public static bool isSFXPaused { get; set; }
		public static bool isBGPaused { get; set; }

		public static void SlowMotion() {
			SlowMotion(0.3f);
		}
		public static void SlowMotion(float speed) {
			Time.timeScale = speed;
		}
		public static void NormalMotion() {
			Time.timeScale = 1f;
		}

		public static void PauseGameToggle() {
			if(Time.timeScale > 0) {
				PauseGame();
			} else {
				UnPauseGame();
			}
		}
		public static void PauseGame() {
			PauseGame(true, true);
		}
		public static void PauseGame(bool isAudioPause) {
			PauseGame(isAudioPause, isAudioPause);
		}
		public static void PauseGame(bool bgPause, bool sfxPaused) {
			Time.timeScale = 0;
			isPaused = true;
			isSFXPaused = sfxPaused;
			isBGPaused = bgPause;
		}
		public static void UnPauseGame() {
			Time.timeScale = 1;
			isPaused = false;
			isSFXPaused = false;
			isBGPaused = false;
		}
		public static float GetAngleTo(this Vector2 from, Vector2 to) {
			Vector2 _diference = from - to;
			float _sign = (from.y < to.y) ? -1.0f : 1.0f;
			float elu = Vector2.Angle(Vector2.right, _diference) * _sign;
			elu = elu.GetPostiveAngle();
			return elu;
		}
		public static int GetSide(this Transform chk, Transform checkWith) {
			int retvalue = 0;
			Vector2 x1 = chk.position.Cast();
			Vector2 x2 = checkWith.transform.position.Cast();

			float an1 = 0;
			an1 = x1.GetAngleTo(x2);

			float an3 = checkWith.transform.eulerAngles.z.GetPostiveAngle();
			float dif = (an1 - an3).GetPostiveAngle();
			//MyDebug.Log("Line rtZ: {0}, ball-Line Z: {1}, dif:{2}", an3, an1, dif);
			retvalue = dif.Between(0, 180) ? 1 : -1;
			return retvalue;
		}

		public static float GetTextMeshWidth(this TextMesh mesh) {
			float width = 0;
			foreach(char symbol in mesh.text) {
				CharacterInfo info;
				if(mesh.font.GetCharacterInfo(symbol, out info, mesh.fontSize, mesh.fontStyle)) {
					width += info.advance;
				}
			}
			return width * mesh.characterSize * 0.1f;
		}


#if !UNITY_NACL || !UNITY_FLASH || !UNITY_WEBPLAYER
		public static bool SaveData(string playerID, string gameData) {
			bool retValue = false;
			retValue = IO.File.WriteFile(playerID, gameData);
			return retValue;
		}
		public static bool LoadData(string playerID, ref string gameData) {
			bool retValue = false;
			gameData = null;
			if(IO.File.FileExists(playerID)) {
				gameData = IO.File.ReadFile(playerID);
				Debug.Log(playerID + " File Found\n" + gameData);
				retValue = true;
			} else {
				Debug.Log(playerID + " File Not Found");
				retValue = false;
			}
			return retValue;
		}
#endif


		public static string Serialize<T>(T serializableObject, SerializationType typeOfSerialization) {
			string returnData = string.Empty;
			switch(typeOfSerialization) {
			case SerializationType.XML:
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				System.IO.TextWriter writer = new System.IO.StringWriter();
				serializer.Serialize(writer, serializableObject);
				returnData = writer.ToString();
				serializer = null;
				writer.Close();
				writer = null;
				break;

			case SerializationType.Binary:
				//Binary Format Deserialize using Memorystreem
				var formatter = new BinaryFormatter();
				var mf = new System.IO.MemoryStream();
				formatter.Serialize(mf, serializableObject);
				returnData = System.Convert.ToBase64String(mf.ToArray());
				mf.Close();
				formatter = null;
				mf = null;
				break;

			case SerializationType.UnityJson:
				returnData = JsonUtility.ToJson(serializableObject, true);
				break;
			}
			return returnData;
		}
		public static T Deserialize<T>(string serializedString, SerializationType typeOfSerialization) {
			object obj = null;
			//T retValue = null;
			switch(typeOfSerialization) {
			case SerializationType.XML:
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				System.IO.TextReader reader = new System.IO.StringReader(serializedString);
				obj = serializer.Deserialize(reader);
				reader.Close();
				reader = null;
				serializer = null;
				break;

			case SerializationType.Binary:
				//Binary Format Deserialize using Memorystreem
				BinaryFormatter formatter = new BinaryFormatter();
				System.IO.MemoryStream mf = new System.IO.MemoryStream(Convert.FromBase64String(serializedString));
				obj = formatter.Deserialize(mf);
				mf.Close();
				mf = null;
				formatter = null;
				break;

			case SerializationType.UnityJson:
				obj = JsonUtility.FromJson<T>(serializedString);
				break;
			}
			return (T)obj;
		}

#if UNITY_IOS
		public static void ITuenICloudSync(string filePath, bool isSync) {
			if(isSync) {
				UnityEngine.iOS.Device.ResetNoBackupFlag(filePath);
			} else {
				UnityEngine.iOS.Device.SetNoBackupFlag(filePath);
			}
		}
#endif

		public static void ClearUnityPlayerPrefs() {
			PlayerPrefs.DeleteAll();
		}

		public static void ChangeLayer(this Transform tr, int toLayer) {
			ChangeLayer(tr, toLayer, false);
		}
		public static void ChangeLayer(this Transform tr, int toLayer, bool isChangeChildLayer) {
			tr.gameObject.layer = toLayer;
			if(isChangeChildLayer) {
				for(int i = 0; i < tr.childCount; i++) {
					ChangeLayer(tr.GetChild(i), toLayer, isChangeChildLayer);
				}
			}
		}
		public static void ChangeCameraWisePosition(this Transform tr, Camera fromCam, Camera toCam) {
			tr.ChangeCameraWisePosition(fromCam, toCam, Vector3.zero);
		}
		public static void ChangeCameraWisePosition(this Transform tr, Camera fromCam, Camera toCam, Vector3 offset) {
			Vector3 pos = tr.position;
			pos = fromCam.WorldToScreenPoint(pos);
			pos = toCam.ScreenToWorldPoint(pos);
			tr.position = pos + offset;
		}

		public static float GetScreenSize(float pixelwidth, float pixelheight, float dpi) {
			float width = pixelwidth / dpi;
			float height = pixelheight / dpi;
			float size = Mathf.Pow(width, 2) + Mathf.Pow(height, 2);
			size = Mathf.Sqrt(size);
			return size;
		}

		public static IEnumerator Wait(float seconds) {
			float timeToShowNextElement = Time.realtimeSinceStartup + seconds;
			while(Time.realtimeSinceStartup < timeToShowNextElement) {
				yield return null;
			}
			yield return null;
		}



		static CultureInfo lcl;
		static bool CultureExists(string name) {
			CultureInfo[] availableCultures =
				CultureInfo.GetCultures(CultureTypes.AllCultures);

			foreach(CultureInfo culture in availableCultures) {
				if(culture.Name.Equals(name))
					return true;
			}

			return false;
		}
		public static CultureInfo GetCultureBasedLocale(string locale) {
			CultureInfo lcl;
			string[] lcl1 = locale.Split('-');
			lcl1[0] = lcl1[0].ToLower();
			if(lcl1.Length > 1) {
				lcl1[1] = lcl1[1].ToUpper();
			}
			locale = string.Join("-", lcl1);
			if(CultureExists(locale)) {
				lcl = new CultureInfo(locale, true);
			} else {
				MyDebug.Warning("Culture Locale is not available: " + locale);
				lcl = new CultureInfo("en-US", true);
			}
			return lcl;
		}

		public static void ReGenHashJsonTable(IDictionary input, ref Dictionary<string, object> output, string prefix) {
			IDictionaryEnumerator enumerator = input.GetEnumerator();
			while(enumerator.MoveNext()) {
				DictionaryEntry entry = (DictionaryEntry)enumerator.Current;
				string dType = entry.Value.GetType().ToString();
				if(dType == "System.Collections.Generic.Dictionary`2[System.String,System.Object]"
				   || dType == "Prime31.JsonObject") {
					if(string.IsNullOrEmpty(prefix)) {
						ReGenHashJsonTable((IDictionary)entry.Value, ref output, entry.Key.ToString());
					} else {
						ReGenHashJsonTable((IDictionary)entry.Value, ref output, prefix + ":" + entry.Key);
					}
				} else if(entry.Value is string ||
					entry.Value is bool ||
					entry.Value is Boolean ||
					entry.Value is byte ||
					entry.Value is Byte ||
					entry.Value is int ||
					entry.Value is Int16 ||
					entry.Value is Int32 ||
					entry.Value is Int64 ||
					entry.Value is long ||
					entry.Value is float ||
					entry.Value is decimal ||
					entry.Value is double ||
					entry.Value is Single ||
					entry.Value is Double) {
					if(string.IsNullOrEmpty(prefix)) {
						output.Add(entry.Key.ToString(), entry.Value.ToString());
					} else {
						output.Add(prefix + ":" + entry.Key, entry.Value.ToString());
					}
				} else {
					MyDebug.Info("type \"" + dType + "\" is for Key: " + entry.Key + " : " +
					Json.encode(entry.Value.ToString())
					);
				}
			}
		}

		public static int GetAndroidSDKIntValue() {
			int APIVer = 0;
#if UNITY_ANDROID && !UNITY_EDITOR
			using(var version = new AndroidJavaClass("android.os.Build$VERSION")) {
			APIVer = version.GetStatic<int>("SDK_INT");
			}
#endif
			MyDebug.Warning("API: " + APIVer);
			return APIVer;
		}

		public static byte ToByte(this float f) {
			f = Mathf.Clamp01(f);
			return (byte)(f * 255f);
		}


		public static void DeleteAllChild(this Transform target) {
			int max = target.childCount;
			for(int i = 0; i < max; i++) {
				GameObject.Destroy(target.GetChild(i).gameObject);
			}
		}

		public static float LerpWithoutClamp(float a, float b, float t) {
			return (b - a) * t + a;
		}
		public static Vector2 LerpWithoutClamp(Vector2 a, Vector2 b, float t) {
			return (b - a) * t + a;
		}
		public static Vector3 LerpWithoutClamp(Vector3 a, Vector3 b, float t) {
			return (b - a) * t + a;
		}

		#region Random Items 
		#region "Unique Random till list once not complete"
		#region "Unique Random till list once not complete"
		public static int GetUniqueRandomIndex(ref List<int> remain, ref List<int> main, ref int last) {
			return GetUniqueRandomIndex(ref remain, ref main, ref last, true);
		}
		public static int GetUniqueRandomIndex(ref List<int> remain, ref List<int> main, ref int last, bool nonRepeat) {
			int uniqueGetTryCount = 0;
			getNewIdx:
			if(remain.Count <= 0) {
				remain.Clear();
				remain.AddRange(main.ToArray());
			}
			int rndIdx = UnityEngine.Random.Range(0, remain.Count);
			//MyDebug.Log(remain.Count + " _ " + main.Count + " _ " + rndIdx);
			int curIdx = remain[rndIdx];
			if(curIdx.Equals(last) && nonRepeat && uniqueGetTryCount < 10) {
				uniqueGetTryCount++;
				goto getNewIdx;
			}
			remain.RemoveAt(rndIdx);
			last = curIdx;
			return curIdx;
		}
		public static void FillUniqueIndex(int count, ref List<int> main) {
			main.Clear();
			for(int im = 0; im < count; im++) {
				main.Add(im);
			}
		}
		#endregion
		#endregion

		public static int Random01() {
			return (UnityEngine.Random.Range(0, 2));
		}
		public static bool RandomBools() {
			return (UnityEngine.Random.Range(0, 2) == 0);
		}
		public static int RandomOnlyOne() {
			return ((UnityEngine.Random.Range(0, 2) * 2) - 1);
		}
		public static Vector2 RandomV2() {
			return new Vector2(UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360)).normalized;
		}
		public static Vector3 RandomV3() {
			return new Vector3(UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360)).normalized;
		}

		public static Color RandomColor(float min, float max) {
			return new Color(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), 1);
		}
		public static Color RandomColor() {
			return RandomColor(0, 1);
		}
		#endregion

		public static int[] MagicNumberGenerate(int total, int count) {
			int[] generateNumber = new int[count];

			if(count == 0 || total == 0) {
				return generateNumber;
			}
			int currentTotal = 0;

			for(int i = 0; i < count; i++) {

				if(i == count - 1) {
					generateNumber[i] = total - currentTotal;
					continue;
				}
				generateNumber[i] = UnityEngine.Random.Range(2, Mathf.Min(((total - (count - i) * 2) - currentTotal), Mathf.FloorToInt(total / (count - 1 - i))));
				currentTotal += generateNumber[i];
			}
			return generateNumber;
		}
	}

	/// <summary>
	/// Possible Menus for Games
	/// </summary>
	[Flags]
	public enum Menus {
		Popup = 1,
		Tutorial = 1 << 1,
		Splash = 1 << 2,
		//
		MainMenu = 1 << 3,
		Option = 1 << 4,
		Store = 1 << 5,
		FreeCoin = 1 << 6,
		//
		GameOver = 1 << 7,
		Pause = 1 << 8,
		Gameplay = 1 << 9,
		//
		MPOption = 1 << 10,
		MPFriendList = 1 << 11,
		MPBetScreen = 1 << 12,
		MPRandomFind = 1 << 13,
		//
		Language = 1 << 14,
		AboutUs = 1 << 15,
		MoreGames = 1 << 16,
		//
		Leaderbard = 1 << 17,
		Achievements = 1 << 18,
		Missions = 1 << 19,
		//
		ControlSelection = 1 << 20,
		LevelSelection = 1 << 21,
		CharacterSelection = 1 << 22,
		//
		Wait = 1 << 23,
		Social = 1 << 24,
		//Other Start from 24
	}

	/// <summary>
	/// Game play state.
	/// </summary>
	public enum GamePlayState {
		Splash,
		MainMenu,
		LevelSelection,
		Gameplay,
		Pause,
		Tutorial,
		Gameover,
		Intermediate,
		LevelComplete
	}

	[System.Serializable]
	public enum SerializationType {
		XML,
		Binary,
		UnityJson
	}
}
