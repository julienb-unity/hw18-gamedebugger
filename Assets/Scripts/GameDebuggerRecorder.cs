﻿using System.Collections.Generic;
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
	private static Dictionary<Type, string> typeToFieldNameMapping = new Dictionary<Type, string>();

	public static int currentFrame ;
	public static bool isPlaying;
	public static bool isPaused;

	static GameDebuggerRecorder()
	{
		var go = new GameObject("GameDebugger");
		go.hideFlags |= HideFlags.DontSave | HideFlags.HideInHierarchy;
		go.AddComponent<GameDebuggerBehaviour>();

		EditorApplication.update += EditorUpdate; 

		GameDebuggerDatabase.Init();
	}

	private static void EditorUpdate()
	{
		if (isPlaying && !isPaused)
		{
			ReplayFrame(currentFrame);
			currentFrame++;
			if (currentFrame >= GameDebuggerDatabase.NumFrameRecords)
				isPlaying = false;
		}	
	}

	public static void StartReplay()
	{
		if (!isPlaying)
		{
			if (isRecording)
				StopRecording();

			isPaused = false;
			isPlaying = true;
			if (currentFrame >= GameDebuggerDatabase.NumFrameRecords)
				currentFrame = 0;
		}
	}

	public static void PauseReplay()
	{
		if (isPlaying)
		{
			isPaused = true;
		}
	}

	public static void StopReplay()
	{
		isPlaying = false;
		currentFrame = 0;
		isPaused = false;
	}
	
	public static bool ReplayFrame(int frame)
	{
		if (frame >= GameDebuggerDatabase.NumFrameRecords)
		{
			return true;
		}

		EditorApplication.isPaused = true;
		GameDebuggerDatabase.ReplayFrame(frame);

		return false;
	}

	public static void StartRecording()
	{
		if (isPlaying)
			StopReplay();
		
		if (isRecording)
			return;

		EditorApplication.isPaused = false;
		
		GameDebuggerDatabase.Clear();
		currentFrame = 0;


		isRecording = true;
	}
	
	public static void StopRecording()
	{
		if (!isRecording)
			return;

		isRecording = false;
		
		Debug.LogFormat("Recorded {0} frames",GameDebuggerDatabase.NumFrameRecords);
//		GameDebuggerDatabase.LogStats();
	}

	public static void AddPropertyToRecord(Type type, string propName)
	{		
		typeToFieldNameMapping.Add(type, propName);
	}

	public static void Update()
	{
		if (!isRecording)
			return;

		GameDebuggerDatabase.RecordFrame(currentFrame);
		
		currentFrame ++;
		//recorderDataStorage.RecordNewFrame();
	}
}