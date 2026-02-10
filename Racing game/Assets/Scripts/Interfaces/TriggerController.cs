using UnityEngine;

public interface TriggerController
{
    void OnObjectEnter(GameObject origin, GameObject go);

    void OnObjectExit(GameObject origin, GameObject go);
}
