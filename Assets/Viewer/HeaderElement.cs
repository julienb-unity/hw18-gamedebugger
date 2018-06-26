using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    public class HeaderElement : VisualElement
    {
        public HeaderElement()
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
            if (GameDebuggerRecorder.isRecording)
                return;
            if (GameDebuggerPlayer.isPlaying)
                GameDebuggerPlayer.StopReplay();
            else
                GameDebuggerPlayer.StartReplay();
        }

        void OnRecord()
        {
            if (GameDebuggerRecorder.isRecording)
                GameDebuggerRecorder.StopRecording();
            else
                GameDebuggerRecorder.StartRecording();
            
        }
        
    }
}