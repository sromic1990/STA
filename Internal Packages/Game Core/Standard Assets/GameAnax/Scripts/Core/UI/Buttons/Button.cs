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

using UnityEngine;

using GameAnax.Core.Attributes;
using GameAnax.Core.Extension;
using GameAnax.Core.InputSystem;
using GameAnax.Core.NotificationSystem;
//using GameAnax.Core.Sound;
using GameAnax.Core.Utility;


namespace GameAnax.Core.UI.Buttons {
	public class Button : MonoBehaviour {
		Transform _tr;
		Transform[] _childrens;
		Renderer _rendr;
		bool _isDisabled, _isSelected, _isDrag, _isTouchDown, _isTouchUp, _isTouchPress;
		int _fingerCount;
		float _lastRectDraw;
		Vector2 _touchDown, _touchUp, _touchPress, _touchPosition, _invalidTouchPosition;


		TextMesh _chkTextMesh;
#if EX2D
		exSpriteFont _chkSpriteFont;
		exSpriteBorder _chkSpriteBorder;
		//
		exSpriteFont _chkSpriteFontChild;
		exSpriteBorder _chkSpriteBorderChild;
#endif
#if UNITY_4 || UNITY_5 || UNITY_5_3_OR_NEWER
		SpriteRenderer _chkSpriteRender, _chkSpriteRenderChild;
#endif
		MeshFilter _meshFilter;
		bool _isMouseIn, _isClickStarts;
		DateTime _clickTime, _lastClickTime;
		//

		public Camera rectCamera;
		[Space(10)]

		public bool isClickOnDragSlide = true;
		public bool isCheckDoubleClick = false;
		public float rectDrawDealy = 0.2f;

		[Space(10)]
		[EnumFlagAttribute]
		public Menus layer = 0;
		public Rect touchBufferPercent = new Rect(0, 0, 0, 0);
		[NonSerialized]
		public Rect touchZone;
		[NonSerialized]
		public int fingerID = -1;

		[Space(10)]
		public ButtonEvent onDownEvent;
		public ButtonEvent onPressEvent;
		public ButtonEvent onUpEvent;
		public ButtonEvent onClickCancelEvent;
		public ButtonEvent onClickEvent;
		public ButtonEvent onDisableClick;
		public ButtonEvent onDoubleClick;
		public ButtonEvent onGotFocusEvent;
		public ButtonEvent onLostFocusEvent;

		[Space(10)]
		public ButtonEffects regularEffect;
		public ButtonEffects clickEffect;
		public ButtonEffects hoverEffect;
		public ButtonEffects disableEffect;
		public ButtonEffects selectedEffect;
		public ButtonEffects selectedClickEffect;
		public ButtonEffects selectedHoverEffect;

		public bool isSelected {
			get { return _isSelected; }
		}

