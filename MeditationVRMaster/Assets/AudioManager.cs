using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[HideInInspector]
	public AudioSource musicAS, speakAS, ambienceAS;
	public AudioClip music, speak, ambience;

	public Intro introScript;

	void Start () {
		musicAS = transform.GetChild (0).GetComponent<AudioSource> ();
		speakAS = transform.GetChild (1).GetComponent<AudioSource> ();
		ambienceAS = transform.GetChild (2).GetComponent<AudioSource> ();

		StartCoroutine (PlayAudio (ambienceAS, ambience, 0f, 1f, 1f, 0f));
	}

	void Update() {
		if (introScript.introEnded) {
			StartCoroutine (PlayAudio (musicAS, music, 0f, .8f, 2f, 0f));
			StartCoroutine (PlayAudio (speakAS, speak, 0f, 1.1f, 1f, 3f));
			StartCoroutine (PlayAudio (ambienceAS, ambience, 1f, 0f, 3f, .5f));
		}
	}

	public IEnumerator PlayAudio (AudioSource aS, AudioClip clip, float fadeFrom, float fadeTo, float fadeDuration, float delay) {
		yield return new WaitForSeconds (delay);
		aS.clip = clip;
		aS.volume = fadeFrom;
		aS.Play ();
		float t = 0;
		while (t < fadeDuration) {
			t += Time.deltaTime;
			float blend = Mathf.Clamp01(t / fadeDuration);
			aS.volume = Mathf.Lerp(fadeFrom, fadeTo, blend);
			yield return null;
		}
	}
}
