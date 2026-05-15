using System;
using UnityEngine;

public enum PowerupType
{
    Shield,
    Health,
    SpeedBoost,
    Missile
}

public class PowerUp : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] PowerUpTypeChannelSO powerUpPicked;
    [SerializeField] VoidEventChannelSO lapCompleted;


    [SerializeField] PowerupType powerupType;
    [SerializeField] GameObject objectToDeactivate;
    Collider currentCollider;

    private void Awake()
    {
        GetComponent<MeshRenderer>();
        currentCollider = GetComponent<Collider>();
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
        currentCollider.enabled = false;
        objectToDeactivate.SetActive(false);
    }

    private void EnablePowerup()
    {
        currentCollider.enabled = true;
        objectToDeactivate.SetActive(true);
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
