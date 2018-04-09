using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBreath : MonoBehaviour {

	public Calibration calibrationScript;

	private Vector3 startScale;

	void Awake () {
		startScale = transform.localScale;
	}

	void LateUpdate () {

		if (calibrationScript.finishedCalibrating) {
			transform.localScale = startScale + (new Vector3(calibrationScript.normRespData,calibrationScript.normRespData,calibrationScript.normRespData) * 2f);
		}

	}
}