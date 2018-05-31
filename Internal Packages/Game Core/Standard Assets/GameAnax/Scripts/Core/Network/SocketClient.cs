using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

using UnityEngine;

using GameAnax.Core.Threader;
using GameAnax.Core.Utility;

using Prime31;


namespace GameAnax.Core.Net {
	public class SocketClient {
		public int requestId { private set; get; }

		JoinUnityMainThread mainThread;
		CoroutineInvoker coroutineInvoker;

		private byte[] _recieveBuffer = new byte[8142];
		private Socket _client;
		private Encoding _encoding = Encoding.Default;
		private float _readWait = 0f, _sendWait = 0f;


		private AddressFamily _family = AddressFamily.InterNetwork;
		private SocketType _socketType = SocketType.Stream;
		private ProtocolType _protocol = ProtocolType.Tcp;

		private string _ipAddress;
		private int _port;

		private Dictionary<int, SocketCallback> callbackList = new Dictionary<int, SocketCallback>();
		private Queue<string> _pendingRequest = new Queue<string>();

		private Coroutine tryToConnect;//, tryToSendPendingData, tryToRead;

		public Action<string> SocketConnectionError;
		public Action<string> SocketConnectionClose;
		public Action SocketConnected;
		public Action<string> SocketReceivedUnknownData;

