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

using GameAnax.Core.Utility;


namespace GameAnax.Core.FX.Viewport {
	public class RotateCamFilter : MonoBehaviour, ICameraFilterBase {
		bool _isEffectOn;
		UnityEngine.Camera _camera;
		//
		public float actulCamSize = 7.68f;
		public float maxCamSize;
		public float maxRotate;
		public float timeToRotate;
		// Use this for initialization
		void Awake() {
			_camera = GetComponent<UnityEngine.Camera>();
		}

		public void StartFilter() {
			MyDebug.Log("Rotate Cam Effect Started");
			PostEffectManager.Me.isEffectOn = true;
			_isEffectOn = true;
			RotateFilterLeft();
		}

		#region Rotate Filter
		void SetValRotateFilter(float value) {
			if(!_isEffectOn) {
				return;
			}
			_camera.orthographicSize = value;
		}
		void RotateFilterLeft() {
			iTween.RotateTo(gameObject, iTween.Hash("z", -maxRotate, "time", timeToRotate, "easetype", iTween.EaseType.linear,
				"oncomplete", "RotateFilterRight"));
			iTween.ValueTo(gameObject, iTween.Hash("from", actulCamSize, "to", maxCamSize, "time", timeToRotate, "easetype", iTween.EaseType.linear,
				"onupdate", "SetValRotateFilter"));
		}
		void RotateFilterRight() {
			iTween.RotateTo(gameObject, iTween.Hash("z", maxRotate, "time", timeToRotate * 2f, "easetype", iTween.EaseType.linear,
				"oncomplete", "StopFliter"));
		}

		public void StopFliter() {
			MyDebug.Log("Stop Fliter Called for Rotate Cam");
			this.CancelInvoke("StopFliter");
			iTween.Stop(gameObject);
			iTween.RotateTo(gameObject, iTween.Hash("z", 0, "time", timeToRotate, "easetype", iTween.EaseType.linear));
			iTween.ValueTo(gameObject, iTween.Hash("from", _camera.orthographicSize, "to", actulCamSize, "time", timeToRotate,
				"easetype", iTween.EaseType.linear,
				"onupdate", "SetValRotateFilter", "oncomplete", "ResetValues"));
		}
		public void ForceStopFliter() {
			MyDebug.Log("Force Stop Fliter Called for Rotate Cam");
			this.CancelInvoke("StopFliter");
			iTween.Stop(gameObject);
			iTween.RotateTo(gameObject, iTween.Hash("z", 0, "time", 0.2f, "easetype", iTween.EaseType.linear,
				"ignoretimescale", true));
			iTween.ValueTo(gameObject, iTween.Hash("from", _camera.orthographicSize, "to", actulCamSize, "time", 0.2f,
				"easetype", iTween.EaseType.linear, "ignoretimescale", true,
				"onupdate", "SetValRotateFilter", "oncomplete", "ResetValues"));
		}
		public void ResetValues() {
			this.CancelInvoke("StopFliter");
			PostEffectManager.Me.isEffectOn = false;
			_isEffectOn = false;
			_camera.orthographicSize = actulCamSize;
			transform.localEulerAngles = Vector3.zero;
		}
		#endregion
	}
}