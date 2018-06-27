using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class Track : VisualElement
    {
        public KeyContainer KeyContainer;

        readonly ITimeConverter m_TimeConverter;
        readonly Color m_LineColor = Color.Lerp(Color.black, Color.white, 0.5f);

        public Track(VisualTreeAsset trackTemplate, ITimeConverter timeConverter)
        {
            m_TimeConverter = timeConverter;

            trackTemplate.CloneTree(this, null);

            KeyContainer = new KeyContainer(m_TimeConverter);
            this.Q(className: "track").Add(KeyContainer);
        }

        public override void DoRepaint()
        {
            if (KeyContainer.keys.Count == 0)
                return;
            var x = m_TimeConverter.TimeToPixel(KeyContainer.keys.First());
            var w = contentRect.width;
            EditorGUI.DrawRect(new Rect(x, 24, w, 1), m_LineColor);
        }
    }
}