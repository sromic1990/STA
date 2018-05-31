//
// Coder:			Ranpariya Ankur {GameAnax} 
// EMail:			ankur.ranpariya@indianic.com
// Copyright:		GameAnax Studio Pvt Ltd	
// Social:			http://www.gameanax.com, @GameAnax, https://www.facebook.com/@gameanax	
// 
// Orignal Source :	N/A
// Last Modified: 	Ranpariya Ankur
// Contributed By:	N/A
// Curtosey By:		N/A
// 
// All rights reserved.
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
// following conditions are met:
// 
//  *	Redistributions of source code must retain the above copyright notice, this list of conditions and the following
//  	disclaimer.
//  *	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
//  	disclaimer in the documentation and/or other materials provided with the distribution.
//  *	Neither the name of the [ORGANIZATION] nor the names of its contributors may be used to endorse or promote products
//  	derived from this software without specific prior written permission.
// 
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
//

using System;
using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core.NotificationSystem;
using GameAnax.Core.Singleton;
using GameAnax.Core.UI.Buttons;
using GameAnax.Core.Utility;

using GameAnax.Game.Utility.Ad;
using GameAnax.Game.Utility.Popup;

#if UNITY_ANDROID && AMAZONSTORE
using com.amazon.device.iap.cpt;
#endif

//using Prime31;


namespace GameAnax.Game.Utility {
	[PersistentSignleton(true, true)]
	public class InAppManager : Singleton<InAppManager> {
		//	Menus lastLayer = 0;
		string pID;
		bool isRestore = false;
		bool isRestoreItemSuccess = false;

		public List<InAppDetail> InAppInfo = new List<InAppDetail>();
		string[] inappids = new string[] { };

		[SerializeField]
		bool isLocalStoreCost;
		public int BestDealIndex;
		public GameObject BestDealBadge;
		public List<Vector2> BestBadgePosition;
		public List<TextMesh> Cost;
		public List<TextMesh> Reward;

		#region iOS => Apple Store
		bool isProductReceived = false;
#if UNITY_IOS
#endif
		#endregion

		#region Android => Google Play Store
#if UNITY_ANDROID
		//duckoff.underground.removeads
		const string NON_CONSU_PRODUCTS = ",balldance.removeads,";
#endif
		#endregion

		#region Android => Amazon Store
#if UNITY_ANDROID && AMAZONSTORE
	// Obtain object used to interact with plugin
	bool isSupported = false;
	string marketPlace = "";
	List<string> unavailableSkus = new List<string>();
	IAmazonIapV2 iapService;
	string supportedMarketplaces = string.Empty;
#endif
		#endregion

		#region Mono Actions
		// Use this for initialization
		void Awake() {
			Me = this;
			NotificationCenter.Me.AddObserver(this, "RestorePurchase");
			NotificationCenter.Me.AddObserver(this, "PurchaseItem");

			isRestore = false;
			isRestoreItemSuccess = false;
			MyDebug.Log("InAppManager::Awake => isRestore? " + isRestore);
#if UNITY_ANDROID && AMAZONSTORE
		iapService = AmazonIapV2Impl.Instance;
#endif
		}
		void Start() {
			GetInAppIdsArrayForIOS();

			#region iOS Methods
#if UNITY_IOS && STOREKIT && !UNITY_EDITOR
		InvokeRepeating("RequestProduct", 0f, 15f);
#endif
			#endregion
			//
			#region Android => Google Play Mathods
#if UNITY_ANDROID && !AMAZONSTORE && !UNITY_EDITOR && GOOGLEIAB
			GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAg1PP4V7cVb2kBIfWYcLYxx9OAXkT6/1EbfOxyI+hkvJN07mP/U1pmdfn8QIvCnCHA6gual2H1tUOu4DHUtwISnVWwSZqzNPYMsgVHEDMV+nC9/K09k+PNkL8PUaSRfg/wcUYyjOlRBNPXgNdzJ+KSYPrsFxdcY5pIdQYzYoO8kOCTfhJFTukPF+vtXMjtZBt2QppgMWtqSOGL79qk5buv9q15YLG6HN0XBTrK+dE2/2KbDZA58tiMMaGTqTTmHFRWZxtkvdosUf5xVofSsoR/3pevhJ9nZ2nhnFjzQiHUooKeUmCrsOzzWk6e80rXCPwrfWJaa1RKe0fgXz2wqCi8QIDAQAB");
#endif
			#endregion
			//
			#region Android => Amazon Store Methods
#if UNITY_ANDROID && AMAZONSTORE && !UNITY_EDITOR
		iapService.GetUserData();
		SkusInput sin = new SkusInput();
		sin.Skus = new List<string>();
		supportedMarketplaces = "USA, UK, Germany, Frane,\nSpain, Italy, Japan, Canada, Brazil, Australia";
#if UNDERGROUND
		supportedMarketplaces = "USA, UK, Germany, Frane";
#endif
		string sku = string.Empty;
		foreach(string s in InAppIDs) {
			sku = s;
#if UNDERGROUND
			sku = sku.Replace("supercell.", "supercell.underground.");
#endif
			sin.Skus.Add(sku);
		}
		iapService.GetProductData(sin);
#endif
			#endregion
		}

