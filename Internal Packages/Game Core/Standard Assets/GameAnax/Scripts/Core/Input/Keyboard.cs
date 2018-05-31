//  
// Coder:			Sharatbabu Aachary, Ranpariya Ankur {GameAnax} 
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

using GameAnax.Core.Delegates;
using GameAnax.Core.Extension;
//using GameAnax.Core.Utility.Popup;


namespace GameAnax.Core.InputSystem {
	public class Keyboard : MonoBehaviour {
		public event SimpleDelegate KeyboardOpen;
		public event SimpleDelegate KeyboardClose;

		void OnKeyBoardOpen() {
			if(textPrinter.text.Equals(preText)) textPrinter.text = "";
			if(null != KeyboardOpen) {
				KeyboardOpen();
			}
		}
		void OnKeyBoardClose() {
			if(string.IsNullOrEmpty(textPrinter.text)) textPrinter.text = preText;
			if(null != KeyboardClose) {
				KeyboardClose();
			}
		}

		public string text {
			set {
				textPrinter.text = string.IsNullOrEmpty(textPrinter.text) ? preText : value;
			}
			get {
				return string.IsNullOrEmpty(textPrinter.text) ||
							 textPrinter.text.Equals(preText) ? "" : textPrinter.text;
			}
		}
		public TextMesh textPrinter;
		public int maxCharacterToShow = 10;
		[SerializeField]
		string preText = "Type Here";
		[SerializeField]
		TouchScreenKeyboardType type = TouchScreenKeyboardType.Default;
		[SerializeField]
		bool isAutoCorrection;
		[SerializeField]
		bool isInputShowWithKB;
		[SerializeField]
		bool isMultiline;
		[SerializeField]
		bool isSecure;
		[SerializeField]
		char secureChracter = '*';

		TouchScreenKeyboard _kbInfo;
		bool _isCheckMe = false;
		string _oldValue = string.Empty;
		void Awake() {
			if(null == textPrinter) {
				textPrinter = GetComponent<TextMesh>();
			}
			if(!isSecure && string.IsNullOrEmpty(secureChracter.ToString())) {
				throw new Exception("Text input is makred as Secure Field but there are no secure character used to mask user entry");
			}
		}
		void Update() {
			if(!_isCheckMe) {
				return;
			}
			if(null == _kbInfo) {
				_isCheckMe = false;
				return;
			}

			if(null == textPrinter) {
				throw new Exception("Text mesh not assinged for this TextHandler");
			}

			//MyDebug.Log("KB Update => KB Active: " + kbInfo.active + " KB Done: " + kbInfo.done + " KB wasCan: " + kbInfo.wasCanceled);
			if(_kbInfo.active) {
				text = _kbInfo.text;
				UpdateText(false);
			}
			//#if UNITY_IOS
			else if(_kbInfo.done) {
				UpdateText(true);
				CloseKeyBoard();
			}
			//#endif
			else if(_kbInfo.wasCanceled) {
				text = _oldValue;
				_oldValue = string.Empty;
				UpdateText(true);
				CloseKeyBoard();
			}
		}


		public void OpenKeyBoard() {
#if(!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
			//PopupMessages.Me.NativeKeyboardMessage();
			return;
#endif
			_oldValue = text;
			if(null == _kbInfo) {
				//MyDebug.Log("No Old Keayboard. Opening new one");
				_kbInfo = TouchScreenKeyboard.Open(text, type, isAutoCorrection, isMultiline, isSecure, isInputShowWithKB, preText);
			}
			//kbInfo.active = true;
			_isCheckMe = true;
			//MyDebug.Log("KB Open => KB Active: " + kbInfo.active + " KB Done: " + kbInfo.done + " KB wasCan: " + kbInfo.wasCanceled);
			OnKeyBoardOpen();
		}
		public void CloseKeyBoard() {
			//MyDebug.Log(name + " Closing");
			if(null != _kbInfo) {
				//MyDebug.Log("KB Close => KB Active: " + kbInfo.active + " KB Done: " + kbInfo.done + " KB wasCan: " + kbInfo.wasCanceled);
				_kbInfo = null;
			}
			_isCheckMe = false;
			OnKeyBoardClose();
		}

		public void SetValue(string valueToSet) {
			text = valueToSet;
			UpdateText(true);
		}
		void UpdateText(bool isLeft) {
			if(isLeft) {
				textPrinter.text = !isSecure ? text.Left(maxCharacterToShow) : GetSecureText(text);
			} else {
				textPrinter.text = !isSecure ? text.Right(maxCharacterToShow) : GetSecureText(text);
			}
		}
		string GetSecureText(string text) {
			return (text.Length > maxCharacterToShow ? "".PadLeft(maxCharacterToShow, secureChracter) : "".PadLeft(text.Length, secureChracter));
		}
	}
}