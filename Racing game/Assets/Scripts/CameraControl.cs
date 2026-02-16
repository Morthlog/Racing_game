using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineStoryboard storyboard;
    private CinemachineFollow cinemachineFollow;
    Coroutine coroutine;
    [SerializeField] GameObject speedEffects;

    [Header("Events")]
    [SerializeField] private VoidEventChannelSO outOfCameraLimits;
    [SerializeField] private VoidEventChannelSO playerRespawned;
    [SerializeField] private VoidEventChannelSO playerDied;
    [SerializeField] private IntEventChannelSO speedBoostUsed;

    void Awake()
    {
        storyboard = GetComponent<CinemachineStoryboard>();
        cinemachineFollow= GetComponent<CinemachineFollow>();
    }

    public void FadeIn(float duration,float startDelay= 0)
    {
        FadeTo(0f, duration, startDelay); 
    }
    public void FadeOut(float duration, float startDelay = 0) 
    {
        FadeTo(1f, duration, startDelay);
    } 

    public void FadeTo(float targetAlpha, float duration, float startDelay = 0)
    {
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(FadeRoutine(targetAlpha, duration,startDelay));
    }

    IEnumerator FadeRoutine(float targetAlpha, float duration, float startDelay=0)
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }
            
        float startAlpha = storyboard.Alpha;
        
        if (duration <= 0f)
        {
            storyboard.Alpha = targetAlpha;
            yield break;
        }

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration;
            storyboard.Alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
            yield return null;
        }

        storyboard.Alpha = targetAlpha;
    }

    public void InactiveView()
    {
        cinemachineFollow.enabled= false;
        FadeOut(1f,1f);
    }

    public void ActiveView()
    {
        cinemachineFollow.enabled = true;
        FadeIn(3f);
    }

    public void OnSpeedboost(int duration)
    {
        speedEffects.SetActive(true);
        StartCoroutine(DisableEffects(duration));
    }

    IEnumerator DisableEffects(int duration)
    {
        yield return new WaitForSeconds(duration);
        speedEffects.SetActive(false);
    }

    private void OnEnable()
    {
        outOfCameraLimits.OnEventRaised += InactiveView;
        playerRespawned.OnEventRaised += ActiveView;
        playerDied.OnEventRaised += InactiveView;
        speedBoostUsed.OnEventRaised += OnSpeedboost;
    }

    private void OnDisable()
    {
        outOfCameraLimits.OnEventRaised -= InactiveView;
        playerRespawned.OnEventRaised -= ActiveView;
        playerDied.OnEventRaised -= InactiveView;
        speedBoostUsed.OnEventRaised -= OnSpeedboost;
    }
}
