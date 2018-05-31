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

using GameAnax.Core.Singleton;
using GameAnax.Core.Utility.Popup;


namespace GameAnax.Game.Utility.Popup {
	[PersistentSignleton(true, true)]
	public class PopupMessages : SingletonAuto<PopupMessages> {
		#region Tutorial related message
		public void TutorialStartMessage() {
			PopupManager.Me.ShowPopup("Let's learn how\nto play the game", ButtonSchemes.None, PopupTypes.TutorialStart, PopupLocations.Top, 03, 0.2d);
		}
		public void TutorialMessage() {
			string message = "Tap when the two lines\nmeet on the center\nof the balloon";
			PopupManager.Me.ShowPopup(message, ButtonSchemes.None, PopupTypes.Tutorial, PopupLocations.Top, 0.21d);
		}

		public void TutorialCompleteMessage() {
			PopupManager.Me.ShowPopup("Great!", "Keep poping balloons and\nmake socre higher as you can",
				ButtonSchemes.OK, PopupTypes.TutorialComplete, PopupLocations.Center, 0.15d);
		}
		#endregion


		#region Gameplay Related Popup Messges
		//public void CompletePreviousLevel() {
		//	string s = "Please complete\nprevious level first";
		//	PopupManager.Me.ShowPopup("Level Locked", s, ButtonSchemes.OK, PopupTypes.RemoveTilePower, PopupLocations.Center, 0.1d);
		//}
		//public void PowerUseBeforeGameOver(string cost, int count) {
		//	string s = "You can change {0} cells on the board\nusing # {1} to extend your\ngameplay, Do you want to use the\nremove tile power up?";
		//	string s1 = string.Format(s, count, cost);
		//	PopupManager.Me.ShowPopup("No Moves Left!!!", s1, ButtonSchemes.YesNo, PopupTypes.RemoveTilePower, PopupLocations.Top, 0.08d, true);
		//}
		//public void NotEnoughTileToRemove(string rtiles) {
		//	string message1 = string.Format("You need atleast {0} cells with\n{1}. The cells with\nother numbers are not eligible\nfor this power up.",
		//						  (GameUtility.Me.eMode.removeTileCount), rtiles);
		//	PopupManager.Me.ShowPopup("Oops!", message1, ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		//}
		//public void RemoveCellPowerMessage(string title, string cost, int count) {
		//	string message1 = string.Format("You will be charged # {0}\nfor remove {1} cells, Are you sure?", cost, count);
		//	PopupManager.Me.ShowPopup(title, message1, ButtonSchemes.YesNo, PopupTypes.RemoveTilePower, PopupLocations.Center, 0.08d, true);
		//}
		//
		//
		//public void HammerPowerMessage(string cost) {
		//	string message1 = string.Format("You will be charged # {0}\nto break cell using hammer,\nAre you sure?", cost);
		//	PopupManager.Me.ShowPopup("Hammer", message1, ButtonSchemes.YesNo, PopupTypes.HammerPower, PopupLocations.Center, 0.08d, true);
		//}
		//public void SelectCellToUseHammerMessage() {
		//	PopupManager.Me.ShowPopup("Hammer", "Select any cell to break\nwith Hammer.", ButtonSchemes.None, PopupTypes.Normal, PopupLocations.Top, 5f, 0.08d);
		//}
		//
		//
		//public void PowerOnSpecialTile(string power) {
		//	PopupManager.Me.ShowPopup("Wrong Selection", "You can't use " + power + "\non special Tile", ButtonSchemes.None,
		//							  PopupTypes.Normal, PopupLocations.Top, 1.5f, 0.1d);
		//}
		//public void PowerOnCollectible(string power) {
		//	PopupManager.Me.ShowPopup("Wrong Selection", "You can't use " + power + "\non Collective Item", ButtonSchemes.None,
		//							  PopupTypes.Normal, PopupLocations.Top, 1.5f, 0.1d);
		//}
		//
		//
		//public void SwapCellPowerMessage(string cost) {
		//	string sTilePowerDescription = "";
		//	sTilePowerDescription = string.Format("You will be charged # {0}\nAre you sure you want to\nSwap cell locations?", cost);
		//	PopupManager.Me.ShowPopup("Swap Cell", sTilePowerDescription, ButtonSchemes.YesNo, PopupTypes.SwitchTilePower, PopupLocations.Center, 0.08d, true);
		//}
		//public void SwapPowerSelectFirstCellMessage() {
		//	PopupManager.Me.ShowPopup("Swap Cell", "Select first cell to move.", ButtonSchemes.None, PopupTypes.Normal, PopupLocations.Top, 5f, 0.08d);
		//}
		//public void SwapPowerSelectSecondCellMessage() {
		//	PopupManager.Me.ShowPopup("Swap Cell", "Select destination cell\nto swap cells location.", ButtonSchemes.None, PopupTypes.Normal, PopupLocations.Top, 5f, 0.08d);
		//}
		//
		//
		//public void ReshufflePowerMessage(string cost) {
		//	string message1 = string.Format("You will be charged # {0}\nto reshuffle the board,\nAre you sure?", cost);
		//	PopupManager.Me.ShowPopup("Reshuffle Board", message1, ButtonSchemes.YesNo, PopupTypes.ReshufflePower, PopupLocations.Center, 0.08d, true);
		//}
		//
		//
		public void EnoughCoinToUsePowerMessage(string power) {
			PopupManager.Me.ShowPopup("Move Ahead?", "You now have enough coins. Do\nwish to use " + power + " power?", ButtonSchemes.YesNo, 0.09d);
		}
		public void StillNotEnoughCoinForPowerMessage() {
			PopupManager.Me.ShowPopup("Oops!", "You still don't have enough\ncoins to use this power. Please\nbuy more coins.", ButtonSchemes.Close, 0.09d);
		}
		#endregion

