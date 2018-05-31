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
using GameAnax.Core.Extension;
using GameAnax.Core.Attributes;
using GameAnax.Core.Singleton;
using GameAnax.Core.UI.Buttons;
using GameAnax.Core.Utility;


namespace GameAnax.Core.InputSystem {
	[PersistentSignleton(true, true)]
	public class ControlSystem : SingletonAuto<ControlSystem> {

		//public event Delegates.Direction TouchSlides;
		//public event Delegates.Tap DoubleTap;

		public event Delegates.Direction CharacterMove;
		public event Delegates.Slide Slide;

		public event Delegates.Tap SingleTap;
		public event Delegates.SimpleDelegate Fire;
		public event Delegates.SimpleDelegate Jump;
		public event Delegates.Tap TouchDown;
		public event Delegates.Tap TouchPressed;
		public event Delegates.Tap TouchUp;


		//private void OnTouchSlides(Vector2 direction) { if(null != TouchSlides) { TouchSlides.Invoke(direction); } }
		//private void OnDoubleTap(int touchId, Direction side, Vector2 point) { if(null != DoubleTap) { DoubleTap.Invoke(touchId, side, point); } }
		private void OnCharacterMove(Vector2 direction) {
			if(null != CharacterMove) { CharacterMove.Invoke(direction); }
		}
		private void OnSingleTap(int touchId, Direction side, Vector2 point) { if(null != SingleTap) { SingleTap.Invoke(touchId, side, point); } }
		private void OnFire() { if(null != Fire) { Fire.Invoke(); } }
		private void OnJump() { if(null != Jump) { Jump.Invoke(); } }
		private void OnTouchDown(int touchId, Direction side, Vector2 point) { if(null != TouchDown) { TouchDown.Invoke(touchId, side, point); } }
		private void OnTouchPressed(int touchId, Direction side, Vector2 point) { if(null != TouchPressed) { TouchPressed.Invoke(touchId, side, point); } }
		private void OnTouchUp(int touchId, Direction side, Vector2 point) { if(null != TouchUp) { TouchUp.Invoke(touchId, side, point); } }
		private void OnSlide(Vector2 direction, Vector2 distance) { if(null != Slide) { Slide.Invoke(direction, distance); } }

		// Use this for initialization
		void Start() {
			_centerPoint.x = Screen.width / 2f;
			_centerPoint.y = Screen.height / 2f;
		}

		// Update is called once per frame
		void Update() {
			if(touchContolHandleBy.Equals(TouchControlHandle.Update)) TouchConrol();
		}
		void FixedUpdate() {
			if(touchContolHandleBy.Equals(TouchControlHandle.FixedUpdate)) TouchConrol();
		}
		void OnGUI() {
			if(touchContolHandleBy.Equals(TouchControlHandle.OnGUI)) TouchConrol();
		}

