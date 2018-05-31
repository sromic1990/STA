using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using GameAnax.Core.UI.Buttons;
using GameAnax.Core.Extension;


namespace GameAnax.Core.Utility.Popup {
	public class MobilePopup : MonoBehaviour {
		public Action<string> callback;

		public Text tite;
		public Text message;

		public Image image;

		public GameObject contentGroup;

		public Transform buttonGroup;
		public PopupButtonElement buttonPrefab;
		private List<PopupButtonElement> buttonsCreated = new List<PopupButtonElement>();
		public PopupOption option;
		public HorizontalLayoutGroup contentLGroup;
		public VerticalLayoutGroup vContentLGroup;


		//void Awake() { }
		//void Start() { }
		//void OnEnable() { }

		public void UpdateView(PopupOption po) {
			option = po;
			UpdateView();
		}
		public void UpdateView() {
			callback = option.callback;

			tite.text = option.title;
			tite.gameObject.SetActive(!string.IsNullOrEmpty(option.title));

			image.sprite = option.image;
			image.gameObject.SetActive(null != option.image);

			message.alignment = (null != option.image) ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
			message.gameObject.SetActive(!string.IsNullOrEmpty(option.message));
			message.text = option.message.Trim() + "\n";
			message.fontSize = option.fontSize;
			message.color = option.messageColor.HexToRGBColor();
			contentGroup.SetActive(option.image != null || !option.message.Trim().IsNulOrEmpty());

			buttonGroup.gameObject.SetActive((option.buttons != null && option.buttons.Length > 0));
			UpdateButtons();

			//LayoutRebuilder.ForceRebuildLayoutImmediate(contentLGroup.GetRectTransform());
			LayoutRebuilder.ForceRebuildLayoutImmediate(vContentLGroup.GetRectTransform());
		}

		private void UpdateButtons() {
			for(int i = 0; i < buttonsCreated.Count; i++) {
				Destroy(buttonsCreated[i].gameObject);
			}
			buttonsCreated.Clear();
			for(int i = 0; i < option.buttons.Length; i++) {
				PopupButtonElement bi = Instantiate(buttonPrefab, buttonGroup);
				buttonsCreated.Add(bi);
				bi.gameObject.SetActive(true);
				bi.mainText.text = option.buttons[i];
				bi.SetBackdoorInfo(i, option.buttons[i]);
			}
		}

		void UpdaetText() {
			LayoutRebuilder.ForceRebuildLayoutImmediate(vContentLGroup.GetRectTransform());
		}

		private void ButtonClicked(ButtonEventArgs args) {
			string returnData = args.data;
			if(callback != null) callback.Invoke(returnData);

			Destroy(gameObject, 0.1f);
		}
	}
}