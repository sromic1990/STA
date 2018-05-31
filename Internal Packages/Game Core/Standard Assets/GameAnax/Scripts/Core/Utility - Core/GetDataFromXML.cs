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

using System.Xml;

using UnityEngine;


namespace GameAnax.Core.Plugins {
	public class GetDataFromXML {
		public static string GetCountryInfo(string inputValue) {
			return GetCountryInfo(inputValue,
				CountryISO.Code_Alpha2,
				CountryISO.Country_Name);
		}

		public static string GetCountryInfo(string inputValue,
			CountryISO inputType,
			CountryISO outputType) {
			string retValue = string.Empty;
			TextAsset textAsset = (TextAsset)Resources.Load("Config/ISO_3166_1_Country_Code_List");
			string xPath = "ISO_3166_1/ISO_3166_1_Entry[" + inputType.ToString() + "='" + inputValue + "']/" + outputType.ToString();
			if(textAsset != null) {
				retValue = RetriveFromXML(textAsset.text.ToString(), xPath);
			} else {
				throw new System.Exception("Country List not found in Resources...");
			}
			return retValue;
		}

		public static string GetLanguageInfo(string inputValue,
			LanguageISO inputType,
			LanguageISO outputType) {
			string retValue = string.Empty;
			if(inputType == LanguageISO.Native_Name) {
				throw new System.Exception("Current version of Language Data is not allowed to search information base of Native Name...");
			}

			TextAsset textAsset = (TextAsset)Resources.Load("Config/ISO_639_x_Language_Code_List");
			string xPath = "ISO_639/ISO_639_Entry[" + inputType.ToString() + "='" + inputValue + "']/" + outputType.ToString();
			if(textAsset != null) {
				retValue = RetriveFromXML(textAsset.text.ToString(), xPath);
			} else {
				throw new System.Exception("Langauge data not found in Resources...");
			}
			return retValue;
		}

		public static string RetriveFromXML(string xmlData, string xPath) {
			XmlDocument xmlDoc = new XmlDocument();
			if(!string.IsNullOrEmpty(xmlData) && !string.IsNullOrEmpty(xPath)) {
				xmlDoc.LoadXml(xmlData);
				return RetriveFromXML(xmlDoc, xPath);
			} else {
				throw new System.Exception("XML Data not Valid");
			}
		}

		public static string RetriveFromXML(XmlDocument xmlDoc, string xPath) {
			string retValue = string.Empty;
			XmlNode xnMessageData = xmlDoc.SelectSingleNode(xPath);
			if(xnMessageData != null) {
				retValue = xnMessageData.InnerText;
			}
			return retValue;
		}
	}

	public enum CountryISO {
		Country_Name,
		Code_Alpha2
	}

	public enum LanguageISO {
		English_Name,
		Native_Name,
		ISO_639_1,
		ISO_639_2T,
		ISO_639_2B,
		ISO_639_3,
		ISO_639_6
	}
}