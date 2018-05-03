using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class HandleTextFile : MonoBehaviour
{
	public EEGListener eeg;
	public Intro introScript;
	private string startTime;
	private TextWriter eegWriter;
	private int seconds;
	private bool startedWriting;

	void Start() {
		eeg = gameObject.GetComponent<EEGListener> ();
		startTime = DateTime.UtcNow.ToString ("HH:mm_MMMM_dd_yyyy");
		eegWriter = new StreamWriter(Application.persistentDataPath + "/VR_" + startTime + ".txt");
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
		eegWriter.WriteLine(seconds + ", " + eeg.PoorSignal + ", " + eeg.Attention + ", " + eeg.Meditation + ", " + eeg.Zone + ", " + eeg.Delta + ", " + eeg.Theta + ", " + eeg.LowAlpha + ", " + eeg.HighAlpha + ", " + eeg.LowBeta + ", " + eeg.HighBeta + ", " + eeg.LowGamma + ", " + eeg.HighGamma + ", " + eeg.Raw);
		yield return new WaitForSeconds (1f);
		StartCoroutine("WriteData");
	}

	void OnApplicationQuit() {
		StopCoroutine("WriteData");
		eegWriter.Close();
	}

}