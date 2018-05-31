using UnityEngine;

public class MicDataInfo : MonoBehaviour {
	public event DelegateVoiceSampleRecevied VoiceSampleReceviedEvent;

	#region Event Inovkers
	private void OnVoiceSampleRecevied(float[] samples) {
		if(VoiceSampleReceviedEvent != null) {
			VoiceSampleReceviedEvent.Invoke(samples);
		}
	}
	#endregion
	private string _display = "";
	[Space(5)]
	public SpectrumMicrophone mic;
	public string micName = "Built-in Microphone";
	public int recordDuration = 1;


	[Space(5)]
	public FFTWindow window = FFTWindow.Rectangular;
	public int sampleCount = 8192;                      // Sample Count.
	public float refdB = 0.05f;                         // RMS value for 0 dB.
	public float threshold = 0.01f;                     // Minimum amplitude to extract pitch (recieve anything)
	public float alpha = 0.05f;                         // The alpha for the low pass filter (I don't really understand this).

	public int dBclamp = 160;                             // Used to clamp dB (I don't really understand this either).

	public float rmsVolume { private set; get; }        // Volume in RMS
	public float dBVolume { private set; get; }         // Volume in DB
	public float pitch { private set; get; }            // Pitch - Hz (is this frequency?)
	public int _frequency { private set; get; }         // frequency

	private float[] _samples;                           // Samples
	private float[] _spectrum;                          // Spectrum
	private float[] _spectrumImg;                       // Sprctrum img


	private float _sumOfSamples = 0;
	private float _maxV = 0;
	private int _maxN = 0;
	private float _averageSepctrum = 0;
	private float _freqN;

	void Awake() {
		mic.deviceName = micName;
		mic.sampleRate = sampleCount;
		mic.captureTime = recordDuration;
	}

	void Start() {
		int size = sampleCount * recordDuration;
		int halfSize = size / 2;

		_samples = new float[size];
		_spectrum = new float[halfSize];
		_spectrumImg = new float[halfSize];
		mic.InitData();
	}

	void Update() {
		// Geting Basic samples from microphone using Audio 
		_samples = mic.GetData(0);
		OnVoiceSampleRecevied(_samples);

		mic.GetSpectrumData(window, out _spectrum, out _spectrumImg);
		//_frequency = mic.GetFrequency();

		if(_samples.Length <= 0) {
			return;
		}

		CalculateRMSnDB();
		CalculatePitch();

		//_display = string.Format("RMS: {0} ({1} dB), Pitch: {2}, frequency: {3}",
		//	rmsVolume.ToString("F2"), dBVolume.ToString("F1"), pitch.ToString("F0"), _frequency);
		//Debug.Log(_display);
	}


	private void CalculateRMSnDB() {
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
	private void CalculatePitch() {
		_maxV = 0;
		_maxN = 0;
		_averageSepctrum = 0;
		// Find the highest sample.
		//Debug.Log(_spectrum.Length + " " + SAMPLECOUNT);
		for(int _index = 0; _index < _spectrum.Length; _index++) {
			_averageSepctrum += _spectrum[_index];
			if(_spectrum[_index] > _maxV && _spectrum[_index] > threshold) {
				_maxV = _spectrum[_index];
				_maxN = _index; // maxN is the index of max
			} else {
				//Debug.Log(string.Format("spectrum {0} = {1}", i, spectrum[i]));
			}
		}

		// Pass the index to a float variable
		_freqN = _maxN;

		// Interpolate index using neighbours
		if(_maxN > 0 && _maxN < _spectrum.Length - 1) {
			float dL = _spectrum[_maxN - 1] / _spectrum[_maxN];
			float dR = _spectrum[_maxN + 1] / _spectrum[_maxN];
			_freqN += 0.5f * (dR * dR - dL * dL);
		}
		// Convert index to frequency
		_freqN = _freqN * (44100 / 2) / _spectrum.Length;

		//pitch = 69 + 12 * (Mathf.Log(_frequency / 440f) / Mathf.Log(2));
		//Debug.Log(string.Format("MaxV: {0}, MaxN: {1}, Freq: {2}, Pitch: {3}",
		//	_maxV.ToString("f3"), _maxN.ToString("f0"),
		//	_freqN.ToString("f4"), _pitch.ToString("f4")));
	}
}
