using UnityEngine;

using GameAnax.Core;
using GameAnax.Core.Attributes;
using GameAnax.Core.Extension;


public class ObjectRotater : MonoBehaviour {
	[EnumFlagAttribute]
	public Axis rotateIn;
	[SerializeField]
	private Vector3 speed;
	private Vector3 _rotation;
	private Transform _tr;
	// Use this for initialization
	void Awake() {
		_tr = GetComponent<Transform>();
	}
	void Start() { }

	// Update is called once per frame
	void Update() {
		_rotation = _tr.localEulerAngles;
		if(((int)rotateIn).Contain((int)Axis.X)) { _rotation.x += speed.x * Time.smoothDeltaTime; }
		if(((int)rotateIn).Contain((int)Axis.Y)) { _rotation.y += speed.y * Time.smoothDeltaTime; }
		if(((int)rotateIn).Contain((int)Axis.Z)) { _rotation.z += speed.z * Time.smoothDeltaTime; }

		_tr.localEulerAngles = _rotation;
	}
}
