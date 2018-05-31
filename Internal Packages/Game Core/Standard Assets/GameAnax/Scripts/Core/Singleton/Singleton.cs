//  
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax
// 
// Orignal Source :	N/A
// Last Modified: 	Ranpariya Ankur
// Contributed By:	N/A
// Curtosey By:		Kleber Lopes da Silva (http://kleber-swf.com/singleton-monobehaviour-unity-projects/)
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

using UnityEngine;

using GameAnax.Core.Utility;



namespace GameAnax.Core.Singleton {
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
		static Type type;
		protected static bool applicationIsQuitting = false;
		protected static object locked = new object();
		static T _me;

		public static T Me {
			get { return Get(); }
			protected internal set { Set(value, SingletonTypes.Precreated); }
		}

		static T FindSingleton() {
			T t = null;
			T[] objects = FindObjectsOfType<T>();
			if(objects.Length > 0) {
				t = objects[0];
				if(objects.Length > 1) {
					Debug.LogWarning("There is more than one instance for [Singleton] of type '" + type +
					"'. Keeping the first named " + objects[0].name + ". Destroying the others.");
					for(var i = 1; i < objects.Length; i++) {
						Destroy(objects[i].gameObject);
					}
				}
			}
			return t;
		}
		static void AutoCreateMe() {
			var gameObject = new GameObject();
			Me = gameObject.AddComponent<T>();
		}
		static void SetPersitant(GameObject go, bool isDontDestroy) {
			if(isDontDestroy) DontDestroyOnLoad(go);
			//MyDebug.Log("Setting Persitant");

		}
		static void SetParentHer(GameObject go) {
			string cName = typeof(T).ToString();
			string[] names = cName.Split('.');
			go.name = names[names.Length - 1];
			if(names.Length <= 1) {
				go.transform.SetParent(null); DontDestroyOnLoad(go); return;
			}

			GameObject go1 = null;
			Transform tr = null;
			string find = "";
			for(int i = 0; i < names.Length - 1; i++) {
				find += names[i];
				go1 = GameObject.Find(find);
				if(null == go1) {
					go1 = new GameObject(names[i]);
					if(null != tr) { go1.transform.SetParent(tr); }
				}
				if(i == 0) { go1.transform.SetParent(null); DontDestroyOnLoad(go1); }
				tr = go1.transform;
				find += "/";
			}
			go.transform.SetParent(tr);
		}

		static Attribute GetParticularAttribute<T1>(object obj) {
			Type type1 = obj.GetType();
			Attribute[] AttributeArray1 = (Attribute[])type1.GetCustomAttributes(typeof(Attribute), false);
			if(AttributeArray1.Length > 0) {
				foreach(Attribute a in AttributeArray1) {
					if(a.GetType().Equals(typeof(T1))) {
						return a;
					}
				}
			}
			return null;
		}


		static PersitantInfo IsPersitant() {
			PersitantInfo pi = new PersitantInfo();
			PersistentSignleton attribute = (PersistentSignleton)GetParticularAttribute<PersistentSignleton>(_me);
			if(null != attribute) {
				//MyDebug.Log("DD: {0}, CH: {1}", attribute.isPersistent, attribute.isChangeHierarchy);
				pi.isDontDestroy = attribute.isPersistent;
				pi.changeHierarchy = attribute.isChangeHierarchy;
			} else {
				//MyDebug.Log("Attribute null");
			}
			return pi;
		}

		static void AutoCreateFromPrefab() {
			SingletonPrefab attribute = (SingletonPrefab)GetParticularAttribute<SingletonPrefab>(_me);
			if(null == attribute) {
				throw new System.Exception("There is no Prefab Atrribute for Singleton of type '" + type + "'.");
			}
			string prefabName = attribute.name;
			if(String.IsNullOrEmpty(prefabName)) {
				throw new System.Exception("Prefab name is empty for Singleton of type '" + type + "'.");
			}

			var gameObject = Instantiate(Resources.Load<GameObject>(prefabName)) as GameObject;
			if(null == gameObject) {
				throw new System.Exception("Could not find Prefab '" + prefabName + "' on Resources for Singleton of type \"" + type + "\".");
			}

			gameObject.name = prefabName;
			T t1 = gameObject.GetComponent<T>();
			if(null == t1) {
				Debug.LogWarning("There wasn't a component of type '" + type + "' on prefab '" + prefabName + "'. Adding new one.");
				Me = gameObject.AddComponent<T>();
			}
		}

		protected static void Set(T value, SingletonTypes sType) {
			if(null == value) {
				if(_me && _me.gameObject) {
					Destroy(_me.gameObject);
				}
				_me = null;
			} else {
				_me = value;
			}

			//MyDebug.Log("Getting Persitatn info");
			PersitantInfo mePi = IsPersitant();
			if(mePi.isDontDestroy && !mePi.changeHierarchy) SetPersitant(_me.gameObject, true);
			if(mePi.changeHierarchy) SetParentHer(_me.gameObject);


		}
		protected static T Get() {
			return Get(SingletonTypes.Precreated);
		}
		protected static T Get(SingletonTypes sType) {
			if(applicationIsQuitting) {
				MyDebug.Warning("[Singleton] Me of '" + typeof(T) +
					"' already destroyed on application quit." +
					" Won't create again - returning null.");
				return null;
			}

			lock(locked) {
				type = typeof(T);
				if(null == _me) {
					T t;
					switch(sType) {
					case SingletonTypes.AutoCraete:
						t = FindSingleton();
						if(null != t) {
							_me = t;
						} else {
							AutoCreateMe();
						}

						break;

					case SingletonTypes.AutoCreateWithPrefab:
						t = FindSingleton();
						if(null != t) {
							_me = t;
						} else {
							AutoCreateFromPrefab();
						}
						break;

					case SingletonTypes.Precreated:
						throw new System.NullReferenceException("[Singleton] " + type.ToString() + " not present in scene");
						break;
					}
					PersitantInfo mePi = IsPersitant();
					if(mePi.isDontDestroy && !mePi.changeHierarchy) SetPersitant(_me.gameObject, true);
					if(mePi.changeHierarchy) SetParentHer(_me.gameObject);
				}
			}
			return _me;
		}

		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed, 
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		protected virtual void OnDestroy() {
			applicationIsQuitting = true;
		}
	}

	public enum SingletonTypes {
		Precreated, AutoCraete, AutoCreateWithPrefab
	}
	public struct PersitantInfo {
		public bool isDontDestroy;
		public bool changeHierarchy;
	}
}