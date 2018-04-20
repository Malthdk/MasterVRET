using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Calibration : MonoBehaviour {

	[HideInInspector]
	public List<float> respCaliDataList, alhpaCaliDataList, alphaDataList;
	public bool calibrating, finishedCalibrating;

	public BluetoothConnection btConnection;
	public EEGListener eegListener;
	public Intro introScript;

	public float calibrationDuration, normRespData, normAlphaData, avgAlpha, alphaVariance;
	public GameObject respiParticleSystem;

	void Awake() {
		respCaliDataList = new List<float>();
		alhpaCaliDataList = new List<float>();
	}

	void Update () {
		if (finishedCalibrating && btConnection.respValue < 1000f && btConnection.respValue > 800f) {
			normRespData = NormaliseData (respCaliDataList ,btConnection.respValue);
		} 
		// NOT USING THIS - NEED TO FIGURE OUT HOW TO USE BASELINE EEG
		if (finishedCalibrating) {
			normAlphaData = NormaliseData (alhpaCaliDataList, eegListener.LowAlpha);
		} 
		if (introScript.startCalibration && !calibrating && !finishedCalibrating) {
			StartCoroutine ("Calibrate");
		}
	}

	// To range between -1 and 1
	float NormaliseData(List<float> list, float data) {
		float minValue = list.Min ();
		float maxValue = list.Max ();

		float normData = 2f * ((data - minValue) / (maxValue - minValue)) - 1;
		return normData;
	}

	// NEED TO RETHINK THIS ONE (NOT ACCURATE ENOUGH!!!!!)
	public IEnumerator AlphaVariance()
	{
		float oldAvgAlpha = avgAlpha;
		float t = 0;
		while (t < 5f) {
			t += Time.deltaTime;
			alphaDataList.Add (eegListener.LowAlpha);
			// Wait one frame, and repeat.
			yield return null;
		}
		avgAlpha = alphaDataList.Average ();
		alphaVariance = avgAlpha / oldAvgAlpha;
		StartCoroutine ("AlphaVariance");
	}

	public IEnumerator Calibrate()
	{
		float t = 0;
		calibrating = true;
		while (t < calibrationDuration) {
			t += Time.deltaTime;
			if (btConnection.respValue < 1000f && btConnection.respValue > 800f) {
				respCaliDataList.Add (btConnection.respValue);
			} 
			// NOT USING THIS - NEED TO FIGURE OUT HOW TO USE BASELINE EEG
			alhpaCaliDataList.Add (eegListener.LowAlpha);
			// Wait one frame, and repeat.
			yield return null;
		}
		calibrating = false;
		finishedCalibrating = true;
	}
}