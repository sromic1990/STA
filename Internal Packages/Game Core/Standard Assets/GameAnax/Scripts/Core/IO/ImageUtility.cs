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
using System.Collections;

using UnityEngine;

using GameAnax.Core.Extension;
using GameAnax.Core.Net;
using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;


namespace GameAnax.Core.IO {
	[PersistentSignleton(false, true)]
	public class ImageUtility : SingletonAuto<ImageUtility> {
		public Texture2D LoadTextrure(string fileName) {
			return LoadTextrure(string.Empty, fileName);
		}
		public Texture2D LoadTextrure(string path, string fileName) {
			Texture2D tex = new Texture2D(0, 0);
			string dataPath = path + fileName;
			byte[] imageData;
			try {
				if(System.IO.File.Exists(dataPath)) {
					imageData = System.IO.File.ReadAllBytes(dataPath);
					tex.LoadImage(imageData);
					tex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
					tex.LoadImage(imageData);
				} else {
					tex = null;
				}
			} catch(System.Exception ex) {
				Debug.LogError(ex.Message);
				tex = null;
			}
			return tex;
		}

		public Sprite GetSprite(Texture2D texture) {
			return GetSprite(texture, new Vector2(0.5f, 0.5f), new Vector4(0, 0, 0, 0));

		}
		public Sprite GetSprite(Texture2D texture, Vector2 pivot) {
			return GetSprite(texture, pivot, new Vector4(0, 0, 0, 0));
		}
		public Sprite GetSprite(Texture2D texture, Vector4 border) {
			return GetSprite(texture, new Vector2(0.5f, 0.5f), border);
		}
		public Sprite GetSprite(Texture2D texture, Vector2 pivot, Vector4 border) {
			Sprite s;
			if(null != texture) {
				s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
					pivot, 100, 0, SpriteMeshType.FullRect, border);
			} else {
				s = null;
			}
			return s;
		}

		public Sprite GetSprite(CacheFileCategories category, string fileName) {
			return GetSprite(category, fileName, false);
		}
		public Sprite GetSprite(string fileName) {
			return GetSprite(CacheFileCategories.Image, fileName, true);
		}

		public Sprite GetSprite(CacheFileCategories category, string fileName, bool isFixedPath) {
			Sprite s;
			Texture2D tex;

			string path = isFixedPath ? "" : File.GetCachePath(category);
			return GetSprite(fileName, true);
		}
		public Sprite GetSprite(string fileName, bool isFixedPath) {
			Sprite s;
			Texture2D tex;

			string path = isFixedPath ? "" : File.DataPath();
			tex = LoadTextrure(path, fileName);
			if(null != tex) {
				s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
					new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect,
					new Vector4(0, 0, 0, 0));
			} else {
				s = null;
			}
			return s;
		}

		public IEnumerator SetIconOrImage(UnityEngine.UI.Image setToImage, string url, CacheFileCategories category) {
			yield return StartCoroutine(SetIconOrImage(setToImage, url, string.Empty, string.Empty, category, null));
		}
		public IEnumerator SetIconOrImage(UnityEngine.UI.Image setToImage, string url, string savePath, CacheFileCategories category) {
			yield return StartCoroutine(SetIconOrImage(setToImage, url, savePath, string.Empty, category, null));
		}
		public IEnumerator SetIconOrImage(UnityEngine.UI.Image setToImage, string imageKeyOrURL, string savePath, string fileName, CacheFileCategories category,
			Action<bool, ContentUsedFrom> onDone) {
			ContentUsedFrom kind = ContentUsedFrom.None;
			bool isDisplay = false;
			Sprite tempSprite = null;
			if(null != setToImage) {
				if(string.IsNullOrEmpty(imageKeyOrURL)) {
					setToImage.sprite = null;
					setToImage.color = setToImage.color.SetChannelA(0f);
					setToImage.gameObject.SetActive(false);
					kind = ContentUsedFrom.None;
				} else {
					if(imageKeyOrURL.IsURL()) {
						string saveFileName = fileName.IsNulOrEmpty() ? File.GetFileNameFromURL(imageKeyOrURL) : fileName;
						string path = !category.Equals(CacheFileCategories.None) ? File.GetCachePath(category) : savePath;
						bool isFixedPath = category.Equals(CacheFileCategories.None);

						bool spriteAvailable = System.IO.File.Exists(savePath + saveFileName);
						if(spriteAvailable) {
							kind = ContentUsedFrom.DownloadCache;
							tempSprite = isFixedPath ? GetSprite(category, savePath + saveFileName, isFixedPath) : GetSprite(category, saveFileName);
							isDisplay = null != tempSprite;
							if(onDone != null) { onDone(isDisplay, kind); }
						}
						//if(null == tempSprite) {
						if(null != DownloaderUtility.Me) yield return StartCoroutine(DownloaderUtility.Me.DownloadImage(imageKeyOrURL, path, saveFileName, true, null));
						spriteAvailable = System.IO.File.Exists(savePath + saveFileName);
						if(spriteAvailable) {
							tempSprite = isFixedPath ? GetSprite(category, savePath + saveFileName, isFixedPath) : GetSprite(category, saveFileName);
							kind = ContentUsedFrom.DownloadFromServer;
						} else
							tempSprite = null;
						//}
					} else {
						kind = ContentUsedFrom.UnityCache;
						tempSprite = SubsetManager.Me.GetIcon(imageKeyOrURL);
					}

					//setToImage.gameObject.SetActive((tempSprite != null));
					//MyDebug.Log("Sprite is available? => {0}", (tempSprite != null));
					if(tempSprite != null) {
						setToImage.sprite = tempSprite;
						setToImage.color = setToImage.color.SetChannelA(1f);
					}
					//else {
					//	setToImage.sprite = null;
					//	setToImage.color = setToImage.color.SetChannelA(0f);
					//}
				}
			}
			isDisplay = null != tempSprite;
			if(onDone != null) { onDone(isDisplay, kind); }
		}

	}
	/// <summary>
	/// Content used from.
	/// </summary>
	public enum ContentUsedFrom {
		None,
		UnityCache,
		UnityResource,
		DownloadCache,
		DownloadFromServer,
		UnityAssetsBundle,
		NavtiveResources,
	}
}
