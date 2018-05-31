using UnityEngine;

using GameAnax.Core;


public class ScoreFlyer2 : MonoBehaviour {
	[SerializeField]
	private TextMesh scoreToShow;
	[SerializeField]
	private ParticleSystem paritcale;

	public void Fly(int value, Vector3 pos, float duration, int layer) {
		transform.ChangeLayer(layer, true);
		//transform.ChangeCameraWisePosition(GPLogic.Me.gpCam, GPLogic.Me.hudCam, new Vector3(0, 0, 2));

		scoreToShow.text = value.ToString();
		iTween.MoveTo(gameObject, iTween.Hash("position", pos, "time", duration, "easeType", iTween.EaseType.linear));
		Destroy(gameObject, duration);
	}
}
