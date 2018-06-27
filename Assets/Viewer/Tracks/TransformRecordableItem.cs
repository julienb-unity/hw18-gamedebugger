using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TransformRecordableItem : ITrackItem
    {
        int m_InstanceId;
        List<int> m_FrameIds;

        readonly Color m_KeyColor = Color.Lerp(Color.black, Color.white, 0.2f);
        readonly Color m_LineColor = Color.Lerp(Color.black, Color.white, 0.5f);
        
        public TransformRecordableItem(int instanceId, int frame)
        {
            m_FrameIds = new List<int>(200){frame};
            m_InstanceId = instanceId;
        }

        public void Draw(Track track, ITimeConverter converter)
        {
            var o = EditorUtility.InstanceIDToObject(m_InstanceId);
            track.Q<Label>().text = o.name;
            
            DrawLine(track, converter);
            DrawKeys(converter);
        }

        public void Refresh(RecordableInfo recordableInfo, int frame)
        {
            var tr = recordableInfo.recordable as TransformRecordable;
            if (tr != null)
            {
                var lastRecords = GameDebuggerDatabase.GetRecords(m_FrameIds.Last());
                var info = lastRecords.records.Find(otherRecInfo => m_InstanceId == otherRecInfo.instanceID);
                if (!tr.ApproximatelyEquals((TransformRecordable) info.recordable))
                    m_FrameIds.Add(frame);
            }
        }

        void DrawKeys(ITimeConverter converter)
        {
            foreach (var frameId in m_FrameIds)
            {
                var frameTime = GameDebuggerDatabase.GetRecords(frameId).time;
                var keyPixelXPos = converter.TimeToPixel(frameTime);
                EditorGUI.DrawRect(new Rect(keyPixelXPos - 3, 22, 6, 6), m_KeyColor);
            }
        }

        void DrawLine(Track track, ITimeConverter converter)
        {
            var frameTime = GameDebuggerDatabase.GetRecords(m_FrameIds.First()).time;
            var x = converter.TimeToPixel(frameTime);
            var w = track.contentRect.width;
            EditorGUI.DrawRect(new Rect(x, 24, w, 1), m_LineColor);
        }
    }
}
