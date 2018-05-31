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


namespace GameAnax.Core.FX {
	public class ShakeObject : MonoBehaviour {
		Vector3 _localPos, _localAngles, _localPosition;
		public GameObject shakeObj;

		void Awake() {
			if(shakeObj == null)
				shakeObj = this.gameObject;
		}

		public void ShakeRot(float amount, float duration) {
			if(amount <= 0 || duration <= 0 || shakeObj.GetComponent<iTween>() != null) {
				return;
			}
			_localAngles = shakeObj.transform.localEulerAngles;
			iTween.ShakeRotation(shakeObj, iTween.Hash("x", amount, "z", amount, "time", duration,
				"islocal", true, "ignoretimescale", true,
				"oncomplete", "StopShakeRot", "oncompletetarget", gameObject));
		}
		public void StopShakeRot() {
			shakeObj.transform.localEulerAngles = _localAngles;
		}

		public void ShakePos(float amount, float duration) {
			if(amount <= 0 || duration <= 0 || shakeObj.GetComponent<iTween>() != null) {
				return;
			}
			_localPosition = shakeObj.transform.localPosition;
			iTween.ShakePosition(shakeObj, iTween.Hash("x", amount, "y", amount, "time", duration,
				"islocal", true, "ignoretimescale", true,
				"oncomplete", "StopShakePos", "oncompletetarget", gameObject));
		}
		public void StopShakePos() {
			shakeObj.transform.localPosition = _localPosition;
		}

		public void ShakeBoth(float amount, float duration) {
			ShakePos(amount, duration);
			ShakeRot(amount, duration);
		}

		public void Stroke(float amount, float duration) {
			// 
			if(amount <= 0 || duration <= 0 || shakeObj.GetComponent<iTween>() != null) {
				return;
			}

			_localPos = shakeObj.transform.localPosition;
			iTween.MoveAdd(shakeObj, iTween.Hash("z", -amount, "time", duration * 0.3f,
				"islocal", true, "ignoretimescale", true,
				"oncomplete", "StopStroke", "oncompleteparams", duration * 0.7f, "oncompletetarget", gameObject));

		}
		public void StopStroke(float duration) {
			shakeObj.transform.localPosition = _localPos;
			iTween.MoveTo(shakeObj, iTween.Hash("z", _localPos.z, "time", duration, "islocal", true));
		}
	}
}