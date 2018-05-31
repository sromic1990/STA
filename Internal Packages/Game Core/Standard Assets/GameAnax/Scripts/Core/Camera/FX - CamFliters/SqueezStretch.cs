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
	public class SqueezStretch : MonoBehaviour, ICameraFilterBase {
		bool _isEffectOn;
		Transform _squeezStretchTr;
		Vector3 _objScale, _objCurScale;
		Vector3 _objPos, _objCurPos;
		UnityEngine.Camera _camera;
		float _camHeight, _camWidth;
		float _fromValScale, _fromValPos;

		public GameObject squeezStretchObj;
		//
		[SerializeField]
		SqueezStretchType _effect;
		[SerializeField]
		float _effectTime;
		[SerializeField]
		float _timeToStretch;

		// Use this for initialization
		void Awake() {
			_camera = GetComponent<UnityEngine.Camera>();
			_squeezStretchTr = squeezStretchObj.GetComponent<Transform>();
			_objScale = _squeezStretchTr.localScale;
			_objCurScale = _squeezStretchTr.localScale;
			_objPos = _squeezStretchTr.localPosition;
			_objCurPos = _squeezStretchTr.localPosition;


			_camHeight = _camera.orthographic ? _camera.orthographicSize : _camera.fieldOfView;
			_camWidth = _camHeight * _camera.aspect;
		}

		public void StartFilter() {
			MyDebug.Log("Started Squeez-Stretch Effect: " + _effect);
			PostEffectManager.Me.isEffectOn = true;
			_isEffectOn = true;
			StartSqueezStretchEffect();
		}


		void ObjSqueezStretchScale(float value) {
			if(!_isEffectOn) {
				return;
			}

			switch(_effect) {
			case SqueezStretchType.Left:
			case SqueezStretchType.Right:
				_objCurScale.x = value;
				break;

			case SqueezStretchType.Top:
			case SqueezStretchType.Bottom:
				_objCurScale.y = value;
				break;

			}
			_squeezStretchTr.localScale = _objCurScale;
		}
		void ObjSqueezStretchPos(float value) {
			if(!_isEffectOn) {
				return;
			}
			_fromValPos = value;
			switch(_effect) {
			case SqueezStretchType.Left:
				_objCurPos.x = _objPos.x - (value * _camWidth);
				break;
			case SqueezStretchType.Right:
				_objCurPos.x = _objPos.x + (value * _camWidth);
				break;

			case SqueezStretchType.Top:
				_objCurPos.y = _objPos.y + (value * _camHeight);
				break;
			case SqueezStretchType.Bottom:
				_objCurPos.y = _objPos.y - (value * _camHeight);
				break;

			}
			_squeezStretchTr.localPosition = _objCurPos;
		}

		void GetFromValue() {

			switch(_effect) {
			case SqueezStretchType.Left:
			case SqueezStretchType.Right:
				_fromValScale = _squeezStretchTr.localScale.x;
				break;

			case SqueezStretchType.Top:
			case SqueezStretchType.Bottom:
				_fromValScale = _squeezStretchTr.localScale.y;
				break;
			}
		}

		void StartSqueezStretchEffect() {
			if(!_isEffectOn) {
				return;
			}
			iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0.5f, "time", _timeToStretch, "easetype", iTween.EaseType.linear,
				"onupdate", "ObjSqueezStretchScale"));
			iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 0.5f, "time", _timeToStretch, "easetype", iTween.EaseType.linear,
				"onupdate", "ObjSqueezStretchPos"));
			this.Invoke("StopFliter", (_timeToStretch + _effectTime + 0.1f));
		}
		public void StopFliter() {
			if(!_isEffectOn) {
				return;
			}
			MyDebug.Log("Stop Fliter Called for SqueezStretch Effect: " + _effect);
			this.CancelInvoke("StopFliter");
			GetFromValue();
			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject, iTween.Hash("from", _fromValScale, "to", 1f, "time", _timeToStretch, "easetype", iTween.EaseType.linear,
				"onupdate", "ObjSqueezStretchScale", "oncomplete", "ResetValues"));
			iTween.ValueTo(gameObject, iTween.Hash("from", _fromValPos, "to", 0f, "time", _timeToStretch, "easetype", iTween.EaseType.linear,
				"ignoretimescale", true, "onupdate", "ObjSqueezStretchPos"));
		}
		public void ForceStopFliter() {
			if(!_isEffectOn) {
				return;
			}
			MyDebug.Log("Froce Stop Fliter Called for SqueezStretch Effect: " + _effect);
			this.CancelInvoke("StopFliter");
			GetFromValue();
			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject, iTween.Hash("from", _fromValScale, "to", 1f, "time", 0.2f, "easetype", iTween.EaseType.linear,
				"ignoretimescale", true,
				"onupdate", "ObjSqueezStretchScale", "oncomplete", "ResetValues"));
			iTween.ValueTo(gameObject, iTween.Hash("from", _fromValPos, "to", 0f, "time", 0.2f, "easetype", iTween.EaseType.linear,
				"ignoretimescale", true, "onupdate", "ObjSqueezStretchPos"));

		}
		public void ResetValues() {
			if(!_isEffectOn) {
				return;
			}
			this.CancelInvoke("StopFliter");
			PostEffectManager.Me.isEffectOn = false;
			_isEffectOn = false;
			_squeezStretchTr.localScale = _objScale;
			_squeezStretchTr.localPosition = _objPos;
		}
	}
}
namespace GameAnax.Core.Enums {
	public enum SqueezStretchType {
		Left,
		Right,
		Top,
		Bottom,
	}
}