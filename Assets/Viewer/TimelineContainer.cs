using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace GameDebugger
{
    class TimelineContainer : VisualContainer
    {
        TimeAreaGUI m_TimeAreaGUI = new TimeAreaGUI();
        
        public TimelineContainer(TimeManager timeMgr)
        {
            name = "timeline";
            AddStyleSheetPath("Stylesheets/Styles");
            
            var timeArea = new VisualContainer();
            timeArea.name = "timeArea";
            Add(timeArea);
            
            var imguiContainer = new IMGUIContainer(() =>
            {
                m_TimeAreaGUI.OnGUI(contentRect);
            });
            timeArea.Add(imguiContainer);
            imguiContainer.StretchToParentSize();
            
            var playhead = new PlayheadElement(timeMgr, m_TimeAreaGUI);
            playhead.name = "playhead";
            timeArea.Add(playhead);
            
            imguiContainer.AddManipulator(new TimeAreaClickManipulator(playhead));
            playhead.AddManipulator(new TimeDragManipulator(playhead));
        }
    }
}
