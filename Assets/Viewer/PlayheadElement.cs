using System;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger {
    class PlayheadElement : VisualElement
    {
        TimeManager m_TimeManager;
        TimeAreaGUI m_TimeAreaGUI;
        
        public PlayheadElement(TimeManager timeMgr, TimeAreaGUI timeArea)
        {
            m_TimeManager = timeMgr;
            m_TimeAreaGUI = timeArea;
            
            var playheadCursor = new Box();
            playheadCursor.name = "playheadCursor";

            var playheadLine = new Box();
            playheadLine.name = "playheadLine";
            
            Add(playheadLine);
            Add(playheadCursor);
        }

        public void SetTimeFromPixel(float pixel)
        {
            style.positionLeft = pixel - contentRect.width/2.0f;
            m_TimeManager.time = m_TimeAreaGUI.PixelToTime(pixel);
        }
    }
}