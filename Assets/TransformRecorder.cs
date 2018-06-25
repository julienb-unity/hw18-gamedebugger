using UnityEngine;

public class TransformRecorder : RecordableComponent<Transform>
{
    [SerializeField]
    private Vector3 position;    
    [SerializeField]
    private Quaternion rotation;    
    [SerializeField]
    private Vector3 scale;    

    public override void OnRecord(GameObject gameObject, Component component)
    {
        var t = component as Transform;
        position = t.position;
        rotation = t.rotation;
        scale = t.localScale;
    }

    public override void OnReplay(GameObject gameObject, Component component)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.transform.localScale = scale;
    }
}


