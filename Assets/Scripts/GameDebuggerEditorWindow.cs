using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameDebuggerEditorWindow : EditorWindow 
{
	[MenuItem("GameDebugger/OpenEditor")]
	public static void Open()
	{
		GetWindow<GameDebuggerEditorWindow>("GameDebuggerEditorWindow");
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Start"))
		{
			GameDebuggerRecorder.Instance.StartRecording();
		}
		
		if (GUILayout.Button("Stop"))
		{
			GameDebuggerRecorder.Instance.StopRecording();
		}
	}
}
