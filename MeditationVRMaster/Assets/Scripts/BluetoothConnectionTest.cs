using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TechTweaking.Bluetooth;

public class BluetoothConnectionTest : MonoBehaviour {

    private BluetoothDevice device;

	public TextMesh connectionStatus;

	void Awake () {
        BluetoothAdapter.enableBluetooth();

        device = new BluetoothDevice();
        device.MacAddress = "E4-A4-71-A7-FD-0F";
    }

    void Start()
    {
        Connect();
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
