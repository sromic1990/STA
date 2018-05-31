using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UnityEngine;

using GameAnax.Core.Threader;

using Prime31;

namespace GameAnax.Core.Net {
	public struct Defaults {
		public const string HOST = "127.0.0.1";
		public const int PORT = 80;
		public Encoding ENCODEING { get { return Encoding.Default; } }
		public const float READ_WAIT = 0f;
	}

	public class TcpScoket {
		TcpClient _client;
		NetworkStream _serverStream;

		JoinUnityMainThread mainThread;
		CoroutineInvoker coroutineInvoker;

		public int requestId { private set; get; }

		private byte[] _recieveBuffer = new byte[8142];
		private Encoding _encoding = Encoding.Default;
		private float _readWait = 0f, _sendWait = 0f;

		private string _ipAddress;
		private int _port;

		private Dictionary<int, SocketCallback> callbackList = new Dictionary<int, SocketCallback>();
		private Queue<string> _pendingRequest = new Queue<string>();

		private Coroutine tryToConnect, tryToSendPendingData, tryToRead;

		public Action<string> SocketConnectionError;
		public Action<string> SocketConnectionClose;
		public Action SocketConnected;
		public Action<string> SocketReceivedUnknownData;

		public TcpScoket() : this(Defaults.HOST, Defaults.PORT, Encoding.Default, 0f, 0f) { }
		public TcpScoket(string ip) : this(ip, Defaults.PORT, Encoding.Default, 0f, 0f) { }
		public TcpScoket(int port) : this(Defaults.HOST, port, Encoding.Default, 0f, 0f) { }
		public TcpScoket(float readSendWait) : this(Defaults.HOST, Defaults.PORT, Encoding.Default, readSendWait, readSendWait) { }
		public TcpScoket(string ip, int port) : this(ip, port, Encoding.Default, 0f, 0f) { }
		public TcpScoket(string ip, Encoding encode) : this(ip, Defaults.PORT, encode, 0f, 0f) { }
		public TcpScoket(string ip, float readSendWait) : this(ip, Defaults.PORT, Encoding.Default, readSendWait, readSendWait) { }
		public TcpScoket(int port, Encoding encode) : this(Defaults.HOST, port, encode, 0f, 0f) { }
		public TcpScoket(int port, float readSendWait) : this(Defaults.HOST, port, Encoding.Default, readSendWait, readSendWait) { }
		public TcpScoket(Encoding encode, float readSendWait) : this(Defaults.HOST, Defaults.PORT, encode, readSendWait, readSendWait) { }
		public TcpScoket(string ip, int port, Encoding encode) : this(ip, port, encode, 0f, 0f) { }
		public TcpScoket(string ip, int port, float readSendWait) : this(ip, port, Encoding.Default, readSendWait, readSendWait) { }
		public TcpScoket(string ip, float readWait, float sendWait) : this(ip, Defaults.PORT, Encoding.Default, readWait, sendWait) { }
		public TcpScoket(int port, float readWait, float sendWait) : this(Defaults.HOST, port, Encoding.Default, readWait, sendWait) { }
		public TcpScoket(Encoding encode, float readWait, float sendWait) : this(Defaults.HOST, Defaults.PORT, encode, readWait, sendWait) { }
		public TcpScoket(string ip, int port, Encoding encode, float readSendWait) : this(ip, port, encode, readSendWait, readSendWait) { }
		public TcpScoket(string ip, int port, float readWait, float sendWait) : this(ip, port, Encoding.Default, readWait, sendWait) { }

		public TcpScoket(string ip, int port, Encoding encode, float readWait, float sendWait) {


			Response socketError = new Response();
			string errorJsonData;

			this._ipAddress = ip;
			this._port = port;
			this._encoding = encode;
			this._readWait = readWait;
			this._sendWait = sendWait;

			try {
				mainThread = JoinUnityMainThread.Me;
				coroutineInvoker = CoroutineInvoker.Me;

				_client = new TcpClient();

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

		~TcpScoket() {
			if(_client != null && _client.Connected) {
				Disconnect();
			}

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
				//CoroutineInvoker.Me.StopCustomCoroutine(tryToConnect);
				//CoroutineInvoker.Me.StopCustomCoroutine(tryToSendPendingData);
				//CoroutineInvoker.Me.StopCustomCoroutine(tryToRead);

				if(_client != null && _client.Connected) {
					_serverStream.Close();
					_serverStream.Dispose();
					_serverStream = null;

					_client.Close();
				}

				OnSocketConnectionClose("manualy closed");

			} catch(ProtocolViolationException pvex) {
				socketError.code = 200;
				socketError.message = "faced Protocol Violation Exception during trying to closing tcp client";
				socketError.status = false;
				socketError.source = "ProtocolViolationException";

				socketError.error.data = pvex.Data;
				socketError.error.message = pvex.Message;
				socketError.error.exceptionSource = pvex.Source;
				socketError.error.helpLink = pvex.HelpLink;

				errorJsonData = JsonUtility.ToJson(socketError);
				OnSocketConnectionError(errorJsonData);
			} catch(SocketException soex) {
				socketError.code = 200;
				socketError.message = "Faced Socket Exception during trying to closing tcp client";
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
				socketError.message = "Faced Exception during trying to  closing tcp client";
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
				_serverStream = _client.GetStream();
				OnSocketConnected();

				mainThread.Enqueue(ExecutePendingRequest());
				mainThread.Enqueue(ReadData());

			} catch(SocketException soex) {
				socketError.code = 200;
				socketError.message = "Faced Socket Exception during trying to connect with tcp client ";
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
				socketError.message = "Faced Exception during trying to connect with tcp client ";
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
			//MyDebug.Log("TCP Server is connected? {0}", _client.Connected);
			while(_chkPendingRequest) {
				if(_client != null) {
					if(_client.Connected) {
						SendRequestToServer();
						if(_sendWait > 0) yield return new WaitForSecondsRealtime(_sendWait);
						else yield return new WaitForEndOfFrame();
					} else {
						//MyDebug.Warning("TCP Client connetion status: Closed}");
						yield return new WaitForSeconds(1f);
					}
				} else {
					//MyDebug.Warning("TCP client not created yet");
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
			byte[] dataToSend = _encoding.GetBytes(jsonStringData);
			lock(_client) {
				_serverStream.Write(dataToSend, 0, dataToSend.Length);
				_serverStream.Flush();
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
						//MyDebug.Log("Reading from socket");
						ReadFromScoket();
						if(_sendWait > 0) yield return new WaitForSecondsRealtime(_readWait);
						else yield return new WaitForEndOfFrame();
					} else {
						//MyDebug.Warning("TCP Client connetion status: Closed}");
						yield return new WaitForSeconds(1f);
					}
				} else {
					//MyDebug.Warning("TCP client not created yet");
					yield return new WaitForSeconds(5f);
				}
			}
		}
		private void ReadFromScoket() {
			if(_client != null && _client.Connected) {
				//MyDebug.Log("Locking tcpClient for read {0} bytes", _receviedDataLength);
				lock(_client) {
					//MyDebug.Log("reading data from stream");
					if(_serverStream != null) {
						_receviedDataLength = (int)_serverStream.Length;
						_readBytes = new byte[_receviedDataLength];
						if(_serverStream.DataAvailable)
							_serverStream.Read(_readBytes, 0, _receviedDataLength - 1);
						else {
							//MyDebug.Log("No data availabe on server network stream");
						}
					} else {
						//MyDebug.Log("Server Network stream not exists");
					}
				}
				//MyDebug.Log("Gettig string of data from bytes");
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