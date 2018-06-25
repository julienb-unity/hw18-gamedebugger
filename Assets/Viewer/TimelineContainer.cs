using System;
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

            var imguiContainer = new IMGUIContainer(() =>
            {
                m_TimeAreaGUI.OnGUI(contentRect);
            });
            imguiContainer.style.height = 50;
            imguiContainer.name = "timeArea";
            
            Add(imguiContainer);
        }
    }
}
