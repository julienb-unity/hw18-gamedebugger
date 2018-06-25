using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    public class HeaderContainer : VisualContainer
    {
        public HeaderContainer()
        {
            name = "header";
            AddToClassList("container");
            AddStyleSheetPath("Stylesheets/Styles");
            
            Add(new Button(OnPlay)
            {
                name = "playButton",
                text = "Play"
            });
            
            Add(new Button(OnRecord)
            {
                name = "recordButton",
                text = "Record"
            });
        }

        void OnPlay()
        {
            Debug.Log("Play");
        }

        void OnRecord()
        {
            if (GameDebuggerRecorder.Instance.isRecording)
                GameDebuggerRecorder.Instance.StopRecording();
            else
                GameDebuggerRecorder.Instance.StartRecording();
            
        }
    }
}