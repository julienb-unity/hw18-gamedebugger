using System.Collections.Generic;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    public class GameDebuggerTracks : VisualElement
    {
        List<GameDebuggerTrack> tracks = new List<GameDebuggerTrack>();

        public GameDebuggerTracks()
        {
            name = "header";
            AddToClassList("container");
            AddStyleSheetPath("Stylesheets/Styles");

            Add(new GameDebuggerTrack("Track #1"));
            Add(new GameDebuggerTrack("Track #2"));
        }
    }
}