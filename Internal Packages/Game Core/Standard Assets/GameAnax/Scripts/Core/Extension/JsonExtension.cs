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
using System.Text;
using System.Collections;
using System.Collections.Generic;

using GameAnax.Core.Utility;

using Prime31;


namespace GameAnax.Core.JSonTools {
	public static class JsonExtension {

		public static JsonObject FromJSonToJSonObject(this string jsonData) {
			JsonObject output = null;
			IDictionary tmp = Json.decode(jsonData) as IDictionary;
			ParseJSonString(tmp, ref output);
			return output;
		}

		public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action) {
			foreach(var i in ie) {
				action(i);
			}
		}

		public static string Repeat(this string str, int count) {
			return new StringBuilder().Insert(0, str, count).ToString();
		}

		public static bool IsEscaped(this string str, int index) {
			bool escaped = false;
			while(index > 0 && str[--index] == '\\') escaped = !escaped;
			return escaped;
		}

		public static bool IsEscaped(this StringBuilder str, int index) {
			return str.ToString().IsEscaped(index);
		}


		static void ParseJSonString(IDictionary input, ref JsonObject output) {
			IDictionaryEnumerator enumerator = input.GetEnumerator();

			if(null == output) {
				output = new JsonObject();
			}

			while(enumerator.MoveNext()) {
				DictionaryEntry entry = (DictionaryEntry)enumerator.Current;

				if(entry.Value.GetType().ToString() == "System.Collections.Generic.Dictionary`2[System.String,System.Object]") {
					ParseJSonString((IDictionary)entry.Value, ref output);
				} else if(entry.Value is string ||
					entry.Value is bool ||
					entry.Value is Boolean ||
					entry.Value is byte ||
					entry.Value is Byte ||
					entry.Value is int ||
					entry.Value is Int16 ||
					entry.Value is Int32 ||
					entry.Value is Int64 ||
					entry.Value is long ||
					entry.Value is float ||
					entry.Value is decimal ||
					entry.Value is double ||
					entry.Value is Single ||
					entry.Value is Double) {
					output.Add(entry.Key.ToString(), entry.Value);
				} else if(entry.Value is Array ||
			 			entry.Value is ArrayList) {
					MyDebug.Info(entry.Key + " has array or list data Not implimented this function yet");
					//output.Add(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
				} else {
					MyDebug.Info(entry.Key + " has unidentified type");
				}
			}

		}
	}
}