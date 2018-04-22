using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EEGManager : MonoBehaviour {

	public Calibration calibrationScript;
	public EEGListener eegScript;
	public Material treeMat;
	public Water waterScript;
	public Text debugText, debugText2;

	public float calculationDuration = 5f;

	private List<float> meditationDataList;

	private float startSpeed, startDisplacement, startWaterFequency, windDisplacement, windSpeed, waterFrequency, meditationAvg;

	void Start () {
		// Initialises list for alpha data and the starting values for sway and speed
		startSpeed = 0.8f;
		startDisplacement = 0.5f;
		startWaterFequency = 0.5f;
		windDisplacement = startDisplacement;

		meditationDataList = new List<float>();

		// Sets the starting values for sway and speed
		treeMat.SetFloat ("_tree_sway_speed", startSpeed);
		treeMat.SetFloat ("_tree_sway_disp", startDisplacement);
		waterScript.waveFrequency = startWaterFequency;

		// Starts calculating the average of the mediation value within 5s and map the values
		StartCoroutine ("CalculateAverage");
	}

	void FixedUpdate () {
		// Set the wind variable to the material
		treeMat.SetFloat ("_tree_sway_disp", windDisplacement);
		treeMat.SetFloat ("_tree_sway_speed", windSpeed);
		waterScript.waveFrequency = waterFrequency;
	}

	void SetValuesToShader() {
		treeMat.SetFloat ("_tree_sway_speed", 2f - (meditationAvg/50));
		treeMat.SetFloat ("_tree_sway_disp", 1f - (meditationAvg/100));
		waterScript.waveFrequency = 1f - (meditationAvg/100);
	}

	IEnumerator MeditationMapping() 
	{
		// Sets the current wind speed value to a temp value and calculates the new
		float speedOld = windSpeed;
		float speedNew = 2f - (meditationAvg/50);

		// Sets the current wind displacement value to a temp value and calculates the new
		float windOld = windDisplacement;
		float windNew = 1f - (meditationAvg/100);

		// Sets the current water wave freq value to a temp value and calculates the new
		float waterOld = waterFrequency;
		float waterNew = 1f - (meditationAvg/100);

		float t = 0;
		while (t < 1f) {
			t += Time.deltaTime;
			// Turn the time into an interpolation factor between 0 and 1.
			float blend = Mathf.Clamp01 (t / 1f);
			windSpeed = Mathf.Clamp (Mathf.Lerp (speedOld, speedNew, blend), .2f, 1.8f);
			windDisplacement = Mathf.Lerp (windOld, windNew, blend);
			waterFrequency = Mathf.Lerp (waterOld, waterNew, blend);

			debugText2.text = "WaterFreq: " + waterFrequency.ToString ();

			yield return null;
		}
	}

	IEnumerator CalculateAverage() 
	{
		float t = 0;
		while (t < calculationDuration) {
			t += Time.deltaTime;
			meditationDataList.Add (eegScript.Meditation);
			yield return null;
		}
		meditationAvg = meditationDataList.Average ();
		debugText.text = "Medi avg: " + meditationAvg.ToString ();
		meditationDataList.Clear ();
		StartCoroutine ("MeditationMapping");
		StartCoroutine ("CalculateAverage");
	}

	/*
	IEnumerator CalcWindVar() 
	{
		float windOld = windDisplacement;
		float windTemp = windDisplacement * alphaVariance; // 0.9
		float t = 0;
		while (t < calculationDuration) {
			t += Time.deltaTime;
			// Turn the time into an interpolation factor between 0 and 1.
			float blend = Mathf.Clamp01 (t / calculationDuration);
			// Maybe we dont need clamp here (as value never gets over 1 or under 0 but keeping it for now
			windDisplacement = Mathf.Clamp (Mathf.Lerp (windOld, windTemp, blend), 0f, 1f);
			yield return null;
		}
	}

	/*
	public IEnumerator AlphaVariance()
	{
		float oldAvgAlpha = avgAlpha;
		float t = 0;
		while (t < calculationDuration) {
			t += Time.deltaTime;
			alphaDataList.Add (eegScript.LowAlpha);
			yield return null;
		}
		avgAlpha = alphaDataList.Average ();
		alphaDataList.Clear ();
		alphaVariance = avgAlpha / oldAvgAlpha;							// 40.000 / 35.000 = 0.9;
		debugText.text = "Alpha variance: " + alphaVariance.ToString();
		StartCoroutine ("AlphaVariance");
		StartCoroutine ("CalcWindVar");
	}*/
}
