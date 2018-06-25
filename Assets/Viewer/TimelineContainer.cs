using System;
using System.Reflection;
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
            })
            {
            };
            imguiContainer.name = "timeArea";
            
            Add(imguiContainer);
        }
    }

    public class TimeAreaGUI
    {
        Type timeAreaType;
        object timeArea;

        MethodInfo beginView;
        MethodInfo endView;
        MethodInfo drawTimeRuler;

        public TimeAreaGUI()
        {
            var assembly = typeof(UnityEditor.Editor).Assembly;
            timeAreaType = assembly.GetType("UnityEditor.TimeArea");
            timeArea = Activator.CreateInstance(timeAreaType, new object[]{false});
            beginView = timeAreaType.GetMethod("BeginViewGUI");
            endView = timeAreaType.GetMethod("EndViewGUI");
            var signature = new[] {typeof (Rect), typeof (float)};
            drawTimeRuler = timeAreaType.GetMethod("TimeRuler", signature);
        }
        
        /*
        public void TimeRuler(Rect position, float frameRate)
        {
            TimeRuler(position, frameRate, true, false, 1f, TimeFormat.TimeFrame);
        }

        public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha)
        {
            TimeRuler(position, frameRate, labels, useEntireHeight, alpha, TimeFormat.TimeFrame);
        }

        public void TimeRuler(Rect position, float frameRate, bool labels, bool useEntireHeight, float alpha,
            TimeFormat timeFormat)
            */
        public void OnGUI(Rect rect)
        {
            beginView.Invoke(timeArea, null);
            drawTimeRuler.Invoke(timeArea, new object[] {rect, 30.0f});
            endView.Invoke(timeArea, null);
        }
    }
}