		void OnEnable() {
#if UNITY_IOS && STOREKIT
			StoreKitManager.productListReceivedEvent += ASKProductListReceived;

			StoreKitManager.purchaseSuccessfulEvent += ASKPurchaseSuccessful;
			StoreKitManager.purchaseFailedEvent += ASKPurchaseFailed;
			StoreKitManager.purchaseCancelledEvent += ASKPurchaseCancel;

			StoreKitManager.restoreTransactionsFailedEvent += ASKRestoreTransactionsFailed;
			StoreKitManager.restoreTransactionsFinishedEvent += ASKRestoreTransactionsFinished;
#endif
#if UNITY_ANDROID && !AMAZONSTORE && GOOGLEIAB
			GoogleIABManager.billingSupportedEvent += IABBillingSupported;
			GoogleIABManager.billingNotSupportedEvent += IABBillingNotSupported;
			GoogleIABManager.queryInventorySucceededEvent += IABQueryInventorySucceeded;
			GoogleIABManager.queryInventoryFailedEvent += IABQueryInventoryFailed;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += IABPurchaseCompleteAwaitingVerification;
			GoogleIABManager.purchaseSucceededEvent += IABPurchaseSucceeded;
			GoogleIABManager.purchaseFailedEvent += IABPurchaseFailed;
			GoogleIABManager.consumePurchaseSucceededEvent += IABConsumePurchaseSucceeded;
			GoogleIABManager.consumePurchaseFailedEvent += IABConsumePurchaseFailed;
#endif
#if UNITY_ANDROID && AMAZONSTORE
		// Register for an event
		iapService.AddGetPurchaseUpdatesResponseListener(OnPurchaseUpdatesResponse);
		iapService.AddGetProductDataResponseListener(OnProductDataResponse);
		iapService.AddPurchaseResponseListener(OnPurchaseResponse);
		iapService.AddGetUserDataResponseListener(OnUserDataResponse);
#endif
		}
		void OnDisable() {
#if UNITY_IOS && STOREKIT
			StoreKitManager.productListReceivedEvent -= ASKProductListReceived;

			StoreKitManager.purchaseSuccessfulEvent -= ASKPurchaseSuccessful;
			StoreKitManager.purchaseFailedEvent -= ASKPurchaseFailed;
			StoreKitManager.purchaseCancelledEvent -= ASKPurchaseCancel;

			StoreKitManager.restoreTransactionsFailedEvent -= ASKRestoreTransactionsFailed;
			StoreKitManager.restoreTransactionsFinishedEvent -= ASKRestoreTransactionsFinished;
#endif
#if UNITY_ANDROID && !AMAZONSTORE && GOOGLEIAB
			GoogleIABManager.billingSupportedEvent -= IABBillingSupported;
			GoogleIABManager.billingNotSupportedEvent -= IABBillingNotSupported;
			GoogleIABManager.queryInventorySucceededEvent -= IABQueryInventorySucceeded;
			GoogleIABManager.queryInventoryFailedEvent -= IABQueryInventoryFailed;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= IABPurchaseCompleteAwaitingVerification;
			GoogleIABManager.purchaseSucceededEvent -= IABPurchaseSucceeded;
			GoogleIABManager.purchaseFailedEvent -= IABPurchaseFailed;
			GoogleIABManager.consumePurchaseSucceededEvent -= IABConsumePurchaseSucceeded;
			GoogleIABManager.consumePurchaseFailedEvent -= IABConsumePurchaseFailed;
#endif
#if UNITY_ANDROID && AMAZONSTORE
		// Unregister for an event
		iapService.RemoveGetPurchaseUpdatesResponseListener(OnPurchaseUpdatesResponse);
		iapService.RemoveGetProductDataResponseListener(OnProductDataResponse);
		iapService.RemovePurchaseResponseListener(OnPurchaseResponse);
		iapService.RemoveGetUserDataResponseListener(OnUserDataResponse);
#endif

		}
		#endregion
		//
		public void RestorePurchase() {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}
#if(UNITY_IOS && !STOREKIT) || (UNITY_ANDROID && !GOOGLEIAB)
			PopupMessages.Me.InAppNotIntegratedMessage();
			return;
#endif

