using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectRecorder  : Recordable<GameObject>
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
