﻿using System;
using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class RigidBodyRecordableItem : TrackItem
    {
        List<int> m_FrameIds;
        List<float> m_Velocities;

        readonly Color m_LowVelocityColor = Color.blue;
        readonly Color m_HighVelocityColor = Color.red;

        public RigidBodyRecordableItem(RecordableInfo recordableInfo, int frame) : base(recordableInfo.instanceID)
        {
            m_FrameIds = new List<int>(200) { frame };
            var rr = (RigidBodyRecordable)recordableInfo.recordable;
            m_Velocities = new List<float>(200) { rr.speed.magnitude };
        }

        protected override void DrawItem(Track track, ITimeConverter converter)
        {
            DrawKeys(track, converter);
        }

        public override void Refresh(RecordableInfo recordableInfo, int frame)
        {
            var rr = recordableInfo.recordable as RigidBodyRecordable;
            if (rr != null)
            {
                var lastRecords = GameDebuggerDatabase.GetRecords(m_FrameIds.Last());
                var info = lastRecords.records.Find(otherRecInfo => m_InstanceId == otherRecInfo.instanceID);
                if (info.recordable != null && !rr.ApproximatelyEquals((RigidBodyRecordable) info.recordable))
                {
                    m_FrameIds.Add(frame);
                    m_Velocities.Add(rr.speed.magnitude);
                }
            }
        }

        public override void OnClick(VisualElement panel, float time)
        {
            var frameInfo = GameDebuggerDatabase.FrameRecords.LastOrDefault(i => i.time < time && i.records.Find(j => j.instanceID == m_InstanceId) != null);
            if (frameInfo != null)
            {
                var recorderInfo = frameInfo.records.Find(i => i.instanceID == m_InstanceId);
                DrawRecorderInfo(panel, recorderInfo.recordable);
            }
        }

        static void DrawRecorderInfo(VisualElement panel, Recordable recorder)
        {
            var rr = recorder as RigidBodyRecordable;
            if (rr != null)
            {
                var text = string.Format(
                    "Speed:\n    {3:F2} m/s\n\nVelocity:\n    x: {0:F2}\n    y: {1:F2}\n    z: {2:F2}",
                    rr.speed.magnitude,
                    rr.speed.x, rr.speed.y, rr.speed.z);
                panel.Add(new Label(text));
            }
        }

        void DrawKeys(Track track, ITimeConverter converter)
        {
            if (m_FrameIds.Count == 0)
                return;

            // Get max velocity.
            var maxVelocity = Mathf.Max(m_Velocities.Max(v => v), 0.1f);

            var height = track.contentRect.height;

            var prevVelocity = m_Velocities[0];
            var prevVelocityHeight = 5.0f + (prevVelocity / maxVelocity) * 25.0f;
            var prevFrameTime = GameDebuggerDatabase.GetRecords(m_FrameIds[0]).time;
            var prevFrameTimeXPos = converter.TimeToPixel(prevFrameTime);
            var prevFrameTimeYPos = (height - prevVelocityHeight) / 2.0f;
            var prevColor = Color.Lerp(m_LowVelocityColor, m_HighVelocityColor, prevVelocity / maxVelocity);
            for (var f = 1; f < m_FrameIds.Count; ++f)
            {
                var frameId = m_FrameIds[f];
                var frameTime = GameDebuggerDatabase.GetRecords(frameId).time;
                var frameTimeXPos = converter.TimeToPixel(frameTime);
                EditorGUI.DrawRect(new Rect(prevFrameTimeXPos, prevFrameTimeYPos, frameTimeXPos - prevFrameTimeXPos, prevVelocityHeight), prevColor);

                prevVelocity = m_Velocities[f];
                prevVelocityHeight = 5.0f + (prevVelocity / maxVelocity) * 25.0f;
                prevFrameTimeXPos = frameTimeXPos;
                prevFrameTimeYPos = (height - prevVelocityHeight) / 2.0f;
                prevColor = Color.Lerp(m_LowVelocityColor, m_HighVelocityColor, prevVelocity / maxVelocity);
            }

            var currentTimeXPos = converter.TimeToPixel(Time.unscaledTime - GameDebuggerDatabase.StartRecordingTime);
            EditorGUI.DrawRect(new Rect(prevFrameTimeXPos, prevFrameTimeYPos, currentTimeXPos - prevFrameTimeXPos, prevVelocityHeight), prevColor);
        }
    }
}
