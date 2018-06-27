using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class AnimatorRecordableItem :ITrackItem
    {
        int m_InstanceId;

        struct LayerInfo
        {
            public float time;
            public string name;
        }
        
        readonly Color m_BackgroundColor = Color.Lerp(Color.black, Color.white, 0.7f);
        List<LayerInfo> m_LayerNames = new List<LayerInfo>(5);

        public AnimatorRecordableItem(RecordableInfo recordableInfo, int frame)
        {
            m_InstanceId = recordableInfo.instanceID;
            m_LayerNames.Add(new LayerInfo
            {
                time= GameDebuggerDatabase.GetRecords(frame).time, 
                name = GetStateNameForRecordable(recordableInfo.recordable)
            });
        }

        static string GetStateNameForRecordable(Recordable rec)
        {
            var animRec = rec as AnimatorRecordable;
            if (animRec == null) return string.Empty;
            var name = animRec.layerNames.FirstOrDefault();
            return name == null? string.Empty : name;
        }
        
        public void Draw(Track track, ITimeConverter converter)
        {
            var o = EditorUtility.InstanceIDToObject(m_InstanceId);
            track.Q<Label>().text = o.name;
            
            DrawBackground(track);
            float lastTime = 0;
            var count = m_LayerNames.Count;
            for (int i = 0; i < count; i++)
            {
                var leftTime = converter.TimeToPixel(m_LayerNames[i].time);
                var rightTime = i+1 == count ? track.contentRect.xMax + 10 : converter.TimeToPixel(m_LayerNames[i + 1].time);
                var rect = track.contentRect;
                rect.xMin = leftTime;
                rect.xMax = rightTime;
                GUI.Box(rect, m_LayerNames[i].name, EditorStyles.helpBox);
            }
        }
        
        void DrawBackground(Track track)
        {
            var keycontainerRect = track.contentRect;
            keycontainerRect.x += 145;
            keycontainerRect.width -= 145;
            keycontainerRect.height -= 5;
            EditorGUI.DrawRect(keycontainerRect, m_BackgroundColor);
        }

        public void Refresh(RecordableInfo recordableInfo, int frame)
        {
            var ar = recordableInfo.recordable as AnimatorRecordable;
            if (ar != null)
            {
                var otherStateName = GetStateNameForRecordable(ar);
                var currentStateName = m_LayerNames.Last().name;
                if (otherStateName != currentStateName)
                    m_LayerNames.Add(new LayerInfo()
                    {
                        name = otherStateName,
                        time = GameDebuggerDatabase.GetRecords(frame).time
                    });
            }
        }
    }
}
