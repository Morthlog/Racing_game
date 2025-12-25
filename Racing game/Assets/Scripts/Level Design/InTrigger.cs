using System.Collections.Generic;
using UnityEngine;

public class InTrigger : MonoBehaviour
{
    private TriggerController controller;

    private void Start()
    {
        controller = GetComponentInParent<TriggerController>();
        if (controller == null)
            Debug.LogError($"{gameObject} tried to find controller on parent and failed");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (controller != null && GameLoopManager.instance.getParentTag(other.gameObject) == "Player")
        {
            controller.OnObjectEnter(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (controller != null && GameLoopManager.instance.getParentTag(other.gameObject) == "Player")
        {
            controller.OnObjectExit(other.gameObject);
        }
    }

}
