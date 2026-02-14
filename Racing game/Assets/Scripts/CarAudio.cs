using System.Collections;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    [Header("Engine properties")]
    [SerializeField] AudioSource engineSource;
    [SerializeField] float minPitch = 0.5f;
    [SerializeField] float maxPitch = 1.7f;
    [SerializeField] float minVolume = 0.1f;
    [SerializeField] float maxRPM = 800f;
    [SerializeField] float gasSmoothTime = 0.1f; //smaller faster reaction
    [SerializeField] float noGasSmoothTime = 1f;
    float interpolatedRPM;
    float currentVelocity;

    CarControl carControl;

    [SerializeField] AudioSource explosionSource;
    [SerializeField] AudioSource shieldSource;
    [SerializeField] AudioSource damageSource;

    [Header("Events")]
    [SerializeField] VoidEventChannelSO playerDied;
    [SerializeField] IntEventChannelSO shieldUsed;
    [SerializeField] VoidEventChannelSO playerDamaged;
    void Start()
    {
        engineSource.loop = true;
        engineSource.Play();

        carControl = GetComponent<CarControl>();
    }

    void Update()
    {
        float targetRPM;
        float targetSmooth;
        if (carControl.IsGasPedalPressed())
        {
            targetRPM = carControl.GetAverageWheelRPM();
            targetSmooth = gasSmoothTime;
           
        }
        else
        {
            targetRPM = 0f;
            targetSmooth = noGasSmoothTime;
        }

        interpolatedRPM = Mathf.SmoothDamp(interpolatedRPM, targetRPM, ref currentVelocity, targetSmooth);

        float rpmRatio = Mathf.Clamp01(interpolatedRPM / maxRPM);
        engineSource.pitch = Mathf.Lerp(minPitch, maxPitch, rpmRatio);
        engineSource.volume = Mathf.Lerp(minVolume, 1, rpmRatio);
    }

    void PlayExplosion()
    {
        explosionSource.Play();
    }

    void PlayShield(int duration)
    {
        shieldSource.Play();
    }

    void PlayDamage()
    {
        damageSource.Play();
    }
    private void OnEnable()
    {
        playerDied.OnEventRaised += PlayExplosion;
        shieldUsed.OnEventRaised += PlayShield;
        playerDamaged.OnEventRaised += PlayDamage;
    }

    private void OnDisable()
    {
        playerDied.OnEventRaised -= PlayExplosion;
        shieldUsed.OnEventRaised -= PlayShield;
        playerDamaged.OnEventRaised -= PlayDamage;
    }
}