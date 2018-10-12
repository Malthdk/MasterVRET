using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	public GameObject particleSystem, fogParticleSystem;
	public Text calibrationText, title;
	public Text[] textObjects;
	public Image logo, mainBackground;
	public Material fadeOverlay;
	public float delay = 3f;

	[HideInInspector]
	public bool introEnded;

	private bool textChanging;
	private int index;

	void Start() {
		// Set starting alpha of all text (except the first) to 0
		Color col = fadeOverlay.color;
		col.a = 1f;
		fadeOverlay.color = col;

		StartCoroutine ("StartIntro");
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

	// Courutine that makes us able to fade in and out graphical objects (by changing their alpha value)
	IEnumerator FadeToMat(Material gfx, float targetOpacity, float duration)
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

	// Courutine that changes text based on index. Uses FadeTo courutine
	IEnumerator StartIntro() {
		StartCoroutine(FadeTo(logo, 1f, 2f));
		yield return new WaitForSeconds (delay);
		StartCoroutine(FadeTo(logo, 0f, 2f));
		yield return new WaitForSeconds (delay);
		StartCoroutine(FadeTo(title, 1f, 2f));
		yield return new WaitForSeconds (delay+3f);
		StartCoroutine(FadeTo(title, 0f, 2f));
		yield return new WaitForSeconds (2f);
		StartCoroutine ("EndIntro");
	}

	// This courutine ends the into secuence by fading out UI elements and activating game elements
	IEnumerator EndIntro() {
		introEnded = true;
		StartCoroutine(FadeToMat(fadeOverlay, 0f, 4f));
		yield return new WaitForSeconds (4f);
		fogParticleSystem.SetActive (true);
	}

}


