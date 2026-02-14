using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int initialHealth = 200;
    [SerializeField] private int currentHealth;

    [Header("Invincibility")]
    [SerializeField]  float defaultInvincibilityTime=0.5f;
    private float currentInvincibilityTime = 1.0f;
    [SerializeField] private bool isInvincible = false;

    [Header("Events")]
    [SerializeField] FloatEventChannelSO normalizedHealth;
    [SerializeField] VoidEventChannelSO playerDied;
    [SerializeField] IntEventChannelSO healthPackUsed;
    [SerializeField] IntEventChannelSO shieldUsed;
    [SerializeField] VoidEventChannelSO playerDamaged;
    private Coroutine invincibilityRoutine;

    private void Awake()
    {
        currentHealth = initialHealth;
        isInvincible = false;
        ResetInvincibilityTime();
    }

    void ResetInvincibilityTime()
    {
        currentInvincibilityTime = defaultInvincibilityTime;
    }
    public void TakeDamage(int dmg)
    {
        if (isInvincible) return;

        playerDamaged.RaiseEvent();
        DecreaseHealth(dmg);
        StartInvincibility();
    }

    void DecreaseHealth(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerDied.RaiseEvent();
        }
        NormalizeHealthAndSendEvent();
    }

    void IncreaseHealth(int value)
    {
        currentHealth += value;
        NormalizeHealthAndSendEvent();
    }


    public void GetSquished()
    {

        Die();
    }

    void NormalizeHealthAndSendEvent()
    {
        float healthNormalized = (float) currentHealth / initialHealth;
        normalizedHealth.RaiseEvent(healthNormalized);
    }

    private void StartInvincibility()
    {
        if (invincibilityRoutine != null)
        {
            StopCoroutine(invincibilityRoutine);
        }

        invincibilityRoutine = StartCoroutine(InvincibilityTimer());
    }

    public void Die()
    {
        isInvincible = false;
        TakeDamage(int.MaxValue);
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;

        yield return new WaitForSeconds(currentInvincibilityTime);
        ResetInvincibilityTime();
        isInvincible = false;
        invincibilityRoutine = null;
    }                       
    
    public void ResetHealth()
    {
        currentHealth = initialHealth;
        NormalizeHealthAndSendEvent();
    }

    void ActivateShield(int duration)
    {
        currentInvincibilityTime = duration;
        StartInvincibility();
    }

    private void OnEnable()
    {
        healthPackUsed.OnEventRaised += IncreaseHealth;
        shieldUsed.OnEventRaised += ActivateShield;
    }

    private void OnDisable()
    {
        healthPackUsed.OnEventRaised -= IncreaseHealth;
        shieldUsed.OnEventRaised -= ActivateShield;
    }


}
