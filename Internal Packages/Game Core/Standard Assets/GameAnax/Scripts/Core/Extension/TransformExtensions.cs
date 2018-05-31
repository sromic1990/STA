//  
// Coder:			Amit Kapadi, Ranpariya Ankur {GameAnax} 
// EMail:			amit.kapadi@indianic.com, ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	N/A
// Last Modified: 	Ranpariya Ankur
// Contributed By:	N/A
// Curtosey By:		Amit Kapadi
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
	public static class TransformExtensions {
		public static void SetLPosX(this Transform tr, float x) {
			Vector3 pos = tr.localPosition;
			pos.x = x;
			tr.localPosition = pos;
		}
		public static void SetLPosY(this Transform tr, float y) {
			Vector3 pos = tr.localPosition;
			pos.y = y;
			tr.localPosition = pos;
		}
		public static void SetLPosZ(this Transform tr, float z) {
			Vector3 pos = tr.localPosition;
			pos.z = z;
			tr.localPosition = pos;
		}

		public static void AddLPosX(this Transform tr, float x) {
			Vector3 pos = tr.localPosition;
			pos.x += x;
			tr.localPosition = pos;
		}
		public static void AddLPosY(this Transform tr, float y) {
			Vector3 pos = tr.localPosition;
			pos.y += y;
			tr.localPosition = pos;
		}
		public static void AddLPosZ(this Transform tr, float z) {
			Vector3 pos = tr.localPosition;
			pos.z += z;
			tr.localPosition = pos;
		}


		public static void SetLScaleX(this Transform tr, float x) {
			Vector3 pos = tr.localScale;
			pos.x = x;
			tr.localScale = pos;
		}
		public static void SetLScaleY(this Transform tr, float y) {
			Vector3 pos = tr.localScale;
			pos.y = y;
			tr.localScale = pos;
		}
		public static void SetLScaleZ(this Transform tr, float z) {
			Vector3 pos = tr.localScale;
			pos.z = z;
			tr.localScale = pos;
		}

		public static void AddLScaleX(this Transform tr, float x) {
			Vector3 pos = tr.localScale;
			pos.x += x;
			tr.localScale = pos;
		}
		public static void AddLScaleY(this Transform tr, float y) {
			Vector3 pos = tr.localScale;
			pos.y += y;
			tr.localPosition = pos;
		}
		public static void AddLScaleZ(this Transform tr, float z) {
			Vector3 pos = tr.localScale;
			pos.z += z;
			tr.localScale = pos;
		}

		public static void SetLEulerX(this Transform tr, float x) {
			Vector3 pos = tr.localEulerAngles;
			pos.x = x;
			tr.localEulerAngles = pos;
		}
		public static void SetLEulerY(this Transform tr, float y) {
			Vector3 pos = tr.localEulerAngles;
			pos.y = y;
			tr.localEulerAngles = pos;
		}
		public static void SetLEulerZ(this Transform tr, float z) {
			Vector3 pos = tr.localEulerAngles;
			pos.z = z;
			tr.localEulerAngles = pos;
		}

		public static void AddLEulerX(this Transform tr, float x) {
			Vector3 pos = tr.localEulerAngles;
			pos.x += x;
			tr.localEulerAngles = pos;
		}
		public static void AddLEulerY(this Transform tr, float y) {
			Vector3 pos = tr.localEulerAngles;
			pos.y += y;
			tr.localEulerAngles = pos;
		}
		public static void AddLEulerZ(this Transform tr, float z) {
			Vector3 pos = tr.localEulerAngles;
			pos.z += z;
			tr.localEulerAngles = pos;
		}


		public static float NearestDist(this List<Transform> v3, Transform with) {
			return with.NearestDist(v3.ToArray());
		}
		public static float NearestDist(this Transform with, List<Transform> v3) {
			return with.NearestDist(v3.ToArray());
		}
		public static float NearestDist(this Transform[] v3, Transform with) {
			return with.NearestDist(v3);
		}
		public static float NearestDist(this Transform with, Transform[] v3) {
			float min = float.PositiveInfinity;
			float dist = 0;
			foreach(Transform v in v3) {
				dist = Vector3.Distance(v.position, with.position);
				min = Mathf.Min(min, dist);
			}
			return min;
		}

		public static float FarestDist(this List<Transform> v3, Transform with) {
			return with.FarestDist(v3.ToArray());
		}
		public static float FarestDist(this Transform with, List<Transform> v3) {
			return with.FarestDist(v3.ToArray());
		}
		public static float FarestDist(this Transform[] v3, Transform with) {
			return with.FarestDist(v3);
		}
		public static float FarestDist(this Transform with, Transform[] v3) {
			float max = float.NegativeInfinity;
			float dist = 0;
			foreach(Transform v in v3) {
				dist = Vector3.Distance(v.position, with.position);
				max = Mathf.Max(max, dist);
			}
			return max;
		}

		public static Transform Nearest(this List<Transform> v3, Transform with) {
			return with.Nearest(v3.ToArray());
		}
		public static Transform Nearest(this Transform with, List<Transform> v3) {
			return with.Nearest(v3.ToArray());
		}
		public static Transform Nearest(this Transform[] v3, Transform with) {
			return with.Nearest(v3);
		}
		public static Transform Nearest(this Transform with, Transform[] v3) {
			Transform t = null;
			float min = float.PositiveInfinity;
			float dist = 0;
			foreach(Transform v in v3) {
				dist = Vector3.Distance(v.position, with.position);
				if(min > dist) {
					t = v;
					min = dist;
				}
			}
			return t;
		}

		public static Transform Farest(this List<Transform> v3, Transform with) {
			return with.Farest(v3.ToArray());
		}
		public static Transform Farest(this Transform with, List<Transform> v3) {
			return with.Farest(v3.ToArray());
		}
		public static Transform Farest(this Transform[] v3, Transform with) {
			return with.Farest(v3);
		}
		public static Transform Farest(this Transform with, Transform[] v3) {
			Transform t = null;
			float max = float.NegativeInfinity;
			float dist = 0;
			foreach(Transform v in v3) {
				dist = Vector3.Distance(v.position, with.position);
				if(max < dist) {
					t = v;
					max = dist;
				}
			}
			return t;
		}

	}
}