			isRestore = true;
			isRestoreItemSuccess = false;
			PopupMessages.Me.RestoreProcessMessage();

			#region iOS Methods
#if UNITY_IOS && STOREKIT
#if !UNITY_EDITOR
		StoreKitBinding.restoreCompletedTransactions();
#endif
#if UNITY_EDITOR
			ASKRestoreTransactionsFailed("Not works in editor mode");
#endif
#endif
			#endregion
			//
			#region Android => Google Play Mathods
#if UNITY_ANDROID && !AMAZONSTORE && GOOGLEIAB
#if !UNITY_EDITOR
		GoogleIAB.queryInventory(inappids);
#endif
#if UNITY_EDITOR
			IABQueryInventoryFailed("Not works in editor mode");
#endif
#endif
			#endregion
			//
			#region Android => Amazon Store Methods
#if UNITY_ANDROID && !UNITY_EDITOR && AMAZONSTORE
#if !UNITY_EDITOR
		ResetInput resetInput = new ResetInput();
		resetInput.Reset = false;
		iapService.GetPurchaseUpdates(resetInput);
#endif
#if UNITY_EDITOR
		MarketPlaceNotSuported();
#endif
#endif
			#endregion
		}
		public void PurchaseItem(ButtonEventArgs args) {
			if(!CoreUtility.Me.ShowInternetConnection()) {
				return;
			}

#if(UNITY_IOS && !STOREKIT) || (UNITY_ANDROID && !GOOGLEIAB)
			PopupMessages.Me.InAppNotIntegratedMessage();
			return;
#endif

			string coststring;
			int index;
			int.TryParse(args.data, out index);

			pID = InAppInfo[index].ID;
			isRestore = false;
			isRestoreItemSuccess = false;
			coststring = isProductReceived && isLocalStoreCost ? InAppInfo[index].CostStringLocaleBase : "$" + InAppInfo[index].CostUSD;
			MyDebug.Log("InAppManager::PurchaseItem => " + pID + ", " + index + ", " + InAppInfo[index].DisplayName + ", " + coststring);
			PopupMessages.Me.InAppProcessMessage(InAppInfo[index].DisplayName, coststring);

#if UNITY_EDITOR
			PurchaseSuccessful(pID, "UnityTest_" + System.DateTime.Now.ToOADate());
			return;
#endif

			#region iOS Methods
#if UNITY_IOS && !UNITY_EDITOR && STOREKIT
		StoreKitBinding.purchaseProduct(pID, 1);
#endif
			#endregion

			#region Android => Google Play Mathods
#if UNITY_ANDROID && !UNITY_EDITOR && !AMAZONSTORE && GOOGLEIAB
		GoogleIAB.queryInventory(inappids);
#endif
			#endregion

			#region Android => Amazon Store Methods
#if UNITY_ANDROID && !UNITY_EDITOR && AMAZONSTORE
#if UNDERGROUND
		pID = pID.Replace("supercell.", "supercell.underground.");
#endif

		if(unavailableSkus.Contains(pID)) {
			ItemUnavailable(pID);
			return;
		}
		// Construct object passed to operation as input
		SkuInput request = new SkuInput();
		request.Sku = pID;
		// Call synchronous operation with input object
		RequestOutput response = iapService.Purchase(request);
#endif
			#endregion

		}