		#region Social Sharng relate Popup Messages
		public void GetMyInfoFailMessage() {
			PopupManager.Me.ShowPopup("Opps!", "We are not able to take basic Facebook\ninfo from your accout. Which are requred\nfor multiplayer. You need to perform the\naction once again to acess facebook\nbased feauters.",
				ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.07d);
		}
		public void FBScorePostSuccessMessage() {
			PopupManager.Me.ShowPopup("Successfull..!!", "Score posted on\nfacebook successfully", ButtonSchemes.OK, PopupTypes.Normal, PopupLocations.Center, 0.09);
		}
		public void FBScorePostFailMessage() {
			PopupManager.Me.ShowPopup("Opps..!!", "Unable to post score facebook,\nPlease try after some time...", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.09d);

		}
		public void FBFriendRetriveFailMessage() {
			PopupManager.Me.ShowPopup("Opps!",
				"We are not able to get your facebook\nfriends. Please try after some time If\nyou are getting this message every\ntime please check your faceboook\nsetting for this app.",
				ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.00d);
		}
		public void FacebookLoginFailMessage() {
			PopupManager.Me.ShowPopup("Oops!", "We are not able to join facebook,\nplease try after some time", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		}

		public void InvalidRequest() {
			PopupManager.Me.ShowPopup("Oops!", "Invalid facebook requeset, \nplease try after some time...", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		}
		public void NoFriendsFound() {
			PopupManager.Me.ShowPopup("Oops!", "We not find any of your friends\nplaying the game. please Invite\nfriends first...", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		}
		public void NotInFriendList() {
			PopupManager.Me.ShowPopup("Oops!", "We not find this friend is\nplaying the game. please\nInvite him / her first...", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.09d);
		}

		public void TwitterSuccessMessage(string shareMessageTitle, string shareMessageStatus) {
			PopupManager.Me.ShowPopup(shareMessageTitle, shareMessageStatus, ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		}
		public void TwitterFailMessage() {
			PopupManager.Me.ShowPopup("Oops!", "Failed to post on twitter,\nplease try after sometime...", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.09d);
		}
		public void SharingFailMessage() {
			PopupManager.Me.ShowPopup("Oops!", "Social post cancelled / failed.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.09d);
		}
		public void ShareSuccessMessage(string shareMessage) {
			PopupManager.Me.ShowPopup(shareMessage, ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		}
		#endregion

		#region Game Utility Related Popup Messages

		public void LeaderboardCommingSoonMessage() {
			PopupManager.Me.ShowPopup("Coming soon", "Leaderboard feature is coming soon.\nPlease stay tuned.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		}

		public void NotEnoughCoinMesssage() {
			PopupManager.Me.ShowPopup("Oops!", "You seem to have run out of Coins.\nWould you like to buy some from\nthe store?", ButtonSchemes.YesNo, PopupTypes.Normal, PopupLocations.Center, 0.08d);
		}
		public void ReviewWithOutCoinMessgae() {
			PopupManager.Me.ShowPopup("Thanks", "We are thankfull for your feedback.", ButtonSchemes.OK, PopupTypes.Normal, PopupLocations.Center, 0.14d);
		}
		public void ReviewWithCoinMessage(string coin) {
			PopupManager.Me.ShowPopup("Grate!", "You get # " + coin + " for review us", ButtonSchemes.OK, PopupTypes.Normal, PopupLocations.Center, 0.14d);

		}
		#endregion

		#region InApp Related Popup Messages
		public void RestoreProcessMessage() {
			PopupManager.Me.ShowPopup("Wait...", "Restoring old purchases\nin progress", ButtonSchemes.None, PopupTypes.Normal, PopupLocations.Center, 0.14d);
		}
		public void InAppProcessMessage(string item, string cost) {
			string msg = string.Format("We are processing request to\npurchase \"{0}\" for {1}", item, cost);
			PopupManager.Me.ShowPopup("Wait...", msg, ButtonSchemes.None, PopupTypes.Normal, PopupLocations.Center, 0.136d);
		}
		public void InAppNotIntegratedMessage() {
			PopupManager.Me.ShowPopup("Opps!", "Inapp purchases are\nnot integrated yet", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);

		}
		public void RestoreSuccessfulMessage() {
			PopupManager.Me.ShowPopup("Restore Successful", "Previous purchase has been\nrestored successfully.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		}
		public void RestoreFailMessage() {
			PopupManager.Me.ShowPopup("Sorry", "Restore process failed,\nPlease try after some time.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		}
		public void NothingToRestoreMessage() {
			PopupManager.Me.ShowPopup("Sorry", "There is nothing to restore.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		}

		public void PurchaseFailMessage() {
			PopupManager.Me.ShowPopup("Sorry", "Purchase process failed\nPlease try after some time.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		}
		public void PurchaseSuccessfulMessage(string message) {
			PopupManager.Me.ShowPopup("Purchase Successful", message, ButtonSchemes.OK, PopupTypes.Normal, PopupLocations.Center, 0.16d);

		}
		public void ItemAlreadyPurchaseMessage() {
			PopupManager.Me.ShowPopup("Purchase Successful", "Item already purchased.\nItem restored successfully.", ButtonSchemes.OK, PopupTypes.Normal, PopupLocations.Center, 0.16d);
		}
		public void PurchaseCanceldMessage() {
			PopupManager.Me.ShowPopup("Purchase Cancelled", "You have cancelled\nthe purchase.", ButtonSchemes.Close, PopupTypes.Normal, PopupLocations.Center, 0.15d);
		}
		public void MarketNotSupportMessage(string supportMarketList) {
			string msg = string.Format("Item or request is not supported for current\nmarket place. Please use any of these\nmarket places ({0})", supportMarketList);
			PopupManager.Me.ShowPopup("Opps!", msg, ButtonSchemes.OK, PopupTypes.Normal, PopupLocations.Center, 0.07d);

		}
		#endregion
	}
}