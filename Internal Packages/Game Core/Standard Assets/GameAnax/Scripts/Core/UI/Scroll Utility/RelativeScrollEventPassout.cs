using System.Collections.Generic;
//
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//
//
namespace GameAnax.Core.ScrollUtility {
	[RequireComponent(typeof(ScrollRect))]
	public class RelativeScrollEventPassout : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler {
		private bool isReady = false;
		private ScrollRect _myScrollrect;

		[Space(10)]
		public bool isAutoCall;
		public bool isBlockSelf;
		//
		public bool isOtherHorizontal;
		public bool isOtherVertical;
		//
		public List<ScrollRect> dependentScroller;

		private bool scrollOther; //This tracks if the other one should be scrolling instead of the current one.
		void Awake() {
			//Get the current scroll rect so we can disable it if the other one is scrolling
			_myScrollrect = this.GetComponent<ScrollRect>();
		}

		void Start() {
			if(isAutoCall) OnReady();
		}

		public void OnReady() {
			isReady = true;
		}



		private Vector2 pedDelta;
		/// <summary>
		/// Ons the begin drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnBeginDrag(PointerEventData eventData) {
			if(!isReady) { return; }
			//
			//Get the absolute values of the x and y differences so we can see which one is bigger and scroll the other scroll rect accordingly
			float horizontal = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
			float vertical = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
			pedDelta = eventData.delta;
			if(isOtherHorizontal && horizontal > vertical) {
				scrollOther = true;
				_myScrollrect.enabled = !isBlockSelf;
				//
				pedDelta.y = 0f;
			} else if(isOtherVertical && vertical > horizontal) {
				scrollOther = true;
				_myScrollrect.enabled = !isBlockSelf;
				//
				pedDelta.x = 0f;
			}
			eventData.delta = pedDelta;
			dependentScroller.ForEach(o => o.OnBeginDrag(eventData));
		}
		/// <summary>
		/// Ons the end drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnEndDrag(PointerEventData eventData) {
			if(!isReady) { return; }
			//
			if(scrollOther) {
				pedDelta = eventData.delta;
				if(isOtherHorizontal) {
					pedDelta.y = 0f;
				} else if(isOtherVertical) {
					pedDelta.x = 0f;
				}
				eventData.delta = pedDelta;
				scrollOther = false;
				_myScrollrect.enabled = true;
				dependentScroller.ForEach(o => o.OnEndDrag(eventData));
			}
		}

		/// <summary>
		/// Ons the drag.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnDrag(PointerEventData eventData) {
			if(!isReady) { return; }
			///
			if(scrollOther) {
				pedDelta = eventData.delta;
				if(isOtherHorizontal) {
					pedDelta.y = 0f;
				} else if(isOtherVertical) {
					pedDelta.x = 0f;
				}
				eventData.delta = pedDelta;

				dependentScroller.ForEach(o => o.OnDrag(eventData));
			}
		}
		/// <summary>
		/// Ons the scroll.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnScroll(PointerEventData eventData) {
			if(!isReady) { return; }
			//
			pedDelta = eventData.scrollDelta;
			if(isOtherHorizontal) {
				pedDelta.y = 0f;
			} else if(isOtherVertical) {
				pedDelta.x = 0f;
			}
			eventData.scrollDelta = pedDelta;
			dependentScroller.ForEach(o => o.OnScroll(eventData));
		}

	}
}