using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class HandleTextFile : MonoBehaviour
{
	public EEGListener eeg;
	public BluetoothConnection bt;
	private string startTime;
	private TextWriter eegWriter, respWriter;
	private int seconds;

	void Start() {
		eeg = gameObject.GetComponent<EEGListener> ();
		startTime = DateTime.UtcNow.ToString ("HH:mm_dd_MMMM_yyyy");
		eegWriter = new StreamWriter(Application.persistentDataPath + "/eegData_" + startTime + ".txt");
		respWriter = new StreamWriter(Application.persistentDataPath + "/respData_" + startTime + ".txt");
		eegWriter.WriteLine("Time, SignalLevel, Attention, Meditation, Zone, Delta, Theta, LowAlpha, HighAlpha, LowBeta, HighBeta, LowGamma, HighGamma, Raw");
		StartCoroutine("WriteData");
	}

	void Update() {
		WriteRespData ();
	}

	IEnumerator WriteData() {
		seconds++;
		eegWriter.WriteLine(seconds + ", " + eeg.PoorSignal + ", " + eeg.Attention + ", " + eeg.Meditation + ", " + eeg.Zone + ", " + eeg.Delta + ", " + eeg.Theta + ", " + eeg.LowAlpha + ", " + eeg.HighAlpha + ", " + eeg.LowBeta + ", " + eeg.HighBeta + ", " + eeg.LowGamma + ", " + eeg.HighGamma + ", " + eeg.Raw);
		yield return new WaitForSeconds (1f);
		StartCoroutine("WriteData");
	}

	void WriteRespData() {
		respWriter.WriteLine (bt.respValue);
	}

	void OnApplicationQuit() {
		StopCoroutine("WriteData");
		eegWriter.Close();
	}

}