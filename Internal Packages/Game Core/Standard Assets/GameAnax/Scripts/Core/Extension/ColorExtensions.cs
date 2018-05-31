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

using UnityEngine;
using GameAnax.Core.Utility;


namespace GameAnax.Core.Extension {
	public static class ColorExtension {
		public static string RGBAToHEX(this Color color) {
			string r = color.r.ToByte().ToString("X").PadLeft(2, '0');
			string g = color.g.ToByte().ToString("X").PadLeft(2, '0');
			string b = color.b.ToByte().ToString("X").PadLeft(2, '0');
			string a = color.a.ToByte().ToString("X").PadLeft(2, '0');
			return (r + g + b + a);
		}
		public static string RGBToHEX(this Color color) {
			string rgbhex = RGBAToHEX(color);
			rgbhex = rgbhex.Left(6);
			return rgbhex;
		}

		public static Color Inverse(this Color color) {
			//http://stackoverflow.com/questions/6961725/algorithm-for-calculating-inverse-color
			if(color.a.Equals(1f))
				return color.InvertRGB();
			else
				return new Color(color.a - color.r, color.a - color.g, color.a - color.b, 1f);

		}
		public static Color InvertRGB(this Color color) {
			return new Color(1f - color.r, 1f - color.g, 1f - color.b, 1f);
		}
		public static Color InvertRGBA(this Color color) {
			return new Color(1f - color.r, 1f - color.g, 1f - color.b, 1f - color.a);
		}


		public static Color HexToRGBColor(this string hex) {
			byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			byte a = hex.Length < 8 ? (byte)255 : byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r, g, b, a);
		}

		public static Color SetChannelA(this Color color, float t) {
			t = Mathf.Clamp01(t);
			color.a = t;
			return color;
		}
		public static Color SetChannelR(this Color color, float t) {
			t = Mathf.Clamp01(t);
			color.r = t;
			return color;
		}
		public static Color SetChannelG(this Color color, float t) {
			t = Mathf.Clamp01(t);
			color.g = t;
			return color;
		}
		public static Color SetChannelB(this Color color, float t) {
			t = Mathf.Clamp01(t);
			color.b = t;
			return color;
		}

	}
}