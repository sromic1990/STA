using System.Collections.Generic;

using UnityEngine;


public class LineDrawer : MonoBehaviour {
	private LineRenderer line;
	private LineRendererCollider2D colliderMaker;
	[HideInInspector]
	public float linePower;
	public float safeDist;
	private float lineSize;
	private List<Vector3> linePoints = new List<Vector3>();

	private float curDist;

	// Use this for initialization
	void Awake() {
		line = GetComponent<LineRenderer>();
		colliderMaker = GetComponent<LineRendererCollider2D>();
	}
	void Start() { }

	// Update is called once per frame
	//void Update() { }
	public void DrawLine(params Vector3[] unityWorldPoint) {
		DrawLine(true, unityWorldPoint);
	}
	public void DrawLine(bool createCol, params Vector3[] unityScreenPoint) {
		//MyDebug.Log("line points are: " + unityScrenPoint.Length + " = " + (null == line));
		linePoints.Clear();
		linePoints.AddRange(unityScreenPoint);
		//MyDebug.Log("Points Count: {0}", linePoints.Count);
		Vector3 tmpPoint;
		for(int i = 0; i < linePoints.Count; i++) {
			tmpPoint = linePoints[i];
			tmpPoint = Camera.main.ScreenToWorldPoint(tmpPoint);
			tmpPoint = line.transform.InverseTransformPoint(tmpPoint);
			tmpPoint.z = 0f;
			linePoints[i] = tmpPoint;
		}

		recheck:
		if(linePoints.Count < 2) { RemoveLine(false); return; }
		for(int i = 0; i < linePoints.Count - 1; i++) {
			curDist = Vector3.Distance(linePoints[i], linePoints[i + 1]);
			if(curDist < safeDist) { linePoints.RemoveAt(i + 1); goto recheck; }
		}

		line.positionCount = linePoints.Count;
		line.SetPositions(linePoints.ToArray());
		lineSize = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
		linePower = Mathf.Min(lineSize, 10);
		linePower = 1f - (linePower / 10);
		linePower = Mathf.Min(linePower, 1f);
		//MyDebug.Log("LS: " + lineSize + ", LP: " + linePower);
		if(createCol) colliderMaker.CreateCollider2D();
	}
	public void RemoveLine() {
		RemoveLine(true);
	}
	public void RemoveLine(bool isResetTouchId) {
		line.positionCount = 0;
		colliderMaker.CreateCollider2D();
		//if(isResetTouchId) GPLogic.Me.tchID = -1;
	}
}
