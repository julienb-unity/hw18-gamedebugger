using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameDebugger
{
	[Serializable]
	public class FrameInfo
	{
		public float time;
		
		[NonSerialized]
		public List<RecordableInfo> records = new List<RecordableInfo>(100);
		
		// dummy list used to hack json serialization
		public List<string> recordableInfoJsons = new List<string>(100);
		
		// JsonUtility cant serialize extended types so we hack it
		public string ToJson()
		{
			recordableInfoJsons.Clear();
			foreach (var recordableInfo in records)
			{
				recordableInfoJsons.Add(recordableInfo.ToJson());
			}
			string json = JsonUtility.ToJson(this, false);
			recordableInfoJsons.Clear();
			return json;
		}

		public static FrameInfo FromJson(string json)
		{
			FrameInfo frameInfo = JsonUtility.FromJson<FrameInfo>(json);
			frameInfo.records = new List<RecordableInfo>();
			foreach (var recordableJson in frameInfo.recordableInfoJsons)
			{
				frameInfo.records.Add(RecordableInfo.FromJson(recordableJson));
			}
			frameInfo.recordableInfoJsons.Clear();
			return frameInfo;
		}
	}

}