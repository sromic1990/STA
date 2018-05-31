using UnityEngine;

using GameAnax.Core.Animation;
using GameAnax.Core.Singleton;
using GameAnax.Core.Utility;


public class MSOnPSManager : Singleton<MSOnPSManager> {
	[Space(5)]
	public Transform ObjParent;
	[Space(5)]
	public GameObject AnimTextMesh;
	public GameObject DustParticle;
	public GameObject StarParticle;
	public GameObject SmallStarParticle;
	public GameObject WinRibbons;
	public GameObject HighScoreParticle;
	public GameObject SparkParticle;
	public GameObject TouchParticle;
	public GameObject SparkFountaionBrust;
	public GameObject SpriteAnimations;
	private SpriteTextureSwapAnimation spriteAni;
	private SpriteTextureSwapAnimation[] spriteAni1;
	public GameObject PopTheScoreGo;


	// Use this for initialization
	void Awake() {
		Me = this;
		if(SpriteAnimations != null) {
			spriteAni = SpriteAnimations.GetComponent<SpriteTextureSwapAnimation>();
			spriteAni.AnimationStateChange += AnimationStateChangeEListener;
		}
	}
	void Start() {
		if(SpriteAnimations != null) {
			SpriteAnimations.SetActive(false);
		}
	}
	// Update is called once per frame
	//void Update() {

	//}

	public GameObject GetMoveObj(Vector3 pos) {
		if(AnimTextMesh == null) {
			return null;
		}
		GameObject tmp = Instantiate(AnimTextMesh) as GameObject;
		pos.z -= 0.25f;
		tmp.SetActive(true);
		tmp.transform.position = pos;
		return tmp;
	}

	public void PlaySpriteAnimation(Transform targetTrans, Color sparkColor, string animationToPaly, bool isChild = false) {
		if(SpriteAnimations == null) {
			return;
		}
		if(isChild) {
			SpriteAnimations.transform.parent = targetTrans;
		}
		SpriteAnimations.transform.position = targetTrans.position - new Vector3(0, 0, 2f);
		SpriteAnimations.SetActive(true);
		if(spriteAni != null && !string.IsNullOrEmpty(animationToPaly)) {
			spriteAni.spriteRendere.color = sparkColor;
			spriteAni.Play(animationToPaly);
		}
	}
	public void StopSpriteAnimation() {
		if(SpriteAnimations == null) {
			return;
		}
		spriteAni.Stop();
		AnimationStateChangeEListener(AnimationStateChnage.End);
	}
	private void AnimationStateChangeEListener(AnimationStateChnage state) {
		if(state.Equals(AnimationStateChnage.End) && spriteAni != null) {
			spriteAni.spriteRendere.color = Color.white;
			spriteAni.spriteRendere.sprite = null;
			//
			SpriteAnimations.transform.parent = ObjParent;
			SpriteAnimations.transform.position = Vector3.zero;
			SpriteAnimations.transform.eulerAngles = Vector3.zero;
			SpriteAnimations.SetActive(false);
		}
	}
	public void PopTheScore(int scoreToPop, Vector3 pos, Vector3 popVal, Color color, float popTime = 1f) {
		if(PopTheScoreGo == null) {
			return;
		}
		GameObject pScore = Instantiate(PopTheScoreGo) as GameObject;
		pScore.transform.position = pos;
		pScore.SetActive(true);
		TextMesh tmpTM = pScore.GetComponent<TextMesh>();
		if(tmpTM != null) {
			tmpTM.text = "+" + scoreToPop;
			tmpTM.color = color;
		}
		iTween.MoveBy(pScore, iTween.Hash("x", popVal.x, "y", popVal.y, "z", popVal.z, "time", popTime, "isLocal", true,
			"easeType", iTween.EaseType.linear));
		CoreUtility.Me.Fade(pScore, 0f, popTime);
		Destroy(pScore, popTime + 0.04f);
	}


	#region Various Particle Effects
	public void CreateTouchParticle(Vector3 worldPos, Color sparkColor) {
		if(TouchParticle == null) {
			return;
		}
		GameObject tchParticle = Instantiate(TouchParticle) as GameObject;
		tchParticle.transform.position = worldPos;
		tchParticle.SetActive(true);
		ParticleSystem ps = tchParticle.GetComponent<ParticleSystem>();
		if(ps != null) {
			ParticleSystem.MainModule mm = ps.main;
			mm.startColor = sparkColor;
			ps.Play();
		}

	}
	public void CreateStartBurst(Vector3 pos) {
		if(StarParticle == null) {
			return;
		}

		GameObject starBurst = Instantiate(StarParticle) as GameObject;
		starBurst.transform.position = pos - new Vector3(0, 0, 1f);
		starBurst.SetActive(true);
		Destroy(starBurst, 1.04f);
	}
	public void CreateSmallStarBrust(Vector3 pos) {
		if(SmallStarParticle == null) {
			return;
		}
		GameObject starBurst = Instantiate(SmallStarParticle) as GameObject;
		starBurst.transform.position = pos - new Vector3(0, 0, 1f);
		starBurst.SetActive(true);
		Destroy(starBurst, 1.04f);
	}
	public void CreateDust(Transform targetTrans) {
		if(DustParticle == null) {
			return;
		}
		GameObject smoke = Instantiate(DustParticle) as GameObject;
		smoke.transform.position = targetTrans.transform.position - new Vector3(0, 0, 1f);
		smoke.SetActive(true);
		Destroy(smoke, 2f);
	}
	public void CreatWinRibbons() {
		if(WinRibbons == null) {
			return;
		}
		GameObject wp = Instantiate(WinRibbons) as GameObject;
		wp.transform.position = new Vector3(0, 10, -2f);
		wp.SetActive(true);
		Destroy(wp, 3f);
	}
	public void CreateBestScoreParticle() {
		if(HighScoreParticle == null) {
			return;
		}
		GameObject wp = Instantiate(HighScoreParticle) as GameObject;
		wp.transform.position = new Vector3(0, 0, -6f);
		wp.SetActive(true);
		Destroy(wp, 5f);
	}
	public void CreateSpark(Transform targetTrans, Color sparkColor, bool isChild = false) {
		if(SparkParticle == null) {
			return;
		}
		GameObject wp = Instantiate(SparkParticle) as GameObject;
		wp.transform.position = targetTrans.position - new Vector3(0, 0, 2f);
		if(isChild) {
			wp.transform.parent = targetTrans;
		}
		wp.SetActive(true);
		ParticleSystem ps = wp.GetComponent<ParticleSystem>();
		if(ps != null) {
			ParticleSystem.MainModule mm = ps.main;
			mm.startColor = sparkColor;
			ps.Play();
		}
		Destroy(wp, 0.54f);
	}
	public GameObject CreateSparkFountaionBrust(Vector3 pos, Color sparkColor) {
		if(SparkFountaionBrust == null) {
			return null;
		}
		GameObject wp = Instantiate(SparkFountaionBrust) as GameObject;
		wp.transform.position = pos - new Vector3(0, 0, 2f);
		wp.SetActive(true);
		ParticleSystem ps = wp.GetComponent<ParticleSystem>();
		if(ps != null) {
			ParticleSystem.MainModule mm = ps.main;
			mm.startColor = sparkColor;
			ps.Play();
		}
		return wp;
	}
	#endregion
}

