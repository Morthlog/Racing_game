using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeControl : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration chromaticAberration;
    private DepthOfField dof;
    private LensDistortion lensDistortion;

    [Header("Events")]
    [SerializeField] IntEventChannelSO speedBoostUsed;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        VolumeProfile profile = volume.profile;

        profile.TryGet(out chromaticAberration);
        profile.TryGet(out dof);
        profile.TryGet(out lensDistortion);
    }

    public void OnSpeedboost(int duration)
    {
        SetEffects(true);
        StartCoroutine(DisableEffects(duration));
    }
    IEnumerator DisableEffects(int duration)
    {
        yield return new WaitForSeconds(duration);
        SetEffects(false);
    }

    void SetEffects(bool value)
    {
        chromaticAberration.active = value;
        dof.active = value;
        lensDistortion.active = value;
    }

    private void OnEnable()
    {
        speedBoostUsed.OnEventRaised += OnSpeedboost;
    }

    private void OnDisable()
    {
        speedBoostUsed.OnEventRaised -= OnSpeedboost;
    }
}
