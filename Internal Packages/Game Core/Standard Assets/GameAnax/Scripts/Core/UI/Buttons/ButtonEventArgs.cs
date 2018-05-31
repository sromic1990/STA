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


namespace GameAnax.Core.UI.Buttons {
	[System.Serializable]
	public class ButtonEventArgs {
		public string data;
		public List<GameObject> extraGameObject;
		public List<object> extraData;
		[HideInInspector]
		public Component sender;
		[HideInInspector]
		public GameObject container;

		public ButtonEventArgs() : this(null, string.Empty, null, null) { }
		public ButtonEventArgs(ButtonEventArgs args) : this(args.sender, args.data, args.container, args.extraGameObject) { }

		public ButtonEventArgs(Component sender) : this(sender, string.Empty, null, null) { }
		public ButtonEventArgs(Component sender, GameObject container) : this(sender, string.Empty, container, null) { }
		public ButtonEventArgs(Component sender, List<GameObject> extra) : this(sender, string.Empty, null, extra) { }

		public ButtonEventArgs(Component sender, string data) : this(sender, data, null, null) { }
		public ButtonEventArgs(Component sender, string data, List<GameObject> extra) : this(sender, data, null, extra) { }
		public ButtonEventArgs(Component sender, string data, GameObject container) : this(sender, data, container, null) { }

		public ButtonEventArgs(Component sender, GameObject container, List<GameObject> extra) : this(sender, string.Empty, container, extra) { }

		public ButtonEventArgs(string data) : this(null, data, null, null) { }
		public ButtonEventArgs(string data, List<GameObject> extra) : this(null, data, null, extra) { }
		public ButtonEventArgs(string data, GameObject container) : this(null, data, container, null) { }
		public ButtonEventArgs(string data, GameObject container, List<GameObject> extra) : this(null, data, container, extra) { }

		public ButtonEventArgs(GameObject container) : this(null, string.Empty, container, null) { }
		public ButtonEventArgs(GameObject container, List<GameObject> extra) : this(null, string.Empty, container, extra) { }

		public ButtonEventArgs(List<GameObject> extra) : this(null, string.Empty, null, extra) { }

		public ButtonEventArgs(Component sender, string data, GameObject container, List<GameObject> extra) {
			this.sender = sender;
			this.data = data;
			this.container = container;
			this.extraGameObject = extra;
		}
	}
}