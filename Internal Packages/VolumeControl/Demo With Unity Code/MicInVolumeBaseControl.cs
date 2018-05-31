using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;

public class MicInVolumeBaseControl : MonoBehaviour {

	[Space(5)]
	public MicDataInfo micInfo;
	public VoiceUsed voiceUsedAt = VoiceUsed.None;
	public float noiceRMS;
	public float moveRMSDif;
	public float jumpRMSDif;

	[Space(5)]
	public float nocieTime = 2f;
	private List<float> _rmsInTime = new List<float>();
	private float _timeUsedNoiceCalc = 0;
	[Space(5)]
	public Transform ball;
	public float sideBoundry4Ball;
	public float speed = 2f;
	private int _direction = 1;
	private bool _isJumping = false;

	private AudioSource _audio;

	// Use this for initialization
	void Awake() {
		_audio = GetComponent<AudioSource>();
	}
	void Start() {
		micInfo.VoiceSampleReceviedEvent += VoiceSampleReceviedEventListner;
	}

	// Update is called once per frame
	void Update() {
		sideBoundry4Ball = (Camera.main.aspect * (Camera.main.orthographicSize - 0.2f)) - 0.5f;
		switch(voiceUsedAt) {
		case VoiceUsed.NoiceCalculation:
			_rmsInTime.Add(micInfo.rmsVolume);
			_timeUsedNoiceCalc += Time.unscaledDeltaTime;
			if(_timeUsedNoiceCalc >= nocieTime) {
				StopNocieCalc();
			}
			break;

		case VoiceUsed.Command:
			if(micInfo.rmsVolume >= (noiceRMS + moveRMSDif)) {
				MoveSphere();
			}
			if(micInfo.rmsVolume >= (noiceRMS + moveRMSDif + jumpRMSDif)) {
				JumpSphere();
			}
			break;
		}
	}


	private int guiX, guiY, guiwidth, guiheight, padding, hbuttons, vbuttons;
	float moveVal, jumpVal;
	void OnGUI() {
		guiX = guiY = padding = 8;
		hbuttons = 4;
		vbuttons = 15;
		guiwidth = (short)((Screen.width - ((hbuttons + 2) * padding)) / hbuttons);
		guiheight = (short)((Screen.height - ((vbuttons + 2) * padding)) / vbuttons);

		guiX = guiY = padding;
		if(voiceUsedAt.Equals(VoiceUsed.None)) {
			if(GUI.Button(new Rect(guiX, guiY, guiwidth, guiheight), "Calc Noice")) {
				StartNoiceCalc();
			}
			guiY += padding + guiheight;
		}
		if(voiceUsedAt.Equals(VoiceUsed.None) && !noiceRMS.Equals(0f)) {
			if(GUI.Button(new Rect(guiX, guiY, guiwidth, guiheight), "Play")) {
				voiceUsedAt = VoiceUsed.Command;
			}
			guiY += padding + guiheight;
		}
		if(voiceUsedAt.Equals(VoiceUsed.Command)) {
			GUI.Label(new Rect(guiX, guiY, guiwidth, guiheight), "CVol: " + micInfo.rmsVolume.ToString("f4"));
			guiX += padding + guiwidth;
			GUI.Label(new Rect(guiX, guiY, guiwidth, guiheight), "Noice: " + noiceRMS.ToString("f4"));
			guiX += padding + guiwidth;
			noiceRMS = GUI.HorizontalSlider(new Rect(guiX, guiY, Screen.width - padding - guiX, guiheight), noiceRMS, -1f, 1f);

			//guiX = padding;
			//moveVal = (noiceRMS + moveRMSDif);
			//guiY += padding + (guiheight );
			//GUI.Label(new Rect(guiX, guiY, guiwidth, guiheight), "Move: " + moveVal.ToString("f4"));
			//guiX += padding + guiwidth;
			//moveRMSDif = GUI.HorizontalSlider(new Rect(guiX, guiY, Screen.width - padding - guiX, guiheight), moveRMSDif, 0f, 0.5f);

			jumpVal = (noiceRMS + moveRMSDif + jumpRMSDif);
			guiX = padding;
			guiY += padding + (guiheight);
			GUI.Label(new Rect(guiX, guiY, guiwidth, guiheight), "Jump: " + jumpVal.ToString("f4"));
			guiX += padding + guiwidth;
			jumpRMSDif = GUI.HorizontalSlider(new Rect(guiX, guiY, Screen.width - padding - guiX, guiheight), jumpRMSDif, 0f, 0.5f);

			guiX = padding;
			guiY += padding + (guiheight);
			if(GUI.Button(new Rect(guiX, guiY, guiwidth, guiheight), "Stop")) {
				voiceUsedAt = VoiceUsed.None;
				CreateClip();
			}
		}
		if(!voiceUsedAt.Equals(VoiceUsed.Command) && c != null) {
			guiX = padding;
			guiY += padding + (guiheight);
			if(GUI.Button(new Rect(guiX, guiY, guiwidth, guiheight), "Play Recroded Audio")) {
				_audio.clip = c;
				_audio.Play();
			}
			guiX = padding;
			guiY += padding + (guiheight);
			if(GUI.Button(new Rect(guiX, guiY, guiwidth, guiheight), "Stop Audio")) {
				_audio.Stop();
			}
		}
	}
	private void StartNoiceCalc() {
		_rmsInTime = new List<float>();
		_timeUsedNoiceCalc = 0f;
		voiceUsedAt = VoiceUsed.NoiceCalculation;
	}
	private void StopNocieCalc() {
		voiceUsedAt = VoiceUsed.None;
		float sum = 0;
		_rmsInTime.ForEach(o => sum += o);
		noiceRMS = (sum / _rmsInTime.Count);
		_rmsInTime.Clear();
	}
	private void MoveSphere() {
		ball.Translate(transform.right * speed * Time.deltaTime * _direction, Space.Self);
		if(ball.position.x > sideBoundry4Ball || ball.position.x < -sideBoundry4Ball) {
			_direction *= -1;
		}
	}

