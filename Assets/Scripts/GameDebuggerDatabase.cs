﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameDebugger;
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
	static Dictionary<int, Recordable> m_sessionRecords = new Dictionary<int, Recordable>();
	static List<List<RecordableInfo>> m_frameRecords = new List<List<RecordableInfo>>();
	private static int m_frame;
	private static bool m_sync;

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
		if (m_frame % 60 == 0)
			m_sync = true;
		else
			m_sync = false;
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


	public class TrackData
	{
		public int GameObjectInstanceID;
		public List<RecordableInfo> recordables = new List<RecordableInfo>();
	}
	public static List<TrackData> GetTrackDatas()
	{
		List<TrackData> result = null;

		foreach (var frameRecord in m_frameRecords)
		{
			foreach (var recordableInfo in frameRecord)
			{
				result = new List<TrackData>();
				var go = EditorUtility.InstanceIDToObject(recordableInfo.instanceID) as GameObject;
				var instanceID = go.GetInstanceID();
				if (go != null)
				{
					var tdata = result.Find(data => data.GameObjectInstanceID == instanceID); 
					if ( tdata == null)
					{
						tdata = new TrackData() {GameObjectInstanceID = instanceID};
					}
					tdata.recordables.Add(recordableInfo);
					result.Add(tdata);
				}
			}

		}
		if (result != null)
		{
			foreach (var trackData in result)
			{
				trackData.recordables.Sort((a, b) => a.instanceID.CompareTo(b.instanceID));
			}
			result.Sort((data, trackData) => data.GameObjectInstanceID.CompareTo(trackData.GameObjectInstanceID));
		}		
		
		return result;
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
				if (!m_sync)
					m_sessionRecords.TryGetValue(o.GetInstanceID(), out previous);
	
				if (recordable.OnRecord(previous,o))
				{
					m_frameRecords[m_frame].Add(new RecordableInfo(o.GetInstanceID(), recordable));
					m_sessionRecords[o.GetInstanceID()] = recordable;
				}
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

	public static void ReplayFrame(int frame)
	{
		int lastSync = frame - frame % 60;
		for (int i = lastSync; i < NumFrameRecords && i <= frame; ++i)
		{
			foreach (var recordableInfo in m_frameRecords[i])
			{
				var obj = EditorUtility.InstanceIDToObject(recordableInfo.instanceID);
				if (obj != null)
					recordableInfo.recordable.OnReplay(obj);
			}
		}
//		for (int i = 0; i < SceneManager.sceneCount; i++)
//		{
//			var scene = SceneManager.GetSceneAt(i);
//			foreach (var rootGameObject in scene.GetRootGameObjects())
//			{
//				ReplayGameObject(frame, rootGameObject);
//			}
//		}
	}

	private static void ReplayGameObject(int frame, GameObject gameObject)
	{
		ReplayData(frame, gameObject);

		foreach (var component in gameObject.GetComponents<Component>())
		{
			ReplayData(frame, component);
		}

		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			ReplayGameObject(frame,gameObject.transform.GetChild(i).gameObject);
		}
	}

	private static void ReplayData(int frame, UnityEngine.Object obj)
	{
		int f = frame;
		int instanceID = obj.GetInstanceID();
		while (f >= 0)
		{
			RecordableInfo rec = null;
			for (int i = 0; i < m_frameRecords[f].Count; ++i)
			{
				var record = m_frameRecords[f][i];
				if (record.instanceID == instanceID)
				{
					rec = record;
					break;
				}			}
			if (rec != null)
			{
				rec.recordable.OnReplay(obj);
				break;
			}
			else
			{
				f--;
			}
		}
	}
}
