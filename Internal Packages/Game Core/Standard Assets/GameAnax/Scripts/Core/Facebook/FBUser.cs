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


namespace GameAnax.Core.Social {
	[System.Serializable]
	public class FBUser : IComparable<FBUser> {
		public string id;           // id 
		public string name;         // name or (first_name,last_name)
		public string gender;       // gender
		public string email;        // email
		public string photoURL;     // picture
		public string profileURL;   // link
		public int ageRangeMin;     // link

		public bool isVerified;     // verified
		public bool installed;      // installed
		public string installType;  // install_type

		public int score;
		public string token;
		public List<string> permissions = new List<string>();

		public int CompareTo(FBUser otherResult) {
			int retVal;
			// Ascending order sorting A-to-Z or 0-9
			retVal = string.Compare(name, otherResult.name, StringComparison.CurrentCultureIgnoreCase);
			return retVal;
		}

		public int CompareByScore(FBUser otherResult) {
			int retVal;
			// Descending order sorting Z-A or 9-0
			retVal = otherResult.score.CompareTo(score);
			// Ascending order sorting A-to-Z or 0-9
			//retVal = Score.CompareTo(otherResult.Score);
			return retVal;
		}
	}

	public class FBUserScoreComparer : IComparer<FBUser> {
		public int Compare(FBUser x, FBUser y) {
			int retval;
			if(null == x) {
				if(null == y) {
					// If x is null and y is null, they're equal. 
					retval = 0;
				} else {
					// If x is null and y is not null, y is greater. 
					retval = -1;
				}
			} else {
				// If x is not null...
				if(null == y) {
					// ...and y is null, x is greater.
					retval = 1;
				} else {
					// ...and y is not null, compare
					// with cusotm logic insted of default comparer
					retval = x.CompareByScore(y);
				}
			}
			return retval;
		}
	}
}