using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GameDebugger;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameDebuggerSerializer 
{
	[Serializable]
	public class Recording
	{
		public List<string> FrameInfos;
	}

	private static string filePath = "Assets/dump.json";

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