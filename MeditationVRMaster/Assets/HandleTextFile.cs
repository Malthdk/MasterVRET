using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class HandleTextFile : MonoBehaviour
{
	private string startTime;

	void Start() {
		startTime = DateTime.UtcNow.ToString ("HH:mm_dd_MMMM_yyyy");
		WriteString ();
	}

	void WriteString()
	{
		string path = "Assets/Resources/test.txt";

		//Write some text to the test.txt file
		TextWriter writer = new StreamWriter(Application.persistentDataPath + "/dataLog_" + startTime + ".txt");
		writer.WriteLine("Test");
		writer.Close();

	}

}