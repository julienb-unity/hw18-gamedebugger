using UnityEditor;
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

            var toggle = new Toggle(OnToggle)
            {
                name = "recordOnPlay",
                text = "Record on play"
            };
            toggle.value = ViewState.Get().recordOnPlay;
            EditorApplication.playModeStateChanged += RecordOnPlay;
            Add(toggle);
        }

        static void OnToggle()
        {
            ViewState.Get().recordOnPlay = !ViewState.Get().recordOnPlay;
        }

        static void RecordOnPlay(PlayModeStateChange state)
        {
            if (ViewState.Get().recordOnPlay && state == PlayModeStateChange.EnteredPlayMode)
                OnRecord();
        }

        static void OnPlay()
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

        static void OnRecord()
        {
            if (GameDebuggerRecorder.isRecording)
                GameDebuggerRecorder.StopRecording();
            else
                GameDebuggerRecorder.StartRecording();
        }
    }
}