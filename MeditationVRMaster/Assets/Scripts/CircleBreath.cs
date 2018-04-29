using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CircleBreath : MonoBehaviour {

	public Calibration calibrationScript;
	private float respValue;

	private Vector3 startScale;

	void Awake () {
		startScale = transform.localScale;
		StartCoroutine ("SmoothenValues");
	}

	IEnumerator SetScale() 
	{
		float scaleOld = transform.localScale.x;
		float scaleNew = transform.localScale.x * respValue;
		float scale;
		float t = 0;
		while (t < .5f) {
			t += Time.deltaTime;
			float blend = Mathf.Clamp01 (t / .5f);

			scale = Mathf.Lerp (scaleOld, scaleNew, blend);
			transform.localScale = startScale + (new Vector3(scale,scale,scale));
			yield return null;
		}
	}

	IEnumerator SmoothenValues() {
		List<float> respValues = new List<float>();
		float t = 0;
		while (t < .5f) {
			t += Time.deltaTime;
			respValues.Add (calibrationScript.normRespData);
			yield return null;
		}
		respValue = respValues.Average ();
		respValues.Clear();
		StartCoroutine ("SetScale");
		StartCoroutine ("SmoothenValues");
	}
}