using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.UIElements;

namespace GameDebugger
{
    public class DebuggerViewer : EditorWindow
    {
        TimeManager m_TimeMgr = new TimeManager();
        
        // Add menu named "My Window" to the Window menu
        [MenuItem("Window/GameDebugger")]
        static void InitWindow()
        {
            // Get existing open window or if none, make a new one:
            var window = (DebuggerViewer)GetWindow(typeof(DebuggerViewer));
            window.titleContent = new GUIContent("Game Debugger");
            window.Show();
        }

        public void OnEnable()
        {
            var root = this.GetRootVisualContainer();

            var header = new HeaderElement();
            var timelineElement = new TimelineElement(m_TimeMgr);

            root.Add(header);
            root.Add(timelineElement);
        }
    }
}