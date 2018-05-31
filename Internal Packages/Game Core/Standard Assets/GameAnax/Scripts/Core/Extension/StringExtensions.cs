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

using System.Collections.Generic;


namespace GameAnax.Core.Extension {
	public static class StringExtension {
		public static Dictionary<string, string> dictionaryFromQueryString(this string self) {
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			string[] array = self.Split('?');
			string[] keyValuePairs;
			if(array.Length < 2) {
				keyValuePairs = self.Split('&');
			} else {
				keyValuePairs = array[1].Split('&');
			}
			string[] array3 = keyValuePairs;
			for(int i = 0; i < array3.Length; i++) {
				string keyValuePair = array3[i];
				string[] array4 = keyValuePair.Split('=');
				dictionary.Add(array4[0], array4[1]);
			}
			return dictionary;
		}
		public static string Left(this string data, int length) {
			string returnData;
			if(data.Length <= length) {
				returnData = data;
			} else {
				returnData = data.Substring(0, length);
			}
			return returnData;
		}
		public static string Right(this string data, int length) {
			string returnData;
			if(data.Length <= length) {
				returnData = data;
			} else {
				returnData = data.Substring(data.Length - length - 1, length);
			}
			return returnData;
		}
		public static string RandomEnumValue<T>() {
			System.Random random = new System.Random();
			string[] values = System.Enum.GetNames(typeof(T));
			string randomValue = values.GetValue(random.Next(values.Length)).ToString();
			return randomValue;
		}
		public static bool IsURL(this string content) {
			bool result = false;
			if(content.StartsWith("http://", System.StringComparison.InvariantCultureIgnoreCase))
				result = true;
			else if(content.StartsWith("https://", System.StringComparison.InvariantCultureIgnoreCase))
				result = true;
			else if(content.StartsWith("ftp://", System.StringComparison.InvariantCultureIgnoreCase))
				result = true;
			else if(content.StartsWith("file://", System.StringComparison.InvariantCultureIgnoreCase))
				result = true;
			return result;
		}

		/// <summary>
		/// Trim all will trim White space and new line character from being and End if any
		/// </summary>
		/// <returns>trimed string</returns>
		/// <param name="text">string to trim</param>
		public static string TrimAll(this string text) {
			if(string.IsNullOrEmpty(text)) { text = string.Empty; }
			text = text.Trim();
			text = text.Trim('\n');
			text = text.Trim('\r');
			text = text.Trim('\n');
			return text;
		}
		public static bool IsNulOrEmpty(this string data) {
			return string.IsNullOrEmpty(data.Trim());
		}
		public static string[] SplitbyNewLine(this string data) {
			string[] result = { data };
			if(data.Contains("\r\n")) {
				data = data.Replace("\r\n", "\n");
			}
			if(data.Contains("\n")) result = data.Split('\n');
			else if(data.Contains("\r")) result = data.Split('\r');
			return result;
		}
	}
}