		void PurchaseSuccessful(string data, string transID, string amazonReceiptId = "") {
			MyDebug.Log("InAppManager::PurchaseSuccessful => Item: " + data);
#if AMAZONSTORE && UNDERGROUND
		data = data.Replace("supercell.underground.", "supercell.");
#endif

			switch(data) {
			case "balldance.removeads":
				MyDebug.Log("RemoveAds Done");
				//TODO Set your Remove Ads Variable here to save value accros sessions
				AdsMCG.Me.HideBannerAd();
				GameUtility.Me.playerData.isRemoveAds = true;
				if(!isRestore) {
#if GOOGLE_TRAK
				GoogleTracking.Me.LogInApp("Remove Ads", (long)0.99f);
				GoogleTracking.Me.LogInAppItem(transID, "Remove Ads", data, InAppTypes.NonConsumable.ToString(), (double)0.99f, (long)1);
#endif
					PopupMessages.Me.PurchaseSuccessfulMessage("Ads have been\nremoved successfully");
				} else {
					isRestoreItemSuccess = true;
				}
				break;

			default:
				InAppDetail iad = new InAppDetail();
				iad = GetIDInfo(data);
				if(iad == null) {
					break;
				}
#if GOOGLE_TRAK
			GoogleTracking.Me.LogInApp(iad.DisplayName, (long)iad.CostUSD);
			GoogleTracking.Me.LogInAppItem(transID, iad.DisplayName, data, iad.InAppType.ToString(), (double)iad.CostUSD, (long)1);
#endif
				if(iad.RewardType == RewardTypes.Coin) {
					//TODO: Awared Coins to user
				}
				string coststring;

				coststring = isProductReceived && isLocalStoreCost ? iad.CostStringLocaleBase : "$" + iad.CostUSD;
				string message = "You got {0} coins from\n\"{1}\" for {3}";
				message = string.Format(message, iad.Reward.ToString("n0"), iad.DisplayName, coststring);
				PopupMessages.Me.PurchaseSuccessfulMessage(message);
				break;

			}
#if UNITY_ANDROID && AMAZONSTORE
		if(!isRestore && !string.IsNullOrEmpty(amazonReceiptId)) {
			NotifyFulfillmentInput nfi = new NotifyFulfillmentInput();
			nfi.ReceiptId = amazonReceiptId;
			nfi.FulfillmentResult = "FULFILLED";
			iapService.NotifyFulfillment(nfi);
		}
#endif

			GameUtility.Me.SavePlayerData();
			CoreUtility.Me.OnInAppSucess(data);
		}
		//
		public void SetInAppInfos() {
			int cost;
			if(isProductReceived && isLocalStoreCost) {
				for(int i = 0; i < Cost.Count; i++) {
					if(Cost[i] == null)
						continue;
					string costStr = InAppInfo[i].CostStringLocaleBase;
					Cost[i].text = costStr;
				}
			} else {
				float cost1 = 0;
				for(int i = 0; i < Cost.Count; i++) {
					if(Cost[i] == null)
						continue;
					cost1 = InAppInfo[i].CostUSD;
					Cost[i].text = "$" + cost1.ToString("n2", CoreUtility.Me.locale);
				}
			}


			for(int i = 0; i < Reward.Count; i++) {
				if(Reward[i] == null)
					continue;
				string inappval = InAppInfo[i].Reward.ToString();
				int.TryParse(inappval, out cost);

				//MyDebug.Log(InAppRew[i + 1] + " - " + inappval + " - " + cost + " - " + cost.ToString("N0", lcl));
				if(!string.IsNullOrEmpty(inappval)) {
					Reward[i].text = cost.ToString("N0", CoreUtility.Me.locale) + " " + InAppInfo[i].RewardItem;
				}
			}

			//int bDeal = 0;
			//Vector3 pos;
			//int.TryParse(PluginManager.GetRemoteParameter(InAppRew[0]), out bDeal);
			//MyDebug.Log("Best Deal: " + bDeal);
			//for(int i = 0; i < BestDealRibin.Length; i++) {
			//	pos = ItemRewardTM[i].transform.localPosition;
			//	BestDealRibin[i].SetActive(false);
			//	if((i + 1).Equals(bDeal)) {
			//		BestDealRibin[i].SetActive(true);
			//		pos.y = 0f;
			//	} else {
			//		pos.y = -0.22f;
			//	}
			//	ItemRewardTM[i].transform.localPosition = pos;
			//}
		}

