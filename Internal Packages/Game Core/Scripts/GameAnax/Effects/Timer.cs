using UnityEngine;

using GameAnax.Core.Extension;
using GameAnax.Core.UI.Buttons;
using GameAnax.Core.Utility;


using GameAnax.Game.Utility;
using GameAnax.Game.Utility.Ad;


public class Timer : MonoBehaviour {

	[SerializeField]
	private string timeVar;
	[SerializeField]
	private Renderer meshRenderer;
	[SerializeField]
	private TextMesh timerText;
	[SerializeField]
	private Button button;

	[SerializeField]
	private bool _isTimerWorking = false;
	[SerializeField]
	private float _timeSpent = 0f;
	[SerializeField]
	private float _timeDif;
	[SerializeField]
	private int _timerToShow;

	// Update is called once per frame
	void Update() {
		if(!_isTimerWorking) { return; }
		_timeSpent += Time.unscaledDeltaTime;
		_timerToShow = (int)((GameUtility.Me.eMode.oneMoreTimerSecs - _timeSpent).RoundUp(0));
		_timeDif = _timeSpent / GameUtility.Me.eMode.oneMoreTimerSecs;
		meshRenderer.material.SetFloat(timeVar, _timeDif);
		if(_timerToShow <= 0) {
			timerText.text = "0";
		} else {
			timerText.text = _timerToShow.ToString();
		}
		if(_timeDif >= 1) {
			StopTimer(false);
		}
	}

	public void PrepareTimer() {
		_isTimerWorking = false;
		transform.localScale = Vector3.one;
		timerText.text = GameUtility.Me.eMode.oneMoreTimerSecs.ToString();
		_timeSpent = 0f;
		meshRenderer.material.SetFloat(timeVar, 0f);
		button.SetDisable(false);
	}
	public void StartTimer() {
		_timeSpent = 0f;
		meshRenderer.material.SetFloat(timeVar, 0f);
		timerText.text = GameUtility.Me.eMode.oneMoreTimerSecs.ToString();
		_isTimerWorking = true;
	}
	public void StopTimer(bool isClicked) {
		_isTimerWorking = false;
		button.SetDisable(true);
		iTween.ScaleTo(gameObject, iTween.Hash("x", 0f, "y", 0f, "time", 0.2f, "oncomplete", "DeactiveTimer"));
		if(!isClicked) {
			AdsMCG.Me.ShowAd("GameOver");
		}
	}
	private void DeactiveTimer() {
		iTween.Stop(gameObject);
		gameObject.SetActive(false);
	}

}
