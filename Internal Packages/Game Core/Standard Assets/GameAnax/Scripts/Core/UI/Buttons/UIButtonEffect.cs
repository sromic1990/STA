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
using UnityEngine.UI;
using JetBrains.Annotations;


namespace GameAnax.Core.UI.Buttons {
	[System.Serializable]
	public class UIButtonChildEffect : IUIButtonEffect {

		[SerializeField]
		private bool _isEnable;
		public bool isEnable { get { return _isEnable; } }
		public GameObject child;

		[SerializeField]
		private Sprite _sprite;
		public Sprite sprite { get { return _sprite; } }
		[SerializeField]
		private Color _color;
		public Color color { get { return _color; } }


		protected UIButtonChildEffect() : this(null, Color.white) { }
		protected UIButtonChildEffect(Sprite s) : this(s, Color.white) { }
		protected UIButtonChildEffect(Color c) : this(null, c) { }
		protected UIButtonChildEffect(Sprite s, Color c) { _sprite = s; _color = c; }
	}


	public interface IUIButtonEffect {
		bool isEnable { get; }
		Sprite sprite { get; }
		Color color { get; }
	}

	[System.Serializable]
	public class UIButtonEffect : IUIButtonEffect {
		[SerializeField]
		private bool _isMainEffectEnable;
		public bool isMainEffectEnable { get { return _isMainEffectEnable; } }
		[SerializeField]
		private bool _isEnable;
		public bool isEnable { get { return _isEnable; } }

		[SerializeField]
		private Sprite _sprite;
		public Sprite sprite { get { return _sprite; } }
		[SerializeField]
		private Color _color;
		public Color color { get { return _color; } }

		public List<UIButtonChildEffect> childEffects;

		public UIButtonEffect() : this(null, Color.white) { }
		public UIButtonEffect(Sprite s) : this(s, Color.white) { }
		public UIButtonEffect(Color c) : this(null, c) { }
		public UIButtonEffect(Sprite s, Color c) { _sprite = s; _color = c; }

		private Image image;
		private Text text;
		private Color tmpColor;
		private Sprite tmpSprite;


		private void UpdateUI(GameObject container, ButtonTransitionStyle transition, IUIButtonEffect effect, IUIButtonEffect regularEffect) {
			if(!effect.isEnable) return;
			tmpSprite = null;
			bool isChangeColor = true;
			image = container.GetComponent<Image>();
			text = container.GetComponent<Text>();
			if((null == image && null == text)) return;

			switch(transition) {
			case ButtonTransitionStyle.ColorSwap:
				tmpColor = effect.color;
				break;

			case ButtonTransitionStyle.ColorMultiplie:
				if(regularEffect.color.Equals(effect.color)) {
					tmpColor = effect.color;
				} else {
					tmpColor = regularEffect.color * effect.color;
				}
				break;

			case ButtonTransitionStyle.SpriteSwap:
				tmpSprite = effect.sprite != null ? effect.sprite : regularEffect.sprite;
				tmpColor = effect.color;
				isChangeColor = false;
				break;

			default:
				return;
				break;
			}


			if(null != image) {
				if(null != tmpSprite) image.sprite = tmpSprite;
				if(isChangeColor) image.color = tmpColor;
			} else if(null != text)
				text.color = tmpColor;

		}

		public void ApplyUIEffect(GameObject container, ButtonTransitionStyle transition, IUIButtonEffect alternetEffect) {
			if(!_isMainEffectEnable) return;
			if(isEnable) {
				UpdateUI(container, transition, this, alternetEffect);
			} else if(alternetEffect.isEnable) {
				UpdateUI(container, transition, alternetEffect, alternetEffect);
			}
			for(int i = 0; i < childEffects.Count; i++) {
				if(childEffects[i].isEnable) {
					UpdateUI(childEffects[i].child, transition, childEffects[i], ((UIButtonEffect)alternetEffect).childEffects[i]);
				}
			}

		}
	}
}