using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerupManager : MonoBehaviour
{
    
    Dictionary<PowerupType,int> powerups = new Dictionary<PowerupType, int>();

    [Header("Events")]
    [SerializeField] PowerUpTypeChannelSO powerUpPicked;
    [SerializeField] IntEventChannelSO increaseHealth;
    [SerializeField] IntEventChannelSO shieldUsed;
    [SerializeField] IntEventChannelSO speedBoostUsed;

    [Header("Powerup Settings")]
    [SerializeField] int healthBoost=10;
    [SerializeField] int speedBoostDuration = 3;
    [SerializeField] int shieldDuration = 3;   
    private GameInputActions actions;
 

    private void Awake()
    {
        foreach (PowerupType t in Enum.GetValues(typeof(PowerupType)))
            powerups[t] = 0;

        actions = new GameInputActions();
    }

    private void OnUseSpeedBoost(InputAction.CallbackContext context)
    {
        if (powerups[PowerupType.SpeedBoost] == 0) return;
        RemoveFromInventory(PowerupType.SpeedBoost);
        speedBoostUsed.RaiseEvent(speedBoostDuration);
    }

    private void OnUseShield(InputAction.CallbackContext context)
    {
        if (powerups[PowerupType.Shield] == 0) return;
        RemoveFromInventory(PowerupType.Shield);
        shieldUsed.RaiseEvent(shieldDuration);
    }

    public void AddToInventory(PowerupType type)
    {
        powerups[type] = 1;
    }

    public void RemoveFromInventory(PowerupType type)
    {
        powerups[type] = 0;
    }

    internal void HandlePowerupPickup(PowerupType type)
    {
        switch (type)
        {
            case PowerupType.Health:
                OnHealthPackPicked();
                break;
            default:
                AddToInventory(type);
                break;
        }       
    }

    public void OnHealthPackPicked()
    {
        increaseHealth.RaiseEvent(healthBoost);
    }

    private void OnPowerupPicked(PowerupType value)
    {
        HandlePowerupPickup(value);
    }

    private void OnEnable()
    {
        powerUpPicked.OnEventRaised += OnPowerupPicked;

        actions.PowerUps.Enable();
        actions.PowerUps.UseShield.performed += OnUseShield;
        actions.PowerUps.UseSpeed.performed += OnUseSpeedBoost;      
    }

    private void OnDisable()
    {
        powerUpPicked.OnEventRaised -= OnPowerupPicked;

        actions.PowerUps.UseShield.performed -= OnUseShield;
        actions.PowerUps.UseSpeed.performed -= OnUseSpeedBoost;
        actions.PowerUps.Disable();
    }
}