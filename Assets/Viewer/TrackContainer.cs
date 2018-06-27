using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TrackContainer : VisualElement
    {
        ListView m_ListView;
        ITimeConverter m_TimeConverter;

        // Keys by Instance ID.
        Dictionary<int, ITrackItem> m_TrackItemByInstance = new Dictionary<int, ITrackItem>();
        int numFrames;

        public TrackContainer(ITimeConverter timeConverter, RefreshScheduler scheduler)
        {
            m_TimeConverter = timeConverter;
            
            var trackTemplate = Resources.Load<VisualTreeAsset>("GameDebuggerTrackTemplate");
            
            m_ListView = new ListView(new List<int>(), 50, () => new Track(trackTemplate, m_TimeConverter), DrawItem);
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
            var instanceId = instanceIdList[index];
            var track = (Track) elt;
            track.item = m_TrackItemByInstance[instanceId];
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

        void RefreshTracks()
        {
            if (EditorApplication.isPaused)
                return;

            if (GameDebuggerDatabase.NumFrameRecords == numFrames)
                return;

            // Get the new instance ID and the new keys.
            for (var f = numFrames; f < GameDebuggerDatabase.NumFrameRecords; ++f)
            {
                var records = GameDebuggerDatabase.GetRecords(f);
                foreach (var recordInfo in records.records)
                {
                    if (!m_TrackItemByInstance.ContainsKey(recordInfo.instanceID))
                        m_TrackItemByInstance[recordInfo.instanceID] = TrackItemFactory.Create(recordInfo, f);
                    else
                    {
                        var item = m_TrackItemByInstance[recordInfo.instanceID];
                        if (item != null)
                            item.Refresh(recordInfo, f);
                    }
                }
            }
            
            numFrames = GameDebuggerDatabase.NumFrameRecords;

            m_ListView.itemsSource = m_TrackItemByInstance.Keys.ToList();
            m_ListView.Refresh();
        }
    }
}
