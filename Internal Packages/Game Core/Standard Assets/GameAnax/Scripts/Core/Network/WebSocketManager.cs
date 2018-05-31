using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;


using GameAnax.Core.Threader;
using GameAnax.Core.Utility;

using WebSocketSharp;
using Prime31;

namespace GameAnax.Core.Net {
	public class WebSocketManager {
		JoinUnityMainThread mainThread;
		CoroutineInvoker coroutineInvoker;
		public int requestId { private set; get; }

		public Action<string> SocketConnectionError;
		public Action<string> SocketConnectionClose;
		public Action SocketConnected;
		public Action<string> SocketReceivedUnknownData;

		private Dictionary<int, SocketCallback> callbackList = new Dictionary<int, SocketCallback>();
		//private List<string> _pendingRequest = new List<string>();
		private Queue<string> _pendingRequest = new Queue<string>();

		private WebSocket _client;
		private Coroutine tryToConnect;
		private Coroutine tryToSendPendingData;

		private string _host = "//localhost/";
		private Encoding _encoding = Encoding.Default;
		private float _readWait = 0f, _sendWait = 0f;


		public WebSocketManager() : this("localhost", Encoding.Default, 0f) { }

		public WebSocketManager(string url) : this(url, Encoding.Default, 0f) { }
		public WebSocketManager(float readSendWait) : this("localhost", Encoding.Default, readSendWait) { }
		public WebSocketManager(Encoding encode) : this("localhost", encode, 0f) { }

		public WebSocketManager(string url, Encoding encode) : this(url, encode, 0f) { }
		public WebSocketManager(string url, float readSendWait) : this(url, Encoding.Default, readSendWait) { }
		public WebSocketManager(Encoding encode, float readSendWait) : this("localhost", encode, readSendWait) { }
		public WebSocketManager(string url, Encoding encode, float sendWait) {

			Response socketError = new Response();
			string errorJsonData;

			this._host = url;
			this._encoding = encode;
			this._sendWait = sendWait;

			try {
				mainThread = JoinUnityMainThread.Me;
				coroutineInvoker = CoroutineInvoker.Me;
			} catch(Exception ex) {
				socketError.code = 200;
				socketError.message = "Faced exception during creating binding with JoinUnityMainThread";
				socketError.status = false;
				socketError.source = "Exception";

				socketError.error.data = ex.Data;
				socketError.error.message = ex.Message;
				socketError.error.exceptionSource = ex.Source;
				socketError.error.helpLink = ex.HelpLink;
				errorJsonData = JsonUtility.ToJson(socketError);
				OnSocketConnectionError(errorJsonData);
			} finally {
				socketError = null;
			}
		}

		~WebSocketManager() {
			//if(null != tryToConnect) CoroutineInvoker.Me.StopCoroutine(tryToConnect);
			//if(null != tryToSendPendingData) CoroutineInvoker.Me.StopCoroutine(tryToSendPendingData);
			this.Disconnect();
			_client = null;
		}


		public void Connect() {
			MyDebug.Log("Coonect Called");
			mainThread.Enqueue(Reconnect());

		}
		public void Disconnect() {
			if(_client.ReadyState.Equals(WebSocketState.Open) || _client.ReadyState.Equals(WebSocketState.Connecting))
				_client.Close();
		}
		private bool _reConnect = false, _chkPendingRequest = false;

		private IEnumerator Reconnect() {
			MyDebug.Log("Reconnect Called");
			_reConnect = true;
			yield return new WaitForSecondsRealtime(0.1f);
			while(_reConnect) {
				if(_client == null || _client.ReadyState == WebSocketState.Closed) {
					MyDebug.Log("Tyring to connect with Web Socket");
					ConnectSocket();
					yield return new WaitForSeconds(5);
				} else {
					//MyDebug.Warning("WebSocket connetion status is: {0}", webSocket.ReadyState);
				}
				yield return new WaitForSeconds(1);
			}
		}

		private IEnumerator ExecutePendingRequest() {
			_chkPendingRequest = true;
			//MyDebug.Warning("Start executing pending request");
			yield return new WaitForSecondsRealtime(1);
			while(_chkPendingRequest) {
				if(_client != null) {
					if(_client.ReadyState.Equals(WebSocketState.Open)) {
						//MyDebug.Warning("WebSocket connetion status \"Open\"");
						SendRequestToServer();
						if(_sendWait > 0) yield return new WaitForSecondsRealtime(_sendWait);
						else yield return new WaitForEndOfFrame();
					} else {
						MyDebug.Warning("WebSocket connetion status is not \"Open\"");
						yield return new WaitForSeconds(1f);
					}
				} else {
					MyDebug.Warning("WebSocket not created yet");
					yield return new WaitForSeconds(5f);
				}
			}
		}

		private void ConnectSocket() {
			MyDebug.Log("Tyring to connect with Web Socket: webSocket.ConnectAsync");
			MyDebug.Log("Connecting to: {0}", _host);
			_client = new WebSocket(_host);
			_client.WaitTime = TimeSpan.FromSeconds(1);
			_client.OnOpen += OnConnectionStablished;
			_client.OnMessage += OnMessageReceived;
			_client.OnError += OnConnectionError;
			_client.OnClose += OnConnectionClose;
			_client.ConnectAsync();
		}
		private void UnSubscribeSocketEvents() {
			if(_client == null) return;
			_client.OnOpen -= OnConnectionStablished;
			_client.OnMessage -= OnMessageReceived;
			_client.OnError -= OnConnectionError;
			_client.OnClose -= OnConnectionClose;
		}

