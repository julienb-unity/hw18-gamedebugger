using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using Object = System.Object;

// Purpose of this class:
//   - MonoBehaviour to be used by the game developer
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
	List<Type> s_recordables = new List<Type>();
	Dictionary<Type,List<Type>> s_TypeToRecordable = new Dictionary<Type, List<Type>>();
	private List<Dictionary<int, Recordable>> s_frameRecords = new List<Dictionary<int, Recordable>>();
	private int s_currentFrame;

	private GameDebuggerRecorder()
	{
		recorderDataStorage = Resources.Load<GameDebuggerDatabase>("GameDebuggerRecording");

		var go = new GameObject("GameDebugger");
		go.hideFlags |= HideFlags.DontSave | HideFlags.HideInHierarchy;
		go.AddComponent<GameDebuggerBehaviour>();
		
		
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (var assembly in assemblies)
			s_recordables.AddRange(ReflectionHelpers.GetDerivedTypes(typeof(Recordable<>),assembly).ToList());

		foreach (var recordable in s_recordables)
		{
			if (recordable.IsGenericType)
				continue;

			var t = recordable.BaseType.GetGenericArguments()[0];
			
			if (!s_TypeToRecordable.ContainsKey(t))
				s_TypeToRecordable[t]= new List<Type>();

			s_TypeToRecordable[t].Add(recordable);				
			Debug.Log(recordable); 
		}
	}

	public void StartRecording()
	{
		if (isRecording)
			return;

		s_frameRecords.Clear();
		s_currentFrame = -1;

		isRecording = true;
	}
	
	public void StopRecording()
	{
		if (!isRecording)
			return;

		isRecording = false;
	}

	public void AddPropertyToRecord(Type type, string propName)
	{		
		typeToFieldNameMapping.Add(type, propName);
	}

	public void Update()
	{
		if (!isRecording)
			return;

		s_currentFrame++;
		s_frameRecords.Add(new Dictionary<int, Recordable>());
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var scene = SceneManager.GetSceneAt(i);
			foreach (var rootGameObject in scene.GetRootGameObjects(	))
			{
				RecordGameObject(rootGameObject);
			}
		}
		
		//recorderDataStorage.RecordNewFrame();
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
	
	
	private void RecordData(Type t, UnityEngine.Object o)
	{
		List<Type> list;
		if (s_TypeToRecordable.TryGetValue(t, out list))
		{
			foreach (var type in list)
			{
				var recordable = (Recordable)Activator.CreateInstance(type);
				recordable.OnRecord(o);
				s_frameRecords[s_currentFrame][o.GetInstanceID()] = recordable;
//				s_frameRecords[s_currentFrame][o.GetInstanceID()]=recordable;
			}
		}
	}
	
	private void RecordGameObject(GameObject gameObject)
	{
		RecordData(typeof(GameObject), gameObject);
		foreach (var component in gameObject.GetComponents<Component>())
		{
			RecordData(component.GetType(), component);
		}

		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			RecordGameObject(gameObject.transform.GetChild(i).gameObject);
		}
	}
}