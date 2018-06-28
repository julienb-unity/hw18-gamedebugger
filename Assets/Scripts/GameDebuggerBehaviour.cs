using UnityEngine;

// Purpose of this class:
//   - Component to add to a temporary game object that gives
//     access to LateUpdate().
public class GameDebuggerBehaviour : MonoBehaviour
{
	public void LateUpdate()
	{
		GameDebuggerRecorder.Update();
	}
}
