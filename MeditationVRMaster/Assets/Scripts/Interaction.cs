using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour {

	public bool startCalibrating;
	public Image btImageEEG;
	public BluetoothConnection btScript;

	void Update() {
		DisplayBluetooth ();
	}

	public void StartControlCondition() {
		if (btScript.tgConnected && !startCalibrating) {
			startCalibrating = true;
		}
	}

	public void Connect() {
		btScript.Connect ();
	}

	void DisplayBluetooth() {
		if (btScript.tgConnected) {
			btImageEEG.color = Color.green;
		} else if (btScript.tgConnecting) {
			btImageEEG.color = Color.yellow;
		} else {
			btImageEEG.color = Color.red;
		}
	}
}
