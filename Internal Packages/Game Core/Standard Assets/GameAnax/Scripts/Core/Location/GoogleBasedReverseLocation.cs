using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Linq;

using UnityEngine;

using GameAnax.Core.Net;


namespace GameAnax.Core.Locaton {

	public enum RequiredData {
		premise,
		streetNumber,
		route,
		subArea,
		area,
		city,
		state,
		county,
		zipCode,
		country,
		palceId
	}

	public class GoogleBasedReverseLocation {
		public Action<string> ReverseGeolocationFail;
		public Action<Dictionary<string, string>> ReverseGeolocationSuccess;


		Dictionary<string, string> keys = new Dictionary<string, string>() {
			{"premise", "premise"},
			{"streetNumber", "street_number"},
			{"route", "route"},
			{"subArea", "neighborhood"},
			{"area", "sublocality"},
			{"city", "locality"},
			{"state", "administrative_area_level_1"},
			{"county", "administrative_area_level_2"},
			{"zipCode", "postal_code"},
			{"country", "country"},
			{"palceId","place_id"}
		};




		WebData _webData;
		string _googleGeocodeApiKey = string.Empty;
		string baseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
		private GoogleBasedReverseLocation() { }
		public GoogleBasedReverseLocation(string googleGeocodeApiKey) {
			if(string.IsNullOrEmpty(googleGeocodeApiKey))
				throw new ArgumentNullException("Google Geocoid API Key");

			_googleGeocodeApiKey = googleGeocodeApiKey;

			_webData = new WebData(Encoding.ASCII);
		}

		List<string> asked;
		public void GetReverseAddress(string latLng, params RequiredData[] need) {
			string s = "";
			need.ToList().ForEach(o => s += o.ToString() + ",");
			s = s.Trim(',');
			asked = s.Split(',').ToList();


			if(asked == null)
				asked = new List<string>();

			Dictionary<string, object> data = new Dictionary<string, object>() {
				{"latlng",latLng },
				{"language","en"},
				{"key",_googleGeocodeApiKey}
			};
			//MyDebug.Log("Calling api");
			ExecuteURLParameters option = new ExecuteURLParameters() {
				url = baseUrl,
				method = WebMethod.GET,
				isMultipart = false,
				postData = data,
				callback = Returndata
			};
			_webData.ExecuteURL(option);
		}
		private void Returndata(string data, WebHeaderCollection haeder) {
			//MyDebug.Log("retruned data");
			if(data.StartsWith("{\"error", StringComparison.InvariantCultureIgnoreCase)) {
				OnReverseGeolocationFail(data);
				return;
			}
			ReverseGeocodingResult reverseResult = JsonUtility.FromJson<ReverseGeocodingResult>(data);
			if(!reverseResult.status.Equals("OK")) {
				OnReverseGeolocationFail(reverseResult.status);
				return;
			}

			Result r = reverseResult.results[0];
			Dictionary<string, string> rData = new Dictionary<string, string>();

			foreach(AddressComponent ac in r.address_components) {
				foreach(string s in keys.Keys)
					if(asked.Contains(s) && ac.types.Contains(keys[s])) {
						rData.Add(s, ac.long_name);
					}
			}
			OnReverseGeolocationSuccess(rData);
		}

		private void OnReverseGeolocationFail(string error) {
			if(null != ReverseGeolocationFail) {
				ReverseGeolocationFail.Invoke(error);
			}
		}
		private void OnReverseGeolocationSuccess(Dictionary<string, string> data) {
			if(null != ReverseGeolocationSuccess) {
				ReverseGeolocationSuccess.Invoke(data);
			}
		}

	}


	[Serializable]
	public struct AddressComponent {
		public string long_name;
		public string short_name;
		public List<string> types;
	}

	[Serializable]
	public struct Bounds {
		public Location northeast;
		public Location southwest;
	}

	[Serializable]
	public struct Location {
		public double lat;
		public double lng;
	}

	[Serializable]
	public struct Viewport {
		public Location northeast;
		public Location southwest;
	}

	[Serializable]
	public struct Geometry {
		public Bounds bounds;
		public Location location;
		public string location_type;
		public Viewport viewport;
	}

	[System.Serializable]
	public struct Result {
		public List<AddressComponent> address_components;
		public string formatted_address;
		public Geometry geometry;
		public string place_id;
		public List<string> types;
	}

	[System.Serializable]
	public struct ReverseGeocodingResult {
		public List<Result> results;
		public string status;
	}

}