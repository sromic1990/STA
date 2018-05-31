using System.Collections;

using UnityEngine;

using GameAnax.Core;
using GameAnax.Core.Utility;


public class EXGuiRipple : MonoBehaviour {
	private exSpriteBorder _border;
	private Transform _tr;
	//
	private float stWidth, endWidth, stHeight, endHeight;


	// Use this for initialization
	void Awake() {
		_border = GetComponent<exSpriteBorder>();
		_tr = transform;
	}
	//void Start() {
	//}
	// Update is called once per frame
	//void Update() {
	//}

	private void SetRippleSize(Transform rippleFor, float zOffset, Color rippleColor) {
		_border.color = rippleColor;

		Vector3 worldPosition = rippleFor.position;
		worldPosition.z -= ((rippleFor.localScale.z / 2f) + 0.025f);
		worldPosition.z += zOffset;
		_tr.position = worldPosition;
		_tr.parent = rippleFor.parent;

		stWidth = (rippleFor.localScale.x * 100f);
		stHeight = (rippleFor.localScale.y * 100f);

		stWidth += ((_border.guiBorder.border.left + _border.guiBorder.border.right));
		stHeight += ((_border.guiBorder.border.top + _border.guiBorder.border.bottom));

		stWidth /= (_border.scale.x * 100f);
		stHeight /= (_border.scale.y * 100f);

		_border.width = stWidth;
		_border.height = stHeight;
		_border.Commit();
	}

	public void StartRipple(float rTime, Transform rippleFor, float zOffset, Color rippleColor) {
		SetRippleSize(rippleFor, zOffset, rippleColor);
		iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 100, "time", rTime,
			"easeType", iTween.EaseType.linear,
			"onupdate", "UpdateValue"
		));
		CoreUtility.Me.Fade(gameObject, 0f);
		Destroy(gameObject, rTime + 0.1f);
	}
	public IEnumerator SetRippleAsGlow(float rTime, Transform rippleFor, float zOffset, Color rippleColor) {
		CoreUtility.Me.Fade(gameObject, 0, 0.01f);
		yield return StartCoroutine(CoreMethods.Wait(0.01f));
		SetRippleSize(rippleFor, zOffset, rippleColor);

		Vector3 worldPosition = rippleFor.position;
		worldPosition.z += ((rippleFor.localScale.z / 2f) - 0.025f);
		_tr.position = worldPosition;

		stWidth = ((_border.guiBorder.border.left + _border.guiBorder.border.right) * 0.6f);
		stHeight = ((_border.guiBorder.border.top + _border.guiBorder.border.bottom) * 0.6f);

		stWidth /= (_border.scale.x * 100f);
		stHeight /= (_border.scale.y * 100f);


		MyDebug.Log("OW: {0}, OH: {1}, NW: {2}, NH: {3}",
			_border.width, _border.height, _border.width + stWidth, _border.height + stHeight);
		_border.width += stWidth;
		_border.height += stHeight;
		_border.Commit();

		CoreUtility.Me.Fade(gameObject, 1, 0.1f);
		yield return StartCoroutine(CoreMethods.Wait(rTime));
		CoreUtility.Me.Fade(gameObject, 0, 0.25f);
		Destroy(gameObject, 0.25f);
	}

	void UpdateValue(float value) {
		_border.width = stWidth + value;
		_border.height = stHeight + value;
	}
}