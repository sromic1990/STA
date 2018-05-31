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

using System.Collections;
using System.Collections.Generic;

using GameAnax.Core;
using GameAnax.Core.Delegates;
using GameAnax.Core.NotificationSystem;
using GameAnax.Core.Singleton;
using GameAnax.Core.UI.Buttons;


namespace GameAnax.Core.Utility.Popup {
	[PersistentSignleton(true)]
	public class PopupManager : Singleton<PopupManager> {
		public event PopupClick PopupClicked;
		private PopupLocations _location;
		private ButtonSchemes _curPopBtnScheme;
		private bool _isShowCoinBalance;

		//
		public PopupDefaultValues defaultValue;
		public PopupTypes type;
		//
		public GameObject centerPopupGrpGo;
		public List<GameObject> centerPopupObjs;
		//
		public SpriteRenderer centerPopupBG;
		public GameObject centerCoinBalance;
		public TextMesh centerTitleTM;
		public exSpriteFont centerTitleE2D;
		public TextMesh centerDiscriptionTM;
		public exSpriteFont centerDiscriptionE2D;

		public GameObject centerYesBtnGrp;
		public GameObject centerNoBtnGrp;
		public GameObject centerOkBtnGrp;
		public TextMesh centerYesBtnTM;
		public TextMesh centerNoBtnTM;
		public TextMesh centerOkBtnTM;

		[Space(10)]
		public GameObject upDownGrpGo;
		public Transform upDownOffset;
		public float offsetValue;
		public Transform upDownMainGroup;
		public float upDownValue;

		public List<GameObject> upDownObjs;
		//
		public SpriteRenderer upDownBG;
		public GameObject upDownCoinBalance;
		public TextMesh upDownTitleTM;
		public exSpriteFont upDownTitleE2D;
		public TextMesh upDownDiscriptionTM;
		public exSpriteFont upDownDiscriptionE2D;
		public GameObject upDownYesBtnGrp;
		public GameObject upDownNoBtnGrp;
		public GameObject upDownOkBtnGrp;
		public TextMesh upDownYesBtnTM;
		public TextMesh upDownNoBtnTM;
		public TextMesh upDownOkBtnTM;


		// Use this for initialization
		private void Awake() {
			Me = this;
			NotificationCenter.Me.AddObserver(this, "PopupClickGot");
		}

		private void OnPopupClicked(string button) {
			if(null != PopupClicked) {
				PopupClicked(button);
			}
		}
		private void PopupClickGot(ButtonEventArgs args) {
			MyDebug.Log("Popup Click Recevied");
			HidePopup(false);
			OnPopupClicked(args.data);
		}

		public void HidePopup() {
			HidePopup(false);
		}
		public void HidePopup(bool isFromBackButton) {
			CancelInvoke("HidePopup");
			MyDebug.Log("Hiding popp " + _location);
			if(_location.Equals(PopupLocations.Top) || _location.Equals(PopupLocations.Bottom)) {
				HideUDPopup();
			} else if(_location.Equals(PopupLocations.Center)) {
				HideCenterPopup();
			}

			if(isFromBackButton && !_curPopBtnScheme.Equals(ButtonSchemes.None)) {
				OnPopupClicked("no");
			}
		}

