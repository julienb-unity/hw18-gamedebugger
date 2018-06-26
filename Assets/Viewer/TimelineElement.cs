using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = UnityEngine.Random;

namespace GameDebugger
{
    class TimelineElement : VisualElement
    {
        TimeAreaGUI m_TimeAreaGUI = new TimeAreaGUI();

        private IVisualElementScheduledItem m_RefreshScheduler;
        private VisualTreeAsset m_ItemTemplate;
        private ListView m_ListView;

        [MenuItem("Hackweek/Toggle auto reload")]
        static void ToggleAutoReload()
        {
            const string key = "UIElements_UXMLLiveReload";
            var b = EditorPrefs.GetBool(key, false);
            EditorPrefs.SetBool(key, !b);
            Debug.Log("Auto reload: " + !b);
        }

        public TimelineElement(TimeManager timeMgr)
        {
            m_ItemTemplate = Resources.Load<VisualTreeAsset>("GameDebuggerTrackItem");

            name = "timeline";
            AddStyleSheetPath("Stylesheets/Styles");
            
            var timeArea = new VisualElement();
            timeArea.name = "timeArea";
            Add(timeArea);
            
            var imguiContainer = new IMGUIContainer(() =>
            {
                m_TimeAreaGUI.OnGUI(timeArea.layout);
            });
            imguiContainer.name = "timeAreaGUI";
            timeArea.Add(imguiContainer);
            imguiContainer.StretchToParentSize();
            
            m_ListView = new ListView(new List<int>(), 50, () => m_ItemTemplate.CloneTree(null), DrawItem);
            m_ListView.selectionType = SelectionType.None;
            Add(m_ListView);

            //the playhead needs to be added at the very end, since it needs to be drawn on top of the tracks
            var playhead = new PlayheadElement(timeMgr, m_TimeAreaGUI);
            playhead.name = "playhead";
            Add(playhead);
            
            imguiContainer.AddManipulator(new PlayheadDragManipulator(playhead));
            playhead.AddManipulator(new PlayheadDragManipulator(playhead));

            m_RefreshScheduler = schedule.Execute(RefreshTracks).Every(100);
        }

        private void RefreshTracks()
        {
            if (EditorApplication.isPaused || !EditorApplication.isPlaying)
                return;

            if (GameDebuggerDatabase.NumFrameRecords == 0)
                return;

            var instanceIdList = (List<int>)m_ListView.itemsSource;
            var records = GameDebuggerDatabase.GetRecords(0);
            foreach (var recordInfo in records)
            {
                UnityEngine.Object o = EditorUtility.InstanceIDToObject(recordInfo.instanceID);
                if (o is GameObject)
                    instanceIdList.Add(recordInfo.instanceID);
            }

            m_ListView.Refresh();
        }

        private void DrawItem(VisualElement elt, int index)
        {
            var instanceIdList = (List<int>)m_ListView.itemsSource;
            UnityEngine.Object o = EditorUtility.InstanceIDToObject(instanceIdList[index]);
            if (o != null)
                elt.Q<Label>().text = o.name;
            else
                elt.Q<Label>().text = "#################";

            var container = elt.Q("itemContainer");
            container.Clear();
            for (int j = 0; j < Random.RandomRange(3,8); j++)
            {
                var item = new Label(j.ToString());
                item.style.positionLeft = j * 20;
                item.AddToClassList("item");
                container.Add(item);
            }
        }
    }
}
