using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class HeaderElement : VisualElement
    {
        ViewState m_State;
        
        public HeaderElement(ViewState state)
        {
            m_State = state;
            
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
            toggle.value = state.recordOnPlay;
            EditorApplication.playModeStateChanged += RecordOnPlay;
            Add(toggle);
        }

        void OnToggle()
        {
            m_State.recordOnPlay = !m_State.recordOnPlay;
        }

        void RecordOnPlay(PlayModeStateChange state)
        {
            if (m_State.recordOnPlay && state == PlayModeStateChange.EnteredPlayMode)
                OnRecord();
        }

        static void OnPlay()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;
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
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            if (GameDebuggerRecorder.isRecording)
                GameDebuggerRecorder.StopRecording();
            else
                GameDebuggerRecorder.StartRecording();
        }
    }
}