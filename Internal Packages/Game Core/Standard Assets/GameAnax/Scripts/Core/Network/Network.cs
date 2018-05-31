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
using System.Net;

using UnityEngine;

using GameAnax.Core.Utility;

namespace GameAnax.Core.Net {
	public static class Network {
		public static bool IsInternetConnection() {
			bool isConnectedToInternet = false;
			if(Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
			   Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
				isConnectedToInternet = true;
			}
			return isConnectedToInternet;
		}
		static bool IsInternetPing(string server) {
			//TODO: Internet Ping not Added Yet
			return false;
		}


		static string[] urls = new string[] {
			"http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.js",
			"http://www.bing.com",
			"http://www.w3schools.com",
			"http://forum.unity3d.com",
			"http://unity3d.com/unity/qa/patch-releases",
			"http://forum.cgpersia.com"
		};

		public static SpeedCheckResult CheckSpeed(int tries = 3) {
			SpeedCheckResult result;
			double speed = -1f;
			double totSpeed = 0d;
			int index = 0;
			string urlToCheckSpeed = "";
			if(IsInternetConnection()) {
				double[] trySpeed = new double[tries];
				for(int i = 0; i < tries; i++) {
					WebClient client = new WebClient();
					index = UnityEngine.Random.Range(0, urls.Length);
					urlToCheckSpeed = urls[index];
					DateTime startTime = DateTime.Now;
					byte[] data = client.DownloadData(urlToCheckSpeed);
					DateTime endTime = DateTime.Now;
					double dataL = 0;
					if(null != data) {
						dataL = data.Length;
					}
					trySpeed[i] = Math.Round((dataL / (endTime - startTime).TotalSeconds));
					MyDebug.Log("URL: {0}, Speed: {1} B/s", urlToCheckSpeed, trySpeed[i]);
					totSpeed += trySpeed[i];
				}
				speed = (totSpeed / (double)tries);
				result.Speed = speed;
				result.Class = GetInternetSpeedClass(speed);
			} else {
				result.Speed = -1d;
				result.Class = SpeedClass.NoInternet;
			}
			return result;
		}
		public static SpeedClass GetInternetSpeedClass(double speedInBytes) {
			// Speed and class check base table via URL
			// http://telecomtalk.info/difference-between-g-e-3g-h-4g-symbols-we-find-out/121666/

			SpeedClass sClass = SpeedClass.NoInternet;
			double sKbps, sMbps;
			sKbps = (speedInBytes * 8d) / 1024d;
			sMbps = (sKbps / 1024d);
			MyDebug.Log("Speed in Kbits/s: {0}, Mbits/s: {1}", sKbps, sMbps);
			if(sKbps <= 3d) {
				sClass = SpeedClass.NoInternet;
			} else if(sKbps > 3d && sKbps <= 14.4d) {
				sClass = SpeedClass.GSM;
			} else if(sKbps > 14.4d && sKbps <= 53.6d) {
				sClass = SpeedClass.GPRS_G;
			} else if(sKbps > 53.6d && sKbps <= 217.6d) {
				sClass = SpeedClass.EDGE_2G;
			} else if(sKbps > 217.6d && sKbps <= 384d) {
				sClass = SpeedClass.UTMS_3G;
			} else if(sKbps > 384d && sMbps <= 7.2d) {
				sClass = SpeedClass.HSPA;
			} else if(sMbps > 7.2d && sMbps <= 14.4d) {
				sClass = SpeedClass.HSPA_P_R6;
			} else if(sMbps > 14.4d && sMbps <= 21.1d) {
				sClass = SpeedClass.HSPA_P_R7;
			} else if(sMbps > 21.1d && sMbps <= 42.2d) {
				sClass = SpeedClass.HSPA_P_R8;
			} else if(sMbps > 42.2d && sMbps <= 84.4d) {
				sClass = SpeedClass.HSPA_P_R9;
			} else if(sMbps > 84.4d && sMbps <= 100d) {
				sClass = SpeedClass.LTE_4G;
			} else if(sMbps > 100d && sMbps <= 168.8d) {
				sClass = SpeedClass.HSPA_P_R10;
			} else if(sMbps > 168.8d) {
				sClass = SpeedClass.LTE_A_4GA;
			}
			return sClass;
		}
	}
	public struct SpeedCheckResult {
		public double Speed;
		public SpeedClass Class;
	}

	/// <summary>
	/// Intenet Speed Class
	/// </summary>
	public enum SpeedClass {
		NoInternet,
		GSM,
		GPRS_G,
		EDGE_2G,
		UTMS_3G,
		HSPA,
		HSPA_P_R6,
		HSPA_P_R7,
		HSPA_P_R8,
		HSPA_P_R9,
		HSPA_P_R10,
		LTE_4G,
		LTE_A_4GA
	}
}
