using UnityEngine;

using GameAnax.Core.Singleton;

#if ADJUST
using com.adjust.sdk;
#endif

namespace GameAnax.Game.Utility.Analytic {
	[PersistentSignleton(true, true)]
	public class GoogleTracking : SingletonAuto<GoogleTracking> {
		private float dTime = 0;
#if GOOGLE_ANALYTICS
	[SerializeField]
	private GoogleAnalyticsV4 googleAnalytics;
#endif
		[SerializeField]
		private float dispathcAfterEvery = 0.25f;

		// Use this for initialization
		void Awake() { Me = this; }
		void Start() {
#if GOOGLE_ANALYTICS
		googleAnalytics.StartSession();
#endif
		}

		// Update is called once per frame
		void Update() {
			dTime += Time.unscaledDeltaTime;
			if(dTime >= dispathcAfterEvery) {
				dTime = 0f;
#if GOOGLE_ANALYTICS
			googleAnalytics.DispatchHits();
#endif
			}
		}

		public void SendGoogleEvent(string eventCategory, string eventAction, string eventLable) {
			SendGoogleEvent(eventCategory, eventAction, eventLable, (long)1f);
		}

		public void SendGoogleEvent(string eventCategory, string eventAction, string eventLable, long value) {
#if GOOGLE_ANALYTICS
		googleAnalytics.LogEvent(eventCategory, eventAction, eventLable, value);
#endif
		}

		public void TrackInAppEvent(string eventLable, long value) {
#if GOOGLE_ANALYTICS
		googleAnalytics.LogEvent("In App", "Purchased", eventLable, value);
#endif
		}

		public void LogScreen(string title) {
#if GOOGLE_ANALYTICS
		googleAnalytics.LogScreen(title);
#endif
		}

		public void SendAdjustEvent(string eventToken) {
#if ADJUST
		Adjust.trackEvent(new AdjustEvent(eventToken));
#endif
		}
	}
}