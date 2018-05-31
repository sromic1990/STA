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

using GameAnax.Core.IO;


namespace GameAnax.Core.Data {
	public static class TextDataReader {
		public static List<string> ReadRecordsFromResources(string fileName) {
			return ReadRecordsFromResources(fileName, '\n');
		}
		public static List<string> ReadRecordsFromResources(string fileName, char recordDilimator) {
			string tempData = File.ReadResourceFile(fileName);
			return GetRecordsFromText(tempData, recordDilimator);
		}

		public static List<List<string>> GetDataFromResources(string fileName) {
			return GetDataFromResources(fileName, '\n', ',');
		}
		public static List<List<string>> GetDataFromResources(string fileName, char fieldDilimator) {
			return GetDataFromResources(fileName, '\n', fieldDilimator);
		}
		public static List<List<string>> GetDataFromResources(string fileName, char recordDilimator, char fieldDilimator) {
			string tempData = string.Empty;
			tempData = File.ReadResourceFile(fileName);
			tempData = tempData.TrimEnd(recordDilimator);

			return GetDataFromText(tempData, recordDilimator, fieldDilimator);
		}

		public static List<string> ReadRecordsFromFile(string fileName) {
			return ReadRecordsFromFile(File.DataPath(), fileName, '\n');
		}
		public static List<string> ReadRecordsFromFile(string path, string fileName) {
			return ReadRecordsFromFile(path, fileName, '\n');
		}
		public static List<string> ReadRecordsFromFile(string fileName, char recordDilimator) {
			return ReadRecordsFromFile(File.DataPath(), fileName, recordDilimator);
		}
		public static List<string> ReadRecordsFromFile(string path, string fileName, char recordDilimator) {
			string tempData = File.ReadFile(path, fileName);
			return GetRecordsFromText(tempData, recordDilimator);
		}

		public static List<List<string>> GetDataFromFile(string fileName) {
			return GetDataFromFile(File.DataPath(), fileName, '\n', ',');
		}
		public static List<List<string>> GetDataFromFile(string path, string fileName) {
			return GetDataFromFile(path, fileName, '\n', ',');
		}
		public static List<List<string>> GetDataFromFile(string fileName, char fieldDilimator) {
			return GetDataFromFile(File.DataPath(), fileName, '\n', fieldDilimator);
		}
		public static List<List<string>> GetDataFromFile(string path, string fileName, char fieldDilimator) {
			return GetDataFromFile(path, fileName, '\n', fieldDilimator);
		}
		public static List<List<string>> GetDataFromFile(string fileName, char recordDilimator, char fieldDilimator) {
			return GetDataFromFile(File.DataPath(), fileName, recordDilimator, fieldDilimator);
		}
		public static List<List<string>> GetDataFromFile(string path, string fileName, char recordDilimator, char fieldDilimator) {
			string tempData = string.Empty;
			tempData = File.ReadFile(path, fileName);
			tempData = tempData.TrimEnd(recordDilimator);

			return GetDataFromText(tempData, recordDilimator, fieldDilimator);
		}


		public static List<string> GetRecordsFromText(string dataText) {
			return GetRecordsFromText(dataText, '\n');
		}
		public static List<string> GetRecordsFromText(string dataText, char recordDilimator) {
			string[] tempRecord = new string[0];
			List<string> retData = new List<string>();
			dataText = dataText.TrimEnd(recordDilimator);
			tempRecord = dataText.Split(recordDilimator);
			foreach(string st in tempRecord) {
				if(st.Trim().StartsWith("#", StringComparison.OrdinalIgnoreCase)) {
					continue;
				}
				retData.Add(st);
			}
			return retData;
		}

		public static List<List<string>> GetDataFromText(string dataText) {
			return GetDataFromText(dataText, '\n', '\t');
		}
		public static List<List<string>> GetDataFromText(string dataText, char fieldDilimator) {
			return GetDataFromText(dataText, '\n', fieldDilimator);
		}

		public static List<List<string>> GetDataFromText(string dataText, char recordDilimator, char fieldDilimator) {
			List<List<string>> retData = new List<List<string>>();
			List<string> recordList = new List<string>();
			List<string> fieldList = new List<string>();
			recordList = GetRecordsFromText(dataText, recordDilimator);
			foreach(string st in recordList) {
				fieldList = new List<string>();
				fieldList.AddRange(st.Split(fieldDilimator));
				retData.Add(fieldList);
			}
			return retData;
		}


