using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core.Attributes;
using GameAnax.Core.Extension;

public class Floater : MonoBehaviour {
	private Transform _tr;

	[EnumFlagAttribute]
	public Axis floatSide;
	[SerializeField]
	private Vector3 speed;
	[SerializeField]
	private Vector2 xOffset, yOffset, zOffset;
	private Vector2 xBoundry, yBoundry, zBoundry;
	private Vector3 curside, offset;

	// Use this for initialization
	void Awake() {
		_tr = GetComponent<Transform>();
	}
	void Start() {
		pos = Vector3.zero;
		curside.x = Random.Range(0, 2) == 0 ? 1 : -1;
		curside.y = Random.Range(0, 2) == 0 ? 1 : -1;
		curside.z = Random.Range(0, 2) == 0 ? 1 : -1;
		if(((int)floatSide).Contain((int)Axis.X)) { xBoundry = new Vector2(_tr.localPosition.x + xOffset.x, _tr.localPosition.x + xOffset.y); }
		if(((int)floatSide).Contain((int)Axis.Y)) { yBoundry = new Vector2(_tr.localPosition.y + yOffset.x, _tr.localPosition.y + yOffset.y); }
		if(((int)floatSide).Contain((int)Axis.Z)) { zBoundry = new Vector2(_tr.localPosition.z + zOffset.x, _tr.localPosition.z + zOffset.y); }
	}

	Vector3 pos;
	// Update is called once per frame
	void Update() {
		pos = _tr.localPosition;
		if(((int)floatSide).Contain((int)Axis.X)) {
			pos.x += speed.x * Time.smoothDeltaTime * curside.x;
			if(curside.x > 0 && pos.x >= xBoundry.y) curside.x = -1f;
			else if(curside.x < 0 && pos.x <= xBoundry.x) curside.x = 1f;
		}
		if(((int)floatSide).Contain((int)Axis.Y)) {
			pos.y += speed.y * Time.smoothDeltaTime * curside.y;
			if(curside.y > 0 && pos.y >= yBoundry.y) curside.y = -1f;
			else if(curside.y < 0 && pos.y <= yBoundry.x) curside.y = 1f;
		}
		if(((int)floatSide).Contain((int)Axis.Z)) {
			pos.z += speed.z * Time.smoothDeltaTime * curside.z;
			if(curside.z > 0 && pos.z >= zBoundry.y) curside.z = -1f;
			else if(curside.z < 0 && pos.z <= zBoundry.x) curside.z = 1f;
		}
		_tr.localPosition = pos;
	}
}
