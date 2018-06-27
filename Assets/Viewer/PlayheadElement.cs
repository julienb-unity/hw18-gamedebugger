using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class PlayheadElement : VisualElement
    {
        TimeManager m_TimeManager;
        ITimeConverter m_TimeConverter;
        
        public PlayheadElement(TimeManager timeMgr, ITimeConverter timeConverter)
        {
            m_TimeManager = timeMgr;
            m_TimeConverter = timeConverter;
            
            var playheadCursor = new Box();
            playheadCursor.name = "playheadCursor";

            var playheadLine = new Box();
            playheadLine.name = "playheadLine";
            
            Add(playheadLine);
            Add(playheadCursor);
        }

        public void SetTime(float time)
        {
            var pixel = m_TimeConverter.TimeToPixel(time);
            style.positionLeft = pixel - contentRect.width/2.0f;
            m_TimeManager.time = (int)Mathf.Floor(time);
            
            GameDebuggerRecorder.ReplayFrame(m_TimeManager.time);
        }

        public float GetTimeForPixel(float pixel)
        {
            return m_TimeConverter.PixelToTime(pixel);
        }
    }
}