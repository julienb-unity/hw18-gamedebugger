using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class PlayheadElement : VisualElement
    {
        float m_Time;
        ITimeConverter m_TimeConverter;
        
        public PlayheadElement(ITimeConverter timeConverter)
        {
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
            m_Time = time;
            RefreshPlayheadPosition();
            GameDebuggerRecorder.ReplayTime(time);
        }

        public void RefreshPlayheadPosition()
        {
            var pixel = m_TimeConverter.TimeToPixel(m_Time);
            if (pixel > parent.style.positionLeft)
            style.positionLeft = pixel - contentRect.width/2.0f;

            if (pixel < 150) //ugly hack, sorry
                style.opacity = 0;
            else
            {
                style.opacity = 100;
            }
        }

        public float GetTimeForPixel(float pixel)
        {
            return m_TimeConverter.PixelToTime(pixel);
        }
    }
}