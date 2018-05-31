//
// Coder:			Sharatbabu K. Achary  {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source:	N/A
// Last Modified: 	Sharatbabu K. Achary on 12th Nov 2014
// Contributed By:	
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
using System.Linq;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GameAnaxFrameEditorWindow : EditorWindow {
#if UNITY_EDITOR

	List<GameObject> selectedGameObjects = new List<GameObject>();
	Vector2 _winScrollPosition;

	[MenuItem("Tools/GameAnax/Show Frame Editor Window")]
	private static void ShowFrameEditorWindow() {
		EditorWindow.GetWindow<GameAnaxFrameEditorWindow>(false, "GameAnax Frame Editor");
	}

	void OnSelectionChange() {
		selectedGameObjects = Selection.gameObjects.Where(go => go.GetComponent<MeshRenderer>() != null).ToList();
		Repaint();
	}

	void OnGUI() {
		_winScrollPosition = EditorGUILayout.BeginScrollView(_winScrollPosition);
		{
			EditorGUILayout.BeginVertical();
			{

				foreach(GameObject go in selectedGameObjects) {
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField(go.name);
					}
					EditorGUILayout.EndHorizontal();
				}

				if(GUILayout.Button("Make Pixel Perfect")) {
					foreach(GameObject go in selectedGameObjects) {
						SetScaleBasedOnTexture(go);
					}
					SceneView.RepaintAll();
				}

			}
			EditorGUILayout.EndVertical();

		}
		EditorGUILayout.EndScrollView();
	}

	void SetScaleBasedOnTexture(GameObject go) {
		Texture tex = go.GetComponent<Renderer>().sharedMaterial.mainTexture;
		go.transform.localScale = new Vector3(tex.width / 100f, tex.height / 100f, 1f);
	}
#endif
}
