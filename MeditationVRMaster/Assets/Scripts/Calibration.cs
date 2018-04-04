using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Calibration : MonoBehaviour {

	[HideInInspector]
	public List<float> caliDataList;
	public bool calibrating = true;

	public BluetoothConnection btConnection;

	public TextMesh calibrationText;

	public float calibrationDuration = 5f;

	public float normRespData;

	void Start () {
		caliDataList = new List<float>();
		StartCoroutine ("Calibrate");
	}

	void Update () {
		if (calibrating && btConnection.respValue != 0f) {
			caliDataList.Add (btConnection.respValue);
		} 
		if (!calibrating) {
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
		yield return new WaitForSeconds(calibrationDuration);
		calibrating = false;
	}
}
