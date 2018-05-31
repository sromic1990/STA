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

using System;
using System.Collections.Generic;

using UnityEngine;
using GameAnax.Core.Utility;
using UnityEngine.UI;


namespace GameAnax.Core.Animation {
	public class SpriteTextureSwapAnimation : MonoBehaviour {
		public Action<AnimationStateChnage> AnimationStateChange;
		AnimationStateChnage _lstAnimationStateChnage;
		//
		float _hold;
		float _timeMoved;
		int _index;
		int _direction = 1;
		int _speed = 1;
		Vector3 _finalScale;

		MAnimation _curAni;
		Image uiImage;
		SpriteRenderer spriteRendere;
		Renderer meshRenderer;


		[SerializeField]
		private RunWith runWith = RunWith.Sprite;
		[SerializeField]
		private int aniIndex = -1;
		[SerializeField]
		private Vector3 commonScaleMultiplier;
		[SerializeField]
		private List<MAnimation> characters;

		[HideInInspector]
		public AnimationState status = AnimationState.Stop;

		// Use this for initialization
		void Awake() {
			uiImage = GetComponent<Image>();
			spriteRendere = GetComponent<SpriteRenderer>();
			meshRenderer = GetComponent<Renderer>();
		}
		void Start() {
			//GetAnimation();
			//CalculateAnimationDuration();
		}
		void OnEnable() {
			GetStaus();
		}

