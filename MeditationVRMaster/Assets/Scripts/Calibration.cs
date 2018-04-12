using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Calibration : MonoBehaviour {

	[HideInInspector]
	public List<float> caliDataList;
	public bool calibrating, finishedCalibrating;

	public BluetoothConnection btConnection;
	public Intro introScript;

	[TextArea]
	public string welcomeText;

	public float calibrationDuration;

	public float normRespData;

	public GameObject respiParticleSystem;

	void Awake() {
		caliDataList = new List<float>();
	}

	void Update () {
		if (finishedCalibrating && btConnection.respValue < 1000f && btConnection.respValue > 800f) {
			normRespData = NormaliseData (btConnection.respValue);
		}

		if (introScript.startCalibration && !calibrating && !finishedCalibrating) {
			StartCoroutine ("Calibrate");
		}
	}

	// To range between -1 and 1
	float NormaliseData(float data) {
		float minValue = caliDataList.Min ();
		float maxValue = caliDataList.Max ();

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
				caliDataList.Add (btConnection.respValue);
			} 
			// Wait one frame, and repeat.
			yield return null;
		}

		calibrating = false;
		finishedCalibrating = true;
	}
}