		public SocketClient() : this("localhost", 80, Encoding.Default, 0f, 0f) { }
		public SocketClient(string ip) : this(ip, 80, Encoding.Default, 0f, 0f) { }
		public SocketClient(int port) : this("localhost", port, Encoding.Default, 0f, 0f) { }
		public SocketClient(float readSendWait) : this("localhost", 80, Encoding.Default, readSendWait, readSendWait) { }
		public SocketClient(string ip, int port) : this(ip, port, Encoding.Default, 0f, 0f) { }
		public SocketClient(string ip, Encoding encode) : this(ip, 80, encode, 0f, 0f) { }
		public SocketClient(string ip, float readSendWait) : this(ip, 80, Encoding.Default, readSendWait, readSendWait) { }
		public SocketClient(int port, Encoding encode) : this("localhost", port, encode, 0f, 0f) { }
		public SocketClient(int port, float readSendWait) : this("localhost", port, Encoding.Default, readSendWait, readSendWait) { }
		public SocketClient(Encoding encode, float readSendWait) : this("localhost", 80, encode, readSendWait, readSendWait) { }
		public SocketClient(string ip, int port, Encoding encode) : this(ip, port, encode, 0f, 0f) { }
		public SocketClient(string ip, int port, float readSendWait) : this(ip, port, Encoding.Default, readSendWait, readSendWait) { }
		public SocketClient(string ip, float readWait, float sendWait) : this(ip, 80, Encoding.Default, readWait, sendWait) { }
		public SocketClient(int port, float readWait, float sendWait) : this("localhost", port, Encoding.Default, readWait, sendWait) { }
		public SocketClient(Encoding encode, float readWait, float sendWait) : this("localhost", 80, encode, readWait, sendWait) { }
		public SocketClient(string ip, int port, Encoding encode, float readSendWait) : this(ip, port, encode, readSendWait, readSendWait) { }
		public SocketClient(string ip, int port, float readWait, float sendWait) : this(ip, port, Encoding.Default, readWait, sendWait) { }
		public SocketClient(string ip, int port, Encoding encode, float readWait, float sendWait) {

			Response socketError = new Response();
			string errorJsonData;

			this._ipAddress = ip;
			this._port = port;
			this._encoding = encode;
			this._readWait = readWait;
			this._sendWait = sendWait;

			try {
				mainThread = JoinUnityMainThread.Me;
				_client = new Socket(_family, _socketType, _protocol);
			} catch(Exception ex) {
				socketError.code = 200;
				socketError.message = "Faced exception during creating socket connection or binding with JoinUnityMainThread";
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

		~SocketClient() {
			if(_client != null && _client.Connected)
				Disconnect();

			_client = null;
		}

		public void Connect() {
			tryToConnect = CoroutineInvoker.Me.Invoke(Reconnect());
		}
		public void Disconnect() {
			Response socketError = new Response();
			string errorJsonData;
			try {
				_reConnect = false; _chkPendingRequest = false; _readData = false;
				CoroutineInvoker.Me.StopCustomCoroutine(tryToConnect);
				//CoroutineInvoker.Me.StopCustomCoroutine(tryToSendPendingData);
				//CoroutineInvoker.Me.StopCustomCoroutine(tryToRead);


				if(_client != null && _client.Connected)
					_client.Disconnect(true);

				OnSocketConnected();

			} catch(SocketException soex) {
				socketError.code = 200;
				socketError.message = "faced Socket Exception during trying to closing socket with reusebility";
				socketError.status = false;
				socketError.source = "SocketException";

				socketError.error.data = soex.Data;
				socketError.error.message = soex.Message;
				socketError.error.exceptionSource = soex.Source;
				socketError.error.helpLink = soex.HelpLink;
				socketError.error.errorCode = soex.ErrorCode;
				errorJsonData = JsonUtility.ToJson(socketError);
				OnSocketConnectionError(errorJsonData);
			} catch(Exception ex) {
				socketError.code = 200;
				socketError.message = "faced Exception during trying to  closing socket with reusebility";
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
		private void ConnectToSocket() {
			Response socketError = new Response();
			string errorJsonData;

			try {
				_client.Connect(_ipAddress, _port);

				OnSocketConnected();

				mainThread.Enqueue(ExecutePendingRequest());
				mainThread.Enqueue(ReadData());
			} catch(SocketException soex) {
				socketError.code = 200;
				socketError.message = "faced Socket Exception during trying to connect with socket ";
				socketError.status = false;
				socketError.source = "SocketException";

				socketError.error.data = soex.Data;
				socketError.error.message = soex.Message;
				socketError.error.exceptionSource = soex.Source;
				socketError.error.helpLink = soex.HelpLink;
				socketError.error.errorCode = soex.ErrorCode;
				errorJsonData = JsonUtility.ToJson(socketError);
				OnSocketConnectionError(errorJsonData);
			} catch(Exception ex) {
				socketError.code = 200;
				socketError.message = "faced Exception during trying to connect with socket ";
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

			//_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
		}
		//private void LastReceiveCallback(IAsyncResult AR) { }
		//private void ReceiveCallback(IAsyncResult AR) {
		//	//Check how much bytes are recieved and call EndRecieve to finalize handshake
		//	int recieved = _clientSocket.EndReceive(AR);
		//
		//	if(recieved <= 0)
		//		return;
		//
		//	//Copy the recieved data into new buffer , to avoid null bytes
		//	byte[] recData = new byte[recieved];
		//	Buffer.BlockCopy(_recieveBuffer, 0, recData, 0, recieved);
		//	//Process data here the way you want , all your bytes will be stored in recData
		//
		//	//Start receiving again
		//	_clientSocket.BeginReceive(_recieveBuffer, 0, _recieveBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
		//}



		private bool _reConnect = false, _chkPendingRequest = false, _readData = false;
		private IEnumerator Reconnect() {
			_reConnect = true;
			yield return new WaitForSecondsRealtime(0.1f);

			while(_reConnect) {
				if(_client != null && !_client.Connected) {
					ConnectToSocket();
					yield return new WaitForSeconds(5);
				} else {
					yield return new WaitForSeconds(1);
				}
			}
		}
		private IEnumerator ExecutePendingRequest() {
			_chkPendingRequest = true;
			yield return new WaitForSecondsRealtime(1);
			MyDebug.Log("TCP Server is connected? {0}", _client.Connected);
			while(_chkPendingRequest) {
				if(_client != null) {
					if(_client.Connected) {
						SendRequestToServer();
						if(_sendWait > 0) yield return new WaitForSecondsRealtime(_sendWait);
						else yield return new WaitForEndOfFrame();
					} else {
						MyDebug.Warning("TCP Client connetion status: Closed}");
						yield return new WaitForSeconds(1f);
					}
				} else {
					MyDebug.Warning("TCP client not created yet");
					yield return new WaitForSeconds(5f);
				}
			}
		}

		public int Request(RequestData data, Action<string> dataCallback) {
			requestId++;
			data.rerquestId = requestId;
			string finalRequestData = Json.encode(data);
			if(!callbackList.ContainsKey(requestId) && null != dataCallback) {
				callbackList.Add(requestId, new SocketCallback(dataCallback));
			}
			_pendingRequest.Enqueue(finalRequestData);
			return requestId;
		}
		string requestData = "Sendign Sample data";
		private void SendRequestToServer() {
			//if(_pendingRequest.Count > 0) {
			//string requestData = _pendingRequest.Dequeue();
			this.Reqeust(requestData);
			//}
		}

		private void Reqeust(string jsonStringData) {
			byte[] sendData = _encoding.GetBytes(jsonStringData);
			SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
			socketAsyncData.SetBuffer(sendData, 0, sendData.Length);
			lock(_client) {
				_client.SendAsync(socketAsyncData);
			}
		}

		private byte[] _readBytes = { };
		private string _receivedString;
		private int _receviedDataLength;

		private IEnumerator ReadData() {
			_readData = true;
			yield return new WaitForSecondsRealtime(2);
			while(_readData) {
				if(_client != null) {
					if(_client.Connected) {
						MyDebug.Log("Reading from socket");
						ReadFromScoket();
						if(_sendWait > 0) yield return new WaitForSecondsRealtime(_readWait);
						else yield return new WaitForEndOfFrame();
					} else {
						MyDebug.Warning("TCP Client connetion status: Closed}");
						yield return new WaitForSeconds(1f);
					}
				} else {
					MyDebug.Warning("TCP client not created yet");
					yield return new WaitForSeconds(5f);
				}
			}
		}
		private void ReadFromScoket() {
			if(_client != null && _client.Connected) {
				lock(_client) {
					MyDebug.Log("reading data from stream");
					lock(_client) {
						_client.Receive(_readBytes);
					}
					_receivedString = _encoding.GetString(_readBytes);
					CheckResponse(_receivedString);
				}

				MyDebug.Log("Gettig string of data from bytes");
				_receivedString = _encoding.GetString(_readBytes);
				CheckResponse(_receivedString);
			}
		}

		private void CheckResponse(string rData) {
			if(string.IsNullOrEmpty(rData)) return;


			int receivedFor = -1;
			string rId;
			IDictionary data = (IDictionary)Json.decode(rData);
			if(null != data && data.Contains("rerquestId")) {
				rId = data["rerquestId"].ToString();
				int.TryParse(rId, out receivedFor);
				if(receivedFor > 0) {
					QueueResponse(receivedFor, rData);
					return;
				}
			}
			OnSocketReceivedUnknownData(rData);
		}
		private void QueueResponse(int id, string data) {
			if(callbackList.ContainsKey(id)) {
				callbackList[id].response = data;
				mainThread.Enqueue(callbackList[id].ExecuteCallback);
				callbackList.Remove(id);
			}
		}


		#region Event Executer
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
		#endregion
	}
}