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


namespace GameAnax.Core.IO {
	public static class File {
		public static string audioPath {
			get { return File.DataPath() + "/Audio/"; }
		}
		public static string imagePath {
			get { return File.DataPath() + "/Images/"; }
		}
		public static string iconPath {
			get { return File.DataPath() + "/Icons/"; }
		}
		public static string videoPath {
			get { return File.DataPath() + "/Video/"; }
		}
		public static string thumbnailPath {
			get { return File.DataPath() + "/Thumbnail/"; }
		}

		/// <summary>
		/// Gets the cache path.
		/// </summary>
		/// <returns>The cache path.</returns>
		/// <param name="category">Category.</param>
		public static string GetCachePath(CacheFileCategories category) {
			string path = "";
			switch(category) {
			case CacheFileCategories.Icon:
				path = iconPath;
				break;
			case CacheFileCategories.Image:
				path = imagePath;
				break;
			case CacheFileCategories.Thumbnail:
				path = thumbnailPath;
				break;
			case CacheFileCategories.Voice:
				path = audioPath;
				break;
			case CacheFileCategories.Video:
				path = videoPath;
				break;
			}
			return path;
		}
		/// <summary>
		/// Gets the file name from URL.
		/// </summary>
		/// <returns>The file name from URL.</returns>
		/// <param name="url">URL.</param>
		public static string GetFileNameFromURL(this string url) {
			string file = "";
			string result;
			string[] urlparts = url.Split('/');
			if(urlparts.Length > 0) {
				file = urlparts[urlparts.Length - 1];
			}
			result = file;
			return result;
		}
		public static string ApplicationPath() {
			string docsPath;
			docsPath = Application.persistentDataPath;
			docsPath = ReplacePath(docsPath);
			return docsPath;
		}
		public static string DataPath() {
			string docsPath;
			docsPath = Application.persistentDataPath;
			docsPath = ReplacePath(docsPath);
			return docsPath;
		}
		public static string GetOSFullVersion() {
			string Str_OS = SystemInfo.operatingSystem;
			string[] versions;
			versions = Str_OS.Split(' ');
			return versions[versions.Length - 1];
		}
		public static float GetOSVersion() {
			string v = GetOSFullVersion();
			string[] vs = v.Split('.');
			string fVs = "";
			float finalVersion = 0;
			if(vs.Length > 0) {
				fVs += vs[0];
			}
			if(vs.Length > 1) {
				fVs += "." + vs[1];
			}
			float.TryParse(fVs, out finalVersion);
			return finalVersion;
		}

		static string ReplacePath(string path) {
#if UNITY_EDITOR
			switch(Application.platform) {
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.WindowsEditor:
				path = path.TrimEnd('/') + "/";
				break;
				//path = path.Replace(@"\Data\", @"\Documents\");
				//path = path.Replace(@"\\", @"\");
				//break;
			}
#elif UNITY_STANDALONE_OSX
#elif UNITY_STANDALONE_WIN
#elif UNITY_STANDALONE_LINUX
#elif UNITY_IOS
			float osVersion = GetOSVersion();
			path = path.TrimEnd('/') + "/";
			
			if(osVersion < 8f){
				path = path.Replace("/Data/", "/Documents/");
			}
			
			if(!path.EndsWith("/Documents/")) {
				path += "/Documents/";
			}
#elif UNITY_ANDROID
			path = path.TrimEnd('/') + "/";
			path = path.Replace("/files/", "/Documents/");
			if(!path.EndsWith("/Documents/")) {
				path += "/Documents/";
			}
#endif
			return path;
		}

		public static bool FileExists(string path, string fileName) {
			string dataPath = path + fileName;
			return System.IO.File.Exists(dataPath);
		}
		public static bool FileExists(string fileName) {
			return FileExists(DataPath(), fileName);

		}

		public static string ReadFile(string fileName) {
			return ReadFile(DataPath(), fileName);
		}
		public static string ReadFile(string path, string fileName) {
			System.IO.FileInfo theSourceFile = null;
			System.IO.StreamReader reader = null;
			string fileData = string.Empty;
			string dataPath = path + fileName;
			try {
				theSourceFile = new System.IO.FileInfo(dataPath);
				if(null != theSourceFile && theSourceFile.Exists) {
					reader = theSourceFile.OpenText();
				}
				if(null == reader) {
					Debug.Log("File::ReadFile => " + dataPath + " was not found or not readable");
					fileData = null;
				} else {
					fileData = reader.ReadToEnd();
				}
			} catch(System.Exception ex) {
				Debug.LogError(ex.Message);
			} finally {
				reader.Close();
				theSourceFile = null;
			}
			return fileData;
		}

		public static bool WriteFile(string fileName, string data) {
			return WriteFile(DataPath(), fileName, data, FileWriteMode.CreateOverwirte);
		}
		public static bool WriteFile(string fileName, string data, FileWriteMode mode) {
			return WriteFile(DataPath(), fileName, data, mode);
		}
		public static bool WriteFile(string path, string fileName, string data) {
			return WriteFile(path, fileName, data, FileWriteMode.CreateOverwirte);
		}
		public static bool WriteFile(string path, string fileName, string data, FileWriteMode mode) {
			bool retValue = false;
			string dataPath = path;
			try {
				if(!System.IO.Directory.Exists(dataPath)) {
					System.IO.Directory.CreateDirectory(dataPath);
				}
				dataPath += fileName;
				switch(mode) {
				case FileWriteMode.Open:
					retValue = false;
					break;
				case FileWriteMode.CreateOverwirte:
					System.IO.File.WriteAllText(dataPath, data);
					break;
				case FileWriteMode.Append:
					System.IO.File.AppendAllText(dataPath, data);
					break;
				}
				retValue = true;
			} catch(System.Exception ex) {
				Debug.LogError("File Write Error\n" + ex.Message);
				retValue = false;
				Debug.LogError(ex.Message);
			}
			return retValue;
		}
		public static bool WriteFile(string path, string fileName, byte[] data) {
			bool retValue = false;
			string dataPath = path;
			try {
				if(!System.IO.Directory.Exists(dataPath)) {
					System.IO.Directory.CreateDirectory(dataPath);
				}
				dataPath += fileName;
				System.IO.File.WriteAllBytes(dataPath, data);
				retValue = true;
			} catch(System.Exception ex) {
				Debug.LogError("File Write Error\n" + ex.Message);
				retValue = false;
			}
			return retValue;
		}

		public static string ReadResourceFile(string fileName) {
			TextAsset fileData = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
			return fileData.text;
		}
	}

	/// <summary>
	/// File Write mode.
	/// </summary>
	public enum FileWriteMode {
		CreateOverwirte = 0,
		Append = 1,
		Open = 2
	}

	/// <summary>
	/// Cache file categories.
	/// </summary>
	public enum CacheFileCategories {
		None = 0,
		Image = 1,
		Icon = 2,
		Thumbnail = 3,
		Voice = 4,
		Video = 5,
		Banner = 6
	}
}