		// Use this for initialization
		void Awake() {
			_tr = GetComponent<Transform>();
			_childrens = GetComponentsInChildren<Transform>();

			if(!_rendr) {
				_rendr = GetComponent<Renderer>();
			}
			_invalidTouchPosition = new Vector3(int.MinValue, int.MinValue);
		}
		void Start() {
			_lastRectDraw = Time.realtimeSinceStartup;
		}
		// Update is called once per frame
		void Update() {
			if(!layer.Contain(CoreMethods.layer)) {
				touchZone = new Rect(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
				return;
			}
			if(rectDrawDealy <= 0) {
				DrawRect();
			} else if(Time.realtimeSinceStartup - _lastRectDraw > rectDrawDealy) {
				_lastRectDraw = Time.realtimeSinceStartup;
				DrawRect();
			}
			_fingerCount = Input.touches.Length;
			if(_fingerCount > 0) {
				foreach(Touch tch in Input.touches) {
					_isTouchDown = (tch.phase == TouchPhase.Began);
					_isTouchUp = (tch.phase == TouchPhase.Canceled || tch.phase == TouchPhase.Ended);
					_isTouchPress = (tch.phase == TouchPhase.Stationary || tch.phase == TouchPhase.Moved);

					_touchPosition = tch.position;
					_touchPosition.y = Screen.height - _touchPosition.y;

					_touchDown = _isTouchDown ? _touchPosition : _invalidTouchPosition;
					_touchUp = _isTouchUp ? _touchPosition : _invalidTouchPosition;
					_touchPress = _isTouchPress ? _touchPosition : _invalidTouchPosition;
					if(fingerID == -1 || fingerID.Equals(tch.fingerId)) {
						CheckButtonEvents(tch.fingerId);
					}
				}

			} else {
				_touchPosition = MouseInput.Me.mousePosition;

				_isTouchDown = MouseInput.Me.isTouchDown;
				_isTouchUp = MouseInput.Me.isTouchUp;
				_isTouchPress = MouseInput.Me.isTouchPressed;

				_touchDown = MouseInput.Me.touchDown;
				_touchUp = MouseInput.Me.touchUp;
				_touchPress = MouseInput.Me.touchPressed;
				CheckButtonEvents();
			}
		}
		void OnBecameVisible() {
			//DrawRect();
			//lastRectDraw = Time.realtimeSinceStartup;
		}
		void OnBecameInvisible() {
			//CancelInvoke("DrawRect");
			//TouchZone = new Rect(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
		}
		void OnGUI() {
#if UNITY_EDITOR && GUIPRINT
			GUI.Box(touchZone, gameObject.name);
#endif
		}

		void CheckButtonEvents(int touchId = -1) {
			if(_isTouchUp && !isClickOnDragSlide) {
				if(Mathf.Abs(_touchDown.x - _touchUp.x) >= (Screen.width * 0.025f) ||
				   Mathf.Abs(_touchDown.y - _touchUp.y) >= (Screen.width * 0.025f)) {
					_isDrag = true;
				} else {
					_isDrag = false;
				}
			}

			fingerID = touchId;
			#region "Mouse / touch enter Envents for Hover in effect"
			if(touchZone.Contains(_touchPosition) && !_isMouseIn) {
				_isMouseIn = true;
				if(!_isDisabled) {
					this.ExecuteEvents(onGotFocusEvent);
					if(!_isSelected) {
						this.ChangeButtonUI(hoverEffect);
					} else {
						this.ChangeButtonUI(selectedHoverEffect);
					}
				}
			}
			#endregion

			#region "Mouse / touch down envents for click start"
			if(touchZone.Contains(_touchDown)) {
				if(!_isDisabled) {
					if(!_isClickStarts) {
						this.ExecuteEvents(onDownEvent);
						if(this._isSelected) {
							this.ChangeButtonUI(selectedClickEffect);
						} else {
							this.ChangeButtonUI(clickEffect);
						}
					}
				}
				if(!_isClickStarts) {
					_isClickStarts = true;
				}
			}
			#endregion

			#region  "Mouse / touch continue pressed envents for click continue"
			if(touchZone.Contains(_touchPress)) {
				if(!_isDisabled) {
					this.ExecuteEvents(onPressEvent);
				}
			}
			#endregion

			#region "Mouse / touch up and click envents"
			if(touchZone.Contains(_touchUp)) {
				if(!_isDisabled) {
					this.ExecuteEvents(onUpEvent);
#if(UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
				this.ExecuteEvents(onLostFocusEvent);
#endif
				}

				#region "Click and Double Clikc Event Settings"
				if(_isClickStarts) {
					_clickTime = DateTime.UtcNow;
					if((!isClickOnDragSlide && !_isDrag) || isClickOnDragSlide) {
						if(!_isDisabled) {
							if((_clickTime - _lastClickTime).TotalSeconds <= Constants.DOUBLE_CLICK_THRESH_HOLD
							   && isCheckDoubleClick) {
								this.ExecuteEvents(onDoubleClick);
							} else {
								this.ExecuteEvents(onClickEvent);
							}
						} else {
							this.ExecuteEvents(onDisableClick);
						}
					}
					_lastClickTime = _clickTime;
					if(!_isDisabled) {
						this.CheckIsSelcted();
						//isMouseIn = false;
					} else {
						this.ChangeButtonUI(disableEffect);
						_isMouseIn = false;
					}
				}
				#endregion
				_isClickStarts = false;
				fingerID = -1;
			}
			#endregion

			#region "Mouse / touch leave envents for Hover end & Click Cancel Setting"
			if(!touchZone.Contains(_touchPosition)) {
				#region "condition for Mouse Hover end"
				if(_isMouseIn) {
					if(!_isDisabled) {
						if(!_isSelected) {
							this.ChangeButtonUI(regularEffect);
						} else {
							this.ChangeButtonUI(selectedEffect);
						}

						_isMouseIn = false;
						this.ExecuteEvents(onLostFocusEvent);
						CheckIsSelcted();
					}
				}
				#endregion

				#region "condition for click cancel"
				if(_isClickStarts && _isTouchUp) {
					_isClickStarts = false;
					if(!_isDisabled) {
						this.ExecuteEvents(onClickCancelEvent);
					}
				}
				#endregion
				fingerID = -1;
			}
			#endregion

		}

		public void SetDisable(bool value) {
			_isDisabled = value;
			if(_isDisabled) {
				this.ChangeButtonUI(disableEffect);
			} else {
				CheckIsSelcted();
			}
		}
		public void SetSelcted(bool value) {
			if(_isDisabled) {
				return;
			}
			_isSelected = value;
			CheckIsSelcted();
		}

		void CheckIsSelcted() {
			if(_isDisabled) {
				return;
			}
			if(!_isSelected) {
				this.ChangeButtonUI(regularEffect);
			} else {
				this.ChangeButtonUI(selectedEffect);
			}
		}
		void DrawRect() {
			if(null == _rendr) {
				throw new MissingComponentException("Render component is missing");
			}
			touchZone = _rendr.GetBoundingRect(rectCamera, touchBufferPercent);
		}
		void ExecuteEvents(ButtonEvent cEvent) {
			cEvent.ExecuteEvents(gameObject, this);

		}
		void ChangeButtonUI(ButtonEffects effect) {
			effect.ChangeButtonUI(gameObject);
		}

	}
}