using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CircleBreath : MonoBehaviour {

	public Calibration calibrationScript;
	public float duration = .2f;
	public float debugVal;
	private float respValue, smoothRespValue, prevSmoothRespValue;
	private bool initialisedSmoothing;

	private Vector3 startScale;

	void Awake () {
		startScale = transform.localScale;
		StartCoroutine ("MovingAverage");
	}

	IEnumerator SetScale() 
	{
		float scaleOld = transform.localScale.x;
		float scale;

		// Makes values proper scale values for our circle
		smoothRespValue += .5f;
		smoothRespValue *= 12f;

		float t = 0;
		while (t < duration) {
			t += Time.deltaTime;
			float blend = Mathf.Clamp01 (t / duration);

			scale = Mathf.Lerp (scaleOld, smoothRespValue, blend);
			transform.localScale = new Vector3(scale,scale,scale);
			yield return null;
		}
	}

	IEnumerator MovingAverage() {
		List<float> respValues = new List<float>();
		float t = 0;
		while (t < duration) {
			t += Time.deltaTime;
			respValues.Add (1f-calibrationScript.normRespData);
			yield return null;
		}
		respValue = respValues.Average ();
		respValues.Clear();
		if (!initialisedSmoothing) {
			prevSmoothRespValue = respValue;
			initialisedSmoothing = true;
		}
		smoothRespValue = ExponentialSmoothing (respValue, prevSmoothRespValue, 0.2f);		
		prevSmoothRespValue = smoothRespValue;
		StartCoroutine ("SetScale");
		StartCoroutine ("MovingAverage");
	}

	float ExponentialSmoothing(float value, float prevValue, float a) {
		float s = a * value + ((1f - a) * prevValue);						// .3 * 18 + .7 * 10 = 6 + 7 = 13
		return s;
	}
}