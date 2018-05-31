using UnityEngine;


public class Blinker : MonoBehaviour {
	[SerializeField]
	private new Renderer renderer;
	[SerializeField]
	private float blinkSpeed;
	[SerializeField]
	private float destroyAfter;
	// Use this for initialization
	void Start() {
		Invoke("Blink", blinkSpeed);
		if(destroyAfter > 0) {
			Destroy(gameObject, destroyAfter);
		}
	}

	// Update is called once per frame
	//void Update() {}

	private void Blink() {
		renderer.enabled = !renderer.enabled;
		Invoke("Blink", blinkSpeed);
	}
}
