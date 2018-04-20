﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	public GameObject particleSystem, breathingCanvas;
	public Calibration calibrationScript;
	public BluetoothConnection btScript;
	public Text calibrationText, title;
	public Text[] textObjects;
	public Image calibrationPanelImage, calibrationImage, logo, mainBackground, btImage;
	public float delay = 1f;

	[HideInInspector]
	public bool startCalibration;

	private bool btConnected;
	private int index;

	void Start() {
		Color col = textObjects[0].color;
		col.a = 0f;
		for (int i = 1; i < textObjects.Length; i++) {
			textObjects [i].color = col;
		}
	}

	void Update() {
		if (Input.GetMouseButtonDown (0) && !startCalibration) {
			index++;
			if (index < textObjects.Length) {
				StartCoroutine ("ChangeText");
			}
		}

		if (index == 1 && !startCalibration) {
			StartCoroutine ("StartCalibration");
		}

		if (Input.GetMouseButtonDown (0) && index == 2){	
			StartCoroutine ("EndIntro");
		}

		if (btScript.connected && !btConnected) {
			btImage.color = Color.green;
			StartCoroutine(FadeTo(btImage, 0f, 5f));
			btConnected = true;
		} else if (!btScript.connected && !btConnected) {
			btImage.color = Color.red;
		}
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

	IEnumerator ChangeText() {
		StartCoroutine(FadeTo(textObjects [index - 1], 0f, 1f));
		yield return new WaitForSeconds (delay);
		StartCoroutine(FadeTo(textObjects [index], 1f, 1f));
	}

	IEnumerator StartCalibration() {
		startCalibration = true;
		yield return new WaitForSeconds (delay+5f);
		StartCoroutine(FadeTo(calibrationPanelImage, 1f, 1f));
		StartCoroutine(FadeTo(calibrationText, 1f, 1f));
		float t = 0;
		while (t < calibrationScript.calibrationDuration) {
			// Step the fade forward one frame.
			t += Time.deltaTime;
			// Turn the time into an interpolation factor between 0 and 1.
			float timeLeft = t / calibrationScript.calibrationDuration;
			// Blend to the corresponding opacity between start & target.
			calibrationImage.fillAmount = timeLeft;
			// Wait one frame, and repeat.
			yield return null;
		}
		StartCoroutine(FadeTo(calibrationPanelImage, 0f, 1f));
		StartCoroutine(FadeTo(calibrationText, 0f, 1.5f));
		StartCoroutine(FadeTo(calibrationImage, 0f, 1f));
		StartCoroutine(FadeTo(title, 0f, 1.2f));
		index++;
		if (index < textObjects.Length) {
			StartCoroutine ("ChangeText");
		}
	}

	IEnumerator EndIntro() {
		StartCoroutine(FadeTo(logo, 0f, 3f));
		StartCoroutine(FadeTo(textObjects[2], 0f, 1.5f));
		StartCoroutine(FadeTo(mainBackground, 0f, 2f));
		yield return new WaitForSeconds (3f);
		particleSystem.SetActive (true);
		breathingCanvas.SetActive (true);
		gameObject.SetActive (false);
	}

}


