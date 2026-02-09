using System;
using UnityEngine;

public class PowerUpHUD : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] PowerUpTypeChannelSO powerPicked;
    [SerializeField] IntEventChannelSO speedBoostUsed, shieldUsed;

    [Header("Tint Overlays")]
    [SerializeField] GameObject shieldOverlay, speedBoostOverlay;

    private void OnPowerPicked(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Shield:
                HideShieldOverlay (type);
                break;
            case PowerupType.SpeedBoost:
                HideSpeedBoostOverlay(type);
                break;
        }
    }

    private void HideSpeedBoostOverlay(PowerupType type)
    {
        speedBoostOverlay.SetActive (false);
    }

    private void HideShieldOverlay(PowerupType type)
    {
        shieldOverlay.SetActive (false);
    }

    private void ShowShieldOverlay(int arg0)
    {
        shieldOverlay.SetActive (true);
    }

    private void ShowSpeedBoostOverlay(int arg0)
    {
        speedBoostOverlay.SetActive (true);
    }

    private void OnEnable()
    {
        powerPicked.OnEventRaised += OnPowerPicked;
        speedBoostUsed.OnEventRaised += ShowSpeedBoostOverlay;
        shieldUsed.OnEventRaised += ShowShieldOverlay;
    }

    private void OnDisable()
    {
        powerPicked.OnEventRaised -= OnPowerPicked;
        speedBoostUsed.OnEventRaised -= ShowSpeedBoostOverlay;
        shieldUsed.OnEventRaised -= ShowShieldOverlay;
    }
}
