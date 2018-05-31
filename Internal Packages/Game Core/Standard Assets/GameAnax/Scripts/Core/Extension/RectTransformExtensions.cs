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

using UnityEngine;

using GameAnax.Core.Utility;


namespace GameAnax.Core.Extension {
	public static class RectTransformExtensions {
		public static void SetDefaultScale(this RectTransform trans) {
			trans.localScale = new Vector3(1, 1, 1);
		}

		public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) {
			trans.pivot = aVec;
			trans.anchorMin = aVec;
			trans.anchorMax = aVec;
		}

		public static Vector2 GetSize(this RectTransform trans) {
			return trans.rect.size;
		}

		public static float GetWidth(this RectTransform trans) {
			return trans.rect.width;
		}

		public static float GetHeight(this RectTransform trans) {
			return trans.rect.height;
		}

		public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos) {
			trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
		}

		public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) {
			trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
		}

		public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) {
			trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
		}

		public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) {
			trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
		}

		public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) {
			trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
		}

		public static void SetSize(this RectTransform trans, Vector2 newSize) {
			Vector2 oldSize = trans.rect.size;
			Vector2 deltaSize = newSize - oldSize;
			trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
			trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
		}

		public static void SetWidth(this RectTransform trans, float newSize) {
			SetSize(trans, new Vector2(newSize, trans.rect.size.y));
		}

		public static void SetHeight(this RectTransform trans, float newSize) {
			SetSize(trans, new Vector2(trans.rect.size.x, newSize));
		}

		public static bool IsRange(this int value, int min, int max) {
			return (value >= min && value <= max);
		}

		public static bool IsRange(this float value, float min, float max) {
			return (value >= min && value <= max);
		}

		public static Rect GetWorldCanwasRect(this RectTransform rt) {
			Vector2 pt = rt.pivot;
			Vector2 sz = rt.GetSize();
			Vector2 pos = rt.position;
			Bounds b = RectTransformUtility.CalculateRelativeRectTransformBounds(rt);
			MyDebug.Log(b.min.x + " " + b.min.y + " " + b.max.x + " " + b.max.y);
			Rect r = new Rect();
			r.xMin = pos.x - (pt.x * sz.x);
			r.xMax = pos.x + ((1 - pt.x) * sz.x);

			r.yMin = pos.y - (pt.y * sz.y);
			r.yMax = pos.y + ((1 - pt.y) * sz.y);
			return r;
		}
	}
}