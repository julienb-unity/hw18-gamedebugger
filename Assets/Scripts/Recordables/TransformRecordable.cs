using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Recordables
{
    [Serializable]
    public class TransformRecordable : Recordable<Transform>
    {
        [SerializeField]
        public Vector3 localPosition;
        [SerializeField]
        public Quaternion localRotation;
        [SerializeField]
        public Vector3 localScale;

        public override bool OnRecord(Recordable previous, Object source)
        {
            var t = source as Transform;
            if (t.gameObject.isStatic)
                return false;
            
            var prev = previous as TransformRecordable;

            if (prev != null && prev.localPosition == t.position && prev.localRotation == t.rotation && prev.localScale == t.localScale)
                return false;
            
            localPosition = t.localPosition;
            localRotation = t.localRotation;
            localScale = t.localScale;

            return true;
        }

        public override void OnReplay(Object source)
        {
            var component = source as Component;
            component.gameObject.transform.localPosition = localPosition;
            component.gameObject.transform.localRotation = localRotation;
            component.gameObject.transform.localScale = localScale;
        }

        public bool ApproximatelyEquals(TransformRecordable other)
        {
            return Vector3.Distance(localPosition, other.localPosition) <= 0.01f;
        }
    }
}


