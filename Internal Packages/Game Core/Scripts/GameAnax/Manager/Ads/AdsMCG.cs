using System;
using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core;
using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;
using GameAnax.Core.NotificationSystem;

using GameAnax.Game.CommonSystem;

#if ADMOB
using GoogleMobileAds.Api;
#endif

#if CHARTBOOST
using ChartboostSDK;
#endif
#region prime[31] iOS 'n' Android Manager Settings
#if CHARTBOOSTP31
#if UNITY_IOS
using ChartboostB = Prime31.ChartboostBinding;
using ChartboostM = Prime31.ChartboostManager;
using GameAnax.Core.NotificationSystem;
#endif
#if UNITY_ANDROID
using ChartboostB = Prime31.ChartboostAndroid;
using ChartboostM = Prime31.ChartboostAndroidManager;
#endif
#endif

#if INMOBI
using InMobiM = InMobiManager;
using InMobiB = InMobiBinding;
#endif
#endregion

namespace GameAnax.Game.Utility.Ad {
	[PersistentSignleton(true, true)]
	public class AdsMCG : SingletonAuto<AdsMCG> {
		private bool isFirstTimeShow = true;
#if ADCOLONY
	private string[] adColonyZones;
#endif

#if UNITY_IOS
		public const string CHARTBOOST_APPID = "59a7d3c4f6cd457b0ce0ef7d";
		public const string CHARTBOOST_APPSECRET = "0d23102318a3e1aa4cd40874b65859c3f69ce387";

		public const string ADCOLONY_APPID = "";

		public const string ADMOB_BANNER_ADUNIT_ID = "";
#endif

#if UNITY_ANDROID
		public const string CHARTBOOST_APPID = "59969a0304b0167be761ea3f";
		public const string CHARTBOOST_APPSECRET = "9ea4bad102db6b89eca834c962e976070cbb6405";

		public const string ADCOLONY_APPID = "";

		public const string ADMOB_BANNER_ADUNIT_ID = "ca-app-pub-5424743688820683/6609668594";
#endif


		private List<AdLocation> adLocations;
		[HideInInspector]
		public bool isBannerAdOnScreen = false;
		private bool _isAdsSuccess = false;
		private GameStore _store = GameStore.iOS;
		// Use this for initialization
		void Awake() { Me = this; }
		void Start() {
#if UNITY_IOS
			_store = GameStore.iOS;
#endif
#if UNITY_ANDROID
			_store = GameStore.GooglePlay;
#endif
			#region Ad Ids
			adLocations = new List<AdLocation>();
			adLocations.Add(new AdLocation {
				Name = "GameStart",
				storeInfo = new List<StoreInfo>() {
					new StoreInfo(){
						Store = GameStore.iOS,
						ShowAdsFrom = Provider.AdMob,
						//
						CBLoation = "GameStart",
						AdMobUnitID = "ca-app-pub-5424743688820683/3340989927"
					},
					new StoreInfo(){
						Store = GameStore.GooglePlay,
						ShowAdsFrom = Provider.AdMob,
						//
						CBLoation = "GameStart",
						AdMobUnitID = ""
					}

				}
			});
			adLocations.Add(new AdLocation {
				Name = "GameOver",
				storeInfo = new List<StoreInfo>() {
					new StoreInfo(){
						Store = GameStore.iOS,
						ShowAdsFrom = Provider.AdMob,
						//
						showAtEveryFrequency = 1,
						//
						CBLoation = "GameOver",
						AdMobUnitID = "ca-app-pub-5424743688820683/7312024522"
					},
					new StoreInfo(){
						Store = GameStore.GooglePlay,
						ShowAdsFrom = Provider.AdMob,
						//
						showAtEveryFrequency = 1,
						//
						CBLoation = "GameOver",
						AdMobUnitID = "",
					}
				}
			});
			adLocations.Add(new AdLocation {
				Name = "BackFromBG",
				storeInfo = new List<StoreInfo>() {
					new StoreInfo(){
						Store = GameStore.iOS,
						ShowAdsFrom = Provider.AdMob,
						//
						CBLoation = "BackFromBG",
						AdMobUnitID = "ca-app-pub-5424743688820683/9766797470"
					},
					new StoreInfo(){
						Store = GameStore.GooglePlay,
						ShowAdsFrom = Provider.AdMob,
						//
						CBLoation = "BackFromBG",
						AdMobUnitID = ""
					}
				}
			});
			adLocations.Add(new AdLocation {
				Name = "FreeContinue",
				storeInfo = new List<StoreInfo>() {
					new StoreInfo(){
						Store = GameStore.iOS,
						ShowAdsFrom = Provider.AdMob,
						Type = AdType.RewardVideo,
						//
						CBLoation = "FreeContinue",
						AdMobUnitID = "ca-app-pub-5424743688820683/9126280010"
					},
					new StoreInfo(){
						Store = GameStore.GooglePlay,
						ShowAdsFrom = Provider.AdMob,
						Type = AdType.RewardVideo,
						//
						CBLoation = "FreeContinue",
						AdMobUnitID = ""
					}
				}
			});
			//adLocations.Add(new AdLocation {
			//	Name = "Pause",
			//	storeInfo = new List<StoreInfo>() {
			//		new StoreInfo(){
			//			Store = GameStore.iOS,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Pause",
			//			AdMobUnitID = "",
			//			AdColonyZone = ""
			//		},
			//		new StoreInfo(){
			//			Store = GameStore.GooglePlay,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Pause",
			//			AdMobUnitID = ""
			//		}
			//	}
			//});
			//adLocations.Add(new AdLocation {
			//	Name = "Main Menu",
			//	storeInfo = new List<StoreInfo>() {
			//		new StoreInfo(){
			//			Store = GameStore.iOS,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Main Menu",
			//			AdMobUnitID = ""
			//		},
			//		new StoreInfo(){
			//			Store = GameStore.GooglePlay,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Main Menu",
			//			AdMobUnitID = ""
			//		}
			//	}
			//});
			//adLocations.Add(new AdLocation {
			//	Name = "Game Win",
			//	storeInfo = new List<StoreInfo>() {
			//		new StoreInfo(){
			//			Store = GameStore.iOS,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Game Win",
			//			AdMobUnitID = ""
			//		},
			//		new StoreInfo(){
			//			Store = GameStore.GooglePlay,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Game Win",
			//			AdMobUnitID = ""
			//		}
			//	}
			//});
			//adLocations.Add(new AdLocation {
			//	Name = "Change Mode",
			//	storeInfo = new List<StoreInfo>() {
			//		new StoreInfo(){
			//			Store = GameStore.iOS,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Change Mode",
			//			AdMobUnitID = ""
			//		},
			//		new StoreInfo(){
			//			Store = GameStore.GooglePlay,
			//			ShowAdsFrom = Provider.AdMob,
			//			//
			//			CBLoation = "Change Mode",
			//			AdMobUnitID = ""
			//		}
			//	}
			//});		
			//adLocations.Add(new AdLocation {
			//	Name = "FreeCash",
			//	storeInfo = new List<StoreInfo>() {
			//		new StoreInfo(){
			//			Store = GameStore.iOS,
			//			ShowAdsFrom = Provider.AdMob,
			//			Type = AdType.RewardVideo,
			//			//
			//			CBLoation = "FreeCash",
			//			AdMobUnitID = ""
			//		},
			//		new StoreInfo(){
			//			Store = GameStore.GooglePlay,
			//			ShowAdsFrom = Provider.AdMob,
			//			Type = AdType.RewardVideo,
			//			//
			//			CBLoation = "FreeCash",
			//			AdMobUnitID = ""
			//		}
			//	}
			//});
			//adLocations.Add(new AdLocation {
			//	Name = "FreeLevel",
			//	storeInfo = new List<StoreInfo>() {
			//		new StoreInfo(){
			//			Store = GameStore.iOS,
			//			ShowAdsFrom = Provider.AdMob,
			//			Type = AdType.RewardVideo,
			//			//
			//			CBLoation = "FreeLevel",
			//			AdMobUnitID = ""
			//		},
			//		new StoreInfo(){
			//			Store = GameStore.GooglePlay,
			//			ShowAdsFrom = Provider.AdMob,
			//			Type = AdType.RewardVideo,
			//			//
			//			CBLoation = "FreeLevel",
			//			AdMobUnitID = ""
			//		}
			//	}
			//});
			#endregion

			#region Chartboost SDK
#if CHARTBOOST
			//Chartboost.setAutoCacheAds(true);
			//Chartboost.setMediation(CBMediation.AdMob, "1.0");
			Chartboost.CreateWithAppId(CHARTBOOST_APPID, CHARTBOOST_APPSECRET);
#endif
			#endregion

			#region AdColony
#if ADCOLONY
		adColonyZones = GetAdColonhyZone ();
		MyDebug.Log ("------------------");
		MyDebug.Log (adColonyZones.toJson ());
		AdColony.Configure (GUtility.Me.AppVersion.ToString (), ADCOLONY_APPID, adColonyZones);
#endif
			#endregion

			StoreInfo storeInfo;
			foreach(AdLocation al in adLocations) {
				storeInfo = al.GetIdsFor(_store);
				if(null == storeInfo) continue;
				if(storeInfo.Type.Equals(AdType.Interstitial)) {
					CacheAdMob(al);
					//TODO: Commnet Bellow line CacheCBInter before final upload.
					//CacheCBInterstitial(al.CBLoation);
				}
				if(storeInfo.Type.Equals(AdType.RewardVideo)) {
					CacheAdMobRewardAd(al);
					//TODO: Commnet Bellow line CacheCBInter before final upload.
					//CacheCBRewardAd(al.CBLoation);
				}
			}
			InvokeRepeating("ShowGameStartAds", 5f, 10f);
		}

