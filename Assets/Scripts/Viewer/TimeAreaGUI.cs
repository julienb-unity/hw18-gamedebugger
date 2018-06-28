using System;
using System.Reflection;
using UnityEngine;

namespace GameDebugger 
{
    class TimeAreaGUI
    {
        object m_TimeArea;
        Rect m_LastRect;

        PropertyInfo m_TimeAreaRect;
        PropertyInfo m_RangeLock;

        Func<float, Rect, float> m_TimeToPixelDel;
        Func<float, Rect, float> m_PixelToTimeDel;
        Action m_BeginViewDel;
        Action m_EndViewDel;
        Action<Rect, float> m_DrawDel;
        Action<float, float> m_SetShowRangeDel;
        
        public TimeAreaGUI()
        {
            //Since the TimeArea is private, we need to get access to it by reflection, 
            var assembly = typeof(UnityEditor.Editor).Assembly;
            var timeAreaType = assembly.GetType("UnityEditor.TimeArea");
            m_TimeArea = Activator.CreateInstance(timeAreaType, new object[]{true, true, false});
            var beginView = timeAreaType.GetMethod("BeginViewGUI");
            var endView = timeAreaType.GetMethod("EndViewGUI");
            var signature = new[] {typeof (Rect), typeof (float)};
            var drawTimeRuler = timeAreaType.GetMethod("TimeRuler", signature);
            m_TimeAreaRect = timeAreaType.GetProperty("rect");
            var timeAreaHRangeMin = timeAreaType.GetProperty("hRangeMin");
            timeAreaHRangeMin.SetValue(m_TimeArea, 0.0f, null);
            var timeToPixel = timeAreaType.GetMethod("TimeToPixel", new[] {typeof(float), typeof(Rect)});
            var pixelToTime = timeAreaType.GetMethod("PixelToTime", new[] {typeof(float), typeof(Rect)});
            var setShownRange = timeAreaType.GetMethod("SetShownHRange", new[]{typeof(float), typeof(float)});
            m_RangeLock = timeAreaType.GetProperty("hRangeLocked");

            m_PixelToTimeDel = (Func<float, Rect, float>) Delegate.CreateDelegate(typeof(Func<float, Rect, float>), m_TimeArea, pixelToTime);
            m_TimeToPixelDel = (Func<float, Rect, float>) Delegate.CreateDelegate(typeof(Func<float, Rect, float>), m_TimeArea, timeToPixel);
            m_BeginViewDel = (Action)Delegate.CreateDelegate(typeof(Action), m_TimeArea, beginView);
            m_EndViewDel = (Action)Delegate.CreateDelegate(typeof(Action), m_TimeArea, endView);
            m_DrawDel = (Action<Rect, float>)Delegate.CreateDelegate(typeof(Action<Rect, float>), m_TimeArea, drawTimeRuler);
            m_SetShowRangeDel = (Action<float, float>)Delegate.CreateDelegate(typeof(Action<float, float>), m_TimeArea, setShownRange);
            
            m_SetShowRangeDel(0.0f, 1);
        }
        
        public float TimeToPixel(float time)
        {
            return m_TimeToPixelDel(time, m_LastRect);
        }

        public float PixelToTime(float pixel)
        {
            return m_PixelToTimeDel(pixel, m_LastRect);
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
            m_BeginViewDel();
            m_DrawDel(rect, 60.0f);
            m_EndViewDel();

            if (GameDebuggerRecorder.IsRecording)
            {
                float maxTime = 0.0f;
                if (GameDebuggerDatabase.NumFrameRecords > 0)
                    maxTime = GameDebuggerDatabase.GetRecords(GameDebuggerDatabase.NumFrameRecords - 1).time;
                maxTime = Math.Max(5.0f, maxTime);
                m_SetShowRangeDel(maxTime -10.0f, maxTime);
                m_RangeLock.SetValue(m_TimeArea, true, null);
            }
            else
            {
                m_RangeLock.SetValue(m_TimeArea, false, null);
            }
        }
    }
}