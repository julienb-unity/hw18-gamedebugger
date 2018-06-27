using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.UIElements;

namespace GameDebugger
{
    [Serializable]
    class ViewState
    {
        public bool recordOnPlay;
    }

    class DebuggerViewer : EditorWindow
    {
        TimeManager m_TimeMgr = new TimeManager();
        RefreshScheduler m_Scheduler;
        ViewState m_State = new ViewState();
        
        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/GameDebugger")]
        static void InitWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = (DebuggerViewer)GetWindow(typeof(DebuggerViewer));
            window.titleContent = new GUIContent("Game Debugger");
            window.Show();
        }

        [MenuItem("Hackweek/Toggle auto reload")]
        static void ToggleAutoReload()
        {
            const string key = "UIElements_UXMLLiveReload";
            var b = EditorPrefs.GetBool(key, false);
            EditorPrefs.SetBool(key, !b);
            Debug.Log("Auto reload: " + !b);
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();
            m_Scheduler = new RefreshScheduler(root);

            var header = new HeaderElement(m_State);
            var timelineElement = new TimelineElement(m_TimeMgr, m_Scheduler);

            root.Add(header);
            root.Add(timelineElement);
        }
    }
}