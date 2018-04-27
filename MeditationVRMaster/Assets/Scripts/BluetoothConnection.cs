using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using TechTweaking.Bluetooth;

public class BluetoothConnection : MonoBehaviour
{
	private  BluetoothDevice device;
	public EEGListener eegListener;
	public Text statusText;
	public float respValue;

	public bool respConnected, tgConnected;

	void Awake ()
	{
		BluetoothAdapter.enableBluetooth();
		device = new BluetoothDevice ();
		//device.MacAddress = "98:D3:32:31:30:0C";
		device.Name = "HC-05";
	}


	void Start ()
	{
		BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;//This would mean a failure in connection! the reason might be that your remote device is OFF
		Connect ();
	}

	void Update() {
		if (device.IsConnected) {
			ManageConnection (device);
		}

		if (Input.GetMouseButtonDown (0) && !respConnected) {
			Connect ();
		}

		if (Input.GetMouseButtonDown (0) && !tgConnected) {
			UnityThinkGear.Init (true);
			UnityThinkGear.StartStream ();
		}
		if (eegListener.PoorSignal < 50) {
			tgConnected = true;
		} else {
			tgConnected = false;
		}
	}

	private void Connect ()
	{

		if (statusText != null) {
			statusText.text = "Trying To Connect. Name is: " + device.MacAddress;
		}

		/* The Property device.MacAdress doesn't require pairing. 
		 * Also Mac Adress in this library is Case sensitive,  all chars must be capital letters
		 */

		//device.Name = "HC-05";
		/* 
		* Trying to identefy a device by its name using the Property device.Name require the remote device to be paired
		* but you can try to alter the parameter 'allowDiscovery' of the Connect(int attempts, int time, bool allowDiscovery) method.
		* allowDiscovery will try to locate the unpaired device, but this is a heavy and undesirable feature, and connection will take a longer time
		*/

		/*
		* The ManageConnection Coroutine will start when the device is ready for reading.
			*/
		//device.ReadingCoroutine = ManageConnection;

		device.connect ();
	}

	//This would mean a failure in connection! the reason might be that your remote device is OFF
	void HandleOnDeviceOff (BluetoothDevice dev)
	{
		if (!string.IsNullOrEmpty (dev.Name)) {
			if (statusText != null) {
				statusText.text = "Status : can't connect to '" + dev.Name + "', device is OFF ";
				respConnected = false;
			}
		} else if (!string.IsNullOrEmpty (dev.MacAddress)) {
			if (statusText != null) {
				statusText.text = "Status : can't connect to '" + dev.MacAddress + "', device is OFF ";
				respConnected = false;
			}
		}
	}

	//############### Reading Data  #####################
	void  ManageConnection (BluetoothDevice device)
	{
		if (device.IsReading) {
			respConnected = true;
			byte [] msg = device.read ();
			if (msg != null) {
				string content = System.Text.ASCIIEncoding.ASCII.GetString (msg);
				if (statusText != null) {
					statusText.text = content;
				}
				respValue = float.Parse (content);
			}
		}
	}
		
	//############### Deregister Events  #####################
	void OnDestroy ()
	{
		BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;

	}

}
