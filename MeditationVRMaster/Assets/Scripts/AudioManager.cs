using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[HideInInspector]
	public AudioSource musicAS, speakAS, ambienceAS;
	public AudioClip music, speak, ambience;

	public Intro introScript;
	public bool audioEnded;
	private bool playingMusic, endingMusic;

	void Start () {
		musicAS = transform.GetChild (0).GetComponent<AudioSource> ();
		speakAS = transform.GetChild (1).GetComponent<AudioSource> ();
		ambienceAS = transform.GetChild (2).GetComponent<AudioSource> ();

		StartCoroutine (PlayAudio (ambienceAS, ambience, 0f, 1f, 1f, 0f));
	}

	void Update() {
		if (introScript.introEnded && !playingMusic) {
			playingMusic = true;
			StartCoroutine (PlayAudio (musicAS, music, 0f, .75f, 2f, 0f));
			StartCoroutine (PlayAudio (speakAS, speak, 0f, 1f, 1f, 5f));
		}

		if (playingMusic && speakAS.isPlaying && !endingMusic) {
			endingMusic = true;
			StartCoroutine ("EndAudio");
		}
	}

	// Ending audio after speak length + 5 seconds
	IEnumerator EndAudio () {
		yield return new WaitForSeconds (speakAS.clip.length + 20f);
		audioEnded = true;

		// Fades out the music volume after speak has ended
		float t = 0;
		while (t < 4f) {
			t += Time.deltaTime;
			float blend = Mathf.Clamp01(t / 4f);
			musicAS.volume = Mathf.Lerp(.75f, 0f, blend);
			yield return null;
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
