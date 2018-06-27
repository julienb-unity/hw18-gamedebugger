using System.Linq;
using UnityEngine;

namespace GameDebugger
{
    class ViewState : ScriptableObject
    {
        public bool recordOnPlay;
        
        static ViewState s_ViewState;
        
        public static ViewState Get()
        {
            var previousObj = Resources.FindObjectsOfTypeAll(typeof(ViewState)).First();
            
            if (previousObj == null)
            {
                s_ViewState = CreateViewState();
            }

            s_ViewState = (ViewState)previousObj;
            return s_ViewState;
        }
        
        static ViewState CreateViewState()
        {
            var state = ScriptableObject.CreateInstance<ViewState>();
            state.hideFlags = HideFlags.HideAndDontSave;
            return state;
        }
    }
}
