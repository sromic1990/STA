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
using UnityEngine.EventSystems;
using UnityEngine.UI;

using GameAnax.Core.Extension;
using GameAnax.Core.IO;
using GameAnax.Core.NotificationSystem;
using GameAnax.Core.Utility;

namespace GameAnax.Core.UI.Buttons {
	[RequireComponent(typeof(CanvasRenderer)), RequireComponent(typeof(RectTransform)), DisallowMultipleComponent]
	public class UIButton : Graphic, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
		Transform _tr;
		public bool isPointerIn { get; set; }
		//
		bool _isSelected;
		public bool isSelected {
			get { return _isSelected; }
		}
		bool _isDisable;
		public bool isDisabled {
			get { return _isDisable; }
		}
		bool _isTouchDownOnButton;
		public bool isDowned {
			get { return _isTouchDownOnButton; }
		}
		//
		[Space(5)]
		public ButtonTransitionStyle transition = ButtonTransitionStyle.ColorSwap;
		public CacheFileCategories fileKind;
		public ButtonStyle style;
		public Buffering downloadProcess;
		//
		[Space(5)]
		public Text mainText;
		public Image mainIconOrImage;
		//
		[Space(5)]
		public string buttonGroup;


		[Space(5)]
		public UIButtonEffect regularEffect = new UIButtonEffect(Color.white);
		public UIButtonEffect hoverEffect = new UIButtonEffect(Color.white);
		public UIButtonEffect selectedEffect = new UIButtonEffect(Color.white);
		public UIButtonEffect selectedHoverEffect = new UIButtonEffect(Color.white);
		public UIButtonEffect disableEffect = new UIButtonEffect(ColorExtension.HexToRGBColor("C8C8C880"));

		[Space(5)]
		[SerializeField]
		protected internal ButtonEvent onClick;
		[SerializeField]
		protected internal ButtonEvent onDisableClick;

		protected override void Awake() {
			base.Awake();
			_tr = GetComponent<Transform>();
		}
		protected override void OnDestroy() {
			base.OnDestroy();
			onClick.RemoveAllListner();
		}
		protected override void OnEnable() {
			base.OnEnable();
			if(_isSelected && selectedEffect.isMainEffectEnable) ChangeEffect(selectedEffect, regularEffect);
		}
		protected override void OnDisable() {
			base.OnDisable();
			TouchExits();
		}

		#region Button setup
		/// <summary>
		/// Sets the button main Text, subtext and icons or image received form Option class
		/// </summary>
		/// <param name="textToShow">Main text receveid from option.</param>
		public virtual void SetButton(string textToShow) {
			SetButton(textToShow, string.Empty);
		}
		/// <summary>
		/// Sets the button main Text, subtext and icons or image received form Option class
		/// </summary>
		/// <param name="textToShow">Main text receveid from option.</param>
		/// <param name="icon1">Mian Icon or Image receveid from option.</param>
		public virtual void SetButton(string textToShow, string icon1) {
			if(null != this.mainText) {
				this.mainText.text = textToShow.TrimAll();
				this.mainText.gameObject.SetActive(!string.IsNullOrEmpty(this.mainText.text));
			}
			if(!string.IsNullOrEmpty(icon1)) {
				DownloadProcess(true);
				StartCoroutine(ImageUtility.Me.SetIconOrImage(mainIconOrImage, icon1, "", "", fileKind,
					(arg1, arg2) => { DownloadProcess(false); }
				));
			} else if(null != mainIconOrImage) {
				mainIconOrImage.gameObject.SetActive(false);
			}
		}
		#endregion

		#region Pointer Event Listner base on Interface
		/// <summary>
		/// Ons the pointer click.
		/// </summary>
		/// <param name="ped">Ped.</param>
		public virtual void OnPointerClick(PointerEventData ped) { OnClick(); }
		/// <summary>
		/// Ons the pointer enter.
		/// </summary>
		/// <param name="ped">Ped.</param>
		public virtual void OnPointerEnter(PointerEventData ped) { TouchEnter(); }
		/// <summary>
		/// Ons the pointer exit.
		/// </summary>
		/// <param name="ped">Ped.</param>
		public virtual void OnPointerExit(PointerEventData ped) { TouchExits(); }
		/// <summary>
		/// Ons the pointer down.
		/// </summary>
		/// <param name="ped">Ped.</param>
		public virtual void OnPointerDown(PointerEventData ped) { TouchDown(); }
		/// <summary>
		/// Ons the pointer up.
		/// </summary>
		/// <param name="ped">Ped.</param>
		public virtual void OnPointerUp(PointerEventData ped) { TouchUp(); }
		#endregion

