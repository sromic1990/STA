using UnityEngine;

using GameAnax.Core;
using GameAnax.Core.Attributes;
using GameAnax.Core.Extension;
using GameAnax.Core.Utility;


public class JumpyCamera : MonoBehaviour {
	[EnumFlagAttribute]
	public Menus layer;


	Vector3 moverpos, followToPos, oriPos;
	float camVelocityY, camVelocityX, curCenterDist, xDif;
	[EnumFlagAttribute]
	public Axis followDirection;
	[SerializeField]
	private Transform mover;
	[SerializeField]
	private Transform followTo;
	[SerializeField]
	private Transform camOffOutOffTR;
	[SerializeField]
	private Vector3 smoothTime;
	[SerializeField]
	private Vector3 maxSpeed;
	[SerializeField]
	private Vector3 offsetBoundry;
	[SerializeField]
	private Vector3 followSnapDist;

	// Use this for initialization
	void Awake() {
		oriPos = mover.localPosition;
		offsetBoundry.x = camOffOutOffTR.transform.localPosition.x;

		if(maxSpeed.x < 1f) maxSpeed.x = float.PositiveInfinity;
		if(maxSpeed.y < 1f) maxSpeed.y = float.PositiveInfinity;
		if(maxSpeed.z < 1f) maxSpeed.z = float.PositiveInfinity;

	}
	// void Start() { }
	// Update is called once per frame
	// void Update() { }

	void LateUpdate() {
		if(!layer.Contain(CoreMethods.layer)) { return; }

		//Calculate Camera and BG Possion as per Bird Location
		moverpos = mover.localPosition;
		followToPos = followTo.localPosition;
		curCenterDist = followToPos.y - moverpos.y;
		if(((int)followDirection).Contain((int)Axis.Y)) {
			if(curCenterDist > 0)
				if(curCenterDist < followSnapDist.y) {
					moverpos.y = followToPos.y;
				} else {
					moverpos.y = Mathf.SmoothDamp(moverpos.y, followToPos.y, ref camVelocityY, smoothTime.y, maxSpeed.y);
				}
		}

		if(((int)followDirection).Contain((int)Axis.X)) {
			if(offsetBoundry.x > 0) {
				if(!followTo.localPosition.x.Between(-offsetBoundry.x, offsetBoundry.x)) {
					xDif = followToPos.x - ((followToPos.x > 0 ? 1f : -1f) * offsetBoundry.x);
					moverpos.x = Mathf.SmoothDamp(moverpos.x, xDif, ref camVelocityX, smoothTime.x, maxSpeed.x);
				} else {
					moverpos.x = Mathf.SmoothDamp(moverpos.x, 0f, ref camVelocityX, smoothTime.x, maxSpeed.x);
				}
			} else {
				if(Mathf.Abs(followToPos.x - moverpos.x) < followSnapDist.x) moverpos.x = followToPos.x;
				else moverpos.x = Mathf.SmoothDamp(moverpos.x, followToPos.x, ref camVelocityX, smoothTime.x, maxSpeed.x);
			}
		}
		mover.localPosition = moverpos;
	}

	public void ResetPosition() {
		mover.localPosition = oriPos;
	}
}