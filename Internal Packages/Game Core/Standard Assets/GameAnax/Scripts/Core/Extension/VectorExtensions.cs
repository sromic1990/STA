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

using UnityEngine;


namespace GameAnax.Core.Extension {
	public static class VectorExtensions {
		public static Vector2 Cast(this Vector3 v3) {
			return new Vector2(v3.x, v3.y);
		}
		public static Vector3 Cast(this Vector2 v2) {
			return v2.Cast(0);
		}
		public static Vector3 Cast(this Vector2 v2, float z) {
			return new Vector3(v2.x, v2.y, z);
		}

		public static Vector3 Clamp(this Vector3 value, float min, float max) {
			return value.Clamp(new Vector3(min, min, min), new Vector3(max, max, max));
		}
		public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max) {
			return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
		}
		public static Vector2 Clamp(this Vector2 value, float min, float max) {
			return value.Clamp(new Vector2(min, min), new Vector2(max, max));
		}
		public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max) {
			return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
		}

		public static Vector3 SetX(this Vector3 v3, float x) {
			v3.x = x;
			return v3;
		}
		public static Vector3 SetY(this Vector3 v3, float y) {
			v3.y = y;
			return v3;
		}
		public static Vector3 SetZ(this Vector3 v3, float z) {
			v3.z = z;
			return v3;
		}

		public static Vector3 AddInX(this Vector3 v3, float x) {
			v3.x += x;
			return v3;
		}
		public static Vector3 AddInY(this Vector3 v3, float y) {
			v3.y += y;
			return v3;
		}
		public static Vector3 AddInZ(this Vector3 v3, float z) {
			v3.z += z;
			return v3;
		}


		public static float NearestDist(this List<Vector3> v3, Vector3 with) {
			return with.NearestDist(v3.ToArray());
		}
		public static float NearestDist(this Vector3 with, List<Vector3> v3) {
			return with.NearestDist(v3.ToArray());
		}
		public static float NearestDist(this Vector3[] v3, Vector3 with) {
			return with.NearestDist(v3);
		}
		public static float NearestDist(this Vector3 with, Vector3[] v3) {
			float min = float.PositiveInfinity;
			float dist = 0;
			foreach(Vector3 v in v3) {
				dist = Vector3.Distance(v, with);
				min = Mathf.Min(min, dist);
			}
			return min;
		}

		public static float FarestDist(this List<Vector3> v3, Vector3 with) {
			return with.FarestDist(v3.ToArray());
		}
		public static float FarestDist(this Vector3 with, List<Vector3> v3) {
			return with.FarestDist(v3.ToArray());
		}
		public static float FarestDist(this Vector3[] v3, Vector3 with) {
			return with.FarestDist(v3);
		}
		public static float FarestDist(this Vector3 with, Vector3[] v3) {
			float max = float.NegativeInfinity;
			float dist = 0;
			foreach(Vector3 v in v3) {
				dist = Vector3.Distance(v, with);
				max = Mathf.Max(max, dist);
			}
			return max;
		}

		public static Vector3 Nearest(this List<Vector3> v3, Vector3 with) {
			return with.Nearest(v3.ToArray());
		}
		public static Vector3 Nearest(this Vector3 with, List<Vector3> v3) {
			return with.Nearest(v3.ToArray());
		}
		public static Vector3 Nearest(this Vector3[] v3, Vector3 with) {
			return with.Nearest(v3);
		}
		public static Vector3 Nearest(this Vector3 with, Vector3[] v3) {
			Vector3 t = Vector3.zero;
			float min = float.PositiveInfinity;
			float dist = 0;
			foreach(Vector3 v in v3) {
				dist = Vector3.Distance(v, with);
				if(min > dist) {
					t = v;
					min = dist;
				}
			}
			return t;
		}

		public static Vector3 Farest(this List<Vector3> v3, Vector3 with) {
			return with.Farest(v3.ToArray());
		}
		public static Vector3 Farest(this Vector3 with, List<Vector3> v3) {
			return with.Farest(v3.ToArray());
		}
		public static Vector3 Farest(this Vector3[] v3, Vector3 with) {
			return with.Farest(v3);
		}
		public static Vector3 Farest(this Vector3 with, Vector3[] v3) {
			Vector3 t = Vector3.zero;
			float max = float.NegativeInfinity;
			float dist = 0;
			foreach(Vector3 v in v3) {
				dist = Vector3.Distance(v, with);
				if(max < dist) {
					t = v;
					max = dist;
				}
			}
			return t;
		}


	}
}