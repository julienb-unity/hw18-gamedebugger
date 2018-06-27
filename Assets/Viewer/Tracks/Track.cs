using System;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger 
{
    class Track : VisualElement
    {
        readonly ITimeConverter m_TimeConverter;

        public ITrackItem item { get; set; }
        
        public Track(VisualTreeAsset trackTemplate, ITimeConverter timeConverter)
        {
            m_TimeConverter = timeConverter;
            trackTemplate.CloneTree(this, null);
        }

        public override void DoRepaint()
        {
            if (item != null)
                item.Draw(this, m_TimeConverter);
        }
    }
}