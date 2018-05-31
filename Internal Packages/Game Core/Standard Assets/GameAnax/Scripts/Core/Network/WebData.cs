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
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;

using GameAnax.Core.Extension;
using GameAnax.Core.Threader;
using GameAnax.Core.Utility;


namespace GameAnax.Core.Net {
	public class WebData {
		private JoinUnityMainThread _mainThread;
		private readonly Encoding _encoding = Encoding.Default;
		private readonly string _lineFeed = "\r\n";

		private List<Thread> myThreads = new List<Thread>();

		public WebData() {
			_mainThread = JoinUnityMainThread.Me;
		}
		public WebData(Encoding encoding) : this() {
			_encoding = encoding;
		}

		~WebData() {
			myThreads.ForEach(o => o.Abort());
			myThreads = null;
		}

		public bool AcceptAllCertifications(object sender, X509Certificate certification, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
			return true;
		}

		public void ExecuteURL(ExecuteURLParameters option) {
			string retValue = string.Empty; // Used to store the return value
			Thread thread = new Thread(() => {
				ExecuteURLThread(option);
			});
			myThreads.Add(thread);
			thread.Start();
		}

		/// <summary>
		/// Executes the URL Thread.
		/// </summary>
		/// <param name="option">Option.</param>
		private void ExecuteURLThread(ExecuteURLParameters option) {
			//MyDebug.Log("executing {0}", url);
			HttpWebResponse wRes = null;
			HttpWebRequest wReq;
			string rawData = string.Empty;

			Stream webResponseStream = null;
			StreamReader webResStreamReader = null;
			HTTPCallback callbacker = new HTTPCallback();
			Response sr = new Response();
			sr.error = null;
			try {
				callbacker._callback = option.callback;
				#region GET URL (No multopart and No Compression)
				if(!option.isMultipart || option.method.Equals(WebMethod.GET)) {
					if(null != option.postData && option.postData.Count > 0) {
						foreach(string s in option.postData.Keys) {
							rawData += s + "=" + WWW.EscapeURL(option.header[s].ToString()) + "&";
						}
						rawData = rawData.TrimEnd('&');
					}
				}
				if(option.method == WebMethod.GET && rawData.Length > 0) {
					if(!option.url.EndsWith("?", StringComparison.OrdinalIgnoreCase) && !option.url.Contains("?")) {
						option.url += "?";
					} else if(!option.url.EndsWith("&", StringComparison.OrdinalIgnoreCase)) {
						option.url += "&";
					}
					option.url += rawData;
					option.url = option.url.TrimEnd('&');
					option.url = option.url.TrimEnd('?');
				}
				//MyDebug.Log("executing {0}", url);
				#endregion

				#region Core part of Creating Request
				//MyDebug.Log("Createing Request for url: {0}", option.url);
				wReq = (HttpWebRequest)WebRequest.Create(option.url);
				wReq.ContentType = "text/plain";


				//MyDebug.Log("Request created");

				if(null != option.header && option.header.Count > 0) {
					//MyDebug.Log("Setting up Header data");
					foreach(string s in option.header.Keys) {
						wReq.Headers.Add(s, option.header[s].ToString());
					}
				}

				wReq.Credentials = CredentialCache.DefaultCredentials;
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);
				wReq.Timeout = option.timeOut;
				wReq.Method = option.method.ToString().ToUpper();
				#endregion

				#region POST or Multipart Past 
				//Stream stream;
				byte[] postData = null;
				//MyDebug.Log("Multipart: {0}, method: {1}", option.isMultipart, option.method);
				#region Non-multipart POST request
				if(!option.isMultipart && option.method.Equals(WebMethod.POST) && rawData.Length > 0) {
					MyDebug.Log("checking non multipart post data");
					postData = _encoding.GetBytes(rawData);
					wReq.ContentType = "application/x-www-form-urlencoded";
					wReq.ContentLength = postData.Length;
				}
				#endregion

				#region multipart POST Request
				if(option.isMultipart && option.method.Equals(WebMethod.POST)) {
					//MyDebug.Log("checking multipart post data");
					string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
					Stream fromStream = new MemoryStream();
					fromStream = CreateMultipartFormData(fromStream, option.postData, formDataBoundary);

					//Add the end of the request.  Start with a newline
					string footer = "--" + formDataBoundary + "--\r\n";
					fromStream.Write(_encoding.GetBytes(footer), 0, _encoding.GetByteCount(footer));

					fromStream.Position = 0;
					postData = new byte[fromStream.Length];
					fromStream.Read(postData, 0, postData.Length);
					fromStream.Close();

					wReq.Method = "POST";
					wReq.ContentType = "multipart/form-data; boundary=" + formDataBoundary;
					wReq.ContentLength = postData.Length;
				}
				#endregion
				if(wReq.Method == "POST") {
					switch(option.compMode) {
					case Compression.Normal:
						//MyDebug.Log("Writing normal stream for post data");
						using(var stream = wReq.GetRequestStream()) {
							stream.Write(postData, 0, postData.Length);
						}
						break;

					case Compression.GZip:
						//MyDebug.Log("Writing GZip stream for post data");
						wReq.Headers.Add(HttpRequestHeader.ContentEncoding, "gzip");
						wReq.Headers.Add("Accept-Encoding", "gzip,deflate");
						using(var stream = wReq.GetRequestStream()) {
							using(var zipStream = new GZipStream(stream, CompressionMode.Compress)) {
								zipStream.Write(postData, 0, postData.Length);
							}
						}
						break;

					case Compression.Deflate:
						//MyDebug.Log("Writing deflate stream for post data");
						wReq.Headers.Add(HttpRequestHeader.ContentEncoding, "deflate");
						wReq.Headers.Add("Accept-Encoding", "gzip,deflate");
						using(var stream = wReq.GetRequestStream()) {
							using(var zipStream = new DeflateStream(stream, CompressionMode.Compress)) {
								zipStream.Write(postData, 0, postData.Length);
							}
						}
						break;
					}
				}
				#endregion
				//MyDebug.Log("Getting Response");
				wRes = (HttpWebResponse)wReq.GetResponse();

				callbacker.responseHeader = wRes.Headers;
				//MyDebug.Log("Getting stream from Response: {0}", wRes.ContentEncoding.ToString());
				webResponseStream = wRes.GetResponseStream();
				if(wRes.ContentEncoding.ToLower().Contains("gzip")) {
					//MyDebug.Log("UnCompressing GZip Stream");
					webResponseStream = new GZipStream(webResponseStream, CompressionMode.Decompress);
				} else if(wRes.ContentEncoding.ToLower().Contains("deflate")) {
					//MyDebug.Log("UnCompressing Deflate Stream");
					webResponseStream = new DeflateStream(webResponseStream, CompressionMode.Decompress);
				}
				//MyDebug.Log("Getting reader from stream");
				webResStreamReader = new StreamReader(webResponseStream, Encoding.Default);
				//MyDebug.Log("Reading text from StreamReader");
				callbacker.webResData = webResStreamReader.ReadToEnd();
				//MyDebug.Log("data read end");
				//MyDebug.Log("TEST DATA: {0}", sr1);
				//wRes.Close();
			} catch(HttpListenerException hlex) {
				sr.status = false;
				sr.error = new Error();
				sr.error.data = hlex.Data;
				sr.error.type = "HttpListenerException";
				sr.error.errorCode = hlex.ErrorCode;
				sr.error.message = hlex.Message;
				sr.error.exceptionSource = hlex.Source;
				sr.error.helpLink = hlex.HelpLink;
				callbacker.webResData = JsonUtility.ToJson(sr);
			} catch(ProtocolViolationException pvex) {
				sr.status = false;
				sr.error.data = pvex.Data;
				sr.error.type = "ProtocolViolationException";
				sr.error.errorCode = 0;
				sr.error.message = pvex.Message;
				sr.error.exceptionSource = pvex.Source;
				sr.error.helpLink = pvex.HelpLink;
				callbacker.webResData = JsonUtility.ToJson(sr);
			} catch(WebException wex) {
				sr.status = false;
				sr.error = new Error();
				sr.error.data = wex.Data;
				sr.error.type = "WebException";
				sr.error.errorCode = 0;
				sr.error.message = wex.Message;
				sr.error.exceptionSource = wex.Source;
				sr.error.helpLink = wex.HelpLink;
				callbacker.webResData = JsonUtility.ToJson(sr);
			} catch(Exception ex) {
				sr.status = false;
				sr.error = new Error();
				sr.error.data = ex.Data;
				sr.error.type = "Exception";
				sr.error.errorCode = 0;
				sr.error.message = ex.Message;
				sr.error.exceptionSource = ex.Source;
				sr.error.helpLink = ex.HelpLink;
				callbacker.webResData = Prime31.Json.encode(sr);
			} finally {
				if(wRes != null) wRes.Close();
				if(webResponseStream != null) webResponseStream.Close();
				if(webResStreamReader != null) webResStreamReader.Close();
				wRes = null;
				webResponseStream = null;
				webResStreamReader = null;
			}

