using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class TimeAreaClickManipulator : Manipulator
    {
        PlayheadElement m_Playhead;
        
        public TimeAreaClickManipulator(PlayheadElement playhead)
        {
            m_Playhead = playhead;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        void OnMouseDown(MouseDownEvent evt)
        {
            m_Playhead.SetTimeFromPixel(evt.mousePosition.x);
        }
    }
}