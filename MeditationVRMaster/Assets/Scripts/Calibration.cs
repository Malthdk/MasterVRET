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

	public float calibrationDuration;

	public float normRespData;

	void Start () {
		caliDataList = new List<float>();
	}

	void Update () {

		// Checks if button is pressed and starts calibration process
		if (Input.GetMouseButtonDown(0) && calibrating == false) {
			StartCoroutine ("Calibrate");
		}
			
/*		if (calibrating && btConnection.respValue < 1000f && btConnection.respValue > 800f) {
			caliDataList.Add (btConnection.respValue);
		} 
		if (finishedCalibrating) {
			normRespData = NormaliseData (btConnection.respValue);
			calibrationText.text = normRespData.ToString();
		}*/
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
		yield return new WaitForSeconds(calibrationDuration);
		calibrating = false;
		finishedCalibrating = true;
	}
}
