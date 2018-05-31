using System;

using UnityEngine;

namespace GameAnax.Core.Utility.Popup {
	public class PopupOption {
		public string title;
		public string message;
		public Sprite image;
		public string[] buttons;


		public string messageColor;
		public int fontSize;

		public Action<string> callback;

		public PopupOption() {
			messageColor = "2C2B2B";
			fontSize = 50;
		}
	}
}