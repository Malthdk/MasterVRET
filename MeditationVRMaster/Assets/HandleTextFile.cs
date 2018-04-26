using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class HandleTextFile : MonoBehaviour
{
	public EEGListener eeg;
	private string startTime;
	private TextWriter writer;
	private int seconds;

	void Start() {
		eeg = gameObject.GetComponent<EEGListener> ();
		startTime = DateTime.UtcNow.ToString ("HH:mm_dd_MMMM_yyyy");
		writer = new StreamWriter(Application.persistentDataPath + "/dataLog_" + startTime + ".txt");
		writer.WriteLine("Time, SignalLevel, Attention, Meditation, Zone, Delta, Theta, LowAlpha, HighAlpha, LowBeta, HighBeta, LowGamma, HighGamma, Raw");
		StartCoroutine("WriteData");
	}

	IEnumerator WriteData() {
		seconds++;
		writer.WriteLine(seconds + ", " + eeg.PoorSignal + ", " + eeg.Attention + ", " + eeg.Meditation + ", " + eeg.Zone + ", " + eeg.Delta + ", " + eeg.Theta + ", " + eeg.LowAlpha + ", " + eeg.HighAlpha + ", " + eeg.LowBeta + ", " + eeg.HighBeta + ", " + eeg.LowGamma + ", " + eeg.HighGamma + ", " + eeg.Raw);
		yield return new WaitForSeconds (1f);
		StartCoroutine("WriteData");
	}

	void OnApplicationQuit() {
		StopCoroutine("WriteData");
		writer.Close();
	}

}