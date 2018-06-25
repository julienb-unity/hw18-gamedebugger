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
//   - Layer between the scene and the editor window
//   - Allows the game dev to define what data to record
//   - Discuss with the database only (never the editor)
public class GameDebuggerRecorder
{
	private static GameDebuggerRecorder m_Instance;
	public static GameDebuggerRecorder Instance
	{
		get
		{
			if (m_Instance == null)
				m_Instance = new GameDebuggerRecorder();
			return m_Instance;
		}
	}

	public bool isRecording;
	public Dictionary<Type, string> typeToFieldNameMapping = new Dictionary<Type, string>();

	private GameDebuggerDatabase recorderDataStorage;

	private GameDebuggerRecorder()
	{
		recorderDataStorage = Resources.Load<GameDebuggerDatabase>("GameDebuggerRecording");

		var go = new GameObject("GameDebugger");
		go.hideFlags |= HideFlags.DontSave | HideFlags.HideInHierarchy;
		go.AddComponent<GameDebuggerBehaviour>();
	}

	public void StartRecording()
	{
		if (isRecording)
			return;

		Debug.Log("Start recording");
		isRecording = true;
	}
	
	public void StopRecording()
	{
		if (!isRecording)
			return;

		Debug.Log("Stop recording");
		isRecording = false;
	}

	public void AddPropertyToRecord(Type type, string propName)
	{		
		typeToFieldNameMapping.Add(type, propName);
	}

	public void Record()
	{
		if (!isRecording)
			return;

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