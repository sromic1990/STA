using System;
using System.Collections;

using UnityEngine;

using GameAnax.Core.Singleton;

[PersistentSignleton(true, true)]
public class DeviceScreenChange : SingletonAuto<DeviceScreenChange> {
	/// <summary>
	/// Occurs when on resolution change.
	/// First parameter is width
	/// Second parameter is height
	/// </summary>
	public event Action<int, int> ResolutionChange;
	public event Action<DeviceOrientation> OrientationChange;
	[SerializeField]
	private float CheckDelay = 0.25f;

	int width, height;
	DeviceOrientation orientation;
	bool isAlive = true;

	void Awake() {
		Me = this;
		width = Screen.width;
		height = Screen.height;
		orientation = Input.deviceOrientation;
	}
	void Start() {
		StartCoroutine(CheckForChange());
	}

	IEnumerator CheckForChange() {
		while(isAlive) {
			// Check for a Resolution Change
			if(width != Screen.width || height != Screen.height) {
				OnResolutionChange();

			}

			// Check for an Orientation Change
			switch(Input.deviceOrientation) {
			case DeviceOrientation.Unknown:     // Ignore
			case DeviceOrientation.FaceUp:      // Ignore
			case DeviceOrientation.FaceDown:    // Ignore
				break;
			default:
				if(orientation != Input.deviceOrientation) {
					OnOrientationChange(Input.deviceOrientation);
				}
				break;
			}

			yield return new WaitForSeconds(CheckDelay);
		}
	}
	public void OnOrientationChange(DeviceOrientation or) {
		orientation = or;
		if(OrientationChange != null) OrientationChange(orientation);
	}

	public void OnResolutionChange() {
		width = Screen.width;
		height = Screen.height;
		if(ResolutionChange != null) ResolutionChange(Screen.width, Screen.height);
	}

	protected override void OnDestroy() {
		base.OnDestroy();
		isAlive = false;
	}

}
