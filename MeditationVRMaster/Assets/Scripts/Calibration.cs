using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Calibration : MonoBehaviour {

	[HideInInspector]
	public List<float> caliDataList;
	public bool calibrating, finishedCalibrating;

	public BluetoothConnection btConnection;

	public TextMesh calibrationText;

	[TextArea]
	public string welcomeText;

	public float calibrationDuration;

	public float normRespData;

	public GameObject respiParticleSystem;

	void Awake() {
		caliDataList = new List<float>();
	}

	void Start () {
		StartCoroutine ("Calibrate");
	}

	void Update () {

		// Checks if button is pressed and starts calibration process
		/*if (Input.GetMouseButtonDown(0) && calibrating == false) {
			StartCoroutine ("Calibrate");
		}*/
			
		if (calibrating && btConnection.respValue < 1000f && btConnection.respValue > 800f) {
			caliDataList.Add (btConnection.respValue);
		} 

		if (finishedCalibrating && btConnection.respValue < 1000f && btConnection.respValue > 800f) {
			normRespData = NormaliseData (btConnection.respValue);
		}
	}

	// To range between -1 and 1
	float NormaliseData(float data) {
		float minValue = caliDataList.Min ();
		float maxValue = caliDataList.Max ();

		float normData = 2f * ((data - minValue) / (maxValue - minValue)) - 1;
		return normData;
	}

	IEnumerator Calibrate()
	{
		calibrating = true;
		calibrationText.text = welcomeText;
		yield return new WaitForSeconds(calibrationDuration);
		respiParticleSystem.SetActive (true);
		calibrationText.text = " ";
		calibrating = false;
		finishedCalibrating = true;
	}
}