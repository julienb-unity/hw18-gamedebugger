using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class PlayheadDragManipulator : MouseManipulator
    {
        bool m_Active;

        PlayheadElement m_Playhead;

        public PlayheadDragManipulator(PlayheadElement playhead)
        {
            m_Playhead = playhead;
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            m_Active = false;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        void OnMouseDown(MouseDownEvent e)
        {
            if (m_Active)
            {
                e.StopImmediatePropagation();
                return;
            }

            if (CanStartManipulation(e))
            {
                m_Active = true;

                target.TakeMouseCapture();
                SetTimeSnappedToFrameFromPixel(e.mousePosition.x);
                
                e.StopPropagation();
            }
        }

        void OnMouseMove(MouseMoveEvent e)
        {
            if (m_Active)
            {
                SetTimeSnappedToFrameFromPixel(e.mousePosition.x);
                e.StopPropagation();
            }
        }

        void OnMouseUp(MouseUpEvent e)
        {
            if (m_Active)
            {
                if (CanStopManipulation(e))
                {
                    m_Active = false;
                    target.ReleaseMouseCapture();
                    e.StopPropagation();
                }
            }
        }

        void SetTime(float time)
        {
            m_Playhead.SetTime(time);
        }

        void SetTimeSnappedToFrameFromPixel(float pixel)
        {
            var time = GetTimeForPixel(pixel);
            if (time < 0)
                return;
            SetTime(time);
        }

        float GetTimeForPixel(float pixel)
        {
            return m_Playhead.GetTimeForPixel(pixel);
        }
    }
}
