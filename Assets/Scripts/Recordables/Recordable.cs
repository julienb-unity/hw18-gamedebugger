using UnityEngine;

namespace Recordables
{
    public abstract class Recordable
    {
        public abstract bool OnRecord(Recordable previous, Object source);
        public abstract void OnReplay(Object source);
    }

    public abstract class Recordable<T> : Recordable where T : Object
    {
    }
}