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
using UnityEngine.UI;

using System.Collections.Generic;


namespace GameAnax.Core.UI {
	[System.Serializable]
	public class ButtonEffects : ButtonEffectBase {
		public List<ButtonEffectBase> ChildObjects;
		public ButtonEffects() : base() { }
		public ButtonEffects(Color mainColor) : base(mainColor) { }


		private SpriteRenderer _chkSpriteRender;
		private TextMesh _chkTextMesh;
		private MeshFilter _meshFilter;
		private Image image;
#if EX2D
		private exSpriteFont _chkSpriteFont;
#endif
		public void ChangeButtonUI(GameObject container) {
			if(isEffect) {
				_chkSpriteRender = container.GetComponent<SpriteRenderer>();
				image = container.GetComponent<Image>();
#if EX2D
				_chkSpriteFont = container.GetComponent<exSpriteFont>();
				if(_chkSpriteFont) {
					if(bitmapFont) _chkSpriteFont.fontInfo = bitmapFont;
					
					_chkSpriteFont.topColor = color;
					_chkSpriteFont.botColor = fontBotttomColor;
					_chkSpriteFont.Commit();
				}  else
#endif
				if(null != image) {
					if(null != sprite) image.sprite = sprite;
					image.color = color;
				} else if(_chkSpriteRender) {
					_chkSpriteRender.color = color;
					if(null != sprite) _chkSpriteRender.sprite = sprite;

				} else {
					_chkTextMesh = container.GetComponent<TextMesh>();
					if(null == _chkTextMesh) container.GetComponent<Renderer>().material.SetColor("_Color", color);
					else _chkTextMesh.color = color;

					if(null == _chkTextMesh && null != mesh) {
						_meshFilter = container.GetComponent<MeshFilter>();
						if(null != _meshFilter) _meshFilter.mesh = mesh;
					}
				}
				if(null != texture)
					container.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);

			}

			foreach(ButtonEffectBase childEffect in ChildObjects) {
				if(childEffect.isEffect) {
					image = childEffect.child.GetComponent<Image>();
					_chkSpriteRender = childEffect.child.GetComponent<SpriteRenderer>();
#if EX2D
					_chkSpriteFont = childEffect.child.GetComponent<exSpriteFont>();

					if(null != _chkSpriteFont) {
						if(childEffect.bitmapFont) {
							_chkSpriteFont.fontInfo = childEffect.bitmapFont;
						}
						_chkSpriteFont.topColor = childEffect.color;
						_chkSpriteFont.botColor = childEffect.fontBotttomColor;
						_chkSpriteFont.Commit();
					} else
#endif
					if(null != image) {
						if(null != childEffect.sprite) image.sprite = childEffect.sprite;
						image.color = childEffect.color;
					} else if(_chkSpriteRender) {
						_chkSpriteRender.color = childEffect.color;
						if(childEffect.sprite) _chkSpriteRender.sprite = childEffect.sprite;

					} else {
						if(null != childEffect.child) {
							_chkTextMesh = childEffect.child.GetComponent<TextMesh>();
							if(null != _chkTextMesh) _chkTextMesh.color = childEffect.color;
							else childEffect.child.GetComponent<Renderer>().material.SetColor("_Color", childEffect.color);

							if(null != childEffect.texture) childEffect.child.GetComponent<Renderer>().material.SetTexture("_MainTex", childEffect.texture);

							if(null == _chkTextMesh && null != childEffect.mesh) {
								_meshFilter = container.GetComponent<MeshFilter>();
								if(null != _meshFilter) _meshFilter.mesh = childEffect.mesh;

							}
						}
					}
				}
			}

		}
	}
	[System.Serializable]
	public class ButtonEffectBase {
		readonly bool _isDisable;

		public bool isEffect;
		public GameObject child;
		public Mesh mesh;
		public Texture texture;
		public Sprite sprite;
		public Color color;
#if EX2D
		public exGUIBorder guiBorder;
		public exBitmapFont bitmapFont;
		public Color fontBotttomColor;
#endif

		public ButtonEffectBase() {
			isEffect = false;
			color = new Color(1, 1, 1, 1);
#if EX2D
			fontBotttomColor = color;
#endif
		}
		public ButtonEffectBase(Color mainColor) {
			_isDisable = false;
			color = mainColor;
#if EX2D
			fontBotttomColor = color;
#endif
		}
	}
}