		static bool CheckTypeForNumbers(object data) {
			bool result = false;
			if(Equals(data.GetType(), typeof(byte)) ||
			   Equals(data.GetType(), typeof(float)) ||
			   Equals(data.GetType(), typeof(int)) ||
			   Equals(data.GetType(), typeof(System.Single)) ||
			   Equals(data.GetType(), typeof(System.Double)) ||
			   Equals(data.GetType(), typeof(System.Decimal)) ||
			   Equals(data.GetType(), typeof(System.Int16)) ||
			   Equals(data.GetType(), typeof(System.Int32)) ||
			   Equals(data.GetType(), typeof(System.Int64))) {
				result = true;
			}
			return result;
		}
		public static List<List<string>> SearchRecoredFromData(List<List<string>> data, string searchValue) {
			return SearchRecoredFromData(data, searchValue, 0, SearchOption.Equal);
		}
		public static List<List<string>> SearchRecoredFromData(List<List<string>> data, string searchValue, int fieldNo) {
			return SearchRecoredFromData(data, searchValue, fieldNo, SearchOption.Equal);
		}
		public static List<List<string>> SearchRecoredFromData(List<List<string>> data, string searchValue, SearchOption serachCriteria) {
			return SearchRecoredFromData(data, searchValue, 0, serachCriteria);
		}
		public static List<List<string>> SearchRecoredFromData(List<List<string>> data, string searchValue, int fieldNo, SearchOption serachCriteria) {
			List<List<string>> retData = new List<List<string>>();

			foreach(List<string> listData in data) {
				switch(serachCriteria) {
				case SearchOption.Greater:
					if(CheckTypeForNumbers(listData[fieldNo]) && CheckTypeForNumbers(searchValue)) {
						if(System.Double.Parse(listData[fieldNo]) > System.Double.Parse(searchValue)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.GreaterOrEqual:
					if(CheckTypeForNumbers(listData[fieldNo]) && CheckTypeForNumbers(searchValue)) {
						if(System.Double.Parse(listData[fieldNo]) >= System.Double.Parse(searchValue)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.Less:
					if(CheckTypeForNumbers(listData[fieldNo]) && CheckTypeForNumbers(searchValue)) {
						if(System.Double.Parse(listData[fieldNo]) < System.Double.Parse(searchValue)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.LessOrEqual:
					if(CheckTypeForNumbers(listData[fieldNo]) && CheckTypeForNumbers(searchValue)) {
						if(System.Double.Parse(listData[fieldNo]) <= System.Double.Parse(searchValue)) {
							retData.Add(listData);
						}
					}
					break;


				case SearchOption.Contians:
					if(listData[fieldNo] is string) {
						if(listData[fieldNo].Contains(searchValue)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.NotContains:
					if(listData[fieldNo] is string) {
						if(!listData[fieldNo].Contains(searchValue)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.NotEndWith:
					if(listData[fieldNo] is string) {
						if(!listData[fieldNo].EndsWith(searchValue, StringComparison.OrdinalIgnoreCase)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.EndWith:
					if(listData[fieldNo] is string) {
						if(listData[fieldNo].EndsWith(searchValue, StringComparison.OrdinalIgnoreCase)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.NotStartWith:
					if(listData[fieldNo] is string) {
						if(!listData[fieldNo].StartsWith(searchValue, StringComparison.OrdinalIgnoreCase)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.StartWith:
					if(listData[fieldNo] is string) {
						if(listData[fieldNo].StartsWith(searchValue, StringComparison.OrdinalIgnoreCase)) {
							retData.Add(listData);
						}
					}
					break;

				case SearchOption.NotEqual:
					if(listData[fieldNo] != searchValue) {
						retData.Add(listData);
					}
					break;

				case SearchOption.Equal:
					if(listData[fieldNo] == searchValue) {
						retData.Add(listData);
					}
					break;

				case SearchOption.IN:
					break;
				}
			}
			return retData;
		}
	}

	/// <summary>
	/// Search option used by File Class to search data
	/// </summary>
	[Serializable]
	public enum SearchOption {
		Equal = 0,
		NotEqual = 1,
		StartWith = 2,
		EndWith = 3,
		Contians = 4,
		NotContains = 5,
		NotStartWith = 6,
		NotEndWith = 7,
		Greater = 8,
		Less = 9,
		GreaterOrEqual = 10,
		LessOrEqual = 11,
		IN = 12
	}
}