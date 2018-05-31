using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

/// <summary>
/// Extend the Unity Microphone to enable exacting spectrum data
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SpectrumMicrophone : MonoBehaviour {
	/// <summary>
	/// Make sure sample rate * capture time is base two
	/// </summary>
	public int captureTime = 1;

	/// <summary>
	/// Make sure sample rate is base two
	/// </summary>
	public int sampleRate = 8192;

	/// <summary>
	/// Make sure InitData is called if sample rate or capture time is changed
	/// </summary>
	float[] m_fetchData = null;
	float[] m_complex = null;
	float[] m_spectrumReal = null;
	float[] m_spectrumImag = null;
	new AudioSource audio;


	void Awake() {
		audio = GetComponent<AudioSource>();
	}
	/// <summary>
	/// Initialize data arrays
	/// </summary>
	public void InitData() {
		CleanUp();
		int size = sampleRate * captureTime;
		int halfSize = size / 2;
		m_fetchData = new float[size];
		m_complex = new float[size];
		m_spectrumReal = new float[halfSize];
		m_spectrumImag = new float[halfSize];
	}

	/// <summary>
	/// The selected microphone
	/// </summary>
	public string deviceName = string.Empty;

	/// <summary>
	/// Processing instances
	/// </summary>
	FourierTransform m_fourierTransform = new FourierTransform();

	/// <summary>
	/// Track the last mic position
	/// </summary>
	int m_lastPosition = 0;

	void OnEnable() {
		InitData();
	}

	public void CleanUp() {
		if(!string.IsNullOrEmpty(deviceName) &&
			Microphone.IsRecording(deviceName)) {
			Microphone.End(deviceName);
		}

		if(null != audio.clip) {
			UnityEngine.Object.DestroyImmediate(audio.clip, true);
			audio.clip = null;
		}
	}

	void OnApplicationQuit() {
		CleanUp();
	}

	public float[] GetData(int sampleOffset) {
		if(null == audio.clip) {
			return m_fetchData;
		}
		if(Microphone.IsRecording(deviceName)) {
			bool hasChanged = true;
			int position = Microphone.GetPosition(deviceName);
			hasChanged = position != m_lastPosition;
			m_lastPosition = position;
			if(hasChanged) {
				audio.clip.GetData(m_fetchData, sampleOffset);
			}
		}
		return m_fetchData;
	}

	public float[] GetLastData() {
		return m_fetchData;
	}

	public int GetFrequency() {
		if(null == audio.clip) {
			return 0;
		} else {
			return audio.clip.frequency;
		}
	}

	public int GetPosition() {
		if(Microphone.IsRecording(deviceName)) {
			return Microphone.GetPosition(deviceName);
		} else {
			return 0;
		}
	}

	/// <summary>
	/// Remove noise from the data
	/// </summary>
	/// <param name="noise"></param>
	/// <param name="sample"></param>
	public void RemoveWaveNoise(float[] noise, float[] sample) {
		if(null != noise &&
			noise.Length == sample.Length) {
			int size = noise.Length;
			for(int index = 0; index < size; ++index) {
				float noiseVal = Mathf.Abs(noise[index]);
				float sampleVal = Mathf.Abs(sample[index]);

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

	/// <summary>
	/// Remove noise from the data
	/// </summary>
	/// <param name="noise"></param>
	/// <param name="spectrum"></param>
	public void RemoveSpectrumNoise(float[] noise, float[] spectrum) {
		if(null == noise) {
			return;
		}

		if(null == spectrum) {
			return;
		}

		if(noise.Length == spectrum.Length) {
			int halfSize = noise.Length;
			for(int index = 0; index < halfSize; ++index) {
				float noiseVal = Mathf.Abs(noise[index]);
				float sampleVal = Mathf.Abs(spectrum[index]);

				//remove the noise data
				sampleVal = Mathf.Max(0f, sampleVal - noiseVal);

				spectrum[index] = sampleVal;
			}
		}
	}

	/// <summary>
	/// Normalize the audio wave
	/// </summary>
	/// <param name="samples"></param>
	public void NormalizeWave(float[] samples) {
		if(null == samples) {
			return;
		}

		//find min and max
		float min = 0;
		float max = 0;
		int size = samples.Length;
		for(int index = 0; index < size; ++index) {
			float val = samples[index];
			if(val > 0f) {
				if(val > max) {
					max = val;
				}
			} else {
				val = -val;
				if(val > min) {
					min = val;
				}
			}
		}

		for(int index = 0; index < size; ++index) {
			float val = samples[index];
			if(val > 0f) {
				if(max != 0f) {
					val /= max;
				}
			} else {
				if(min != 0f) {
					val /= max;
				}
			}

			samples[index] = val;
		}
	}

	public class SpectrumDataArgs : EventArgs {
		public float[] Spectrum;
	}

	/// <summary>
	/// Event for spectrum data changes
	/// </summary>
	public EventHandler<SpectrumDataArgs> SpectrumChanged = null;

	public void GetSpectrumData(float[] samples, float[] spectrumReal, float[] spectrumImg, FFTWindow fftWindow) {
		if(null == samples) {
			return;
		}

		int size = samples.Length;
		if(null == m_complex ||
			m_complex.Length != size) {
			m_complex = new float[size];
		}

		m_fourierTransform.FFT(samples, m_complex, spectrumReal, spectrumImg);
	}

	public void GetSpectrumData(FFTWindow fftWindow, out float[] spectrumReal, out float[] spectrumImag) {
		m_fourierTransform.FFT(m_fetchData, m_complex, m_spectrumReal, m_spectrumImag);

		// notify event that data changed
		if(null != SpectrumChanged) {
			SpectrumDataArgs args = new SpectrumDataArgs();
			args.Spectrum = m_spectrumReal;
			SpectrumChanged.Invoke(this, args);
		}

		spectrumReal = m_spectrumReal;
		spectrumImag = m_spectrumImag;
	}

	public float[] GetLastSpectrumData() {
		return m_spectrumReal;
	}

	void Update() {
		try {
			if(null == audio.clip) {
				//if(Application.isWebPlayer) {
				//	switch(Application.platform) {
				//	case RuntimePlatform.OSXWebPlayer:
				//	case RuntimePlatform.WindowsWebPlayer:
				//		if(!Application.HasUserAuthorization(UserAuthorization.Microphone)) {
				//			Application.RequestUserAuthorization(UserAuthorization.Microphone);
				//		}
				//		if(!Application.HasUserAuthorization(UserAuthorization.Microphone)) {
				//			return;
				//		}
				//		break;
				//	}
				//}
			}

			if(string.IsNullOrEmpty(deviceName)) {
				return;
			}

			if(null == audio.clip) {
				audio.clip = Microphone.Start(deviceName, true, captureTime, sampleRate);
			}
		} catch(System.Exception ex) {
			Debug.Log(string.Format("Update exception={0}", ex));
		}
	}
}