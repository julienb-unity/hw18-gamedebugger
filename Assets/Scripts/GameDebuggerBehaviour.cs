using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDebuggerBehaviour : MonoBehaviour
{
	public void LateUpdate()
	{
		GameDebuggerRecorder.Instance.Update();
	}
}
