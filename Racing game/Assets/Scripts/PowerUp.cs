using System;
using UnityEngine;

public enum PowerupType
{
    Shield,
    Health,
    SpeedBoost
}

public class PowerUp : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] PowerUpTypeChannelSO powerUpPicked;
    [SerializeField] VoidEventChannelSO lapCompleted;


    [SerializeField] PowerupType powerupType;
    private MeshRenderer meshRenderer;
    BoxCollider boxCollider;

    private void Awake()
    {
        meshRenderer= GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }
    public void OnTriggerEnter(Collider other)
    {
        GameObject go = other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject;

        if (!go.CompareTag("Player"))
            return;

        powerUpPicked.RaiseEvent(powerupType);
        DisablePowerup();
    }

    private void DisablePowerup()
    {
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
    }

    private void EnablePowerup()
    {
        meshRenderer.enabled = true;
        boxCollider.enabled = true;
    }

    private void OnEnable()
    {
        lapCompleted.OnEventRaised += EnablePowerup;
    }
    private void OnDisable()
    {
        lapCompleted.OnEventRaised -= EnablePowerup;
    }
}
