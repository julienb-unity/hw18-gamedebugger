using System;
using System.Text;
using Recordables;
using UnityEngine;

namespace GameDebugger
{
	[Serializable]
	public class RecordableInfo
	{
		public int instanceID;
		public int localIdentifierInFile;
		
		[NonSerialized]
		public Recordable recordable;

		public RecordableInfo()
		{
		}

		public RecordableInfo( int instanceID, int localIdentifierInFile, Recordable recordable)
		{
			this.instanceID = instanceID;
			this.recordable = recordable;
			this.localIdentifierInFile = localIdentifierInFile;
		}
		public RecordableInfo(int localIdentifierInFile, Recordable recordable)
		{
			GameDebuggerSerializer.localFileIDToInstanceID.TryGetValue(localIdentifierInFile, out this.instanceID);
			this.recordable = recordable;
			this.localIdentifierInFile = localIdentifierInFile;
		}

		// JsonUtility cant serialize extended types so need to save dynamic type separatly
		static StringBuilder sb = new StringBuilder();
		public string ToJson()
		{
			sb.Length = 0;
			sb.Append(localIdentifierInFile);
			sb.Append("&");
			sb.Append(recordable.GetType().FullName);
			sb.Append("&");
			sb.Append(JsonUtility.ToJson(recordable, false));

			return sb.ToString();
		}

		public static RecordableInfo FromJson(string json)
		{
			int index = json.IndexOf("&");
			int localFileID = Int32.Parse(json.Substring(0, index));
			int indexForType = json.IndexOf("&", index + 1, StringComparison.Ordinal);
			string type = json.Substring(index + 1, indexForType - index - 1);

			Recordable rec = (Recordable)JsonUtility.FromJson(json.Substring(indexForType + 1), Type.GetType(type));
			return new RecordableInfo(localFileID, rec);
		}
	}
}