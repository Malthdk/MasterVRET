using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class AntiAliasing : MonoBehaviour {

	void Start () {
		QualitySettings.antiAliasing = 2;
    }

}
