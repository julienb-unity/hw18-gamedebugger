using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class TimeAreaCallbackManipulator : Manipulator
    {
        PlayheadElement m_Playhead;

        public TimeAreaCallbackManipulator(TimeAreaGUI timeArea, PlayheadElement playhead)
        {
            m_Playhead = playhead;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<WheelEvent>(OnWheelEvent);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<WheelEvent>(OnWheelEvent);
        }
        
        void OnMouseMove(MouseMoveEvent evt)
        {
            OnRefresh();
        }
        
        void OnWheelEvent(WheelEvent evt)
        {
            OnRefresh();
        }

        void OnRefresh()
        {
            m_Playhead.RefreshPlayheadPosition();
        }
    }
}