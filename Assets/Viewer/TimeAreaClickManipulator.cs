using System;
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
            target.RegisterCallback<MouseDownEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseUp);
        }

        void OnMouseUp(MouseDownEvent evt)
        {
            m_Playhead.SetTimeFromPixel(evt.mousePosition.x);
        }
    }
}