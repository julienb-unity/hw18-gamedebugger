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
        VisualElement m_ExtraViwer;

        // Keys by Instance ID.
        Dictionary<int, TrackItem> m_TrackItemByInstance = new Dictionary<int, TrackItem>();
        int numFrames;

        public TrackContainer(ITimeConverter timeConverter, RefreshScheduler scheduler, VisualElement extraViewer)
        {
            m_TimeConverter = timeConverter;
            m_ExtraViwer = extraViewer;
            
            var trackTemplate = Resources.Load<VisualTreeAsset>("Replay/GameDebuggerTrackTemplate");
            
            m_ListView = new ListView(new List<int>(), 50, () => CreateTrack(trackTemplate), DrawItem);
            m_ListView.selectionType = SelectionType.None;
            Add(m_ListView);
            
            scheduler.ExitPlayMode += () =>
            {
                m_ListView.itemsSource = null;
                m_ListView.Refresh();
            };
            scheduler.Refresh += RefreshTracks;
        }

        Track CreateTrack(VisualTreeAsset asset)
        {
            var track = new Track(asset, m_TimeConverter);
            track.AddManipulator(new TrackContentManipulator(m_TimeConverter, m_ExtraViwer));
            return track;
        }

        void DrawItem(VisualElement elt, int index)
        {
            var instanceIdList = (List<int>)m_ListView.itemsSource;
            var instanceId = instanceIdList[index];
            var track = (Track) elt;
            track.item = m_TrackItemByInstance[instanceId];
        }

        void RefreshTracks()
        {
            if (EditorApplication.isPaused)
                return;

            var newNumFrames = GameDebuggerDatabase.NumFrameRecords;
            if (newNumFrames == numFrames)
                return;

            // Get the new instance ID and the new keys.
            for (var f = numFrames; f < newNumFrames; ++f)
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
            
            numFrames = newNumFrames;

            m_ListView.itemsSource = m_TrackItemByInstance.Keys.ToList();
            m_ListView.Refresh();
        }
    }
}