		InAppDetail GetIDInfo(string bundleID) {
			InAppDetail iad = null;

			foreach(InAppDetail id in InAppInfo) {
				if(id.ID.Equals(bundleID)) {
					iad = id;
					break;
				}
			}
			return iad;
		}
		void GetInAppIdsArrayForIOS() {
			List<string> iids = new List<string>();
			string s = string.Empty;

			InAppInfo.ForEach(o => {
				iids.Add(o.ID);
				s += o.ID + "\n";
			});
			inappids = iids.ToArray();
			MyDebug.Log("InAppManager::GetInAppIdsArrayForIOS => " + inappids.Length + "\n" + s);
		}
		//
		//
		#region "iOS Events Handelers"
#if UNITY_IOS && STOREKIT
		void RequestProduct() {
			if(isProductReceived) {
				CancelInvoke("RequestProduct");
				return;
			}
			MyDebug.Log("Requesting product for the iOS");
			StoreKitBinding.requestProductData(inappids);
		}
		void ASKPurchaseSuccessful(StoreKitTransaction data) {
			//PurchaseSuccessful(data.productIdentifier);
			PurchaseSuccessful(data.productIdentifier, data.transactionIdentifier);

		}
		void ASKProductListReceived(List<StoreKitProduct> pList) {
			if(pList.Count > 0) {
				isProductReceived = true;
				MyDebug.Log(" ->" + pList[0].productIdentifier);
			}
			string s = string.Empty;
			foreach(StoreKitProduct skp in pList) {
				InAppDetail ipd = GetIDInfo(skp.productIdentifier);
				if(ipd == null)
					continue;
				ipd.CostStringLocaleBase = skp.formattedPrice;
				s += skp.formattedPrice + '\t' + skp.title + '\t' + skp.productIdentifier + '\t' + skp.description + '\t' + skp.countryCode + '\n';
			}
			s = "\n" + s;
			MyDebug.Log(s);
			SetInAppInfos();
		}
		void ASKPurchaseFailed(string error) {
			MyDebug.Log("InAppManager::PurchaseFailed => ERROR: " + error);
			PopupMessages.Me.PurchaseFailMessage();
			CoreUtility.Me.OnInAppFail(pID);
		}
		void ASKPurchaseCancel(string error) {
			MyDebug.Log("InAppManager::PurchaseCancel => ERROR: " + error);
			PopupMessages.Me.PurchaseCanceldMessage();
			CoreUtility.Me.OnInAppFail(pID);
		}
		void ASKRestoreTransactionsFailed(string error) {
			MyDebug.Log("InAppManager::RestoreTransactionsFalied => ERROR: " + error);
			PopupMessages.Me.RestoreFailMessage();
			isRestore = false;
			MyDebug.Log("InAppManager::RestoreTransactionsFailed => isRestore?" + isRestore);
		}
		void ASKRestoreTransactionsFinished() {
			MyDebug.Log("InAppManager::RestoreTransactionsFinished =>");
			PopupMessages.Me.RestoreSuccessfulMessage();
		}
#endif
		#endregion

