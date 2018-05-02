using System.Collections;
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

	// To range between 0 and 1
	float NormaliseData01(List<float> list, float data) {
		float minValue = list.Min ();
		float maxValue = list.Max ();

		float normData = (data - minValue) / (maxValue - minValue);
		return normData;
	}

	public IEnumerator Calibrate()
	{
		float t = 0;
		calibrating = true;
		while (t < calibrationDuration) {
			t += Time.deltaTime;
			// Wait one frame, and repeat.
			yield return null;
		}
		calibrating = false;
		finishedCalibrating = true;
	}
}