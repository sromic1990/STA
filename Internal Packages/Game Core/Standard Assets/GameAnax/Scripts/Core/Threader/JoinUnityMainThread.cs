using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;

namespace GameAnax.Core.Threader {
	[PersistentSignleton(false, true)]
	public class JoinUnityMainThread : SingletonAuto<JoinUnityMainThread> {
		private readonly Queue<Action> _waitingForMainThread = new Queue<Action>();

		// Use this for initialization
		void Awake() {
			Me = this;
		}
		//void Start() { }

		// Update is called once per frame
		void Update() {
			lock(_waitingForMainThread) {
				while(_waitingForMainThread.Count > 0) {
					_waitingForMainThread.Dequeue().Invoke();
				}
			}
		}

		public void Enqueue(IEnumerator action) {
			//MyDebug.Log("Enqueue IEnumerator {0}", action.ToString());
			lock(_waitingForMainThread) {
				_waitingForMainThread.Enqueue(() => {
					StartCoroutine(action);
				});
			}
		}

		public void Enqueue(Action action) {
			//MyDebug.Log("Enqueue action {0}", action.ToString());
			Enqueue(ActionWrapper(action));
		}
		IEnumerator ActionWrapper(Action a) {
			//MyDebug.Log("Executing action {0}", a.ToString());
			a();
			yield return null;
		}
	}
}