	private void JumpSphere() {
		if(_isJumping) {
			return;
		}
		Debug.Log("Call Jump at " + DateTime.Now.ToString("O"));
		_isJumping = true;
		iTween.Stop(ball.gameObject);
		iTween.MoveTo(ball.gameObject, iTween.Hash("name", "jumpUp", "y", 2f, "time", 0.3f
			, "easeType", iTween.EaseType.linear
			, "oncomplete", "MoveBallDown", "oncompletetarget", this.gameObject
		));
		Invoke("JumpEnd", 1.5f);
	}
	private void MoveBallDown() {
		iTween.StopByName(ball.gameObject, "jumpUp");
		iTween.MoveTo(ball.gameObject, iTween.Hash("name", "jumpDown", "y", 0f, "time", 0.2f
			, "easeType", iTween.EaseType.linear
		//, "oncomplete", "JumpEnd", "oncompletetarget", this.gameObject
		));

	}
	private void JumpEnd() {
		_isJumping = false;
	}

	private void Play() {
		voiceUsedAt = VoiceUsed.Command;
	}
	List<float> audioData = new List<float>();
	List<float> noiceData = new List<float>();
	void VoiceSampleReceviedEventListner(float[] samples) {
		if(voiceUsedAt.Equals(VoiceUsed.NoiceCalculation)) {
			noiceData.AddRange(samples);
		}
		if(voiceUsedAt.Equals(VoiceUsed.Command)) {
			audioData.AddRange(samples);
		}

	}

	AudioClip c = new AudioClip();
	public void CreateClip() {
		float[] noice = noiceData.ToArray();
		float[] tmpSample = audioData.ToArray();
		RemoveWaveNoise(noice, tmpSample);

		c = AudioClip.Create("Play", tmpSample.Length, 1, 44100, false);
		c.SetData(tmpSample, 0);
		Debug.Log(c.length + " - " + c.samples + " - " + c.frequency + " - " + c.channels + " - ");
		_audio.clip = c;
	}
	public void RemoveWaveNoise(float[] noise, float[] sample) {

		int noiceSize = noise.Length;
		int sampleBunch = sample.Length / noiceSize;
		for(int sampleIndex = 0; sampleIndex < sampleBunch; sampleIndex++) {
			for(int index = 0; index < noiceSize; ++index) {
				float noiseVal = Mathf.Abs(noise[index]);
				float sampleVal = Mathf.Abs(sample[(index * sampleBunch) + index]);

				//remove the noise data
				if(sampleVal < 0f) {
					if(noiseVal < 0f) {
						sampleVal = Mathf.Min(0f, sampleVal - noiseVal);
					} else {
						sampleVal = Mathf.Min(0f, sampleVal + noiseVal);
					}
				} else {
					if(noiseVal < 0f) {
						sampleVal = Mathf.Max(0f, sampleVal + noiseVal);
					} else {
						sampleVal = Mathf.Max(0f, sampleVal - noiseVal);
					}
				}
				sample[index] = sampleVal;
			}
		}
	}

}
