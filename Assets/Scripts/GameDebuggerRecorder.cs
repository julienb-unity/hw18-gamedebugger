using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEditor;

// Purpose of this class:
//   - Layer between the scene and the editor window
//   - Allows the game dev to define what data to record
//   - Discuss with the database only (never the editor)
[InitializeOnLoad]
public class GameDebuggerRecorder
{

	public static bool isRecording;
	public static Dictionary<Type, string> typeToFieldNameMapping = new Dictionary<Type, string>();

	private static GameDebuggerDatabase recorderDataStorage;
	private static int m_currentFrame ;
	
	static GameDebuggerRecorder()
	{
		recorderDataStorage = Resources.Load<GameDebuggerDatabase>("GameDebuggerRecording");

		var go = new GameObject("GameDebugger");
		go.hideFlags |= HideFlags.DontSave | HideFlags.HideInHierarchy;
		go.AddComponent<GameDebuggerBehaviour>();

		GameDebuggerDatabase.Init();
	}

	public static void StartRecording()
	{
		if (isRecording)
			return;

		GameDebuggerDatabase.Clear();
		m_currentFrame = 0;


		isRecording = true;
	}
	
	public static void StopRecording()
	{
		if (!isRecording)
			return;

		isRecording = false;
	}

	public static void AddPropertyToRecord(Type type, string propName)
	{		
		typeToFieldNameMapping.Add(type, propName);
	}

	public static void Update()
	{
		if (!isRecording)
			return;

		GameDebuggerDatabase.RecordFrame(m_currentFrame);
		
		m_currentFrame ++;
		//recorderDataStorage.RecordNewFrame();
	}
}