using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class GameDebuggerPlayer
{
	public static bool isPlaying;

	static GameDebuggerPlayer()
	{
		EditorApplication.update += Update; 
	}

	private static int m_frame = 0;
	public static void StartReplay()
	{
		if (isPlaying)
			return;

		m_frame = 0;
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

//		if (ReplayFrame(m_frame)) return;
//
//		m_frame++;
	}

	public static bool ReplayFrame(int frame)
	{
		StartReplay();
		
		if (frame >= GameDebuggerDatabase.NumFrameRecords)
		{
			StopReplay();
			return true;
		}

		GameDebuggerDatabase.ReplayFrame(frame);

		return false;
	}
}