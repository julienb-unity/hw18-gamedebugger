using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingFilter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] filteredObjects;
    private void Awake()
    {
        GameDebuggerDatabase.FilteredGameObjects += GameDebuggerDatabaseOnFilteredGameObjects;
    }

    private void GameDebuggerDatabaseOnFilteredGameObjects(List<GameObject> obj)
    {
        obj.AddRange(filteredObjects);
    }
}
