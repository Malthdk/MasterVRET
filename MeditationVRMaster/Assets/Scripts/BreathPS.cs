using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BreathPS : MonoBehaviour {

	private ParticleSystem ps;
	private ParticleSystem.Particle[] particles;
	private Vector3[] startingVel;
	private bool gottenStartingVel, activated;
	private float respValue;

	public Calibration calibrationScript;
	public Text debugTxt;


    void Awake () {
		ps = this.transform.GetComponent<ParticleSystem> ();
		particles = new ParticleSystem.Particle[1000];
		StartCoroutine ("SmoothenValues");
    }

    void LateUpdate () {

		if (!gottenStartingVel) {
			GetStartingVelocity ();
			gottenStartingVel = true;
		}

		if (calibrationScript.finishedCalibrating) {
			int numParticlesAlive = ps.GetParticles(particles);

			for (int i = 0; i < numParticlesAlive; i++)
			{
				particles[i].velocity = startingVel[i] * calibrationScript.normRespData;
			}

			ps.SetParticles(particles, numParticlesAlive);
		}
	}

	IEnumerator SmoothenValues() {
		List<float> respValues = new List<float>();
		float t = 0;
		while (t < 1f) {
			t += Time.deltaTime;
			respValues.Add (calibrationScript.normRespData);
			yield return null;
		}
		respValue = respValues.Average ();
		respValues.Clear();
		debugTxt.text = "Smooth: " + respValue.ToString ();
		StartCoroutine ("SmoothenValues");
	}

	void GetStartingVelocity () { // THIS FUNCTION NEEDS TO BE CALLED SOMEWHERE AFTER START()
		int numParticles = ps.GetParticles(particles);

		startingVel = new Vector3[numParticles];

		for (int i = 0; i < numParticles; i++)
		{
			startingVel[i] = particles[i].velocity;
		}
	}
}