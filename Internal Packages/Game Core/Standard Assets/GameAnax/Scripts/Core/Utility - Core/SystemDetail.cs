using System;
using System.Net;
using System.Net.NetworkInformation;

using UnityEngine;

using GameAnax.Core.Extension;
using GameAnax.Core.Net;
using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;

namespace GameAnax.Core {
	[PersistentSignleton(true, true)]
	public class SystemDetail : SingletonAuto<SystemDetail> {

		public Action<string> HostnameRetriveSucess;
		public Action<IPInfo[], string> LocalIPRetriveSuccess;
		public Action<string> LocalIPRetriveFail;
		public Action<string> PublicIPRetriveSuccess;
		public Action<string> PublicIPRetriveFail;

		public Action<string> MACAddressRetriveSuccess;

		private void OnHostnameRetriveSucess(string hostName) {
			if(HostnameRetriveSucess != null) {
				HostnameRetriveSucess.Invoke(hostName);
			}
		}
		private void OnLocalIPSuccess(IPInfo[] ipaddresses, string address) {
			if(LocalIPRetriveSuccess != null) {
				LocalIPRetriveSuccess.Invoke(ipaddresses, address);
			}
		}
		private void OnLocalIPFail(string error) {
			if(LocalIPRetriveFail != null) {
				LocalIPRetriveFail.Invoke(error);
			}
		}

		private void OnPublicIPRecevied(string ipaddress) {
			if(PublicIPRetriveSuccess != null) {
				PublicIPRetriveSuccess.Invoke(ipaddress);
			}
		}
		private void OnPublicIPFail(string error) {
			if(PublicIPRetriveFail != null) {
				PublicIPRetriveFail.Invoke(error);
			}
		}

		private void OnMACAddressRetriveSuccess(string macid) {
			if(MACAddressRetriveSuccess != null) {
				MACAddressRetriveSuccess.Invoke(macid);
			}
		}

		private WebData wd;
		// Use this for initialization
		void Awake() {
			wd = new WebData();
		}
		// Update is called once per frame
		//void Update() { }

		public void GetIPAddress() {
			// Local IP Address (returns your internal IP address)
			string hostName = Dns.GetHostName();
			if(!hostName.IsNulOrEmpty()) {
				OnHostnameRetriveSucess(hostName);
				IPHostEntry localIpAddresses = Dns.GetHostEntry(hostName);
				if(localIpAddresses.AddressList.Length > 0) {
					IPInfo[] ips = new IPInfo[localIpAddresses.AddressList.Length];
					string address = localIpAddresses.AddressList[0].ToString();
					for(int x = 0; x < ips.Length; x++) {
						ips[x].type = localIpAddresses.AddressList[x].AddressFamily.ToString();
						ips[x].address = localIpAddresses.AddressList[x].ToString();
					}
					OnLocalIPSuccess(ips, address);
				}
			} else {
				OnLocalIPFail("Not able to detect host information");
			}
			GetPublicIPAddress();
		}
		public void GetPublicIPAddress() {
			if(Net.Network.IsInternetConnection()) {
				wd.ExecuteURL(new ExecuteURLParameters() {
					url = "http://checkip.dyndns.org",
					timeOut = 10000,
					method = WebMethod.GET,
					callback = GetPublicIPResposne
				});
			} else {
				OnPublicIPFail("No Internet Connection Available");
			}
		}
		private void GetPublicIPResposne(string data, WebHeaderCollection header) {

			MyDebug.Log("Data: {0}", data);
			if(data.Contains("error")) {
				MyDebug.Log("SystemDetail::GetPublicIPAddress => data with error: {0}", data);
				OnPublicIPFail("Unknown Error occured to retrive public ip");
				return;
			}
			string[] a = data.Split(':');
			if(a.Length > 1) {
				string a2 = a[1];
				string[] a3 = a2.Split('<');
				string a4 = a3[0];
				OnPublicIPRecevied(a4);
			} else {
				MyDebug.Log("SystemDetail::GetPublicIPAddress => Unknown data: {0}", data);
				OnPublicIPFail("Unknown data recevied");
			}
		}

		public void GetMACAddress() {
			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
			string sMacAddress = string.Empty;
			foreach(NetworkInterface adapter in nics) {
				if(sMacAddress == string.Empty)// only return MAC Address from first card  
				{
					IPInterfaceProperties properties = adapter.GetIPProperties();
					sMacAddress = adapter.GetPhysicalAddress().ToString();
				}
			}
			if(!sMacAddress.IsNulOrEmpty()) {
				int loop = sMacAddress.Length / 2;
				string s = string.Empty;
				for(int i = 0; i < loop; i++) {
					s += sMacAddress.Substring(i * 2, 2) + ":";
				}
				s = s.TrimEnd(':');
				OnMACAddressRetriveSuccess(s);
			}
		}
	}

	public struct IPInfo {
		public string address;
		public string type;
	}
}