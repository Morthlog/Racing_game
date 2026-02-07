using System;
using System.Collections;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int initialHealth = 200;
    [SerializeField] private int currentHealth;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityTime = 1.0f;
    [SerializeField] private bool isInvincible = false;


    private Coroutine invincibilityRoutine;

    [SerializeField] VoidEventChannelSO playerDied;


    private void Awake()
    {
        currentHealth = initialHealth;
        isInvincible = false;
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;

        currentHealth -= dmg;
        if (currentHealth <= 0) 
        { 
            currentHealth = 0;
            playerDied.RaiseEvent();   
        }

        StartInvincibility();
    }

    public void GetSquished()
    {
        isInvincible = false;
        TakeDamage(int.MaxValue);
    }

    private void StartInvincibility()
    {
        if (invincibilityRoutine != null)
        {
            StopCoroutine(invincibilityRoutine);
        }

        invincibilityRoutine = StartCoroutine(InvincibilityTimer());
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityTime);

        isInvincible = false;
        invincibilityRoutine = null;
    }                       
    
    public void ResetHealth()
    {
        currentHealth = initialHealth;
    }
}
