using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        readonly Color m_BackgroundColor = Color.Lerp(Color.black, Color.white, 0.7f);
        readonly Color m_KeyColor = Color.Lerp(Color.black, Color.white, 0.2f);
        readonly Color m_LineColor = Color.Lerp(Color.black, Color.white, 0.5f);

        public TransformRecordableItem(RecordableInfo recordableInfo, int frame)
        {
            m_FrameIds = new List<int>(200){frame};
            m_InstanceId = recordableInfo.instanceID;
        }

        public void Draw(Track track, ITimeConverter converter)
        {
            var o = EditorUtility.InstanceIDToObject(m_InstanceId);
            track.Q<Label>().text = o.name;
            
            DrawBackground(track);
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

        public void OnClick(VisualElement panel, float time)
        {
            var frameInfo = GameDebuggerDatabase.FrameRecords.FindLast(i => i.time < time && i.records.Find(j => j.instanceID == m_InstanceId) != null);
            var recorderInfo = frameInfo.records.Find(i => i.instanceID == m_InstanceId);
            DrawRecorderInfo(panel, recorderInfo.recordable);
        }

        static void DrawRecorderInfo(VisualElement panel, Recordable recorder)
        {
            var transformRecordable = recorder as TransformRecordable;
            if (transformRecordable!= null)
                panel.Add(new Label(TransformToString(transformRecordable)));
        }

        static string TransformToString(TransformRecordable rec)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Transform: ");
            stringBuilder.Append("Position ").Append(rec.localPosition).AppendLine();
            stringBuilder.Append("Rotation ").Append(rec.localRotation).AppendLine();
            stringBuilder.Append("Scale ").Append(rec.localScale).AppendLine();
            return stringBuilder.ToString();
        }

        void DrawBackground(Track track)
        {
            var keycontainerRect = track.contentRect;
            keycontainerRect.x += 145;
            keycontainerRect.width -= 145;
            keycontainerRect.height -= 5;
            EditorGUI.DrawRect(keycontainerRect, m_BackgroundColor);
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
            var x = converter.TimeToPixel(0);
            var w = converter.TimeToPixel(Time.unscaledTime - GameDebuggerDatabase.StartRecordingTime) - x;
            EditorGUI.DrawRect(new Rect(x, 24, w, 1), m_LineColor);
        }
    }
}
