using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechTweaking.Bluetooth;

public class BluetoothConnectionTest : MonoBehaviour {

    private BluetoothDevice device;

	public TextMesh connectionStatus;

	void Awake () {
        BluetoothAdapter.askEnableBluetooth();

        device = new BluetoothDevice();
        device.Name = "HC-05";
		device.connect();
    }

    public void Connect() {
        device.connect();
    }

    void Update () {
		if (device.IsConnected) {
			connectionStatus.text = "Bluetooth connected! :)";
		} else {
			connectionStatus.text = "Bluetooth not connected! :(";
		}
    }
}
