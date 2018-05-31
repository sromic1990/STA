using System.Collections;
using System;

using UnityEngine;

using GameAnax.Core.Singleton;

namespace GameAnax.Core.Threader {
	[PersistentSignleton(true, true)]
	public class CoroutineInvoker : SingletonAuto<CoroutineInvoker> {
		// Use this for initialization
		void Awake() { Me = this; }
		public Coroutine Invoke(IEnumerator c) { return StartCoroutine(c); }
		public void Invoke(Action action) { if(null != action) action.Invoke(); }
		public void StopCustomCoroutine(Coroutine c) { StopCoroutine(c); }
	}
}