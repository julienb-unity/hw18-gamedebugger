using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Object = System.Object;

// Purpose of this class:
//   - MonoBehaviour to be used by the game developer
//   - Allows the game dev to define what data to record
//   - Discuss with the database only (never the editor)
public class GameDebuggerRecorder : MonoBehaviour
{
	public static GameDebuggerRecorder Instance;
		
	public bool isRecording;
	public Dictionary<Type, string> typeToFieldNameMapping = new Dictionary<Type, string>();

	private GameDebuggerDatabase recorderDataStorage;

	public void Awake()
	{
		Instance = this;
	}
	
	public void Start()
	{
		AddPropertyToRecord(typeof(Transform), "position");
		recorderDataStorage = Resources.Load<GameDebuggerDatabase>("GameDebuggerRecording");
	}

	public void StartRecording()
	{
		isRecording = true;
	}
	
	public void StopRecording()
	{
		isRecording = false;
	}

	public void AddPropertyToRecord(Type type, string propName)
	{		
		typeToFieldNameMapping.Add(type, propName);
		
	}

	public void Update()
	{
		recorderDataStorage.RecordNewFrame();
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