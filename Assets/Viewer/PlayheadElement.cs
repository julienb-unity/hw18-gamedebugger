using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class PlayheadElement : VisualElement
    {
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
            var pixel = m_TimeConverter.TimeToPixel(time);
            style.positionLeft = pixel - contentRect.width/2.0f;
            
            GameDebuggerRecorder.ReplayFrame((int)time); //TODO DONT CAST AS INT!
        }

        public float GetTimeForPixel(float pixel)
        {
            return m_TimeConverter.PixelToTime(pixel);
        }
    }
}