		#region overrides from Graphic Class to do not render any thing it self
		public override void SetMaterialDirty() { return; }
		public override void SetVerticesDirty() { return; }
		// Probably not necessary since the chain of calls `Rebuild()`->`UpdateGeometry()`->`DoMeshGeneration()`->`OnPopulateMesh()` won't happen; so here really just as a fail-safe.
		protected override void OnPopulateMesh(VertexHelper vh) {
			vh.Clear();
			return;
		}
		#endregion

		#region System Action Event Executor
		private void TouchEnter() {
			isPointerIn = true;
			if(_isDisable) return;

			if(!_isSelected && hoverEffect.isMainEffectEnable) ChangeEffect(hoverEffect, regularEffect);
			else if(_isSelected && selectedHoverEffect.isMainEffectEnable) ChangeEffect(selectedHoverEffect, selectedEffect);
		}
		private void TouchExits() {
			isPointerIn = false;
			if(_isDisable) return;


			if(!_isSelected && regularEffect.isMainEffectEnable) ChangeEffect(regularEffect, regularEffect);
			else if(_isSelected && selectedEffect.isMainEffectEnable) ChangeEffect(selectedEffect, regularEffect);
		}
		private void TouchDown() {
			_isTouchDownOnButton = true;
		}
		private void TouchUp() {
			if(isPointerIn) { } else { }
			_isTouchDownOnButton = false;
		}

		public void OnClick() {
			if(_isDisable) {
				onDisableClick.Invoke();
				ExecuteCustomEvents(onDisableClick);
			} else {
				Select();
				onClick.Invoke();
				ExecuteCustomEvents(onClick);
			}
		}
		#endregion

		public void Disable(bool newValue) {
			_isDisable = newValue;
			if(_isDisable && disableEffect.isMainEffectEnable) ChangeEffect(disableEffect, regularEffect);
			else {
				if(!_isSelected && regularEffect.isMainEffectEnable) ChangeEffect(regularEffect, regularEffect);
				else if(_isSelected && selectedEffect.isMainEffectEnable) ChangeEffect(selectedEffect, regularEffect);
			}
		}

		/// <summary>
		/// Select this instance.
		/// </summary>
		public void Select() {
			if(!buttonGroup.IsNulOrEmpty())
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "UpdateGroupSelection", buttonGroup));
			if(_isDisable) return;

			switch(style) {
			case ButtonStyle.Selectable:
				_isSelected = true;
				if(selectedEffect.isMainEffectEnable) ChangeEffect(selectedEffect, regularEffect);
				break;

			case ButtonStyle.Toggle:
				_isSelected = !_isSelected;
				if(_isSelected && selectedEffect.isMainEffectEnable) ChangeEffect(selectedEffect, regularEffect);
				else Deselect();
				break;
			}
		}
		/// <summary>
		/// Des the select.
		/// </summary>
		public void Deselect() {
			_isSelected = false;
			if(_isDisable) return;

			if(isPointerIn) {
				if(hoverEffect.isMainEffectEnable) ChangeEffect(hoverEffect, regularEffect);
				else if(regularEffect.isMainEffectEnable) ChangeEffect(regularEffect, regularEffect);
			} else {
				if(regularEffect.isMainEffectEnable) ChangeEffect(regularEffect, regularEffect);
			}
		}
		/// <summary>
		/// Changes the effect.
		/// </summary>
		/// <param name="effect">Effect.</param>
		void ChangeEffect(UIButtonEffect effect, IUIButtonEffect alternetEffects) {

			if(!effect.isMainEffectEnable) return;
			effect.ApplyUIEffect(gameObject, transition, alternetEffects);
		}

		public virtual void DownloadProcess(bool isActive) {
			if(null == downloadProcess) return;
			if(isActive)
				downloadProcess.StartLoading();
			else
				downloadProcess.StopLoading();
		}


		void ExecuteCustomEvents(ButtonEvent events) {
			if(events.baseData.isSound) { }
			events.ExecuteEvents(gameObject, this);
		}

	}

	public enum ButtonTransitionStyle {
		None,
		ColorMultiplie,
		ColorSwap,
		SpriteSwap
	}
	public enum ButtonStyle {
		Regular,
		Selectable,
		Toggle
	}
}

