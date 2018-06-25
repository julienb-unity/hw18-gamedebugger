﻿using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace GameDebugger
{
    class TimeDragManipulator : MouseManipulator
    {
        Vector2 m_Start;
        bool m_Active;

        PlayheadElement m_Playhead;

        public TimeDragManipulator(PlayheadElement playhead)
        {
            m_Playhead = playhead;
            activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
            m_Active = false;
        }

        static Rect CalculatePosition(float x, float y, float width, float height)
        {
            return new Rect(x, y, width, height);
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
                m_Start = e.localMousePosition;

                m_Active = true;

                target.TakeMouseCapture();
                e.StopPropagation();
            }
        }

        void OnMouseMove(MouseMoveEvent e)
        {
            if (m_Active)
            {
                var diff = e.localMousePosition - m_Start;
                SetTimeForPixel(target.layout.x + diff.x);
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

        void SetTimeForPixel(float pixel)
        {
            m_Playhead.SetTimeFromPixel(pixel);
        }
    }
}