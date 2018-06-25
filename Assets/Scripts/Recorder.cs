using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
	public void Start()
	{
		GameDebuggerRecorder.AddPropertyToRecord(typeof(Transform), "position");
	}
}
