using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Purpose of this class:
//   - Fetch data from scene
//   - Provide API for the editor window to retrieve the recorded data
//   - Provide API for the editor window to send data
[CreateAssetMenu]
public class GameDebuggerDatabase : ScriptableObject
{
	static Dictionary<Type,List<Type>> m_TypeToRecordable = new Dictionary<Type, List<Type>>();
	static List<Dictionary<int, Recordable>> m_frameRecords = new List<Dictionary<int, Recordable>>();
	private static int m_frame;
	public static int NumFrameRecords
	{
		get { return m_frameRecords.Count; }
	}

	public static void Init()
	{
		List<Type> recordables = new List<Type>();
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (var assembly in assemblies)
			recordables.AddRange(ReflectionHelpers.GetDerivedTypes(typeof(Recordable<>),assembly).ToList());

		foreach (var recordable in recordables)
		{
			if (recordable.IsGenericType)
				continue;

			var t = recordable.BaseType.GetGenericArguments()[0];
			
			if (!m_TypeToRecordable.ContainsKey(t))
				m_TypeToRecordable[t]= new List<Type>();

			m_TypeToRecordable[t].Add(recordable);				
			Debug.Log(recordable); 
		}

	}

	public static void Clear()
	{
		m_frameRecords.Clear();
	}

	public static void RecordFrame(int frame)
	{
		m_frame = frame;
		m_frameRecords.Add(new Dictionary<int, Recordable>());
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var scene = SceneManager.GetSceneAt(i);
			foreach (var rootGameObject in scene.GetRootGameObjects(	))
			{
				RecordGameObject(rootGameObject);
			}
		}
	}
	
	private static void RecordData(Type t, UnityEngine.Object o)
	{
		List<Type> list;
		if (m_TypeToRecordable.TryGetValue(t, out list))
		{
			foreach (var type in list)
			{
				var recordable = (Recordable)Activator.CreateInstance(type);
				recordable.OnRecord(o);
				m_frameRecords[m_frame][o.GetInstanceID()] = recordable;
			}
		}
	}

	private static void RecordGameObject(GameObject gameObject)
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

	public static Dictionary<int, Recordable> GetRecords(int frame)
	{
		return m_frameRecords[frame];
	}
}
