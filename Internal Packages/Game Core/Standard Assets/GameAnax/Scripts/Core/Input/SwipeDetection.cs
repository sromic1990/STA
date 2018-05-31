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
using GameAnax.Core.Singleton;


namespace GameAnax.Core.InputSystem {
	public delegate void Siwpe(Direction swipeDirection);
	[PersistentSignleton(true, true)]
	public class SwipeDetection : SingletonAuto<SwipeDetection> {
		public event Siwpe SwipeDetected;
		/// <summary>
		/// The direction threshhold in pixel.
		/// </summary>
		public float directionThreshhold = 30;
		/// <summary>
		/// The event threshhold. is swipe distance / swipe time
		/// </summary>
		public float eventThreshhold = 1;

		bool _isDown;
		float _time, _threshX, _threshY;
		Vector2 _swipe;
		int _swipeDir = 0;

		private void OnSwipeDetected(Direction direction) {
			if(null != SwipeDetected) {
				SwipeDetected.Invoke(direction);
			}
		}

		void Update() {
			if(MouseInput.Me.isTouchDown) {
				_isDown = true;
				_swipe = MouseInput.Me.touchDown;
				_time = Time.realtimeSinceStartup;
				_swipeDir = 0;
			}
			if(_isDown && MouseInput.Me.isTouchUp) {
				_swipe = MouseInput.Me.touchUp - _swipe;
				_time = Time.realtimeSinceStartup - _time;
				_isDown = false;
				_threshX = Mathf.Abs(_swipe.x) / _time;
				_threshY = Mathf.Abs(_swipe.y) / _time;

				if(Mathf.Abs(_threshX) > eventThreshhold && Mathf.Abs(_swipe.x) >= directionThreshhold) {
					if(_swipe.x > 0f) {
						_swipeDir = _swipeDir.SetFlag((int)Direction.Right);
					} else if(_swipe.x < 0f) {
						_swipeDir = _swipeDir.SetFlag((int)Direction.Left);
					}
				}
				if(Mathf.Abs(_threshY) > eventThreshhold && Mathf.Abs(_swipe.y) >= directionThreshhold) {
					if(_swipe.y < 0f) {
						_swipeDir = _swipeDir.SetFlag((int)Direction.Up);
					}
					if(_swipe.y > 0f) {
						_swipeDir = _swipeDir.SetFlag((int)Direction.Down);
					}
				}

				if(_swipeDir > 0) {
					OnSwipeDetected((Direction)_swipeDir);
				} else {
					_swipeDir = 0;
				}

			}
		}
	}


}