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

public class GameDebuggerPlayer
{
	private static GameDebuggerPlayer m_Instance;
	public static GameDebuggerPlayer Instance
	{
		get
		{
			if (m_Instance == null)
				m_Instance = new GameDebuggerPlayer();
			return m_Instance;
		}
	}

	public bool isPlaying;

	private GameDebuggerPlayer()
	{
		EditorApplication.update += Update; 
	}

	private int frame = 0;
	public void StartReplay()
	{
		if (isPlaying)
			return;

		frame = 0;
		EditorApplication.isPaused = true;
		

		isPlaying = true;

	}
	
	public void StopReplay()
	{
		if (!isPlaying)
			return;

		isPlaying = false;
	}

	public void Update()
	{
		if (!isPlaying)
			return;

		if (frame >= GameDebuggerRecorder.Instance.s_frameRecords.Count)
		{
			StopReplay();
			return;
		}
		foreach (var recordable in GameDebuggerRecorder.Instance.s_frameRecords[frame])
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