		// Update is called once per frame
		void Update() {
			if(status != AnimationState.Play || _curAni == null || aniIndex.Equals(-1)) {
				//MyDebug.Log("Animation return ST: " + status + ", aniIndex: " + aniIndex + ", isNull: " + (_curAni == null));
				return;
			}
			_timeMoved += ((_curAni.IsIgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * _speed);

			_hold = (1f / _curAni.FPS);
			if(_timeMoved >= _hold) {
				switch(_curAni.AType) {
				case AnimationType.Once:
					//MyDebug.Log("Animation Frame Changed");
					_index++;
					if(runWith.Equals(RunWith.Sprite)) {
						//MyDebug.Log("Animation Sprite Change");
						_index = Mathf.Clamp(_index, 0, _curAni.Sprites.Count);
						if(_index >= _curAni.Sprites.Count) {
							_index = _curAni.Sprites.Count - 1;
							Stop();
							OnAnimationStateChange(AnimationStateChnage.End);
							return;
						}
					} else if(runWith.Equals(RunWith.Texture2D)) {
						_index = Mathf.Clamp(_index, 0, _curAni.Textures.Count);
						if(_index >= _curAni.Textures.Count) {
							_index = _curAni.Textures.Count - 1;
							Stop();
							OnAnimationStateChange(AnimationStateChnage.End);
							return;
						}
					}
					break;

				case AnimationType.Loop:
					_index++;
					if(runWith.Equals(RunWith.Sprite)) {
						if(_index >= _curAni.Sprites.Count) {
							OnAnimationStateChange(AnimationStateChnage.LoopStartAgain);
							_index = 0;
						}
					} else if(runWith.Equals(RunWith.Texture2D)) {
						if(_index >= _curAni.Textures.Count) {
							OnAnimationStateChange(AnimationStateChnage.LoopStartAgain);
							_index = 0;
						}
					}
					break;

				case AnimationType.PingPong:
					_index += _direction;
					if(runWith.Equals(RunWith.Sprite) && _index >= (_curAni.Sprites.Count - 1)) {
						_direction = -1;
						OnAnimationStateChange(AnimationStateChnage.PingPongChange);
					} else if(runWith.Equals(RunWith.Texture2D) && _index >= (_curAni.Textures.Count - 1)) {
						_direction = -1;
						OnAnimationStateChange(AnimationStateChnage.PingPongChange);
					}
					if(_index <= 0) {
						_direction = 1;
						OnAnimationStateChange(AnimationStateChnage.PingPongChange);
					}
					break;
				}
				_timeMoved = 0;
				if(!_lstAnimationStateChnage.Equals(AnimationStateChnage.Playing)) {
					OnAnimationStateChange(AnimationStateChnage.Playing);
				}
				SetFrame();
			}
		}
		//
		void GetStaus() {
			if(aniIndex.Equals(-1) || _curAni == null) {
				return;
			}
			if(_curAni.IsAutoPlay) {
				status = AnimationState.Play;
			} else {
				status = AnimationState.Stop;
			}
		}
		void SetFrame() {
			if(aniIndex.Equals(-1)) {
				return;
			}
			if(uiImage) {
				if(_curAni.Sprites[_index] == null) {
					return;
				}
				uiImage.sprite = _curAni.Sprites[_index];
			} else if(spriteRendere) {
				//MyDebug.Warning("Animation Sprite Updating");
				if(_curAni.Sprites[_index] == null) {
					return;
				}
				spriteRendere.sprite = _curAni.Sprites[_index];
				//MyDebug.Warning("Animation Sprite Updated");
			} else if(meshRenderer) {
				if(_curAni.Textures[_index] == null) {
					return;
				}
				meshRenderer.material.mainTexture = _curAni.Textures[_index];
			}
		}
		//
		private void Play() {
			//MyDebug.Warning("Animatin play Start");
			_index = 0;
			_direction = 1;
			_speed = 1;
			status = AnimationState.Play;
			OnAnimationStateChange(AnimationStateChnage.Start);
		}
		public void Stop() {
			status = AnimationState.Stop;
			_direction = 1;
			_speed = 0;
			SetFrame();
			OnAnimationStateChange(AnimationStateChnage.Stop);
		}
		public void Pause() {
			status = AnimationState.Pause;
			_speed = 0;
			SetFrame();
			OnAnimationStateChange(AnimationStateChnage.Pause);

		}
		public void Resume() {
			status = AnimationState.Play;
			_speed = 1;
			SetFrame();
			OnAnimationStateChange(AnimationStateChnage.Resume);

		}

		public void Play(int newAnimationIndex) {
			if(newAnimationIndex >= characters.Count || newAnimationIndex < 0) {
				throw new IndexOutOfRangeException("Provided animation index is not valid");
			}
			if(characters[newAnimationIndex] == null) {
				throw new NullReferenceException("There are no animation at provided index");
			}
			aniIndex = newAnimationIndex;
			//MyDebug.Log("Getting animation");
			GetAnimation();
			//MyDebug.Log("trying to play animation");
			Play();
		}
		public void Play(string animationName) {
			int index = GetAnimatinIndexByName(animationName);
			Play(index);
		}

		void GetAnimation() {
			if(characters == null || characters.Count <= 0 || aniIndex.Equals(-1) || aniIndex >= characters.Count) {
				_curAni = null;
				aniIndex = -1;
			} else {
				_curAni = characters[aniIndex];
				//MyDebug.Warning(_curAni.AnimationName);

				_finalScale = new Vector3(
					_curAni.Scale.x * commonScaleMultiplier.x,
					_curAni.Scale.y * commonScaleMultiplier.y,
					_curAni.Scale.z * commonScaleMultiplier.z);
				transform.localScale = _finalScale;
			}
			GetStaus();
		}
		void CalculateAnimationDuration() {
			foreach(MAnimation chr in characters) {
				chr.Duration = 0f;
				if(chr.Sprites.Count > 0) {
					chr.Duration = ((float)chr.Sprites.Count / chr.FPS);
				} else if(chr.Textures.Count > 0) {
					chr.Duration = ((float)chr.Textures.Count / chr.FPS);
				}
			}
		}
		int GetAnimatinIndexByName(string aniName) {
			int index = -1;
			for(int i = 0; i < characters.Count; i++) {
				if(characters[i].AnimationName.Equals(aniName)) {
					index = i;
					break;
				}
			}
			if(index < 0) {
				throw new System.Exception(string.Format("Animation {0} is not setup on obejct: {1}", aniName, name));
			}
			return index;
		}

		void OnAnimationStateChange(AnimationStateChnage newState) {
			if(AnimationStateChange != null) {
				AnimationStateChange.Invoke(newState);
			}
			_lstAnimationStateChnage = newState;
		}
	}

	public enum AnimationType {
		Once,
		Loop,
		PingPong
	}
	public enum AnimationState {
		Play,
		Pause,
		Stop
	}
	public enum AnimationStateChnage {
		Start,
		Stop,
		Resume,
		Pause,
		End,
		LoopStartAgain,
		PingPongChange,
		Playing
	}

	public enum RunWith {
		Texture2D,
		Sprite
	}

	[System.Serializable]
	public class MAnimation {
		[Space(5)]
		public string AnimationName;
		[Range(0, 120)]
		public float FPS;
		//[NonSerialized]
		public float Duration;
		//
		[Space(5)]
		public bool IsAutoPlay;
		public bool IsIgnoreTimeScale;
		//
		[Space(5)]
		public AnimationType AType;
		public List<Sprite> Sprites;
		public List<Texture> Textures;
		//
		[Space(5)]
		public Vector3 Scale;

		public MAnimation() {
			AnimationName = "";
			FPS = 18;
			Duration = 0;

			//
			IsAutoPlay = false;
			IsIgnoreTimeScale = false;
			//
			AType = AnimationType.Loop;
			Sprites = new List<Sprite>();
			Textures = new List<Texture>();
			//
			Scale = Vector3.one;
		}
	}
}