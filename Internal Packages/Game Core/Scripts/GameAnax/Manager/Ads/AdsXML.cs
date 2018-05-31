using UnityEngine;
//
using System;
using System.Collections.Generic;

public class AdLocation {
	public string Name = "";
	public List<StoreInfo> storeInfo = new List<StoreInfo>();
	public StoreInfo GetIdsFor(GameStore store) {
		return storeInfo.Find(o => o.Store.Equals(store));
	}
}
[Serializable]
public class StoreInfo {
	public GameStore Store = GameStore.iOS;
	public Provider ShowAdsFrom = Provider.None;
	public AdType Type = AdType.Interstitial;

	public string CBLoation = "";
	public string AdMobUnitID = "";
	public long InMobi;
	public string AdColonyZone;

	public int showAtEveryFrequency = 1;
	public int requestCount = 0;
}
[Flags]
public enum Provider {
	None,
	AdMob = 1,
	Chartboost = 2,
	AdColony = 4,
	Inmobi = 8
}
[Flags]
public enum GameStore {
	iOS = 1,
	GooglePlay = 1 << 1,

	AppleTV = 1 << 2,
	Amazon = 1 << 3,
	AmazonUnderground = 1 << 4,

	MacApp = 1 << 5,
	WindowsStandalone = 1 << 6,
	LinuxStandAlone = 1 << 7,

	FBGameroomWindows = 1 << 8,
	FBGameroomWebGL = 1 << 9
}
public enum AdType {
	Interstitial,
	RewardVideo,
	MoreApp
}