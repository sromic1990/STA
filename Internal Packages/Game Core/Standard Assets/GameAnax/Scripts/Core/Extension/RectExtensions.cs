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


namespace GameAnax.Core.Extension {
	public static class RectExtensions {
		public const int SIZE = 8;
		static readonly Vector3[] points = new Vector3[SIZE];
		static readonly Vector3[] pos = new Vector3[SIZE];

		public static Vector2 GetRectToWordlPoint(this Rect rect, Camera cam) {
			Vector2 b = new Vector2(rect.xMin, rect.yMin);
			Vector2 c = new Vector2(rect.xMax, rect.yMax);
			b = cam.ScreenToWorldPoint(b);
			c = cam.ScreenToWorldPoint(c);
			b = c - b;
			return b;
		}
		public static Rect AddRectBuffer(this Rect orignal, Rect buffer) {
			buffer.xMax = Mathf.Clamp(buffer.xMax, -100f, 100f);
			buffer.xMin = Mathf.Clamp(buffer.xMin, -100f, 100f);

			buffer.yMax = Mathf.Clamp(buffer.yMax, -100f, 100f);
			buffer.yMin = Mathf.Clamp(buffer.yMin, -100f, 100f);
			//
			buffer.xMax = (buffer.xMax / 100) * Screen.width;
			buffer.xMin = (buffer.xMin / 100) * Screen.width;

			buffer.yMax = (buffer.yMax / 100) * Screen.height;
			buffer.yMin = (buffer.yMin / 100) * Screen.height;
			Rect objectRect = new Rect(orignal.xMin - buffer.xMin, orignal.yMin - buffer.yMin,
								  orignal.xMax - orignal.xMin + buffer.xMax, orignal.yMax - orignal.yMin + buffer.yMax);

			return objectRect;
		}
		public static Rect GetBoundingRect(this Renderer obj, Camera cam) {
			return GetBoundingRect(obj, cam, new Rect(0, 0, 0, 0));
		}
		public static Rect GetBoundingRect(this Renderer obj, Camera cam, Rect buffer) {
			Rect finalRect = new Rect();
			Rect camRect = new Rect();
			Rect objectRect = new Rect();
			//
			Vector3 center = obj.bounds.center;
			Vector3 extents = obj.bounds.extents;

			pos[0] = new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z);
			pos[1] = new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z);
			pos[2] = new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z);
			pos[3] = new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z);
			pos[4] = new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z);
			pos[5] = new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z);
			pos[6] = new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z);
			pos[7] = new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z);

			for(int i = 0; i < points.Length; i++) {
				points[i] = cam.WorldToScreenPoint(pos[i]);
				points[i].y = Screen.height - points[i].y;
			}

			float xMin, xMax, yMin, yMax;
			xMin = xMax = points[0].x;
			yMin = yMax = points[0].y;

			for(int i = 0; i < points.Length - 1; i++) {
				if(points[i].x < xMin) {
					xMin = points[i].x;
				} else if(points[i].x > xMax) {
					xMax = points[i].x;
				}

				if(points[i].y < yMin) {
					yMin = points[i].y;
				} else if(points[i].y > yMax) {
					yMax = points[i].y;
				}
			}
			objectRect = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);

			objectRect = AddRectBuffer(objectRect, buffer);

			//objectRect = new Rect(xMin - buffer.xMin, yMin - buffer.yMin,
			//	xMax - xMin + buffer.xMax, yMax - yMin + buffer.yMax);

			camRect = cam.pixelRect;
			camRect.y = Screen.height - camRect.yMax;

			finalRect.xMin = objectRect.xMin <= camRect.xMin ? camRect.xMin : objectRect.xMin;
			finalRect.xMax = objectRect.xMax >= camRect.xMax ? camRect.xMax : objectRect.xMax;

			finalRect.yMin = objectRect.yMin <= camRect.yMin ? camRect.yMin : objectRect.yMin;
			finalRect.yMax = objectRect.yMax >= camRect.yMax ? camRect.yMax : objectRect.yMax;

			if(finalRect.xMin >= camRect.xMax || finalRect.xMax <= camRect.xMin) {
				finalRect.xMax = finalRect.xMin = float.MinValue;
			}

			if(finalRect.yMin >= camRect.yMax || finalRect.yMax <= camRect.yMin) {
				finalRect.yMax = finalRect.yMin = float.MinValue;
			}
			return finalRect;
		}

	}
}