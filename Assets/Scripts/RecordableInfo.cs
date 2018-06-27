﻿using System;
using System.Text;
using Recordables;
using UnityEngine;

namespace GameDebugger
{
	[Serializable]
	public class RecordableInfo
	{
		public int instanceID;
		
		[SerializeField]
		public Recordable recordable;

		public RecordableInfo()
		{
		}

		public RecordableInfo(int instanceID, Recordable recordable)
		{
			this.instanceID = instanceID;
			this.recordable = recordable;
		}

		// JsonUtility cant serialize extended types so need to save dynamic type separatly
		public string ToJson()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(instanceID);
			sb.Append("&");
			sb.Append(recordable.GetType().FullName);
			sb.Append("&");
			sb.Append(JsonUtility.ToJson(recordable, false));
			
			return sb.ToString();
		}

		public static RecordableInfo FromJson(string json)
		{
			int index = json.IndexOf("&");
			int instanceId = Int32.Parse(json.Substring(0, index));
			int indexForType = json.IndexOf("&", index + 1, StringComparison.Ordinal);
			string type = json.Substring(index + 1, indexForType - index - 1);
		
			Recordable rec = (Recordable)JsonUtility.FromJson(json.Substring(indexForType + 1), Type.GetType(type));
			return new RecordableInfo(instanceId, rec);
		}
	}
}