using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace GameDebugger
{
    interface ITrackItem
    {
        void Draw(Track track, ITimeConverter converter);
        void Refresh(RecordableInfo recordableInfo, int frame);
        void OnClick(VisualElement panel, float time);
    }
}
