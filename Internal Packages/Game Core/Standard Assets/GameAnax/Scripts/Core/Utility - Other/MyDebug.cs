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


namespace GameAnax.Core.Utility {
	public class MyDebug {
		static string _logDataOnScreen = string.Empty;
		static string _logDataOnFile = string.Empty;
#if MYDEBUG
		static GUIStyle _style;
		static Rect _startRect;
		static bool _allowDrag, _showLogOnScreen;
		static Vector2 _scrollPosition;
		static int _guiX, _guiY, _guiwidth, _guiheight, _padding, _hbuttons, _vbuttons;
		static string _buttonText = "";
#endif
		public static bool isLogDataOnScreen = true;
		public static bool isLogDataOnFile = false;
		public static bool isLogDataOnConsole = true;
		static void Awake() {
#if MYDEBUG
			_guiX = _guiY = _padding = 8;

			_hbuttons = UnityEngine.Camera.main.aspect > 1f ? 5 : 2;
			_vbuttons = UnityEngine.Camera.main.aspect > 1f ? 10 : 15;

			_guiwidth = (Screen.width - ((_hbuttons + 2) * _padding)) / _hbuttons;
			_guiheight = (Screen.height - ((_vbuttons + 2) * _padding)) / _vbuttons;

			_startRect = new Rect(_guiX, _guiheight + (_padding * 2), Screen.width - (_padding * 2), (Screen.height / 2) - _guiheight - (_padding * 2));
#endif
		}
#if MYDEBUG
		void OnGUI() {
			if(null == _style) {
				_style = new GUIStyle(GUI.skin.textField);
				_style.richText = true;
				_style.alignment = TextAnchor.UpperLeft;
			}

			_guiX = _padding;
			_guiY = _padding;

			_buttonText = _showLogOnScreen ? "Hide Log" : "Show Log";
			if(GUI.Button(new Rect(_guiX, _guiY, _guiwidth, _guiheight), _buttonText)) {
				_showLogOnScreen = !_showLogOnScreen;
			}
			if(_showLogOnScreen) {
				_startRect = GUI.Window(0, _startRect, DoMyWindow, "");
			}
		}

		void DoMyWindow(int windowID) {
			GUILayout.BeginArea(new Rect(_padding, _padding, _startRect.width - (_padding * 2), _startRect.height - (_padding * 2)));
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition,
				GUILayout.Width(_startRect.width - (_padding * 2)), GUILayout.Height(_startRect.height - (_padding * 2)));
			GUILayout.Label(_logDataOnScreen, _style);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			if(_allowDrag) {
				GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
			}
		}
#endif
		static string _logTime {
			get {
				return string.Format("{0}:{1}:{2}.{3}",
							   DateTime.Now.Hour, DateTime.Now.Minute,
							   DateTime.Now.Second, DateTime.Now.Millisecond);
			}

		}
		const string logColor = "#ffffffff";
		const string infoColor = "#ffffffff";
		const string warningColor = "#ffa500ff";
		const string errorColor = "#ff0000ff";

		public static void Log(string data, params object[] para) {
			Log(true, data, para);
		}
		public static void Log(bool printInEditor, string data, params object[] para) {
			object formatedSting = string.Format(data, para);
			Log(printInEditor, formatedSting);
		}
		public static void Log(object data) {
			Log(true, data);

		}
		public static void Log(bool printInEditor, object data) {
			string s = string.Format("l: @ {0}: {1}", _logTime, data);
			LogOnScreenOrFile(s, logColor);

#if UNITY_EDITOR
			//s = string.Format("<color={0}>{1}</color>", logColor, s);
#endif
#if MYDEBUG && UNITY_EDITOR
			if(isLogDataOnConsole && printInEditor) Debug.Log(s);
#endif
#if MYDEBUG && !UNITY_EDITOR
			if(isLogDataOnConsole) Debug.Log(s);
#endif

		}

		public static void Info(string data, params object[] para) {
			Info(true, data, para);
		}
		public static void Info(bool printInEditor, string data, params object[] para) {
			object formatedSting = string.Format(data, para);
			Info(printInEditor, formatedSting);
		}
		public static void Info(object data) {
			Info(true, data);

		}
		public static void Info(bool printInEditor, object data) {
			string s = string.Format("i: @ {0}: {1}", _logTime, data);
			LogOnScreenOrFile(s, logColor);
#if UNITY_EDITOR
			//s = string.Format("<color={0}>{1}</color>", infoColor, s);
#endif
#if MYDEBUG && UNITY_EDITOR
			if(isLogDataOnConsole && printInEditor) Debug.Log(s);
#endif
#if MYDEBUG && !UNITY_EDITOR
			if(isLogDataOnConsole) Debug.Log(s);
#endif

		}

		public static void Warning(string data, params object[] para) {
			Warning(true, data, para);
		}
		public static void Warning(bool printInEditor, string data, params object[] para) {
			object formatedSting = string.Format(data, para);
			Warning(printInEditor, formatedSting);
		}
		public static void Warning(object data) {
			Warning(true, data);

		}
		public static void Warning(bool printInEditor, object data) {
			string s = string.Format("w: @ {0}: {1}", _logTime, data);
			LogOnScreenOrFile(s, logColor);

#if UNITY_EDITOR
			//s = string.Format("<color={0}>{1}</color>", warningColor, s);
#endif
#if MYDEBUG && UNITY_EDITOR
			if(isLogDataOnConsole && printInEditor) Debug.LogWarning(s);
#endif
#if MYDEBUG && !UNITY_EDITOR
			if(isLogDataOnConsole) Debug.LogWarning(s);
#endif

		}

		public static void Error(string data, params object[] para) {
			Error(true, data, para);
		}
		public static void Error(bool printInEditor, string data, params object[] para) {
			object formatedSting = string.Format(data, para);
			Error(printInEditor, formatedSting);
		}
		public static void Error(object data) {
			Error(true, data);

		}
		public static void Error(bool printInEditor, object data) {
			string s = string.Format("e: @ {0}: {1}", _logTime, data);
			LogOnScreenOrFile(s, logColor);

#if UNITY_EDITOR
			//s = string.Format("<color={0}>{1}</color>", errorColor, s);
#endif
#if MYDEBUG && UNITY_EDITOR
			if(isLogDataOnConsole && printInEditor) Debug.LogError(s);
#endif
#if MYDEBUG && !UNITY_EDITOR
			if(isLogDataOnConsole) Debug.LogError(s);
#endif

		}

		static void LogOnScreenOrFile(string data, string hexColor) {
			if(isLogDataOnScreen) _logDataOnScreen += string.Format("<color={0}>{1}</color>\n", hexColor, data);
			if(isLogDataOnFile) _logDataOnFile += string.Format("{0}\n", data);
		}
	}
}