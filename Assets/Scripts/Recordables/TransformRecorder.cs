using UnityEngine;

public class TransformRecorder : RecordableComponent<Transform>
{
    [SerializeField]
    private Vector3 position;    
    [SerializeField]
    private Quaternion rotation;    
    [SerializeField]
    private Vector3 scale;    

    public override void OnRecord(Object source)
    {
        var t = source as Transform;
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;
    }

    public override void OnReplay(Object source)
    {
        var component = source as Component;
        component.gameObject.transform.position = position;
        component.gameObject.transform.rotation = rotation;
        component.gameObject.transform.localScale = scale;
    }
}


