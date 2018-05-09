using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Calibration : MonoBehaviour {

	public bool calibrating, finishedCalibrating;

	public BluetoothConnection btConnection;
	public EEGListener eegListener;
	public Interaction interScript;
	public Image calibrationImage;

	public float calibrationDuration, normRespData;


	void Update () {
		if (interScript.startCalibrating && !calibrating && !finishedCalibrating) {
			StartCoroutine ("Calibrate");
		}
	}

	public IEnumerator Calibrate()
	{
		float t = 0;
		calibrating = true;
		while (t < calibrationDuration) {
			t += Time.deltaTime;
			float timeLeft = t / calibrationDuration;
			calibrationImage.fillAmount = timeLeft;
			// Wait one frame, and repeat.
			yield return null;
		}
		calibrating = false;
		finishedCalibrating = true;
	}
}