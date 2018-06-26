using UnityEngine;

namespace Recordables
{
    public class GameObjectRecordable  : Recordable<GameObject>
    {
        [SerializeField] private bool isActive;
    
        public override void OnRecord(Object source)
        {
            var go = source as GameObject;
            isActive = go.activeSelf;
        }

        public override void OnReplay(Object source)
        {
            var go = source as GameObject;
            go.SetActive(isActive);
        }
    }
}
