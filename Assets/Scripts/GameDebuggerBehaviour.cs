using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose of this class:
//   - Component to add to a temporary game object that gives
//     access to LateUpdate().
[ExecuteInEditMode]
public class GameDebuggerBehaviour : MonoBehaviour
{
	public void LateUpdate()
	{
		GameDebuggerRecorder.Instance.Update();
	}
}
