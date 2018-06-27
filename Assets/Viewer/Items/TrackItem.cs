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
        List<int> m_Keys = new List<int>(200);
        
        RecordableInfo recordableInfo { get; set; }
        
        public TrackItem(RecordableInfo recordableInfo, int frame)
        {
            m_Keys.Add(frame);
            this.recordableInfo = recordableInfo;
        }

        public void DrawOnTrack(Track track)
        {
            var o = EditorUtility.InstanceIDToObject(recordableInfo.instanceID);
            track.Q<Label>().text = o.name;
            track.KeyContainer.keys = m_Keys;
        }

        public void Refresh(int frame)
        {
            var tr = recordableInfo.recordable as TransformRecordable;
            if (tr != null)
            {
                var lastRecords = GameDebuggerDatabase.GetRecords(m_Keys.Last());
                var info = lastRecords.Find(otherRecInfo => recordableInfo.instanceID == otherRecInfo.instanceID);
                if (!tr.ApproximatelyEquals((TransformRecordable)info.recordable))
                    m_Keys.Add(frame);
            }
        }
    }
}
