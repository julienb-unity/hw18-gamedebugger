using System;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    public class TimelineContainer : VisualContainer
    {
        TimeAreaGUI m_TimeAreaGUI = new TimeAreaGUI();
        
        public TimelineContainer()
        {
            name = "timeline";
            AddToClassList("container");
            AddStyleSheetPath("Stylesheets/Styles");
            
            var container = new VisualContainer();
            container.name = "timeArea";
            Add(container);
            
            var imguiContainer = new IMGUIContainer(() =>
            {
                m_TimeAreaGUI.OnGUI(contentRect);
            });
            container.Add(imguiContainer);
            imguiContainer.StretchToParentSize();
            
            var slider = new VisualContainer();
            slider.name = "dragSlider";
            container.Add(slider);
            slider.StretchToParentWidth();
            
            var playhead = new Box();
            playhead.name = "playhead";
            playhead.AddManipulator(new HorizontalDragger());
            slider.Add(playhead);
            
            container.Add(slider);
            
        }
    }
}
