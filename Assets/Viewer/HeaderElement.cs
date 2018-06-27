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
            if (GameDebuggerRecorder.isPlaying)
            {
                if (GameDebuggerRecorder.isPaused)
                    GameDebuggerRecorder.StartReplay();
                else
                    GameDebuggerRecorder.PauseReplay();
            }
            else
                GameDebuggerRecorder.StartReplay();
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