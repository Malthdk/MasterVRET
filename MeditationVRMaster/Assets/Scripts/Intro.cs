using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	public GameObject particleSystem, breathingCanvas, fogParticleSystem;
	public Calibration calibrationScript;
	public BluetoothConnection btScript;
	public Text calibrationText, title;
	public Text[] textObjects;
	public Image calibrationPanelImage, calibrationImage, logo, mainBackground, btImageResp, btImageEEG;
	public float delay = 1f;

	[HideInInspector]
	public bool startCalibration, introEnded;

	private bool btConnected, textChanging;
	private int index;

	void Start() {
		// Set starting alpha of all text (except the first) to 0
		Color col = textObjects[0].color;
		col.a = 0f;
		for (int i = 1; i < textObjects.Length; i++) {
			textObjects [i].color = col;
		}
	}

	void Update() {
		// If mouse button is down and we are not calibrating
		if (Input.GetMouseButtonDown (0) && !startCalibration && !textChanging && btScript.tgConnected && btScript.respConnected) {
			textChanging = true;
			index++;
			if (index < textObjects.Length) {
				StartCoroutine ("ChangeText");
				if (index == 1) {
					StartCoroutine ("StartCalibration");
				}
			}
		}

		if (Input.GetMouseButtonDown (0) && index == 2){	
			StartCoroutine ("EndIntro");
		}

		// Used to display bluetooth icon
		DisplayBluetooth ();
	}

	void DisplayBluetooth() {
		if (btScript.respConnected) {
			btImageResp.color = Color.green;
		} else {
			btImageResp.color = Color.red;
		}
		if (btScript.tgConnected) {
			btImageEEG.color = Color.green;
		} else if (btScript.tgConnecting) {
			btImageEEG.color = Color.yellow;
		} else {
			btImageEEG.color = Color.red;
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

	// Courutine that changes text based on index. Uses FadeTo courutine
	IEnumerator ChangeText() {
		StartCoroutine(FadeTo(textObjects [index - 1], 0f, 1f));
		yield return new WaitForSeconds (delay);
		StartCoroutine(FadeTo(textObjects [index], 1f, 1f));
	}

	// This courutine controls the calibration UI in the intro secuence
	IEnumerator StartCalibration() {
		yield return new WaitForSeconds (delay+3.5f);
		textChanging = false;
		startCalibration = true;
		StartCoroutine(FadeTo(calibrationPanelImage, 1f, 1f));
		StartCoroutine(FadeTo(calibrationText, 1f, 1f));
		float t = 0;
		while (t < calibrationScript.calibrationDuration) {
			// Step the fade forward one frame.
			t += Time.deltaTime;
			// Turn the time into an interpolation factor between 0 and 1.
			float timeLeft = t / calibrationScript.calibrationDuration;
			calibrationImage.fillAmount = timeLeft;
			// Wait one frame, and repeat.
			yield return null;
		}
		StartCoroutine(FadeTo(calibrationPanelImage, 0f, 1f));
		StartCoroutine(FadeTo(calibrationText, 0f, 1.5f));
		StartCoroutine(FadeTo(calibrationImage, 0f, 1f));
		StartCoroutine(FadeTo(title, 0f, 1f));
		index++;
		if (index < textObjects.Length) {
			StartCoroutine ("ChangeText");
		}
	}

	// This courutine ends the into secuence by fading out UI elements and activating game elements
	IEnumerator EndIntro() {
		introEnded = true;
		StartCoroutine(FadeTo(logo, 0f, 3f));
		StartCoroutine(FadeTo(textObjects[2], 0f, 1.5f));
		StartCoroutine(FadeTo(mainBackground, 0f, 2f));
		StartCoroutine(FadeTo(btImageEEG, 0f, 1.2f));
		StartCoroutine(FadeTo(btImageResp, 0f, 1.4f));
		yield return new WaitForSeconds (3f);
		particleSystem.SetActive (true);
		fogParticleSystem.SetActive (true);
		//breathingCanvas.SetActive (true);	// Set true for breating exercise
		Destroy(this.gameObject);
	}

}