		#region "Google IAB Events Handelers"
#if UNITY_ANDROID && !AMAZONSTORE && GOOGLEIAB
		void IABBillingSupported() {
			MyDebug.Log("InAppManager::IABBillingSupported =>");
		}

		void IABBillingNotSupported(string error) {
			MyDebug.Log("InAppManager::IABBillingNotSupported: " + error);
		}
		//
		void IABQueryInventorySucceeded(List<GooglePurchase> purchases, List<GoogleSkuInfo> skus) {
			GameUtility.Me.HideProgressDialog();
			MyDebug.Log("InAppManager::IABQueryInventorySucceeded => " + purchases.Count + " : " + skus.Count);
			if(purchases == null || skus == null) {
				MyDebug.Log("Not able to retrive product list from Google, please try after some time.");
				PopupMessages.Me.PurchaseFailMessage();
				CoreUtility.Me.OnInAppFail(pID);
				return;
			}

			if(purchases.Count <= 0 && !isRestore) {
				//Fresh Purchase
				GoogleIAB.purchaseProduct(pID);
			} else if(purchases.Count > 0 && !isRestore) {
				//Conuseme and Purchase
				for(int i = 0; i < purchases.Count; i++) {
					if(!NON_CONSU_PRODUCTS.Contains("," + purchases[i].productId + ",")) {
						GoogleIAB.consumeProduct(purchases[i].productId);
					} else if(pID == purchases[i].productId) {
						PurchaseSuccessful(purchases[i].productId, purchases[i].orderId);
						return;
					}
				}
				GoogleIAB.purchaseProduct(pID);
			} else if(isRestore && purchases.Count > 0) {
				//Restore
				for(int i = 0; i < purchases.Count; i++) {
					if(!NON_CONSU_PRODUCTS.Contains("," + purchases[i].productId + ",")) {
						GoogleIAB.consumeProduct(purchases[i].productId);
					} else {
						PurchaseSuccessful(purchases[i].productId, purchases[i].orderId);

					}
				}
				PopupMessages.Me.RestoreSuccessfulMessage();
				isRestore = false;
			} else if(purchases.Count <= 0 && isRestore) {
				MyDebug.Log("Nothing to Restore");
				PopupMessages.Me.NothingToRestoreMessage();
				isRestore = false;

			}
		}
		//
		void IABQueryInventoryFailed(string error) {
			MyDebug.Log("InAppManager::IABQueryInventoryFailed => ERROR: " + error);
			if(!isRestore) {
				PopupMessages.Me.PurchaseFailMessage();
				CoreUtility.Me.OnInAppFail(pID);
			} else {
				PopupMessages.Me.RestoreFailMessage();
			}
			isRestore = false;
		}
		//
		void IABPurchaseCompleteAwaitingVerification(string purchaseData, string signature) {
			MyDebug.Log("InAppManager::IABPurchaseCompleteAwaitingVerification => purchaseData: " + purchaseData + " --- signature: " + signature);
		}
		//
		void IABPurchaseSucceeded(GooglePurchase purchase) {
			PurchaseSuccessful(purchase.productId, purchase.orderId);
		}
		//
		void IABPurchaseFailed(string error, int response) {
			MyDebug.Log("InAppManager::IABPurchaseFailed => ERROR: " + error + " RESPONSE: " + response);
			PopupMessages.Me.PurchaseFailMessage();
			CoreUtility.Me.OnInAppFail(pID);
		}
		//
		void IABConsumePurchaseSucceeded(GooglePurchase purchase) {
			MyDebug.Log("InAppManager::IABConsumePurchaseSucceeded => " + purchase.productId);
		}
		//
		void IABConsumePurchaseFailed(string error) {
			MyDebug.Log("InAppManager::IABConsumePurchaseFailed => " + error);
		}
#endif
		#endregion

