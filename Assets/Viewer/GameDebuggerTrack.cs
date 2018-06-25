using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace GameDebugger
{
    public class GameDebuggerTrack : VisualContainer
    {
        public GameDebuggerTrack(string name)
        {
            style.flexDirection = FlexDirection.Row;
            Add(new Label(name));
            Add(new Button()
            {
                name = "playButton",
                text = "wuuut?"
            });
        }
    }
}