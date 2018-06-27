using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TrackContainer : VisualElement
    {
        ListView m_ListView;
        ITimeConverter m_TimeConverter;

        class KeyContainer : VisualElement
        {
            public List<int> keys;

            private readonly ITimeConverter m_TimeConverter;
            private readonly Color m_KeyColor = Color.Lerp(Color.black, Color.white, 0.2f);

            public KeyContainer(ITimeConverter timeConverter)
            {
                m_TimeConverter = timeConverter;
            }

            public override void DoRepaint()
            {
                foreach (var key in keys)
                {
                    var x = m_TimeConverter.TimeToPixel(key);
                    EditorGUI.DrawRect(new Rect(x - 3, 22, 6, 6), m_KeyColor);
                }
            }
        }

        class Track : VisualElement
        {
            public KeyContainer KeyContainer;

            private readonly ITimeConverter m_TimeConverter;
            private readonly Color m_LineColor = Color.Lerp(Color.black, Color.white, 0.5f);

            public Track(VisualTreeAsset itemTemplate, ITimeConverter timeConverter)
            {
                m_TimeConverter = timeConverter;

                itemTemplate.CloneTree(this, null);

                KeyContainer = new KeyContainer(m_TimeConverter);
                this.Q(className: "track").Add(KeyContainer);
            }

            public override void DoRepaint()
            {
                if (KeyContainer.keys.Count == 0)
                    return;
                var x = m_TimeConverter.TimeToPixel(KeyContainer.keys.First());
                var w = contentRect.width;
                EditorGUI.DrawRect(new Rect(x, 24, w, 1), m_LineColor);
            }
        }

        // Keys by Instance ID.
        private Dictionary<int, List<int>> m_InstanceIdKeyMap = new Dictionary<int, List<int>>(300);
        private int numFrames;

        public TrackContainer(ITimeConverter timeConverter, RefreshScheduler scheduler)
        {
            m_TimeConverter = timeConverter;
            
            var itemTemplate = Resources.Load<VisualTreeAsset>("GameDebuggerTrackItem");
            
            m_ListView = new ListView(new List<int>(), 50, () => new Track(itemTemplate, m_TimeConverter), DrawItem);
            m_ListView.selectionType = SelectionType.None;
            Add(m_ListView);
            
            scheduler.ExitPlayMode += () =>
            {
                m_ListView.itemsSource = null;
                m_ListView.Refresh();
            };
            scheduler.Refresh += RefreshTracks;
        }

        private void DrawItem(VisualElement elt, int index)
        {
            var instanceIdList = (List<int>)m_ListView.itemsSource;
            var instanceId = instanceIdList[index];
            UnityEngine.Object o = EditorUtility.InstanceIDToObject(instanceId);
            elt.Q<Label>().text = o.name;
            Track trackItem = (Track) elt;
            trackItem.KeyContainer.keys = m_InstanceIdKeyMap[instanceId];
        }

        void DrawClipAtTime(VisualElement trackContainer, float time, float end, string label)
        {
            var clip = new Box();
            var startPixelPos = m_TimeConverter.TimeToPixel(time);
            clip.style.positionLeft = startPixelPos;
            clip.AddToClassList("clip");
            clip.Add(new Label(label));
            var widthInPixels = m_TimeConverter.TimeToPixel(end) - startPixelPos;
            clip.style.width = widthInPixels;
            trackContainer.Add(clip);
        }

        private void RefreshTracks()
        {
            if (EditorApplication.isPaused)
                return;

            if (GameDebuggerDatabase.NumFrameRecords == numFrames)
                return;

            // Get the new instance ID and the new keys.
            for (var f = numFrames; f < GameDebuggerDatabase.NumFrameRecords; ++f)
            {
                var records = GameDebuggerDatabase.GetRecords(f);
                foreach (var recordInfo in records)
                {
                    if (!m_InstanceIdKeyMap.ContainsKey(recordInfo.instanceID))
                    {
                        var keys = new List<int>(200) { f };
                        m_InstanceIdKeyMap[recordInfo.instanceID] = keys;
                    }
                    else
                    {
                        var keys = m_InstanceIdKeyMap[recordInfo.instanceID];
                        var tr = recordInfo.recordable as TransformRecordable;
                        if (tr != null)
                        {
                            var lastRecords = GameDebuggerDatabase.GetRecords(keys.Last());
                            var info = lastRecords.Find(recordableInfo => recordableInfo.instanceID == recordInfo.instanceID);
                            if (!tr.ApproximatelyEquals((TransformRecordable)info.recordable))
                                keys.Add(f);
                        }
                    }
                }
            }

            numFrames = GameDebuggerDatabase.NumFrameRecords;

            m_ListView.itemsSource = m_InstanceIdKeyMap.Keys.ToList();
            m_ListView.Refresh();
        }
    }
}
