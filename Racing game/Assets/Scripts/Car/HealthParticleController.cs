using UnityEngine;

public class HealthParticleListener : MonoBehaviour
{
    public FloatEventChannelSO normalizedHealth;
    public ParticleSystem smokeSystem;

    [Header("Activation Settings")]
    [Range(0f, 1f)]
    public float activationThreshold = 0.5f;

    [Header("Settings Start of Effect (Health 50%)")]
    public float minSize = 0.4f;
    public float minGravity = -0.1f;
    public float minEmission = 5f;
    public Color healthyColor = new(0.5f, 0.5f, 0.5f, 0.5f);

    [Header("Settings Critical (Health 0%)")]
    public float maxSize = 3.5f;
    public float maxGravity = -1.0f;
    public float maxEmission = 50f;
    public Color damagedColor = new(0.2f, 0f, 0f, 1f);

    private ParticleSystem.MainModule psMain;
    private ParticleSystem.EmissionModule psEmission;

    private void Awake()
    {
        smokeSystem = GetComponent<ParticleSystem>();

        psMain = smokeSystem.main;
        psEmission = smokeSystem.emission;
        
        psEmission.enabled = false;
        smokeSystem.Play();
    }
    private void UpdateParticles(float currentHealthNormalized)
    {
        if (currentHealthNormalized > activationThreshold)
        {
            psEmission.enabled = false;
            return;
        }

        psEmission.enabled = true;

        // InverseLerp: converts arbitrary limits values to  0 - 1 Range
        float intensity = Mathf.InverseLerp(activationThreshold, 0f, currentHealthNormalized);

        psMain.startSize = Mathf.Lerp(minSize, maxSize, intensity);
        psMain.gravityModifier = Mathf.Lerp(minGravity, maxGravity, intensity);
        psMain.startColor = Color.Lerp(healthyColor, damagedColor, intensity);
        psEmission.rateOverTime = Mathf.Lerp(minEmission, maxEmission, intensity);
    }

    private void OnEnable()
    {
        normalizedHealth.OnEventRaised += UpdateParticles;
    }

    private void OnDisable()
    {
        normalizedHealth.OnEventRaised -= UpdateParticles;
    }

}