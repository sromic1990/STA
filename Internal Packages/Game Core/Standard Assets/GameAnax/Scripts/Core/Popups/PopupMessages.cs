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

using GameAnax.Core.Singleton;


namespace GameAnax.Core.Utility.Popup {
	[PersistentSignleton(true, true)]
	public class PopupMessages : SingletonAuto<PopupMessages> {

		public void NativeKeyboardMessage() {
			PopupOption po = new PopupOption() {
				title = "Opps!",
				message = "Native Keyboard is not support\nfor current platform",
				buttons = new[] { "Close" },
				image = null
			};
			MobilePopupManager.Me.ShowPopup(po);
		}
		public void PushOnMessage() {
			PopupOption po = new PopupOption() {
				title = "Opps!",
				message = "Please turn on notification to\nsend and accept challanges\nfrom friends",
				buttons = new[] { "Close" },
				image = null
			};
			MobilePopupManager.Me.ShowPopup(po);
		}
		public void InternetConnectionMessgae() {
			PopupOption po = new PopupOption() {
				title = "Opps!",
				message = "Please check your\ninternet connection.",
				buttons = new[] { "Close" },
				image = null
			};
			MobilePopupManager.Me.ShowPopup(po);
		}
		public void NoFreeContentMessage() {
			PopupOption po = new PopupOption() {
				title = "Opps!",
				message = "Free content not available yet,\nPlease try later...",
				buttons = new[] { "Close" },
				image = null
			};
			MobilePopupManager.Me.ShowPopup(po);
		}
		public void NativeFuncionNonAvailableMessage() {
			PopupOption po = new PopupOption() {
				title = "Opps!",
				message = "Native functionality not\navailalable in unity editor mode",
				buttons = new[] { "Close" },
				image = null
			};
		}
		//public void NativeKeyboardMessage() {
		//	PopupManager.Me.ShowPopup("Native Keyboard is not support\nfor current platform", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Top, 0.16d);
		//}
		//public void PushOnMessage() {
		//	PopupManager.Me.ShowPopup("Please turn on notification to\nsend and accept challanges\nfrom friends", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		//}
		//public void InternetConnectionMessgae() {
		//	PopupManager.Me.ShowPopup("Oops!", "Please check your\ninternet connection.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		//}
		//public void NoFreeContentMessage() {
		//	PopupManager.Me.ShowPopup("Oops!", "Free content not available yet,\nPlease try later...", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		//}
		//public void NativeFuncionNonAvailableMessage() {
		//	PopupManager.Me.ShowPopup("Oops!", "Native functionality not\navailalable in unity editor mode", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		//}
	}
}