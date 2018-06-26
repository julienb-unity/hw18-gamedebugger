using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TrackContainer : VisualElement
    {
        [MenuItem("Hackweek/Toggle auto reload")]
        static void ToggleAutoReload()
        {
            const string key = "UIElements_UXMLLiveReload";
            var b = EditorPrefs.GetBool(key, false);
            EditorPrefs.SetBool(key, !b);
            Debug.Log("Auto reload: " + !b);
        }
        
        ListView m_ListView;

        public TrackContainer(RefreshScheduler scheduler)
        {
            var itemTemplate = Resources.Load<VisualTreeAsset>("GameDebuggerTrackItem");
            
            m_ListView = new ListView(new List<int>(), 50, () => itemTemplate.CloneTree(null), DrawItem);
            m_ListView.selectionType = SelectionType.None;
            Add(m_ListView);
            
            scheduler.ExitPlayMode += () =>
            {
                m_ListView.itemsSource = null;
                m_ListView.Refresh();
            };
            scheduler.Refresh += RefreshTracks;
        }
       
        void DrawItem(VisualElement elt, int index)
        {
            var instanceIdList = (List<int>)m_ListView.itemsSource;
            UnityEngine.Object o = EditorUtility.InstanceIDToObject(instanceIdList[index]);
            elt.Q<Label>().text = o.name;

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
        
        void RefreshTracks()
        {
            if (EditorApplication.isPaused)
                return;

            if (GameDebuggerDatabase.NumFrameRecords == 0)
                return;

            var instanceIdList = (List<int>)m_ListView.itemsSource;
            var records = GameDebuggerDatabase.GetRecords(0);
            foreach (var recordInfo in records)
            {
                Object o = EditorUtility.InstanceIDToObject(recordInfo.instanceID);
                instanceIdList.Add(recordInfo.instanceID);
            }

            m_ListView.Refresh();
        }
    }
}