		#region ShowPopup Overlods
		public void ShowPopup(string message) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, float autoHide) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, float autoHide, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string message, float autoHide, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string message, PopupTypes pType) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, float autoHide) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, float autoHide, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, float autoHide, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string message, PopupTypes pType, PopupLocations location) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, PopupLocations location, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, PopupLocations location, float autoHide) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, PopupLocations location, float autoHide, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, PopupLocations location, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, PopupLocations location, float autoHide, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupTypes pType, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, pType, location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string message, PopupLocations location) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupLocations location, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupLocations location, float autoHide) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupLocations location, float autoHide, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupLocations location, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string message, PopupLocations location, float autoHide, double charSize) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string message, ButtonSchemes btnScheme) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, float autoHide) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, float autoHide, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, double charSize) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, float autoHide, double charSize) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, double charSize) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide, double charSize) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location, double charSize) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, -1, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, -1, charSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide, double charSize) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, defaultValue.popupType, location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location) {
			ShowPopup("", message, btnScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide) {
			ShowPopup("", message, btnScheme, pType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, double charSize) {
			ShowPopup("", message, btnScheme, pType, location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide, double charSize) {
			ShowPopup("", message, btnScheme, pType, location, autoHide, charSize, defaultValue.isShowCoins);
		}

		//
		//
		//
		public void ShowPopup(string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup("", message, btnScheme, pType, location, autoHide, charSize, isShowCoins);
		}
		//
		//
		//
		public void ShowPopup(string title, string message) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, float autoHide) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, float autoHide, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, float autoHide, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string title, string message, PopupTypes pType) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, float autoHide) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, float autoHide, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, float autoHide, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location, float autoHide) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location, float autoHide, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location, float autoHide, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupTypes pType, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, pType, location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string title, string message, PopupLocations location) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupLocations location, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupLocations location, float autoHide) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupLocations location, float autoHide, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupLocations location, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupLocations location, float autoHide, double charSize) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup(title, message, defaultValue.buttonScheme, defaultValue.popupType, location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string title, string message, ButtonSchemes btnScheme) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, float autoHide) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, float autoHide, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, double charSize) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, double charSize, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, float autoHide, double charSize) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, double charSize) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, double charSize, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide, double charSize) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, pType, defaultValue.location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location, double charSize) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, -1, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, -1, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide, double charSize) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, autoHide, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, defaultValue.popupType, location, autoHide, charSize, isShowCoins);
		}

		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location) {
			ShowPopup(title, message, btnScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, pType, location, defaultValue.autoHideAfter, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide) {
			ShowPopup(title, message, btnScheme, pType, location, autoHide, defaultValue.tmCharSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide,
			bool isShowCoins) {
			ShowPopup(title, message, btnScheme, pType, location, autoHide, defaultValue.tmCharSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, double charSize) {
			ShowPopup(title, message, btnScheme, pType, location, defaultValue.autoHideAfter, charSize, defaultValue.isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, double charSize, bool isShowCoins) {
			ShowPopup(title, message, btnScheme, pType, location, defaultValue.autoHideAfter, charSize, isShowCoins);
		}
		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide, double charSize) {
			ShowPopup(title, message, btnScheme, pType, location, autoHide, charSize, defaultValue.isShowCoins);
		}

		#endregion

		public void ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHide, double charSize, bool isShowCoins) {
			StartCoroutine(ShowPopup(title, message, btnScheme, pType, location, autoHide, charSize, isShowCoins,
				defaultValue.popupBGColor, defaultValue.descriptionTextColor, false));
		}
		/// <summary>
		/// Shows the popup over the all screen. in most case Behind the popup item is not accessible coz we change Screen Flag to Popup and Layer
		/// but may at some place Raycast base item will accessible
		/// </summary>
		/// <param name="title">Title of the Popup</param>
		/// <param name="message">Message to diplay in popup.</param>
		/// <param name="btnScheme">Button scheme like YesNo, Ok or Close</param>
		/// <param name="pType">PopupType used for special action like Power Use, Tutorial or normal one</param>
		/// <param name="location">Select locaiotn where you want to show Popup, On Top Edge of the screen, Bottom Edge of the screen or Center of the screen</param>
		/// <param name="autoHideTime">Put value greaterthan 0 if you want to close popup after a time if user not intract. its only work if Popup type is None</param>
		/// <param name="charSize">Character size of discription of the popup</param>
		/// <param name="isShowCoins">(true) wil show current Coin Balance durng popupscreen</param>
		/// <param name="bgColor">This color value set to background box of the Popup</param>
		/// <param name="detailTextColor">this color will apply to discription text if the popup</param>
		/// 
		/// <param name="i">If set to <c>true</c> i.</param>
		private IEnumerator ShowPopup(string title, string message, ButtonSchemes btnScheme, PopupTypes pType, PopupLocations location, float autoHideTime, double charSize, bool isShowCoins,
			Color bgColor, Color detailTextColor, bool i) {
			if(location.Equals(PopupLocations.None)) {
				throw new System.Exception("Popup location \"None\" is not allowed");
			}
			//MyDebug.Log("Show popup before Hide");
			CancelInvoke("HidePopup");
			StopCoroutine("HidePopup");

			if(!location.Equals(location) && !location.Equals(PopupLocations.None)) {
				if(location.Equals(PopupLocations.Top) || location.Equals(PopupLocations.Bottom)) {
					HideUDPopup();
				} else if(location.Equals(PopupLocations.Center)) {
					HideCenterPopup();
				}
				yield return StartCoroutine(CoreMethods.Wait(0.4f));
			}

			//MyDebug.Log("Show popup after Hide");
			//MyDebug.Log("AM: " + GUtility.Me.CoreUtility.Me.activeMenu + " GCL: " + CoreMethods.layer + " GS: " + CoreMethods.gameStatus + ", LgS: " + lastState + ", ll: " + lastLayer);
			if(!CoreUtility.Me.activeMenu.Equals(Menus.Popup)) {
				CoreUtility.Me.SetLastStack(CoreUtility.Me.activeMenu);
			}
			//MyDebug.Log("GCL: " + CoreMethods.layer + ", LL: " + lastLayer);
			if(!CoreMethods.layer.Equals(Menus.Wait) && !CoreMethods.layer.Equals(Menus.Popup)) {
				//MyDebug.Log("Last Layer set to " + CoreMethods.layer);
				lastLayer = CoreMethods.layer;
			}
			CoreMethods.layer = Menus.Wait;
			//isPopupOnScreen = true;

			_curPopBtnScheme = btnScheme;
			centerCoinBalance.SetActive(false);
			centerYesBtnGrp.SetActive(false);
			centerNoBtnGrp.SetActive(false);
			centerOkBtnGrp.SetActive(false);

			upDownCoinBalance.SetActive(false);
			upDownYesBtnGrp.SetActive(false);
			upDownNoBtnGrp.SetActive(false);
			upDownOkBtnGrp.SetActive(false);

			centerPopupBG.color = bgColor;
			upDownBG.color = bgColor;

			//centerDiscriptionTM.color = detailTextColor;
			//upDownDiscriptionTM.color = detailTextColor;

			switch(_curPopBtnScheme) {
			case ButtonSchemes.YesNo:
				centerYesBtnGrp.SetActive(true);
				centerNoBtnGrp.SetActive(true);
				//
				upDownYesBtnGrp.SetActive(true);
				upDownNoBtnGrp.SetActive(true);

				centerYesBtnTM.text = "Yes";
				upDownYesBtnTM.text = "Yes";

				centerNoBtnTM.text = "No";
				upDownNoBtnTM.text = "No";

				break;

			case ButtonSchemes.OK:
				centerOkBtnGrp.SetActive(true);
				upDownOkBtnGrp.SetActive(true);

				centerOkBtnTM.text = "OK!";
				upDownOkBtnTM.text = "OK!";
				break;

			case ButtonSchemes.Close:
				centerOkBtnGrp.SetActive(true);
				upDownOkBtnGrp.SetActive(true);

				centerOkBtnTM.text = "Close";
				upDownOkBtnTM.text = "Close";
				break;
			}
			title = title.Replace("\\n", "\n");
			message = message.Replace("\\n", "\n");
			if(charSize <= 0) {
				charSize = defaultValue.tmCharSize;
			}
			centerTitleTM.text = upDownTitleTM.text = centerTitleE2D.text = upDownTitleE2D.text = title;
			centerDiscriptionTM.text = upDownDiscriptionTM.text = centerDiscriptionE2D.text = upDownDiscriptionE2D.text = message;

			centerDiscriptionTM.characterSize = upDownDiscriptionTM.characterSize = (float)charSize;
			centerDiscriptionE2D.scale = upDownDiscriptionE2D.scale = new Vector2((float)charSize, (float)charSize);
			//CPDiscriptionTM.color = UDDiscriptionTM.color = detailTextColor;
			//
			//
			//CPBG.color = UDBG.color = bgColor;
			this._isShowCoinBalance = isShowCoins;
			type = pType;
			_location = location;

			if(location.Equals(PopupLocations.Center)) {
				yield return StartCoroutine(ShowCenterPopup());
			} else if(location.Equals(PopupLocations.Top) || location.Equals(PopupLocations.Bottom)) {
				yield return StartCoroutine(ShowUDPopup(location));
			}
			if(autoHideTime > 0f && _curPopBtnScheme.Equals(ButtonSchemes.None)) {
				Invoke("HidePopup", autoHideTime);
			}
			//MyDebug.Log("AM: " + MenuManager.Me.CoreUtility.Me.activeMenu + " GCL: " + CoreMethods.layer + " GS: " + CoreMethods.gameStatus + ", LgS: " + lastState + ", ll: " + lastLayer);
		}

		private float menuWait;
		private Menus lastLayer;
		//private GamePlayState lastState;
		//private Menus lastMenu;
		private IEnumerator ShowCenterPopup() {
			centerCoinBalance.SetActive(_isShowCoinBalance);
			centerPopupGrpGo.SetActive(true);
			menuWait = 0f;
			yield return StartCoroutine(CoreMethods.Wait(menuWait));

			AfterPopupShow();
		}
		private void HideCenterPopup() {
			MyDebug.Log("Hiding Top popup");
			CoreMethods.layer = Menus.Wait;
			AfterPopupHide();
		}

		private IEnumerator ShowUDPopup(PopupLocations location) {
			Vector3 pos;
			//AdsMCG.Me.HideBannerAds();
			upDownCoinBalance.SetActive(_isShowCoinBalance);
			if(location.Equals(PopupLocations.Top)) {
				pos = upDownOffset.localPosition;
				pos.y = offsetValue;
				upDownOffset.localPosition = pos;

				pos = upDownMainGroup.localPosition;
				pos.y = upDownValue;
				upDownMainGroup.localPosition = pos;
			} else if(location.Equals(PopupLocations.Bottom)) {
				pos = upDownOffset.localPosition;
				pos.y = -offsetValue;
				upDownOffset.localPosition = pos;

				pos = upDownMainGroup.localPosition;
				pos.y = -upDownValue;
				upDownMainGroup.localPosition = pos;
			}
			upDownGrpGo.SetActive(true);
			menuWait = 0f;
			//menuWait = 0.5f + (UDPopupObjs.Count * 0.05f);
			//StartCoroutine(CoreUtility.Me.PunchInObjects(UDPopupObjs, false));
			yield return StartCoroutine(CoreMethods.Wait(menuWait));
			//
			AfterPopupShow();
		}
		private void HideUDPopup() {
			MyDebug.Log("Hiding Top popup");
			CoreMethods.layer = Menus.Wait;
			//StartCoroutine(CoreUtility.Me.PunchOutObjects(UDPopupObjs));
			//yield return StartCoroutine(CoreMethods.Wait(0));
			//yield return StartCoroutine(CoreMethods.Wait(0.2f));
			AfterPopupHide();
		}
		private void AfterPopupShow() {
			CoreUtility.Me.activeMenu = Menus.Popup;
			CoreUtility.Me.OnScreenChange("Popup", "Open");
			CoreMethods.layer = Menus.Popup;
			//CoreMethods.gameStatus = GamePlayState.Pause;
		}
		private void AfterPopupHide() {
			CoreUtility.Me.OnScreenChange("Popup", "Hide");
			upDownGrpGo.SetActive(false);
			centerPopupGrpGo.SetActive(false);

			centerYesBtnGrp.SetActive(false);
			centerNoBtnGrp.SetActive(false);
			centerOkBtnGrp.SetActive(false);

			//MyDebug.Log("AM: " + MenuManager.Me.CoreUtility.Me.activeMenu + " GCL: " + CoreMethods.layer + " GS: " + CoreMethods.gameStatus + ", LgS: " + lastState + ", ll: " + lastLayer);
			if(!lastLayer.Equals(0) && CoreUtility.Me.activeMenu.Equals(Menus.Popup)) {
				CoreMethods.layer = lastLayer;
				//CoreMethods.gameStatus = lastState;
				if(CoreUtility.Me.lastMenuStack.Count > 0) {
					CoreUtility.Me.activeMenu = CoreUtility.Me.lastMenuStack[0];
					CoreUtility.Me.lastMenuStack.RemoveAt(0);
				}
			}
			//MyDebug.Log("Last Layer Become empty");
			lastLayer = 0;
			//MyDebug.Log("AM: " + MenuManager.Me.CoreUtility.Me.activeMenu + " GCL: " + CoreMethods.layer + " GS: " + CoreMethods.gameStatus + ", LgS: " + lastState + ", ll: " + lastLayer);
			_location = PopupLocations.None;
			_curPopBtnScheme = ButtonSchemes.None;
			//isPopupOnScreen = false;
		}
	}

	[System.Serializable]
	public class PopupDefaultValues {
		public ButtonSchemes buttonScheme;
		public PopupTypes popupType;
		public PopupLocations location;
		public float autoHideAfter;
		public double tmCharSize;
		public bool isShowCoins;
		public Color popupBGColor;
		public Color descriptionTextColor;
	}

	public enum ButtonSchemes {
		None,
		YesNo,
		OK,
		Close
	}

	public enum PopupTypes {
		Normal,
		TutorialStart,
		Tutorial,
		TutorialComplete,
		//
		RemoveTilePower,
		SwitchTilePower,
		UndoPower,
		HammerPower,
		ReshufflePower
	}
	public enum PopupLocations {
		None,
		Center,
		Top,
		Bottom
	}
}