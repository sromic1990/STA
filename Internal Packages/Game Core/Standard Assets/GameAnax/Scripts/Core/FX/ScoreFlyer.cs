using UnityEngine;


public class ScoreFlyer : MonoBehaviour {
	[SerializeField]
	private TextMesh scoreToShow;
	[SerializeField]
	private float flySpeed;
	[SerializeField]
	private float flyDuraion;
	// Use this for initialization
	//void Start() { }
	// Update is called once per frame
	//void Update() { }

	public void FlyScore(int score) {
		scoreToShow.text = "+" + score;
		float y = transform.position.y + (flySpeed * flyDuraion);
		iTween.MoveTo(gameObject, iTween.Hash("y", y, "speed", flySpeed, "easeType", iTween.EaseType.linear));
		iTween.FadeTo(gameObject, iTween.Hash("alpha", 0f, "speed", flySpeed, "delay", flySpeed * 0.5f));
		Destroy(gameObject, flyDuraion);
	}
}
