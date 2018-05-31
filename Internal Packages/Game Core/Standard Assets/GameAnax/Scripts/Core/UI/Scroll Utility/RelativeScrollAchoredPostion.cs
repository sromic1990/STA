using System.Collections.Generic;
//
using UnityEngine;
using UnityEngine.EventSystems;
//
//
namespace GameAnax.Core.ScrollUtility {
	public class RelativeScrollAchoredPostion : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler {
		private Vector2 _anchorPos;
		[Space(10)]
		public bool isHorizontal;
		public bool isVertical;

		[Space(10)]
		public RectTransform mainRect;
		public List<RectTransform> dependentRects;

		void UpdateAnchoPosition() {
			if(!isHorizontal && !isVertical) { return; }
			dependentRects.ForEach(o => {
				_anchorPos = o.anchoredPosition;
				if(isHorizontal) _anchorPos.x = mainRect.anchoredPosition.x;
				if(isVertical) _anchorPos.y = mainRect.anchoredPosition.y;
				o.anchoredPosition = _anchorPos;
			});
		}

		/// <summary>
		/// Ons the begin drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnBeginDrag(PointerEventData eventData) {
			UpdateAnchoPosition();
		}
		/// <summary>
		/// Ons the end drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag(PointerEventData eventData) {
			UpdateAnchoPosition();
		}
		/// <summary>
		/// Ons the drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnDrag(PointerEventData eventData) {
			UpdateAnchoPosition();
		}

		/// <summary>
		/// Ons the scroll.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnScroll(PointerEventData eventData) {
			UpdateAnchoPosition();
		}
	}
}