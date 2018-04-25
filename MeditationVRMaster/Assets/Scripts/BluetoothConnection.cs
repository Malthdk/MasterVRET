using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using TechTweaking.Bluetooth;

public class BluetoothConnection : MonoBehaviour
{

	private  BluetoothDevice device;
	public EEGListener eegListener;
	public TextMesh statusText;
	public float respValue;

	public bool respConnected, tgConnected;

	void Awake ()
	{
		device = new BluetoothDevice ();

		if (BluetoothAdapter.isBluetoothEnabled ()) {
			connect ();
		} else {

			//BluetoothAdapter.enableBluetooth(); //you can by this force enabling Bluetooth without asking the user
			if (statusText != null)
				statusText.text = "Status : Please enable your Bluetooth";

			BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
			BluetoothAdapter.listenToBluetoothState (); // if you want to listen to the following two events  OnBluetoothOFF or OnBluetoothON

			BluetoothAdapter.askEnableBluetooth ();//Ask user to enable Bluetooth

		}
	}

	void Start ()
	{
		BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;//This would mean a failure in connection! the reason might be that your remote device is OFF

		BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound; //Because connecting using the 'Name' property is just searching, the Plugin might not find it!(only for 'Name').
		statusText.gameObject.SetActive (false);
	}

	void Update() {
		if (device.IsConnected) {
			ManageConnection (device);
		}

		if (Input.GetMouseButtonDown (0) && !tgConnected) {
			UnityThinkGear.Init (true);
			UnityThinkGear.StartStream ();
		}

		if (Input.GetMouseButtonDown (0) && tgConnected) {
			if (BluetoothAdapter.isBluetoothEnabled ()) {
				connect ();
			} else {
				BluetoothAdapter.enableBluetooth(); //you can by this force enabling Bluetooth without asking the user
				if (statusText != null)
					statusText.text = "Status : Please enable your Bluetooth";

				BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
				BluetoothAdapter.listenToBluetoothState (); // if you want to listen to the following two events  OnBluetoothOFF or OnBluetoothON
			}
		}

		if (eegListener.PoorSignal < 80) {
			tgConnected = true;
		}
	}

	private void connect ()
	{

		if (statusText != null) {
			statusText.text = "Status : Trying To Connect";
		}

		respConnected = false;

		/* The Property device.MacAdress doesn't require pairing. 
		 * Also Mac Adress in this library is Case sensitive,  all chars must be capital letters
		 */
		//device.MacAddress = "XX:XX:XX:XX:XX:XX";

		device.Name = "HC-05";
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


	//############### Handlers/Recievers #####################
	void HandleOnBluetoothStateChanged (bool isBtEnabled)
	{
		if (isBtEnabled) {
			connect ();
			//We now don't need our recievers
			BluetoothAdapter.OnBluetoothStateChanged -= HandleOnBluetoothStateChanged;
			BluetoothAdapter.stopListenToBluetoothState ();
		}
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

	//Because connecting using the 'Name' property is just searching, the Plugin might not find it!.
	void HandleOnDeviceNotFound (BluetoothDevice dev)
	{
		if (!string.IsNullOrEmpty (dev.Name)) {
			statusText.text = "Status : Can't find a device with the name '" + dev.Name + "', device might be OFF or not paird yet ";

		} 
	}

	public void disconnect ()
	{
		if (device != null)
			device.close ();
	}

	//############### Reading Data  #####################
	//Please note that you don't have to use this Couroutienes/IEnumerator, you can just put your code in the Update() method.
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
		BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;

	}

}
