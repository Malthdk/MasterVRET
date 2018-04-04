using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathPS : MonoBehaviour {

	private ParticleSystem ps;
	private ParticleSystem.Particle[] particles;
	private Vector3[] startingVel;
	private bool gottenStartingVel = false;

	public Calibration calibrationScript;

	public float testingParam;

    void Awake () {
		ps = this.transform.GetComponent<ParticleSystem> ();
		particles = new ParticleSystem.Particle[1000];
    }

    void LateUpdate () {

		if (!gottenStartingVel) {
			GetStartingVelocity ();
			gottenStartingVel = true;
		}

		if (!calibrationScript.calibrating) {
			int numParticlesAlive = ps.GetParticles(particles);

			for (int i = 0; i < numParticlesAlive; i++)
			{
				particles[i].velocity = startingVel[i] * calibrationScript.normRespData;
			}

			ps.SetParticles(particles, numParticlesAlive);
		}

		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			int numParticlesAlive = ps.GetParticles(particles);

			for (int i = 0; i < numParticlesAlive; i++)
			{
				particles[i].velocity = startingVel[i] * -1f;
			}

			ps.SetParticles(particles, numParticlesAlive);
		}


		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			int numParticlesAlive = ps.GetParticles(particles);

			for (int i = 0; i < numParticlesAlive; i++)
			{
				particles[i].velocity = startingVel[i] * 1f;
			}

			ps.SetParticles(particles, numParticlesAlive);
		}
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