		#region Touch Control System
		private bool _isIgnoreClick = false;
		private int _touchCount = 0, _curFingerId = -1;
		private float _axisX, _axisY;
		private Vector2 _touchPoint, _centerPoint;//, _lastTouchWorld = Vector2.zero, _touchWorld, _touchDelta = Vector2.zero;
		private void TouchConrol() {
			//FIXME: Axis based Control (Used by Keyboard / Joy Stick / Game Pad
			_axisY = Input.GetAxis("Vertical");
			_axisX = Input.GetAxis("Horizontal");
			OnCharacterMove(new Vector2(_axisX, _axisY));
			if(Input.GetAxis("Jump") > 0f) { OnJump(); }
			if(Input.GetAxis("Fire1") > 0f) { OnFire(); }

			_touchCount = Input.touchCount;
			if(_touchCount > 0) {
				//FIXME: Touch Based Control work on Touch screen
				foreach(Touch tch in Input.touches) {
					_touchPoint = tch.position;
					_touchPoint.y = Screen.height - _touchPoint.y;
					_isIgnoreClick = false;
					if(!_touchZone.Contains(_touchPoint)) { continue; }
					//for(int i = 0; i < excludeButtons.Length; i++) {
					//	if(excludeButtons[i].touchZone.Contains(_touchPoint)) {
					//		_isIgnoreClick = true;
					//		MyDebug.Log("Touch Ignoring by exclude buttons");
					//	}
					//}

					if(_isIgnoreClick) { continue; }
					if(tch.phase.Equals(TouchPhase.Began)) {
						//Touch Start
						DetactTap(_touchPoint, tch.fingerId);
						OnTouchDown(tch.fingerId, (Direction)GetTouchSide(_touchPoint), _touchPoint);
					} else
					//Touch Pressed or hold
					if((tch.phase.Equals(TouchPhase.Stationary) || tch.phase.Equals(TouchPhase.Moved))) {
						OnTouchPressed(tch.fingerId, (Direction)GetTouchSide(_touchPoint), _touchPoint);
						TouchSlideontrol(tch.deltaPosition);
					} else
					//Touch End
					if((tch.phase.Equals(TouchPhase.Ended) || tch.phase.Equals(TouchPhase.Canceled))) {
						OnTouchUp(tch.fingerId, (Direction)GetTouchSide(_touchPoint), _touchPoint);
					}
				}
			} else {
				//FIXME: Mouse based Control works on PC or non touch based control or touch pad / Game Pad
				_touchPoint = MouseInput.Me.mousePosition;
				if(_touchZone.Contains(_touchPoint)) {
					_isIgnoreClick = false;
					//for(int i = 0; i < excludeButtons.Length; i++) {
					//	if(excludeButtons[i].touchZone.Contains(_touchPoint)) {
					//		_isIgnoreClick = true;
					//	}
					//}
					if(!_isIgnoreClick) {
						if(MouseInput.Me.isTouchDown) {
							OnTouchDown(0, (Direction)GetTouchSide(_touchPoint), _touchPoint);
							DetactTap(_touchPoint, 0);
						} else if(MouseInput.Me.isTouchPressed) {
							OnTouchPressed(0, (Direction)GetTouchSide(_touchPoint), _touchPoint);
							TouchSlideontrol(MouseInput.Me.mouseDeltaPosition);
						} else if(MouseInput.Me.isTouchUp) {
							OnTouchUp(0, (Direction)GetTouchSide(_touchPoint), _touchPoint);
						}
					}
				}
			}



			if(rectDrawDealy <= 0) {
				DrawRect();
			} else if(Time.realtimeSinceStartup - _lastRectDraw > rectDrawDealy) {
				_lastRectDraw = Time.realtimeSinceStartup;
				DrawRect();
			}
		}
		//int _side;
		private int GetTouchSide(Vector2 touchPoint) {
			int side = 0;
			if(touchPoint.x < _centerPoint.x) {
				side = side.SetFlag((int)Direction.Left);
			} else if(touchPoint.x > _centerPoint.x) {
				side = side.SetFlag((int)Direction.Right);
			}
			if(touchPoint.y < _centerPoint.y) {
				side = side.SetFlag((int)Direction.Down);
			} else if(touchPoint.y > _centerPoint.y) {
				side = side.SetFlag((int)Direction.Up);
			}
			return side;
		}
		private void DetactTap(Vector2 touchPoint, int touchId) {
			int _side = GetTouchSide(touchPoint);
			OnSingleTap(touchId, (Direction)_side, touchPoint);
		}
		private void TouchSlideontrol(Vector2 touchDelta) {
			touchDelta.y = Screen.height - touchDelta.y;
			touchDelta = Camera.main.ScreenToWorldPoint(touchDelta);
			OnSlide(touchDelta.normalized, touchDelta);
			OnCharacterMove(touchDelta.normalized);
		}

		#endregion

		#region Touch Zone for tap Setting
		[Header("Touch area related items")]
		[EnumFlagAttribute]
		public Menus layer;
		Rect _touchZone;
		Buffer _zoneBorder = Buffer.Zero;

		float _lastRectDraw, _xMin = 0, _xMax = 0, _yMin = 0, _yMax = 0;

		public float rectDrawDealy = 0.2f;
		//public Button[] excludeButtons;
		public TouchControlHandle touchContolHandleBy;
		public ScreenTouchZoneType zoneType = ScreenTouchZoneType.FullScreen;
		public Buffer zoneBorderWBanner = Buffer.Zero;
		public Buffer zoneBorderNoBanner = Buffer.Zero;