		#region "Amazon Store InApp"

#if UNITY_ANDROID && AMAZONSTORE
	void ItemUnavailable(string sku) {
		//#if UNDERGROUND
		//sku = sku.Replace("supercell.underground.", "supercell.");
		//#endif
		switch(sku) {
		case "duckoff.removeads":
			sku = "Remove Ads";
			break;

		case "supercell.coinpack1":
			sku = "Coin Pack 1";
			break;

		case "supercell.coinpack2":
			sku = "Coin Pack 2";
			break;

		case "supercell.coinpack3":
			sku = "Coin Pack 3";
			break;

		case "supercell.coinpack4":
			sku = "Coin Pack 4";
			break;

		case "supercell.coinpack5":
			sku = "Coin Pack 5";
			break;

		}
		MarketPlaceNotSuported();
		isRestore = false;
		//CoreMethods.layer = lastLayer;
	}
	void MarketPlaceNotSuported() {
		PopupsText.Me.MarketNotSupportMessage(supportedMarketplaces);
		isRestore = false;		
	}

	void OnUserDataResponse(GetUserDataResponse args) {
		string requestId = args.RequestId;
		string userId = string.Empty;
		if(args.AmazonUserData != null) {
			userId = args.AmazonUserData.UserId;
			marketPlace = args.AmazonUserData.Marketplace;
		}
		string status = args.Status;

		string rdata = string.Format("R_ID: {0}\nUID: {1}\nMPlace: {2}\nStatus: {3}", requestId, userId, marketPlace, status);
		MyDebug.Log("InAppManager::OnUserDataResponse => " + rdata);

		if(status.ToUpper().Equals("NOT_SUPPORTED")) {
			isSupported = false;
		} else {
			isSupported = true;
		}
	}
	void OnProductDataResponse(GetProductDataResponse args) {
		string sku = string.Empty;
		string productType = string.Empty;
		string price = string.Empty;
		string title = string.Empty;
		string description = string.Empty;
		string smallIconUrl = string.Empty;

		string status = args.Status;
		string requestId = args.RequestId;
		Dictionary<string, ProductData> productDataMap = args.ProductDataMap;
		unavailableSkus = args.UnavailableSkus;
		/*
		if(productDataMap != null) {
			// for each item in the productDataMap you can get the following values for a given SKU
			// (replace "sku" with the actual SKU)
			sku = productDataMap["sku"].Sku;
			productType = productDataMap["sku"].ProductType;
			price = productDataMap["sku"].Price;
			title = productDataMap["sku"].Title;
			description = productDataMap["sku"].Description;
			smallIconUrl = productDataMap["sku"].SmallIconUrl;
		}
		*/
		string rdata = string.Format("R_ID: {0}\nStatus: {1}",
						   requestId, status);
		MyDebug.Log("InAppManager::OnProductDataResponse => " + rdata);

		if(status.ToUpper().Equals("NOT_SUPPORTED")) {
			MarketPlaceNotSuported();
			return;
		}

		isRestore = false;
	}

