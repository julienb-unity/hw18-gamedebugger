﻿using System;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger {
    class PlayheadElement : Box
    {
        TimeManager m_TimeManager;
        TimeAreaGUI m_TimeAreaGUI;
        
        public PlayheadElement(TimeManager timeMgr, TimeAreaGUI timeArea)
        {
            m_TimeManager = timeMgr;
            m_TimeAreaGUI = timeArea;
        }

        public void SetTimeFromPixel(float pixel)
        {
            style.positionLeft = pixel - contentRect.width/2.0f;
            m_TimeManager.time = m_TimeAreaGUI.PixelToTime(pixel);
        }
    }
}