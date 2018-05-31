using UnityEngine;
using UnityEngine.UI;
//
using System.Collections.Generic;
//
namespace GameAnax.Core.ScrollUtility {
	[RequireComponent(typeof(ScrollRect))]
	public class RelativeScrollNormalizedPosition : MonoBehaviour {
		private ScrollRect _myScrollrect;

		[Space(10)]
		public bool isOtherHorizontal;
		public bool isOtherVertical;
		//
		public ScrollRect dependentScroller;


		void Awake() {
			_myScrollrect = this.GetComponent<ScrollRect>();
		}
		void OnEnable() {
			_myScrollrect.onValueChanged.AddListener(OnScrollValueChanged);
		}
		void OnDisable() {
			_myScrollrect.onValueChanged.RemoveListener(OnScrollValueChanged);
		}

		void OnScrollValueChanged(Vector2 normlizedPosition) {
			if(!isOtherHorizontal && !isOtherVertical) { return; }
			if(isOtherHorizontal) dependentScroller.horizontalNormalizedPosition = normlizedPosition.x;
			if(isOtherVertical) dependentScroller.verticalNormalizedPosition = normlizedPosition.y;
		}
	}
}