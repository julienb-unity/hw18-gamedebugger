using UnityEngine;

public abstract class Recordable
{
    public abstract void OnRecord(Object source);
    public abstract void OnReplay(Object source);
}

public abstract class RecordableComponent<T> : Recordable<T> where T : Component
{
}

public abstract class Recordable<T> : Recordable where T : Object
{
}
