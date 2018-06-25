using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Purpose of this class:
//   - Fetch data from scene
//   - Provide API for the editor window to retrieve the recorded data
//   - Provide API for the editor window to send data
[CreateAssetMenu]
public class GameDebuggerDatabase : ScriptableObject
{
	private int currentFrame = 0;
	public List<UnityObjectRecordedData> recording = new List<UnityObjectRecordedData>();

	public Dictionary<int, UnityObjectRecordedData> instanceIdToDatamapping = new Dictionary<int, UnityObjectRecordedData>();
	
	private void Awake()
	{
		recording.Clear();
	}

	public void LateUpdate()
	{
		if (!RecordingManager.Instance.isRecording) return;

		currentFrame++;
		
		foreach (var obj in FindObjectsOfType<UnityEngine.Object>())
		{
			foreach (Type type in RecordingManager.Instance.typeToFieldNameMapping.Keys)
			{
				if (obj.GetType() == type)
				{
					UnityObjectRecordedData data = GetCachedObjectData(obj.GetInstanceID());

					if (data == null)
					{
						data = new UnityObjectRecordedData(obj.GetInstanceID());
						recording.Add(data);
					}
					
					UpdateObjData(data, RecordingManager.Instance.typeToFieldNameMapping[type]);
				}
			}
		}
	}

	private UnityObjectRecordedData GetCachedObjectData(int instanceId)
	{
		if (instanceIdToDatamapping.ContainsKey(instanceId)) return instanceIdToDatamapping[instanceId];
		foreach (var objectRecordedData in recording)
		{
			if (objectRecordedData.instanceId == instanceId)
			{
				instanceIdToDatamapping.Add(instanceId, objectRecordedData);
				return objectRecordedData;
			}
		}
		return null;
	}
	
	private void UpdateObjData(UnityObjectRecordedData obj, string fieldName)
	{
		PropertyData propertyData = obj.GetPropertyData(fieldName);
		if (propertyData == null)
		{
			propertyData = new PropertyData(currentFrame);
			obj.propertyData.Add(propertyData);
		}
		propertyData.frameData.Add(obj.GetType().GetField(fieldName).GetValue(obj));
	}
}
