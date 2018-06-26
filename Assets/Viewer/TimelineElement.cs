using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

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
            
            var playhead = new PlayheadElement(timeMgr, m_TimeAreaGUI);
            playhead.name = "playhead";
            timeArea.Add(playhead);
            
            imguiContainer.AddManipulator(new PlayheadDragManipulator(playhead));
            playhead.AddManipulator(new PlayheadDragManipulator(playhead));

            var tracks = new GameDebuggerTracks();
            tracks.name = "tracks";
            Add(tracks);
        }
    }
}
