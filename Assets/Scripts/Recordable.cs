using UnityEngine;

public abstract class Recordable
{
}
public class RecordableComponent<T> : Recordable<T> where T : Component
{
    public virtual void OnRecord(GameObject go, Component component)
    {
        
    }
    public virtual void OnReplay(GameObject go, Component component)
    {
    }
}

public class Recordable<T> : Recordable where T : Object
{
    public virtual void OnRecord(GameObject go, T data)
    {
        
    }
    public virtual void OnReplay(GameObject go, T data)
    {
    }
}
