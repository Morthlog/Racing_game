using System.Collections.Generic;
using UnityEngine;

public class CarCollision : MonoBehaviour
{
    [SerializeField] VoidEventChannelSO outOfCameraLimits;
    [SerializeField] VoidEventChannelSO playerRespawned;
    private bool isOutOfBounds = false;
    private HashSet<EntityId> bounds = new HashSet<EntityId>();
    private string CameraBoundUser = "CameraBoundUser";
    private string CameraBound = "CameraBound";
    private string player = "Player";

    private void Start()
    {
        if (!CompareTag(player)) return;
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            if (child == transform)
                continue;

            if (!child.CompareTag(CameraBoundUser))
                continue;

            CarCollision script = child.gameObject.AddComponent<CarCollision>();

            script.outOfCameraLimits = outOfCameraLimits;
            script.playerRespawned = playerRespawned;

            Debug.Log("Added CarCollision to: " + child.name);
        }
        Destroy(this);
    }


    void OnTriggerEnter(Collider other)
    {
        if (isOutOfBounds) return;

        if (other.CompareTag(CameraBound))
        {
            bounds.Add(other.gameObject.GetEntityId());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isOutOfBounds) return;

        if (!other.CompareTag(CameraBound)) return;

        if (!bounds.Remove(other.gameObject.GetEntityId())) return;
            
        if (bounds.Count != 0) return;
            
        isOutOfBounds = true;
        outOfCameraLimits.RaiseEvent();
        Debug.Log("Out of Bounds");
    }

    public void ResetOutOfBounds()
    {
        isOutOfBounds = false;
        bounds.Clear();
    }

    private void OnEnable()
    {
        if (playerRespawned != null)
            playerRespawned.OnEventRaised += ResetOutOfBounds;
    }


    private void OnDisable()
    {
        if (playerRespawned != null)
            playerRespawned.OnEventRaised -= ResetOutOfBounds;
    }
}
