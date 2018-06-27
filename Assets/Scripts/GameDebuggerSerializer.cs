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
	public class FrameInfo
	{
		public List<string> RecordableInfoJsons;
		
		public FrameInfo(List<string> recordableInfoJsons)
		{
			RecordableInfoJsons = recordableInfoJsons;
		}
		public FrameInfo()
		{
		}
	}
	
	[Serializable]
	public class Recording
	{
		public List<FrameInfo> FrameInfos;
	}

	private static string filePath = "Assets/dump.json";

	public static void DumpToFile()
	{
		Recording r = new Recording();
		r.FrameInfos = new List<FrameInfo>();

		foreach (var frameData in GameDebuggerDatabase.FrameRecords)
		{
			var list = new List<string>();
			foreach (var recordableInfo in frameData.records)
			{
				list.Add(recordableInfo.ToJson());
			}
			r.FrameInfos.Add(new FrameInfo(list));
		}
		Stopwatch s = new Stopwatch();
		s.Start();
		
		File.WriteAllText(filePath, JsonUtility.ToJson(r, true));
		
		long length = new FileInfo(filePath).Length;

		Debug.Log(string.Format("Serialization time {0}s, Size:{1}", s.Elapsed.Seconds, BytesPretyPrint.ToPrettySize(length, 1)));
	}
	
	public static bool LoadDataFromFile()
	{
		if (!File.Exists(filePath)) return false;
		var rec = JsonUtility.FromJson<Recording>(File.ReadAllText(filePath));
		List<GameDebuggerDatabase.FrameInfo> frameRecords = new List<GameDebuggerDatabase.FrameInfo>();
		
		foreach (var frameData in rec.FrameInfos)
		{
			var list = new GameDebuggerDatabase.FrameInfo();
			foreach (var recordableInfoJson in frameData.RecordableInfoJsons)
			{
				list.records.Add(RecordableInfo.FromJson(recordableInfoJson));
			}
			frameRecords.Add(list);
		}

		GameDebuggerDatabase.FrameRecords = frameRecords;
		return true;
	}
}