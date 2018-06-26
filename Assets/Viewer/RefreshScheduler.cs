using System;
using UnityEditor;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class RefreshScheduler
    {
        IVisualElementScheduledItem m_Scheduler;

        public event Action Refresh;
        public event Action ExitPlayMode;

        public RefreshScheduler(IVisualElementScheduler scheduler)
        {
            m_Scheduler = scheduler.Execute(InvokeRefresh).Every(100);
            m_Scheduler.Pause();
            
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    m_Scheduler.Resume();
                }
                else if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    m_Scheduler.Pause();
                    InvokeExitPlayMode();
                }
            };
        }

        void InvokeRefresh()
        {
            if (Refresh != null) Refresh.Invoke();
        }

        void InvokeExitPlayMode()
        {
            if (ExitPlayMode != null) ExitPlayMode.Invoke();
        }
    }
}