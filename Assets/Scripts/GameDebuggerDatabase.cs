using System;
using System.Collections.Generic;
using System.Linq;
using GameDebugger;
using Recordables;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

// Purpose of this class:
//   - Fetch data from scene
//   - Provide API for the editor window to retrieve the recorded data
//   - Provide API for the editor window to send data
public class GameDebuggerDatabase
{
	public static event Action GameDebugerDatabaseLoaded;
	public static event Action<List<GameObject>> FilteredGameObjects;
	private static Dictionary<Type,List<Type>> m_TypeToRecordable = new Dictionary<Type, List<Type>>();
	private static Dictionary<int, Recordable> m_sessionRecords = new Dictionary<int, Recordable>();
	private static List<FrameInfo> m_frameRecords = new List<FrameInfo>();
	private static int m_frame;
	private static float m_StartRecordingTime;
	private static bool m_sync;

	public static int NumFrameRecords
	{
		get { return m_frameRecords.Count; }
	}
	
	public static List<FrameInfo> FrameRecords
	{
		get { return m_frameRecords; }
		set
		{
			m_frameRecords = value;
			if (GameDebugerDatabaseLoaded != null) GameDebugerDatabaseLoaded();
		}
	}

	public static float StartRecordingTime
	{
		get { return m_StartRecordingTime; }
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
		// First recording frame, save starting time.
		if (frame == 0)
			m_StartRecordingTime = Time.unscaledTime;

		m_frame = frame;
		if (m_frame % 60 == 0)
			m_sync = true;
		else
			m_sync = false;
		
		m_frameRecords.Add(new FrameInfo() { time = Time.unscaledTime - m_StartRecordingTime });
		List<GameObject> objects = new List<GameObject>(); 
		if (FilteredGameObjects != null)
		{
			FilteredGameObjects(objects);
			var list = objects.Distinct().ToList();

			foreach (var gameObject in list)
			{
				RecordGameObject(gameObject);
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
				if (!m_sync)
					m_sessionRecords.TryGetValue(o.GetInstanceID(), out previous);
	
				if (recordable.OnRecord(previous, o))
				{
					var component = o as Component;
					var gameObject = component == null ? null : component.gameObject;
					m_frameRecords[m_frame].records.Add(new RecordableInfo(o.GetInstanceID(),GameDebuggerSerializer.GetID(o), recordable));
					m_sessionRecords[o.GetInstanceID()] = recordable;
				}
			}
		}
	}

	private static void RecordGameObject(GameObject gameObject)
	{
		foreach (var component in gameObject.GetComponents<Component>())
		{
			RecordData(component.GetType(), component);
		}

		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			RecordGameObject(gameObject.transform.GetChild(i).gameObject);
		}
	}

	public static FrameInfo GetRecords(int frame)
	{
		return m_frameRecords[frame];
	}
	
	public static int ReplayTime(double time)
	{
		if (NumFrameRecords <= 0)
			return 0;
		int frame = NumFrameRecords -1;
		while (m_frameRecords[frame].time > time)
			frame--;
		if (frame>=0)
			ReplayFrame(frame);
		return frame;
	}

	public static void ReplayFrame(int frame)
	{
		HashSet<SkinnedMeshRenderer> smrs = new HashSet<SkinnedMeshRenderer>();

		int lastSync = frame - frame % 60;
		for (int i = lastSync; i < NumFrameRecords && i <= frame; ++i)
		{
			foreach (var recordableInfo in m_frameRecords[i].records)
			{
				var obj = EditorUtility.InstanceIDToObject(recordableInfo.instanceID);
				if (obj != null)
				{
					recordableInfo.recordable.OnReplay(obj);

					var smr = ((Component) obj).GetComponent<SkinnedMeshRenderer>();
					if (smr != null)
						smrs.Add(smr);
					else
					{
//						Debug.LogError("smr is null");
					}
				}
				else
				{
					Debug.LogError("Object is null for instanceId:" + recordableInfo.instanceID);
				}
			}
		}

		// HACK: Force update of the SkinnedMeshRenderer component.
		//       Bug happens apparently only on Mac.
		//       Tested from 2018.2.0b6 up until 2018.3.0a1, still happens.
		foreach (var smr in smrs)
		{
			smr.enabled = false;
			smr.enabled = true;
		}
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
			for (int i = 0; i < m_frameRecords[f].records.Count; ++i)
			{
				var record = m_frameRecords[f].records[i];
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
