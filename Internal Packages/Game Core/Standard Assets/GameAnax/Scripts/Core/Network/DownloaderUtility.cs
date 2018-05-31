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
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;

using GameAnax.Core.Singleton;
using GameAnax.Core.IO;
using GameAnax.Core.Utility;


namespace GameAnax.Core.Net {
	[PersistentSignleton(true, true)]
	public class DownloaderUtility : SingletonAuto<DownloaderUtility> {
		public bool AcceptAllCertifications(object sender, X509Certificate certification, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			return true;
		}

		/// <summary>
		/// Downloads the asset bundle.
		/// </summary>
		/// <returns>The asset bundle.</returns>
		/// <param name="url">URL.</param>
		/// <param name="path">Path.</param>
		/// <param name="saveFileName">Save file name.</param>
		/// <param name="keep">If set to <c>true</c> keep.</param>
		/// <param name="onDone">On done.</param>
		public IEnumerator DownloadAssetBundle(string url, string path, string saveFileName, bool keep, Action<string, AssetBundle, Dictionary<string, string>> onDone) {
			WWW www = new WWW(url);
			yield return www;
			if(!string.IsNullOrEmpty(www.error)) {
				MyDebug.Warning(www.error);
				if(onDone != null) { onDone(www.error, null, www.responseHeaders); }
			} else {
				if(keep) {
					File.WriteFile(path, saveFileName, www.bytes);
				}
				if(onDone != null) { onDone(string.Empty, www.assetBundle, www.responseHeaders); }
			}
		}

		/// <summary>
		/// Downloads the voice clip.
		/// </summary>
		/// <returns>The voice clip.</returns>
		/// <param name="url">URL.</param>
		/// <param name="saveFileName">Save file name.</param>
		/// <param name="keep">If set to <c>true</c> keep.</param>
		public IEnumerator DownloadVoiceClip(string url, string path, string saveFileName, bool keep, Action<string, AudioClip, Dictionary<string, string>> onDone) {
			WWW www = new WWW(url);
			yield return www;
			if(!string.IsNullOrEmpty(www.error)) {
				MyDebug.Warning(www.error);
				if(onDone != null) { onDone(www.error, null, www.responseHeaders); }
			} else {
				if(keep) {
					File.WriteFile(path, saveFileName, www.bytes);
				}
				if(onDone != null) { onDone(string.Empty, www.GetAudioClip(), www.responseHeaders); }
			}
		}

		/// <summary>
		/// Downloads the Text Data
		/// </summary>
		/// <returns>The text data.</returns>
		/// <param name="url">URL.</param>
		/// <param name="saveFileName">Save file name.</param>
		/// <param name="keep">If set to <c>true</c> keep.</param>
		public IEnumerator DownloadText(string url, string path, string saveFileName, bool keep, Action<string, string, Dictionary<string, string>> onDone) {
			WWW www = new WWW(url);
			yield return www;
			if(!string.IsNullOrEmpty(www.error)) {
				MyDebug.Warning(www.error);
				if(onDone != null) { onDone(www.error, null, www.responseHeaders); }
			} else {
				if(keep) {
					File.WriteFile(path, saveFileName, www.bytes);
				}
				if(onDone != null) { onDone(string.Empty, www.text, www.responseHeaders); }
			}
		}

		/// <summary>
		/// Downloads the image.
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="url">URL.</param>
		/// <param name="saveFileName">Save file name.</param>
		/// <param name="keep">If set to <c>true</c> keep.</param>
		public IEnumerator DownloadImage(string url, string path, string saveFileName, bool keep, Action<string, Texture2D, Dictionary<string, string>> onDone) {
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
			WWW www = new WWW(url);
			yield return www;
			if(!string.IsNullOrEmpty(www.error)) {
				MyDebug.Warning(www.error);
				if(onDone != null) { onDone(www.error, null, www.responseHeaders); }
			} else {
				if(keep) {
					File.WriteFile(path, saveFileName, www.bytes);
				}
				if(onDone != null) { onDone(string.Empty, www.texture, www.responseHeaders); }
			}
		}
#if UNITY_STANDALONE
		public IEnumerator DownloadMovieTexture(string url, string path, string saveFileName, bool keep, Action<string, MovieTexture, Dictionary<string, string>> onDone) {
			System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
			WWW www = new WWW(url);
			yield return www;
			if(!string.IsNullOrEmpty(www.error)) {
				MyDebug.Warning(www.error);
				if(onDone != null) { onDone(www.error, null, www.responseHeaders); }
			} else {
				if(keep) {
					File.WriteFile(path, saveFileName, www.bytes);
				}
				if(onDone != null) { onDone(string.Empty, www.GetMovieTexture(), www.responseHeaders); }
			}
		}
#endif
	}
}