using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScript : MonoBehaviour {

	[TextArea]
	public string welcomeText;

	public float initialDelay;

	public TextMesh textObject;

	public Calibration calibrationScript;

	void Awake () {
		StartCoroutine ("DisplayText");
	}

	void Update () {
		if (calibrationScript.finishedCalibrating) {
			textObject.gameObject.SetActive (false);
		} 
	}

	IEnumerator  DisplayText ()
	{
		textObject.gameObject.SetActive (false);
		yield return new WaitForSeconds(initialDelay);
		textObject.text = welcomeText;
		textObject.gameObject.SetActive (true);
	}
}
