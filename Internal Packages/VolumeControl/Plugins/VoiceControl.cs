using UnityEngine;
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//
using MiniJSON;

using GameAnax.Core.Singleton;


public delegate void DelegateVoiceSampleRecevied(float[] samples);
[PersistentSignleton(true)]
public class VoiceControl : SingletonAuto<VoiceControl> {
	public event DelegateVoiceSampleRecevied VoiceSampleReceviedEvent;

	[Space(5)]
	public FFTWindow window = FFTWindow.Rectangular;
	public int sampleCount;                             // Sample Count.
	public float refdB = 0.05f;                         // RMS value for 0 dB.
	public float threshold = 0.01f;                     // Minimum amplitude to extract pitch (recieve anything)
	public float alpha = 0.05f;                         // The alpha for the low pass filter (I don't really understand this).

	public int dBclamp = 160;                           // Used to clamp dB (I don't really understand this either).

	public float rmsVolume { private set; get; }        // Volume in RMS
	public float dBVolume { private set; get; }         // Volume in DB

	private float[] _samples;                           // Samples
	private float _sumOfSamples = 0;
	private bool _isMicStarted = false;
	private bool _isInitialized = false;

	#region Mono Action
	void Awake() { }
	void Start() {
		this.InitMicUses();
	}
	void OnApplicationQuit() {

		StopMicUses();
	}
	void OnApplicationPause(bool isGoesBackground) {
		if(_isMicStarted) {
			if(isGoesBackground) {
				StopMicUses(true);
			} else {
				StartMicUses();
			}
		}
	}
	#endregion

	#region Event Inovkers
	private void OnVoiceSampleRecevied(float[] samples) {
		if(VoiceSampleReceviedEvent != null) {
			VoiceSampleReceviedEvent.Invoke(samples);
		}
	}
	#endregion

	#region iOS Method Declaration
#if UNITY_IOS
	[DllImport("__Internal")]
	private static extern bool askForMicPermission();
	[DllImport("__Internal")]
	private static extern int isHasMicPermssion();
	[DllImport("__Internal")]
	private static extern void initPitchy();
	[DllImport("__Internal")]
	private static extern void startPitchy();
	[DllImport("__Internal")]
	private static extern void stopPitchy();
	[DllImport("__Internal")]
	private static extern bool isPitchyStarted();

#endif
	#endregion

	#region Metods for Mic Management
	public MicPermission IsMicPermission() {
		int c = -1;
#if UNITY_IOS
		c = isHasMicPermssion();
#endif
		return (MicPermission)c;
	}
	public bool AskMicPermission() {
		bool isPermission = false;
#if UNITY_IOS
		isPermission = askForMicPermission();
#endif
		return isPermission;
	}
	public void InitMicUses() {
		if(_isInitialized) {
			return;
		}
#if UNITY_EDITOR
		Debug.Log("Mic data is not supported from Unity Editor, currently is using iOS / Android Native code");
		return;
#endif

#if UNITY_IOS
		if(Application.platform.Equals(RuntimePlatform.IPhonePlayer)
			|| Application.platform.Equals(RuntimePlatform.tvOS)
		) {
			initPitchy();
		}
#endif
	}
	public bool IsMicUses() {
#if UNITY_EDITOR
		Debug.Log("Mic data is not supported from Unity Editor, currently is using iOS / Android Native code");
		return false;
#endif
#if UNITY_IOS
		if(Application.platform.Equals(RuntimePlatform.IPhonePlayer)
			|| Application.platform.Equals(RuntimePlatform.tvOS)
		) {
			return isPitchyStarted();
		}
#endif
		return false;
	}
	public void StartMicUses() {
#if UNITY_EDITOR
		Debug.Log("Mic data is not supported from Unity Editor, currently is using iOS / Android Native code");
		return;
#endif
		_isMicStarted = true;
#if UNITY_IOS
		if(Application.platform.Equals(RuntimePlatform.IPhonePlayer)
			|| Application.platform.Equals(RuntimePlatform.tvOS)
		) {
			startPitchy();
		}
#endif
	}
	public void StopMicUses() {
		StopMicUses(false);
	}
	void StopMicUses(bool fromApplicationEvent) {
#if UNITY_EDITOR
		Debug.Log("Mic data is not supported from Unity Editor, currently is using iOS / Android Native code");
		return;
#endif
		if(!fromApplicationEvent) {
			_isMicStarted = false;
		}

#if UNITY_IOS
		if(Application.platform.Equals(RuntimePlatform.IPhonePlayer)
			|| Application.platform.Equals(RuntimePlatform.tvOS)
		) {

			stopPitchy();
		}
#endif
	}


