using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TrackItem
    {
        readonly Color m_KeyColor = Color.Lerp(Color.black, Color.white, 0.2f);
        readonly Color m_LineColor = Color.Lerp(Color.black, Color.white, 0.5f);
        
        List<int> m_Keys = new List<int>(200);
        RecordableInfo recordableInfo { get; set; }
        
        public TrackItem(RecordableInfo recordableInfo, int frame)
        {
            m_Keys.Add(frame);
            this.recordableInfo = recordableInfo;
        }

        public void Draw(Track track, ITimeConverter converter)
        {
            var o = EditorUtility.InstanceIDToObject(recordableInfo.instanceID);
            track.Q<Label>().text = o.name;
            
            DrawLine(track, converter);
            DrawKeys(converter);
        }
        
        public void Refresh(int frame)
        {
            var tr = recordableInfo.recordable as TransformRecordable;
            if (tr != null)
            {
                var lastRecords = GameDebuggerDatabase.GetRecords(m_Keys.LastOrDefault());
                var info = lastRecords.Find(otherRecInfo => recordableInfo.instanceID == otherRecInfo.instanceID);
                if (!tr.ApproximatelyEquals((TransformRecordable)info.recordable))
                    m_Keys.Add(frame);
            }
        }

        void DrawKeys(ITimeConverter converter)
        {
            foreach (var key in m_Keys)
            {
                var keyPixelXPos = converter.TimeToPixel(key);
                EditorGUI.DrawRect(new Rect(keyPixelXPos - 3, 22, 6, 6), m_KeyColor);
            }
        }

        void DrawLine(Track track, ITimeConverter converter)
        {
            var x = converter.TimeToPixel(m_Keys.First());
            var w = track.contentRect.width;
            EditorGUI.DrawRect(new Rect(x, 24, w, 1), m_LineColor);
        }
    }
}
