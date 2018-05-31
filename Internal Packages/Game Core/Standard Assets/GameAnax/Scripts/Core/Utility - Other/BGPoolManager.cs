using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGPoolManager : MonoBehaviour {

	public List<BGPool> bgs;
	// Use this for initialization
	//void Start() { }
	//Update is called once per frame
	//void Update() { }

	public void RestBGsPosition() {
		bgs.ForEach(o => o.ResetPos());
	}

}