		public TouchZoneBorderBase borderBase = TouchZoneBorderBase.None;
		void DrawRect() {
			_touchZone = new Rect();
			switch(zoneType) {
			case ScreenTouchZoneType.FullScreen:
				_xMin = 0;
				_xMax = Screen.width;
				_yMin = 0;
				_yMax = Screen.height;

				break;
			case ScreenTouchZoneType.HalfBottom:
				_xMin = 0;
				_xMax = Screen.width;
				_yMin = Screen.height / 2;
				_yMax = Screen.height;
				break;

			case ScreenTouchZoneType.HalfTop:
				_xMin = 0;
				_xMax = Screen.width;
				_yMin = 0;
				_yMax = Screen.height / 2;
				break;

			case ScreenTouchZoneType.HalfLeft:
				_xMin = 0;
				_xMax = Screen.width / 2;
				_yMin = 0;
				_yMax = Screen.height;
				break;

			case ScreenTouchZoneType.HalfRight:
				_xMin = Screen.width / 2;
				_xMax = Screen.width;
				_yMin = 0;
				_yMax = Screen.height;
				break;

			case ScreenTouchZoneType.BottomLeftCorner:
				_xMin = 0;
				_xMax = Screen.width / 2;
				_yMin = Screen.height / 2;
				_yMax = Screen.height;
				break;

			case ScreenTouchZoneType.BottomRightCorner:
				_xMin = Screen.width / 2;
				_xMax = Screen.width;
				_yMin = Screen.height / 2;
				_yMax = Screen.height;
				break;
			case ScreenTouchZoneType.TopLeftCorner:
				_xMin = 0;
				_xMax = Screen.width / 2;
				_yMin = 0;
				_yMax = Screen.height / 2;
				break;

			case ScreenTouchZoneType.TopRightCorner:
				_xMin = Screen.width / 2;
				_xMax = Screen.width;
				_yMin = 0;
				_yMax = Screen.height / 2;
				break;
			}

			_touchZone.xMin = _xMin;
			_touchZone.xMax = _xMax;
			_touchZone.yMin = _yMin;
			_touchZone.yMax = _yMax;
			_zoneBorder = zoneBorderNoBanner;
			//zoneBorder = AdsMCG.Me.IsBannerAdOnScreen ? zoneBorderNBanner : zoneBorderNoBanner;

			if(borderBase == TouchZoneBorderBase.RectSize) {
				_touchZone.xMin += (_touchZone.width * (_zoneBorder.Left / 100f));
				_touchZone.xMax -= (_touchZone.width * (_zoneBorder.Right / 100f));
				_touchZone.yMin += (_touchZone.height * (_zoneBorder.Top / 100f));
				_touchZone.yMax -= (_touchZone.height * (_zoneBorder.Bottom / 100f));
			} else if(borderBase == TouchZoneBorderBase.ScreenSize) {
				_touchZone.xMin += (Screen.width * (_zoneBorder.Left / 100f));
				_touchZone.xMax -= (Screen.width * (_zoneBorder.Right / 100f));
				_touchZone.yMin += (Screen.height * (_zoneBorder.Top / 100f));
				_touchZone.yMax -= (Screen.height * (_zoneBorder.Bottom / 100f));
			}
		}
		#endregion
	}
	[System.Serializable]
	public class Buffer {
		public float Left = 0;
		public float Right = 0;
		public float Top = 0;
		public float Bottom = 0;

		public Buffer() {
			this.Left = 0;
			this.Right = 0;
			this.Top = 0;
			this.Bottom = 0;
		}

		public Buffer(float left, float top, float right, float bottom) {
			this.Left = left;
			this.Right = right;
			this.Top = top;
			this.Bottom = bottom;
		}

		public static Buffer Zero {
			get { return new Buffer(0, 0, 0, 0); }
		}
	}

	public enum ScreenTouchZoneType {
		HalfLeft = 0,
		HalfRight = 1,
		HalfTop = 2,
		HalfBottom = 3,
		TopLeftCorner = 4,
		TopRightCorner = 5,
		BottomLeftCorner = 6,
		BottomRightCorner = 7,
		FullScreen = 8
	}
	public enum TouchZoneBorderBase {
		None = 0,
		ScreenSize = 1,
		RectSize = 2
	}
	public enum TouchControlHandle {
		Update,
		OnGUI,
		FixedUpdate
	}
}