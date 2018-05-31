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

using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core.Enums;
using GameAnax.Core.Singleton;
using GameAnax.Core.FX.Viewport;
using GameAnax.Core.Utility;


namespace GameAnax.Core.FX {
	[PersistentSignleton(true)]
	public class PostEffectManager : Singleton<PostEffectManager> {
		List<int> _effectToRemove = new List<int>();
		//
		[HideInInspector]
		public bool isEffectOn = false;
		public List<ICameraFilterBase> postEffects;

		// Use this for initialization
		void Awake() {
			Me = this;

			//Populate list Of Effcts
			postEffects = new List<ICameraFilterBase>();
			postEffects.AddRange(GetComponents<ICameraFilterBase>());

			// Idenify this script and remove from Effect List
			for(int i = 0; i < postEffects.Count; i++) {
				if(ReferenceEquals(postEffects[i], this)) {
					_effectToRemove.Add(i);
				}
			}
			for(int i = _effectToRemove.Count - 1; i == 0; i--) {
				postEffects.RemoveAt(_effectToRemove[i]);
			}

			CoreMethods.FillUniqueIndex(postEffects.Count, ref mainBars);
		}
#if GUIPRINT
		int guiX, guiY, guiwidth, guiheight, padding, hbuttons, vbuttons;
		void OnGUI() {
			guiX = guiY = padding = 8;
			hbuttons = 4;
			vbuttons = 15;
			guiwidth = (Screen.width - ((hbuttons + 2) * padding)) / hbuttons;
			guiheight = (Screen.height - ((vbuttons + 2) * padding)) / vbuttons;
			guiX = (Screen.width - guiwidth) / 2;
			if(CoreMethods.gameStatus.Equals(GamePlayState.Gameplay)) {
				if(!isEffectOn) {
					if(GUI.Button(new Rect(guiX, guiY, guiwidth, guiheight), "Effect")) {
						StartEffect();
					}
				} else {
					if(GUI.Button(new Rect(guiX, guiY, guiwidth, guiheight), "Fast Stop")) {
						FastStopEffect();
					}
				}

			}
		}
#endif

		int lastBar = 0;
		public void StartEffect() {
			CoreMethods.GetUniqueRandomIndex(ref tempBars, ref mainBars, ref lastBar);
			StartEffect(lastBar);
		}
		public void StartEffect(int effectID) {
			StartEffect(effectID, -1f);
		}
		int curEffID = -1;
		public void StartEffect(int effectID, float effTime) {
			if(isEffectOn) {
				return;
			}
			curEffID = effectID;
			MyDebug.Warning("Effect Start: " + curEffID);
			switch(curEffID) {
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				postEffects[curEffID].StartFilter();
				break;

			default:
				break;
			}
			if(effTime >= 0) {
				Invoke("StopEffect", effTime);
			}
		}

		public void StopEffect() {
			MyDebug.Warning("Stop Effect called");
			CancelInvoke("StopEffect");
			switch(curEffID) {
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				postEffects[curEffID].StopFliter();
				break;

			default:
				break;
			}
		}
		public void FastStopEffect() {
			MyDebug.Warning("FastStop Effect called");
			CancelInvoke("StopEffect");
			switch(curEffID) {
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				postEffects[curEffID].ForceStopFliter();
				break;

			default:
				break;

			}
		}

		#region "Unique Rando till list once not complete"
		List<int> mainBars = new List<int>();
		List<int> tempBars = new List<int>();
		//int GetRandom() {
		//	return GetRandom(true);
		//}
		//int GetRandom(bool isUniqueBar) {
		//	if(tempBars.Count <= 0) {
		//		tempBars.Clear();
		//		tempBars.AddRange(mainBars.ToArray());
		//	}
		//	int x = Random.Range(0, tempBars.Count);
		//	int f = tempBars[x];
		//	if(isUniqueBar) {
		//		tempBars.RemoveAt(x);
		//	}
		//	return f;
		//}
		//void FillValues() {
		//	mainBars.Clear();
		//	for(int i = 0; i < postEffects.Count; i++) {
		//		mainBars.Add(i);
		//	}
		//}
		#endregion
	}
}