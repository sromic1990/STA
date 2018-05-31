using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core.Extension;
using GameAnax.Core.UI.Buttons;

public class PopupButtonElement : UIButton {
	public void SetBackdoorInfo(int index, string text) {
		for(int i = 0; i < onClick.customEvents.Count; i++) {
			if(onClick.customEvents[i].eventData.data.IsNulOrEmpty()) {
				onClick.customEvents[i].eventData.data = index + "," + text;
			}
		}
	}
}
