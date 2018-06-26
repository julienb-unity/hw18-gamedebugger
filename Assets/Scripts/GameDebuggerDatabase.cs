using System;
using System.Collections.Generic;
using System.Linq;
using GameDebugger;
using Recordables;
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
	static Dictionary<int, Recordable> m_sessionRecords = new Dictionary<int, Recordable>();
	static List<List<RecordableInfo>> m_frameRecords = new List<List<RecordableInfo>>();
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
		m_sessionRecords.Clear();
		m_frameRecords.Clear();
	}

	public static void RecordFrame(int frame)
	{
		m_frame = frame;
		m_frameRecords.Add(new List<RecordableInfo>());
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			var scene = SceneManager.GetSceneAt(i);
			foreach (var rootGameObject in scene.GetRootGameObjects())
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
				Recordable previous = null;
				m_sessionRecords.TryGetValue(o.GetInstanceID(), out previous);
				if (recordable.OnRecord(previous,o))
				{
					m_frameRecords[m_frame][o.GetInstanceID()] = recordable;
					m_sessionRecords[o.GetInstanceID()] = recordable;
				}
				m_frameRecords[m_frame].Add(new RecordableInfo(o.GetInstanceID(), recordable));
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

	public static List<RecordableInfo> GetRecords(int frame)
	{
		return m_frameRecords[frame];
	}
}
