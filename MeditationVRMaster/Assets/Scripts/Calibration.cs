﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Calibration : MonoBehaviour {

	[HideInInspector]
	public List<float> respCaliDataList;
	public bool calibrating, finishedCalibrating;

	public BluetoothConnection btConnection;
	public EEGListener eegListener;
	public Intro introScript;
	public Text debugText;

	public float calibrationDuration, normRespData;
	public GameObject respiParticleSystem, eegManager;

	void Awake() {
		respCaliDataList = new List<float>();
	}

	void Update () {
		if (finishedCalibrating && btConnection.respValue < 300f && btConnection.respValue > 100f) {
			normRespData = NormaliseData (respCaliDataList ,btConnection.respValue);
			debugText.text = "Resp data: " + normRespData.ToString ();
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
			if (btConnection.respValue < 300f && btConnection.respValue > 100f) {
				respCaliDataList.Add (btConnection.respValue);
			} 
			// Wait one frame, and repeat.
			yield return null;
		}
		eegManager.SetActive (true);
		calibrating = false;
		finishedCalibrating = true;
	}
}