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

using GameAnax.Core.Enums;
using GameAnax.Core.Utility;


namespace GameAnax.Core.FX.Viewport {
	public class SplitCamEffect : MonoBehaviour, ICameraFilterBase {
		bool _isEffectOn;
		UnityEngine.Camera _camera;
		Rect _camRect, _camCurrRect;
		float _fromVal;

		//
		[SerializeField]
		SplitType _effect;
		[SerializeField]
		float _effectTime;
		[SerializeField]
		float _timeToSplit;

		// Use this for initialization
		void Awake() {
			_camera = GetComponent<UnityEngine.Camera>();
			_camRect = _camera.rect;
			_camCurrRect = _camera.rect;
		}

		public void StartFilter() {
			MyDebug.Log("Started Split Cam Effect: " + _effect);
			PostEffectManager.Me.isEffectOn = true;
			_isEffectOn = true;
			StartSplitEffect();
		}

		#region Rotate Filter
		void SetValCamSplitFilter(float value) {
			if(!_isEffectOn) {
				return;
			}

			switch(_effect) {
			case SplitType.Left:
				_camCurrRect.width = value;
				break;

			case SplitType.Right:
				_camCurrRect.x = 1f - value;
				_camCurrRect.width = value;
				break;

			case SplitType.Top:
				_camCurrRect.y = 1f - value;
				_camCurrRect.height = value;
				break;

			case SplitType.Bottom:
				_camCurrRect.height = value;
				break;

			case SplitType.TopLeft:
				_camCurrRect.width = value;
				_camCurrRect.y = 1f - value;
				_camCurrRect.height = value;
				break;

			case SplitType.TopRight:
				_camCurrRect.x = 1f - value;
				_camCurrRect.width = value;

				_camCurrRect.y = 1f - value;
				_camCurrRect.height = value;
				break;

			case SplitType.BottomLeft:
				_camCurrRect.width = value;
				_camCurrRect.height = value;
				break;

			case SplitType.BottomRight:
				_camCurrRect.x = 1f - value;
				_camCurrRect.width = value;

				_camCurrRect.height = value;
				break;
			}
			_camera.rect = _camCurrRect;
		}
		void SetValCamSplitFilterOff(float value) {
			if(!_isEffectOn) {
				return;
			}

			switch(_effect) {
			case SplitType.Left:
				_camCurrRect.width = value;
				break;

			case SplitType.Right:
				_camCurrRect.x = 1f - value;
				_camCurrRect.width = value;
				break;

			case SplitType.Top:
				_camCurrRect.y = 1f - value;
				_camCurrRect.height = value;
				break;

			case SplitType.Bottom:
				_camCurrRect.height = value;
				break;

			case SplitType.TopLeft:
				_camCurrRect.width = value;

				_camCurrRect.y = 1f - value;
				_camCurrRect.height = value;

				break;

			case SplitType.TopRight:
				_camCurrRect.x = 1f - value;
				_camCurrRect.width = value;

				_camCurrRect.y = 1f - value;
				_camCurrRect.height = value;
				break;

			case SplitType.BottomLeft:
				_camCurrRect.width = value;

				_camCurrRect.height = value;
				break;

			case SplitType.BottomRight:
				_camCurrRect.x = 1f - value;
				_camCurrRect.width = value;

				_camCurrRect.height = value;
				break;
			}
			_camera.rect = _camCurrRect;
		}
		void GetFromValue() {

			switch(_effect) {
			case SplitType.Left:
			case SplitType.TopLeft:
			case SplitType.BottomLeft:
				_fromVal = _camCurrRect.width;
				break;

			case SplitType.Right:
			case SplitType.BottomRight:
			case SplitType.TopRight:
				_fromVal = _camCurrRect.width;

				break;

			case SplitType.Top:
				_fromVal = _camCurrRect.height;
				break;

			case SplitType.Bottom:
				_fromVal = _camCurrRect.height;
				break;
			}
		}
		void StartSplitEffect() {
			if(!_isEffectOn) {
				return;
			}
			iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0.5f, "time", _timeToSplit, "easetype", iTween.EaseType.linear,
				"onupdate", "SetValCamSplitFilter"));
			this.Invoke("StopFliter", (_timeToSplit + _effectTime + 0.1f));
		}

		public void StopFliter() {
			if(!_isEffectOn) {
				return;
			}
			MyDebug.Log("Stop Fliter Called for Split Cam Effect: " + _effect);
			this.CancelInvoke("StopFliter");
			GetFromValue();
			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject, iTween.Hash("from", _fromVal, "to", 1f, "time", _timeToSplit, "easetype", iTween.EaseType.linear,
				"onupdate", "SetValCamSplitFilterOff", "oncomplete", "ResetValues"));
		}
		public void ForceStopFliter() {
			if(!_isEffectOn) {
				return;
			}
			MyDebug.Log("Froce Stop Fliter Called for Split Cam Effect: " + _effect);
			this.CancelInvoke("StopFliter");
			GetFromValue();
			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject, iTween.Hash("from", _fromVal, "to", 1f, "time", 0.2f, "easetype", iTween.EaseType.linear,
				"ignoretimescale", true,
				"onupdate", "SetValCamSplitFilterOff", "oncomplete", "ResetValues"));
		}
		public void ResetValues() {
			if(!_isEffectOn) {
				return;
			}
			this.CancelInvoke("StopFliter");
			PostEffectManager.Me.isEffectOn = false;
			_isEffectOn = false;
			_camera.rect = _camRect;
		}
		#endregion
	}
}
namespace GameAnax.Core.Enums {
	public enum SplitType {
		Left,
		Right,
		Top,
		Bottom,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight,
	}
}