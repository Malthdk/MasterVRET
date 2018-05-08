using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public AudioManager audioScript;
	public Image panel;
	public Text txt, title;

	private bool gameover;

	void Update () {
		if (audioScript.audioEnded && !gameover) {
			gameover = true;
			StartCoroutine(FadeTo(panel, 1f, 1f));
			StartCoroutine(FadeTo(txt, 1f, 2f));
			StartCoroutine(FadeTo(title, 1f, 2.2f));
		}
	}

	// Courutine that makes us able to fade in and out graphical objects (by changing their alpha value)
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
}
