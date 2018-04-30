using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CircleBreath : MonoBehaviour {

	public Calibration calibrationScript;
	public float duration = .2f;
	public float debugVal;
	private float respValue;

	private Vector3 startScale;

	void Awake () {
		startScale = transform.localScale;
		StartCoroutine ("SmoothenValues");
	}

	IEnumerator SetScale() 
	{
		float scaleOld = transform.localScale.x;
		float scale;
		float t = 0;
		while (t < duration) {
			t += Time.deltaTime;
			float blend = Mathf.Clamp01 (t / duration);

			scale = Mathf.Lerp (scaleOld, respValue, blend);
			transform.localScale = new Vector3(scale,scale,scale);
			yield return null;
		}
	}

	IEnumerator SmoothenValues() {
		List<float> respValues = new List<float>();
		float t = 0;
		while (t < duration) {
			t += Time.deltaTime;
			respValues.Add (calibrationScript.normRespData);
			yield return null;
		}
		respValue = respValues.Average ();
		respValue += .5f;
		respValue *= 12f;
		respValues.Clear();
		StartCoroutine ("SetScale");
		StartCoroutine ("SmoothenValues");
	}
}