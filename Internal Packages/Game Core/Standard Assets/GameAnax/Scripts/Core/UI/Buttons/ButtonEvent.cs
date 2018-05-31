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

using GameAnax.Core.NotificationSystem;
using GameAnax.Core.Utility;


namespace GameAnax.Core.UI.Buttons {
	[System.Serializable]
	public class ButtonEvent {
		public delegate void ActionCallBack(ButtonEventArgs args);
		public EventBase baseData;
		public List<ObserverEvent> customEvents;
		ActionCallBack func;

		public void AddListner(ActionCallBack callback, ButtonEventArgs args) { func += (o) => { callback.Invoke(args); }; }
		public void RemoveListner(ActionCallBack callback) { func -= new ActionCallBack(callback); }
		public void RemoveAllListner() { func = null; }
		public void Invoke() {
			if(null != func) {
				if(baseData.isSound) {
					//SFX.Me.AudioPlaySFX(baseData.sfxClipNo);
				}
				func(null);
			}
		}
		Transform[] _childrens;

		public void ExecuteEvents(GameObject container, Component sender) {
			foreach(ObserverEvent b in customEvents) {
				b.eventData.container = container;
				b.eventData.sender = sender;
				switch(baseData.canPause) {
				case PauseEffect.Pause:
					CoreMethods.PauseGame();
					break;

				case PauseEffect.Unpause:
					CoreMethods.UnPauseGame();
					break;
				}
				if(baseData.isSound) {
					//SFX.Me.AudioPlaySFX(cEvent.baseData.sfxClipNo);
				}
				//MyDebug.Log(b.method);
				switch(b.callMethodAt) {
				case MethodCallTypes.Broadcasting:
					NotificationCenter.Me.PostNotification(new NotificationInfo(sender, b.method, b.eventData));
					break;
				case MethodCallTypes.OnlyOnSelf:
					CallMethodOnProviedObject(container.transform, b);
					break;
				case MethodCallTypes.SelfAndChildern:
					CallMethodOnProviedObject(container.transform, b);
					if(null == _childrens) _childrens = container.GetComponentsInChildren<Transform>();


					foreach(Transform child in _childrens) {
						CallMethodOnProviedObject(child, b);
					}
					break;
				case MethodCallTypes.Selected:
					b.selectedRecevicer.ForEach(o => CallMethodOnProviedObject(o, b));
					break;
				}

			}
		}
		void CallMethodOnProviedObject(Component target, ObserverEvent b) {
			//MyDebug.Log("{0} method sent to  {1}", b.method, target.name);
			target.SendMessage(b.method, b.eventData, SendMessageOptions.DontRequireReceiver);

		}
	}

	[System.Serializable]
	public class ObserverEvent {
		public string method = string.Empty;
		public MethodCallTypes callMethodAt = MethodCallTypes.Broadcasting;
		public ButtonEventArgs eventData;
		public List<Component> selectedRecevicer;
	}

	[System.Serializable]
	public class EventBase {
		public bool isSound = false;
		public int sfxClipNo = -1;
		public PauseEffect canPause;
	}

	public enum PauseEffect {
		Unchanged = 0,
		Pause = 1,
		Unpause = 2
	}

	public enum MethodCallTypes {
		Broadcasting,
		OnlyOnSelf,
		SelfAndChildern,
		Selected,
	}
}
