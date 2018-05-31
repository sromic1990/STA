//
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	N/A
// Last Modified: 	Ranpariya Ankur
// Contributed By:	Panakaj Chhopala
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
	[RequireComponent(typeof(ParticleSystem))]
	public class UnscaledTimeParticleAnimator : MonoBehaviour {
		bool _isSimulate;
		float _deltaTime;
		ParticleSystem _pSystem;
		ParticleSystem.MainModule _main;

		// Use this for initialization
		void Awake() {
			_pSystem = GetComponent<ParticleSystem>();
			_main = _pSystem.main;
			_isSimulate = _main.playOnAwake;
		}

		public void Update() {
			if(!_isSimulate) {
				return;
			}
			_deltaTime += Time.unscaledDeltaTime;
			if(_deltaTime >= _main.duration && _main.loop) {
				_deltaTime -= _main.duration;
			} else if(_deltaTime >= _main.duration && !_main.loop) {
				Stop();
			}
			_pSystem.Simulate(Time.unscaledDeltaTime, true, false);
		}

		public void Simulate() {
			_deltaTime = 0;
			_isSimulate = true;
			_pSystem.Simulate(Time.unscaledDeltaTime, true, true);
		}
		public void Stop() {
			_isSimulate = false;
			_deltaTime = 0;
			_pSystem.Stop();
		}
		public void Play() {
			_isSimulate = false;
			_deltaTime = 0;
			_pSystem.Play();
		}
	}
}