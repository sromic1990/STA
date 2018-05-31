//
// Coder:			
// EMail:			
// Copyright:		
// Social:			
// 
// Orignal Source :	Somewhere form Unity Answer and Community
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
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

namespace GameAnax.Core.Plugins {
	public class CallbackHelper : MonoBehaviour {
		Queue<Action> actionQueue;
		public CallbackHelper() {
			this.actionQueue = new Queue<Action>();
		}

		public void AddActionToQueue(Action action) {
			object actionQueue = this.actionQueue;
			Monitor.Enter(actionQueue);
			try {
				this.actionQueue.Enqueue(action);
			} finally {
				Monitor.Exit(actionQueue);
			}
		}

		public void DisableIfEmpty() {
			object actionQueue = this.actionQueue;
			Monitor.Enter(actionQueue);
			try {
				if(this.actionQueue.Count == 0) {
					base.enabled = false;
				}
			} finally {
				Monitor.Exit(actionQueue);
			}
		}
	}
}