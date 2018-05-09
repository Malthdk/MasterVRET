using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class EEGListener : MonoBehaviour {
	
	public Texture2D[] signalIcons;
	public Text raw, signal, medi, alpha, algoAlpha;
	
    ThinkGearController controller;
	
	public int Raw = 0;
	public int PoorSignal = 200;
	public int Attention = 0;
	public int Meditation = 0;
	public int Blink = 0;
	public float Delta = 0.0f;
	public float Theta = 0.0f;
	public float LowAlpha = 0.0f;
	public float HighAlpha = 0.0f;
	public float LowBeta = 0.0f;
	public float HighBeta = 0.0f;
	public float LowGamma = 0.0f;
	public float HighGamma = 0.0f;

	public int Algo_Attention = 0;
	public int Algo_Meditation = 0;
	public float Algo_Delta = 0.0f;
	public float Algo_Theta = 0.0f;
	public float Algo_Alpha = 0.0f;
	public float Algo_Beta = 0.0f;
	public float Algo_Gamma = 0.0f;
	public float Zone;

	public string ConnectState;


    //Tommy add 20161020
    private bool showListViewFlag = false;
	private ArrayList deviceList;
	private ArrayList displayedStrArr;

	private List<float> alphaData;
	public float averageAlpha;

	void Start () {
		controller = GameObject.Find("ThinkGear").GetComponent<ThinkGearController>();

		controller.UpdateConnectStateEvent += OnUpdateConnectState;
		controller.UpdateRawdataEvent += OnUpdateRaw;
		controller.UpdatePoorSignalEvent += OnUpdatePoorSignal;
		controller.UpdateAttentionEvent += OnUpdateAttention;
		controller.UpdateMeditationEvent += OnUpdateMeditation;
		
		controller.UpdateDeltaEvent += OnUpdateDelta;
		controller.UpdateThetaEvent += OnUpdateTheta;

		controller.UpdateHighAlphaEvent += OnUpdateHighAlpha;
		controller.UpdateHighBetaEvent += OnUpdateHighBeta;
		controller.UpdateHighGammaEvent += OnUpdateHighGamma;

		controller.UpdateLowAlphaEvent += OnUpdateLowAlpha;
		controller.UpdateLowBetaEvent += OnUpdateLowBeta;
		controller.UpdateLowGammaEvent += OnUpdateLowGamma;

		controller.UpdateBlinkEvent += OnUpdateBlink;

		controller.UpdateDeviceInfoEvent += OnUpdateDeviceInfo;
        controller.Algo_UpdateAttentionEvent += OnAlgo_UpdateAttentionEvent;
        controller.Algo_UpdateMeditationEvent += OnAlgo_UpdateMeditationEvent;
        controller.Algo_UpdateDeltaEvent += OnAlgo_UpdateDeltaEvent;
        controller.Algo_UpdateThetaEvent += OnAlgo_UpdateThetaEvent;
        controller.Algo_UpdateAlphaEvent += OnAlgo_UpdateAlphaEvent;
        controller.Algo_UpdateBetaEvent += OnAlgo_UpdateBetaEvent;
        controller.Algo_UpdateGammaEvent += OnAlgo_UpdateGammaEvent;

		deviceList = new ArrayList();
		displayedStrArr = new ArrayList();
		alphaData = new List<float>();
	}

	void CalculateZone(){
		Zone = (Attention * 0.45f) + (Meditation * 0.55f);
	}

	void OnUpdateConnectState(string value)
	{
		ConnectState = value;
	}
    void OnAlgo_UpdateAttentionEvent(int value)
    {
        Algo_Attention = value;
    }
    void OnAlgo_UpdateMeditationEvent(int value)
    {
        Algo_Meditation = value;

    }

    void OnAlgo_UpdateDeltaEvent(float value)
    {
        Algo_Delta = value;
    }
    void OnAlgo_UpdateThetaEvent(float value)
    {
        Algo_Theta = value;
    }
    void OnAlgo_UpdateAlphaEvent(float value)
    {
        Algo_Alpha = value;
    }
    void OnAlgo_UpdateBetaEvent(float value)
    {
        Algo_Beta = value;
    }
    void OnAlgo_UpdateGammaEvent(float value)
    {
        Algo_Gamma = value;
    }


    void OnUpdatePoorSignal(int value){
		PoorSignal = value;
	}
	void OnUpdateRaw(int value){
		Raw = value;
	}
	void OnUpdateAttention(int value){
		Attention = value;
	}
	void OnUpdateMeditation(int value){
		Meditation = value;

	}
	void OnUpdateDelta(float value){
		Delta = value;
	}
	void OnUpdateTheta(float value){
		Theta = value;
	}
	void OnUpdateHighAlpha(float value){
		HighAlpha = value;
	}
	void OnUpdateHighBeta(float value){
		HighBeta = value;
	}
	void OnUpdateHighGamma(float value){
		HighGamma = value;
	}
	void OnUpdateLowAlpha(float value){
		LowAlpha = value;
	}
	void OnUpdateLowBeta(float value){
		LowBeta = value;
	}
	void OnUpdateLowGamma(float value){
		LowGamma = value;
	}

	void OnUpdateBlink(int value){
		Blink = value;
	}


	void OnUpdateDeviceInfo(string deviceInfo){
		//deviceFound deviceInfo = NSF4F1BF;MindWave Mobile;BAFCEB11-2DB6-70B3-B038-B4AD2EFC6309
		// FMGID ; name ; ConnectId
		Add2DeviceListArray(deviceInfo);
	}

	void Update(){
		if (medi != null)
			raw.text = "Medi: " + Meditation.ToString();
		if (signal != null)
			signal.text = "Signal: " + PoorSignal.ToString();
		CalculateZone ();
	}

	void Add2DeviceListArray(string element){
		string mfgid = "";
		string name = "";
		string deviceId = "";

		mfgid = element.Split(";"[0])[0];
		name = element.Split(";"[0])[1];
		deviceId = element.Split(";"[0])[2];
		print("Add2DeviceListArray  mfgid : "+mfgid + " name: "+name+" deviceId: "+deviceId);

		int  deviceCount = 0;
		deviceCount = deviceList.Count;
		print("deviceCount : "+deviceCount);
		if(deviceCount > 0){
            for(int i = 0; i < deviceList.Count; i++) {
				if(deviceList[i] == deviceId){
					break;
				}
				else{
					displayedStrArr.Add(mfgid+" "+name);
					deviceList.Add(deviceId);
					break;
				}

			}
		}
		else{
			displayedStrArr.Add(mfgid+" "+name);
			deviceList.Add(deviceId);
		}
			
		print("deviceList : "+deviceList);
		print("displayedStrArr : "+displayedStrArr);
	}


}
