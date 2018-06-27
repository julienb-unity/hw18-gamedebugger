﻿using System;
using System.Collections.Generic;
using System.Linq;
using Recordables;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class RigidBodyRecordableItem : ITrackItem
    {
        int m_InstanceId;
        List<int> m_FrameIds;
        List<float> m_Velocities;

        readonly Color m_BackgroundColor = Color.Lerp(Color.black, Color.white, 0.7f);
        readonly Color m_LowVelocityColor = Color.blue;
        readonly Color m_HighVelocityColor = Color.red;
        readonly Color m_LineColor = Color.Lerp(Color.black, Color.white, 0.5f);

        public RigidBodyRecordableItem(RecordableInfo recordableInfo, int frame)
        {
            m_FrameIds = new List<int>(200) { frame };
            var rr = (RigidBodyRecordable)recordableInfo.recordable;
            m_Velocities = new List<float>(200) { rr.speed.magnitude };
            m_InstanceId = recordableInfo.instanceID;
        }

        public void Draw(Track track, ITimeConverter converter)
        {
            var o = EditorUtility.InstanceIDToObject(m_InstanceId);
            track.Q<Label>().text = o.name;

            DrawBackground(track);
            DrawKeys(converter);
        }

        public void Refresh(RecordableInfo recordableInfo, int frame)
        {
            var rr = recordableInfo.recordable as RigidBodyRecordable;
            if (rr != null)
            {
                var lastRecords = GameDebuggerDatabase.GetRecords(m_FrameIds.Last());
                var info = lastRecords.records.Find(otherRecInfo => m_InstanceId == otherRecInfo.instanceID);
                if (!rr.ApproximatelyEquals((RigidBodyRecordable) info.recordable))
                {
                    m_FrameIds.Add(frame);
                    m_Velocities.Add(rr.speed.magnitude);
                }
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

        void DrawKeys(ITimeConverter converter)
        {
            if (m_FrameIds.Count == 0)
                return;

            // Get max velocity.
            var maxVelocity = m_Velocities.Max(v => v);

            var prevFrameTime = GameDebuggerDatabase.GetRecords(m_FrameIds[0]).time;
            var prevFrameTimeXPos = converter.TimeToPixel(prevFrameTime);
            for (var f = 1; f < m_FrameIds.Count; ++f)
            {
                var frameId = m_FrameIds[f];
                var frameTime = GameDebuggerDatabase.GetRecords(frameId).time;
                var velocity = m_Velocities[f];
                var frameTimeXPos = converter.TimeToPixel(frameTime);
                EditorGUI.DrawRect(new Rect(prevFrameTimeXPos, 10, frameTimeXPos - prevFrameTimeXPos, 30), Color.Lerp(m_LowVelocityColor, m_HighVelocityColor, velocity / maxVelocity));

                prevFrameTimeXPos = frameTimeXPos;
            }
        }
    }
}