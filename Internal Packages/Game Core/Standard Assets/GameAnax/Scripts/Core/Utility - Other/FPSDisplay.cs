//
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	http://wiki.unity3d.com/index.php?title=FramesPerSecond
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

using System.Collections;

using UnityEngine;
using UnityEngine.UI;


namespace GameAnax.Core.Utility {
	[AddComponentMenu("Utility/FPSDisplay")]
	public class FPSDisplay : MonoBehaviour {
		// Script Copied from Unity Wiki (http://wiki.unity3d.com/index.php?title=FramesPerSecond)
		// Attach this to any object to make a frames/second indicator.
		//
		// It calculates frames/second over each updateInterval,
		// so the display does not keep changing wildly.
		//
		// It is also fairly accurate at very low FPS counts (<10).
		// We do this not by simply counting frames per interval, but
		// by accumulating FPS for each frame. This way we end up with
		// corstartRect overall FPS even if the interval renders something like
		// 5.5 frames.
		[SerializeField]
		private bool updateColor = true;     // Do you want the color to change if the FPS gets low
		[SerializeField]
		private bool allowDrag = true;       // Do you want to allow the dragging of the FPS window
		[SerializeField]
		private float frequency = 0.25F;     // The update frequency of the fps
		[SerializeField]
		private int nbDecimal = 1;           // How many decimal do you want to display
		[SerializeField]
		private bool isGUIFPS;
		[SerializeField]
		private Text uiText;
		[SerializeField]
		private TextMesh tdText;
		[SerializeField]
		private GUIText guiText;

		private Rect _startRect;                    // The rect the window is initially displayed at.
		void Awake() {
			if(null != guiText) guiText.text = string.Empty;
			if(null != tdText) tdText.text = string.Empty;
			if(null != uiText) uiText.text = string.Empty;
			_startRect = new Rect((Screen.width - 75) / 2, 10, 75, 50);
		}
#if MYDEBUG


		void Start() {
			StartCoroutine(FPS());
		}
		void Update() {
			_accum += (1f / Time.unscaledDeltaTime);
			++_frames;
		}

		private float _accum = 0f;                  // FPS accumulated over the interval
		private int _frames = 0;                    // Frames drawn over the interval
		private Color _color = Color.white;         // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
		private string _sFPS = "";                  // The fps formatted into a string.
		private GUIStyle style;                     // The style the text will be displayed at, based en defaultSkin.label.

		IEnumerator FPS() {
			// Infinite loop executed every "frenquency" secondes.
			while(true) {
				// Update the FPS
				float fps = _accum / _frames;
				_sFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
				//Update the color
				_color = (fps >= 30) ? Color.green : ((fps > 24) ? Color.yellow : ((fps > 15) ? Color.magenta : Color.red));
				_accum = 0.0F;
				_frames = 0;
				yield return new WaitForSeconds(frequency);
				if(null != guiText) guiText.text = _sFPS + " FPS";
				if(null != tdText) tdText.text = _sFPS + " FPS";
				if(null != uiText) uiText.text = _sFPS + " FPS";
			}
		}

		void OnGUI() {
			if(!isGUIFPS) { return; }
			// Copy the default label skin, change the color and the alignement
			if(null == style) {
				style = new GUIStyle(GUI.skin.label);
				style.normal.textColor = Color.white;
				style.alignment = TextAnchor.MiddleCenter;
			}

			GUI.color = updateColor ? _color : Color.white;
			_startRect = GUI.Window(0, _startRect, DoMyWindow, "");
		}

		void DoMyWindow(int windowID) {
			GUI.Label(new Rect(0, 0, _startRect.width, _startRect.height), _sFPS + " FPS", style);
			if(allowDrag)
				GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
		}
#endif
	}
}