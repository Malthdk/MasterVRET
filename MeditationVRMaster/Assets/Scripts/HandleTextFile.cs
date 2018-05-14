using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class HandleTextFile : MonoBehaviour
{
	public EEGListener eeg;
	public Intro introScript;
	private float delta, theta, lowAlpha, highAlpha, lowBeta, highBeta, lowGamma, highGamma;
	private string startTime;
	private TextWriter eegWriter;
	private int seconds;
	private bool startedWriting;

	void Start() {
		eeg = gameObject.GetComponent<EEGListener> ();
		startTime = DateTime.UtcNow.ToString ("HH:mm_MMMM_dd_yyyy");
		eegWriter = new StreamWriter(Application.persistentDataPath + "/BIO_" + startTime + ".txt");
		eegWriter.WriteLine("Time, SignalLevel, Attention, Meditation, Zone, Delta, Theta, LowAlpha, HighAlpha, LowBeta, HighBeta, LowGamma, HighGamma, Raw");
		StartCoroutine("WriteData");
	}

	void Update() {
		if (introScript.startCalibration && !startedWriting) {
			startedWriting = true;
			StartCoroutine ("WriteData");
		}
	}

	IEnumerator WriteData() {
		seconds++;
		eegWriter.WriteLine(seconds + ", " + eeg.PoorSignal + ", " + eeg.Attention + ", " + eeg.Meditation + ", " + eeg.Zone + ", " + delta + ", " + theta + ", " + lowAlpha + ", " + highAlpha + ", " + lowBeta + ", " + highBeta + ", " + lowGamma + ", " + highGamma + ", " + eeg.Raw);
		yield return new WaitForSeconds (1f);
		StartCoroutine("WriteData");
		StartCoroutine("CalcAverage");
	}

	IEnumerator CalcAverage() {
		float delta_ = 0; 
		float theta_ = 0; 
		float lowAlpha_ = 0;
		float highAlpha_ = 0;
		float lowBeta_ = 0;
		float highBeta_ = 0;
		float lowGamma_ = 0;
		float highGamma_ = 0;
		int counter = 0;

		float t = 0;
		while (t < 1f) {
			t += Time.deltaTime;
			delta_ += eeg.Delta; 
			theta_ += eeg.Theta; 
			lowAlpha_ += eeg.LowAlpha;
			highAlpha_ += eeg.HighAlpha;
			lowBeta_ += eeg.LowBeta;
			highBeta_ += eeg.HighBeta;
			lowGamma_ += eeg.LowGamma;
			highGamma_ += eeg.HighGamma;
			counter++;
			yield return null;
		}

		delta = delta_ / counter; 
		theta = theta_ / counter; 
		lowAlpha = lowAlpha_ / counter;
		highAlpha = highAlpha_ / counter;
		lowBeta = lowBeta_ / counter;
		highBeta = highBeta_ / counter;
		lowGamma = lowGamma_ / counter;
		highGamma = highGamma_ / counter;
	}

	void OnApplicationQuit() {
		StopCoroutine("WriteData");
		eegWriter.Close();
	}

}