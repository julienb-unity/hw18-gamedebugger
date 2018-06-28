using System;
using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class AnimatorRecordableItem : TrackItem
    {
        struct LayerInfo
        {
            public float time;
            public string name;
        }
        
        List<LayerInfo> m_LayerNames = new List<LayerInfo>(5);

        public AnimatorRecordableItem(RecordableInfo recordableInfo, int frame) : base(recordableInfo.instanceID)
        {
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
        
        protected override void DrawItem(Track track, ITimeConverter converter)
        {
            float lastTime = 0;
            var count = m_LayerNames.Count;
            var oldColor = GUI.backgroundColor;
            for (int i = 0; i < count; i++)
            {
                var leftTime = converter.TimeToPixel(m_LayerNames[i].time);

                var nextTime = i + 1 == count
                    ? Time.unscaledTime - GameDebuggerDatabase.StartRecordingTime
                    : m_LayerNames[i + 1].time;

                var rightTime = converter.TimeToPixel(nextTime);
                var rect = track.contentRect;
                rect.xMin = leftTime;
                rect.xMax = rightTime;
                rect.yMax -= 5;
                GUI.backgroundColor = StringToColor(m_LayerNames[i].name);
                GUI.Box(rect, m_LayerNames[i].name, EditorStyles.helpBox);
            }

            GUI.backgroundColor = oldColor;
        }

        public override void Refresh(RecordableInfo recordableInfo, int frame)
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

        public override void OnClick(VisualElement panel, float time)
        {
            for(int i = m_LayerNames.Count - 1; i >= 0; i--)
            {
                if (time > m_LayerNames[i].time)
                {
                    DisplayInExtraPanel(panel, m_LayerNames[i].name);
                    return;
                }
            }
        }

        static void DisplayInExtraPanel(VisualElement panel, string labelName)
        {
            panel.Add(new Label(labelName));
        }

        static Color StringToColor(string str)
        {
            var hash = str.GetHashCode();
            var color =  new Color(((hash & 0xFF0000) >> 16)/255.0f, ((hash & 0x00FF00) >> 8)/255.0f, (hash & 0x0000FF)/255.0f);
            return color;
        }
    }
}
