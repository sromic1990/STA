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


namespace GameAnax.Core.UI {
	public class UILayoutManual : MonoBehaviour {
		public bool isCamBase = true;
		public bool isEnable = true;
		public Camera guiCamera;
		public Vector2 editiorScreenSize;
		public Vector2[] escapeRatio;
		public List<UIObject> elements;

		float _editorScreenRatio, _currentScreenRatio;
		float _editorWidth, _currentWidth;
		AxisToSet _currLoc;

		//Vector3 camCenter = Vector3.zero;
		float prsFactorX, prsFactorY, prsFactorZ;
		void Awake() {
			_editorScreenRatio = (float)System.Math.Round((editiorScreenSize.x / editiorScreenSize.y), 2);
			_currentScreenRatio = (float)System.Math.Round((float)Screen.width / (float)Screen.height, 2);

			_editorWidth = guiCamera.orthographicSize * _editorScreenRatio;
			_currentWidth = guiCamera.orthographicSize * _currentScreenRatio;
		}

		// Use this for initialization
		void Start() {
			if(!isEnable) {
				return;
			}
			for(int i = 0; i < escapeRatio.Length; i++) {
				float t = (float)System.Math.Round((escapeRatio[i].x / escapeRatio[i].y), 2);
				if(t == _currentScreenRatio) {
					return;
				}
			}
			if(_currentScreenRatio != _editorScreenRatio) {
				AutoUISetting();
			}
		}
		void AutoUISetting() {
			foreach(UIObject t in elements) {
				if(null == t.objectToSet) {
					continue;
				}

				switch(t.option) {
				case UISettingOption.Position:
					if(isCamBase) {
						_currLoc = t.axis;
						float x1 = (_currentWidth - _editorWidth);
						Vector3 pos = t.objectToSet.transform.localPosition;
						if(_currLoc == AxisToSet.X || _currLoc == AxisToSet.XY || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.XZ) {
							if(t.mode == UISettingMode.Addition) {
								pos.x += (x1 * t.multiplier);
							} else if(t.mode == UISettingMode.Subtraction) {
								pos.x -= (x1 * t.multiplier);
							}
						}
						if(_currLoc == AxisToSet.Y || _currLoc == AxisToSet.XY || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.YZ) {
							if(t.mode == UISettingMode.Addition) {
								pos.y += (x1 * t.multiplier);
							} else if(t.mode == UISettingMode.Subtraction) {
								pos.y -= (x1 * t.multiplier);
							}
						}
						if(_currLoc == AxisToSet.Z || _currLoc == AxisToSet.XZ || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.YZ) {
							if(t.mode == UISettingMode.Addition) {
								pos.z += (x1 * t.multiplier);
							} else if(t.mode == UISettingMode.Subtraction) {
								pos.z -= (x1 * t.multiplier);
							}
						}
						t.objectToSet.transform.localPosition = pos;
					} else {
						prsFactorX = (float)System.Math.Round((_currentScreenRatio * (t.objectToSet.transform.position.x)) / _editorScreenRatio, 6);
						prsFactorY = (float)System.Math.Round((_currentScreenRatio * (t.objectToSet.transform.position.y)) / _editorScreenRatio, 6);
						prsFactorZ = (float)System.Math.Round((_currentScreenRatio * (t.objectToSet.transform.position.z)) / _editorScreenRatio, 6);

						prsFactorX = prsFactorX - t.objectToSet.transform.position.x;
						prsFactorY = prsFactorY - t.objectToSet.transform.position.y;
						prsFactorZ = prsFactorZ - t.objectToSet.transform.position.z;
						if(t.mode == UISettingMode.Addition) {
							prsFactorX = t.objectToSet.transform.position.x + (prsFactorX * t.multiplier);
							prsFactorY = t.objectToSet.transform.position.y + (prsFactorY * t.multiplier);
							prsFactorZ = t.objectToSet.transform.position.z + (prsFactorZ * t.multiplier);
						} else if(t.mode == UISettingMode.Subtraction) {
							prsFactorX = t.objectToSet.transform.position.x - (prsFactorX * t.multiplier);
							prsFactorY = t.objectToSet.transform.position.y - (prsFactorY * t.multiplier);
							prsFactorZ = t.objectToSet.transform.position.z - (prsFactorZ * t.multiplier);
						}
						_currLoc = t.axis;
						if(_currLoc == AxisToSet.X || _currLoc == AxisToSet.XY || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.XZ) {
							t.objectToSet.transform.position = new Vector3(prsFactorX, t.objectToSet.transform.position.y,
								t.objectToSet.transform.position.z);
						}
						if(_currLoc == AxisToSet.Y || _currLoc == AxisToSet.XY || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.YZ) {
							t.objectToSet.transform.position = new Vector3(t.objectToSet.transform.position.x, prsFactorY,
								t.objectToSet.transform.position.z);
						}
						if(_currLoc == AxisToSet.Z || _currLoc == AxisToSet.XZ || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.YZ) {
							t.objectToSet.transform.position = new Vector3(t.objectToSet.transform.position.x,
								t.objectToSet.transform.position.y, prsFactorZ);
						}
					}
					break;

				case UISettingOption.Scale:
					prsFactorX = (float)System.Math.Round((_currentScreenRatio * t.objectToSet.transform.localScale.x) / _editorScreenRatio,
						6);
					prsFactorY = (float)System.Math.Round((_currentScreenRatio * t.objectToSet.transform.localScale.y) / _editorScreenRatio,
						6);
					prsFactorZ = (float)System.Math.Round((_currentScreenRatio * t.objectToSet.transform.localScale.z) / _editorScreenRatio,
						6);

					prsFactorX = prsFactorX - t.objectToSet.transform.localScale.x;
					prsFactorY = prsFactorY - t.objectToSet.transform.localScale.y;
					prsFactorZ = prsFactorZ - t.objectToSet.transform.localScale.z;
					if(t.mode == UISettingMode.Addition) {
						prsFactorX = t.objectToSet.transform.localScale.x + (prsFactorX * t.multiplier);
						prsFactorY = t.objectToSet.transform.localScale.y + (prsFactorY * t.multiplier);
						prsFactorZ = t.objectToSet.transform.localScale.z + (prsFactorZ * t.multiplier);
					} else if(t.mode == UISettingMode.Subtraction) {
						prsFactorX = t.objectToSet.transform.localScale.x - (prsFactorX * t.multiplier);
						prsFactorY = t.objectToSet.transform.localScale.y - (prsFactorY * t.multiplier);
						prsFactorZ = t.objectToSet.transform.localScale.z - (prsFactorZ * t.multiplier);
					}
					_currLoc = t.axis;
					if(_currLoc == AxisToSet.X || _currLoc == AxisToSet.XY || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.XZ) {
						t.objectToSet.transform.localScale = new Vector3(prsFactorX, t.objectToSet.transform.localScale.y,
							t.objectToSet.transform.localScale.z);
					}
					if(_currLoc == AxisToSet.Y || _currLoc == AxisToSet.XY || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.YZ) {
						t.objectToSet.transform.localScale = new Vector3(t.objectToSet.transform.localScale.x, prsFactorY,
							t.objectToSet.transform.localScale.z);
					}
					if(_currLoc == AxisToSet.Z || _currLoc == AxisToSet.XZ || _currLoc == AxisToSet.XYZ || _currLoc == AxisToSet.YZ) {
						t.objectToSet.transform.localScale = new Vector3(t.objectToSet.transform.localScale.x,
							t.objectToSet.transform.localScale.y, prsFactorZ);
					}
					break;

				case UISettingOption.None:
					break;
				default:
					break;
				}
			}
		}
	}
}