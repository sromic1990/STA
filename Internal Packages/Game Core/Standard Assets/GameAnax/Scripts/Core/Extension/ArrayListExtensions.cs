//
// Coder:			Amit Kapadi, Ranpariya Ankur {GameAnax} 
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
	public static class ArrayListExtensions {
		static System.Random rng = new System.Random();
		public static void Shuffle<T>(this IList<T> list) {
			int n = list.Count;
			while(n > 1) {
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
		public static void Shuffle<T>(this T[] list) {
			int n = list.Length;
			while(n > 1) {
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static int LastIndex<T>(this List<T> list) {
			return list.Count - 1;
		}
		public static T Last<T>(this List<T> list) {
			return list[list.Count - 1];
		}
		public static T First<T>(this List<T> list) {
			return list[0];
		}
		public static T Random<T>(this List<T> list) {
			return list[list.RandomIndex()];
		}
		public static int RandomIndex<T>(this List<T> list) {
			return UnityEngine.Random.Range(0, list.Count);
		}
		public static int RandomHeadOrTailIndex<T>(this List<T> list) {
			return (CoreMethods.Random01() == 0 ? 0 : list.Count - 1);
		}
		public static T RandomHeadOrTail<T>(this List<T> list) {
			int index = (CoreMethods.Random01() == 0 ? 0 : list.Count - 1);
			return list[index];
		}

		public static int LastIndex<T>(this T[] array) {
			return array.Length - 1;
		}
		public static T Last<T>(this T[] array) {
			return array[array.Length - 1];
		}
		public static T First<T>(this T[] array) {
			return array[0];
		}
		public static T Random<T>(this T[] array) {
			int a = UnityEngine.Random.Range(0, array.Length);
			return array[a];
		}
		public static int RandomIndex<T>(this T[] array) {
			return UnityEngine.Random.Range(0, array.Length);
		}
		public static int RandomHeadOrTailIndex<T>(this T[] array) {
			return (CoreMethods.Random01() == 0 ? 0 : array.Length - 1);
		}
		public static T RandomHeadOrTail<T>(this T[] array) {
			int index = (CoreMethods.Random01() == 0 ? 0 : array.Length - 1);
			return array[index];
		}
	}
}