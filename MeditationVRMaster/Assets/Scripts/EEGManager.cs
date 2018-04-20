using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEGManager : MonoBehaviour {

	public Calibration calibrationScript;
	public Material treeMat;
	public Water waterScript;

	private float startSpeed, startDisplacement;

	void Start () {
		startSpeed = 0.8f;
		startDisplacement = 0.5f;
	}
	
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			treeMat.SetFloat ("_tree_sway_speed", 10f);
		}
	}

}
