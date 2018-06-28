using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    class TrackContentManipulator : Manipulator
    {
        ITimeConverter m_TimeConverter;
        VisualElement m_ExtraViewPanel;

        public TrackContentManipulator(ITimeConverter converter, VisualElement extraViewPanel)
        {
            m_TimeConverter = converter;
            m_ExtraViewPanel = extraViewPanel;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            var track = target as Track;
            if (track != null && track.item != null)
            {
                m_ExtraViewPanel.Clear();
                track.item.OnClick(m_ExtraViewPanel, m_TimeConverter.PixelToTime(evt.mousePosition.x));
            }
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }
    }
}
