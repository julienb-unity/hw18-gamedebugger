using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleSheets;

namespace GameDebugger
{
    abstract class TrackItem
    {
        readonly Color m_BackgroundColor = Color.Lerp(Color.black, Color.white, 0.7f);
        
        protected int m_InstanceId;

        public void Draw(Track track, ITimeConverter converter)
        {
            // Set track icon and name.
            track.Q<VisualElement>("trackIcon").style.backgroundImage = ItemImage();
            track.Q<Label>().text = ItemName();

            DrawBackground(track);
            DrawItem(track, converter);
        }
        
        protected TrackItem(int instanceId)
        {
            m_InstanceId = instanceId;
        }

        protected virtual Texture2D ItemImage()
        {
            var o = EditorUtility.InstanceIDToObject(m_InstanceId);
            return EditorGUIUtility.ObjectContent(null, o.GetType()).image as Texture2D;
        }

        protected virtual string ItemName()
        {
            var o = EditorUtility.InstanceIDToObject(m_InstanceId);
            if (o == null)
            {
                Debug.LogError("Cant find object instance " + m_InstanceId);
                return "undefined";
            }
            return o.name;
        }
        
        protected abstract void DrawItem(Track track, ITimeConverter converter);
        
        public abstract void Refresh(RecordableInfo recordableInfo, int frame);
        
        public virtual void OnClick(VisualElement panel, float time) {}

        protected virtual void DrawBackground(Track track)
        {
            var keycontainerRect = track.contentRect;
            keycontainerRect.x += 155;
            keycontainerRect.width -= 155;
            keycontainerRect.height -= 5;
            EditorGUI.DrawRect(keycontainerRect, m_BackgroundColor);
        }
    }
}
