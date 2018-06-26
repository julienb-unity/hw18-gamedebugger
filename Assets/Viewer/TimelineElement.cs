using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TimelineElement : VisualElement
    {
        TimeAreaGUI m_TimeAreaGUI = new TimeAreaGUI();
        
        public TimelineElement(TimeManager timeMgr)
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
            timeArea.Add(imguiContainer);
            imguiContainer.StretchToParentSize();
            
            var tracks = new GameDebuggerTracks();
            tracks.name = "tracks";
            Add(tracks);
            
            //the playhead needs to be added at the very end, since it needs to be drawn on top of the tracks
            var playhead = new PlayheadElement(timeMgr, m_TimeAreaGUI);
            playhead.name = "playhead";
            Add(playhead);
            
            imguiContainer.AddManipulator(new PlayheadDragManipulator(playhead));
            playhead.AddManipulator(new PlayheadDragManipulator(playhead));
        }
    }
}
