using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using Object = System.Object;

[InitializeOnLoad]
public static class GameDebuggerPlayer
{
	public static bool isPlaying;

	static GameDebuggerPlayer()
	{
		EditorApplication.update += Update; 
	}

	private static int frame = 0;
	public static void StartReplay()
	{
		if (isPlaying)
			return;

		frame = 0;
		EditorApplication.isPaused = true;
		

		isPlaying = true;

	}
	
	public static void StopReplay()
	{
		if (!isPlaying)
			return;

		isPlaying = false;
	}

	public static void Update()
	{
		if (!isPlaying)
			return;

		if (frame >= GameDebuggerDatabase.NumFrameRecords)
		{
			StopReplay();
			return;
		}
		foreach (var recordable in GameDebuggerDatabase.GetRecords(frame))
		{
			var o = EditorUtility.InstanceIDToObject(recordable.Key);
			if (o != null)
			{
				recordable.Value.OnReplay(o);
			}
		}

		frame++;
	}

}