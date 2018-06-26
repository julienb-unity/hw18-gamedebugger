using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TimelineElement : VisualElement
    {
        public TimelineElement(TimeManager timeMgr, RefreshScheduler scheduler)
        {
            name = "timeline";
            AddStyleSheetPath("Stylesheets/Styles");

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
            var playhead = new PlayheadElement(timeMgr, timeProvider);
            playhead.name = "playhead";
            Add(playhead);

            imguiContainer.AddManipulator(new PlayheadDragManipulator(playhead));
            playhead.AddManipulator(new PlayheadDragManipulator(playhead));

            var trackContainer = new TrackContainer(timeProvider, scheduler);
            Add(trackContainer);

            //playhead needs to be on top of the tracks
            playhead.BringToFront();
        }
    }
}
