﻿using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TimelineElement : VisualElement
    {
        public TimelineElement(RefreshScheduler scheduler, VisualElement extraViewer)
        {
            name = "timeline";

            var timeArea = new VisualElement();
            timeArea.name = "timeArea";
            Add(timeArea);

            var timeAreaGUI = new TimeAreaGUI();
            var imguiContainer = new IMGUIContainer(() =>
            {
                timeAreaGUI.OnGUI(timeArea.layout);
            });
            imguiContainer.name = "timeAreaGUI";
            timeArea.Add(imguiContainer);
            imguiContainer.StretchToParentSize();
            
            var timeProvider = new TimeConverter(timeAreaGUI);
            var playhead = new PlayheadElement(timeProvider);
            playhead.name = "playhead";
            Add(playhead);

            imguiContainer.AddManipulator(new PlayheadDragManipulator(playhead));
            playhead.AddManipulator(new PlayheadDragManipulator(playhead));
            imguiContainer.AddManipulator(new TimeAreaCallbackManipulator(timeAreaGUI, playhead));
            imguiContainer.AddManipulator(new TimeAreaResizeManipulator(playhead));

            var trackContainer = new TrackContainer(timeProvider, scheduler, extraViewer);
            Add(trackContainer);

            //playhead needs to be on top of the tracks
            playhead.BringToFront();

            scheduler.Refresh += () =>
            {
                if (!GameDebuggerRecorder.isPlaying)
                    return;
                var frame = GameDebuggerRecorder.currentFrame;
                var time = GameDebuggerDatabase.GetRecords(frame).time;
                playhead.SetTime(time);
                
            };
        }
    }
}
