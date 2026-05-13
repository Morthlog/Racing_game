using System;
using UnityEngine;

public class PowerUpHUD : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] PowerUpTypeChannelSO powerUpPicked;
    [SerializeField] IntEventChannelSO speedBoostUsed, shieldUsed;
    [SerializeField] VoidEventChannelSO missileUsed;

    [Header("Tint Overlays")]
    [SerializeField] GameObject shieldOverlay;
    [SerializeField] GameObject speedBoostOverlay;
    [SerializeField] GameObject missileOverlay;
    private void OnPowerUpPicked(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Shield:
                HideShieldOverlay();
                break;
            case PowerupType.SpeedBoost:
                HideSpeedBoostOverlay();
                break;
            case PowerupType.Missile:
                HideMissileOverlay();
                break;
        }
    }

    private void HideMissileOverlay()
    {
        missileOverlay.SetActive(false);
    }

    private void HideSpeedBoostOverlay()
    {
        speedBoostOverlay.SetActive (false);
    }

    private void HideShieldOverlay()
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
    private void ShowMissileOverlay()
    {
        missileOverlay.SetActive(true);
    }

    private void OnEnable()
    {
        powerUpPicked.OnEventRaised += OnPowerUpPicked;
        speedBoostUsed.OnEventRaised += ShowSpeedBoostOverlay;
        shieldUsed.OnEventRaised += ShowShieldOverlay;
        missileUsed.OnEventRaised += ShowMissileOverlay;

    }

    private void OnDisable()
    {
        powerUpPicked.OnEventRaised -= OnPowerUpPicked;
        speedBoostUsed.OnEventRaised -= ShowSpeedBoostOverlay;
        shieldUsed.OnEventRaised -= ShowShieldOverlay;
        missileUsed.OnEventRaised -= ShowMissileOverlay;
    }
}
