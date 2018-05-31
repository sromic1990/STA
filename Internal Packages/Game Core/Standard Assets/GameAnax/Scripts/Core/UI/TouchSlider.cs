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

using GameAnax.Core.Extension;
using GameAnax.Core.Attributes;
using GameAnax.Core.InputSystem;


namespace GameAnax.Core.UI {
	public class TouchSlider : MonoBehaviour {
		//
		int _fingerCount, fingerId = -1, sDown = -1;
		Vector2 _touchPosition, _vLimits;
		Vector3 _pos, _wsPoint, _swPoint;
		Bounds _bounds;
		//
		float _setValCutOff, _setValPosiVal, _slideCutOff, _slideBaseVal, _slideTmpVal, _lastRectDraw, _boundOffset, _valueFactor;
		//
		[EnumFlagAttribute]
		public Menus layer = 0;
		public float rectDrawDealy = 0.2f;
		//
		public bool isAllowDragOutSideArea = true;
		//
		Renderer _tThumbRend;
		[SerializeField]
		Rect _rectBuffer;
		[SerializeField]
		Camera _barCam;
		[SerializeField]
		Transform _marker;
		[SerializeField]
		Renderer _mainBar;
		//
		public float minValue;
		public float maxValue;
		[HideInInspector]
		public Rect barRect;
		[HideInInspector]
		public float value;

		void Awake() {
			if(!_barCam) {
				_barCam = GameObject.FindWithTag("GUI_Camera").GetComponent<Camera>();
			}

			_pos = _marker.position;
			_tThumbRend = _marker.GetComponent<Renderer>();
			_bounds = _mainBar.bounds;
			BoundSetting();
		}
		void Start() {
			DrawRect();
			_lastRectDraw = Time.realtimeSinceStartup;
		}
		void Update() {
			if(!layer.Equals(CoreMethods.layer)) {
				barRect = new Rect(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
				return;
			}
			if(rectDrawDealy <= 0) {
				DrawRect();
			} else if(Time.realtimeSinceStartup - _lastRectDraw > rectDrawDealy) {
				_lastRectDraw = Time.realtimeSinceStartup;
				DrawRect();
			}

			BoundSetting();

		}
		void OnGUI() {
#if UNITY_EDITOR && GUIPRINT
			GUI.Box(barRect, gameObject.name);
#endif
			_fingerCount = Input.touches.Length;
			if(_fingerCount > 0) {
				foreach(Touch tch in Input.touches) {

					if(fingerId != -1 && !fingerId.Equals(tch.fingerId)) {
						continue;
					}

					_touchPosition = tch.position;
					_touchPosition.y = Screen.height - _touchPosition.y;

					//Indentify Touch Start
					if(tch.phase == TouchPhase.Began && barRect.Contains(_touchPosition) && fingerId == -1 && !fingerId.Equals(tch.fingerId)) {
						fingerId = tch.fingerId;
						sDown = 0;
					}
					// Indentify Touch End
					else if((tch.phase == TouchPhase.Ended || tch.phase == TouchPhase.Canceled) && fingerId.Equals(tch.fingerId)) {
						fingerId = -1;
						sDown = -1;
					}
					//Indentify Touch value changes for that Particular Object
					else if(tch.phase == TouchPhase.Moved && fingerId.Equals(tch.fingerId) && sDown == 0) {
						if(isAllowDragOutSideArea) {
							Slide(_touchPosition.x);
						} else if(barRect.Contains(_touchPosition)) {
							Slide(_touchPosition.x);
						}
					}
				}
			} else {
				if(MouseInput.Me.isTouchDown && barRect.Contains(MouseInput.Me.touchDown)) {
					sDown = 0;
				} else if(sDown == 0 && MouseInput.Me.isTouchUp) {
					sDown = -1;
				} else if(MouseInput.Me.isTouchPressed && sDown == 0) {
					if(isAllowDragOutSideArea) {
						Slide(MouseInput.Me.touchPressed.x);
					} else if(barRect.Contains(MouseInput.Me.touchPressed)) {
						Slide(MouseInput.Me.touchPressed.x);
					}
				}
			}

		}

		void DrawRect() {
			barRect = _mainBar.GetBoundingRect(_barCam, _rectBuffer);
		}

		public void BoundSetting() {
			//Bound Setting
			_boundOffset = (maxValue - minValue);
			_vLimits = new Vector2(_bounds.min.x, _bounds.max.x);
			_valueFactor = (_vLimits.y - _vLimits.x) / _boundOffset;
		}
		public float Slide(float xPosSlide) {
			float retVal;
			_wsPoint = _barCam.WorldToScreenPoint(new Vector3(_marker.position.x, _pos.y, _pos.z));
			_swPoint = _barCam.ScreenToWorldPoint(new Vector3(xPosSlide, _wsPoint.y, _wsPoint.z));
			_marker.position = new Vector3(_swPoint.x, _pos.y, _pos.z);

			if(_tThumbRend.bounds.center.x < _vLimits.x) {
				_marker.position = new Vector3(_vLimits.x, _pos.y, _pos.z);
			}
			if(_tThumbRend.bounds.center.x > _vLimits.y) {
				_marker.position = new Vector3(_vLimits.y, _pos.y, _pos.z);
			}

			_slideTmpVal = (_marker.position.x - _vLimits.x);
			_slideBaseVal = (_slideTmpVal / _valueFactor);
			retVal = _slideBaseVal + minValue;
			_slideCutOff = _slideBaseVal / _boundOffset;
			_mainBar.material.SetFloat("_CutOff", _slideCutOff);
			value = retVal;
			return retVal;
		}
		public void SetValue(float baseVal) {
			value = baseVal;
			_setValCutOff = (baseVal - minValue) / _boundOffset;
			_setValPosiVal = baseVal - minValue;
			_marker.position = new Vector3((_setValPosiVal * _valueFactor) + _vLimits.x, _pos.y, _pos.z);
			_mainBar.material.SetFloat("_CutOff", _setValCutOff);
		}
	}
}