	void OnPurchaseResponse(PurchaseResponse args) {
		string sku = string.Empty;
		string productType = string.Empty;

		string userId = string.Empty;

		string receiptId = string.Empty;
		long purchaseDate = 0;
		long cancelDate = 0;

		string status = args.Status;
		string requestID = args.RequestId;

		if(args.AmazonUserData != null) {
			userId = args.AmazonUserData.UserId;
			marketPlace = args.AmazonUserData.Marketplace;
		}
		if(args.PurchaseReceipt != null) {
			sku = args.PurchaseReceipt.Sku;
			productType = args.PurchaseReceipt.ProductType;
			receiptId = args.PurchaseReceipt.ReceiptId;
			purchaseDate = args.PurchaseReceipt.PurchaseDate;
			cancelDate = args.PurchaseReceipt.CancelDate;
		}

		string rdata = string.Format("R_ID: {0}\nUID: {1}\nMPlace: {2}\nStatus: {3}\nReceiptID: {4}\nPDate: {5}\nCDate: {6}\nSKU: {7}\nPType:{8}",
						   requestID, userId, marketPlace, status, receiptId,
						   purchaseDate, cancelDate, sku, productType);
		MyDebug.Log("InAppManager::OnPurchaseResponse => " + rdata);

		status = status.ToUpper();
		switch(status) {
		case "NOT_SUPPORTED":
			MarketPlaceNotSuported();
			return;
			break;
		case "SUCCESSFUL":
			PurchaseSuccessful(sku, args.PurchaseReceipt.ReceiptId, args.PurchaseReceipt.ReceiptId);
			break;
		case "ALREADY_PURCHASED":
			if(productType.ToUpper().Equals("ENTITLED") || NON_CONSU_PRODUCTS.Contains("," + pID + ",")) {
				sku = pID;
				string rID = string.Empty;
				isRestore = true;
				PurchaseSuccessful(sku, rID, rID);
				PopupsText.Me.ItemAlreadyPurchaseMessage();
				return;
			}
			break;
		case "FAILED":
		case "INVALID_SKU":
			if(!isRestore) {
				PopupsText.Me.PurchaseFailMessage();
			} else {
				PopupsText.Me.RestoreFailMessage();
			}
			break;
		}

		isRestore = false;
	}
	void OnPurchaseUpdatesResponse(GetPurchaseUpdatesResponse args) {

		string requestId = args.RequestId;
		string userId = string.Empty;
		if(args.AmazonUserData != null) {
			userId = args.AmazonUserData.UserId;
			marketPlace = args.AmazonUserData.Marketplace;
		}
		List<PurchaseReceipt> receipts = args.Receipts;
		string status = args.Status;
		bool hasMore = args.HasMore;

		string rdata = string.Format("R_ID: {0}\nUID: {1}\nMPlace: {2}\nMore: {3}\nStatus: {4}", requestId, userId, marketPlace, hasMore, status);
		MyDebug.Log("OnPurchaseUpdatesResponse::OnUserDataResponse => " + rdata);

		status = status.ToUpper();
		switch(status) {
		case "NOT_SUPPORTED":
			MarketPlaceNotSuported();
			return;
			break;
		case "SUCCESSFUL":
			if(receipts != null && receipts.Count > 0) {
				for(int i = 0; i < receipts.Count; i++) {
					if(receipts[i].ProductType.ToUpper().Equals("ENTITLED")) {
						PurchaseSuccessful(receipts[i].Sku, receipts[i].ReceiptId, r eceipts[i].ReceiptId);
					}
				}
				PopupsText.Me.RestoreSuccessfulMessage();
			} else {
				PopupsText.Me.NothingToRestoreMessage();
			}
			return;
			break;
		case "FAILED":
			if(!isRestore) {
				PopupsText.Me.PurchaseFailMessage();
			} else {
				PopupsText.Me.RestoreFailMessage();
			}
			break;
		}
		isRestore = false;
	}
#endif
		#endregion
	}

	[System.Serializable]
	public class InAppDetail {
		public string DisplayName;
		public string ID;
		//
		public float CostUSD;
		[NonSerialized]
		public string CostStringLocaleBase;
		//
		public string RewardText;
		public int Reward;
		public RewardTypes RewardType;
		public string RewardItem;
		public InAppTypes InAppType;
	}
	public enum InAppTypes {
		Consumable,
		NonConsumable,
		Subscription,
		AutoRenewSubscription
	}
	public enum RewardTypes {
		Coin,
		Gems,
		Power
	}
}