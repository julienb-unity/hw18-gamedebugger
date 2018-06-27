using System;
using System.Reflection;
using UnityEngine;

namespace GameDebugger 
{
    class TimeAreaGUI
    {
        object m_TimeArea;
        Rect m_LastRect;

        MethodInfo m_BeginView;
        MethodInfo m_EndView;
        MethodInfo m_DrawTimeRuler;
        PropertyInfo m_TimeAreaRect;
        MethodInfo m_TimeToPixel;
        MethodInfo m_PixelToTime;
        MethodInfo m_SetShownRange;
        PropertyInfo m_RangeLock;

        public TimeAreaGUI()
        {
            //Since the TimeArea is private, we need to get access to it by reflection, 
            var assembly = typeof(UnityEditor.Editor).Assembly;
            var timeAreaType = assembly.GetType("UnityEditor.TimeArea");
            m_TimeArea = Activator.CreateInstance(timeAreaType, new object[]{true, true, false});
            m_BeginView = timeAreaType.GetMethod("BeginViewGUI");
            m_EndView = timeAreaType.GetMethod("EndViewGUI");
            var signature = new[] {typeof (Rect), typeof (float)};
            m_DrawTimeRuler = timeAreaType.GetMethod("TimeRuler", signature);
            m_TimeAreaRect = timeAreaType.GetProperty("rect");
            var timeAreaHRangeMin = timeAreaType.GetProperty("hRangeMin");
            timeAreaHRangeMin.SetValue(m_TimeArea, 0.0f, null);
            m_TimeToPixel = timeAreaType.GetMethod("TimeToPixel", new[] {typeof(float), typeof(Rect)});
            m_PixelToTime = timeAreaType.GetMethod("PixelToTime", new[] {typeof(float), typeof(Rect)});
            m_SetShownRange = timeAreaType.GetMethod("SetShownHRange", new[]{typeof(float), typeof(float)});
            m_SetShownRange.Invoke(m_TimeArea, new object[] {0.0f, (float)1});
            m_RangeLock = timeAreaType.GetProperty("hRangeLocked");
        }

        public float TimeToPixel(float time)
        {
            return (float)m_TimeToPixel.Invoke(m_TimeArea, new object[]{time, m_LastRect});
        }

        public float PixelToTime(float pixel)
        {
            return (float)m_PixelToTime.Invoke(m_TimeArea, new object[]{ pixel, m_LastRect});
        }
        
        /*
        public void TimeRuler(Rect position, float frameRate)
        {
            TimeRuler(position, frameRate, true, false, 1f, TimeFormat.TimeFrame);
        }

        public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha)
        {
            TimeRuler(position, frameRate, labels, useEntireHeight, alpha, TimeFormat.TimeFrame);
        }

        public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha,
            TimeFormat timeFormat)
            */
        public void OnGUI(Rect rect)
        {
            if (float.IsNaN(rect.width) || float.IsNaN(rect.height))
                return;
            
            m_LastRect = rect;
            m_TimeAreaRect.SetValue(m_TimeArea, rect, null);
            m_BeginView.Invoke(m_TimeArea, null);
            m_DrawTimeRuler.Invoke(m_TimeArea, new object[] {rect, 60.0f});
            m_EndView.Invoke(m_TimeArea, null);

            if (GameDebuggerRecorder.IsRecording)
            {
                float maxTime = 0.0f;
                if (GameDebuggerDatabase.NumFrameRecords > 0)
                    maxTime = GameDebuggerDatabase.GetRecords(GameDebuggerDatabase.NumFrameRecords - 1).time;
                maxTime = Math.Max(5.0f, maxTime);
                m_SetShownRange.Invoke(m_TimeArea, new object[] { 0.0f, maxTime });
                m_RangeLock.SetValue(m_TimeArea, true, null);
            }
            else
            {
                m_RangeLock.SetValue(m_TimeArea, false, null);
            }
        }
    }
}