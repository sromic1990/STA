using UnityEngine;
using GameAnax.Core.Extension;


public class ScaleFadeEffect : MonoBehaviour {
	[SerializeField]
	private Vector2 startScale;
	[SerializeField]
	private Vector2 destScale;
	[SerializeField]
	[Range(0f, 100f)]
	private float scaleSpeed;
	[SerializeField]
	private float fadeAfter;
	[SerializeField]
	private float destroyAfter;
	private float fadeDuration;
	private Transform _tr;
	// Use this for initialization
	void Awake() {
		_tr = GetComponent<Transform>();
		Vector3 scale = startScale.Cast();
		scale.z = _tr.localScale.z;
		_tr.localScale = scale;
		fadeDuration = destroyAfter - fadeAfter;

	}
	private void Fade() {
		iTween.FadeTo(gameObject, iTween.Hash("alpha", 0f, "time", fadeDuration, "easeType", iTween.EaseType.linear));
	}
	public void StartEffect() {
		iTween.ScaleTo(gameObject, iTween.Hash("x", destScale.x, "y", destScale.y, "speed", scaleSpeed));
		if(fadeAfter > 0) {
			Invoke("Fade", fadeAfter);
		}
		if(destroyAfter > 0) {
			Destroy(gameObject, destroyAfter);
		}

	}
}