		public int Request(RequestData data) {
			return Request(data, null);
		}
		public int Request(RequestData data, Action<string> dataCallback) {
			requestId++;
			string finalRequestData;
			data.rerquestId = requestId;

			finalRequestData = Json.encode(data);
			MyDebug.Log("{0} => request has been queued", finalRequestData);
			if(!callbackList.ContainsKey(requestId) && null != dataCallback) {
				callbackList.Add(requestId, new SocketCallback(dataCallback));
			}
			_pendingRequest.Enqueue(finalRequestData);
			return requestId;
		}
		private void Reqeust(string jsonStringData) {
			byte[] sendData = _encoding.GetBytes(jsonStringData);
			MyDebug.Log("Send Data: {0}, {1}", jsonStringData, sendData.Length);
			_client.Send(sendData);
		}

		string requestData;
		private void SendRequestToServer() {
			//MyDebug.Log("Tyeing to execute request");
			if(_pendingRequest.Count > 0) {
				requestData = _pendingRequest.Dequeue();
				this.Reqeust(requestData);
			}
		}

		private void OnConnectionStablished(object sender, System.EventArgs e) {
			Debug.Log("Connected to WebServer");
			mainThread.Enqueue(ExecutePendingRequest());
			OnSocketConnected();
		}
		private void OnMessageReceived(object sender, MessageEventArgs e) {
			CheckResponse(sender, e);
		}
		private void OnConnectionError(object sender, ErrorEventArgs e) {
			MyDebug.Log("Error: " + e.Message);
			Response socketError = new Response();
			socketError.code = 200;
			socketError.message = e.Message;
			socketError.status = false;
			socketError.source = "OnConnectionError";

			socketError.error.data = e.Exception.Data;
			socketError.error.message = e.Exception.Message;
			socketError.error.exceptionSource = e.Exception.Source;
			socketError.error.helpLink = e.Exception.HelpLink;

			_client = null;
			string jsonData;
			jsonData = JsonUtility.ToJson(socketError);
			OnSocketConnectionError(jsonData);
		}
		private void OnConnectionClose(object sender, CloseEventArgs e) {
			Response closeInfo = new Response();
			closeInfo.code = e.Code;
			closeInfo.message = e.Reason;
			closeInfo.source = "OnConnectionClose";
			closeInfo.error = null;


			UnSubscribeSocketEvents();
			_client = null;

			string jsonData;
			jsonData = JsonUtility.ToJson(closeInfo);
			OnSocketConnectionClose(jsonData);
		}



		private void OnSocketConnected() {
			if(null != SocketConnected) {
				SocketConnected.Invoke();
			}
		}
		private void OnSocketConnectionError(string data) {
			if(null != SocketConnectionError) {
				SocketConnectionError.Invoke(data);
			}
		}
		private void OnSocketConnectionClose(string data) {
			if(null != SocketConnectionClose) {
				SocketConnectionClose.Invoke(data);
			}
		}
		private void OnSocketReceivedUnknownData(string data) {
			if(SocketReceivedUnknownData != null) {
				SocketReceivedUnknownData.Invoke(data);
			}
		}

		private void CheckResponse(object sender, MessageEventArgs e) {
			int receivedFor = -1;
			string rId;
			string rData = string.Empty;
			if(e.IsPing) {
				MyDebug.Log("Ping Received");
				return;
			} else if(e.IsText) {
				rData = e.Data;
			} else if(e.IsBinary && e.RawData != null) {
				rData = _encoding.GetString(e.RawData);
			}
			if(string.IsNullOrEmpty(rData)) return;

			IDictionary data = (IDictionary)Json.decode(rData);
			if(null != data && data.Contains("rerquestId")) {
				rId = data["rerquestId"].ToString();
				int.TryParse(rId, out receivedFor);
				if(receivedFor > 0) {
					QueueResponse(receivedFor, rData);
					return;
				}
			}
			MyDebug.Log("Recevied Unknown Data: {0}", rData);
			OnSocketReceivedUnknownData(rData);
		}

		private void QueueResponse(int id, string data) {
			if(callbackList.ContainsKey(id)) {
				callbackList[id].response = data;
				mainThread.Enqueue(callbackList[id].ExecuteCallback);
				callbackList.Remove(id);
			}
		}
	}

	public class SocketCallback {
		public Action<string> _callback = null;
		public string response = string.Empty;
		public SocketCallback() { }
		public SocketCallback(Action<string> callback) {
			_callback = callback;
		}
		public void ExecuteCallback() {
			if(_callback != null) {
				_callback(response);
			}
		}
	}

	[System.Serializable]
	public class RequestData {
		public int rerquestId;
		public string method;
		public string key;
		public string accessToken;

		public Dictionary<string, object> data = new Dictionary<string, object>();
	}
}