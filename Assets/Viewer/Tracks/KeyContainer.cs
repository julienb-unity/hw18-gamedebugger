using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class KeyContainer : VisualElement
    {
        public List<int> keys;

        readonly ITimeConverter m_TimeConverter;
        readonly Color m_KeyColor = Color.Lerp(Color.black, Color.white, 0.2f);

        public KeyContainer(ITimeConverter timeConverter)
        {
            m_TimeConverter = timeConverter;
        }

        public override void DoRepaint()
        {
            foreach (var key in keys)
            {
                var x = m_TimeConverter.TimeToPixel(key);
                EditorGUI.DrawRect(new Rect(x - 3, 22, 6, 6), m_KeyColor);
            }
        }
    }
}