using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Calibration : MonoBehaviour {

	[HideInInspector]
	public List<float> respCaliDataList, alhpaCaliDataList;
	public bool calibrating, finishedCalibrating;

	public BluetoothConnection btConnection;
	public EEGListener eegListener;
	public Intro introScript;

	public float calibrationDuration, normRespData, alphaAvg;
	public GameObject respiParticleSystem, eegManager;

	void Awake() {
		respCaliDataList = new List<float>();
		alhpaCaliDataList = new List<float>();
	}

	void Update () {
		if (finishedCalibrating && btConnection.respValue < 1000f && btConnection.respValue > 800f) {
			normRespData = NormaliseData (respCaliDataList ,btConnection.respValue);
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

	public IEnumerator Calibrate()
	{
		float t = 0;
		calibrating = true;
		while (t < calibrationDuration) {
			t += Time.deltaTime;
			if (btConnection.respValue < 1000f && btConnection.respValue > 800f) {
				respCaliDataList.Add (btConnection.respValue);
			} 
			alhpaCaliDataList.Add (eegListener.LowAlpha);
			// Wait one frame, and repeat.
			yield return null;
		}
		alphaAvg = alhpaCaliDataList.Average();
		eegManager.SetActive (true);
		calibrating = false;
		finishedCalibrating = true;
	}
}