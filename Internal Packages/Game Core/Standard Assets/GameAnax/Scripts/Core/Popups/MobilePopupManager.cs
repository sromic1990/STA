using System;

using UnityEngine;

using GameAnax.Core.Singleton;


namespace GameAnax.Core.Utility.Popup {
	public class MobilePopupManager : Singleton<MobilePopupManager> {
		[SerializeField]
		private Transform popupParent;
		[SerializeField]
		private MobilePopup popupPrefab;
		// Use this for initialization
		void Awake() {
			Me = this;
		}

		public MobilePopup ShowPopup(PopupOption option) {
			if(string.IsNullOrEmpty(option.message) && null == option.image) {
				throw new ArgumentNullException("message or image");
			}

			MobilePopup mp = Instantiate(popupPrefab, popupParent);
			mp.option = option;
			mp.UpdateView();
			mp.gameObject.SetActive(true);
			return mp;
		}

	}

}