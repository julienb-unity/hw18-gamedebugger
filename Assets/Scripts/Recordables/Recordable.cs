using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Recordables
{
    [Serializable]
    public class Recordable 
    {
        public virtual bool OnRecord(Recordable previous, Object source)
        {
            return true;
        }

        public virtual void OnReplay(Object source)
        {
        }
    }

    [Serializable]
    public class Recordable<T> : Recordable where T : Object
    {
    }
}