		void OnEnable() {
			#region Google Mobile Ads
#if ADMOB
			// Google AdMob for Untiy Events
			GoogleMobileAdsScript.Me.BannerAdLoaded += GMABannerAdLoaded;
			GoogleMobileAdsScript.Me.BannerAdFailedToLoad += GMABannerAdFailedToLoad;
			GoogleMobileAdsScript.Me.BannerAdOpening += GMABannerAdOpening;
			GoogleMobileAdsScript.Me.BannerAdClosed += GMABannerAdClosed;
			GoogleMobileAdsScript.Me.BannerAdLeftApplication += GMABannerAdLeftApplication;

			GoogleMobileAdsScript.Me.InterstitialLoaded += GMAInterstitialLoaded;
			GoogleMobileAdsScript.Me.InterstitialFailedToLoad += GMAInterstitialFailedToLoad;
			GoogleMobileAdsScript.Me.BannerAdOpening += GMAInterstitialOpening;
			GoogleMobileAdsScript.Me.InterstitialClosed += GMAInterstitialClosed;
			GoogleMobileAdsScript.Me.InterstitialLeftApplication += GMAInterstitialLeftApplication;

			GoogleMobileAdsScript.Me.RewardAdLoaded += GMARewardAdLoaded;
			GoogleMobileAdsScript.Me.RewardAdFailedToLoad += GMARewardAdFailedToLoad;
			GoogleMobileAdsScript.Me.RewardAdOpening += GMARewardAdOpened;
			GoogleMobileAdsScript.Me.RewardAdStarted += GMARewardAdStarted;
			GoogleMobileAdsScript.Me.RewardAdRewarded += GMARewardAdewarded;
			GoogleMobileAdsScript.Me.RewardAdClosed += GMARewardAdClosed;
			GoogleMobileAdsScript.Me.RewardAdLeftApplication += GMARewardAdLeftApplication;
#endif
			#endregion

			#region Chartboost SDK
#if CHARTBOOST
			Chartboost.didInitialize += CBDidInitialize;

			Chartboost.didFailToLoadInterstitial += CBDidFailToLoadInterstitial;
			Chartboost.didDismissInterstitial += CBDidDismissInterstitial;
			Chartboost.didCloseInterstitial += CBDidCloseInterstitial;
			Chartboost.didClickInterstitial += CBDidClickInterstitial;
			Chartboost.didCacheInterstitial += CBDidCacheInterstitial;
			Chartboost.shouldDisplayInterstitial += CBShouldDisplayInterstitial;
			Chartboost.didDisplayInterstitial += CBDidDisplayInterstitial;

			Chartboost.didFailToRecordClick += CBDidFailToRecordClick;
			Chartboost.didFailToLoadRewardedVideo += CBDidFailToLoadRewardedVideo;
			Chartboost.didDismissRewardedVideo += CBDidDismissRewardedVideo;
			Chartboost.didCloseRewardedVideo += CBDidCloseRewardedVideo;
			Chartboost.didClickRewardedVideo += CBDidClickRewardedVideo;
			Chartboost.didCacheRewardedVideo += CBDidCacheRewardedVideo;
			Chartboost.shouldDisplayRewardedVideo += CBShouldDisplayRewardedVideo;
			Chartboost.didCompleteRewardedVideo += CBDidCompleteRewardedVideo;
			Chartboost.didDisplayRewardedVideo += CBDidDisplayRewardedVideo;

			Chartboost.didCacheInPlay += CBDidCacheInPlay;
			Chartboost.didFailToLoadInPlay += CBDidFailToLoadInPlay;
			Chartboost.didPauseClickForConfirmation += CBDidPauseClickForConfirmation;
			Chartboost.willDisplayVideo += CBWillDisplayVideo;
#if UNITY_IPHONE
			Chartboost.didCompleteAppStoreSheetFlow += CBDidCompleteAppStoreSheetFlow;
#endif
#endif
			#endregion

			#region AdColony
#if ADCOLONY && (UNITY_IOS || UNITY_ANDROID)
		//AdColony
		AdColony.OnVideoStarted += OnVideoStarted;
		AdColony.OnVideoFinished += OnVideoFinished;
		AdColony.OnV4VCResult += OnV4VCResult;
		AdColony.OnAdAvailabilityChange += OnAdAvailabilityChange;
#endif
			#endregion

			#region InMobi
#if INMOBI
#if UNITY_IOS
		InMobiM.interstitialDidReceiveAdEvent += InMobiInterstitialDidReceiveAd;
		InMobiM.interstitialDidFailToReceiveAdWithErrorEvent += InMobiInterstitialDidFailToReceiveAdWithError;
		InMobiM.interstitialWillPresentScreenEvent += InMobiInterstitialWillPresentScreen;
		InMobiM.interstitialDidPresentScreenEvent += InMobiInterstitialDidPresentScreen;
		InMobiM.interstitialWillDismissScreenEvent += InMobiInterstitialWillDismissScreen;
		InMobiM.interstitialDidDismissScreenEvent += InMobiInterstitialDidDismissScreen;
		InMobiM.interstitialWillLeaveApplicationEvent += InMobiInterstitialWillLeaveApplication;
		InMobiM.interstitialDidInteractEvent += InMobiInterstitialDidInteract;
		InMobiM.interstitialDidFailToPresentScreenWithErrorEvent += InMobiInterstitialDidFailToPresentScreenWithError;
		InMobiM.interstitialRewardActionCompletedWithRewardsEvent += InMobiInterstitialRewardActionCompletedWithRewards;

		InMobiM.bannerDidReceiveAdEvent += InMobiBannerDidReceiveAd;
		InMobiM.bannerDidFailToReceiveAdWithErrorEvent += InMobiBannerDidFailToReceiveAdWithError;
		InMobiM.bannerDidInteractEvent += InMobiBannerDidInteract;
		InMobiM.bannerWillPresentScreenEvent += InMobiBannerWillPresentScreen;
		InMobiM.bannerDidPresentScreenEvent += InMobiBannerDidPresentScreen;
		InMobiM.bannerWillDismissScreenEvent += InMobiBannerWillDismissScreen;
		InMobiM.bannerDidDismissScreenEvent += InMobiBannerDidDismissScreen;
		InMobiM.bannerWillLeaveApplicationEvent += InMobiBannerWillLeaveApplication;
		InMobiM.bannerRewardActionCompletedWithRewardsEvent += InMobiBannerRewardActionCompletedWithRewards;
#endif
#endif
			#endregion
		}
		void OnDisable() {
			#region Google Mobile Ads Script
#if ADMOB
			// Google AdMob for Untiy Events
			GoogleMobileAdsScript.Me.BannerAdLoaded -= GMABannerAdLoaded;
			GoogleMobileAdsScript.Me.BannerAdFailedToLoad -= GMABannerAdFailedToLoad;
			GoogleMobileAdsScript.Me.BannerAdOpening -= GMABannerAdOpening;
			GoogleMobileAdsScript.Me.BannerAdClosed -= GMABannerAdClosed;
			GoogleMobileAdsScript.Me.BannerAdLeftApplication -= GMABannerAdLeftApplication;

			GoogleMobileAdsScript.Me.InterstitialLoaded -= GMAInterstitialLoaded;
			GoogleMobileAdsScript.Me.InterstitialFailedToLoad -= GMAInterstitialFailedToLoad;
			GoogleMobileAdsScript.Me.InterstitialOpening -= GMAInterstitialOpening;
			GoogleMobileAdsScript.Me.InterstitialClosed -= GMAInterstitialClosed;
			GoogleMobileAdsScript.Me.InterstitialLeftApplication -= GMAInterstitialLeftApplication;


			GoogleMobileAdsScript.Me.RewardAdLoaded -= GMARewardAdLoaded;
			GoogleMobileAdsScript.Me.RewardAdFailedToLoad -= GMARewardAdFailedToLoad;
			GoogleMobileAdsScript.Me.RewardAdOpening -= GMARewardAdOpened;
			GoogleMobileAdsScript.Me.RewardAdStarted -= GMARewardAdStarted;
			GoogleMobileAdsScript.Me.RewardAdRewarded -= GMARewardAdewarded;
			GoogleMobileAdsScript.Me.RewardAdClosed -= GMARewardAdClosed;
			GoogleMobileAdsScript.Me.RewardAdLeftApplication -= GMARewardAdLeftApplication;
#endif
			#endregion

			#region Chartboost SDK
#if CHARTBOOST
			Chartboost.didInitialize += CBDidInitialize;

			Chartboost.didFailToLoadInterstitial += CBDidFailToLoadInterstitial;
			Chartboost.didDismissInterstitial += CBDidDismissInterstitial;
			Chartboost.didCloseInterstitial += CBDidCloseInterstitial;
			Chartboost.didClickInterstitial += CBDidClickInterstitial;
			Chartboost.didCacheInterstitial += CBDidCacheInterstitial;
			Chartboost.shouldDisplayInterstitial += CBShouldDisplayInterstitial;
			Chartboost.didDisplayInterstitial += CBDidDisplayInterstitial;

			Chartboost.didFailToRecordClick += CBDidFailToRecordClick;
			Chartboost.didFailToLoadRewardedVideo += CBDidFailToLoadRewardedVideo;
			Chartboost.didDismissRewardedVideo += CBDidDismissRewardedVideo;
			Chartboost.didCloseRewardedVideo += CBDidCloseRewardedVideo;
			Chartboost.didClickRewardedVideo += CBDidClickRewardedVideo;
			Chartboost.didCacheRewardedVideo += CBDidCacheRewardedVideo;
			Chartboost.shouldDisplayRewardedVideo += CBShouldDisplayRewardedVideo;
			Chartboost.didCompleteRewardedVideo += CBDidCompleteRewardedVideo;
			Chartboost.didDisplayRewardedVideo += CBDidDisplayRewardedVideo;

			Chartboost.didCacheInPlay += CBDidCacheInPlay;
			Chartboost.didFailToLoadInPlay += CBDidFailToLoadInPlay;
			Chartboost.didPauseClickForConfirmation += CBDidPauseClickForConfirmation;
			Chartboost.willDisplayVideo += CBWillDisplayVideo;
#if UNITY_IPHONE
			Chartboost.didCompleteAppStoreSheetFlow += CBDidCompleteAppStoreSheetFlow;
#endif
#endif
			#endregion

			#region AdColony
#if ADCOLONY && (UNITY_IOS || UNITY_ANDROID)
		//AdColony
		AdColony.OnVideoStarted -= OnVideoStarted;
		AdColony.OnVideoFinished -= OnVideoFinished;
		AdColony.OnV4VCResult -= OnV4VCResult;
		AdColony.OnAdAvailabilityChange -= OnAdAvailabilityChange;
#endif
			#endregion

			#region InMobi
#if INMOBI
#if UNITY_IOS
		InMobiM.interstitialDidReceiveAdEvent -= InMobiInterstitialDidReceiveAd;
		InMobiM.interstitialDidFailToReceiveAdWithErrorEvent -= InMobiInterstitialDidFailToReceiveAdWithError;
		InMobiM.interstitialWillPresentScreenEvent -= InMobiInterstitialWillPresentScreen;
		InMobiM.interstitialDidPresentScreenEvent -= InMobiInterstitialDidPresentScreen;
		InMobiM.interstitialWillDismissScreenEvent -= InMobiInterstitialWillDismissScreen;
		InMobiM.interstitialDidDismissScreenEvent -= InMobiInterstitialDidDismissScreen;
		InMobiM.interstitialWillLeaveApplicationEvent -= InMobiInterstitialWillLeaveApplication;
		InMobiM.interstitialDidInteractEvent -= InMobiInterstitialDidInteract;
		InMobiM.interstitialDidFailToPresentScreenWithErrorEvent -= InMobiInterstitialDidFailToPresentScreenWithError;
		InMobiM.interstitialRewardActionCompletedWithRewardsEvent -= InMobiInterstitialRewardActionCompletedWithRewards;

		InMobiM.bannerDidReceiveAdEvent -= InMobiBannerDidReceiveAd;
		InMobiM.bannerDidFailToReceiveAdWithErrorEvent -= InMobiBannerDidFailToReceiveAdWithError;
		InMobiM.bannerDidInteractEvent -= InMobiBannerDidInteract;
		InMobiM.bannerWillPresentScreenEvent -= InMobiBannerWillPresentScreen;
		InMobiM.bannerDidPresentScreenEvent -= InMobiBannerDidPresentScreen;
		InMobiM.bannerWillDismissScreenEvent -= InMobiBannerWillDismissScreen;
		InMobiM.bannerDidDismissScreenEvent -= InMobiBannerDidDismissScreen;
		InMobiM.bannerWillLeaveApplicationEvent -= InMobiBannerWillLeaveApplication;
		InMobiM.bannerRewardActionCompletedWithRewardsEvent -= InMobiBannerRewardActionCompletedWithRewards;
#endif
#endif
			#endregion
		}

