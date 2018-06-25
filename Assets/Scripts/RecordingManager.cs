
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

public class RecordingManager
{
	public static RecordingManager Instance
	{
		get
		{
			if (instance == null) instance = new RecordingManager();
			return instance;
		}
	}
	
	private static RecordingManager instance;
	
	public bool isRecording;
	public Dictionary<Type, string> typeToFieldNameMapping = new Dictionary<Type, string>();

	private RecordedData2 recorderDataStorage;
	
	public RecordingManager()
	{
		typeToFieldNameMapping.Add(typeof(GameObject), "position");
		recorderDataStorage = Resources.Load<RecordedData2>("RecorderDataStorage.asset");
	}

	public void StartRecording()
	{
		isRecording = true;
	}
	
	public void StopRecording()
	{
		isRecording = false;
	}

	public List<UnityEngine.Object> GetAllMonitoredObejcts(int frameNumber)
	{
		List<UnityEngine.Object> allObjects = new List<UnityEngine.Object>();
//		RecordedData2.FrameData framedata = recorderDataStorage.recording[frameNumber];
//		foreach (var objData in framedata.data)
//		{
//			UnityEngine.Object x = EditorUtility.InstanceIDToObject(objData.instanceId);
//			allObjects.Add(x);
//		}
		return allObjects;
	}
	
	public List<string> GetAllMonitoredFields(int frameNumber, int instanceId)
	{
		return GetAllMonitoredFields(frameNumber, EditorUtility.InstanceIDToObject(instanceId));
	}
	
	public List<string> GetAllMonitoredFields(int frameNumber, UnityEngine.Object obj)
	{
//		List<string> properties = new List<string>();
//		foreach (RecordedData.ObjectSerializedData objData in recorderDataStorage.recording[frameNumber].data)
//		{
//			if (objData.instanceId == obj.GetInstanceID())
//			{
//				foreach (var key in objData.propertiesData.Keys)
//				{
//					properties.Add(key);
//				}
//			}
//		}
//		return properties;
		return null;
	}
	
	public object GetFieldValue(int frameNumber, UnityEngine.Object obj, string fieldName)
	{
//		foreach (RecordedData.ObjectSerializedData objData in recorderDataStorage.recording[frameNumber].data)
//		{
//			if (objData.instanceId == obj.GetInstanceID())
//			{
//				foreach (var key in objData.propertiesData.Keys)
//				{
//					if (key == fieldName)
//					{
//						return objData.propertiesData[key];
//					}
//				}
//			}
//		}
		return null;
	}
}