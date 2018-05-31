using UnityEngine;


public class BGPool : MonoBehaviour {
	private Transform _tr;
	private Vector3 _oriPos, _curPos;
	// Use this for initialization
	void Awake() {
		_tr = GetComponent<Transform>();
		_oriPos = _tr.localPosition;
		_curPos = _tr.localPosition;
	}
	//void Start() { }

	// Update is called once per frame
	//void Update() { }

	string _tagIn = "";
	void OnTriggerEnter2D(Collider2D triggerIn) {
		_tagIn = triggerIn.tag;
		if(_tagIn.ToLower().Equals("maincamera")) { MoveUP(); }
	}
	public float lastCallTime = 0;
	public void MoveUP() {
		if((Time.realtimeSinceStartup - lastCallTime) < 1f) return;
		lastCallTime = Time.realtimeSinceStartup;
		_curPos = _tr.localPosition;
		_curPos.y += (15.36f * 3f);
		_tr.localPosition = _curPos;
	}
	public void ResetPos() {
		_tr.localPosition = _oriPos;
		lastCallTime = 0;
	}
}
