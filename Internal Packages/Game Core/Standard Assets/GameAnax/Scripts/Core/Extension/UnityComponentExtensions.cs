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
using UnityEngine.UI;


namespace GameAnax.Core.Extension {
	public static class UnityComponentExtensions {
		public static GameObject FindChildGameObject(this GameObject obj, string name) {
			Transform t;
			t = obj.transform.Find(name);
			if(null == t) { return null; } else { return t.gameObject; }
		}
		public static T GetOrAddComponent<T>(this Component self) where T : Component {
			T result = self.GetComponent<T>();
			if(null == result) { result = self.gameObject.AddComponent<T>(); }
			return result;
		}

		public static T GetComponenFromUpword<T>(this Transform tr) {
			if(tr.parent == null) {
				return default(T);
			} else {
				T x = tr.parent.GetComponent<T>();
				if(x != null) {
					return x;
				} else {
					return tr.parent.GetComponenFromUpword<T>();
				}
			}
		}
		public static T GetComponenFromUpword<T>(this GameObject go) {
			return go.transform.GetComponenFromUpword<T>();
		}

		public static GameObject GetParentGameObject(this Component t) {
			return t.GetParentTransform().gameObject;
		}

		public static Transform GetTransform(this Component t) {
			return t.GetComponent<Transform>();
		}
		public static Transform GetParentTransform(this Component t) {
			return t.GetComponent<Transform>().parent;
		}

		public static RectTransform GetRectTransform(this Component t) {
			return t.GetComponent<RectTransform>();
		}
		public static RectTransform GetParentRectTransform(this Component t) {
			return t.GetParentTransform().GetComponent<RectTransform>();
		}

		//public static Renderer GetRenderer(this Component t) {
		//	return t.GetComponent<Renderer>();
		//}
		//public static MeshRenderer GetMeshRenderer(this Component t) {
		//	return t.GetComponent<MeshRenderer>();
		//}
		//public static SkinnedMeshRenderer GetSkinnedMeshRenderer(this Component t) {
		//	return t.GetComponent<SkinnedMeshRenderer>();
		//}
		//public static SpriteRenderer GetSpriteRenderer(this Component t) {
		//	return t.GetComponent<SpriteRenderer>();
		//}
		//public static Image GetImage(this Component t) {
		//	return t.GetComponent<Image>();
		//}
		//public static RawImage GetRawImage(this Component t) {
		//	return t.GetComponent<RawImage>();
		//}

		//public static TextMesh GetTextMesh(this Component t) {
		//	return t.GetComponent<TextMesh>();
		//}
		//public static Text GetText(this Component t) {
		//	return t.GetComponent<Text>();
		//}

		//public static Button GetButton(this Component t) {
		//	return t.GetComponent<Button>();
		//}

		//public static Rigidbody GetRigidbody(this Component t) {
		//	return t.GetComponent<Rigidbody>();
		//}
		//public static Collider GetCollider(this Component t) {
		//	return t.GetComponent<Collider>();
		//}
		//public static BoxCollider GetBoxCollider(this Component t) {
		//	return t.GetComponent<BoxCollider>();
		//}
		//public static SphereCollider GetSphereCollider(this Component t) {
		//	return t.GetComponent<SphereCollider>();
		//}
		//public static CapsuleCollider GetCapsuleCollider(this Component t) {
		//	return t.GetComponent<CapsuleCollider>();
		//}
		//public static MeshCollider GetMeshCollider(this Component t) {
		//	return t.GetComponent<MeshCollider>();
		//}
		//public static WheelCollider GetWheelCollider(this Component t) {
		//	return t.GetComponent<WheelCollider>();
		//}
		//public static TerrainCollider GetTerrainCollider(this Component t) {
		//	return t.GetComponent<TerrainCollider>();
		//}

		//public static Cloth GetCloth(this Component t) {
		//	return t.GetComponent<Cloth>();
		//}

		//public static HingeJoint GetHingeJoint(this Component t) {
		//	return t.GetComponent<HingeJoint>();
		//}
		//public static FixedJoint GetFixedJoint(this Component t) {
		//	return t.GetComponent<FixedJoint>();
		//}
		//public static SpringJoint GetSpringJoint(this Component t) {
		//	return t.GetComponent<SpringJoint>();
		//}
		//public static ConfigurableJoint GetConfigurableJoint(this Component t) {
		//	return t.GetComponent<ConfigurableJoint>();
		//}
		//public static CharacterJoint GetCharacterJoint(this Component t) {
		//	return t.GetComponent<CharacterJoint>();
		//}


		//public static Rigidbody2D GetRigidbody2D(this Component t) {
		//	return t.GetComponent<Rigidbody2D>();
		//}
		//public static Collider2D GetCollider2D(this Component t) {
		//	return t.GetComponent<Collider2D>();
		//}
		//public static BoxCollider2D GetBoxCollider2D(this Component t) {
		//	return t.GetComponent<BoxCollider2D>();
		//}
		//public static CircleCollider2D GetCircleCollider2D(this Component t) {
		//	return t.GetComponent<CircleCollider2D>();
		//}
		//public static PolygonCollider2D GetPolygonCollider2D(this Component t) {
		//	return t.GetComponent<PolygonCollider2D>();
		//}
		//public static EdgeCollider2D GetEdgeCollider2D(this Component t) {
		//	return t.GetComponent<EdgeCollider2D>();
		//}
		//public static CompositeCollider2D GetCompositeCollider2D(this Component t) {
		//	return t.GetComponent<CompositeCollider2D>();
		//}
	}
}

