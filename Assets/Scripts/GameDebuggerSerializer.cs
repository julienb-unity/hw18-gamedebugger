using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameDebugger;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class GameDebuggerSerializer 
{
	[Serializable]
	public class Recording
	{
		public List<string> FrameInfos;
	}

	private static string filePath = "dump.json";
	public static Dictionary<int, int> localFileIDToInstanceID;

	public static void DumpToFile()
	{
		Stopwatch s = new Stopwatch();
		s.Start();
		
		Recording r = new Recording();
		r.FrameInfos = new List<string>();

		foreach (var frameData in GameDebuggerDatabase.FrameRecords)
		{
			r.FrameInfos.Add(frameData.ToJson());
		}
		
		File.WriteAllText(filePath, JsonUtility.ToJson(r, true));
		
		long length = new FileInfo(filePath).Length;

		Debug.Log(string.Format("Serialization time {0}s, Size:{1}", s.Elapsed.Seconds, BytesPretyPrint.ToPrettySize(length, 1)));
	}
	
	public static bool LoadDataFromFile()
	{
		if (!File.Exists(filePath)) return false;

		List<Component> components = new List<Component>();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var scene = SceneManager.GetSceneAt(i);
			foreach (var rootGameObject in scene.GetRootGameObjects())
			{
				components.AddRange(rootGameObject.GetComponents<Component>());
			}
		}
		
		localFileIDToInstanceID = new Dictionary<int, int>(components.Count);
		foreach (var component in components) 
		{
			var id = UnityEditor.Unsupported.GetLocalIdentifierInFile(component.GetInstanceID());
			if (id != 0)
				localFileIDToInstanceID.Add(id,component.GetInstanceID());
		}
		
		var rec = JsonUtility.FromJson<Recording>(File.ReadAllText(filePath));
		List<FrameInfo> frameRecords = new List<FrameInfo>();
		
		foreach (var frameData in rec.FrameInfos)
		{
			frameRecords.Add(FrameInfo.FromJson(frameData));
		}

		GameDebuggerDatabase.FrameRecords = frameRecords;
		return true;
	}
}