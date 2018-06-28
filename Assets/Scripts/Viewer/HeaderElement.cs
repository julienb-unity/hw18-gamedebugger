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
            
            var toggle = new Toggle(OnToggle)
            {
                name = "recordOnPlay",
                text = "Record on play"
            };
            toggle.value = state.recordOnPlay;
            EditorApplication.playModeStateChanged += RecordOnPlay;
            Add(toggle);

            Add(new Button(OnRecord)
            {
                name = "recordButton",
                text = "Record"
            });

            Add(new Button(OnPlay)
            {
                name = "playButton",
                text = "Play"
            });

            Add(new Button(OnSaveRecording)
            {
                name = "saveRecordingButton",
                text = "Save recording"
            });

            Add(new Button(OnLoadSavedRecording)
            {
                name = "loadLastRecordingButton",
                text = "Load from file"
            });
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
            if (GameDebuggerRecorder.IsRecording)
                GameDebuggerRecorder.StopRecording();
            else
                GameDebuggerRecorder.StartRecording();
        }
        
        static void OnLoadSavedRecording()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            bool success = GameDebuggerSerializer.LoadDataFromFile();
            if (!success)
            {
                Debug.LogError("Cant load replay data from file");
            }
        }
        
        static void OnSaveRecording()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            GameDebuggerSerializer.DumpToFile();
        }
    }
}