			//MyDebug.Log("Ready to ques");
			//MyDebug.Warning("TEST {0}", callbacker.webResData);
			_mainThread.Enqueue(callbacker.ExecuteCallback);
			Thread.CurrentThread.Abort();
		}


		/// <summary>
		/// Uploads the file.
		/// </summary>
		/// <param name="options">Options.</param>
		public void UploadFile(UploadFileParameters options) {
			string retValue = string.Empty; // Used to store the return value
			Thread thread = new Thread(() => {
				UploadFileInThread(options);
			});
			myThreads.Add(thread);
			thread.Start();
		}
		/// <summary>
		/// Uploads the file in thread.
		/// </summary>
		/// <param name="options">Options.</param>
		private void UploadFileInThread(UploadFileParameters options) {
			WebResponse wRes = null;
			HttpWebRequest wReq = null;
			Stream responseStream = null;
			StreamReader responseReader = null;
			HTTPCallback callbacker = new HTTPCallback();
			Response sr = new Response();
			sr.error = null;


			if(options.filesToUpload == null) options.filesToUpload = new List<UploadFileInfo>();
			foreach(UploadFileList ufl in options.uploadList) {
				string fileName = GameAnax.Core.IO.File.GetFileNameFromURL(ufl.filePath);
				int fileNameLength = fileName.Length;
				int x = ufl.filePath.LastIndexOf(fileName, StringComparison.InvariantCulture);

				string path = ufl.filePath.Left(x);
				Debug.Log(path);
				if(!File.Exists(ufl.filePath)) {
					Debug.LogWarning(string.Format("{0} is not available at location {1}", fileName, path));
					continue;
				}

				byte[] fileData = File.ReadAllBytes(ufl.filePath);
				options.filesToUpload.Add(new UploadFileInfo() { contentType = ufl.contentType, fileBytes = fileData, fileName = fileName, uniqueKey = ufl.key });
			}

			try {
				callbacker._callback = options.callback;
				string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

				wReq = (HttpWebRequest)WebRequest.Create(options.url);
				wReq.ContentType = "multipart/form-data; boundary=" + formDataBoundary;
				wReq.Method = "POST";
				wReq.CookieContainer = new CookieContainer();
				wReq.Credentials = CredentialCache.DefaultCredentials;
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertifications);

				if(null != options.header && options.header.Count > 0) {
					foreach(string headrKey in options.header.Keys) {
						wReq.Headers.Add(headrKey, options.header[headrKey].ToString());
					}
				}

				#region x
				Stream fromStream = new MemoryStream();
				fromStream = CreateMultipartFormData(fromStream, options.postData, formDataBoundary);
				fromStream = CreateMultiPartFileData(fromStream, options.filesToUpload, formDataBoundary);

				// Add the end of the request.  Start with a newline
				string footer = "--" + formDataBoundary + "--\r\n";
				fromStream.Write(_encoding.GetBytes(footer), 0, _encoding.GetByteCount(footer));

				fromStream.Position = 0;
				byte[] formData = new byte[fromStream.Length];
				fromStream.Read(formData, 0, formData.Length);
				fromStream.Close();

				wReq.ContentLength = formData.Length;
				using(var stream = wReq.GetRequestStream()) {
					stream.Write(formData, 0, formData.Length);
				}


				#endregion
				wRes = wReq.GetResponse();

				callbacker.responseHeader = wRes.Headers;
				responseStream = wRes.GetResponseStream();
				responseReader = new StreamReader(responseStream);
				callbacker.webResData = responseReader.ReadToEnd();
				responseReader.Close();

			} catch(HttpListenerException hlex) {
				sr.status = false;
				sr.error = new Error();

				sr.error.data = hlex.Data;
				sr.error.type = "HttpListenerException";
				sr.error.errorCode = hlex.ErrorCode;
				sr.error.message = hlex.Message;
				sr.error.exceptionSource = hlex.Source;
				sr.error.helpLink = hlex.HelpLink;
				callbacker.webResData = JsonUtility.ToJson(sr);
			} catch(ProtocolViolationException pvex) {
				sr.status = false;
				sr.error = new Error();

				sr.error.data = pvex.Data;
				sr.error.type = "ProtocolViolationException";
				sr.error.errorCode = 0;
				sr.error.message = pvex.Message;
				sr.error.exceptionSource = pvex.Source;
				sr.error.helpLink = pvex.HelpLink;
				callbacker.webResData = JsonUtility.ToJson(sr);
			} catch(WebException wex) {
				sr.status = false;
				sr.error = new Error();

				sr.error.data = wex.Data;
				sr.error.type = "WebException";
				sr.error.errorCode = 0;
				sr.error.message = wex.Message;
				sr.error.exceptionSource = wex.Source;
				sr.error.helpLink = wex.HelpLink;
				callbacker.webResData = JsonUtility.ToJson(sr);
			} catch(Exception ex) {
				sr.status = false;
				sr.error = new Error();

				sr.error.data = ex.Data;
				sr.error.type = "Exception";
				sr.error.errorCode = 0;
				sr.error.message = ex.Message;
				sr.error.exceptionSource = ex.Source;
				sr.error.helpLink = ex.HelpLink;
				callbacker.webResData = JsonUtility.ToJson(sr);
			} finally {
				if(wRes != null) wRes.Close();
				if(responseReader != null) responseReader.Close();
				if(responseStream != null) responseStream.Close();

				wRes = null;
				responseReader = null;
				responseStream = null;
			}
			_mainThread.Enqueue(callbacker.ExecuteCallback);
			Thread.CurrentThread.Abort();
		}


		private Stream CreateMultipartFormData(Stream stream, Dictionary<string, object> postParameters, string boundry) {
			if(null == postParameters) return stream;
			string postData;
			foreach(var s in postParameters.Keys) {
				if(null == postParameters[s]) continue;
				postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
						boundry, s, postParameters[s]);
				stream.Write(_encoding.GetBytes(postData), 0, _encoding.GetByteCount(postData));
				stream.Write(_encoding.GetBytes(_lineFeed), 0, _encoding.GetByteCount(_lineFeed));
			}
			return stream;
		}
		private Stream CreateMultiPartFileData(Stream stream, List<UploadFileInfo> fileInfo, string boundry) {
			if(null == fileInfo) return stream;
			string fileHeader;
			string fileDataFormat = "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";
			foreach(UploadFileInfo f in fileInfo) {
				if(null == f) continue;
				fileHeader = string.Format(fileDataFormat, boundry, f.uniqueKey, f.fileName, f.contentType ?? "application/octet-stream");
				stream.Write(_encoding.GetBytes(fileHeader), 0, _encoding.GetByteCount(fileHeader));
				stream.Write(f.fileBytes, 0, f.fileBytes.Length);
				stream.Write(_encoding.GetBytes(_lineFeed), 0, _encoding.GetByteCount(_lineFeed));
			}
			return stream;
		}

		private class HTTPCallback {
			public Action<string, WebHeaderCollection> _callback = null;
			public string webResData = string.Empty;
			public WebHeaderCollection responseHeader = null;
			public void ExecuteCallback() {
				//MyDebug.Log("is Callback: {0}", (_callback != null));
				if(_callback != null) {
					_callback(webResData, responseHeader);
				}
			}
		}
	}

	public class UploadFileInfo {
		public string uniqueKey;
		public string fileName;
		public string contentType;
		public byte[] fileBytes;
	}
	public class UploadFileList {
		public string filePath;
		public string contentType;
		public string key;
	}

	public enum WebMethod {
		GET,
		POST
	}
	public enum Compression {
		Normal,
		GZip,
		Deflate
	}

	[Serializable]
	public class Response {
		public int code;
		public bool status;
		public string message;

		public string source;
		public Error error;
		public Response() {
			code = 0;
			status = false;
			message = string.Empty;
			source = string.Empty;
		}
	}

	[Serializable]
	public class Error {
		public IDictionary data;
		public int errorCode;
		public string type;
		public string message;
		public string exceptionSource;
		public string helpLink;

		public Error() {
			data = null;
			errorCode = 0;
			type = string.Empty;
			message = string.Empty;
			exceptionSource = string.Empty;
			helpLink = string.Empty;
		}
	}

	public class ExecuteURLParameters {
		public string url;
		public int timeOut;
		public WebMethod method;
		public bool isMultipart;
		public Compression compMode;

		public Dictionary<string, object> postData;
		public Dictionary<string, object> header;

		public Action<string, WebHeaderCollection> callback;

		public ExecuteURLParameters() {
			url = string.Empty;
			timeOut = 2000;
			method = WebMethod.POST;
			isMultipart = true;
			compMode = Compression.Normal;
			postData = null;
			header = null;
			callback = null;

		}
	}
	public class UploadFileParameters {
		public string url;
		public Compression compMode;
		public int timeOut;

		public List<UploadFileList> uploadList;
		public List<UploadFileInfo> filesToUpload;

		public Dictionary<string, object> postData;
		public Dictionary<string, object> header;

		public Action<string, WebHeaderCollection> callback;

		public UploadFileParameters() {
			url = string.Empty;
			postData = null;
			compMode = Compression.GZip;
			timeOut = 2000;

			header = null;
			uploadList = null;
			filesToUpload = null;
			callback = null;
		}
	}
	public static class MIMETypes {
		//public string 
		//text/plain
		//text/html
		//text/css
		//text/javascript
		//text/xml

		//image/gif
		//image/png
		//image/jpeg
		//image/bmp
		//image/webp
		//image/tiff
		//image/svg+xml

		//audio/mpeg
		//audio/ogg
		//audio/midi,
		//audio/webm
		//audio/wave
		//audio/wav
		//audio/x-wav
		//audio/x-pn-wav
		//audio/*

		//video/mp4
		//video/webm
		//video/ogg
		//video/mpeg

		//application/octet-stream
		//application/pkcs12
		//application/vnd.mspowerpoint
		//application/xhtml+xml
		//application/xml
		//application/pdf
		//application/json
		//application/ogg

		//multipart/form-data
		//multipart/byterange

	}
}