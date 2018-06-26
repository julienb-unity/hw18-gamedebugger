using UnityEngine;

namespace Recordables
{
    public class GameObjectRecordable  : Recordable<GameObject>
    {
        [SerializeField] private bool isActive;
    
        public override bool OnRecord(Recordable previous, Object source)
        {
            var go = source as GameObject;
            var prev = previous as GameObjectRecordable;

            if (prev != null && prev.isActive == go.activeSelf)
                return false;
            
            isActive = go.activeSelf;
            return true;
        }

        public override void OnReplay(Object source)
        {
            var go = source as GameObject;
            go.SetActive(isActive);
        }
    }
}
