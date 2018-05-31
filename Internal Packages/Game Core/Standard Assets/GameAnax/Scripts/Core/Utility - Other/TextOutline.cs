//
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	N/A
// Last Modified: 	Ranpariya Ankur
// Contributed By:	N/A
// Curtosey By:		Amit Kapadi
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

using GameAnax.Core.Attributes;
using GameAnax.Core.Extension;


namespace GameAnax.Core.Utility {

	[RequireComponent(typeof(TextMesh))]
	public class TextOutline : MonoBehaviour {
		public float _pixelSize = 1;
		public Color _outlineColor = Color.black;
		public bool _isHide;

		[HideInInspector]
		[SerializeField]
		TextMesh _textMesh;
		[HideInInspector]
		[SerializeField]
		MeshRenderer _meshRenderer;

		[HideInInspector]
		[SerializeField]
		GameObject _outlines;
		[HideInInspector]
		[SerializeField]
		List<MeshRenderer> _otherMesh;
		[HideInInspector]
		[SerializeField]
		List<TextMesh> _otherTM;

		void Awake() {
			GetMyComponent();
		}
		void Start() {
			SetOutline();
		}

		void GetMyComponent() {
			_otherMesh = new List<MeshRenderer>();
			_otherTM = new List<TextMesh>();

			_textMesh = GetComponent<TextMesh>();
			_meshRenderer = GetComponent<MeshRenderer>();

		}
		public void SetOutline() {
			if(null == _outlines) {
				_outlines = new GameObject("Outlines");
			}
			if(_isHide)
				_outlines.hideFlags = HideFlags.HideInHierarchy;
			else
				_outlines.hideFlags = HideFlags.None;

			_outlines.transform.parent = transform;
			_outlines.transform.localPosition = Vector3.zero;
			_outlines.transform.localScale = new Vector3(1, 1, 1);
			_outlines.transform.localEulerAngles = Vector3.zero;

			GameObject outline = null;
			MeshRenderer otherMeshRenderer = null;
			TextMesh other = null;
			string newName;
			for(int i = 0; i < 8; i++) {
				newName = "Outline_" + i.ToString().PadLeft(2, '0');

				if(_otherTM.Count <= i || null == _otherTM[i]) {
					outline = _outlines.FindChildGameObject(newName);
					if(null == outline) outline = new GameObject(newName);
					outline.layer = gameObject.layer;

					other = outline.GetComponent<TextMesh>();
					if(null == other) other = outline.AddComponent<TextMesh>();
					otherMeshRenderer = outline.GetComponent<MeshRenderer>();
					if(!_otherTM.Contains(other)) _otherTM.Add(other);
					if(!_otherMesh.Contains(otherMeshRenderer)) _otherMesh.Add(otherMeshRenderer);
				}
				if(_isHide)
					outline.hideFlags = HideFlags.HideInHierarchy;
				else
					outline.hideFlags = HideFlags.None;


				outline.transform.parent = _outlines.transform;
				outline.transform.localScale = new Vector3(1, 1, 1);
				outline.transform.localEulerAngles = Vector3.zero;

				otherMeshRenderer.sharedMaterial = _meshRenderer.sharedMaterial;
				otherMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				otherMeshRenderer.receiveShadows = false;
				otherMeshRenderer.sortingLayerID = _meshRenderer.sortingLayerID;
				otherMeshRenderer.sortingLayerName = _meshRenderer.sortingLayerName;

				Vector3 pixelOffset = GetOffset(i) * (_pixelSize / 100f);
				Vector3 worldPoint;

				other.transform.localPosition = Vector3.zero;
				other.transform.localPosition += (pixelOffset);
				worldPoint = other.transform.localPosition;
				worldPoint.z = 0.25f;
				other.transform.localPosition = worldPoint;
			}
		}

		void LateUpdate() {
			_outlineColor.a = _textMesh.color.a * _textMesh.color.a;
			for(int i = 0; i < _otherTM.Count; i++) {
				_otherTM[i].color = _outlineColor;
				_otherTM[i].text = _textMesh.text;
				_otherTM[i].alignment = _textMesh.alignment;
				_otherTM[i].anchor = _textMesh.anchor;
				_otherTM[i].characterSize = _textMesh.characterSize;
				_otherTM[i].font = _textMesh.font;
				_otherTM[i].fontSize = _textMesh.fontSize;
				_otherTM[i].fontStyle = _textMesh.fontStyle;
				_otherTM[i].richText = _textMesh.richText;
				_otherTM[i].tabSize = _textMesh.tabSize;
				_otherTM[i].lineSpacing = _textMesh.lineSpacing;
				_otherTM[i].offsetZ = _textMesh.offsetZ;

				_otherMesh[i].sortingLayerID = _meshRenderer.sortingLayerID;
				_otherMesh[i].sortingLayerName = _meshRenderer.sortingLayerName;
			}
		}

		Vector3 GetOffset(int i) {
			switch(i % 8) {
			case 0:
				return new Vector3(0, 1, 0);
			case 1:
				return new Vector3(1, 1, 0);
			case 2:
				return new Vector3(1, 0, 0);
			case 3:
				return new Vector3(1, -1, 0);
			case 4:
				return new Vector3(0, -1, 0);
			case 5:
				return new Vector3(-1, -1, 0);
			case 6:
				return new Vector3(-1, 0, 0);
			case 7:
				return new Vector3(-1, 1, 0);
			default:
				return Vector3.zero;
			}
		}
#if UNITY_EDITOR
		[ButtonInspector("Create Outline")]
		public void CreateOutLine() {
			GetMyComponent();
			SetOutline();
			LateUpdate();
		}
#endif

	}
}