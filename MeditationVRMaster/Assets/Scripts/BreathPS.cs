using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathPS : MonoBehaviour {

	private ParticleSystem ps;
	private ParticleSystem.Particle[] particles;

	void Awake () {
		ps = this.transform.GetComponent<ParticleSystem> ();
		particles = new ParticleSystem.Particle[ps.main.maxParticles];
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			int numParticlesAlive = ps.GetParticles(particles);

			for (int i = 0; i < numParticlesAlive; i++)
			{
				Debug.Log (i);
				particles[i].velocity += Vector3.up * 20f;
			}

			ps.SetParticles(particles, numParticlesAlive);
		}


		if (Input.GetKeyDown (KeyCode.Mouse1)) {
			ps.Emit (20);
		}
	}
}
