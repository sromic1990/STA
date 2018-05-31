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


namespace GameAnax.Core.Sound {
	//[RequireComponent(typeof(AudioSource))]
	public class AudioManager : MonoBehaviour {
		public PlayArea currentArea { get; set; }

		bool _isFirstASource;
		bool _isSwapping = false;

		AudioSource _proxyLast;
		AudioSource _proxyNew;

		public AudioClip SClip {
			get {
				GetProxies();
				return _proxyNew != null ? _proxyNew.clip : null;
			}
		}

		float _volume = 0;
		bool _isMute = false;

		public AudioSource first;
		public AudioSource second;
		public AudioType soundType;
		public PlayArea area;

		[Range(0.01f, 2f)]
		public float PersonalVolumeFactor = 1;

		// Use this for initialization
		void Awake() {
		}
		void Start() {
			GetProxies();
			VolumeSetter();
			if(_proxyNew.playOnAwake) {
				_proxyNew.Play();
			}
		}
		void OnEnable() {
			GetProxies();
			if(_proxyNew.playOnAwake) {
				_proxyNew.Play();
			}

		}
		// Update is called once per frame
		void Update() {
			VolumeSetter();

		}
		private void VolumeSetter() {
			if(!area.Equals(currentArea) && !area.Equals(PlayArea.Both)) {
				if(first)
					first.Pause();
				if(second)
					second.Pause();
			}
			switch(soundType) {
			case AudioType.SFX:
				_volume = CoreUtility.Me.settings.sfxVolume;
				_isMute = !CoreUtility.Me.settings.isSFX;
				break;

			case AudioType.MUSIC:
				_volume = CoreUtility.Me.settings.musicVolume;
				_isMute = !CoreUtility.Me.settings.isMusic;
				break;

			case AudioType.SFXPASUE:
				_volume = CoreUtility.Me.settings.sfxVolume;
				_isMute = CoreMethods.isSFXPaused;
				break;

			case AudioType.MUSICPAUSE:
				_volume = CoreUtility.Me.settings.musicVolume;
				_isMute = CoreMethods.isBGPaused;
				break;
			}
			if(!_isSwapping) {
				_proxyNew.volume = _volume * PersonalVolumeFactor;
				//if(second) {
				//	second.volume = _volume * PersonalVolumeFactor;
				//}
			}
			_proxyNew.mute = _isMute;
			_proxyLast.mute = _isMute;
		}
		public void GetProxies() {
			if(_isFirstASource) {
				_proxyLast = second;
				_proxyNew = first;
			} else {
				_proxyLast = first;
				_proxyNew = second;
			}
		}
		public void SetClip(AudioClip clip) {
			GetProxies();
			if(_proxyNew) {
				_proxyNew.clip = clip;
			}
		}
		public void Play(AudioClip aClip, bool isLoop) {
			SetClip(aClip);
			Play(isLoop);
		}

		public void Play(AudioClip aClip) {
			SetClip(aClip);
			Play(false);
		}
		public void Play(bool isLoop) {
			GetProxies();
			if(_proxyLast)
				_proxyLast.Stop();
			if(_proxyNew) {
				_proxyNew.loop = isLoop;
				_proxyNew.Play();
			}

		}
		public void Play() {
			Play(false);
		}
		public void Stop() {
			if(first) {
				first.Stop();
			}
			if(second) {
				second.Stop();
			}
		}

		public bool IsPlaying() {
			bool retvalue = false;
			GetProxies();
			retvalue = _proxyNew.isPlaying;
			return retvalue;
		}

		public void CrossFade(AudioClip aClip, float swapTime) {
			CrossFade(aClip, swapTime, true, false);
		}
		public void CrossFade(AudioClip aClip, float swapTime, bool isLoop) {
			CrossFade(aClip, swapTime, true, isLoop);
		}

		public void CrossFade(AudioClip aClip, float swapTime, bool isStart, bool isLoop) {
			_isSwapping = true;
			_isFirstASource = !_isFirstASource;
			GetProxies();
			_proxyNew.volume = 0;
			_proxyLast.volume = _volume * PersonalVolumeFactor; ;
			_proxyNew.clip = aClip;
			if(isStart) {
				_proxyNew.loop = isLoop;
				_proxyNew.Play();
			} else
				_proxyNew.Stop();

			iTween.Stop(gameObject);
			iTween.ValueTo(gameObject, iTween.Hash("name", "swapeSound",
				"time", swapTime, "from", 0f, "to", _volume,
				"onUpdate", "ChangeVolume", "onUpdateTarget", gameObject,
				"onComplete", "ChangeComplete", "onCompleteTarget", gameObject));
		}

		public void ChangeVolume(float value) {
			_proxyLast.volume = (_volume - value) * PersonalVolumeFactor; ;
			_proxyNew.volume = value * PersonalVolumeFactor; ;
		}
		public void ChangeComplete() {
			_proxyLast.Stop();
			_proxyLast.clip = null;
			_isSwapping = false;
		}
	}

	public enum AudioType {
		SFX,
		MUSIC,
		SFXPASUE,
		MUSICPAUSE,
	}
	public enum PlayArea {
		Menus,
		GamePlay,
		Both,
		None
	}
}