	#endregion

	#region Listeaning data form Native Code and Calculation
	private object _tSampleFromJson;
	private List<object> _rawJSonSample;
	private string _error = "";
	private Dictionary<string, object> _jsonData = new Dictionary<string, object>();
	//	
	private void PitchyInitialized() {
		_isInitialized = true;
		Debug.Log("Pithy is Initialized");
	}
	private void PitchyVoiceSampleReceived(string sampleArrayInJSon) {
		//Debug.Log("Data Recevied " + DateTime.Now.ToString("O"));
		//Debug.Log("sampleArrayInJSon: " + sampleArrayInJSon);
		try {
			//Debug.Log("Converting JSon string Data in iDic");
			IDictionary o = Json.Deserialize(sampleArrayInJSon) as IDictionary;

			//Debug.Log("Converting iDic Data in Dictionry of string & object");
			_jsonData = o as Dictionary<string, object>;

			//Debug.Log("Trying to get Sampples from Dictionary");
			if(_jsonData.ContainsKey("samples")) {
				_tSampleFromJson = _jsonData["samples"];
				if(_tSampleFromJson != null) {
					//Debug.Log("Converting Samples data in list of object");
					_rawJSonSample = (_tSampleFromJson as List<object>);
				}
			} else {
				//Debug.Log("json data does not contain voice samples");
			}
		} catch(Exception ex) {
			_error = string.Format("ERROR: {0}, InnerMessage:D {1}", ex.Message, ex.InnerException);
			Debug.LogWarning(_error);
			return;
		}

		if(null == _rawJSonSample) {
			_error = string.Format("No sample data");
			Debug.LogError(_error);
			return;
		}
		//if(!IsPowerOfTwo(_rawJSonSample.Count)) {
		//	_error = string.Format("Sample data is not power of two..." + _rawJSonSample.Count);
		//	Debug.LogError(_error);
		//	return;
		//}


		_samples = GetSamples(_rawJSonSample);
		if(!_rawJSonSample.Count.Equals(_samples.Length)) {
			_error = string.Format("Raw sample data from Mic may have some invalid value,\nraw sample data must have value between -1 to 1, " + _rawJSonSample.Count, " - " + _samples.Length);
			Debug.LogError(_error);
			//return;
		}
		//Debug.Log("Data Validated " + DateTime.Now.ToString("O"));



		CalculateRMSnDB();
		//Debug.Log("Data Calucalted in dB & RMS " + DateTime.Now.ToString("O"));

		OnVoiceSampleRecevied(_samples);
	}
	#endregion

	#region Calculation on Data recevied from Native Code
	private bool IsPowerOfTwo(int length) {
		bool ret = (length & (length - 1)) == 0;
		return ret;
	}
	private float[] GetSamples(List<object> rawSamples) {
		List<float> sam = new List<float>();
		float f = -2;
		string s = "-2";
		foreach(object o in rawSamples) {
			s = o.ToString();
			float.TryParse(s, out f);
			if(f <= -1 || f >= 1) {
				continue;
			}
			sam.Add(f);
		}
		return sam.ToArray();
	}
	private void CalculateRMSnDB() {
		if(_samples.Length <= 0) {
			return;
		}
		// Sums squared samples
		_sumOfSamples = 0;
		int _count = _samples.Length;
		//Debug.Log("sample Count: " + _count);
		for(int i = 0; i < _count; i++) {
			_sumOfSamples += Mathf.Pow(_samples[i], 2);
		}

		// RMS is the square root of the average value of the samples.
		rmsVolume = Mathf.Sqrt(_sumOfSamples / _count);
		dBVolume = 20 * Mathf.Log10(rmsVolume / refdB);

		// Clamp it to {clamp} min
		if(dBVolume < -dBclamp) {
			dBVolume = -dBclamp;
		}
	}
	#endregion
}
public enum MicPermission {
	Undetermined = -1,
	Denied = 0,
	Granted = 1
}