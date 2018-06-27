using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace GameDebugger
{
    [Serializable]
    class ViewState
    {
        public bool recordOnPlay;
    }

    class DebuggerViewer : EditorWindow
    {
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

            var debuggerView = new VisualElement();
            var extraViewer = new VisualElement();
            debuggerView.name = "debuggerView";
            extraViewer.name = "extraViewer";
            debuggerView.style.flex = 1;
            
            var header = new HeaderElement(m_State);
            var timelineElement = new TimelineElement(m_Scheduler, extraViewer);

            debuggerView.Add(header);
            debuggerView.Add(timelineElement);
            
            root.Add(debuggerView);
            
            extraViewer.Add(new Label("This will change soon... be patient"));
            root.Add(extraViewer);
            root.style.flexDirection = FlexDirection.Row;
        }
        
    }
}