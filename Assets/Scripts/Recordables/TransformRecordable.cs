using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Recordables
{
    public class TransformRecordable : Recordable<Transform>
    {
        [SerializeField]
        private Vector3 position;    
        [SerializeField]
        private Quaternion rotation;    
        [SerializeField]
        private Vector3 scale;    

        public override bool OnRecord(Recordable previous, Object source)
        {
            var t = source as Transform;
            if (t.gameObject.isStatic)
                return false;
            
            var prev = previous as TransformRecordable;

            if (prev != null && prev.position == t.position && prev.rotation == t.rotation && prev.scale == t.localScale)
                return false;
            
            position = t.position;
            rotation = t.rotation;
            scale = t.localScale;

            return true;
        }

        public override void OnReplay(Object source)
        {
            var component = source as Component;
            component.gameObject.transform.position = position;
            component.gameObject.transform.rotation = rotation;
            component.gameObject.transform.localScale = scale;
        }

        public bool ApproximatelyEquals(TransformRecordable other)
        {
            return Vector3.Distance(position, other.position) <= 0.01f;
        }
    }
}


