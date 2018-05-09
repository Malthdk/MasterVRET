﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using TechTweaking.Bluetooth;

public class BluetoothConnection : MonoBehaviour
{
	public EEGListener eegListener;

	public bool tgConnected, tgConnecting;

	void Update() {
		if (eegListener.Raw != 0 && eegListener.PoorSignal != 0) {
			tgConnected = false;
			tgConnecting = true;
		} else if (eegListener.PoorSignal == 0) {
			tgConnecting = false;
			tgConnected = true;
		} else {
			tgConnecting = false;
			tgConnected = false;
		}
	}

	public void Connect() {
		if (!tgConnected) {
			UnityThinkGear.Init (true);
			UnityThinkGear.StartStream ();
		}
	}
}
