using UnityEngine;
using UnityEditor;

// Purpose of this class:
//   - Layer between the scene and the editor window
//   - Allows the game dev to define what data to record
//   - Discuss with the database only (never the editor)

[InitializeOnLoad]
public class GameDebuggerRecorder
{
	public static bool IsRecording;
	public static int currentFrame;
	public static bool isPlaying;
	public static bool isPaused;

	private static GameObject m_UpdaterGo;

	static GameDebuggerRecorder()
	{
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
			if (IsRecording)
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
	
	public static void ReplayFrame(int frame)
	{
		if (frame >= GameDebuggerDatabase.NumFrameRecords)
		{
			return;
		}

		EditorApplication.isPaused = true;
		GameDebuggerDatabase.ReplayFrame(frame);
	}

	public static void ReplayTime(float time)
	{
		EditorApplication.isPaused = true;
		GameDebuggerDatabase.ReplayTime(time);
	}

	public static void StartRecording()
	{
		if (isPlaying)
			StopReplay();
		
		if (IsRecording)
			return;

		GameDebuggerDatabase.Clear();
		currentFrame = 0;

		// Create hidden GameObject for the LateUpdate callback.
		m_UpdaterGo = new GameObject("GameDebugger");
		m_UpdaterGo.hideFlags |= HideFlags.DontSave | HideFlags.HideInHierarchy;
		m_UpdaterGo.AddComponent<GameDebuggerBehaviour>();

		IsRecording = true;
	}

	public static void StopRecording()
	{
		if (!IsRecording)
			return;

		IsRecording = false;
		
		// Remove hidden GameObject.
		UnityEngine.Object.DestroyImmediate(m_UpdaterGo);

		Debug.LogFormat("Recorded {0} frames",GameDebuggerDatabase.NumFrameRecords);
		GameDebuggerSerializer.DumpToFile();
	}

	public static void Update()
	{
		if (!IsRecording)
			return;

		GameDebuggerDatabase.RecordFrame(currentFrame);
		
		currentFrame ++;
	}
}