using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathingTech : MonoBehaviour {

	public Image timerImage;
	public Text instructions, countdown;

	void Start () {
		StartCoroutine ("Pranayama");
	}

	IEnumerator Pranayama() {
		instructions.text = "Breath in";
		StartCoroutine (Counter(4));
		yield return new WaitForSeconds (4f);
		instructions.text = "Hold your breath";
		StartCoroutine (Counter(4));
		yield return new WaitForSeconds (4f);
		instructions.text = "Breath out";
		StartCoroutine (Counter(6));
		yield return new WaitForSeconds (6f);
		instructions.text = "Hold";
		StartCoroutine (Counter(2));
		yield return new WaitForSeconds (2f);
		StartCoroutine ("Pranayama");
	}

	IEnumerator FadeTo(Graphic gfx, float targetOpacity, float duration)
	{
		Color color;

		// Cache the current color of the material, and its initial opacity.
		color = gfx.color;
		float startOpacity = color.a;

		// Track how many seconds we've been fading.
		float t = 0;

		while (t < duration) {
			// Step the fade forward one frame.
			t += Time.deltaTime;
			// Turn the time into an interpolation factor between 0 and 1.
			float blend = Mathf.Clamp01(t / duration);

			// Blend to the corresponding opacity between start & target.
			color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

			// Apply the resulting color to the material.
			gfx.color = color;

			// Wait one frame, and repeat.
			yield return null;
		}
	}

	IEnumerator Counter(int seconds) {
		for (int i = 1; i <= seconds; i++) {
			countdown.text = i.ToString ();
			yield return new WaitForSeconds (1f);
		}
	}
}
