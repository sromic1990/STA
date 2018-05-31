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

using System.Collections;

using UnityEngine;

using GameAnax.Core;
using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;

using GameAnax.Game.Enums;
using GameAnax.Game.Leaderboard;
using GameAnax.Game.Utility;

//using Prime31;

namespace GameAnax.Game.Leaderboard {
	[PersistentSignleton(true, true)]
	public class AchievementManager : Singleton<AchievementManager> {
		// Use this for initialization
		void Awake() {
			Me = this;
		}
		public void FetchStatus() {
			foreach(Achievement ad in GameUtility.Me.achievements) {
				ad.isAchieved = PlayerPrefs.GetInt(ad.prefKey, 0) == 1;
			}
		}
		//
		public void CheckAchievement(int score, AchievementType AType) {
			string concatedID = string.Empty;
			switch(AType) {
			case AchievementType.Score:
				for(int i = GameUtility.Me.achievements.Count - 1; i >= 0; i--) {
					if(score >= GameUtility.Me.achievements[i].value &&
					   GameUtility.Me.achievements[i].type == AType) {
						concatedID = GameUtility.Me.achievements[i].keyGameCenteriOS;
						break;
					}
				}
				break;

			case AchievementType.Combination:
				for(int j = 0; j < GameUtility.Me.achievements.Count; j++) {
					if(score == GameUtility.Me.achievements[j].value &&
					   GameUtility.Me.achievements[j].type == AType) {
						concatedID = GameUtility.Me.achievements[j].keyGameCenteriOS;
						break;
					}
				}
				break;
			default:
				concatedID = string.Empty;
				break;
			}

			if(string.IsNullOrEmpty(concatedID)) {
				//MyDebug.Log("AchievementManager::CheckAchievement=> " + AType.ToString() + " achievement for value " + score + " not available");
				return;
			}

			StartCoroutine(FindReportAchievement(concatedID));
		}

		IEnumerator FindReportAchievement(string iOSID) {
			yield return StartCoroutine(CoreMethods.Wait(0f));
			Achievement ad = null;
			foreach(Achievement ad1 in GameUtility.Me.achievements) {
				if(ad1.keyGameCenteriOS.Equals(iOSID)) {
					ad = ad1;
				}
			}
			if(ad == null) {
				MyDebug.Log("AchievementManager::FindReportAchievement => Achievement: " + iOSID + " not available");
				yield break;
			}
			if(ad.isAchieved) {
				MyDebug.Log("AchievementManager::FindReportAchievement => Achievement " + iOSID + " all ready reported");
				yield break;
			}
			//Reporting to data to GameCentre / Google Play service / Swarm / Amazon
#if UNITY_IOS
			if(!string.IsNullOrEmpty(ad.keyGameCenteriOS)) {
				ReportAchievement(ad.keyGameCenteriOS);
			}
#endif

#if UNITY_ANDROID
#if GPGSERVIES && !AMAZONSTORE
			if(!string.IsNullOrEmpty(ad.keyGooglePlayService)) {
				ReportAchievement(ad.keyGooglePlayService);
			}
#endif
#if AMAZONSTORE && GAMECIRCLE
			if(!string.IsNullOrEmpty(ad.AmazonID)) {
				AGSAchievementsClient.UpdateAchievementProgress(ad.AmazonID, 100f);
			}
#endif
#endif
			ad.isAchieved = true;
			//TODO: Report Achievement Unlocked to analytics System

			//TODO: Save Your Player Progress now.
			//PlayerPrefs.SetInt(ad.PrefKey, 1);
		}

		void ReportAchievement(string achievementID) {
#if UNITY_IOS && GAMECENTER
			if(!GameCenterBinding.isPlayerAuthenticated()) {
				GameCenterBinding.authenticateLocalPlayer();
			}
			GameCenterBinding.reportAchievement(achievementID, 100f);
			GameCenterBinding.showCompletionBannerForAchievements();
#endif

#if UNITY_ANDROID
#if GPGSERVIES && !AMAZONSTORE
			PlayGameServices.unlockAchievement(achievementID, true);
#endif
#endif
		}

	}
}