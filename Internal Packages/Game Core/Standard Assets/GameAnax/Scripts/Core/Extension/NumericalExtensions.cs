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

using GameAnax.Core.Utility;


namespace GameAnax.Core.Extension {
	public static class NumericalExtension {
		public static bool Between(this float fact, float minVlaue, float maxValue) {
			if(fact >= minVlaue && fact <= maxValue) {
				return true;
			} else {
				return false;
			}
		}
		public static bool Between(this short fact, short minVlaue, short maxValue) {
			if(fact >= minVlaue && fact <= maxValue) {
				return true;
			} else {
				return false;
			}
		}

		public static bool Between(this double fact, double minVlaue, double maxValue) {
			if(fact >= minVlaue && fact <= maxValue) {
				return true;
			} else {
				return false;
			}
		}
		public static bool Between(this int fact, int minVlaue, int maxValue) {
			if(fact >= minVlaue && fact <= maxValue) {
				return true;
			} else {
				return false;
			}
		}

		public static int RoundUp(this int number, int places) {
			decimal d = (decimal)number;
			return (int)d.RoundUp(places);
		}
		public static int RoundUp(this int number) {

			return (int)number.RoundUp(0);
		}

		public static float RoundUp(this float number, int places) {
			decimal d = (decimal)number;
			return (float)d.RoundUp(places);
		}
		public static float RoundUp(this float number) {
			return (float)number.RoundUp(0);
		}

		public static long RoundUp(this long number, int places) {
			decimal d = (decimal)number;
			return (long)d.RoundUp(places);

		}
		public static long RoundUp(this long number) {
			return (long)number.RoundUp(0);

		}
		public static decimal RoundUp(this decimal number, int places) {
			decimal factor = RoundFactor(places);
			number *= factor;
			number = Math.Ceiling(number);
			number /= factor;
			return number;
		}

		public static int RoundDown(this int number, int places) {
			decimal d = (decimal)number;
			return (int)d.RoundDown(places);
		}
		public static int RoundDown(this int number) {
			return (int)number.RoundDown(0);
		}

		public static float RoundDown(this float number, int places) {
			decimal d = (decimal)number;
			return (float)d.RoundDown(places);
		}
		public static float RoundDown(this float number) {
			return (float)number.RoundDown(0);
		}

		public static long RoundDown(this long number, int places) {
			decimal d = (decimal)number;
			return (long)d.RoundDown(places);
		}
		public static long RoundDown(this long number) {
			return (long)number.RoundDown(0);
		}

		public static decimal RoundDown(this decimal number, int places) {
			decimal factor = RoundFactor(places);
			number *= factor;
			number = Math.Floor(number);
			number /= factor;
			return number;
		}
		static decimal RoundFactor(this int places) {
			decimal factor = 1m;

			if(places < 0) {
				places = -places;
				for(int i = 0; i < places; i++)
					factor /= 10m;
			} else {
				for(int i = 0; i < places; i++)
					factor *= 10m;
			}

			return factor;
		}

		public static float GetPostiveAngle(this float angle) {
			float f = angle;
			f += f < 0f ? 360f : 0f;
			f = f % 360;
			return f;
		}
		public static float Get0to180Angle(this float angle) {
			float f = angle.GetPostiveAngle();
			if(f > 180f)
				f -= 360f;
			return f;
		}

	}
}