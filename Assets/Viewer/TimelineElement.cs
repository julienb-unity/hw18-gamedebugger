using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TimelineElement : VisualElement
    {
        TimeAreaGUI m_TimeAreaGUI = new TimeAreaGUI();

        public TimelineElement(TimeManager timeMgr, RefreshScheduler scheduler)
        {
            name = "timeline";
            AddStyleSheetPath("Stylesheets/Styles");
            
            var timeArea = new VisualElement();
            timeArea.name = "timeArea";
            Add(timeArea);
            
            var imguiContainer = new IMGUIContainer(() =>
            {
                m_TimeAreaGUI.OnGUI(timeArea.layout);
            });
            imguiContainer.name = "timeAreaGUI";
            timeArea.Add(imguiContainer);
            imguiContainer.StretchToParentSize();
            
            var playhead = new PlayheadElement(timeMgr, m_TimeAreaGUI);
            playhead.name = "playhead";
            Add(playhead);
            
            imguiContainer.AddManipulator(new PlayheadDragManipulator(playhead));
            playhead.AddManipulator(new PlayheadDragManipulator(playhead));
            
            var trackContainer = new TrackContainer(scheduler);
            Add(trackContainer);
            
            //playhead needs to be on top of the tracks
            playhead.BringToFront();
        }
    }
}