		private DateTime _lastBGAddShown;
		void OnApplicationPause(bool isPaused) {
			if(!CoreUtility.Me.isGameStarted) {
				return;
			}
			if(!isPaused) {
				if(!CoreMethods.gameStatus.Equals(GamePlayState.Gameplay) && !CoreMethods.gameStatus.Equals(GamePlayState.Pause) &&
				   DateTime.UtcNow > _lastBGAddShown.AddSeconds(GameUtility.Me.bgAdsSecAfter)) {
					MyDebug.Log("Last BackFromBG Call at: " + _lastBGAddShown.ToString("O") + " " + DateTime.UtcNow.ToString("O"));
					ShowAd("BackFromBG");
				}
			}
			if(isPaused) GameUtility.Me.SavePlayerData();
		}

		private string[] GetAdColonhyZone() {
			List<string> zones = new List<string>();
			foreach(AdLocation al1 in adLocations) {
				StoreInfo si = al1.GetIdsFor(_store);
				if(null == si) continue;
				if(!zones.Contains(si.AdColonyZone) && !string.IsNullOrEmpty(si.AdColonyZone)) {
					zones.Add(si.AdColonyZone);
				}
			}
			zones.ForEach(z => MyDebug.Log(false, z));
			return zones.ToArray();
		}
		private AdLocation GetPlacement(string placemnetName) {
			AdLocation al = new AdLocation();
			foreach(AdLocation al1 in adLocations) {
				if(al1.Name.Equals(placemnetName, StringComparison.OrdinalIgnoreCase)) {
					al = al1;
				}
			}
			return al;
		}
#if !ADMOB
		public void ShowBannerAd() {
#endif
#if ADMOB
		public void ShowBannerAd(AdPosition position = AdPosition.Top) {
#endif

			if(GameUtility.Me.playerData.isRemoveAds) {
				MyDebug.Log("AdsMCG::ShowAds => Remove Ad purchsed");
				return;
			}
#if ADMOB
			GoogleMobileAdsScript.Me.ShowBanner(ADMOB_BANNER_ADUNIT_ID, position);
#endif
		}
		public void HideBannerAd() {
			isBannerAdOnScreen = false;
			//TODO: invole delgate or event to adjust Menus for Banner Ad			
#if ADMOB
			GoogleMobileAdsScript.Me.HideBanner(ADMOB_BANNER_ADUNIT_ID);
#endif
		}

		public bool IsAdAvailable(string placement) {
			bool retValue = false;
			if(GameUtility.Me.playerData.isRemoveAds && !placement.ToLower().Contains("free")) {
				MyDebug.Log("AdsMCG::IsAdAvailable => Remove Ad purchsed");
				return false;
			}

			AdLocation al = GetPlacement(placement);
			StoreInfo storeInfo = al.GetIdsFor(_store);
			if(null == storeInfo) {
				MyDebug.Log("AdsMCG::IsAdsAvailable => There are not info of {0} store for {1} location", _store, al.Name);
				return false;
			}

			MyDebug.Log(false, "AdsMCG::IsAdsAvailable => Is Ads Available Called");
			switch(storeInfo.Type) {
			case AdType.Interstitial:
				retValue = IsInterstitialAvailable(storeInfo);
				break;

			case AdType.RewardVideo:
				retValue = IsRewardAdAvailable(storeInfo);
				break;

			case AdType.MoreApp:
				retValue = false;
				MyDebug.Warning(false, "AdsMCG::IsAdsAvailable => More App Wall Is not Integrated");
				break;
			}
			return retValue;
		}
		public void ShowAd(string placement) {
			if(GameUtility.Me.playerData.isRemoveAds && !placement.ToLower().Contains("free")) {
				MyDebug.Log("AdsMCG::ShowAd => Remove Ad purchsed");
				return;
			}

			if(CoreUtility.Me.settings.gameLaunchCount < GameUtility.Me.noAdUntilGameLunch
			|| CoreUtility.Me.settings.gamePlayCount < GameUtility.Me.noAdUntilGamePlay
			|| CoreUtility.Me.settings.gameOverCount < GameUtility.Me.noAdUntilGameOver) {
				MyDebug.Log("AdsMCG::ShowAds => Ad will not show becasue noAd rule applied\n" +
					"launch count: {0}, launch need: {1},\nplay count; {2}, play need: {3},\nover Count: {4}, over need: {5}\n\n",
					CoreUtility.Me.settings.gameLaunchCount,
					GameUtility.Me.noAdUntilGameLunch, CoreUtility.Me.settings.gamePlayCount, GameUtility.Me.noAdUntilGamePlay,
					CoreUtility.Me.settings.gamePlayCount, GameUtility.Me.noAdUntilGameOver);
				return;
			}
			if(!IsAdAvailable(placement)) {
				MyDebug.Log(false, "AdsMCG::ShowAd => ad not available for location {0}", placement);
				return;
			}

			AdLocation al = GetPlacement(placement);
			StoreInfo storeInfo = al.GetIdsFor(_store);
			if(null == storeInfo) {
				MyDebug.Log(false, "AdsMCG::ShowAd => There are not info of {0} store for {1} location", _store, al.Name);
				return;
			}

			storeInfo.requestCount++;
			if(storeInfo.requestCount % storeInfo.showAtEveryFrequency != 0) {
				MyDebug.Log(false, "AdsMCG::ShowAds => display Frequency not match for placement " + al.Name);
				return;
			}
			MyDebug.Log(false, "AdsMCG::ShowAds => Show Ad Called");
			switch(storeInfo.Type) {
			case AdType.Interstitial:
				ShowInterstitialAd(storeInfo);
				break;

			case AdType.RewardVideo:
				ShowRewardAd(storeInfo);
				break;

			case AdType.MoreApp:
				MyDebug.Warning(false, "AdsMCG::ShowAds => More App Wall Is not Integrated");
				break;
			}
		}



		private bool IsInterstitialAvailable(StoreInfo al) {
			bool retValue = false;
			MyDebug.Log(false, "AdsMCG::IsInterstitialAvailable => Is Interstitial Ad Available Called");

			switch(al.ShowAdsFrom) {
#if ADCOLONY
			case Provider.AdColony:
				string rawStatus, status;
				rawStatus = AdColony.StatusForZone (al.AdColonyZone);
				status = string.Format ("Zone {0} status is {1}", al.AdColonyZone, rawStatus);
				MyDebug.Log (false,status);
				retValue =AdColony.IsVideoAvailable (al.AdColonyZone);
				break;
#endif

#if ADMOB
			case Provider.AdMob:
				retValue = GoogleMobileAdsScript.Me.IsInterstitialReady(al.AdMobUnitID);
				//if(!retValue) GoogleMobileAdsScript.Me.RequestInterstitial(al.AdMobUnitID);
				break;
#endif

#if CHARTBOOST
			case Provider.Chartboost:
				retValue = Chartboost.hasInterstitial(new CBLocation(al.CBLoation));
				if(!retValue) Chartboost.cacheInterstitial(new CBLocation(al.CBLoation));
				break;
#endif
			case Provider.Inmobi:
			default:
				break;
			}
			return retValue;
		}
		private void ShowInterstitialAd(StoreInfo al) {
			MyDebug.Log(false, "AdsMCG::ShowInterstitialAd => Show Interstitial Ad Called");
			switch(al.ShowAdsFrom) {
#if ADCOLONY
			case Provider.AdColony:
				string rawStatus, status;
				rawStatus = AdColony.StatusForZone (al.AdColonyZone);
				status = string.Format ("Zone {0} status is {1}", al.AdColonyZone, rawStatus);
				MyDebug.Log (false,status);
				if (AdColony.IsVideoAvailable (al.AdColonyZone)) {
					ShowAdColonyAds (al.AdColonyZone);
				}
				break;
#endif

#if ADMOB
			case Provider.AdMob:
				if(GoogleMobileAdsScript.Me.IsInterstitialReady(al.AdMobUnitID)) {
					ShowAdMobAds(al.AdMobUnitID);
				}
				break;
#endif

#if CHARTBOOST
			case Provider.Chartboost:
				if(Chartboost.hasInterstitial(new CBLocation(al.CBLoation))) {
					ShowChartboostAds(al.CBLoation);
				}
				break;
#endif
			case Provider.Inmobi:
				break;

			}
		}

		private bool IsRewardAdAvailable(StoreInfo al) {
			bool retValue = false;
			MyDebug.Log(false, "AdsMCG::IsRewardAdAvailable => IsReward Ad Available Called");

			switch(al.ShowAdsFrom) {
#if ADCOLONY
			case Provider.AdColony:
				string rawStatus, status;
				rawStatus = AdColony.StatusForZone (al.AdColonyZone);
				status = string.Format ("Zone {0} status is {1}", al.AdColonyZone, rawStatus);
				MyDebug.Log (false,status);
				retValue = AdColony.IsVideoAvailable (al.AdColonyZone);
				break;
#endif

#if ADMOB
			case Provider.AdMob:
				retValue = GoogleMobileAdsScript.Me.IsRewardVideoAdReady(al.AdMobUnitID);
				//if(!retValue) GoogleMobileAdsScript.Me.RequestRewardVideoAd(al.AdMobUnitID);
				break;
#endif

#if CHARTBOOST
			case Provider.Chartboost:
				retValue = Chartboost.hasRewardedVideo(new CBLocation(al.CBLoation));
				if(!retValue) Chartboost.cacheRewardedVideo(new CBLocation(al.CBLoation));
				break;
#endif
			case Provider.Inmobi:
			default:
				break;
			}
			return retValue;
		}
		private void ShowRewardAd(StoreInfo al) {
			MyDebug.Log(false, "AdsMCG::ShowRewardAd => Show Reward Ads Called");
			switch(al.ShowAdsFrom) {
#if ADCOLONY
			case Provider.AdColony:
				string rawStatus, status;
				rawStatus = AdColony.StatusForZone (al.AdColonyZone);
				status = string.Format ("Zone {0} status is {1}", al.AdColonyZone, rawStatus);
				MyDebug.Log (false,status);
				if (AdColony.IsVideoAvailable (al.AdColonyZone)) {
					ShowAdColonyAds (al.AdColonyZone);
				} else {
					NotificationCenter.Me.PostNotification (new Notification (this, "AdsFail", "AdColony"));
				}
				break;
#endif

#if ADMOB
			case Provider.AdMob:
				if(GoogleMobileAdsScript.Me.IsRewardVideoAdReady(al.AdMobUnitID)) {
					ShowAdMobRewardAds(al.AdMobUnitID);
				} else {
					NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsFail", "AdMob"));
				}
				break;
#endif

#if CHARTBOOST
			case Provider.Chartboost:
				if(Chartboost.hasRewardedVideo(new CBLocation(al.CBLoation))) {
					ShowCBRewardAd(al.CBLoation);
				} else {
					NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsFail", "Chartboost"));
				}
				break;
#endif
			case Provider.Inmobi:
				break;

			}
		}

		private void ShowGameStartAds() {
			if(GameUtility.Me.playerData.isRemoveAds) {
				MyDebug.Log(false, "AdsMCG::IsInterstitialAvailable => Remove Ad purchsed");
				CancelInvoke("ShowGameStartAds");
				return;
			}
			if(!isFirstTimeShow || !CoreMethods.gameStatus.Equals(GamePlayState.MainMenu) ||
				CoreUtility.Me.settings.gameLaunchCount < GameUtility.Me.adAfterGameStart) {
				return;
			}
			if(IsAdAvailable("GameStart")) {
				CancelInvoke("ShowGameStartAds");
				isFirstTimeShow = false;
				ShowAd("GameStart");
			}
		}

		#region Google Mobile Ads (AdMob) Events

		private void ShowAdMobAds(string adUnitID) {
#if ADMOB
			if(GoogleMobileAdsScript.Me.IsInterstitialReady(adUnitID)) {
				MyDebug.Log(false, "AdsMCG::ShowAdMobAds => Showing AdMob Interstitial for {0}", adUnitID);
				GoogleMobileAdsScript.Me.ShowInterstitial(adUnitID);
			} else {
				MyDebug.Log(false, "AdsMCG::ShowAdMobAds => AdMob Interstitial is not ready");
			}
#endif
		}

		private void CacheAdMob(AdLocation al) {
			if(GameUtility.Me.playerData.isRemoveAds && !al.Name.ToLower().Contains("free")) {
				MyDebug.Log(false, "AdsMCG::CacheAdMobRewardAd => Remove Ad purchsed");
				return;
			}
			StoreInfo si = al.GetIdsFor(_store);
			CacheAdMob(si.AdMobUnitID);
		}
		private void CacheAdMob(string adUnitId) {
#if ADMOB
			MyDebug.Log(false, "AdsMCG::CacheAdMob => Requesting Admob Interstatial for: {0}", adUnitId);
			GoogleMobileAdsScript.Me.RequestInterstitial(adUnitId);
#endif
		}

		private void CacheAdMobRewardAd(AdLocation al) {
			StoreInfo si = al.GetIdsFor(_store);
			CacheAdMobRewardAd(si.AdMobUnitID);
		}
		private void CacheAdMobRewardAd(string adUnitId) {
#if ADMOB
			MyDebug.Log(false, "AdsMCG::CacheAdMobRewardAd => Requesting Admob Interstatial for: {0}", adUnitId);
			GoogleMobileAdsScript.Me.RequestRewardVideoAd(adUnitId);
#endif
		}
		private void ShowAdMobRewardAds(string adUnitID) {
#if ADMOB
			if(GoogleMobileAdsScript.Me.IsRewardVideoAdReady(adUnitID)) {
				MyDebug.Log(false, "AdsMCG::ShowAdMobRewardAds => Showing AdMob Interstitial for {0}", adUnitID);
				GoogleMobileAdsScript.Me.ShowRewardVideoAd(adUnitID);
			} else {
				MyDebug.Log(false, "AdsMCG::ShowAdMobRewardAds => AdMob Interstitial is not ready");
			}
#endif
		}

#if ADMOB
		// Google AdMob for Untiy Events
		private void GMABannerAdLoaded(string adUnitID) {
			isBannerAdOnScreen = true;
			if(GoogleMobileAdsScript.Me.isHideBanner) {
				HideBannerAd();
			}
			MenuManager.Me.AdJustTopBottomBarForBannerAds();
		}
		private void GMABannerAdFailedToLoad(string adUnitID, string error) {
			isBannerAdOnScreen = false;
			MenuManager.Me.AdJustTopBottomBarForBannerAds();
		}
		private void GMABannerAdOpening(string adUnitID) { }
		private void GMABannerAdClosing(string adUnitID) { }
		private void GMABannerAdClosed(string adUnitID) { }
		private void GMABannerAdLeftApplication(string adUnitID) { }

		private void GMAInterstitialLoaded(string adUnitID) { }
		private void GMAInterstitialFailedToLoad(string adUnitID, string error) { }
		private void GMAInterstitialOpening(string adUnitID) {
			AdLocation al = GetPlacement("GameStart");
			StoreInfo si = al.GetIdsFor(_store);
			if(si.AdMobUnitID.Equals(adUnitID)) {
				isFirstTimeShow = false;
				MyDebug.Log(false, "Game Start Ads Shown at " + DateTime.UtcNow.ToString("O"));
			}
		}
		private void GMAInterstitialClosing(string adUnitID) { }
		private void GMAInterstitialClosed(string adUnitID) {
			AdLocation al = GetPlacement("BackFromBG");
			StoreInfo si = al.GetIdsFor(_store);
			if(si.AdMobUnitID.Equals(adUnitID)) {
				MyDebug.Log(false, "Back From BG Ads Shown at " + DateTime.UtcNow.ToString("O"));
				_lastBGAddShown = DateTime.UtcNow;
			}
			CacheAdMob(adUnitID);
		}
		private void GMAInterstitialLeftApplication(string adUnitID) { }

		private void GMARewardAdLoaded(string adUnitID) { }
		private void GMARewardAdFailedToLoad(string adUnitID, string error) { }
		private void GMARewardAdOpened(string adUnitID) {
			_isAdsSuccess = false;
		}
		private void GMARewardAdStarted(string adUnitID) { }
		private void GMARewardAdewarded(string adUnitID, Reward args) {
			_isAdsSuccess = true;
		}
		private void GMARewardAdClosed(string adUnitID) {
			MyDebug.Log(false, "AdsMCG::GMARewardAdClosed => isAddSuccess = " + _isAdsSuccess);
			if(_isAdsSuccess)
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsSuccess", "AdMob"));
			else
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsCancelled", "AdMob"));
			_isAdsSuccess = false;
			CacheAdMobRewardAd(adUnitID);
		}
		private void GMARewardAdLeftApplication(string adUnitID) { }
#endif
		#endregion

		#region Chartboost SDK
#if CHARTBOOST
		private CBLocation cbLocation;
#endif
		private void ShowChartboostAds(string placement) {
#if CHARTBOOST
			cbLocation = new CBLocation(placement);
			if(Chartboost.hasInterstitial(cbLocation)) {
				MyDebug.Log(false, "AdsMCG::ShowChartboostAds => Showing CB Interstitial for " + placement);
				Chartboost.showInterstitial(cbLocation);
			} else {
				MyDebug.Log(false, "AdsMCG::ShowChartboostAds => Cache not Interstitial");
			}
#endif
		}
		private void CacheCBInterstitial(string placement) {
#if CHARTBOOST
			cbLocation = new CBLocation(placement);
			if(GameUtility.Me.playerData.isRemoveAds && !placement.ToLower().Contains("free")) {
				MyDebug.Log(false, "AdsMCG::CacheCBInterstitial => Remove Ad purchsed");
				return;
			}
			MyDebug.Log(false, "AdsMCG::CacheCBInterstitial => Caching CB Interstitial for " + placement);
			Chartboost.cacheInterstitial(cbLocation);
#endif
		}
		private void ShowCBRewardAd(string placement) {
#if CHARTBOOST
			cbLocation = new CBLocation(placement);
			if(Chartboost.hasRewardedVideo(cbLocation)) {
				Chartboost.showRewardedVideo(cbLocation);
			} else {
				MyDebug.Log(false, "AdsMCG::ShowCBRewardAd => Cache not Interstitialed");
			}
#endif
		}
		private void CacheCBRewardAd(string placement) {
#if CHARTBOOST
			cbLocation = new CBLocation(placement);
			Chartboost.cacheRewardedVideo(cbLocation);
#endif
		}

#if CHARTBOOST
		void CBDidInitialize(bool status) {
			MyDebug.Log(false, "AdsMGS::CBDidInitialize: {0}", status);
		}

		void CBDidFailToLoadInterstitial(CBLocation location, CBImpressionError error) {
			MyDebug.Log(false, "AdsMGS::CBDidFailToLoadInterstitial: {0} at location {1}", error, location);
		}
		void CBDidDismissInterstitial(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidDismissInterstitial: " + location);
		}
		void CBDidCloseInterstitial(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidCloseInterstitial: " + location);

			AdLocation al = GetPlacement("BackFromBG");
			StoreInfo si = al.GetIdsFor(_store);
			if(si.CBLoation.Equals(location.ToString())) {
				MyDebug.Log(false, "Back From BG Ads Shown at " + DateTime.UtcNow.ToString("O"));
				_lastBGAddShown = DateTime.UtcNow;
			}
		}
		void CBDidClickInterstitial(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidClickInterstitial: " + location);
		}
		void CBDidCacheInterstitial(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidCacheInterstitial: " + location);
		}
		bool CBShouldDisplayInterstitial(CBLocation location) {
			// return true if you want to allow the interstitial to be displayed
			MyDebug.Log(false, "AdsMGS::CBShouldDisplayInterstitial @" + location);
			return true;
		}
		void CBDidDisplayInterstitial(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidDisplayInterstitial: " + location);
			AdLocation al = GetPlacement("GameStart");
			StoreInfo si = al.GetIdsFor(_store);
			if(si.CBLoation.Equals(location.ToString())) {
				isFirstTimeShow = false;
				MyDebug.Log(false, "Game Start Ads Shown at " + DateTime.UtcNow.ToString("O"));
			}
		}

		void CBDidFailToRecordClick(CBLocation location, CBClickError error) {
			MyDebug.Log(false, "AdsMGS::CBDidFailToRecordClick: {0} at location: {1}", error, location);
		}

		void CBDidFailToLoadRewardedVideo(CBLocation location, CBImpressionError error) {
			MyDebug.Log(false, "AdsMGS::CBDidFailToLoadRewardedVideo: {0} at location {1}", error, location);
		}
		void CBDidDismissRewardedVideo(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidDismissRewardedVideo: " + location);
		}
		void CBDidCloseRewardedVideo(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidCloseRewardedVideo: " + location);
			if(_isAdsSuccess)
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsSuccess", "AdMob"));
			else
				NotificationCenter.Me.PostNotification(new NotificationInfo(this, "AdsCancelled", "AdMob"));
			_isAdsSuccess = false;
		}
		void CBDidClickRewardedVideo(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidClickRewardedVideo: " + location);
		}
		void CBDidCacheRewardedVideo(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidCacheRewardedVideo: " + location);
		}
		bool CBShouldDisplayRewardedVideo(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBShouldDisplayRewardedVideo @" + location);
			return true;
		}
		void CBDidCompleteRewardedVideo(CBLocation location, int reward) {
			MyDebug.Log(false, "AdsMGS::CBDidCompleteRewardedVideo: reward {0} at location {1}", reward, location);
			_isAdsSuccess = true;

		}
		void CBDidDisplayRewardedVideo(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidDisplayRewardedVideo: " + location);
			_isAdsSuccess = false;
		}

		void CBDidCacheInPlay(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBDidCacheInPlay called: " + location);
		}
		void CBDidFailToLoadInPlay(CBLocation location, CBImpressionError error) {
			MyDebug.Log(false, "AdsMGS::CBDidFailToLoadInPlay: {0} at location: {1}", error, location);
		}

		void CBDidPauseClickForConfirmation() {
#if UNITY_IPHONE
			MyDebug.Log(false, "AdsMGS::CBDidPauseClickForConfirmation called");
			//activeAgeGate = true;
#endif
		}
		void CBWillDisplayVideo(CBLocation location) {
			MyDebug.Log(false, "AdsMGS::CBWillDisplayVideo: " + location);
		}
		void CBDidCompleteAppStoreSheetFlow() {
#if UNITY_IPHONE
			MyDebug.Log(false, "AdsMGS::CBDidCompleteAppStoreSheetFlow");
#endif
		}
#endif
		#endregion

		#region AdColony
		private void ShowAdColonyAds(string zoneID) {
#if ADCOLONY
		if (AdColony.IsVideoAvailable (zoneID)) {
			AdColony.ShowVideoAd (zoneID);
		} else {
			MyDebug.Log (false,"AdsMCG::ShowAdColonyAds => Cache not Interstitialed");
			NotificationCenter.Me.PostNotification (new Notification (this, "AdsFail", "AdColony"));
		}
#endif
		}

#if ADCOLONY && (UNITY_IOS || UNITY_ANDROID)
	private void OnVideoStarted ()
	{
		MyDebug.Log (false,"AdsMCG::OnVideoStarted ==> On Video Started");
		GameEngine.PauseGame (true);
	}
	private void OnVideoFinished (bool isAdShown)
	{
		MyDebug.Log (false,"AdsMCG::OnVideoFinished ==> On Video Finished, and Ad was shown: " + isAdShown);

		if (!isAdShown) {
			NotificationCenter.Me.PostNotification (new Notification (this, "AdsFail", "AdColony"));
		} else {
			NotificationCenter.Me.PostNotification (new Notification (this, "AdsSuccess", "AdColony"));			
		}
		GameEngine.UnPauseGame ();
	}
	// The V4VCResult Delegate assigned in Start, AdColony calls this after confirming V4VC transactions with your server
	// success - true: transaction completed, virtual currency awarded by your server - false: transaction failed, no virtual currency awarded
	// name - The name of your virtual currency, defined in your AdColony account
	// amount - The amount of virtual currency awarded for watching the video, defined in your AdColony account
	private void OnV4VCResult (bool success, string currencyName, int amount)
	{
		if (success) {
			MyDebug.Log (false,"AdsMCG::OnV4VCResult ==> V4VC SUCCESS: name = " + currencyName + ", amount = " + amount);
			GUtility.Me.PProgress.Cash += amount;
			GUtility.Me.SavePProgres ();
		} else {
			MyDebug.LogWarning (false,"AdsMCG::OnV4VCResult ==> V4VC FAILED!");
		}
	}
	private void OnAdAvailabilityChange (bool avail, string zoneID)
	{
		MyDebug.Log (false,"AdsMCG::OnAdAvailabilityChange ==> Ad Availability Changed to available=" + avail + " for zone: " + zoneID);
	}
#endif

		#endregion

		#region "Inmobi"
		private void ShowInMobi(long placement, string key = "GameOver") {
#if INMOBI
		if(InMobiB.isInterstitialReady(key)) {
		InMobiB.presentInterstitial(key);
		} else {
		MyDebug.Log(false,"AdsMCG::ShowInMobi => Cache not Interstitialed");
		}
#endif
			CacheInMobi(key, placement);
		}

		private void CacheInMobi(string key, long placement) {
			if(GameUtility.Me.playerData.isRemoveAds && !key.ToLower().Contains("free")) {
				MyDebug.Log(false, "AdsMCG::CacheInMobi => Remove Ad purchsed");
				return;
			}
#if INMOBI
		InMobiB.loadInterstitial(key, placement);
#endif
		}
#if INMOBI
	void InMobiInterstitialDidReceiveAd() {
		MyDebug.Logfalse,("GUtilitu:: => InMobiInterstitialDidReceiveAd");
	}
	void InMobiInterstitialDidFailToReceiveAdWithError(string error) {
		MyDebug.Log(false,"GUtilitu:: =>  InMobiInterstitialDidFailToReceiveAdWithError: " + error);
	}
	void InMobiInterstitialWillPresentScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialWillPresentScreen");
	}
	void InMobiInterstitialDidPresentScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialDidPresentScreen");
	}
	void InMobiInterstitialWillDismissScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialWillDismissScreen");
	}
	void InMobiInterstitialDidDismissScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialDidDismissScreen");
	}
	void InMobiInterstitialWillLeaveApplication() {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialWillLeaveApplication");
	}
	void InMobiInterstitialDidInteract(Dictionary<string,object> param) {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialDidInteract");
		MyDebug.Log(param);
	}
	void InMobiInterstitialDidFailToPresentScreenWithError(string error) {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialDidFailToPresentScreenWithError: " + error);
	}
	void InMobiInterstitialRewardActionCompletedWithRewards(Dictionary<string,object> param) {
		MyDebug.Log(false,"GUtilitu:: => InMobiInterstitialRewardActionCompletedWithRewards");
		MyDebug.Log(param);
	}
	
	void InMobiBannerRewardActionCompletedWithRewards(Dictionary<string,object> param) {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerRewardActionCompletedWithRewards");
		MyDebug.Log(param);
	}
	void InMobiBannerDidReceiveAd() {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerDidReceiveAd");
	}
	void InMobiBannerDidFailToReceiveAdWithError(string error) {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerDidFailToReceiveAdWithError: " + error);
	}
	void InMobiBannerDidInteract(Dictionary<string,object> param) {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerDidInteract");
		MyDebug.Log(param);
	}
	void InMobiBannerWillPresentScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerWillPresentScreen");
	}
	void InMobiBannerDidPresentScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerDidPresentScreen");
	}
	void InMobiBannerWillDismissScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerWillDismissScreen");
	}
	void InMobiBannerDidDismissScreen() {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerDidDismissScreen");
	}
	void InMobiBannerWillLeaveApplication() {
		MyDebug.Log(false,"GUtilitu:: => InMobiBannerWillLeaveApplication");
	}
#endif
		#endregion
	}
}