using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class TimeAreaResizeManipulator : Manipulator
    {
        PlayheadElement m_Playhead;

        public TimeAreaResizeManipulator(PlayheadElement playhead)
        {
            m_Playhead = playhead;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        void OnGeometryChanged(GeometryChangedEvent evt)
        {
            m_Playhead.RefreshPlayheadPosition();
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }
    }
}