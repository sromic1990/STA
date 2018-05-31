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

using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;


namespace GameAnax.Core.Sound {
	[PersistentSignleton(true, true)]
	public class SFX : SingletonAuto<SFX> {
		AudioClip _currentClip;
		AudioSource _audioSource;
		//
		public List<AudioClip> sfxClips = new List<AudioClip>();

		public bool isPlaying {
			get { return _audioSource.isPlaying; }
		}
		// Use this for initialization
		void Awake() {
			Me = this;
			_audioSource = GetComponent<AudioSource>();
		}

		void Update() {
			if(CoreMethods.isSFXPaused) {
				_audioSource.Pause();
			}
			_audioSource.volume = CoreUtility.Me.settings.sfxVolume;
			_audioSource.mute = !CoreUtility.Me.settings.isSFX;
		}

		public void StopSFX() {
			_audioSource.Stop();
		}

		public void AudioPlaySFX(int clipNo) {
			if(clipNo < 0) {
				return;
			}
			if(sfxClips.Count > 0 && sfxClips.Count > clipNo && sfxClips[clipNo] != null) {
				_currentClip = sfxClips[clipNo];
				_audioSource.clip = _currentClip;
				_audioSource.Play();
			}
		}

		public void PlayOneShot(int clipNo) {
			if(clipNo < 0) {
				return;
			}
			if(sfxClips.Count > 0 && sfxClips.Count > clipNo && sfxClips[clipNo] != null) {
				_audioSource.PlayOneShot(sfxClips[clipNo]);
			}
		}
	}
}