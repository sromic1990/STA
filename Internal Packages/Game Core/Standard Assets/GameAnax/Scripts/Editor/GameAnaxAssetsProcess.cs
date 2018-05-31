//
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source:	N/A
// Last Modified: 	Ranpariya Ankur
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

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameAnaxAssetsPreProcess : AssetPostprocessor {
	float globalScaleModifier = 1f;
	void OnPostprocessModel() {
		ModelImporter importer = assetImporter as ModelImporter;
		importer.globalScale = globalScaleModifier;
		importer.importMaterials = false;
		importer.animationType = ModelImporterAnimationType.None;
		importer.materialSearch = ModelImporterMaterialSearch.Everywhere;
	}
	/*
	void OnPostprocessTexture() {
		TextureImporter textureImporter = (TextureImporter)assetImporter;

		textureImporter.convertToNormalmap = false;
		textureImporter.mipmapEnabled = false;
		textureImporter.textureType = TextureImporterType.Sprite;
		textureImporter.spriteImportMode = SpriteImportMode.Single;
		textureImporter.spritePixelsPerUnit = 100f;
		textureImporter.spritePivot = new Vector2(0.5f, 0.5f);
		textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
		textureImporter.crunchedCompression = false;

		textureImporter.maxTextureSize = 2048;
		if(assetPath.Contains("_bump") || assetPath.Contains("_normal")) {
			textureImporter.convertToNormalmap = true;
			SetAdvanceTextureOption(ref textureImporter);

		}
		if(assetPath.Contains("_cube")) {
			textureImporter.generateCubemap = TextureImporterGenerateCubemap.AutoCubemap;
			SetAdvanceTextureOption(ref textureImporter);
		}
	}
	void SetAdvanceTextureOption(ref TextureImporter textureImporter) {
		textureImporter.textureType = TextureImporterType.Default;
		textureImporter.wrapMode = TextureWrapMode.Repeat;
		textureImporter.filterMode = FilterMode.Bilinear;
		textureImporter.anisoLevel = 7;
	}
	*/
}
