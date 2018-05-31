using UnityEngine;

using System;
using System.Collections;
using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;

namespace GameAnax.Core.Locaton {
	[PersistentSignleton(true, true)]
	public class LocationService : SingletonAuto<LocationService> {
		LocationData ldata;
		public Action<LocationData> LocationReceived;
		public Action<string> LocationAcquireFailed;

		public void GetLatLang() {
			StartCoroutine(CheckLocation(10));
		}
		public void GetLatLang(int maxWait) {
			StartCoroutine(CheckLocation(maxWait));
		}

		private IEnumerator CheckLocation(int maxWait) {
			// Checking about is user allowed location for the app / devices
			if(!Input.location.isEnabledByUser) {
				OnLocationAcquireFailed("Location services disabled by user");
				yield break;
			}

			// Starting Locaton services to aquire
			Input.location.Start();

			// wait up to max proviede seconds to determine locaton
			while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
				yield return new WaitForSeconds(1);
				maxWait--;
			}

			// When it's not get location within time bound
			if(maxWait < 1 && Input.location.status == LocationServiceStatus.Initializing) {
				OnLocationAcquireFailed("Location request time out");
				yield break;
			}

			// when Connection has failed before max time to aquire
			if(Input.location.status == LocationServiceStatus.Failed) {
				OnLocationAcquireFailed("Unable to detect location");
				yield break;
			}

			// Access granted and location value could be retrieved
			MyDebug.Log("Lat: {0}, Long: {1}, Alti: {2}, hAcu: {3}, vAcu: {4}, ts: {5}",
				Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.altitude,
				Input.location.lastData.horizontalAccuracy, Input.location.lastData.verticalAccuracy, Input.location.lastData.timestamp);

			ldata.latitude = Input.location.lastData.latitude;
			ldata.longitude = Input.location.lastData.longitude;
			ldata.altitude = Input.location.lastData.altitude;

			ldata.horizontalAccuracy = Input.location.lastData.horizontalAccuracy;
			ldata.verticalAccuracy = Input.location.lastData.verticalAccuracy;

			ldata.timestamp = Input.location.lastData.timestamp;

			OnLocationReceived(ldata);

			// Stop service if there is no need to query location updates continuously
			Input.location.Stop();
		}

		private void OnLocationAcquireFailed(string error) {
			if(LocationAcquireFailed != null) {
				LocationAcquireFailed.Invoke(error);
			}
		}

		private void OnLocationReceived(LocationData data) {
			if(LocationReceived != null) {
				LocationReceived.Invoke(data);
			}
		}


	}

	public struct LocationData {

		public float latitude;
		public float longitude;

		public float altitude;
		public float horizontalAccuracy;
		public float verticalAccuracy;
		public double timestamp;

	}
}