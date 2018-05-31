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

using GameAnax.Core.Singleton;


namespace GameAnax.Core.InputSystem {
	[PersistentSignleton(true, true)]
	public class MouseInput : SingletonAuto<MouseInput> {
		public Vector2 invalidPos { get { return new Vector2(int.MinValue, int.MinValue); } }
		public Vector2 touchPressed { get; private set; }
		public Vector2 touchDown { get; private set; }
		public Vector2 touchUp { get; private set; }
		public Vector2 mousePosition { get; private set; }
		public Vector2 mouseDeltaPosition { get; private set; }

		public bool isTouchDown { get; private set; }
		public bool isTouchPressed { get; private set; }
		public bool isTouchUp { get; private set; }

		private Vector2 _lastMousePosition;

		// Update is called once per frame
		void Update() {
			mousePosition = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			if(!_lastMousePosition.Equals(Vector3.zero))
				mouseDeltaPosition = mousePosition - _lastMousePosition;

#if !UNITY_EDITOR && !UNITY_STANDALONE
			if(!Input.GetMouseButtonDown(0) &&
			     !Input.GetMouseButton(0) &&
			     !Input.GetMouseButtonUp(0)) {
				mousePosition = invalidPos;
			}
#endif
			if(Input.GetMouseButtonDown(0)) {
				touchDown = mousePosition;
				isTouchDown = true;
			} else {
				isTouchDown = false;
				touchDown = invalidPos;
			}

			if(Input.GetMouseButton(0)) {
				touchPressed = mousePosition;
				isTouchPressed = true;
			} else {
				touchPressed = invalidPos;
				isTouchPressed = false;
			}

			if(Input.GetMouseButtonUp(0)) {
				touchUp = mousePosition;
				isTouchUp = true;
			} else {
				touchUp = invalidPos;
				isTouchUp = false;
			}
			_lastMousePosition = mousePosition;
		}
	}
}