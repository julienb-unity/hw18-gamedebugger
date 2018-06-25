using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// Purpose of this class:
//   - Fetch data from scene
//   - Provide API for the editor window to retrieve the recorded data
//   - Provide API for the editor window to send data
[CreateAssetMenu]
public class GameDebuggerDatabase : ScriptableObject
{
	private int currentFrame = -1;
	
	public List<UnityObjectRecordedData> recording = new List<UnityObjectRecordedData>();

	public Dictionary<int, UnityObjectRecordedData> instanceIdToDatamapping = new Dictionary<int, UnityObjectRecordedData>();
	
	private void Awake()
	{
		recording.Clear();
	}

	public void RecordNewFrame()
	{
		currentFrame++;
		
		foreach (var obj in FindObjectsOfType<UnityEngine.Object>())
		{
			foreach (Type type in GameDebuggerRecorder.Instance.typeToFieldNameMapping.Keys)
			{
				if (obj.GetType() == type)
				{
					UnityObjectRecordedData data = GetCachedObjectData(obj.GetInstanceID());

					if (data == null)
					{
						data = new UnityObjectRecordedData(obj.GetInstanceID());
						recording.Add(data);
					}
					
					UpdateObjData(data, obj, GameDebuggerRecorder.Instance.typeToFieldNameMapping[type]);
				}
			}
		}
		EditorUtility.SetDirty(this);
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
	
	private void UpdateObjData(UnityObjectRecordedData objData, UnityEngine.Object obj, string fieldName)
	{
		PropertyData propertyData = objData.GetPropertyData(fieldName);
		if (propertyData == null)
		{
			propertyData = new PropertyData(currentFrame);
			objData.propertyData.Add(propertyData);
		}
		FieldInfo fieldInfo = obj.GetType().GetField(fieldName);
		
		if (fieldInfo != null)
		{
			propertyData.frameData.Add(fieldInfo.GetValue(obj));
		}
		else
		{
			PropertyInfo propertyInfo = obj.GetType().GetProperty(fieldName);
			if (propertyInfo != null)
			{
				propertyData.frameData.Add(propertyInfo.GetValue(obj, null));
			}
			else
			{
				Debug.LogError("No property with name " + fieldName + " in type " + obj.GetType());
			}
		}
	}
}
